using System;

namespace Joostit.NeuralNerd.NnLib.Networking.Elements
{
    public abstract class Neuron
    {

        public NeuronCoordinate Coordinate { get; set; }

        public double Activation { get; protected set; }

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
                return $"{this.GetType().Name} [{Coordinate.ToString()}]";
            }
            else
            {
                return base.ToString();
            }
        }
    }
}
