using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace NeuralNerdApp
{
    public static class GrayScaleBrushes
    {

        private static Brush[] brushes = new Brush[256];

        static GrayScaleBrushes()
        {

            for (int i = 0; i <= byte.MaxValue; i++)
            {
                byte v = (byte)i;
                Brush brush = new SolidColorBrush(Color.FromRgb(v, v, v));
                brushes[i] = brush;
            }
        }

        public static Brush Get(byte value)
        {
            return brushes[value];
        }


    }
}
