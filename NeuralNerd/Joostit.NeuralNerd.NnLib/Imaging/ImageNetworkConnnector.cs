using Joostit.NeuralNerd.NnLib.Imaging.Utils;
using Joostit.NeuralNerd.NnLib.Networking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Imaging
{
    /// <summary>
    /// Connects an image source to the network input neurons
    /// </summary>
    public class ImageNetworkConnnector
    {

        public NeuralNetwork network;

        public ImageNetworkConnnector(NeuralNetwork network)
        {
            this.network = network;
        }


        public void SetInputNeurons(ImageStimulus stimulus)
        {
            if (stimulus.inputStimuli.Length != network.InputLayer.Count)
            {
                throw new InvalidOperationException("The number of input stimuli should be equal to the input layer neuron count");
            }

            for (int i = 0; i < network.InputLayer.Count; i++)
            {
                network.InputLayer.Neurons[i].Activation = stimulus.inputStimuli[i];
            }
        }


    }
}
