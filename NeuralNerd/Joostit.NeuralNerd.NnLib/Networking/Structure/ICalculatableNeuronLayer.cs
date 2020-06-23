using Joostit.NeuralNerd.NnLib.Networking.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public interface ICalculatableNeuronLayer : INeuronLayer
    {
        void Calculate();
        new CalculatedNeuron[] Neurons { get; }
    }
}
