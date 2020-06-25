using Joostit.NeuralNerd.NnLib.Networking;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Imaging
{
    public class LearningStimuliLoader
    {

        private const string pngExtension = ".png";

        public StimulusCache Stimuli { get; private set; }  = new StimulusCache();

        private Dictionary<string, int> neuronRowsForOutcomeNames = new Dictionary<string, int>();

        private NeuralNetwork network;
        private int outcomeArrayLength = -1;

        public LearningStimuliLoader(NeuralNetwork network)
        {
            this.network = network;
            DefineOutcomeInfo();
        }

        private void DefineOutcomeInfo()
        {
            foreach(var outputNeuron in network.OutputLayer.Neurons)
            {
                neuronRowsForOutcomeNames.Add(outputNeuron.Name, outputNeuron.Coordinate.Row);
            }
            outcomeArrayLength = neuronRowsForOutcomeNames.Count;
        }

        public void LoadImages(string basePath)
        {
            DirectoryInfo rootDir = new DirectoryInfo(basePath);

            foreach (var outcomeDir in rootDir.GetDirectories())
            {
                LoadFilesFromOutcomeDir(outcomeDir);
            }
        }

        private void LoadFilesFromOutcomeDir(DirectoryInfo outcomeDir)
        {
            int expectedOutcomeIndex = neuronRowsForOutcomeNames[outcomeDir.Name];

            // Load images parallel
            Parallel.ForEach(outcomeDir.GetFiles(), (file) =>
            {
                if (pngExtension.Equals(file.Extension.ToLower()))
                {
                    Stimuli.AddNewStimulus(file.FullName, CreateOutcomeList(expectedOutcomeIndex));
                }
            });
        }


        private double[] CreateOutcomeList(int correctOutcomeIndex)
        {
            double[] retVal = new double[outcomeArrayLength];
            retVal[correctOutcomeIndex] = 0;
            return retVal;
        }


    }
}
