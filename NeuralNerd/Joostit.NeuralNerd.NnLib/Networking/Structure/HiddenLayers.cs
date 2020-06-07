using Joostit.NeuralNerd.NnLib.Networking.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class HiddenLayers : List<HiddenLayer>
    {
        public void Calculate()
        {
            foreach (ICalculatableNeuronLayer layer in this)
            {
                layer.Calculate();
            }
        }
    }
}
