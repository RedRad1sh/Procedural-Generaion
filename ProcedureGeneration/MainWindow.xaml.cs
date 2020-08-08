using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcedureGeneration
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewPerlinNoise();
        }

        private void NewPerlinNoise()
        {
            PerlinNoise perl = new PerlinNoise();
            PerlinNoise.Lerps lerp = WhatLerp();
            int height = (int)PerlinImage.ActualHeight;
            int width = (int)PerlinImage.ActualWidth;
            WriteableBitmap bmp = BitmapFactory.New(width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    float xOfPoint = (float)i / (int)(Size.Value);
                    float yOfPoint = (float)j / (int)(Size.Value);
                    float noise = PerlinNoise.Noise(xOfPoint, yOfPoint, (int)Octaves.Value, (float)Persistense.Value,(float) Frequency.Value, lerp);
                    PaintPixel(i,j,bmp,noise);
                }
            }
            PerlinImage.Source = bmp;
        }

        private PerlinNoise.Lerps WhatLerp()
        {
            if ((bool) LerpWithQuinticCurve.IsChecked)
                return PerlinNoise.Lerps.LinLerpWithQuintic;
            if ((bool)CosLerp.IsChecked)
                return PerlinNoise.Lerps.CosLerp;
            return PerlinNoise.Lerps.LinearLerp;
        }

        private void PaintPixel(int i, int j, WriteableBitmap bmp, float noise)
        {
            if ((bool) ColorsAdd.IsChecked)
            {
                if (noise < 0.05)
                    bmp.SetPixel(i, j, Colors.DarkBlue);
                else if (noise < 0.09)
                    bmp.SetPixel(i, j, Colors.Blue);
                else if (noise < 0.12)
                    bmp.SetPixel(i, j, Colors.AntiqueWhite);
                else if (noise < 0.235)
                    bmp.SetPixel(i, j, Colors.Chartreuse);
                else if (noise < 0.35)
                    bmp.SetPixel(i, j, Colors.DimGray);
                else if (noise < 1)
                    bmp.SetPixel(i, j, Colors.Gray);
            }
            else bmp.SetPixel(i, j, Color.FromScRgb(1, noise * 2, noise * 2, noise * 2));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewPerlinNoise();
        }
    }
}
