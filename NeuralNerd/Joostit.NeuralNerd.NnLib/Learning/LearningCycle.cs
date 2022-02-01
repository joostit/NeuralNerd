using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    internal class LearningCycle
    {

        public double Cost { get; set; }

        private NetworkParameters networkParameters;


        public LearningCycle()
        {
            Cost = double.MaxValue;
        }

        public LearningCycle(double costSum, NeuralNetwork network)
        {
            Cost = costSum;
            networkParameters = ExtractNetworkParameters(network);
        }


        private NetworkParameters ExtractNetworkParameters(NeuralNetwork network)
        {
            NetworkParameters parameters = new NetworkParameters();

            parameters.HiddenLayerBiases = new double[network.HiddenLayers.Count][];
            parameters.HiddenLayerWeights = new double[network.HiddenLayers.Count][][];

            for(int hiddenLayerIndex = 0; hiddenLayerIndex < network.HiddenLayers.Count; hiddenLayerIndex++)
            {
                ICalculatableNeuronLayer currentLayer = network.HiddenLayers[hiddenLayerIndex];
                parameters.HiddenLayerBiases[hiddenLayerIndex] = new double[currentLayer.Neurons.Length];
                parameters.HiddenLayerWeights[hiddenLayerIndex] = new double[currentLayer.Neurons.Length][];

                ExtractLayerParameters(currentLayer, parameters.HiddenLayerBiases[hiddenLayerIndex], parameters.HiddenLayerWeights[hiddenLayerIndex]);
            }

            parameters.OutputLayerBiases = new double[network.OutputLayer.Neurons.Length];
            parameters.OutputLayerWeights = new double[network.OutputLayer.Neurons.Length][];

            ExtractLayerParameters(network.OutputLayer, parameters.OutputLayerBiases, parameters.OutputLayerWeights);

            return parameters;
        }


        private void ExtractLayerParameters(ICalculatableNeuronLayer sourceLayer, double[] biasesTarget, double[][] weightsTarget)
        {
            for (int neuronIndex = 0; neuronIndex < sourceLayer.Neurons.Length; neuronIndex++)
            {
                CalculatedNeuron currentNeuron = sourceLayer.Neurons[neuronIndex];
                weightsTarget[neuronIndex] = new double[currentNeuron.Dendrites.Count];

                biasesTarget[neuronIndex] = sourceLayer.Neurons[neuronIndex].Bias;

                for(int weightIndex = 0; weightIndex < currentNeuron.Dendrites.Count; weightIndex++)
                {
                    weightsTarget[neuronIndex][weightIndex] = currentNeuron.Dendrites[weightIndex].Weight;
                }

            }
        }


        public void ApplyParameters(NeuralNetwork network)
        {
            for (int hiddenLayerIndex = 0; hiddenLayerIndex < network.HiddenLayers.Count; hiddenLayerIndex++)
            {
                ICalculatableNeuronLayer currentLayer = network.HiddenLayers[hiddenLayerIndex];
                ApplyLayerParameters(currentLayer, networkParameters.HiddenLayerBiases[hiddenLayerIndex], networkParameters.HiddenLayerWeights[hiddenLayerIndex]);
            }

            ApplyLayerParameters(network.OutputLayer, networkParameters.OutputLayerBiases, networkParameters.OutputLayerWeights);
        }

        private void ApplyLayerParameters(ICalculatableNeuronLayer targetLayer, double[] biasesSource, double[][] weightsSource)
        {
            for (int neuronIndex = 0; neuronIndex < targetLayer.Neurons.Length; neuronIndex++)
            {
                CalculatedNeuron currentNeuron = targetLayer.Neurons[neuronIndex];

                targetLayer.Neurons[neuronIndex].Bias = biasesSource[neuronIndex];

                for (int weightIndex = 0; weightIndex < currentNeuron.Dendrites.Count; weightIndex++)
                {
                    currentNeuron.Dendrites[weightIndex].Weight = weightsSource[neuronIndex][weightIndex];
                }

            }
        }
    }
}
