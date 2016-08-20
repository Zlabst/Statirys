using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Windows.Media;
using LiveCharts;

namespace Graphics
{
    public partial class MediaChart : Page, INotifyPropertyChanged
    {
        private int to;
        private double yto;
        private double ysep;
        private double yfrom;
        private int from;
        private int step = 6;
        private int curStep;
        private int left = 0;
        int res;
        double temp;

        ChartValues<double> data = new ChartValues<double>();
        List<string> date = new List<string>();
        List<DateTime> dateConverted = new List<DateTime>();

        public MediaChart(List<DateTime> date,List<int> values)
        {
            InitializeComponent();

            this.dateConverted = date;
            for (int i = 0; i < date.Count; i++)
            {
                this.date.Add(dateConverted[i].ToString("dd MMM"));
                this.data.Add(values[i]);
            }
            SeriesCollection = new SeriesCollection // задать значения элементам графика
            {
                new ColumnSeries
                {
                    Title = "Media",
                    Values = this.data
                }
            };
            Labels = this.date;

            To = date.Count;
            res = To - step;
            if (res >= 0)
            {
                From = To - step;
                res = To - From;
                curStep = step;
            }
            else
            {
                From = 0;
                curStep = To - From;
            }
            SetYScale();
            DataContext = this;
            //задать изначальные значения левой и правой границы отрисованной области

        }

        public int From
        {
            get { return from; }
            set
            {
                from = value;
                OnPropertyChanged("From");
            }
        }
        public int To
        {
            get { return to; }
            set
            {
                to = value;
                OnPropertyChanged("To");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<int, string> Formatter { get; set; }

        private void NextOnClick(object sender, RoutedEventArgs e)
        {

            From += curStep;
            To += curStep;
            if (To > date.Count)
            {
                To = date.Count;
                From = To - curStep;
            }
            SetYScale();
        }
        private void PrevOnClick(object sender, RoutedEventArgs e)
        {
            From -= curStep;
            To -= curStep;
            if (From < left)
            {
                From = left;
                To = From + curStep;
            }
            SetYScale();
        }
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {

            From += step / 2;
            To -= step / 2;
            res = To - From;
            if(res>=step)
            {
                SetYScale();
                res = To - From;
                curStep = res;
            }
            else
            {
                From -= step / 2;
                To += step / 2;
            }
            
        }
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if(curStep>35)
            {
                return;
            }
            From -= step / 2;
            To += step / 2;
            if (To > date.Count)
            {
                res = To - date.Count;
                To = date.Count;
                From -= res;
                if (From < left)
                {
                    From = left;
                }
            }
            if (From < left)
            {
                res = left - From;
                From = left;
                To += res;
                if (To > date.Count)
                {
                    To = date.Count;
                }
            }
            SetYScale();
            res = To - From;
            curStep = res;
        }

        public double YTo
        {
            get { return yto; }
            set
            {
                yto = value;
                OnPropertyChanged("YTo");
            }
        }

        public double YFrom
        {
            get { return yfrom; }
            set
            {
                yfrom = value;
                OnPropertyChanged("YFrom");
            }
        }

        public double YSep
        {
            get { return ysep; }
            set
            {
                ysep = value;
                OnPropertyChanged("YSep");
            }
        }

        public void SetYScale()
        {

            int start = From;
            int stop = To;
            if (stop == -1)
                stop = dateConverted.Count;
            YTo = 0;
            if(stop==0 && start==0)
            {
                YTo = data[0];
            }
            else
            {
                for (int i = start; i < stop; i++)
                {
                    if (data[i] > YTo)
                        YTo = data[i];
                }
            }
            
            YTo += YTo / 100 * 20;
            YFrom = 0;
            YSep = YTo / 6;
        }
    }
}
