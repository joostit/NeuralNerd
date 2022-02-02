using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking.Elements
{
    public class InputNeuron : Neuron
    {

        public string Name { get; set; } = "";

        public InputNeuron()
        {

        }

        public InputNeuron(int layerIndex, int rowIndex)
            :base(layerIndex, rowIndex)
        {

        }

    }
}
