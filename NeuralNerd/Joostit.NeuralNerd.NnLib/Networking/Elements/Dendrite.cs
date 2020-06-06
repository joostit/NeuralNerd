using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking
{
    [Serializable]
    public class Dendrite
    {

        [XmlAttribute]
        public string InputNeuronId { get; set; }

        [XmlIgnore]
        public Neuron Input { get; set; }

        public double Weight { get; set; }




    }
}
