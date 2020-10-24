using System;
using System.Drawing;
using System.Windows.Forms;

namespace Recognizer
{
    internal static class Program
    {
        private const int ResizeRate = 2;

        private static Bitmap ConvertToBitmap(int width, int height, Func<int, int, Color> getPixelColor)  //конвертация изображения в битмап заполненными прямоугольниками 
        {
            var bmp = new Bitmap(ResizeRate * width, ResizeRate * height);
            using (var g = Graphics.FromImage(bmp))
            {
                for (var x = 0; x < width; x++)
                    for (var y = 0; y < height; y++)
                        g.FillRectangle(new SolidBrush(getPixelColor(x, y)),
                            ResizeRate * x,
                            ResizeRate * y,
                            ResizeRate,
                            ResizeRate
                        );
            }

            return bmp;
        }

        private static Bitmap ConvertToBitmap(Pixel[,] array)
        {
            return ConvertToBitmap(array.GetLength(0), array.GetLength(1),
                (x, y) => Color.FromArgb(array[x, y].R, array[x, y].G, array[x, y].B));
        }

        private static Bitmap ConvertToBitmap(double[,] array)
        {
            return ConvertToBitmap(array.GetLength(0), array.GetLength(1), (x, y) =>
            {
                var gray = (int)(255 * array[x, y]);
                gray = Math.Min(gray, 255);
                gray = Math.Max(gray, 0);
                return Color.FromArgb(gray, gray, gray);
            });
        }

        private static PictureBox CreateBox(Bitmap bmp)
        {
            return new PictureBox
            {
                Size = bmp.Size,
                Dock = DockStyle.Fill,
                Image = bmp
            };
        }

        public static Pixel[,] LoadPixels(Bitmap bmp) //перевод изображения в двумерный массив из экземляров класса пиксель
        {
            var pixels = new Pixel[bmp.Width, bmp.Height];
            for (var x = 0; x < bmp.Width; x++)
                for (var y = 0; y < bmp.Height; y++)
                    pixels[x, y] = new Pixel(bmp.GetPixel(x, y)); //вот здесь при создании экземпляра при помощи метода GetPixel передается структура Color
            return pixels;
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var bmp = (Bitmap)Image.FromFile("eurobot.bmp"); //загрузка изображения
            var pixels = LoadPixels(bmp); // разбивка изображения на двумерный массив заполненный экземплярами класса Pixel

            var form = new Form //создает новое окно
            {
                ClientSize = new Size(3 * ResizeRate * bmp.Width, 2 * ResizeRate * bmp.Height)
            };

            var panel = new TableLayoutPanel //разбивка окна на "сетку" для размещения элементов по ячейкам (без привязки коорд)
            {
                RowCount = 2,
                ColumnCount = 3,
                Dock = DockStyle.Fill
            };
            form.Controls.Add(panel); //добавление сетки в окно

            panel.Controls.Add(CreateBox(ConvertToBitmap(pixels)), 0, 0); //вывод эталлоного изображения
            var grayscale = GrayscaleTask.ToGrayscale(pixels);
            panel.Controls.Add(CreateBox(ConvertToBitmap(grayscale)), 1, 0); //
            var clear = MedianFilterTask.MedianFilter(grayscale);
            panel.Controls.Add(CreateBox(ConvertToBitmap(clear)), 2, 0);
            var sobell = SobelFilterTask.SobelFilter(clear, new double[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } });
            //   var sobell = SobelFilterTask.SobelFilter(new double[,] { { 1 } }, new double[,] { { 2 } });
            panel.Controls.Add(CreateBox(ConvertToBitmap(sobell)), 0, 1);
            var trashhold = ThresholdFilterTask.ThresholdFilter(sobell, 0.2);
            panel.Controls.Add(CreateBox(ConvertToBitmap(trashhold)), 1, 1);

            var bitmap = ConvertToBitmap(sobell);
            using (var g = Graphics.FromImage(bitmap))
            {
                var lines = HoughTransformTask.HoughAlgorithm(sobell);
                var pen = new Pen(Color.Red, 2);
                foreach (var e in lines)
                    g.DrawLine(pen, e.X0 * ResizeRate, e.Y0 * ResizeRate, e.X1 * ResizeRate, e.Y1 * ResizeRate);
            }

            panel.Controls.Add(CreateBox(bitmap), 2, 1);
            Application.Run(form);
        }
    }
}