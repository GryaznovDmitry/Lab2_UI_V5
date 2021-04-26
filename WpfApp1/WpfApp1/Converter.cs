using System.Numerics;
using System.Windows.Data;
using System;
using ClassLibrary1;
using System.Collections.Generic;

namespace WpfApp1
{
    [ValueConversion(typeof(Grid2D), typeof(string))]
    public class GridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Grid2D grid = (Grid2D)value;
            string str = $"Grid X: Step = {grid.StepX}, \nStep Num = {grid.NodeNumX}\n" +
                         $"Grid Y: Step = {grid.StepY}, \nStep Num = {grid.NodeNumY}\n "; ;
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}