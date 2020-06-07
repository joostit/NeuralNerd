using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Construction
{
    public class NeuralNetworkLinker
    {


        /// <summary>
        /// Links all dendrite connections. Adds Object references based on Neuron coordinates
        /// </summary>
        /// <param name="network"></param>
        public void Link(NeuralNetwork network)
        {
            Dictionary<NeuronCoordinate, Neuron> neuronDictionary = CreateNeuronDictionary(network);

            CreateObjectReferences(network, neuronDictionary);
        }

        private void CreateObjectReferences(NeuralNetwork network, Dictionary<NeuronCoordinate, Neuron> neuronDictionary)
        {
            foreach(INeuronLayer hiddenLayer in network.HiddenLayers)
            {
                foreach (CalculatedNeuron neuron in hiddenLayer)
                {
                    CreateDendriteObjectReferences(neuron, neuronDictionary);
                }
            }

            foreach (CalculatedNeuron neuron in network.OutputLayer)
            {
                CreateDendriteObjectReferences(neuron, neuronDictionary);
            }
        }


        private void CreateDendriteObjectReferences(CalculatedNeuron neuron, Dictionary<NeuronCoordinate, Neuron> neuronDictionary)
        {
            try
            {
                foreach (Dendrite dendrite in neuron.Dendrites)
                {
                    dendrite.Input = neuronDictionary[dendrite.InputNeuronCoordinate];
                }
            }
            catch(KeyNotFoundException e)
            {
                throw new InvalidOperationException($"Configuration error: Cannot find Dendrite input neuron coordinate: {e.Message}", e);
            }
        }


        private Dictionary<NeuronCoordinate, Neuron> CreateNeuronDictionary(NeuralNetwork network)
        {
            Dictionary<NeuronCoordinate, Neuron> neuronDictionary = new Dictionary<NeuronCoordinate, Neuron>();

            foreach (INeuronLayer layer in network.GetAllLayers())
            {
                foreach (Neuron neuron in layer)
                {
                    try
                    {
                        neuronDictionary.Add(neuron.Coordinate, neuron);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidOperationException($"Configuration Error. Neurons with identical Coorinate found: ({e.Message})", e);
                    }
                }
            }

            return neuronDictionary;
        }
    }
}
