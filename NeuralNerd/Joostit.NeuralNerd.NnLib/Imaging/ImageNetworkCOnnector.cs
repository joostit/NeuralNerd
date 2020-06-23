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
    public class ImageNetworkConnector
    {

        public NeuralNetwork Network { get; set; }

        public ImageNetworkConnector()
        {

        }


        public void SetInputNeurons(ImageStimulus stimulus)
        {
            if (stimulus.inputStimuli.Count != Network.InputLayer.Count)
            {
                throw new InvalidOperationException("The number of input stimuli should be equal to the input layer neuron count");
            }

            for (int i = 0; i < Network.InputLayer.Count; i++)
            {
                Network.InputLayer.Neurons[i].Activation = stimulus.inputStimuli[i];
            }
        }


    }
}
