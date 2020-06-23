using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class InputLayer : INeuronLayer
    {

        public InputNeuron[] Neurons { get; set; }

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

        Neuron INeuronLayer.this[int index]
        {
            get
            {
                return Neurons[index];
            }
        }

        [Obsolete("This constructor is only for XML serialization")]
        public InputLayer()
        {

        }

        public InputLayer(int size)
        {
            Neurons = new InputNeuron[size];
        }

    }
}
