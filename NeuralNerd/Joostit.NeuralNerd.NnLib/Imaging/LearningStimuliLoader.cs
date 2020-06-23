using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Imaging
{
    public class LearningStimuliLoader
    {

        private const string pngExtension = ".png";

        public StimulusCache Stimuli { get; private set; }  = new StimulusCache();

        public LearningStimuliLoader()
        {



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

            // Load images parallel
            Parallel.ForEach(outcomeDir.GetFiles(), (file) =>
            {
                if (pngExtension.Equals(file.Extension.ToLower()))
                {
                    Stimuli.AddNewStimulus(file.FullName, CreateOutcomeList(outcomeDir));
                }
            });

            // Non-parallel
            foreach (var file in outcomeDir.GetFiles())
            {
                
            }
        }

        private List<double> CreateOutcomeList(DirectoryInfo outcomeDir)
        {
            List<double> retVal = new List<double>();

            for (int i = 0; i < 10; i++)
            {
                if (i.ToString() == outcomeDir.Name)
                {
                    retVal.Add(1);
                }
                else
                {
                    retVal.Add(0);
                }
            }
            return retVal;
        }
    }
}
