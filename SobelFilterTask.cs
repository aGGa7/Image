using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] g, double[,] sx) //фильтр Собеля, здесь: sx это ядро (окно) по оси Х - для получения 
        {                                                           // окна по оси У достаточно произвести транспонирование sx
            int lengthSX = sx.GetLength(0);
            int lengthSY = sx.GetLength(1);
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var result = new double[width, height];
            for (int x = lengthSX / 2; x < width - lengthSX / 2; x++) //цикл для прохода по входящему изображению. Начальные и конечные значения цикла
                for (int y = lengthSY / 2; y < height - lengthSY / 2; y++) // (или с каким отступом от края изображения начинать обработку) зависят от размера ядра(окна) sx
                {                   //свертка двух матриц - поэлементное умножение одной матрицы на другую, и затем сложение всех получившихся значений
                    double gx = 0; //это результат свертки двух матриц по оси Х
                    double gy = 0;// по оси У. Первая матрица это окрестности пикселя изображения, вторая матрица - ядро(окно) sx
                    for (int i = 0; i < lengthSX; i++) //цикл для произведения свертки (поэлементное умножение двух матриц)
                        for (int j = 0; j < lengthSY; j++)//размер окретсности пикселя изображения = размеру ядра
                        {
                            gx += g[x + i - lengthSX / 2, y + j - lengthSX / 2] * sx[i, j]; //собсна сама свертка по оси Х. Для оси У делаю транспонирование
                            gy += g[x + i - lengthSY / 2, y + j - lengthSY / 2] * sx[j, i]; //сразу же делаю транспонирование матрицы sx (столбцы это строки)
                        }
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy); //результирующее значение пикселя после фильтрации
                }
            return result;
        }
    }
}