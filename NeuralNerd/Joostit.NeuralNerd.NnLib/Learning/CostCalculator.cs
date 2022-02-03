using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    public static class CostCalculator
    {
        public static double Calculate(NeuralNetwork network, ImageStimulus currentStimulus)
        {
            double total = 0;
            double currentCost;
            double distance;
            for (int rowIndex = 0; rowIndex < network.OutputLayer.Neurons.Length; rowIndex++)
            {
                double realOutcome = network.OutputLayer.Neurons[rowIndex].Activation;
                double expected = currentStimulus.ExpectedOutcomes[rowIndex];

                distance = Math.Abs(realOutcome - expected);

                if (distance > .8)
                {
                    distance += 3;
                }
                else
                {
                    // Need at least a value of 1 for the Power multiplication to work
                    distance += 1;
                }

                // This prevents the algorithm from settling with all outcomes 0.0.
                if (realOutcome < expected)
                {
                    distance += 2;
                }

                currentCost = distance * distance * distance;
                total += currentCost;
            }

            return total;
        }
    }
}
