using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZedGraph;

namespace Fomins
{
    public static class FominsFunctionality
    {
        public static void FillGrid(Grid myGrid, string photoPath, int size, int kolLikes, int kolComments, int row, int column,int kolcolumns)
        {
          
            myGrid.HorizontalAlignment = HorizontalAlignment.Left;
            myGrid.VerticalAlignment = VerticalAlignment.Top;

            System.Windows.Controls.Image image1 = new System.Windows.Controls.Image();
            image1.Width = size;
            image1.Height = size;
            image1.Stretch = Stretch.UniformToFill;
            image1.Margin = new Thickness(5, 5, 5, 5);
            image1.Source = new BitmapImage(new Uri(photoPath));
            Grid.SetRow(image1, row);
            Grid.SetColumn(image1, column);
            myGrid.Children.Add(image1);

            Canvas canvas1 = new Canvas();
            canvas1.Width = size - 10;
            canvas1.Height = size / 6;
            canvas1.VerticalAlignment = VerticalAlignment.Bottom;
            canvas1.HorizontalAlignment = HorizontalAlignment.Center;
            canvas1.Margin = new Thickness(10, 10, 10, 5);
            System.Windows.Media.Color color = Colors.Black;
            color.A = 180;
            canvas1.Background = new SolidColorBrush(color);
            Grid.SetRow(canvas1, row);
            Grid.SetColumn(canvas1, column);
            myGrid.Children.Add(canvas1);

            TextBox heart = new TextBox();
            heart.Background = new SolidColorBrush(Colors.Transparent);
            heart.BorderBrush = new SolidColorBrush(Colors.Transparent);
            heart.Foreground = new SolidColorBrush(Colors.White);
            heart.FontSize = size / 8.33;
            heart.IsReadOnly = true;
            heart.Text = "❤";
            /*Convert.ToString(((char)10084));*/    //9825
            Canvas.SetTop(heart, 0.001);
            Canvas.SetLeft(heart, 5);
            canvas1.Children.Add(heart);

            TextBox likes = new TextBox();
            likes.Background = new SolidColorBrush(Colors.Transparent);
            likes.BorderBrush = new SolidColorBrush(Colors.Transparent);
            likes.Foreground = new SolidColorBrush(Colors.White);
            likes.FontSize = size / 10.7;
            likes.IsReadOnly = true;
            likes.Text = kolLikes.ToString();
            Canvas.SetTop(likes, 2);
            Canvas.SetLeft(likes, size / 6);
            canvas1.Children.Add(likes);

            TextBox cloud = new TextBox();
            cloud.Background = new SolidColorBrush(Colors.Transparent);
            cloud.BorderBrush = new SolidColorBrush(Colors.Transparent);
            cloud.Foreground = new SolidColorBrush(Colors.White);
            cloud.FontSize = size / 10.7;
            cloud.IsReadOnly = true;
            cloud.Text = "Com:";
            /*Convert.ToString(((char)10084));*/    //9825
            Canvas.SetTop(cloud, 2);
            Canvas.SetLeft(cloud, size / 2.5);
            canvas1.Children.Add(cloud);

            TextBox comments = new TextBox();
            comments.Background = new SolidColorBrush(Colors.Transparent);
            comments.BorderBrush = new SolidColorBrush(Colors.Transparent);
            comments.Foreground = new SolidColorBrush(Colors.White);
            comments.FontSize = size / 10.7;
            comments.IsReadOnly = true;
            comments.Text = kolComments.ToString();
            Canvas.SetTop(comments, 2);
            Canvas.SetLeft(comments, size / 1.578);
            canvas1.Children.Add(comments);
        }
        public static Bitmap GetGraph(List<int> data, List<DateTime> date, string xAxis, string yAxis, string graphName)
        {
            ZedGraphControl zedGraph = new ZedGraphControl();
            if (data.Count == 0)
            {
                return null;
            }
            GraphPane myPane = zedGraph.GraphPane;
            myPane.CurveList.Clear();
            PointPairList list1 = new PointPairList();
            DateTime l, r;
            l = date.First();
            r = date.Last();

            for (int i = 0; i < data.Count; i++)
            {
                list1.Add(date[i].ToOADate(), data[i]);
            }
            LineItem myCurve = myPane.AddCurve("Line1",
            list1, System.Drawing.Color.Red, SymbolType.Diamond);

            myPane.XAxis.Type = AxisType.Date;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.DashOff = 0;
            myPane.YAxis.MajorGrid.Color = System.Drawing.Color.DarkGray;
            myPane.XAxis.Scale.Min = l.ToOADate();
            myPane.XAxis.Scale.Max = r.ToOADate();
            myPane.XAxis.Scale.Format = "dd-MMM-yy";
            myPane.XAxis.Title.Text = xAxis;
            myPane.YAxis.Title.Text = yAxis;
            myPane.Title.Text = graphName;
            myPane.Legend.IsVisible = false;
            zedGraph.Enabled = false;

            myCurve.Line.Width = 4.0F;
            myCurve.Line.Color = System.Drawing.Color.FromArgb(189, 43, 43);
            myCurve.Line.Fill = new Fill(System.Drawing.Color.FromArgb(100, 207, 236, 255),
            System.Drawing.Color.FromArgb(200, 145, 213, 255), 90F);

            myCurve.Symbol.Size = 12.0F;
            myCurve.Symbol.Fill = new Fill(System.Drawing.Color.FromArgb(145, 16, 16));
            myCurve.Symbol.Border.Color = System.Drawing.Color.FromArgb(99, 4, 4);

            zedGraph.AxisChange();
            zedGraph.Invalidate();

            Bitmap bmp = zedGraph.MasterPane.GetImage(1024, 768, 96);
            return bmp;
        }
    }
}
