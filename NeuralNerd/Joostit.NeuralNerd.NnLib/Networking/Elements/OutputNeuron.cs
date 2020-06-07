using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Elements
{
    public class OutputNeuron : CalculatedNeuron
    {

        public string Name { get; set; } = "";

        public OutputNeuron()
        {

        }

        public OutputNeuron(int layerIndex, int rowIndex)
            : base(layerIndex, rowIndex)
        {

        }

    }
}
