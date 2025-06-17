using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class OutputLayer : MatrixCalculatableLayer, ICalculatableNeuronLayer
    {

        public OutputNeuron[] Neurons { get; set; }

        [XmlIgnore]
        public int Count
        {
            get
            {
                return Neurons.Length;
            }
        }

        [XmlIgnore]
        Neuron[] INeuronLayer.Neurons
        {
            get
            {
                return Neurons;
            }
        }

        [XmlIgnore]
        CalculatedNeuron[] ICalculatableNeuronLayer.Neurons
        {
            get
            {
                return Neurons;
            }
        }

        Neuron INeuronLayer.this[int index]
        {
            get
            {
                return Neurons[index];
            }
        }

        [Obsolete("This constructor is only for XML serialization")]
        public OutputLayer()
        {

        }

        public OutputLayer(int size)
        {
            Neurons = new OutputNeuron[size];
        }

        public void Calculate()
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Calculate();
            }
        }


        public void CreateMatrixes(int previousLayerNeuronCount)
        {
            weightMatrix = new double[Neurons.Length, previousLayerNeuronCount];
            biasMatrix = new double[Neurons.Length];

            for (int neuronIndex = 0; neuronIndex < Neurons.Count(); neuronIndex++)
            {
                OutputNeuron neuron = Neurons[neuronIndex];
                for(int weightIndex = 0; weightIndex < neuron.Dendrites.Length; weightIndex++)
                {
                    weightMatrix[neuronIndex, weightIndex] = neuron.Dendrites[weightIndex].Weight;
                }

                biasMatrix[neuronIndex] = neuron.Bias;
            }
        }

    }
}
