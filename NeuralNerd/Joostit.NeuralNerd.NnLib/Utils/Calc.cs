using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Utils
{
    public static class Calc
    {

        public static double Sigmoid(double value)
        {
            return 1.0 / (1.0 + Math.Pow(Math.E, -value));
        }


    }
}
