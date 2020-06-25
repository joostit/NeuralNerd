using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Imaging
{
    public class StimulusCache
    {

        public List<ImageStimulus> Cache { get; private set; } = new List<ImageStimulus>();

        private object cacheLock = new object();

        public void AddNewStimulus(string filePath, double[] expectedOutcome)
        {
            ImageStimulus stimulus = new ImageStimulus(filePath);

            stimulus.ExpectedOutcomes = expectedOutcome;

            lock (cacheLock)
            {
                Cache.Add(stimulus);
            }
        }

    }
}
