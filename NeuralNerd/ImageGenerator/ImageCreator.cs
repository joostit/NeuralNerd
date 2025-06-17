using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace ImageGenerator
{
    internal class ImageCreator
    {
        private string imageDirectory = "Images";
        private const int imageHeight = 28;
        private const int imageWidth = 28;
        private const int totalImageCount = 300;
        private const string testCharacters = "abcdefhjkmnprtuvwxyzABCDEFGHJKLMNPRTUVWXY";

        private Font[] fonts = CreateFonts();

        private int fontIndex;
        private int characterIndex;


        private static Font[] CreateFonts()
        {
            return new Font[]
            {
                new Font("Arial", 14),
                new Font("Courier New", 14),
                new Font("Comic Sans MS", 14),
                new Font("Calibri", 14),
                new Font("Arial", 18, FontStyle.Bold),
                new Font("Courier New", 18, FontStyle.Bold),
                new Font("Comic Sans MS", 18, FontStyle.Bold),
                new Font("Calibri", 18, FontStyle.Bold),
                new Font("Arial", 10),
                new Font("Courier New", 10),
                new Font("Comic Sans MS", 10),
                new Font("Calibri", 10),
            };
        }


        public void Run()
        {
            int generatedCount = 0;

            Console.WriteLine("TestData image generator");
            PrepareStorageFolder();

            while (generatedCount <= totalImageCount)
            {
                Bitmap img = CreateNextImageType();

                img.Save(Path.Combine(GetImageFolderPath(), $"{generatedCount}.jpg"));
                generatedCount++;
            }
        }


        private void PrepareStorageFolder()
        {
            if (Directory.Exists(GetImageFolderPath()))
            {
                Directory.Delete(GetImageFolderPath(), true);
            }

            Directory.CreateDirectory(GetImageFolderPath());
        }


        private string GetImageFolderPath()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imageDirectory);
            return path;
        }


        private Bitmap CreateNextImageType()
        {
            Bitmap bmp = new Bitmap(5,5, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            if (characterIndex < testCharacters.Length)
            {
                bmp = CreateNextCharImage(testCharacters[characterIndex]);
            }
            else
            {
                if (characterIndex == testCharacters.Length) // White
                {
                    bmp = CreateFilledImage(Color.White);
                }
                else if (characterIndex == testCharacters.Length + 1) // Black
                {
                    bmp = CreateFilledImage(Color.Black);
                }
                else if (characterIndex == testCharacters.Length + 2) // Noise
                {
                    bmp = CreateNoiseImage(30);
                }
                else if (characterIndex == testCharacters.Length + 3) // Noise
                {
                    bmp = CreateNoiseImage(40);
                }
                else if (characterIndex == testCharacters.Length + 4) // Noise
                {
                    bmp = CreateNoiseImage(60);
                }
                else
                {
                    throw new InvalidOperationException("Bug: Shoudld never reach this index");
                }
            }

            characterIndex++;

            if (characterIndex == testCharacters.Length + 5)
            {
                characterIndex = 0;
            }

           

            return bmp;
        }


        private Bitmap CreateNoiseImage(int percentage)
        {
            Bitmap bmp = GetNewBitmap();
            Graphics g = Graphics.FromImage(bmp);
            Random rnd = new Random();

            for(int x = 0; x < bmp.Width; x++)
            {
                for(int y = 0; y <bmp.Height; y++)
                {
                    int val = (int) rnd.NextInt64(0, 101);

                    if(val <= percentage)
                    {
                        bmp.SetPixel(x, y, Color.White);
                    }

                }
            }


            return bmp;
        }


        private Bitmap CreateFilledImage(Color color)
        {
            Bitmap bmp = GetNewBitmap();
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(color);

            return bmp;
        }

        private Bitmap CreateNextCharImage(char character)
        {

            Bitmap bmp = GetNewBitmap();
            Graphics g = Graphics.FromImage(bmp);

            Font font = GetNextFont();

            g.DrawString(character.ToString(), font, Brushes.White, 10 - (font.Size / 2), 10 - (font.Size / 2));

            g.Dispose();
            return bmp;
        }


        private Font GetNextFont()
        {
            Font fnt = fonts[fontIndex];
            fontIndex++;
            if(fontIndex >= fonts.Length)
            {
                fontIndex = 0;
            }
            return fnt;
        }


        private Bitmap GetNewBitmap()
        {
            Bitmap bmp = new Bitmap(imageWidth, imageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.Dispose();
            return bmp;
        }
    
    }
}
