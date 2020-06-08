using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Construction;
using Joostit.NeuralNerd.NnLib.Networking;
using System;
using System.Diagnostics;

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
                InputNeuronCount = 784,
                HiddenLayerCount = 2,
                HiddenLayerNeuronCount = 16,
                OutputNeuronCount = 10
            });

            NetworkConfiguration config = new NetworkConfiguration();
            config.Network = network;

            network.Calculate();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            network.Calculate();
            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine("Ended");
            Console.ReadKey();
        }

        private static void CreateTestNetwork()
        {
            NewNetworkBuilder builder = new NewNetworkBuilder();

            NeuralNetwork network = builder.BuildNetwork(new NetworkParameters()
            {
                InputNeuronCount = 4,
                HiddenLayerCount = 2,
                HiddenLayerNeuronCount = 4,
                OutputNeuronCount = 3
            });

            NetworkConfiguration config = new NetworkConfiguration();
            config.Network = network;

            network.InputLayer[0].Name = "Pixel [0, 0]";
            network.InputLayer[1].Name = "Pixel [1, 0]";
            network.InputLayer[2].Name = "Pixel [1, 0]";
            network.InputLayer[3].Name = "Pixel [1, 1]";

            network.HiddenLayers[0][0].Bias = -3;
            network.HiddenLayers[0][0].Dendrites[0].Weight = 2;
            network.HiddenLayers[0][0].Dendrites[1].Weight = 2;
            network.HiddenLayers[0][0].Dendrites[2].Weight = -1;
            network.HiddenLayers[0][0].Dendrites[3].Weight = 1;

            network.HiddenLayers[0][1].Bias = -3;
            network.HiddenLayers[0][1].Dendrites[0].Weight = 2;
            network.HiddenLayers[0][1].Dendrites[1].Weight = -1;
            network.HiddenLayers[0][1].Dendrites[2].Weight = 2;
            network.HiddenLayers[0][1].Dendrites[3].Weight = -1;

            network.HiddenLayers[0][2].Bias = -3;
            network.HiddenLayers[0][2].Dendrites[0].Weight = -1;
            network.HiddenLayers[0][2].Dendrites[1].Weight = 2;
            network.HiddenLayers[0][2].Dendrites[2].Weight = -1;
            network.HiddenLayers[0][2].Dendrites[3].Weight = 2;

            network.HiddenLayers[0][3].Bias = -3;
            network.HiddenLayers[0][3].Dendrites[0].Weight = -1;
            network.HiddenLayers[0][3].Dendrites[1].Weight = -1;
            network.HiddenLayers[0][3].Dendrites[2].Weight = 2;
            network.HiddenLayers[0][3].Dendrites[3].Weight = 2;


            network.HiddenLayers[1][0].Bias = -1;
            network.HiddenLayers[1][0].Dendrites[0].Weight = 1;
            network.HiddenLayers[1][0].Dendrites[1].Weight = 1;
            network.HiddenLayers[1][0].Dendrites[2].Weight = -1;
            network.HiddenLayers[1][0].Dendrites[3].Weight = -1;

            network.HiddenLayers[1][1].Bias = -1;
            network.HiddenLayers[1][1].Dendrites[0].Weight = 1;
            network.HiddenLayers[1][1].Dendrites[1].Weight = -1;
            network.HiddenLayers[1][1].Dendrites[2].Weight = 1;
            network.HiddenLayers[1][1].Dendrites[3].Weight = -1;

            network.HiddenLayers[1][2].Bias = -1;
            network.HiddenLayers[1][2].Dendrites[0].Weight = 1;
            network.HiddenLayers[1][2].Dendrites[1].Weight = -1;
            network.HiddenLayers[1][2].Dendrites[2].Weight = -1;
            network.HiddenLayers[1][2].Dendrites[3].Weight = -2;

            network.HiddenLayers[1][3].Bias = -1;
            network.HiddenLayers[1][3].Dendrites[0].Weight = 0;
            network.HiddenLayers[1][3].Dendrites[1].Weight = 0;
            network.HiddenLayers[1][3].Dendrites[2].Weight = 0;
            network.HiddenLayers[1][3].Dendrites[3].Weight = 2;

            network.OutputLayer[0].Bias = -2;
            network.OutputLayer[0].Name = "Go right";
            network.OutputLayer[0].Dendrites[0].Weight = 2;
            network.OutputLayer[0].Dendrites[1].Weight = 1;
            network.OutputLayer[0].Dendrites[2].Weight = 1;
            network.OutputLayer[0].Dendrites[3].Weight = 1;

            network.OutputLayer[1].Bias = -2;
            network.OutputLayer[1].Name = "Go left";
            network.OutputLayer[1].Dendrites[0].Weight = 1;
            network.OutputLayer[1].Dendrites[1].Weight = 2;
            network.OutputLayer[1].Dendrites[2].Weight = 1;
            network.OutputLayer[1].Dendrites[3].Weight = 1;

            network.OutputLayer[2].Bias = -2;
            network.OutputLayer[2].Name = "Stop";
            network.OutputLayer[2].Dendrites[0].Weight = 1;
            network.OutputLayer[2].Dendrites[1].Weight = 1;
            network.OutputLayer[2].Dendrites[2].Weight = 2;
            network.OutputLayer[2].Dendrites[3].Weight = -1;


            ConfigurationFileHandler handler = new ConfigurationFileHandler();
            handler.Save(config, "SimpleNetwork.nn.xml");

            NetworkConfiguration loadedConfig = handler.Load("SimpleNetwork.nn.xml");

            GC.KeepAlive(loadedConfig);
        }
    }
}
