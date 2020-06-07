using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking
{
    public class NeuralNetwork
    {

        public InputLayer InputLayer { get; set; } = new InputLayer();

        public HiddenLayers HiddenLayers { get; set; } = new HiddenLayers();

        public OutputLayer OutputLayer { get; set; } = new OutputLayer();

        public List<INeuronLayer> GetAllLayers()
        {
            List<INeuronLayer> retVal = new List<INeuronLayer>();
            retVal.Add(InputLayer);
            retVal.AddRange(HiddenLayers);
            retVal.Add(OutputLayer);
            return retVal;
        }


        public void Calculate()
        {
            HiddenLayers.Calculate();
            OutputLayer.Calculate();
        }

    }
}
