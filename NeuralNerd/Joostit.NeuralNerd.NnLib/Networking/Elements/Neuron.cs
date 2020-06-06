using System;

namespace Joostit.NeuralNerd.NnLib
{
    public abstract class Neuron
    {

        public string Id { get; set; }

        public string Activation { get; protected set; }

        public Neuron()
           : this(null)
        {
        }

        public Neuron(string id)
        {
            this.Id = id;
        }

    }
}
