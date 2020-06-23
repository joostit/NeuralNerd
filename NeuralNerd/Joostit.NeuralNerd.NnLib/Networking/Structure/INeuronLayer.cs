using Joostit.NeuralNerd.NnLib.Networking.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public interface INeuronLayer
    {
        Neuron[] Neurons { get; }
        Neuron this[int index] { get; }
        int Count { get; }
    }
}
