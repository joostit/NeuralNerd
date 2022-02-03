using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    public class NetworkLearnParameters
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

        public NetworkLearnParameters(NeuralNetwork network)
        {
            ExtractNetworkParameters(network);
        }

        public NetworkLearnParameters()
        {

        }

        public void ExtractNetworkParameters(NeuralNetwork network)
        {

            HiddenLayerBiases = new double[network.HiddenLayers.Count][];
            HiddenLayerWeights = new double[network.HiddenLayers.Count][][];

            for (int hiddenLayerIndex = 0; hiddenLayerIndex < network.HiddenLayers.Count; hiddenLayerIndex++)
            {
                ICalculatableNeuronLayer currentLayer = network.HiddenLayers[hiddenLayerIndex];
                HiddenLayerBiases[hiddenLayerIndex] = new double[currentLayer.Neurons.Length];
                HiddenLayerWeights[hiddenLayerIndex] = new double[currentLayer.Neurons.Length][];

                ExtractLayerParameters(currentLayer, HiddenLayerBiases[hiddenLayerIndex], HiddenLayerWeights[hiddenLayerIndex]);
            }

            OutputLayerBiases = new double[network.OutputLayer.Neurons.Length];
            OutputLayerWeights = new double[network.OutputLayer.Neurons.Length][];

            ExtractLayerParameters(network.OutputLayer, OutputLayerBiases, OutputLayerWeights);

        }


        private void ExtractLayerParameters(ICalculatableNeuronLayer sourceLayer, double[] biasesTarget, double[][] weightsTarget)
        {
            for (int neuronIndex = 0; neuronIndex < sourceLayer.Neurons.Length; neuronIndex++)
            {
                CalculatedNeuron currentNeuron = sourceLayer.Neurons[neuronIndex];
                weightsTarget[neuronIndex] = new double[currentNeuron.Dendrites.Length];

                biasesTarget[neuronIndex] = sourceLayer.Neurons[neuronIndex].Bias;

                for (int weightIndex = 0; weightIndex < currentNeuron.Dendrites.Length; weightIndex++)
                {
                    weightsTarget[neuronIndex][weightIndex] = currentNeuron.Dendrites[weightIndex].Weight_Fast;
                }

            }
        }


        public void ApplyParameters(NeuralNetwork network)
        {
            for (int hiddenLayerIndex = 0; hiddenLayerIndex < network.HiddenLayers.Count; hiddenLayerIndex++)
            {
                ICalculatableNeuronLayer currentLayer = network.HiddenLayers[hiddenLayerIndex];
                ApplyLayerParameters(currentLayer, HiddenLayerBiases[hiddenLayerIndex], HiddenLayerWeights[hiddenLayerIndex]);
            }

            ApplyLayerParameters(network.OutputLayer, OutputLayerBiases, OutputLayerWeights);
        }

        private void ApplyLayerParameters(ICalculatableNeuronLayer targetLayer, double[] biasesSource, double[][] weightsSource)
        {
            for (int neuronIndex = 0; neuronIndex < targetLayer.Neurons.Length; neuronIndex++)
            {
                CalculatedNeuron currentNeuron = targetLayer.Neurons[neuronIndex];

                targetLayer.Neurons[neuronIndex].Bias = biasesSource[neuronIndex];

                for (int weightIndex = 0; weightIndex < currentNeuron.Dendrites.Length; weightIndex++)
                {
                    currentNeuron.Dendrites[weightIndex].Weight_Fast = weightsSource[neuronIndex][weightIndex];
                }

            }
        }


        public NetworkLearnParameters Clone()
        {
            NetworkLearnParameters retVal = new NetworkLearnParameters()
            {
                HiddenLayerBiases = CopyDoubleArray(HiddenLayerBiases),
                HiddenLayerWeights = CopyTripleArray(HiddenLayerWeights),
                OutputLayerWeights = CopyDoubleArray(OutputLayerWeights)
            };

            retVal.OutputLayerBiases = new double[OutputLayerBiases.Length];
            Array.Copy(OutputLayerBiases, retVal.OutputLayerBiases, OutputLayerBiases.Length);

            return retVal;
        }



        private double[][] CopyDoubleArray(double[][] source)
        {
            var len = source.Length;
            var dest = new double[len][];

            for (var x = 0; x < len; x++)
            {
                var inner = source[x];
                var ilen = inner.Length;
                var newer = new double[ilen];
                Array.Copy(inner, newer, ilen);
                dest[x] = newer;
            }

            return dest;
        }


        private double[][][] CopyTripleArray(double[][][] source)
        {
            double[][][] dest = new double[source.Length][][];
            for(int i = 0; i < source.Length; i++)
            {
                dest[i] = CopyDoubleArray(source[i]);
            }   

            return dest;
        }
    }
}
