using Joostit.NeuralNerd.NnLib.Networking.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Networking.Structure
{
    public abstract class MatrixCalculatableLayer
    {

        /// <summary>
        /// WeightMatrix: Rows = neuronIndex. Columns = Weights for previous layer neurons
        /// </summary>
        public double[,] weightMatrix;
        public double[] biasMatrix;
        public double[] activationMatrix;


    }
}
