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
        private Random randomizer = new Random();

        private bool keepRunning = true;

        private readonly ILearnTaskDispatcher dispatcher;

        private readonly NeuralNetwork network;
        private readonly StimulusCache stimuli;
        private readonly ImageNetworkConnector connector;
        private readonly int parametersToChangePerCycle = 1;

        public LearnerTask(ILearnTaskDispatcher dispatcher, NetworkParameters parameters, StimulusCache stimuli, int parametersToChangePerCycle)
        {
            this.dispatcher = dispatcher;
            this.stimuli = stimuli;
            this.parametersToChangePerCycle = parametersToChangePerCycle;

            NewNetworkBuilder builder = new NewNetworkBuilder();
            network = builder.BuildNetwork(parameters);

            connector = new ImageNetworkConnector(network);
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

            NetworkLearnParameters parameters = dispatcher.GetNextTask(double.MaxValue, null);

            while (keepRunning)
            {
                cost = RunSingleLearningCycle();

                parameters = dispatcher.GetNextTask(cost, parameters);

                if(parameters != null)
                {
                    NudgeRandomParameter(1);
                }
                else
                {
                    keepRunning = false;
                }
            }
        }


        private double RunSingleLearningCycle()
        {
            double costSum = 0;

            foreach (var stimulus in stimuli.Cache)
            {
                connector.SetInputNeurons(stimulus);
                network.Calculate();
                double passCost = CalculateCost(stimulus);
                costSum += passCost;
            }

            return costSum;

        }

        public void Stop()
        {
            keepRunning = false;
        }


        private void NudgeRandomParameter(int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                NudgeSingleRandomParameter(randomizer.Next(0, 2) == 0);
            }
        }

        private void NudgeSingleRandomParameter(bool doWeightInsteadOfBias)
        {
            var layers = network.GetAllCalculatableLayers();
            int layerIndex = randomizer.Next(0, layers.Count);
            var selectedLayer = layers[layerIndex];

            int neuronIndex = randomizer.Next(0, selectedLayer.Neurons.Length);
            var selectedNeuron = selectedLayer.Neurons[neuronIndex];

            if (doWeightInsteadOfBias)
            {
                int dentriteIndex = randomizer.Next(0, selectedNeuron.Dendrites.Length);
                var selectedDendrite = selectedNeuron.Dendrites[dentriteIndex];

                selectedDendrite.Weight_Fast = GetRandomNudge(selectedDendrite.Weight_Fast);
            }
            else
            {
                selectedNeuron.Bias = GetRandomNudge(selectedNeuron.Bias);
            }
        }


        private double GetRandomNudge(double original)
        {
            double nudgePercentage = randomizer.NextDouble() * 2 - 1;

            nudgePercentage = nudgePercentage * 2;

            if (original < 0.01)
            {
                return nudgePercentage;
            }
            else
            {
                return original + (nudgePercentage * original);
            }

        }


        private double CalculateCost(ImageStimulus currentStimulus)
        {
            double total = 0;
            double currentCost;
            double distance;
            for (int rowIndex = 0; rowIndex < network.OutputLayer.Neurons.Length; rowIndex++)
            {
                double realOutcome = network.OutputLayer.Neurons[rowIndex].Activation;
                double expected = currentStimulus.ExpectedOutcomes[rowIndex];

                distance = Math.Abs(realOutcome - expected);

                if (distance > .8)
                {
                    distance += 3;
                }
                else
                {
                    // Need at least a value of 1 for the Power multiplication to work
                    distance += 1;
                }

                // This prevents the algorithm from settling with all outcomes 0.0.
                if (realOutcome < expected)
                {
                    distance += 2;
                }

                currentCost = distance * distance * distance;
                total += currentCost;
            }

            return total;
        }

    }
}
