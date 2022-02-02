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
        public Neuron Input;

        public double Weight_Fast;

        public double Weight
        {
            get
            {
                return Weight_Fast;
            }
            set
            {
                Weight_Fast = value;
            }
        }

        public override string ToString()
        {
            if (InputNeuronCoordinate != null)
            {
                return $"{this.GetType().Name} to [{InputNeuronCoordinate}]. Weight: {this.Weight_Fast}";
            }
            else
            {
                return base.ToString();
            }
        }

    }
}
