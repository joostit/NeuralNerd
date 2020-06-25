using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Learning;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNerdApp
{
    public class NetworkUiContext
    {

        public NetworkConfiguration NetworkConfig { get; set; }
        public ImageLearner Learner { get; set; }

        public OperationalStates State { get; set; } = OperationalStates.Idle;

    }
}
