using Joostit.NeuralNerd.NnLib.Imaging.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Imaging
{
    public class ImageStimulus
    {
          
        public List<double> ExpectedOutcomes { get; set; } = new List<double>();

        public LockBitmap Image { get; private set; }


        public List<double> inputStimuli { get; private set; } = new List<double>();


        public ImageStimulus(string imagePath)
        {
            var bitmap = new Bitmap(imagePath);
            Image = new LockBitmap(bitmap);
            DefineInputStimuli();
        }


        private void DefineInputStimuli()
        {

            double pixelLuminocity = 0;
            Color pixelValue;
            Image.LockBits();

            for (int y = 0; y < Image.Height; y++)
            {
                for (int x = 0; x < Image.Width; x++)
                {
                    pixelValue = Image.GetPixel(x, y);
                    pixelLuminocity = pixelValue.GetBrightness();
                    inputStimuli.Add(pixelLuminocity);

                }
            }

            Image.UnlockBits();
        }

    }
}
