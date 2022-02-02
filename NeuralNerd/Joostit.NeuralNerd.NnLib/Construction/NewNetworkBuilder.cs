using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Construction
{
    public class NewNetworkBuilder
    {

        NeuralNetwork network = null;

        private int layerCounter = 0;


        public NeuralNetwork BuildNetwork(NetworkParameters parameters)
        {
            if (network != null) { throw new InvalidOperationException("Cannot reuse a NetworkBuilder after constructing a neural network"); }
            
            network = new NeuralNetwork();

            CreateNeurons(parameters);

            // Create dendrite objects
            CreateHiddenLayerDendrites();
            CreateOutputLayerDendrites();

            NeuralNetworkLinker linker = new NeuralNetworkLinker();
            linker.Link(network);

            return network;
        }

        private void CreateNeurons(NetworkParameters parameters)
        {
            CreateInputNeurons(parameters);
            CreateHiddenNeurons(parameters);
            CreateOutputNeurons(parameters);
        }

        private void CreateOutputLayerDendrites()
        {
            CreateDendritesForLayer(network.OutputLayer, network.HiddenLayers[network.HiddenLayers.Count-1]);
        }

        private void CreateHiddenLayerDendrites()
        {
            for(int hiddenLayerI = 0; hiddenLayerI < network.HiddenLayers.Count; hiddenLayerI++)
            {
                INeuronLayer precedingLayer;
                if(hiddenLayerI == 0)
                {
                    precedingLayer = network.InputLayer;
                }
                else
                {
                    precedingLayer = network.HiddenLayers[hiddenLayerI - 1];
                }
                CreateDendritesForLayer(network.HiddenLayers[hiddenLayerI], precedingLayer);
            }
        }

        private void CreateDendritesForLayer(ICalculatableNeuronLayer layer, INeuronLayer precedingLayer)
        {
            foreach(CalculatedNeuron targetNeuron in layer.Neurons)
            {
                targetNeuron.Dendrites = new Dendrite[precedingLayer.Neurons.Length];
                for(int i = 0; i < precedingLayer.Neurons.Length; i++)
                {
                    Dendrite dendrite = new Dendrite();
                    dendrite.InputNeuronCoordinate = precedingLayer.Neurons[i].Coordinate.Clone();
                    targetNeuron.Dendrites[i] = dendrite;
                }

            }
        }

        private void CreateOutputNeurons(NetworkParameters parameters)
        {
            network.OutputLayer = new OutputLayer(parameters.OutputNeuronCount);
            for (int rowId = 0; rowId < parameters.OutputNeuronCount; rowId++)
            {
                OutputNeuron neuron = new OutputNeuron(layerCounter, rowId);
                neuron.Name = $"{rowId}";
                network.OutputLayer.Neurons[rowId] = neuron;
            }
            layerCounter++;
        }

        private void CreateHiddenNeurons(NetworkParameters parameters)
        {
            for (int hiddenLayerId = 0; hiddenLayerId < parameters.HiddenLayerCount; hiddenLayerId++)
            {
                HiddenLayer layer = new HiddenLayer(parameters.HiddenLayerNeuronCount);
                for (int rowId = 0; rowId < parameters.HiddenLayerNeuronCount; rowId++)
                {
                    HiddenNeuron neuron = new HiddenNeuron(layerCounter, rowId);
                    layer.Neurons[rowId] = neuron;
                }
                network.HiddenLayers.Add(layer);
                layerCounter++;
            }
        }

        private void CreateInputNeurons(NetworkParameters parameters)
        {
            network.InputLayer = new InputLayer(parameters.InputNeuronCount);
            for (int rowId = 0; rowId < parameters.InputNeuronCount; rowId++)
            {
                InputNeuron neuron = new InputNeuron(layerCounter, rowId);
                neuron.Name = $"Input {rowId}";
                network.InputLayer.Neurons[rowId] = neuron;
            }
            layerCounter++;
        }

    }
}
