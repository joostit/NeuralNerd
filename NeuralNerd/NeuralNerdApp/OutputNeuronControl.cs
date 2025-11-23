using Joostit.NeuralNerd.NnLib.Networking.Elements;
using NeuralNerdApp.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NeuralNerdApp
{
    class OutputNeuronControl : CalculatedNeuronControl
    {
        public const double OutputNeuronWidth = 200;

        private TextBlock nameLabel;

        private SolidColorBrush originalBorderColor;

        public event EventHandler SelectionChanged;

        public new OutputNeuron Neuron
        {
            get
            {
                return base.Neuron as OutputNeuron;
            }
            set
            {
                base.Neuron = value;
            }
        }

        private bool _isSelected = false;


        public bool IsSelected {

            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    UpdateIsSelected();
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        // RoutedCommand for double-click
        private static readonly RoutedCommand NeuronIconClickCommand = new RoutedCommand();

        public OutputNeuronControl(OutputNeuron neuron)
            : base(neuron)
        {
            this.Width = OutputNeuronWidth;
            CreateNameLabel();
            contentCanvas.Width = OutputNeuronWidth;
            originalBorderColor = neuronIcon.BorderBrush as SolidColorBrush;

            // Add InputBinding for mouse double-click
            var ClickBinding = new MouseBinding(
                NeuronIconClickCommand,
                new MouseGesture(MouseAction.LeftClick)
            );
            neuronIcon.InputBindings.Add(ClickBinding);

            CommandBindings.Add(new CommandBinding(
                NeuronIconClickCommand,
                NeuronIcon_ClickExecuted
            ));
        }


        // Handler for double-click command
        private void NeuronIcon_ClickExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsEditable)
            {
                IsSelected = !IsSelected;
            }
        }


        private void CreateNameLabel()
        {
            nameLabel = new TextBlock();
            nameLabel.FontSize = 11;
            nameLabel.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(nameLabel, Size + 5);
            Canvas.SetTop(nameLabel, (Size / 2) - 8);
            contentCanvas.Children.Add(nameLabel);
        }


        public override void Update()
        {
            base.Update();
            nameLabel.Text = Neuron.Name;
        }

        private void UpdateIsSelected()
        {
            // Set border to indicate selection
            if (IsSelected)
            {
                neuronIcon.BorderThickness = new Thickness(4);
                neuronIcon.BorderBrush = Brushes.Green;
            }
            else
            {
                neuronIcon.BorderThickness = new Thickness(1);
                neuronIcon.BorderBrush = originalBorderColor;
            }

            Update();
        }
    }
}
