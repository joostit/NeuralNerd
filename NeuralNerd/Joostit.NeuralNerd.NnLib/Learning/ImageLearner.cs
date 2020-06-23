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

        public void Initialize(string imagePath)
        {
            LearningStimuliLoader loader = new LearningStimuliLoader();
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
            });
        }

        /// <summary>
        /// Initializes a new learning cycle
        /// </summary>
        public void InitializeNew()
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

    }
}
