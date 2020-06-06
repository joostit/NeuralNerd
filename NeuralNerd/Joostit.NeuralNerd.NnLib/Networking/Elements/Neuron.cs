using System;

namespace Joostit.NeuralNerd.NnLib
{
    public abstract class Neuron
    {

        public string Id { get; set; }

        public double Activation { get; protected set; }

        public Neuron()
           : this(null)
        {
        }

        public Neuron(string id)
        {
            this.Id = id;
        }


        public override string ToString()
        {
            if (!String.IsNullOrWhiteSpace(Id))
            {
                return $"{this.GetType().Name} [{Id}]";
            }
            else
            {
                return base.ToString();
            }
        }
    }
}
