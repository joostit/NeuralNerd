using Joostit.NeuralNerd.NnLib.Networking.Elements;
using NeuralNerdApp.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NeuralNerdApp
{
    class CalculatedNeuronControl : NeuronControl
    {

        public event EventHandler ConfigurationChanged;

        public new CalculatedNeuron Neuron
        {
            get
            {
                return base.Neuron as CalculatedNeuron;
            }
            set
            {
                base.Neuron = value;
            }
        }

        // RoutedCommand for double-click
        private static readonly RoutedCommand NeuronIconDoubleClickCommand = new RoutedCommand();


        public CalculatedNeuronControl(CalculatedNeuron neuron)
            : base(neuron)
        {
            // Add InputBinding for mouse double-click
            var doubleClickBinding = new MouseBinding(
                NeuronIconDoubleClickCommand,
                new MouseGesture(MouseAction.LeftDoubleClick)
            );
            neuronIcon.InputBindings.Add(doubleClickBinding);

            CommandBindings.Add(new CommandBinding(
                NeuronIconDoubleClickCommand,
                NeuronIcon_DoubleClickExecuted
            ));

        }

        // Handler for double-click command
        private void NeuronIcon_DoubleClickExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsEditable)
            {
                bool hasChanged = CalculatedNeuronEditWindow.Show(Neuron);

                if (hasChanged)
                {
                    Update();
                    RaiseConfigurationChanged();
                }
            }
        }


        private void RaiseConfigurationChanged()
        {
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        internal override void Clear()
        {
            base.Clear();

            ConfigurationChanged = null;
        }

    }
}
