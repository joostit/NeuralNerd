using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Elements
{
    public class HiddenNeuron : CalculatedNeuron
    {

        public HiddenNeuron()
        {

        }

        public HiddenNeuron(int layerIndex, int rowIndex)
            :base(layerIndex, rowIndex)
        {

        }

    }
}
