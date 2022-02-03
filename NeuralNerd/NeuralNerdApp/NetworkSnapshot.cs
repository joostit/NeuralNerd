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

        public double LowestCostSoFar { get; private set; }

        public NetworkSnapshot(ImageLearner learner)
        {
            // Race condition here, but for now we're fine with that
            LowestCostSoFar = learner.LowestCostSoFar;
            Pass = learner.GetLastLearningPass();
        }
    }
}
