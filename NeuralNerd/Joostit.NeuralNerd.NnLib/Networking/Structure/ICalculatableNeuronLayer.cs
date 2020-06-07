using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public interface ICalculatableNeuronLayer : INeuronLayer
    {
        void Calculate();
    }
}
