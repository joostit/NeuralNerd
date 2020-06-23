using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public class OutputLayer : ICalculatableNeuronLayer
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

            //Parallel.ForEach<CalculatedNeuron>(this, (neuron) =>
            //{
            //    neuron.Calculate();
            //});
        }

    }
}
