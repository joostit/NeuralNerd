using Joostit.NeuralNerd.NnLib.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Configuration
{
    [XmlRoot]
    public class NetworkConfiguration
    {

        public NeuralNetwork Network { get; set; }


    }
}
