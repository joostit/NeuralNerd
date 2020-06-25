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

        public void Learn()
        {
            ImageNetworkConnector connector = new ImageNetworkConnector();
            connector.Network = Network;

            Parallel.ForEach(Stimuli.Cache, (stimulus) =>
            {
                connector.SetInputNeurons(stimulus);
                Network.Calculate();
                CalculateCost(stimulus);
            });
        }


        public void RandomizeNeuronParameters()
        {
            RandomizeParameters();
        }


        private void RandomizeParameters()
        {
            foreach(var hiddenLayer in Network.HiddenLayers)
            {
                RandomizeLayer(hiddenLayer);
            }

            RandomizeLayer(Network.OutputLayer);
        }


        private void RandomizeLayer(ICalculatableNeuronLayer layer)
        {
            foreach(CalculatedNeuron neuron in layer.Neurons)
            {
                neuron.Bias = GetRandomBias();
                foreach(var dendrite in neuron.Dendrites)
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
            for(int rowIndex = 0; rowIndex < Network.OutputLayer.Neurons.Length; rowIndex++)
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
