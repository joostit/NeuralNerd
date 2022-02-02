using Joostit.NeuralNerd.NnLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking.Elements
{ 
    public abstract class CalculatedNeuron : Neuron
    {

        public Dendrite[] Dendrites;

        public double Bias;

        public CalculatedNeuron()
        {

        }

        public CalculatedNeuron(int layerIndex, int rowIndex)
            : base(layerIndex, rowIndex)
        {

        }

        public void Calculate()
        {
            double weightedSum = Bias;

            Dendrite dendrite;
            for (int i = 0; i < Dendrites.Length; i++)
            {
                dendrite = Dendrites[i];
                weightedSum += dendrite.Weight_Fast * dendrite.Input.Activation;
            }

            Activation = Calc.Sigmoid(weightedSum);
        }

    }
}
