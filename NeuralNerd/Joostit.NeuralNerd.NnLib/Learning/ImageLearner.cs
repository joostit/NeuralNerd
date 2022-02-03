using Joostit.NeuralNerd.NnLib.Construction;
using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    public class ImageLearner : ILearnTaskDispatcher
    {


        public StimulusCache Stimuli { get; set; }
        public NeuralNetwork Network { get; private set; }
        public double LowestCostSoFar { get; private set; }
        public double LastCost { get; private set; }
        public int TotalParameters { get; private set; }
        public long LearningPassIndex { get; private set; }

        private Random stimulusRandomizer = new Random();
        private ImageStimulus LastStimulus;
        private Random randomizer = new Random();
        private volatile bool learningInterruptFlag;
        private Stopwatch rateStopwatch = new Stopwatch();
        private long lastPassIndex = 0;
        private long lastMilliseconds = 0;

        private List<LearnerTask> learners = new List<LearnerTask>();
        private object taskDispatchLock = new object();
        private const int parametersToChangePerCycle = 1;
        private LearningCycle lowestCostCycle;

        public int PassesPerSecond
        {
            get
            {
                long currentPassIndex = LearningPassIndex;
                long currentMs = rateStopwatch.ElapsedMilliseconds;
                double rate = ((currentPassIndex - lastPassIndex) / (double)(currentMs - lastMilliseconds)) * 1000;

                lastMilliseconds = currentMs;
                lastPassIndex = currentPassIndex;
                return (int)Math.Round(rate);
            }
        }


        public ImageLearner(NeuralNetwork Network)
        {
            this.Network = Network;
        }


        public StimulusCache LoadStimuli(string imagePath)
        {
            LearningStimuliLoader loader = new LearningStimuliLoader(Network);
            loader.LoadImages(imagePath);
            Stimuli = loader.Stimuli;
            return Stimuli;
        }

        public async Task<StimulusCache> LoadStimuliAsync(string imagePath)
        {
            StimulusCache cache = null;

            await Task.Run(() =>
            {
                cache = LoadStimuli(imagePath);
            });

            return cache;
        }


        public async Task LearnAsync(string stimuliPath)
        {
            await StartAsyncLearningTasks(10, stimuliPath);
        }


        public async Task StartAsyncLearningTasks(int count, string stimuliPath)
        {
            LearningPassIndex = 0;
            lowestCostCycle = new LearningCycle(double.MaxValue, Network);

            StimulusCache stimuli = await LoadStimuliAsync(stimuliPath);

            NetworkParameters parameters = new NetworkParameters()
            {
                InputNeuronCount = Network.InputLayer.Neurons.Length,
                HiddenLayerCount = Network.HiddenLayers.Count,
                HiddenLayerNeuronCount = Network.HiddenLayers[0].Neurons.Length,
                OutputNeuronCount = Network.OutputLayer.Neurons.Length
            };

            rateStopwatch.Reset();
            rateStopwatch.Start();
            lastMilliseconds = 0;

            learners.Clear();
            for (int i = 0; i < count; i++)
            {
                learners.Add(new LearnerTask(this, parameters, stimuli, parametersToChangePerCycle));
            }

            learningInterruptFlag = false;
            learners.ForEach(task => task.StartCycles());
        }


        NetworkLearnParameters ILearnTaskDispatcher.GetNextTask(double lastCost, NetworkLearnParameters lastParameters)
        {
            NetworkLearnParameters nextParametersToUse;

            if (learningInterruptFlag)
            {
                return null;
            }

            lock (taskDispatchLock)
            {
                LearningPassIndex++;
                LastCost = lastCost;

                // If this is the first run for the task, we always return a fresh set of parameters to start with
                if (lastParameters == null)
                {
                    nextParametersToUse = lowestCostCycle.networkParameters;
                }
                else
                {
                    // If the current run is an improvement over the previous one
                    if (lastCost < lowestCostCycle.Cost)
                    {
                        // Take the last run parameters as the new starting point for others,
                        // and let all other tasks start with this one from now on.
                        lowestCostCycle.Cost = lastCost;
                        lowestCostCycle.CycleId = LearningPassIndex;
                        lowestCostCycle.networkParameters = lastParameters.Clone();

                        LowestCostSoFar = lastCost;

                        // Give back the same set of parameters, since this is the most successfull
                        nextParametersToUse = lastParameters;
                    }
                    else
                    {
                        // This last run was not the most successfull. Return the current most succcessfull parameters
                        nextParametersToUse = lowestCostCycle.networkParameters.Clone();
                    }
                }

                return nextParametersToUse;
            }

        }


        public void StopLearning()
        {
            learningInterruptFlag = true;

            learners.ForEach(learner => learner.Stop());
        }


        public NetworkLearningPass GetLastLearningPass()
        {
            if (lowestCostCycle != null)
            {
                lowestCostCycle.networkParameters.ApplyParameters(Network);

                ImageNetworkConnector connector = new ImageNetworkConnector(Network);

                LastStimulus = GetRandomStimulus();
                connector.SetInputNeurons(LastStimulus);
                Network.Calculate();
            }

            return new NetworkLearningPass(Network)
            {
                Cost = LastCost,
                Stimulus = LastStimulus,
                PassIndex = LearningPassIndex,
                PassesPerSecond = PassesPerSecond
            };
        }


        private ImageStimulus GetRandomStimulus()
        {
            if (Stimuli != null)
            {
                return Stimuli.Cache[stimulusRandomizer.Next(Stimuli.Cache.Count)];
            }
            else
            {
                return null;
            }
        }


        public void RandomizeNeuronParameters()
        {
            RandomizeParameters();
        }


        private void RandomizeParameters()
        {
            foreach (var hiddenLayer in Network.HiddenLayers)
            {
                RandomizeLayer(hiddenLayer);
            }

            RandomizeLayer(Network.OutputLayer);
        }


        private void RandomizeLayer(ICalculatableNeuronLayer layer)
        {
            foreach (CalculatedNeuron neuron in layer.Neurons)
            {
                neuron.Bias = GetRandomBias();

                foreach (var dendrite in neuron.Dendrites)
                {
                    dendrite.Weight_Fast = GetRandomWeight();
                }
            }
        }


        private double GetRandomWeight()
        {
            return randomizer.Next(-2000, 2000) / 1000.0;
        }


        private double GetRandomBias()
        {
            return randomizer.Next(-1000, 1000) / 1000.0;
        }


        public void CalculateCosts()
        {
            if (LastStimulus != null)
            {
                Network.Calculate();
                LastCost = CostCalculator.Calculate(Network, LastStimulus);
                LowestCostSoFar = LastCost;
            }
        }


        public void LoadSingleStimulus(string path)
        {
            LearningStimuliLoader loader = new LearningStimuliLoader(Network);
            loader.LoadSingleStimulus(path);
            Stimuli = loader.Stimuli;

            ImageNetworkConnector connector = new ImageNetworkConnector(Network);

            LastStimulus = Stimuli.Cache[0];
            connector.SetInputNeurons(LastStimulus);

            CalculateCosts();
        }

    }
}
