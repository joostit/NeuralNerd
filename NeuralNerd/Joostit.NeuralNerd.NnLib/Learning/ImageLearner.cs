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
    public class ImageLearner
    {

        private Random randomizer = new Random();

        public StimulusCache Stimuli { get; set; }

        public NeuralNetwork Network { get; private set; }

        public double LowestCostSoFar { get; private set; }

        public double LastCost { get; private set; }

        public ImageStimulus LastStimulus { get; set; }

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

        public void LoadStimuli(string imagePath)
        {
            LearningStimuliLoader loader = new LearningStimuliLoader(Network);
            loader.LoadImages(imagePath);
            Stimuli = loader.Stimuli;
        }

        public async Task LoadStimuliAsync(string imagePath)
        {
            await Task.Run(() =>
            {
                LoadStimuli(imagePath);
            });
        }


        public async Task LearnAsync()
        {
            await Task.Run(() =>
            {
                Learn(1000000000000);
            });
        }


        public void Learn(long passes)
        {
            learningInterruptFlag = false;
            if(Stimuli == null)
            {
                return;
            }         

            ImageNetworkConnector connector = new ImageNetworkConnector();
            connector.Network = Network;

            LearningCycle lowestCycle = new LearningCycle();
            LowestCostSoFar = double.MaxValue;

            int totalNeurons = GetParameterCount(Network);

            rateStopwatch.Reset();
            rateStopwatch.Start();
            lastMilliseconds = 0;

            for (learningPassIndex = 0; learningPassIndex < passes; learningPassIndex++)
            {
                LastCost = RunLearningCycle(connector);

                // If the current run is an omprovement over the previous one
                if (LastCost < LowestCostSoFar)
                {
                    // Continue from the current parameters
                    lowestCycle = new LearningCycle(LastCost, connector.Network);
                    LowestCostSoFar = LastCost;
                }
                else
                {
                    // Revert to the previous set of more effective parameters
                    lowestCycle.ApplyParameters(connector.Network);
                }


                if (learningInterruptFlag)
                {
                    break;
                }
                else
                {
                    // Randomize some parameters
                    // ToDo: Fine tune this
                    if ((learningPassIndex + 1) < passes)
                    {
                        NudgeRandomParameter(1);
                    }
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

                selectedDendrite.Weight = GetRandomNudge(selectedDendrite.Weight);
            }
            else
            {
                selectedNeuron.Bias = GetRandomNudge(selectedNeuron.Bias);
            }
        }

        private void NudgeParameters(int oneInHowMany)
        {
            foreach (var hiddenLayer in Network.GetAllCalculatableLayers())
            {
                NudgeLayer(hiddenLayer, oneInHowMany);
            }
        }

        public void StopLearning()
        {
            learningInterruptFlag = true;
        }

        private void NudgeLayer(ICalculatableNeuronLayer layer, int oneInHowMany)
        {
            foreach (CalculatedNeuron neuron in layer.Neurons)
            {
                if (RandomChance(oneInHowMany))
                {
                    neuron.Bias = GetRandomNudge(neuron.Bias);
                }

                foreach (var dendrite in neuron.Dendrites)
                {
                    if (RandomChance(oneInHowMany))
                    {
                        dendrite.Weight = GetRandomNudge(dendrite.Weight);
                    }
                }
            }
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
            RandomizeParameters(1);
        }


        private void RandomizeParameters(int oneInHowMany)
        {
            foreach (var hiddenLayer in Network.HiddenLayers)
            {
                RandomizeLayer(hiddenLayer, oneInHowMany);
            }

            RandomizeLayer(Network.OutputLayer, oneInHowMany);
        }


        private void RandomizeLayer(ICalculatableNeuronLayer layer, int oneInHowMany)
        {
            foreach (CalculatedNeuron neuron in layer.Neurons)
            {
                if (RandomChance(oneInHowMany))
                {
                    neuron.Bias = GetRandomBias();
                }

                foreach (var dendrite in neuron.Dendrites)
                {
                    if (RandomChance(oneInHowMany))
                    {
                        dendrite.Weight = GetRandomWeight();
                    }
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


        private bool RandomChance(int oneInHowMany)
        {
            return randomizer.Next(oneInHowMany) == 0;
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
