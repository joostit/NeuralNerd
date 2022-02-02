using Joostit.NeuralNerd.NnLib.Imaging.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Joostit.NeuralNerd.NnLib.Imaging
{
    public class ImageStimulus
    {

        public double[] ExpectedOutcomes;

        public LockBitmap Image { get; private set; }


        public double[] inputStimuli;


        public ImageStimulus(string imagePath)
        {
            var bitmap = new Bitmap(imagePath);
            Image = new LockBitmap(bitmap);
            DefineInputStimuli();
        }


        private void DefineInputStimuli()
        {
            Image.LockBits();
            inputStimuli = new double[Image.Width * Image.Height];
            int inputStimulusIndex = 0;
            double pixelLuminocity = 0;
            Color pixelValue;
            

            for (int y = 0; y < Image.Height; y++)
            {
                for (int x = 0; x < Image.Width; x++)
                {
                    pixelValue = Image.GetPixel(x, y);
                    pixelLuminocity = pixelValue.GetBrightness();
                    inputStimuli[inputStimulusIndex] = pixelLuminocity;
                    inputStimulusIndex++;
                }
            }

            Image.UnlockBits();
        }

    }
}
