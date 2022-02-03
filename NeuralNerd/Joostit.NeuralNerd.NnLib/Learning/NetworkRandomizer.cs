using Joostit.NeuralNerd.NnLib.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    public class NetworkRandomizer
    {

        private Random randomizer = new Random();

        public void NudgeRandomParameter(NeuralNetwork network, NetworkLearnParameters parameters, int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                NudgeSingleRandomParameter(network, parameters, randomizer.Next(0, 2) == 0);
            }
        }

        private void NudgeSingleRandomParameter(NeuralNetwork network, NetworkLearnParameters parameters, bool doWeightInsteadOfBias)
        {

            var layers = network.GetAllCalculatableLayers();
            int layerIndex = randomizer.Next(0, layers.Count);
            var selectedLayer = layers[layerIndex];

            int neuronIndex = randomizer.Next(0, selectedLayer.Neurons.Length);
            var selectedNeuron = selectedLayer.Neurons[neuronIndex];

            if (doWeightInsteadOfBias)
            {
                int dentriteIndex = randomizer.Next(0, selectedNeuron.Dendrites.Length);
                var selectedDendrite = selectedNeuron.Dendrites[dentriteIndex];

                selectedDendrite.Weight_Fast = GetRandomNudge(selectedDendrite.Weight_Fast);
                applyWeightToParameter(network, parameters, layerIndex, neuronIndex, dentriteIndex, selectedDendrite.Weight_Fast);
            }
            else
            {
                selectedNeuron.Bias = GetRandomNudge(selectedNeuron.Bias);
                applyBiasToParameter(network, parameters, layerIndex, neuronIndex, selectedNeuron.Bias);
            }
        }

        private void applyBiasToParameter(NeuralNetwork network, NetworkLearnParameters parameters, int layerIndex, int neuronIndex, double value)
        {
            if (layerIndex < network.HiddenLayers.Count)
            {
                parameters.HiddenLayerBiases[layerIndex][neuronIndex] = value;
            }
            else
            {
                parameters.OutputLayerBiases[neuronIndex] = value;
            }
        }


        private void applyWeightToParameter(NeuralNetwork network, NetworkLearnParameters parameters, int layerIndex, int neuronIndex, int dendriteIndex, double value)
        {
            if (layerIndex < network.HiddenLayers.Count)
            {
                parameters.HiddenLayerWeights[layerIndex][neuronIndex][dendriteIndex] = value;
            }
            else
            {
                parameters.OutputLayerWeights[neuronIndex][dendriteIndex] = value;
            }
        }


        private double GetRandomNudge(double original)
        {
            double nudgePercentage = randomizer.NextDouble() * 2 - 1;

            nudgePercentage = nudgePercentage * 2;

            if (original < 0.01)
            {
                return nudgePercentage;
            }
            else
            {
                return original + (nudgePercentage * original);
            }

        }


    }
}
