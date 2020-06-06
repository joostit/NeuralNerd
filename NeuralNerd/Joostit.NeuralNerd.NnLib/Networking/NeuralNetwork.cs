using Joostit.NeuralNerd.NnLib.Networking.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Networking
{
    public class NeuralNetwork
    {

        public InputColumn InputColumn { get; set; } = new InputColumn();

        public HiddenColumns HiddenColumns { get; set; } = new HiddenColumns();

        public OutputColumn OutputColumn { get; set; } = new OutputColumn();

    }
}
