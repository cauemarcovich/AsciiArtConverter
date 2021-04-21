using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace AsciiArtConverter
{
    class Program
    {
        const string CHARS = " .-:*+=%$@##";

        static string filepath;
        static int maxWidth = 256;

        static void Main(string[] args)
        {
            var success = SetConfiguration(args);
            if (success)
                GenerateASCII();
            Console.WriteLine("Press any key to exit...");
        }

        static void GenerateASCII()
        {
            var image = ReadImage(filepath);
            var text = ConvertToAscii(image);
            Console.WriteLine(text);
        }

        static Bitmap ReadImage(string filepath)
        {
            var sourceImage = Bitmap.FromFile(filepath);

            var imageWidth = sourceImage.Width;
            while (imageWidth >= maxWidth)
                imageWidth /= 2;
            var imageHeight = (int)Math.Ceiling((double)sourceImage.Height * imageWidth / sourceImage.Width);

            return ConvertToLowerResolution(sourceImage, imageWidth, imageHeight);
        }

        static string ConvertToAscii(Bitmap image)
        {
            Color pixel, grey;
            int normalizedColor, charIndex;

            bool skipLine = false;

            var result = new StringBuilder();
            for (int h = 0; h < image.Height; h++)
            {
                if (skipLine)
                {
                    skipLine = false;
                    continue;
                }

                for (int w = 0; w < image.Width; w++)
                {
                    pixel = image.GetPixel(w, h);
                    normalizedColor = (int)(0.2126 * pixel.R + 0.7152 * pixel.G + 0.0722 * pixel.B);
                    grey = Color.FromArgb(normalizedColor, normalizedColor, normalizedColor);

                    charIndex = (grey.R * CHARS.Length) / 255;
                    result.Append(CHARS[charIndex]);
                }

                result.Append("\n");
                skipLine = true;
            }
            return result.ToString();
        }

        static Bitmap ConvertToLowerResolution(Image sourceImage, int width, int height)
        {
            var image = new Bitmap(width, height);

            var graphicDrawer = Graphics.FromImage((Image)image);
            graphicDrawer.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphicDrawer.DrawImage(sourceImage, 0, 0, width, height);
            graphicDrawer.Dispose();

            return image;
        }

        static bool SetConfiguration(string[] args)
        {
            if (args.Length <= 1)
            {
                Console.WriteLine("No parameter was specified.");
                return false;
            }

            for (int i = 0; i < args.Length; i += 2)
            {
                if (i + 2 > args.Length) break;

                if (args[i] == "-file")
                    filepath = args[i + 1];
                else if (args[i] == "-maxwidth")
                {
                    if (!Int32.TryParse(args[i + 1], out maxWidth))
                    {
                        Console.WriteLine("MaxWidth specified is not Int32.");
                        return false;
                    }
                }
            }

            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("The file was not specified.");
                return false;
            }
            else if (!File.Exists(filepath))
            {
                Console.WriteLine("The file not exists.");
                return false;
            }

            return true;
        }
    }
}
