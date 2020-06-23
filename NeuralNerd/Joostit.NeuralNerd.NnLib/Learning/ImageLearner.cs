using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    public class ImageLearner
    {
        public StimulusCache Stimuli { get; set; }

        public NeuralNetwork Network { get; set; }

        public ImageLearner()
        {

        }


        public void Learn()
        {
            ImageNetworkConnector connector = new ImageNetworkConnector();
            connector.Network = Network;

            Parallel.ForEach(Stimuli.Cache, (stimulus) =>
            {
                connector.SetInputNeurons(stimulus);
                Network.Calculate();
            });
        }



    }
}
