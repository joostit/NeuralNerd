using Joostit.NeuralNerd.NnLib.Construction;
using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Networking;
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

        private readonly NeuralNetwork network;
        private readonly StimulusCache stimuli;
        private readonly ImageNetworkConnector connector;
        private readonly int parametersToChangePerCycle;

        private NetworkRandomizer randomizer = new NetworkRandomizer();

        public LearnerTask(ILearnTaskDispatcher dispatcher, NetworkParameters parameters, StimulusCache stimuli, int parametersToChangePerCycle)
        {
            this.dispatcher = dispatcher;
            this.stimuli = stimuli;
            this.parametersToChangePerCycle = parametersToChangePerCycle;

            NewNetworkBuilder builder = new NewNetworkBuilder();
            network = builder.BuildNetwork(parameters);

            connector = new ImageNetworkConnector(network);
        }


        public void StartCycles()
        {
            keepRunning = true;
            Task.Run(() =>
            {
                DoLearningCycles();
            });
        }


        private void DoLearningCycles()
        {
            double lastCost = double.MaxValue;

            NetworkLearnParameters parameters = null;
            NetworkLearnParameters lastParams;

            while (keepRunning)
            {
                lastParams = parameters;

                parameters = dispatcher.GetNextTask(lastCost, lastParams);

                if(parameters == null)
                {
                    keepRunning = false;
                    break;
                }

                randomizer.NudgeRandomParameter(network, parameters, parametersToChangePerCycle);

                if (parameters != lastParams)
                {
                    parameters.ApplyParameters(connector.network);
                }

                lastCost = RunSingleLearningCycle();

            }
        }


        private double RunSingleLearningCycle()
        {
            double costSum = 0;

            foreach (var stimulus in stimuli.Cache)
            {
                connector.SetInputNeurons(stimulus);
                network.Calculate();
                double passCost = CostCalculator.Calculate(network, stimulus);
                costSum += passCost;
            }

            return costSum;

        }

        public void Stop()
        {
            keepRunning = false;
        }


        

        

    }
}
