using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    internal class NetworkParameters
    {

        /// <summary>
        /// [HiddenLayerIndex] [NeuronIndex]
        /// </summary>
        public double[][] HiddenLayerBiases;

        /// <summary>
        /// [HiddenLayerIndex] [NeuronIndex] [InputIndex]
        /// </summary>
        public double[][][] HiddenLayerWeights;

        /// <summary>
        /// [NeuronIndex]
        /// </summary>
        public double[] OutputLayerBiases;

        /// <summary>
        /// [NeuronIndex] [InputIndex]
        /// </summary>
        public double[][] OutputLayerWeights;

    }
}
