//Представьте себе робота-уборщика на кухне, которого только что случайно пнула хозяйка.Ему нужно сориентироваться, где он теперь находится и куда повёрнут.К счастью у робота есть камера, а пол на кухне выложен квадратной кафельной плиткой. Осталось немного обработать изображение с видеокамеры, выделить границы объектов и по ним сориентироваться.

//Первым шагом нужно перевести цветное изображение в оттенки серого.Его будет проще анализировать.

//Выполните эту задачу в файле GrayscaleTask.cs

namespace Recognizer
{
    public static class GrayscaleTask
    {
        /* 
		 * Переведите изображение в серую гамму.
		 * 
		 * original[x, y] - массив пикселей с координатами x, y. 
		 * Каждый канал R,G,B лежит в диапазоне от 0 до 255.
		 * 
		 * Получившийся массив должен иметь те же размеры, 
		 * grayscale[x, y] - яркость пикселя (x,y) в диапазоне от 0.0 до 1.0
		 *
		 * Используйте формулу:
		 * Яркость = (0.299*R + 0.587*G + 0.114*B) / 255
		 * 
		 * Почему формула именно такая — читайте в википедии 
		 * http://ru.wikipedia.org/wiki/Оттенки_серого
		 */

        public static double[,] ToGrayscale(Pixel[,] original)
        {
            double[,] grayscale = new double[original.GetLength(0), original.GetLength(1)];
            int lenghtX = original.GetLength(0);
            int lenghtY = original.GetLength(1);
            for (int x = 0; x < lenghtX; x++)
                for (int y = 0; y < lenghtY; y++)
                {
                    grayscale[x, y] = (0.299 * original[x, y].R + 0.587 * original[x, y].G + 0.114 * original[x, y].B) / 255;
                }
            return grayscale;
        }
    }
}