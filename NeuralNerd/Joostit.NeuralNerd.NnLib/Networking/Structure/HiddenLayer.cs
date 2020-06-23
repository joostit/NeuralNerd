using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class HiddenLayer : ICalculatableNeuronLayer
    {

        public HiddenNeuron[] Neurons { get; set; }

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



        public Neuron this[int index]
        {
            get
            {
                return Neurons[index];
            }
        }


        [Obsolete("This constructor is only for XML serialization")]
        public HiddenLayer()
        {

        }

        public HiddenLayer(int size)
        {
            Neurons = new HiddenNeuron[size];
        }


        public void Calculate()
        {
            for(int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Calculate();
            }

        }
      
    }
}
