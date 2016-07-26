using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using ZedGraph;


namespace Fomins
{
    public static class FominsFunctionality
    {
        public static Bitmap GetGraph(List<int> data, List<DateTime> date, string xAxis, string yAxis, string graphName)
        {
            ZedGraphControl zedGraph = new ZedGraphControl();
            if (data.Count == 0)
            {
                return null;
            }

            if (data.Count == 1)
            {
                data.Add(data[0]);
                date.Add(date[0].AddSeconds(1));
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
                  list1, Color.Red, SymbolType.Diamond);

            myPane.XAxis.Type = AxisType.Date;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.DashOff = 0;
            myPane.YAxis.MajorGrid.Color = Color.DarkGray;
            myPane.XAxis.Scale.Min = l.ToOADate();
            myPane.XAxis.Scale.Max = r.ToOADate();
            myPane.XAxis.Scale.Format = "dd-MMM-yy";
            myPane.XAxis.Title.Text = xAxis;
            myPane.YAxis.Title.Text = yAxis;
            myPane.Title.Text = graphName;
            myPane.Legend.IsVisible = false;
            zedGraph.Enabled = false;

            myCurve.Line.Width = 4.0F;
            myCurve.Line.Color = Color.FromArgb(189, 43, 43);
            myCurve.Line.Fill = new Fill(Color.FromArgb(100, 207, 236, 255),
                Color.FromArgb(200, 145, 213, 255), 90F);

            myCurve.Symbol.Size = 12.0F;
            myCurve.Symbol.Fill = new Fill(Color.FromArgb(145, 16, 16));
            myCurve.Symbol.Border.Color = Color.FromArgb(99, 4, 4);

            zedGraph.AxisChange();
            zedGraph.Invalidate();

            Bitmap bmp = zedGraph.MasterPane.GetImage(1024, 768, 96);
            return bmp;
        }
    }
}
