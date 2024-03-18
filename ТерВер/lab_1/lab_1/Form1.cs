using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace lab_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Создание объекта для генерации чисел
            Random rnd = new Random();

            List<int> numbers = new List<int>();


            for (int i = 0; i < 101; i++)
            {
                numbers.Add(rnd.Next(0, 100));
            }
            //создаем элемент Chart
            Chart myChart = new Chart();
            //кладем его на форму и растягиваем на все окно.
            myChart.Parent = this;
            myChart.Dock = DockStyle.Fill;
            //добавляем в Chart область для рисования графиков, их может быть
            //много, поэтому даем ей имя.
            myChart.ChartAreas.Add(new ChartArea("Math functions"));
            //Создаем и настраиваем набор точек для рисования графика, в том
            //не забыв указать имя области на которой хотим отобразить этот
            //набор точек.
            Series mySeriesOfPoint = new Series("Sinus");
            mySeriesOfPoint.ChartType = SeriesChartType.Point;
            mySeriesOfPoint.ChartArea = "Math functions";
            //numbers.GroupBy(x => (int)((x - min) / rangeSize)).OrderBy(x => x.Key).Select(x => x.Count()).ToList();

            var groups = numbers.GroupBy(n => n);
            foreach (var group in groups) 
            {
                myChart.Series.AddXY(group,);
            }
            //Добавляем созданный набор точек в Chart
            myChart.Series.Add(mySeriesOfPoint);
        }
        private void chart1_Click(object sender, EventArgs e)
        {
        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
