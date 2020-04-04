namespace Concierge.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class Utilities
    {
        public static int CalculateBonus(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static BitmapImage ToBitmapImage(this System.Drawing.Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static string FormatName(string name)
        {
            char[] ch = name.ToArray();
            int offset = 0;

            for (int i = 1; i < ch.Length; i++)
            {
                if (char.IsUpper(ch[i]))
                {
                    name = name.Insert(i + offset, " ");
                    offset++;
                }
            }

            return name;
        }

        public static Brush SetUsedTextStyle(int total, int used)
        {
            if (total <= used)
                return Brushes.DarkRed;
            else
                return Brushes.White;
        }

        public static Brush SetUsedBoxStyle(int total, int used)
        {
            if (total <= used)
                return Brushes.IndianRed;
            else
                return new SolidColorBrush(Color.FromArgb(255, 62, 62, 66));
        }

        public static Brush SetTotalTextStyle(int total, int used)
        {
            if (total <= used)
                return Brushes.DarkGray;
            else
                return Brushes.White;
        }

        public static Brush SetTotalBoxStyle(int total, int used)
        {
            if (total <= used)
                return new SolidColorBrush(Color.FromArgb(255, 15, 15, 15));
            else
                return new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
        }
    }
}
