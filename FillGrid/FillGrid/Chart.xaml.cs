using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace NewChart
{
    public partial class Chart : Page, INotifyPropertyChanged
    {
        private double to;
        private double ysep;
        private double yto;
        private double yfrom;
        private double from;
        private double step = 6;
        private double curStep = 6;
        private double left, right,res;
        private string xTitle, yTitle;
        List<DateTime> date = new List<DateTime>();
        List<int> data = new List<int>();

        public Chart(List<DateTime> dateList,List<int> dataList,string xTitle,string yTitle)
        {
            InitializeComponent();

            YTitle = yTitle;
            XTitle = xTitle;

            date = dateList;
            data = dataList;

            if (date.Count == 0)
                return;
            if(date.Count==1)
            {
                date.Add(date[0]);
                DateTime prev = date[0].AddDays(-1);
                date[0]=prev;
                data.Add(data[0]);
            }
            if(date.Count>0 && date.Count<step)
            {
                From = date[0].Date.AddYears(1899).AddDays(-1).ToOADate();
                To = date[date.Count-1].Date.AddYears(1899).AddDays(-1).ToOADate();
            }
            if (date.Count >= step)
            {
                To = date[date.Count - 1].Date.AddYears(1899).AddDays(-1).ToOADate();   //установка границ отрисовки графика
                From = (To-step);
            }
            left= date[0].Date.AddYears(1899).AddDays(-1).ToOADate();
            right= date[date.Count - 1].Date.AddYears(1899).AddDays(-1).ToOADate();   

            SetYScale();

            var dayConfig = Mappers.Xy<DateModel>()
             .X(dateModel => dateModel.DateTime.Ticks / TimeSpan.FromDays(1).Ticks)
             .Y(dateModel => dateModel.Value);                                         //распределение данных по осям

            Series = new SeriesCollection(dayConfig)
            {
                new LineSeries                                                  //создание линии
                {
                    Title=yTitle,
                    Values = GetData(date,data),
                    StrokeThickness = 1
                }
            };

            res = To - From;
            curStep = step;

            Formatter = value => new DateTime((long)(value * TimeSpan.FromDays(1).Ticks)).ToString("dd MMM yyyy");    //форматирование оси x

            DataContext = this;
        }

        public Func<double, string> Formatter { get; set; }
        public SeriesCollection Series { get; set; }

        public string YTitle
        {
            get { return yTitle; }
            set
            {
                yTitle = value;
                OnPropertyChanged("YTitle");
            }
        }

        public string XTitle
        {
            get { return xTitle; }
            set
            {
                xTitle = value;
                OnPropertyChanged("XTitle");
            }
        }

        public double From
        {
            get { return from; }
            set
            {
                from = value;
                OnPropertyChanged("From");
            }
        }

        public double To
        {
            get { return to; }
            set
            {
                to = value;
                OnPropertyChanged("To");
            }
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
            bool exist = false;
            int change = 0;
            bool check = false;
            int start = 0;
            int stop = 0;
            for (int i=0;i<date.Count;i++)
            {
                if (date[i].Date == DateTime.FromOADate(From).AddYears(-1899).AddDays(1))
                {
                    exist = true;
                    start = i;
                    break;
                }
                if (DateTime.FromOADate(From).AddYears(-1899).AddDays(1) > date[i].Date && !check)
                {
                    check = true;
                    change = i;
                }
            }
            if (!exist)
            {
                start = change;
            }

            exist = false;

            for (int i=start;i<date.Count;i++)
            {
                DateTime dt= DateTime.FromOADate(To).AddYears(-1899).AddDays(1);
                if (date[i].Date == DateTime.FromOADate(To).AddYears(-1899).AddDays(1))
                {
                    exist = true;
                    stop = i;
                    break;
                }
                if(DateTime.FromOADate(To).AddYears(-1899).AddDays(1)< date[i].Date)
                {
                    change = i;
                }
            }
            if(!exist)
            {
                stop = change - 1;
            }

            YTo = 0;
            for (int i = start; i <= stop; i++)
            {
                if (data[i] > YTo)
                    YTo = data[i];
            }
            YTo += YTo/100*20;
            YFrom = 0;
            YSep=YTo/6;
        }          //установка масштаба оси y

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ChartValues<DateModel> GetData(List<DateTime> date, List<int> data)
        {
            ChartValues<DateModel> values = new ChartValues<DateModel>();
            for (int i = 0; i < date.Count; i++)
            {
                DateModel dm = new DateModel();
                dm.DateTime = date[i].Date;
                dm.Value = data[i];
                values.Add(dm);
            }
            return values;
        }

        private void PrevButtonOnClick(object sender, RoutedEventArgs e)
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

        private void NextButtonOnClick(object sender, RoutedEventArgs e)
        {
            From += curStep;
            To += curStep;
            if (To > right)
            {
                To = right;
                From = To - curStep;
            }
            SetYScale();
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (res <= step)
                return;
            From += step / 2;
            To -= step / 2;
            SetYScale();
            res = To - From;
            curStep = res;
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (curStep > 50)
            {
                return;
            }
            From -= step / 2;
            To += step / 2;
            if (To > right)
            {
                res = To - right;
                To = right;
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
                if (To > right)
                {
                    To = right;
                }
            }
            SetYScale();
            res = To - From;
            curStep = res;
        }

    }

    public class DateModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

}