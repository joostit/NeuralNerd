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


        /// <summary>
        /// Creates a deep clone
        /// </summary>
        /// <param name="originalToClone"></param>
        public ImageStimulus(ImageStimulus originalToClone)
        {
            Bitmap originalBitmap = originalToClone.Image.Source;
            var bitmap = originalBitmap.Clone(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height), originalBitmap.PixelFormat);
            Image = new LockBitmap(bitmap);

            ExpectedOutcomes = new double[originalToClone.ExpectedOutcomes.Length];
            Array.Copy(originalToClone.ExpectedOutcomes, ExpectedOutcomes, originalToClone.ExpectedOutcomes.Length);

            UpdateInputStimuli();
        }


        public ImageStimulus(int width, int height)
        {
            var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Image = new LockBitmap(bitmap);
            UpdateInputStimuli();
        }

        public ImageStimulus(string imagePath)
        {
            var bitmap = new Bitmap(imagePath);
            Image = new LockBitmap(bitmap);
            UpdateInputStimuli();
        }


        public void UpdateInputStimuli()
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

        internal ImageStimulus DeepClone()
        {
            return new ImageStimulus(this);
        }
    }
}
