using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Learning;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNerdApp
{
    public class NetworkSnapshot
    {

        public NetworkLearningPass Pass { get; private set; }

        public double AverageCostSoFar { get; private set; }

        public NetworkSnapshot(ImageLearner learner)
        {
            // Race condition here, but for now we're fine with that
            AverageCostSoFar = learner.AverageCost;
            Pass = learner.LastPass;
        }
    }
}
