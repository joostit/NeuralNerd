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

        private Random randomizer = new Random();

        public StimulusCache Stimuli { get; set; }

        public NeuralNetwork Network { get; private set; }

        public double LowestCostSoFar { get; private set; }

        public double LastCost { get; private set; }

        public int TotalParameters { get; private set; }

        private ImageStimulus LastStimulus;

        private long learningPassIndex;

        private volatile bool learningInterruptFlag;

        private Stopwatch rateStopwatch = new Stopwatch();

        private long lastPassIndex = 0;

        private long lastMilliseconds = 0;

        public int PassesPerSecond
        {
            get
            {
                long currentPassIndex = learningPassIndex;
                long currentMs = rateStopwatch.ElapsedMilliseconds;
                double rate = ((currentPassIndex - lastPassIndex) / (double) (currentMs - lastMilliseconds)) * 1000;

                lastMilliseconds = currentMs;
                lastPassIndex = currentPassIndex;
                return (int) Math.Round(rate);
            }
        }


        public long LearningPassIndex
        {
            get
            {
                return learningPassIndex;
            }
        }

        public NetworkLearningPass LastPass
        {
            get
            {
                return GetLastLearningPass();
            }
        }


        public ImageLearner(NeuralNetwork Network)
        {
            this.Network = Network;
        }

        public StimulusCache LoadStimuli(string imagePath)
        {
            this.imagePath = imagePath;
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


        public async Task LearnAsync()
        {
            await StartAsyncLearningTasks(1);

            //await Task.Run(() =>
            //{
            //    Learn(1000000000000);
            //});
        }

        // <summary>
        // //
        // ///   TODO: Clean this up!!!!
        // //
        // </summary>


        private List<LearnerTask> learners = new List<LearnerTask>();
        private object taskDispatchLock = new object();
        private const int parametersToChangePerCycle = 1;
        private string imagePath;
        private long currentLearnIndex;
        private LearningCycle lowestCostCycle;

        public async Task StartAsyncLearningTasks(int count)
        {
            currentLearnIndex = 0;
            lowestCostCycle = new LearningCycle(double.MaxValue, Network);

            StimulusCache stimuli = await LoadStimuliAsync(imagePath);

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
            for(int i = 0; i < count; i++)
            {
                learners.Add(new LearnerTask(this, parameters, stimuli, parametersToChangePerCycle));
            }

            learningInterruptFlag = false;
            learners.ForEach(task => task.Start());
        }


        public NetworkLearnParameters GetNextTask(double lastCost, NetworkLearnParameters lastParameters)
        {
            NetworkLearnParameters nextParametersToUse;

            if (learningInterruptFlag)
            {
                return null;
            }

            lock (taskDispatchLock)
            {
                currentLearnIndex++;

                // HOOK UP LEGACY
                learningPassIndex = currentLearnIndex;
                LastCost = lastCost;
                // END LEGACY

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
                        lowestCostCycle.CycleId = currentLearnIndex;
                        lowestCostCycle.networkParameters = lastParameters.Clone();

                        // START To hook up with the existing visualization
                        lastParameters.ApplyParameters(Network);
                        LowestCostSoFar = lastCost;
                        //END HOOK UP

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



        // <summary>
        // ////////////////
        // </summary>
        // <param name="passes"></param>





        public void Learn(long passes)
        {
            learningInterruptFlag = false;
            if(Stimuli == null)
            {
                return;
            }         

            ImageNetworkConnector connector = new ImageNetworkConnector(Network);

            LowestCostSoFar = double.MaxValue;
            LearningCycle lowestCycle = new LearningCycle(double.MaxValue, connector.network);

            TotalParameters = GetParameterCount(Network);

            rateStopwatch.Reset();
            rateStopwatch.Start();
            lastMilliseconds = 0;

            for (learningPassIndex = 0; learningPassIndex < passes; learningPassIndex++)
            {
                LastCost = RunLearningCycle(connector);

                // If the current run is an omprovement over the previous one
                if (LastCost < lowestCycle.Cost)
                {
                    // Continue from the current parameters
                    lowestCycle = new LearningCycle(LastCost, connector.network);
                    LowestCostSoFar = LastCost;
                 }
                else
                {
                    // Revert to the previous set of more effective parameters
                    lowestCycle.networkParameters.ApplyParameters(connector.network);
                }

                if (learningInterruptFlag)
                {
                    break;
                }
                else
                {
                    NudgeRandomParameter(1);
                }
            }

            rateStopwatch.Stop();
        }



        private void NudgeRandomParameter(int howMany)
        {
            for(int i = 0; i < howMany; i++)
            {
                NudgeSingleRandomParameter(randomizer.Next(0,2) == 0);
            }
        }

        private void NudgeSingleRandomParameter(bool doWeightInsteadOfBias)
        {
            var layers = Network.GetAllCalculatableLayers();
            int layerIndex = randomizer.Next(0, layers.Count);
            var selectedLayer = layers[layerIndex];

            int neuronIndex = randomizer.Next(0, selectedLayer.Neurons.Length);
            var selectedNeuron = selectedLayer.Neurons[neuronIndex];

            if (doWeightInsteadOfBias)
            {
                int dentriteIndex = randomizer.Next(0, selectedNeuron.Dendrites.Length);
                var selectedDendrite = selectedNeuron.Dendrites[dentriteIndex];

                selectedDendrite.Weight_Fast = GetRandomNudge(selectedDendrite.Weight_Fast);
            }
            else
            {
                selectedNeuron.Bias = GetRandomNudge(selectedNeuron.Bias);
            }
        }


        public void StopLearning()
        {
            learningInterruptFlag = true;

            learners.ForEach(learner => learner.Stop());
        }


        private double GetRandomNudge(double original)
        {
            double nudgePercentage = randomizer.NextDouble() * 2 - 1;

            nudgePercentage = nudgePercentage * 2;

            if (original < 0.01)
            {
                return nudgePercentage;
            }
            else
            {
                return original + (nudgePercentage * original);
            }

        }


        private int GetParameterCount(NeuralNetwork network)
        {
            int total = 0;

            foreach(var layer in network.GetAllCalculatableLayers())
            {
                foreach(var neuron in layer.Neurons)
                {
                    total += 1;     // For the bias
                    total += neuron.Dendrites.Length;
                }
            }

            return total;
        }



        private double RunLearningCycle(ImageNetworkConnector connector)
        {
            double costSum = 0;

            foreach (var stimulus in Stimuli.Cache)
            {
                connector.SetInputNeurons(stimulus);
                Network.Calculate();
                double passCost = CalculateCost(stimulus);
                LastStimulus = stimulus;
                costSum += passCost;
            }

            return costSum;

        }


        private NetworkLearningPass GetLastLearningPass()
        {
            return new NetworkLearningPass(Network)
            {
                Cost = LastCost,
                Stimulus = LastStimulus,
                PassIndex = LearningPassIndex,
                PassesPerSecond = PassesPerSecond
            };
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


        private double CalculateCost(ImageStimulus currentStimulus)
        {
            double total = 0;
            double currentCost;
            double distance;
            for (int rowIndex = 0; rowIndex < Network.OutputLayer.Neurons.Length; rowIndex++)
            {
                double realOutcome = Network.OutputLayer.Neurons[rowIndex].Activation;
                double expected = currentStimulus.ExpectedOutcomes[rowIndex];

                distance = Math.Abs(realOutcome - expected);
                

                if(distance > .8)
                {
                    distance += 3;
                }
                else
                {
                    distance += 1;
                }


                if (realOutcome < expected)
                {
                    distance += 2;
                }

                currentCost = distance * distance * distance;
                total += currentCost;
            }

            return total;
        }


        public void LoadSingleStimulus(string path)
        {
            LearningStimuliLoader loader = new LearningStimuliLoader(Network);
            loader.LoadSingleStimulus(path);
            Stimuli = loader.Stimuli;

            Learn(1);
        }

    }
}
