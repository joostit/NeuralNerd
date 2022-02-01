using Joostit.NeuralNerd.NnLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking.Elements
{ 
    public abstract class CalculatedNeuron : Neuron
    {

        public List<Dendrite> Dendrites { get; set; } = new List<Dendrite>();

        public double Bias { get; set; }

        public CalculatedNeuron()
        {

        }

        public CalculatedNeuron(int layerIndex, int rowIndex)
            : base(layerIndex, rowIndex)
        {

        }

        public void Calculate()
        {
            double weightedSum = 0.0;

            foreach (var dendrite in Dendrites)
            {
                weightedSum += dendrite.Weight * dendrite.Input.Activation;
            }

            weightedSum += Bias;

            Activation = (2 * Calc.Sigmoid(weightedSum)) - 1;
        }

    }
}
