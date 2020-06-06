using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking
{
    public class NeuralNetwork
    {

        public InputLayer InputLayer { get; set; } = new InputLayer();

        public HiddenLayers HiddenLayers { get; set; } = new HiddenLayers();

        public OutputLayer OutputLayer { get; set; } = new OutputLayer();

    }
}
