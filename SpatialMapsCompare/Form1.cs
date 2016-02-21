using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GeoLib;

namespace SpatialMapsCompare
{
    public class Form1 : Form
    {
        private List<C2DPoint> reference_polygon_C2DPoints = new List<C2DPoint>
        {
            new C2DPoint(0.50001, 0.750001),
            new C2DPoint(0.250001, 0.75001),
            new C2DPoint(0.250001, 0.5001),
            new C2DPoint(1E-05, 0.5001),
            new C2DPoint(1E-05, 0.25001),
            new C2DPoint(0.250001, 0.25001),
            new C2DPoint(0.250001, 0.0001),
            new C2DPoint(1.0001, 0.0001),
            new C2DPoint(1.0001, 0.5001),
            new C2DPoint(0.5001, 0.5001),
            new C2DPoint(0.5001, 0.7501)
        };

        private List<C2DPoint> oryginal_polygon_C2DPoints = new List<C2DPoint>
        {
            new C2DPoint(0.50001, 0.750001),
            new C2DPoint(0.250001, 0.750001),
            new C2DPoint(0.250001, 0.50001),
            new C2DPoint(1E-05, 0.50001),
            new C2DPoint(1E-05, 0.250001),
            new C2DPoint(0.250001, 0.250001),
            new C2DPoint(0.250001, 1E-05),
            new C2DPoint(1.00001, 1E-05),
            new C2DPoint(1.00001, 0.50001),
            new C2DPoint(0.50001, 0.50001),
            new C2DPoint(0.5001, 0.75001)
        };

        private List<C2DPoint> user_polygon_C2DPoints = new List<C2DPoint>
        {
            new C2DPoint(0.9060227713, 0.7615049749),
            new C2DPoint(0.9077990317, 0.6170206608),
            new C2DPoint(0.9503555075, 0.6028332772),
            new C2DPoint(0.9982237396, 0.5124085694),
            new C2DPoint(1.0, 0.4042553071),
            new C2DPoint(0.9716252329, 0.2109890933),
            new C2DPoint(0.7482250164, 0.1968073847),
            new C2DPoint(0.4485806658, 0.2003542306),
            new C2DPoint(0.4202127086, 0.1294286627),
            new C2DPoint(0.4343955522, 0.0549619239),
            new C2DPoint(0.4680809407, 0.0177285545),
            new C2DPoint(0.390070761, 0.0),
            new C2DPoint(0.2659561247, 0.0230459859),
            new C2DPoint(0.2180850551, 0.0248222463),
            new C2DPoint(0.1524825935, 0.0709198929),
            new C2DPoint(0.1187943675, 0.1577977549),
            new C2DPoint(0.06914874, 0.2145359392),
            new C2DPoint(0.0354599465, 0.2340407541),
            new C2DPoint(0.0, 0.3120543388),
            new C2DPoint(0.0035462784, 0.5531887847),
            new C2DPoint(0.0301419476, 0.645384078),
            new C2DPoint(0.406027595, 0.6950285706),
            new C2DPoint(0.4627657793, 0.6666651533),
            new C2DPoint(0.5177311082, 0.5549593702),
            new C2DPoint(0.5567350631, 0.526595953),
            new C2DPoint(0.6046089702, 0.5319133843),
            new C2DPoint(0.7074448011, 0.6152444004),
            new C2DPoint(0.6985805239, 0.744678738)
        };

        private List<C2DPoint> polygon_101_ET1_C2DPoints = new List<C2DPoint>
        {
            new C2DPoint(0.6105029523, 0.4965778411),
            new C2DPoint(0.6105029523, 0.3566753218),
            new C2DPoint(0.2975934225, 0.3566753218),
            new C2DPoint(0.2975934225, 0.4967174783),
            new C2DPoint(0.0, 0.4967174783),
            new C2DPoint(0.0, 0.0),
            new C2DPoint(1.0, 0.0),
            new C2DPoint(1.0, 0.4979393039),
            new C2DPoint(0.6105029523, 0.4965778411)
        };

        private List<C2DPoint> polygon_101_ET2_C2DPoints = new List<C2DPoint>
        {
            new C2DPoint(1.0, 0.0),
            new C2DPoint(1.0, 0.4076410106),
            new C2DPoint(0.3009, 0.4076410106),
            new C2DPoint(0.300954817, 0.5541427993),
            new C2DPoint(0.047771, 0.5541427),
            new C2DPoint(0.0477711657, 0.4060508864),
            new C2DPoint(0.0, 0.40605),
            new C2DPoint(0.0, 0.0),
            new C2DPoint(1.0, 0.0)
        };

        private List<C2DPoint> polygon_101_ET3_C2DPoints = new List<C2DPoint>
        {
            new C2DPoint(1.0, 0.0669970844),
            new C2DPoint(1.0, 0.3921625343),
            new C2DPoint(0.3284303375, 0.3921625343),
            new C2DPoint(0.3284303375, 0.4411756199),
            new C2DPoint(0.1209150941, 0.4411756199),
            new C2DPoint(0.1209150941, 0.2908533975),
            new C2DPoint(0.0408490588, 0.2908533975),
            new C2DPoint(0.0408490588, 0.1029433884),
            new C2DPoint(0.0, 0.1029433884),
            new C2DPoint(0.0, 0.0),
            new C2DPoint(0.1617641529, 0.0),
            new C2DPoint(0.1617641529, 0.0669970844),
            new C2DPoint(1.0, 0.0669970844)
        };

        private C2DPointSet chart_temp_blue = new C2DPointSet();

        private C2DPointSet chart_temp_yellow = new C2DPointSet();

        private Series temp_series1 = new Series();

        private Series temp_series2 = new Series();

        private CGrid some_grid = new CGrid();

        private C2DPolygon oryginal_polygon = new C2DPolygon();

        private C2DPolygon compared_polygon = new C2DPolygon();

        private C2DPolygon polygon_101_ET1 = new C2DPolygon();

        private C2DPolygon polygon_101_ET2 = new C2DPolygon();

        private C2DPolygon polygon_101_ET3 = new C2DPolygon();

        private List<C2DHoledPolygon> BluePolys = new List<C2DHoledPolygon>();

        private List<C2DHoledPolygon> YellowPolys = new List<C2DHoledPolygon>();

        private List<C2DHoledPolygon> BluePolysNoOverlap = new List<C2DHoledPolygon>();

        private List<C2DHoledPolygon> YellowPolysNoOverlap = new List<C2DHoledPolygon>();

        private List<C2DHoledPolygon> TempPolys = new List<C2DHoledPolygon>();

        private double area;

        private double compared_area;

        private double precision = 0.001;

        private IContainer components;

        private Label label1;

        private Label label2;

        private Button button1;

        private DataGridView dataGridView1;

        private BindingSource form1BindingSource;

        private Panel panel1;

        private Chart chart1;

        private DataSet dataSet1;

        private DataTable dataTable1;

        private Label label3;

        private Label label4;

        private Label label5;

        private Label label6;

        private Label label7;

        private Label label8;

        private ContextMenuStrip contextMenuStrip1;

        private ToolStripMenuItem aboutToolStripMenuItem;

        private ToolStripMenuItem helpToolStripMenuItem;

        private Label label9;

        private Label label10;

        private DataGridViewTextBoxColumn X;

        private DataGridViewTextBoxColumn Y;

        private DataGridViewTextBoxColumn OryginalX;

        private DataGridViewTextBoxColumn OryginalY;

        private GroupBox groupBox1;

        private Label label11;

        private Label label12;

        private Label label13;

        private Label label14;

        private Label label15;

        private Label label16;

        private Label label17;

        private Label label18;

        private Label label19;

        private Label label20;

        private Label label21;

        private Label label24;

        private Label label25;

        private Label label22;

        private Label label23;

        private CheckBox checkBox2;

        private CheckBox checkBox1;

        private GroupBox groupBox2;

        private RadioButton radioButton3;

        private RadioButton radioButton2;

        private RadioButton radioButton1;

        private TrackBar trackBar1;

        private Label label26;

        private Label label27;

        private Button button2;

        private Timer timer1;

        public Form1()
        {
            this.InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.oryginal_polygon_C2DPoints = this.reference_polygon_C2DPoints;
            this.oryginal_polygon = new C2DPolygon(this.oryginal_polygon_C2DPoints, true);
            this.compared_polygon = new C2DPolygon(this.user_polygon_C2DPoints, true);
            this.polygon_101_ET1 = new C2DPolygon(this.polygon_101_ET1_C2DPoints, true);
            this.polygon_101_ET2 = new C2DPolygon(this.polygon_101_ET2_C2DPoints, true);
            this.polygon_101_ET3 = new C2DPolygon(this.polygon_101_ET3_C2DPoints, true);
            this.area = this.oryginal_polygon.GetArea();
            this.fill_grid(this.compared_polygon, 0);
            this.fill_grid(this.oryginal_polygon, 2);
            this.temp_series1 = this.chart1.Series[0];
            this.temp_series2 = this.chart1.Series[1];
        }

        private void fill_grid(C2DPolygon polygon, int cell_index = 0)
        {
            if (this.dataGridView1.Rows.Count <= polygon.Lines.Count)
            {
                this.dataGridView1.Rows.Add(polygon.Lines.Count * 2 - this.dataGridView1.Rows.Count + 1);
            }
            for (int i = 0; i < polygon.Lines.Count; i++)
            {
                this.dataGridView1.Rows[i].Cells[cell_index].Value = polygon.Lines[i].GetPointFrom().x;
                this.dataGridView1.Rows[i].Cells[cell_index + 1].Value = polygon.Lines[i].GetPointFrom().y;
                this.dataGridView1.Rows[i + 1].Cells[cell_index].Value = polygon.Lines[i].GetPointTo().x;
                this.dataGridView1.Rows[i + 1].Cells[cell_index + 1].Value = polygon.Lines[i].GetPointTo().y;
            }
        }

        private void draw_series_chart1(C2DPolygon polygon, string series_name)
        {
            this.chart1.Series[series_name].Points.Clear();
            for (int i = 0; i < polygon.Lines.Count; i++)
            {
                this.chart1.Series[series_name].Points.AddXY(polygon.Lines[i].GetPointFrom().x, polygon.Lines[i].GetPointFrom().y);
                this.chart1.Series[series_name].Points.AddXY(polygon.Lines[i].GetPointTo().x, polygon.Lines[i].GetPointTo().y);
            }
        }

        private void draw_series_chart1(C2DHoledPolyBase holed_polygon, string series_name)
        {
            this.chart1.Series[series_name].Points.Clear();
            C2DPolygon polygon = new C2DPolygon(holed_polygon.Rim);
            this.draw_series_chart1(polygon, series_name);
        }

        private void chart1_add_series(C2DPolygon polygon)
        {
            Series series = new Series();
            for (int i = 0; i < polygon.Lines.Count; i++)
            {
                series.Points.AddXY(polygon.Lines[i].GetPointFrom().x, polygon.Lines[i].GetPointFrom().y);
                series.Points.AddXY(polygon.Lines[i].GetPointTo().x, polygon.Lines[i].GetPointTo().y);
            }
            series.ChartArea = "ChartArea1";
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 5;
            this.chart1.Series.Add(series);
        }

        private void chart1_add_series(C2DHoledPolyBase holed_polygon)
        {
            C2DPolygon polygon = new C2DPolygon(holed_polygon.Rim);
            this.chart1_add_series(polygon);
        }

        private void chart1_draw_holed(List<C2DHoledPolygon> holed_polygons)
        {
            for (int i = 0; i < holed_polygons.Count<C2DHoledPolygon>(); i++)
            {
                this.chart1_add_series(holed_polygons[i]);
            }
        }

        private void read_from_rows(string series_name, int first_row_index = 0)
        {
            this.user_polygon_C2DPoints.Clear();
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    this.user_polygon_C2DPoints.Add(new C2DPoint((double)this.dataGridView1.Rows[i].Cells[first_row_index].Value, (double)this.dataGridView1.Rows[i].Cells[first_row_index + 1].Value));
                }
            }
            this.compared_polygon = new C2DPolygon(this.user_polygon_C2DPoints, true);
        }

        private double calculate(C2DPolygon compared_polygon)
        {
            this.trackBar1.Visible = true;
            this.label26.Visible = true;
            this.label27.Visible = true;
            this.chart1.Series.Clear();
            this.chart1.Series.Add(this.temp_series1);
            this.chart1.Series.Add(this.temp_series2);
            this.read_from_rows("Series2", 0);
            this.some_grid = new CGrid();
            this.compared_polygon.Grow((double)this.trackBar1.Value / 100.0);
            this.draw_series_chart1(this.oryginal_polygon, "Series1");
            if (this.compared_polygon.Lines.Count > 0)
            {
                this.draw_series_chart1(this.compared_polygon, "Series2");
            }
            this.BluePolys = new List<C2DHoledPolygon>();
            this.YellowPolys = new List<C2DHoledPolygon>();
            this.BluePolysNoOverlap = new List<C2DHoledPolygon>();
            this.YellowPolysNoOverlap = new List<C2DHoledPolygon>();
            this.TempPolys = new List<C2DHoledPolygon>();
            this.compared_area = this.compared_polygon.GetArea();
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            this.oryginal_polygon.GetOverlaps(this.compared_polygon, this.BluePolys, this.some_grid);
            this.compared_polygon.GetOverlaps(this.oryginal_polygon, this.YellowPolys, this.some_grid);
            this.oryginal_polygon.GetNonOverlaps(this.compared_polygon, this.BluePolysNoOverlap, this.some_grid);
            this.compared_polygon.GetNonOverlaps(this.oryginal_polygon, this.YellowPolysNoOverlap, this.some_grid);
            foreach (C2DHoledPolygon current in this.BluePolys)
            {
                num += current.GetArea();
            }
            foreach (C2DHoledPolygon current2 in this.YellowPolys)
            {
                num2 += current2.GetArea();
            }
            foreach (C2DHoledPolygon current3 in this.BluePolysNoOverlap)
            {
                num3 += current3.GetArea();
            }
            foreach (C2DHoledPolygon current4 in this.YellowPolysNoOverlap)
            {
                num4 += current4.GetArea();
            }
            double num5 = num3 + num4;
            this.label2.Text = Math.Round(this.area, 3).ToString();
            this.label3.Text = Math.Round(this.compared_area, 3).ToString();
            this.label5.Text = Math.Round(this.area - this.compared_area, 3).ToString();
            this.label12.Text = Math.Round(this.oryginal_polygon.GetPerimeter(), 3).ToString();
            this.label14.Text = Math.Round(this.compared_polygon.GetPerimeter(), 3).ToString();
            this.label7.Text = Math.Round(num, 3).ToString();
            this.label15.Text = Math.Round(num2, 3).ToString();
            this.label17.Text = Math.Round(num3, 3).ToString();
            this.label9.Text = Math.Round(num4, 3).ToString();
            this.label22.Text = Math.Round(num + num2, 3).ToString();
            this.label24.Text = Math.Round(num5, 3).ToString();
            if (this.compared_area != 0.0)
            {
                this.label19.Text = Math.Round((this.area - num5) / this.area * 100.0, 5).ToString();
            }
            else
            {
                MessageBox.Show("Figure area can not be zero.");
            }
            this.temp_series1 = this.chart1.Series[0];
            this.temp_series2 = this.chart1.Series[1];
            this.checkBox1.Checked = true;
            this.checkBox2.Checked = false;
            this.checkBox1.Show();
            this.checkBox2.Show();
            return this.compared_area;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.calculate(this.compared_polygon);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This software was written by Piotr Falkowski, all rights reserved.\nFor licensing and questions write: piotr.falkowski.fm@gmail.com. Build in Microsoft (R) Visual Studio(R) Express 13 using .NET 4.5 and GeoLib geometric library: www.geolib.co.uk.");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Note, that the number of cells every pair of columns (odd and even)  must be equal.\nThe odd column represents X value while even the Y value. It is also crucial, that any two points from different poits-sets would NOT be equal (coincidental).");
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }

        private void update_chart()
        {
            this.chart1.Series.Clear();
            if (this.checkBox1.Checked)
            {
                this.chart1.Series.Add(this.temp_series1);
                this.chart1.Series.Add(this.temp_series2);
            }
            if (this.checkBox2.Checked)
            {
                this.chart1_draw_holed(this.BluePolysNoOverlap);
                this.chart1_draw_holed(this.YellowPolysNoOverlap);
            }
        }

        private void update_figures_grid()
        {
            this.dataGridView1.Rows.Clear();
            this.fill_grid(this.compared_polygon, 0);
            this.fill_grid(this.oryginal_polygon, 2);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.update_chart();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.update_chart();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.compared_polygon = this.polygon_101_ET1;
                this.update_figures_grid();
                this.calculate(this.compared_polygon);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.compared_polygon = this.polygon_101_ET2;
            this.update_figures_grid();
            this.calculate(this.compared_polygon);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.compared_polygon = this.polygon_101_ET3;
            this.update_figures_grid();
            this.calculate(this.compared_polygon);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.calculate(this.compared_polygon);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.trackBar1.Value = 100;
            this.compared_area = this.compared_polygon.GetArea();
            bool flag = false;
            bool flag2 = false;
            while (this.compared_area > this.area * (1.0 + this.precision) || this.compared_area < this.area * (1.0 - this.precision))
            {
                if (this.compared_area > this.area * (1.0 + this.precision))
                {
                    this.trackBar1.Value--;
                    this.calculate(this.compared_polygon);
                    flag = true;
                }
                else if (this.compared_area < this.area * (1.0 - this.precision))
                {
                    this.trackBar1.Value++;
                    this.calculate(this.compared_polygon);
                    flag2 = true;
                }
                if (flag && flag2)
                {
                    break;
                }
            }
            this.compared_polygon = this.move_to_zero_point(this.compared_polygon);
        }

        private C2DPolygon move_to_zero_point(C2DPolygon polygon)
        {
            List<C2DPoint> list = new List<C2DPoint>();
            polygon.GetPointsCopy(list);
            double num = list.Min((C2DPoint Item) => Item.x);
            double num2 = list.Min((C2DPoint Item) => Item.y);
            foreach (C2DPoint current in list.ToList<C2DPoint>())
            {
                current.x -= num;
                current.y -= num2;
            }
            return new C2DPolygon(list, false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ChartArea chartArea = new ChartArea();
            Legend legend = new Legend();
            Series series = new Series();
            Series series2 = new Series();
            this.label1 = new Label();
            this.label2 = new Label();
            this.button1 = new Button();
            this.dataGridView1 = new DataGridView();
            this.X = new DataGridViewTextBoxColumn();
            this.Y = new DataGridViewTextBoxColumn();
            this.OryginalX = new DataGridViewTextBoxColumn();
            this.OryginalY = new DataGridViewTextBoxColumn();
            this.panel1 = new Panel();
            this.groupBox2 = new GroupBox();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.groupBox1 = new GroupBox();
            this.label24 = new Label();
            this.label25 = new Label();
            this.label22 = new Label();
            this.label23 = new Label();
            this.label21 = new Label();
            this.label19 = new Label();
            this.label20 = new Label();
            this.label17 = new Label();
            this.label18 = new Label();
            this.label15 = new Label();
            this.label16 = new Label();
            this.label11 = new Label();
            this.label12 = new Label();
            this.label13 = new Label();
            this.label14 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label9 = new Label();
            this.label6 = new Label();
            this.label10 = new Label();
            this.label5 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.chart1 = new Chart();
            this.dataSet1 = new DataSet();
            this.dataTable1 = new DataTable();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.checkBox2 = new CheckBox();
            this.checkBox1 = new CheckBox();
            this.trackBar1 = new TrackBar();
            this.label26 = new Label();
            this.label27 = new Label();
            this.button2 = new Button();
            this.timer1 = new Timer(this.components);
            this.form1BindingSource = new BindingSource(this.components);
            ((ISupportInitialize)this.dataGridView1).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize)this.chart1).BeginInit();
            ((ISupportInitialize)this.dataSet1).BeginInit();
            ((ISupportInitialize)this.dataTable1).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((ISupportInitialize)this.trackBar1).BeginInit();
            ((ISupportInitialize)this.form1BindingSource).BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label1.ForeColor = Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Blue Figure Area =";
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label2.Location = new System.Drawing.Point(129, 16);
            this.label2.Name = "label2";
            this.label2.Size = new Size(14, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "0";
            this.button1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.button1.Location = new System.Drawing.Point(874, 523);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Calculate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.dataGridView1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[]
            {
                this.X,
                this.Y,
                this.OryginalX,
                this.OryginalY
            });
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new Size(352, 483);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.X.FillWeight = 48.89976f;
            this.X.HeaderText = "Your X";
            this.X.Name = "X";
            this.X.Width = 64;
            this.Y.FillWeight = 151.1003f;
            this.Y.HeaderText = "Your Y";
            this.Y.Name = "Y";
            this.Y.Width = 64;
            this.OryginalX.HeaderText = "Oryginal X";
            this.OryginalX.Name = "OryginalX";
            this.OryginalX.Width = 80;
            this.OryginalY.HeaderText = "Oryginal Y";
            this.OryginalY.Name = "OryginalY";
            this.OryginalY.Width = 80;
            this.panel1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(15, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(934, 483);
            this.panel1.TabIndex = 5;
            this.groupBox2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Location = new System.Drawing.Point(386, 280);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(230, 203);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load";
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 65);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(69, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "101_ET3";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(69, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "101_ET2";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(69, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "101_ET1";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.groupBox1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.groupBox1.BackColor = Color.Transparent;
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(614, 280);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(320, 203);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.label24.AutoSize = true;
            this.label24.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label24.Location = new System.Drawing.Point(205, 146);
            this.label24.Name = "label24";
            this.label24.Size = new Size(14, 13);
            this.label24.TabIndex = 29;
            this.label24.Text = "0";
            this.label25.AutoSize = true;
            this.label25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label25.ForeColor = SystemColors.ControlText;
            this.label25.Location = new System.Drawing.Point(8, 146);
            this.label25.Name = "label25";
            this.label25.Size = new Size(196, 13);
            this.label25.TabIndex = 28;
            this.label25.Text = "Non-overlapping areas raw sum =";
            this.label22.AutoSize = true;
            this.label22.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label22.Location = new System.Drawing.Point(158, 133);
            this.label22.Name = "label22";
            this.label22.Size = new Size(14, 13);
            this.label22.TabIndex = 27;
            this.label22.Text = "0";
            this.label23.AutoSize = true;
            this.label23.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            this.label23.ForeColor = Color.FromArgb(64, 64, 64);
            this.label23.Location = new System.Drawing.Point(8, 133);
            this.label23.Name = "label23";
            this.label23.Size = new Size(144, 13);
            this.label23.TabIndex = 26;
            this.label23.Text = "Overlapping areas raw sum =";
            this.label21.AutoSize = true;
            this.label21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label21.Location = new System.Drawing.Point(295, 172);
            this.label21.Name = "label21";
            this.label21.Size = new Size(16, 13);
            this.label21.TabIndex = 25;
            this.label21.Text = "%";
            this.label19.AutoSize = true;
            this.label19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label19.Location = new System.Drawing.Point(245, 172);
            this.label19.Name = "label19";
            this.label19.Size = new Size(14, 13);
            this.label19.TabIndex = 24;
            this.label19.Text = "0";
            this.label20.AutoSize = true;
            this.label20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label20.ForeColor = Color.DarkRed;
            this.label20.Location = new System.Drawing.Point(152, 172);
            this.label20.Name = "label20";
            this.label20.Size = new Size(87, 13);
            this.label20.TabIndex = 23;
            this.label20.Text = "Match index =";
            this.label17.AutoSize = true;
            this.label17.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label17.Location = new System.Drawing.Point(152, 107);
            this.label17.Name = "label17";
            this.label17.Size = new Size(14, 13);
            this.label17.TabIndex = 22;
            this.label17.Text = "0";
            this.label18.AutoSize = true;
            this.label18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            this.label18.ForeColor = Color.FromArgb(64, 64, 64);
            this.label18.Location = new System.Drawing.Point(8, 107);
            this.label18.Name = "label18";
            this.label18.Size = new Size(140, 13);
            this.label18.TabIndex = 21;
            this.label18.Text = "Blue non-overlapping area =";
            this.label15.AutoSize = true;
            this.label15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label15.Location = new System.Drawing.Point(142, 94);
            this.label15.Name = "label15";
            this.label15.Size = new Size(14, 13);
            this.label15.TabIndex = 20;
            this.label15.Text = "0";
            this.label16.AutoSize = true;
            this.label16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            this.label16.ForeColor = Color.FromArgb(64, 64, 64);
            this.label16.Location = new System.Drawing.Point(8, 94);
            this.label16.Name = "label16";
            this.label16.Size = new Size(129, 13);
            this.label16.TabIndex = 19;
            this.label16.Text = "Yellow overlapping area =";
            this.label11.AutoSize = true;
            this.label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label11.ForeColor = Color.RoyalBlue;
            this.label11.Location = new System.Drawing.Point(8, 42);
            this.label11.Name = "label11";
            this.label11.Size = new Size(139, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Blue Figure Perimeter =";
            this.label12.AutoSize = true;
            this.label12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label12.Location = new System.Drawing.Point(151, 42);
            this.label12.Name = "label12";
            this.label12.Size = new Size(14, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "0";
            this.label13.AutoSize = true;
            this.label13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label13.ForeColor = Color.DarkGoldenrod;
            this.label13.Location = new System.Drawing.Point(8, 55);
            this.label13.Name = "label13";
            this.label13.Size = new Size(148, 13);
            this.label13.TabIndex = 17;
            this.label13.Text = "Yellow figure Perimeter =";
            this.label14.AutoSize = true;
            this.label14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label14.Location = new System.Drawing.Point(158, 55);
            this.label14.Name = "label14";
            this.label14.Size = new Size(14, 13);
            this.label14.TabIndex = 18;
            this.label14.Text = "0";
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label4.ForeColor = Color.DarkGoldenrod;
            this.label4.Location = new System.Drawing.Point(8, 29);
            this.label4.Name = "label4";
            this.label4.Size = new Size(121, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Yellow figure Area =";
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label3.Location = new System.Drawing.Point(136, 29);
            this.label3.Name = "label3";
            this.label3.Size = new Size(14, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "0";
            this.label9.AutoSize = true;
            this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label9.Location = new System.Drawing.Point(164, 120);
            this.label9.Name = "label9";
            this.label9.Size = new Size(14, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "0";
            this.label6.AutoSize = true;
            this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            this.label6.ForeColor = Color.FromArgb(64, 64, 64);
            this.label6.Location = new System.Drawing.Point(8, 68);
            this.label6.Name = "label6";
            this.label6.Size = new Size(138, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Difference between areas =";
            this.label10.AutoSize = true;
            this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            this.label10.ForeColor = Color.FromArgb(64, 64, 64);
            this.label10.Location = new System.Drawing.Point(8, 120);
            this.label10.Name = "label10";
            this.label10.Size = new Size(150, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Yellow non-overlapping area =";
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label5.Location = new System.Drawing.Point(149, 68);
            this.label5.Name = "label5";
            this.label5.Size = new Size(14, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "0";
            this.label7.AutoSize = true;
            this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label7.Location = new System.Drawing.Point(129, 81);
            this.label7.Name = "label7";
            this.label7.Size = new Size(14, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "0";
            this.label8.AutoSize = true;
            this.label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            this.label8.ForeColor = Color.FromArgb(64, 64, 64);
            this.label8.Location = new System.Drawing.Point(8, 81);
            this.label8.Name = "label8";
            this.label8.Size = new Size(119, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Blue overlapping area =";
            this.chart1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            chartArea.AlignmentOrientation = AreaAlignmentOrientations.All;
            chartArea.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea);
            legend.Name = "Legend1";
            this.chart1.Legends.Add(legend);
            this.chart1.Location = new System.Drawing.Point(358, 0);
            this.chart1.Name = "chart1";
            series.BorderWidth = 9;
            series.ChartArea = "ChartArea1";
            series.ChartType = SeriesChartType.Line;
            series.Legend = "Legend1";
            series.Name = "Series1";
            series2.BorderWidth = 9;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            this.chart1.Series.Add(series);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new Size(576, 255);
            this.chart1.TabIndex = 4;
            this.chart1.Text = "chart1";
            this.dataSet1.DataSetName = "NewDataSet";
            this.dataSet1.Tables.AddRange(new DataTable[]
            {
                this.dataTable1
            });
            this.dataTable1.TableName = "Table1";
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[]
            {
                this.aboutToolStripMenuItem,
                this.helpToolStripMenuItem
            });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(108, 48);
            this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new Size(107, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new EventHandler(this.helpToolStripMenuItem_Click);
            this.checkBox2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(720, 542);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(132, 17);
            this.checkBox2.TabIndex = 24;
            this.checkBox2.Text = "Show difference areas";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.Visible = false;
            this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.checkBox1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(720, 521);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(87, 17);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "Show figures";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.trackBar1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.trackBar1.Location = new System.Drawing.Point(60, 514);
            this.trackBar1.Maximum = 200;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new Size(307, 45);
            this.trackBar1.TabIndex = 31;
            this.trackBar1.Value = 100;
            this.trackBar1.Visible = false;
            this.trackBar1.Scroll += new EventHandler(this.trackBar1_Scroll);
            this.label26.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.label26.AutoSize = true;
            this.label26.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label26.Location = new System.Drawing.Point(373, 528);
            this.label26.Name = "label26";
            this.label26.Size = new Size(36, 13);
            this.label26.TabIndex = 32;
            this.label26.Text = "Grow";
            this.label26.Visible = false;
            this.label27.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.label27.AutoSize = true;
            this.label27.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            this.label27.Location = new System.Drawing.Point(11, 528);
            this.label27.Name = "label27";
            this.label27.Size = new Size(43, 13);
            this.label27.TabIndex = 33;
            this.label27.Text = "Shrink";
            this.label27.Visible = false;
            this.button2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.button2.Location = new System.Drawing.Point(415, 523);
            this.button2.Name = "button2";
            this.button2.Size = new Size(91, 23);
            this.button2.TabIndex = 34;
            this.button2.Text = "Scale To Area";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.timer1.Interval = 50;
            this.form1BindingSource.DataSource = typeof(Form1);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(961, 558);
            this.ContextMenuStrip = this.contextMenuStrip1;
            base.Controls.Add(this.button2);
            base.Controls.Add(this.label27);
            base.Controls.Add(this.label26);
            base.Controls.Add(this.trackBar1);
            base.Controls.Add(this.checkBox2);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.button1);
            base.Name = "Form1";
            this.Text = "5";
            base.Load += new EventHandler(this.Form1_Load);
            ((ISupportInitialize)this.dataGridView1).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((ISupportInitialize)this.chart1).EndInit();
            ((ISupportInitialize)this.dataSet1).EndInit();
            ((ISupportInitialize)this.dataTable1).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((ISupportInitialize)this.trackBar1).EndInit();
            ((ISupportInitialize)this.form1BindingSource).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
