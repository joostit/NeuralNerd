using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking
{
    public class NeuralNetwork
    {

        public InputLayer InputLayer { get; set; }

        public HiddenLayers HiddenLayers { get; set; } = new HiddenLayers();

        public OutputLayer OutputLayer { get; set; }

        public List<INeuronLayer> GetAllLayers()
        {
            List<INeuronLayer> retVal = new List<INeuronLayer>();
            retVal.Add(InputLayer);
            retVal.AddRange(HiddenLayers);
            retVal.Add(OutputLayer);
            return retVal;
        }


        public List<ICalculatableNeuronLayer> GetAllCalculatableLayers()
        {
            List<ICalculatableNeuronLayer> retVal = new List<ICalculatableNeuronLayer>();
            retVal.AddRange(HiddenLayers);
            retVal.Add(OutputLayer);
            return retVal;
        }


        public void Calculate()
        {
            HiddenLayers.Calculate();
            OutputLayer.Calculate();
        }


        public void CreateMatrixes()
        {
            for(int layerIndex = 0; layerIndex < HiddenLayers.Count; layerIndex++)
            {
                int previousLayerNeuronCount;

                if(layerIndex == 0)
                {
                    previousLayerNeuronCount = InputLayer.Count;
                }
                else 
                {
                    previousLayerNeuronCount = HiddenLayers[layerIndex].Count;
                }

                HiddenLayers[layerIndex].CreateMatrixes(previousLayerNeuronCount);
            }

            OutputLayer.CreateMatrixes(HiddenLayers[HiddenLayers.Count - 1].Count);
            
        }

    }
}
