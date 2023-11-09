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



namespace WindowsFormsApp
{

    public partial class Form1 : Form
    {

        void CalculateFunction(double listOfPoints)
        {

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.chart1.Series[0].Points.Clear();
            foreach (Control c in Controls)
            {
                if (c.GetType() == typeof(GroupBox))
                    foreach (Control d in c.Controls)
                        if (d.GetType() == typeof(TextBox))
                            d.Text = string.Empty;

                if (c.GetType() == typeof(TextBox))
                    c.Text = string.Empty;
            }
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Point> listOfPoints = new List<Point>();

            // наполнение списка
            Point p;
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                try
                { 
                p.x = Convert.ToDouble(textBox1.Text);
                p.y = Convert.ToDouble(textBox2.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 1 [" + textBox1.Text + ", " + textBox2.Text + "] не будут учитываться при построении графика");                    
                }
            }
            if (textBox3.Text != "" && textBox4.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox3.Text);
                p.y = Convert.ToDouble(textBox4.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 2 [" + textBox3.Text + ", " + textBox4.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox5.Text != "" && textBox6.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox5.Text);
                p.y = Convert.ToDouble(textBox6.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 3 [" + textBox5.Text + ", " + textBox6.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox7.Text != "" && textBox8.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox7.Text);
                p.y = Convert.ToDouble(textBox8.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 4 [" + textBox7.Text + ", " + textBox8.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox9.Text != "" && textBox10.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox9.Text);
                p.y = Convert.ToDouble(textBox10.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 5 [" + textBox9.Text + ", " + textBox10.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox11.Text != "" && textBox12.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox11.Text);
                p.y = Convert.ToDouble(textBox12.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 6 [" + textBox11.Text + ", " + textBox12.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox13.Text != "" && textBox14.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox13.Text);
                p.y = Convert.ToDouble(textBox14.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 7 [" + textBox13.Text + ", " + textBox14.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox15.Text != "" && textBox16.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox15.Text);
                p.y = Convert.ToDouble(textBox16.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 8 [" + textBox15.Text + ", " + textBox16.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox17.Text != "" && textBox18.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox17.Text);
                p.y = Convert.ToDouble(textBox18.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 9 [" + textBox17.Text + ", " + textBox18.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox19.Text != "" && textBox20.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox19.Text);
                p.y = Convert.ToDouble(textBox20.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 10 [" + textBox19.Text + ", " + textBox20.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox21.Text != "" && textBox22.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox21.Text);
                p.y = Convert.ToDouble(textBox22.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 11 [" + textBox21.Text + ", " + textBox22.Text + "] не будут учитываться при построении графика");
                }
            }
            if (textBox23.Text != "" && textBox24.Text != "")
            {
                try
                {
                p.x = Convert.ToDouble(textBox23.Text);
                p.y = Convert.ToDouble(textBox24.Text);
                listOfPoints.Add(p);
                }
                catch
                {
                    MessageBox.Show("Неверный формат записи первой точки координат. " +
                                    "Координаты должны быть записаны в виде целого числа или десятичной дроби, " +
                                    "дробная часть которой отделена запятой. " +
                                    "Координаты точки 12 [" + textBox23.Text + ", " + textBox24.Text + "] не будут учитываться при построении графика");
                }
            }

            // сортировка списка
            listOfPoints.Sort((left, right) => left.x.CompareTo(right.x));

            // сообщение, если есть одинаковые x
            for (int i = 1, sizeOfTheList = listOfPoints.Count; i < sizeOfTheList; i++)
            {
                if (listOfPoints[i].x == listOfPoints[i - 1].x)
                {
                    MessageBox.Show("Введены несколько точек с одинаковыми координатами x! Точка с координатами [" 
                                    + listOfPoints[i].x + "; " + listOfPoints[i].y
                                    + "] не будет учитываться при интерполяции");
                    listOfPoints.RemoveAt(i);
                    --sizeOfTheList;
                    --i;
                }
            }

            List<Point> listInterPolation = new List<Point>(); // список для хранения интерполируемых точек
            void Interpolation() // функция интерполяции
            {
                int n = listOfPoints.Count - 1; // количество интервалов между точками
                double[] k = new double[n + 1];
                double[] c = new double[n + 2];
                k[1] = 0;
                c[1] = 0;
                int i, j, l, m; // счётчики
                double a, b, q, g, h, d; // коэффициенты
                double x, y; // полученные при интерполяции координаты
                for (i = 2; i <= n; i++)
                {
                    j = i - 1;
                    m = j - 1;
                    a = listOfPoints[i].x - listOfPoints[j].x;
                    b = listOfPoints[j].x - listOfPoints[m].x;
                    g = 2 * (a + b) - b * c[j];
                    c[i] = a / g;
                    k[i] = (3.0 * ((listOfPoints[i].y - listOfPoints[j].y) / a - (listOfPoints[j].y - listOfPoints[m].y) / b) - b * k[j]) / g;
                }
                c[n + 1] = k[n];
                for (i = n; i >= 0; i--)
                {
                    c[i] = k[i] - c[i] * c[i + 1];
                }

                int nx = 100; // количество интерполируемых точек между заданными пользователем точками
                for (i = 1; i <= n; i++)
                {
                    h = (listOfPoints[i].x - listOfPoints[i - 1].x) / (nx - 1);
                    for (l = 0; l < nx; l++)
                    {
                        x = listOfPoints[i - 1].x + l * h;
                        q = listOfPoints[i].x - listOfPoints[i - 1].x;
                        g = x - listOfPoints[i - 1].x;
                        b = (listOfPoints[i].y - listOfPoints[i - 1].y) / q - (c[i + 1] + 2 * c[i]) * q / 3.0;
                        d = (c[i + 1] - c[i]) / q * g;
                        y = listOfPoints[i - 1].y + g * (b + g * (c[i] + d / 3.0));

                        p.x = Convert.ToDouble(x);
                        p.y = Convert.ToDouble(y);
                        listInterPolation.Add(p);
                    }
                }
            }
            try
            {
                Interpolation();
                chart1.ChartAreas[0].AxisX.Minimum = listInterPolation[0].x;
                chart1.ChartAreas[0].AxisX.Maximum = listInterPolation[listInterPolation.Count - 1].x;
                this.chart1.Series[0].Points.Clear();

                foreach (Point point in listInterPolation)
                {
                    this.chart1.Series[0].Points.AddXY(point.x, point.y);
                }
            }
            catch 
            {
                MessageBox.Show("Недостаточно точек для интерполяции функции");
            }
        }
    }
}

struct Point
{
    public double x;
    public double y;
}