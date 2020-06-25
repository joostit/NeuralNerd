using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Networking;
using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    public class NetworkLearningPass
    {

        public NetworkState State { get; set; }

        public ImageStimulus Stimulus { get; set; }

        public double Cost { get; set; }


        public NetworkLearningPass(NeuralNetwork network)
        {
            CopyStateFromNetwork(network);
        }   


        private void CopyStateFromNetwork(NeuralNetwork network)
        {
            NetworkState state = new NetworkState();
            state.InputLayerActivations = CopyNeuronActivation(network.InputLayer);

            state.HiddenLayersActivations = new double[network.HiddenLayers.Count][];

            for(int i = 0; i < network.HiddenLayers.Count; i++)
            {
                state.HiddenLayersActivations[i] = CopyNeuronActivation(network.HiddenLayers[i]);
            }

            state.OutputLayerActivations = CopyNeuronActivation(network.OutputLayer);
        }


        private double[] CopyNeuronActivation(INeuronLayer layer)
        {
            double[] retVal = new double[layer.Count];

            for(int i = 0; i < layer.Count; i++)
            {
                retVal[i] = layer.Neurons[i].Activation;
            }
            return retVal;
        }
    }
}
