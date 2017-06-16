using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneLayerNeuronNetwork
{
    class Segmentation
    {
        private int lastCountLine = 5;

        private List<int> linstLine = new List<int>();
        private List<int> linstcolumn = new List<int>();

        public Bitmap selectLine(Bitmap image, bool binar)
        {
            int sum = 0;
            Bitmap result;
            if (binar)
            {
                double treshold = 0.7;

                result = new Bitmap(image.Width, image.Height);

                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        result.SetPixel(i, j, image.GetPixel(i, j).GetBrightness() < treshold ? Color.Black : Color.White);
                    }
                }
            }
            else
            {
                result = (Bitmap)image.Clone();
            }
            // Bitmap result = (Bitmap)image.Clone();

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    if (pixelColor.Name != "ffffffff")
                    {
                        sum += 1;
                    }
                }
                linstLine.Add(sum);
                sum = 0;
            }
            vectorX(result);
            vectorY(result);

            return result;
        }
        public Bitmap drawling(Bitmap img, int x, int y, int endX, int endY)
        {
            Pen blackPen = new Pen(Color.Red, 1);
            Pen greenkPen = new Pen(Color.Green, 1);

            if (x != 0)
            {
                using (var graphics = Graphics.FromImage(img))
                {
                    graphics.DrawLine(blackPen, x, y, endX, endY);
                }
            }
            else
            {
                using (var graphics = Graphics.FromImage(img))
                {
                    graphics.DrawLine(blackPen, 0, y, img.Width, y);
                }
            }

            return img;
        }

        private void vectorX(Bitmap imag)
        {
            for (int y = 0; y < linstLine.Count; y++)
            {
                if (linstLine[y] != 0)
                {
                    if (linstLine[y - 1] == 0)
                    {
                        drawling(imag, 0, y, 0, 0);
                    }
                    if (linstLine[y + 1] == 0)
                    {
                        drawling(imag, 0, y, 0, 0);
                    }
                }
            }

        }


        private void vectorY(Bitmap img)
        {
            DateTime before = DateTime.Now;

            int summa = 0;
            int endY = 0;
            int name = 0;

            for (int yRed = 0; yRed < img.Height; yRed++)
            {
                Color decRed = img.GetPixel(0, yRed);

                if (decRed.Name == "ffff0000")
                {
                    linstcolumn.Clear();
                    for (int X = 0; X < img.Width; X++)
                    {
                        for (int Y = yRed + 1; Y < img.Height; Y++)
                        {
                            Color clr = img.GetPixel(X, Y);
                            if (clr.Name != "ffff0000")
                            {
                                if (clr.Name != "ffffffff")
                                    summa += 1;

                                //  img.SetPixel(X, Y, Color.Green);
                            }
                            else
                            {
                                endY = Y;
                                break;
                            }
                        }
                        linstcolumn.Add(summa);
                        summa = 0;
                    }
                    if (linstcolumn.Count != 0)
                    {
                        seletPixel(img, yRed, endY, name);
                        name++;
                    }
                }


            }

            TimeSpan sp = DateTime.Now - before;
            MessageBox.Show(sp.ToString());
            // img.Save("tmp/asda.bmp");
        }


        private void seletPixel(Bitmap imag, int startY, int endY, int name)
        {
            int startX = 0;
            int i = 0;
            for (int x = 0; x < linstcolumn.Count; x++)
            {

                if (linstcolumn[x] != 0)
                {

                    if (linstcolumn[x - 1] == 0 && startX != 0)
                    {
                        //drawling(imag, x, startY, x, endY);

                        crapImage(imag, startX, startY, x, endY, name, i);
                        startX = 0;
                    }
                    if (linstcolumn[x - 1] == 0 && startX == 0)
                    {
                      //  drawling(imag, x, startY, x, endY);

                        startX = x;
                    }
                    if (linstcolumn[x + 1] == 0 && startX != 0)
                    {
                      //  drawling(imag, x, startY, x, endY);

                        crapImage(imag, startX, startY, x, endY, name, i);
                        startX = 0;

                    }
                    if (linstcolumn[x + 1] == 0 && startX == 0)
                    {
                      //  drawling(imag, x, startY, x, endY);
                        startX = x;
                    }
                    i++;
                }

            }
        }
        private void crapImage(Bitmap img, int cropX, int cropY, int endX, int endY, int name, int iteration)
        {
            string path = @"C:\Users\Вадим\Desktop\numbers\save\" + name + @"\";
            Rectangle rect = new Rectangle();
            rect.Location = new Point
                (
                Math.Min(cropX, endX),
                Math.Min(cropY+1, endY)
                );
            rect.Size = new Size(
                Math.Abs(cropX - endX),
                Math.Abs(cropY+1 - endY)
                );

            Bitmap cropped = new Bitmap(rect.Width+1, rect.Height+1);

            using (Graphics g = Graphics.FromImage(cropped))
            {
                g.DrawImage(img, new Rectangle(0, 0, cropped.Width, cropped.Height), rect, GraphicsUnit.Pixel);
            }
            cropped.Save(@"C:\Users\Вадим\Desktop\numbers\save\" + name + "_" + iteration + ".png");
            //DirectoryInfo di = Directory.CreateDirectory(path);
        }
    }

}
