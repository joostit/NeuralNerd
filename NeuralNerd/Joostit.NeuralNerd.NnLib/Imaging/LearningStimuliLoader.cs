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

        private const string badOutcomeDirName = "x";

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
            if (outcomeDir.Name != badOutcomeDirName)
            {
                int expectedOutcomeIndex = neuronRowsForOutcomeNames[outcomeDir.Name];

                // Load images parallel
                Parallel.ForEach(outcomeDir.GetFiles(), (file) =>
                {
                    Stimuli.AddNewStimulus(file.FullName, CreateOutcomeList(expectedOutcomeIndex));
                });
            }
            else
            {
                // Load bad images parallel
                Parallel.ForEach(outcomeDir.GetFiles(), (file) =>
                {
                    Stimuli.AddNewStimulus(file.FullName, CreateBadOutcomeList());
                });
            }
        }


        public void LoadSingleStimulus(string path)
        {
            Stimuli.AddNewStimulus(path, new double[outcomeArrayLength]);
        }


        private double[] CreateOutcomeList(int correctOutcomeIndex)
        {
            double[] retVal = new double[outcomeArrayLength];
            retVal[correctOutcomeIndex] = 1;
            return retVal;
        }

        private double[] CreateBadOutcomeList()
        {
            double[] retVal = new double[outcomeArrayLength];
            return retVal;
        }


    }
}
