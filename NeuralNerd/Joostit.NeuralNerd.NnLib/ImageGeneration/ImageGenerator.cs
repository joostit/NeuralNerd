using Joostit.NeuralNerd.NnLib.Configuration;
using Joostit.NeuralNerd.NnLib.Imaging;
using Joostit.NeuralNerd.NnLib.Learning;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Joostit.NeuralNerd.NnLib.ImageGeneration
{
    public class ImageGenerator
    {
        private const int imageWidth = 16;
        private const int imageHeight = 16;

        private Random random = new Random();
        //private ImageStimulus currentimage = new ImageStimulus(imageWidth, imageHeight);

        private NetworkConfiguration networkConfig;
        private ImageLearner learner;

        private const string outputFolderName = "ImgGen";

        // Inputs:
        // Image size
        // Neural network expected stimuli

        public void Run(NetworkConfiguration networkConfig, ImageLearner learner, int generatedOutputNeuronIndex)
        {
            ImageStimulus currentimage = new ImageStimulus(imageWidth, imageHeight);
            ImageStimulus nextAttemptImage;
            this.networkConfig = networkConfig;
            this.learner = learner;
            double lowestCost = double.MaxValue;
            double newCost;

            if (!Directory.Exists(outputFolderName))
            {
                Directory.CreateDirectory(outputFolderName);
            }

            // Create image

            currentimage.ExpectedOutcomes = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            currentimage.ExpectedOutcomes[generatedOutputNeuronIndex] = 1;

            // Fill image with noise
            fillImageWithNoise(currentimage);
            //saveImage(currentimage, "step_0_noise.png");   // For debug

            // Calculate cost
            lowestCost = learner.LoadSingleStimulus(currentimage);

            // Iterate until cost is low enough or for a maximum number of cycles
            for (int i = 0; i <= 1000000; i++)
            {
                // Make a copy of the image
                nextAttemptImage = currentimage.DeepClone();

                // Change some pixels
                ChangePixels(nextAttemptImage);

                // get the new cost.
                newCost = learner.LoadSingleStimulus(nextAttemptImage);

                //if the cost is better than the previous try, keep using the copied image
                if (newCost <= lowestCost)
                {
                    currentimage = nextAttemptImage;
                    lowestCost = newCost;
                }

                if(i % 50000 == 0)
                {
                    saveImage(currentimage, $"step_{i}.png");   // For debug
                }
                
            }

        }

        private void saveImage(ImageStimulus stimulus, string filename)
        {
            string path = Path.Combine(outputFolderName, filename);
            stimulus.Image.SaveAsPng(path);
        }

        private void ChangePixels(ImageStimulus image)
        {
            int pixelCount = random.Next(10);
            int size = random.Next(5);
            int x;
            int y;
            Color color;
            image.Image.LockBits();

            for (int i = 0; i < pixelCount; i++)
            {
                x = random.Next(imageWidth);
                y = random.Next(imageHeight);
                color = getRandomColor(2);
                SetBigPixel(image, x, y, color, size);
            }

            //for (int i = 0; i < pixelCount; i++)
            //{
            //    x = random.Next(imageWidth);
            //    y = random.Next(imageHeight);
            //    color = getRandomColor(2);
            //    image.Image.SetPixel(x, y, Color.White);
            //}

            image.Image.UnlockBits();
            image.UpdateInputStimuli();
        }


        private void SetBigPixel(ImageStimulus image, int x, int y, Color color, int size)
        {
            if (size >= 4)
            {
                SafeSetPixel(image, x - 2, y - 2, color);
                SafeSetPixel(image, x - 2, y - 1, color);
                SafeSetPixel(image, x - 2, y, color);
                SafeSetPixel(image, x - 2, y + 1, color);
                SafeSetPixel(image, x - 2, y + 2, color);

                SafeSetPixel(image, x + 2, y - 2, color);
                SafeSetPixel(image, x + 2, y - 1, color);
                SafeSetPixel(image, x + 2, y, color);
                SafeSetPixel(image, x + 2, y + 1, color);
                SafeSetPixel(image, x + 2, y + 2, color);
            }

            if (size >= 2)
            {
                SafeSetPixel(image, x - 1, y - 2, color);
                SafeSetPixel(image, x - 1, y - 1, color);
                SafeSetPixel(image, x - 1, y, color);
                SafeSetPixel(image, x - 1, y + 1, color);
                SafeSetPixel(image, x - 1, y + 2, color);

                SafeSetPixel(image, x + 1, y - 2, color);
                SafeSetPixel(image, x + 1, y - 1, color);
                SafeSetPixel(image, x + 1, y, color);
                SafeSetPixel(image, x + 1, y + 1, color);
                SafeSetPixel(image, x + 1, y + 2, color);
            }

            SafeSetPixel(image, x, y - 2, color);
            SafeSetPixel(image, x, y - 1, color);
            SafeSetPixel(image, x, y, color);
            SafeSetPixel(image, x, y + 1, color);
            SafeSetPixel(image, x, y + 2, color);

        }

        private void SafeSetPixel(ImageStimulus image, int x, int y, Color color)
        {
            if(x > -1
                && x < imageWidth
                && y > -1
                && y < imageHeight)
            {
                image.Image.SetPixel(x, y, color);
            }
        }



        private void fillImageWithNoise(ImageStimulus image)
        {
            Color color;
            image.Image.LockBits();
            for (int x = 0; x < imageWidth; x++)
            {
                for(int y = 0; y < imageHeight; y++)
                {
                    color = getRandomColor(2);
                    image.Image.SetPixel(x, y, color);
                }
            }
            image.Image.UnlockBits();
            image.UpdateInputStimuli();
        }

        private Color getRandomColor(int odds)
        {

            int val = random.Next(256);
            Color color = Color.FromArgb(val, val, val);
            //bool blackPixel = random.Next(odds) < 1;
            //if (blackPixel)
            //{
            //    color = Color.Black;
            //}
            //else
            //{
            //    color = Color.White;
            //}
            return color;
        }

    }
}
