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

        public NeuronCoordinate InputNeuronCoordinate;

        [XmlIgnore]
        public Neuron Input;

        public double Weight;


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
