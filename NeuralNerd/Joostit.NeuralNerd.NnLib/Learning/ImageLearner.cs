using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    public class ImageLearner
    {

        private Random randomizer = new Random();

        public StimulusCache Stimuli { get; set; }

        public NeuralNetwork Network { get; private set; }

        public double AverageCost
        {
            get
            {
                lock (learningPassesLock)
                {
                    return costSum / learningPassesIndex;
                }
            }
        }

        public NetworkLearningPass LastPass
        {
            get
            {
                return GetLastLearningPass();
            }
        }


        private volatile NetworkLearningPass[] learningPasses;
        private volatile int learningPassesIndex = 0;
        private volatile object learningPassesLock = new object();

        private double costSum = 0;

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
                Learn();
            });
        }


        public void Learn()
        {
            learningPasses = new NetworkLearningPass[Stimuli.Cache.Count];

            ImageNetworkConnector connector = new ImageNetworkConnector();
            connector.Network = Network;

            // Cannot go parallel because we're working in the same Network object. Fix this!
            //Parallel.ForEach(Stimuli.Cache, (stimulus) =>
            foreach (var stimulus in Stimuli.Cache)
            {
                connector.SetInputNeurons(stimulus);
                Network.Calculate();
                double passCost = CalculateCost(stimulus);

                NetworkLearningPass passInfo = new NetworkLearningPass(Network)
                {
                    Stimulus = stimulus,
                    Cost = passCost,
                };

                AddLearningPassInfo(passInfo);
            }
        }

        private void AddLearningPassInfo(NetworkLearningPass passInfo)
        {
            lock (learningPassesLock)
            {
                costSum += passInfo.Cost;

                learningPasses[learningPassesIndex] = passInfo;
                learningPassesIndex++;
            }
        }


        private NetworkLearningPass GetLastLearningPass()
        {
            lock (learningPassesLock)
            {
                if (learningPassesIndex == 0)
                {
                    return null;
                }
                else
                {
                    return learningPasses[learningPassesIndex - 1];
                }
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
                    dendrite.Weight = GetRandomWeight();
                }
            }
        }


        private double GetRandomWeight()
        {
            return randomizer.Next(-3000, 3000) / 1000.0;
        }

        private double GetRandomBias()
        {
            return randomizer.Next(-10000, 10000) / 1000.0;
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

                distance = realOutcome - expected;
                currentCost = distance * distance;
                total += currentCost;
            }

            return total / Network.OutputLayer.Neurons.Length;
        }

    }
}
