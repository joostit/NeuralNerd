using Joostit.NeuralNerd.NnLib.Configuration;
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


            NewNetworkBuilder builder = new NewNetworkBuilder();

            NeuralNetwork network = builder.BuildNetwork(new NetworkParameters()
            {
                InputNeuronCount = 6,
                HiddenLayerCount = 2,
                HiddenLayerNeuronCount = 4,
                OutputNeuronCount = 3
            });



            NetworkConfiguration config = new NetworkConfiguration();
            config.Network = network;


            ConfigurationFileHandler handler = new ConfigurationFileHandler();
            handler.Save(config, "SimpleNetwork.xml");

            NetworkConfiguration loadedConfig = handler.Load("SimpleNetwork.xml");

            GC.KeepAlive(loadedConfig);






            Console.WriteLine("Ended");
        }
    }
}
