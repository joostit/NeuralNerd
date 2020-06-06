using Joostit.NeuralNerd.NnLib.Construction;
using Joostit.NeuralNerd.NnLib.Networking;
using System;

namespace Joostit.NeuralNerd.NnCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running");



            NetworkBuilder builder = new NetworkBuilder();

            NeuralNetwork network = builder.BuildNetwork(new NetworkParameters()
            {
                InputNeuronCount = 6,
                HiddenLayerCount = 2,
                HiddenLayerNeuronCount = 4,
                OutputNeuronCount = 3
            });

            GC.KeepAlive(network);

            Console.WriteLine("Ended");
        }
    }
}
