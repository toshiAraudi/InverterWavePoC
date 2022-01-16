using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace csvPoC
{
    public partial class Form1 : Form
    {
        string FILEPATH = @"CSVFILEPATH";
        string[] lines = null;
        string[] columns = null;
        int CLMLEN = 0;
        int INITPOINT = 100000;
        int ITER = 1000;

        public Form1()
        {
            InitializeComponent();
        }

        private void CsvRead()
        {
            using (var sr = new StreamReader(FILEPATH))
            {
                string filetext = sr.ReadToEnd();
                lines = filetext.Replace("\r\n", "\n").Split(new[] { '\n', '\r' });
            }
            columns = lines[0].Split(',');
            CLMLEN = columns.Length;
        }

        private void ChartInit()
        {
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();

            ChartArea chartAria = new ChartArea("chartArea");
            chart1.ChartAreas.Add(chartAria);
        }

        private void Charting()
        {
            for(int clm = 1; clm < CLMLEN; clm++)
            {
                Series series = new Series();
                series.ChartType = SeriesChartType.Line;
                series.LegendText = columns[clm];

                //for (int i = 1; i < lines.Length; i++)
                for (int i = 1; i < INITPOINT; i++)
                {
                    
                    try
                    {
                        var sp = lines[i].Split(',');
                        var x = double.Parse(sp[0]);
                        var y = double.Parse(sp[clm]);
                        series.Points.AddXY(x, y);
                    }
                    catch(Exception e)
                    {

                    }
                }
                chart1.Series.Add(series);
            }

            chart1.ChartAreas["chartArea"].AxisX.ScaleView.Size = int.Parse(label1.Text);
            chart1.ChartAreas["chartArea"].AxisX.IsMarginVisible = false;
        }

        private void AddPointsBlock(int point, int rows)
        {
            for (int clm = 1; clm < CLMLEN; clm++)
            {
                Series series = chart1.Series[clm - 1];

                //for (int i = 1; i < lines.Length; i++)
                for (int i = point; i < point + rows; i++)
                {

                    try
                    {
                        var sp = lines[i].Split(',');
                        var x = double.Parse(sp[0]);
                        var y = double.Parse(sp[clm]);
                        series.Points.AddXY(x, y);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }


        private void ScaleUpdate(bool ud)
        {
            int scale = int.Parse(label1.Text);
            if (ud) scale *= 10;
            else scale /= 10;


            label1.Text = scale.ToString();
            chart1.ChartAreas["chartArea"].AxisX.ScaleView.Size = scale;
            chart1.ChartAreas["chartArea"].AxisX.IsMarginVisible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CsvRead();
            ChartInit();
            Charting();

            /* 
            int p = INITPOINT;
            while (p < lines.Length)
            {
                int numOfPoints = ITER;
                if (p + ITER >= lines.Length)
                {
                    numOfPoints = lines.Length - p - 1;
                }
                Task.Run(() => AddPointsBlock(p, numOfPoints));

            } */
            //MessageBox.Show("finished" + lines.Length.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ScaleUpdate(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ScaleUpdate(false);
        }
    }
}
