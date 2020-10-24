//Пора превратить изображение в черно-белое.

//Сделать это можно с помощью порогового преобразования.Реализуйте его в методе

//public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)

//Метод должен заменять пиксели со значением больше либо равному T на белый(1.0), а остальные на черный(0.0).

//Пороговое значение найдите так, чтобы:

//если N — общее количество пикселей изображения, то как минимум(int)(whitePixelsFraction* N) пикселей стали белыми;
//при этом белыми стало как можно меньше пикселей.
//Обратите внимание, что если пиксель некоторого цвета алгоритм сделал белым, то и все другие пиксели того же цвета должны стать белыми
//Например, одноцветное изображение 10x10 при whitePixelsFraction=0.2 должно целиком стать белым.Потому что как минимум 20% пикселей нужно сделать белыми, 
//но остальные 80% имеют тот же цвет, а значит тоже должны стать белыми
using System.Collections.Generic;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            int countBlackPixel = original.Length - (int)(original.Length * whitePixelsFraction); //рассчет количества черных пикселей
            double T = 0; //то самое пороговое значение по которому будет определеятся кто белый а кто черный пиксель
            SortedList<double, int> diagrammimage = new SortedList<double, int>(); //отсортированная коллекция для количественного "анализа" пикселелей в исходном изображении
            int lenghtX = original.GetLength(0);
            int lenghtY = original.GetLength(1);
            double[,] result = new double[lenghtX, lenghtY]; //массив с результатами
            for (int x = 0; x < lenghtX; x++)
                for (int y = 0; y < lenghtY; y++)
                {
                    if (diagrammimage.ContainsKey(original[x, y])) //заполняем коллекцию, здесь
                        diagrammimage[original[x, y]]++; //ключ в коллекции это значение пикселя
                    else
                        diagrammimage.Add(original[x, y], 1); //а значение в коллекции это какое количество пикселей с этим значением
                }
            foreach (var pixelcount in diagrammimage) //получаем пары "ключ-значение" из коллекции
            {
                countBlackPixel -= pixelcount.Value; //коллекция уже отсортирована начиная от самых "темных" пикселей к "светлым"
                if (countBlackPixel < 0) //нужно просто отсчитать нужное количество "темных" пикселей (в значениях коллекции и содержится количество пикселей)
                {
                    T = pixelcount.Key; //и на том значении пикселя (значение которого и является ключом в коллекции) 
                    break; //на котором я получаю нужное кол-во черных пикселей я устанавливаю в качестве порогового фильтра
                }

            }
            for (int x = 0; x < lenghtX; x++)
                for (int y = 0; y < lenghtY; y++)
                {
                    if ((int)(original.Length * whitePixelsFraction) == 0) //костыль для случаев когда нужно получить черное изображение
                        result[x, y] = 0;
                    else
                        result[x, y] = original[x, y] >= T ? 1 : 0; //если пиксель выше или равен пороговому значению то устанавливаю его в 1, если ниже то 0
                }
            return result;
        }
    }
}

