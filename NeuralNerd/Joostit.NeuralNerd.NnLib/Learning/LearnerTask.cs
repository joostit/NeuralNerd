using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    internal class LearnerTask
    {

        private bool keepRunning = true;

        private readonly ILearnTaskDispatcher dispatcher;

        public LearnerTask(ILearnTaskDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }


        public void Start()
        {
            keepRunning = true;
            Task.Run(() =>
            {
                DoLearningCycles();
            });
        }


        private void DoLearningCycles()
        {
            double cost = 0;

            while (keepRunning)
            {



                dispatcher.GetNextTask(cost);
            }
        }


        public void Stop()
        {
            keepRunning = false;
        }


    }
}
