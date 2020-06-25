using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    public class NetworkState
    {
        public double[] InputLayerActivations { get; set; }

        public double[][] HiddenLayersActivations { get; set; }

        public double[] OutputLayerActivations { get; set; }
    }
}
