using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Construction;
using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Learning;
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
            Stopwatch sw = new Stopwatch();
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

            
            
            Console.WriteLine("Creating learner and randomizing network parameters...");
            sw.Start();
            ImageLearner learner = new ImageLearner(network);
            learner.RandomizeNeuronParameters();
            sw.Stop();
            Console.WriteLine($"Took: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine("");
            sw.Reset();


            Console.WriteLine("Loading stimuli...");
            sw.Start();
            learner.LoadStimuli(@"C:\Joost\Projects\MNIST_Dataset\mnist_png\training");
            sw.Stop();
            Console.WriteLine($"Took: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine("");
            sw.Reset();

            Console.WriteLine("Running the learn method...");
            sw.Start();
            learner.Learn();
            sw.Stop();
            Console.WriteLine($"Took: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine("");
            sw.Reset();


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

            network.InputLayer.Neurons[0].Name = "Pixel [0, 0]";
            network.InputLayer.Neurons[1].Name = "Pixel [1, 0]";
            network.InputLayer.Neurons[2].Name = "Pixel [1, 0]";
            network.InputLayer.Neurons[3].Name = "Pixel [1, 1]";

            network.HiddenLayers[0].Neurons[0].Bias = -3;
            network.HiddenLayers[0].Neurons[0].Dendrites[0].Weight = 2;
            network.HiddenLayers[0].Neurons[0].Dendrites[1].Weight = 2;
            network.HiddenLayers[0].Neurons[0].Dendrites[2].Weight = -1;
            network.HiddenLayers[0].Neurons[0].Dendrites[3].Weight = 1;

            network.HiddenLayers[0].Neurons[1].Bias = -3;
            network.HiddenLayers[0].Neurons[1].Dendrites[0].Weight = 2;
            network.HiddenLayers[0].Neurons[1].Dendrites[1].Weight = -1;
            network.HiddenLayers[0].Neurons[1].Dendrites[2].Weight = 2;
            network.HiddenLayers[0].Neurons[1].Dendrites[3].Weight = -1;

            network.HiddenLayers[0].Neurons[2].Bias = -3;
            network.HiddenLayers[0].Neurons[2].Dendrites[0].Weight = -1;
            network.HiddenLayers[0].Neurons[2].Dendrites[1].Weight = 2;
            network.HiddenLayers[0].Neurons[2].Dendrites[2].Weight = -1;
            network.HiddenLayers[0].Neurons[2].Dendrites[3].Weight = 2;

            network.HiddenLayers[0].Neurons[3].Bias = -3;
            network.HiddenLayers[0].Neurons[3].Dendrites[0].Weight = -1;
            network.HiddenLayers[0].Neurons[3].Dendrites[1].Weight = -1;
            network.HiddenLayers[0].Neurons[3].Dendrites[2].Weight = 2;
            network.HiddenLayers[0].Neurons[3].Dendrites[3].Weight = 2;


            network.HiddenLayers[1].Neurons[0].Bias = -1;
            network.HiddenLayers[1].Neurons[0].Dendrites[0].Weight = 1;
            network.HiddenLayers[1].Neurons[0].Dendrites[1].Weight = 1;
            network.HiddenLayers[1].Neurons[0].Dendrites[2].Weight = -1;
            network.HiddenLayers[1].Neurons[0].Dendrites[3].Weight = -1;

            network.HiddenLayers[1].Neurons[1].Bias = -1;
            network.HiddenLayers[1].Neurons[1].Dendrites[0].Weight = 1;
            network.HiddenLayers[1].Neurons[1].Dendrites[1].Weight = -1;
            network.HiddenLayers[1].Neurons[1].Dendrites[2].Weight = 1;
            network.HiddenLayers[1].Neurons[1].Dendrites[3].Weight = -1;

            network.HiddenLayers[1].Neurons[2].Bias = -1;
            network.HiddenLayers[1].Neurons[2].Dendrites[0].Weight = 1;
            network.HiddenLayers[1].Neurons[2].Dendrites[1].Weight = -1;
            network.HiddenLayers[1].Neurons[2].Dendrites[2].Weight = -1;
            network.HiddenLayers[1].Neurons[2].Dendrites[3].Weight = -2;

            network.HiddenLayers[1].Neurons[3].Bias = -1;
            network.HiddenLayers[1].Neurons[3].Dendrites[0].Weight = 0;
            network.HiddenLayers[1].Neurons[3].Dendrites[1].Weight = 0;
            network.HiddenLayers[1].Neurons[3].Dendrites[2].Weight = 0;
            network.HiddenLayers[1].Neurons[3].Dendrites[3].Weight = 2;

            network.OutputLayer.Neurons[0].Bias = -2;
            network.OutputLayer.Neurons[0].Name = "Go right";
            network.OutputLayer.Neurons[0].Dendrites[0].Weight = 2;
            network.OutputLayer.Neurons[0].Dendrites[1].Weight = 1;
            network.OutputLayer.Neurons[0].Dendrites[2].Weight = 1;
            network.OutputLayer.Neurons[0].Dendrites[3].Weight = 1;

            network.OutputLayer.Neurons[1].Bias = -2;
            network.OutputLayer.Neurons[1].Name = "Go left";
            network.OutputLayer.Neurons[1].Dendrites[0].Weight = 1;
            network.OutputLayer.Neurons[1].Dendrites[1].Weight = 2;
            network.OutputLayer.Neurons[1].Dendrites[2].Weight = 1;
            network.OutputLayer.Neurons[1].Dendrites[3].Weight = 1;

            network.OutputLayer.Neurons[2].Bias = -2;
            network.OutputLayer.Neurons[2].Name = "Stop";
            network.OutputLayer.Neurons[2].Dendrites[0].Weight = 1;
            network.OutputLayer.Neurons[2].Dendrites[1].Weight = 1;
            network.OutputLayer.Neurons[2].Dendrites[2].Weight = 2;
            network.OutputLayer.Neurons[2].Dendrites[3].Weight = -1;


            ConfigurationFileHandler handler = new ConfigurationFileHandler();
            handler.Save(config, "SimpleNetwork.nn.xml");

            NetworkConfiguration loadedConfig = handler.Load("SimpleNetwork.nn.xml");

            GC.KeepAlive(loadedConfig);
        }
    }
}
