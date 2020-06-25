using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Imaging
{
    public class StimulusCache
    {


        public List<ImageStimulus> Cache { get; private set; } = new List<ImageStimulus>();

        public void AddNewStimulus(string filePath, double[] expectedOutcome)
        {
            ImageStimulus stimulus = new ImageStimulus(filePath);

            stimulus.ExpectedOutcomes = expectedOutcome;

            Cache.Add(stimulus);
        }

    }
}
