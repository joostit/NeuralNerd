using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking.Elements
{
    [Serializable]
    public class Dendrite
    {

        public NeuronCoordinate InputNeuronCoordinate { get; set; }

        [XmlIgnore]
        public Neuron Input { get; set; }

        public double Weight { get; set; }

        public override string ToString()
        {
            if (InputNeuronCoordinate != null)
            {
                return $"{this.GetType().Name} to [{InputNeuronCoordinate}]. Weight: {this.Weight}";
            }
            else
            {
                return base.ToString();
            }
        }

    }
}
