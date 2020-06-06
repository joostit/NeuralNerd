using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Construction
{
    public class NetworkBuilder
    {

        NeuralNetwork network = null;

        private int layerCounter = 0;


        public NeuralNetwork BuildNetwork(NetworkParameters parameters)
        {
            if (network != null) { throw new InvalidOperationException("Cannot reuse a NetworkBuilder after constructing a neural network"); }
            
            network = new NeuralNetwork();

            CreateNeurons(parameters);

            return network;
        }

        private void CreateNeurons(NetworkParameters parameters)
        {
            // Create Neuron objects
            CreateInputNeurons(parameters);
            CreateHiddenNeurons(parameters);
            CreateOutputNeurons(parameters);

            // Create dendrite objects
            CreateDendrites();
        }

        private void CreateDendrites()
        {
            
        }

        private void CreateOutputNeurons(NetworkParameters parameters)
        {
            for (int rowId = 0; rowId < parameters.OutputNeuronCount; rowId++)
            {
                OutputNeuron neuron = new OutputNeuron(layerCounter, rowId);
                network.OutputLayer.Add(neuron);
            }
            layerCounter++;
        }

        private void CreateHiddenNeurons(NetworkParameters parameters)
        {
            for (int hiddenLayerId = 0; hiddenLayerId < parameters.HiddenLayerCount; hiddenLayerId++)
            {
                HiddenLayer layer = new HiddenLayer();
                for (int rowId = 0; rowId < parameters.HiddenLayerNeuronCount; rowId++)
                {
                    HiddenNeuron neuron = new HiddenNeuron(layerCounter, rowId);
                    layer.Add(neuron);
                }
                network.HiddenLayers.Add(layer);
                layerCounter++;
            }
        }

        private void CreateInputNeurons(NetworkParameters parameters)
        {
            for (int rowId = 0; rowId < parameters.InputNeuronCount; rowId++)
            {
                InputNeuron neuron = new InputNeuron(layerCounter, rowId);
                network.InputLayer.Add(neuron);
            }
            layerCounter++;
        }

    }
}
