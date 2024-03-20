using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Demo_2
{
    public partial class Form1 : Form
    {

        private List<int> numbersList;
        private void Form1_Resize(object sender, EventArgs e)
        {
            chart1.Size = new Size(this.ClientSize.Width - 50, this.ClientSize.Height - 50);
            chart1.Location = new Point(25, 25);
        }
        public Form1()
        {
            InitializeComponent();
            numbersList = new List<int>();
            this.Resize += new EventHandler(Form1_Resize);
        }
        public void button1_Click(object sender, EventArgs e)
        {

            // Генерируем случайное число и добавляем его в список
            Random random = new Random();

            List<double> P = new List<double>();
            List<double> alpha = new List<double>();
            List<int> ksi = new List<int>(new int[100]);


            double p = 0.25;
            double q = 1 - p;

            string path = @"C:\Users\leone\Desktop\Учеба\ТИ и тервер\ТерВер\Demo_2\content.txt";
            string path1 = @"C:\Users\leone\Desktop\Учеба\ТИ и тервер\ТерВер\Demo_2\content1.txt";

            for (int i = 0; i < 100; i++)
            {
                double rnd = random.NextDouble();
                alpha.Add(rnd);
            }

            for (int i = 0; i < 100; i++)
            {
                int m = 0;
                double pm = p;
                double gamma = pm;
                while (alpha[i] >= gamma)
                {
                    m++;
                    pm *= q;
                    gamma += pm;
                }
                ksi[i] = m;
            }

            for (int i = 0; i < 100; i++)
            {
                numbersList.Add(ksi[i]);

            }
            File.WriteAllLines(path1, ksi.ConvertAll(x => x.ToString()));
            File.WriteAllText(path1, string.Join(" ", ksi));
            ksi.Sort();
            File.WriteAllLines(path, ksi.ConvertAll(x => x.ToString()));
            File.WriteAllText(path, string.Join(" ", ksi));


            // Создание словаря для хранения частоты появления чисел
            Dictionary<int, int> frequency = new Dictionary<int, int>();
            

            foreach (int num in numbersList)
            {
                if (frequency.ContainsKey(num))
                {
                    frequency[num]++;
                }
                else
                {
                    frequency[num] = 1;
                }
            }
            // Очистка графика перед построением нового

            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.Interval = 1;

            // Создание серии для отображения данных на графике
            Series series = new Series("Частота появления чисел");
            series.ChartType = SeriesChartType.Column;

            // Добавление точек на график (значение x - число, значение y - частота)

            foreach (KeyValuePair<int, int> entry in frequency)
            {
                series.Points.AddXY(entry.Key, (float)entry.Value/100);
            }
            chart1.ChartAreas[0].AxisX.Title = "Значение";
            chart1.ChartAreas[0].AxisY.Title = "Частота";
            // Добавление серии на график
            chart1.Series.Add(series);
            frequency.Clear();
            numbersList.Clear();
        }
        private void chart1_Click(object sender, EventArgs e)
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
