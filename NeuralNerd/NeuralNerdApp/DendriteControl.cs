using Joostit.NeuralNerd.NnLib.Networking.Elements;
using NeuralNerdApp.CustomControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace NeuralNerdApp
{
    public sealed class DendriteControl : Arrow
    {

        public CalculatedNeuron Target { get; set; }

        public Neuron Source { get; set; }

        public Dendrite Dendrite { get; set; }

        public DendriteControl(Dendrite dendrite, Neuron source, CalculatedNeuron target)
        {
            this.Dendrite = dendrite;
            this.Source = source;
            this.Target = target;

            this.HeadHeight = 3;
            this.HeadWidth = 3;
            this.Stroke = Brushes.Black;
            this.StrokeThickness = .5;

            base.HasArrowHead = false;
        }


    }
}
