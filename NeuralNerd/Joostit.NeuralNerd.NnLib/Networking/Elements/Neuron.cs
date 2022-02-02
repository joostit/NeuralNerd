using System;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Networking.Elements
{
    public abstract class Neuron
    {

        public NeuronCoordinate Coordinate;

        [XmlIgnore]
        public double Activation;

        public Neuron()
           : this(-1, -1)
        {
        }

        public Neuron(int layerIndex, int rowIndex)
        {
            this.Coordinate = new NeuronCoordinate(layerIndex, rowIndex);
        }


        public override string ToString()
        {
            if (Coordinate != null)
            {
                return $"{this.GetType().Name} [{Coordinate.ToString()}]. Activation: {Activation}";
            }
            else
            {
                return base.ToString();
            }
        }
    }
}
