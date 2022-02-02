using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Elements;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    internal class LearningCycle
    {

        public double Cost { get; set; }

        public NetworkParameters networkParameters { get; set; }


        public LearningCycle(double costSum, NetworkParameters parameters)
        {
            networkParameters = parameters;
        }

        public LearningCycle(double costSum, NeuralNetwork network)
        {
            Cost = costSum;
            networkParameters = new NetworkParameters(network);
        }


        
    }
}
