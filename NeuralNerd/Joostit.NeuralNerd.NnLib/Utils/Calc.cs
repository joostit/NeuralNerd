using System;
using System.Collections.Generic;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Utils
{
    public static class Calc
    {

        public static double Sigmoid(double value)
        {
            if (value < -45.0) return 0.0;
            else if (value > 45.0) return 1.0;
            else return 1.0 / (1.0 + Math.Exp(-value));

        }


    }
}
