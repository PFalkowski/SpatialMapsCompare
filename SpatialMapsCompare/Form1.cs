using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GeoLib;
using SpatialMapsCompare.Properties;

namespace SpatialMapsCompare
{
    public class Form1 : Form
    {
        private List<C2DPoint> _referencePolygonC2DPoints = new List<C2DPoint>
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

        private List<C2DPoint> _oryginalPolygonC2DPoints = new List<C2DPoint>
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

        private List<C2DPoint> _userPolygonC2DPoints = new List<C2DPoint>
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

        private List<C2DPoint> _polygon101Et1C2DPoints = new List<C2DPoint>
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

        private List<C2DPoint> _polygon101Et2C2DPoints = new List<C2DPoint>
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

        private List<C2DPoint> _polygon101Et3C2DPoints = new List<C2DPoint>
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

        private C2DPointSet _chartTempBlue = new C2DPointSet();

        private C2DPointSet _chartTempYellow = new C2DPointSet();

        private Series _tempSeries1 = new Series();

        private Series _tempSeries2 = new Series();

        private CGrid _someGrid = new CGrid();

        private C2DPolygon _oryginalPolygon = new C2DPolygon();

        private C2DPolygon _comparedPolygon = new C2DPolygon();

        private C2DPolygon _polygon101Et1 = new C2DPolygon();

        private C2DPolygon _polygon101Et2 = new C2DPolygon();

        private C2DPolygon _polygon101Et3 = new C2DPolygon();

        private List<C2DHoledPolygon> _bluePolys = new List<C2DHoledPolygon>();

        private List<C2DHoledPolygon> _yellowPolys = new List<C2DHoledPolygon>();

        private List<C2DHoledPolygon> _bluePolysNoOverlap = new List<C2DHoledPolygon>();

        private List<C2DHoledPolygon> _yellowPolysNoOverlap = new List<C2DHoledPolygon>();

        private List<C2DHoledPolygon> _tempPolys = new List<C2DHoledPolygon>();

        private double _area;

        private double _comparedArea;

        private double _precision = 0.001;

        private IContainer components;

        private Label _label1;

        private Label _label2;

        private Button _button1;

        private DataGridView _dataGridView1;

        private BindingSource _form1BindingSource;

        private Panel _panel1;

        private Chart _chart1;

        private DataSet _dataSet1;

        private DataTable _dataTable1;

        private Label _label3;

        private Label _label4;

        private Label _label5;

        private Label _label6;

        private Label _label7;

        private Label _label8;

        private ContextMenuStrip _contextMenuStrip1;

        private ToolStripMenuItem _aboutToolStripMenuItem;

        private ToolStripMenuItem _helpToolStripMenuItem;

        private Label _label9;

        private Label _label10;

        private DataGridViewTextBoxColumn _x;

        private DataGridViewTextBoxColumn _y;

        private DataGridViewTextBoxColumn _oryginalX;

        private DataGridViewTextBoxColumn _oryginalY;

        private GroupBox _groupBox1;

        private Label _label11;

        private Label _label12;

        private Label _label13;

        private Label _label14;

        private Label _label15;

        private Label _label16;

        private Label _label17;

        private Label _label18;

        private Label _label19;

        private Label _label20;

        private Label _label21;

        private Label _label24;

        private Label _label25;

        private Label _label22;

        private Label _label23;

        private CheckBox _checkBox2;

        private CheckBox _checkBox1;

        private GroupBox _groupBox2;

        private RadioButton _radioButton3;

        private RadioButton _radioButton2;

        private RadioButton _radioButton1;

        private TrackBar _trackBar1;

        private Label _label26;

        private Label _label27;

        private Button _button2;

        private Timer _timer1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _oryginalPolygonC2DPoints = _referencePolygonC2DPoints;
            _oryginalPolygon = new C2DPolygon(_oryginalPolygonC2DPoints, true);
            _comparedPolygon = new C2DPolygon(_userPolygonC2DPoints, true);
            _polygon101Et1 = new C2DPolygon(_polygon101Et1C2DPoints, true);
            _polygon101Et2 = new C2DPolygon(_polygon101Et2C2DPoints, true);
            _polygon101Et3 = new C2DPolygon(_polygon101Et3C2DPoints, true);
            _area = _oryginalPolygon.GetArea();
            fill_grid(_comparedPolygon, 0);
            fill_grid(_oryginalPolygon, 2);
            _tempSeries1 = _chart1.Series[0];
            _tempSeries2 = _chart1.Series[1];
        }

        private void fill_grid(C2DPolygon polygon, int cellIndex = 0)
        {
            if (_dataGridView1.Rows.Count <= polygon.Lines.Count)
            {
                _dataGridView1.Rows.Add(polygon.Lines.Count * 2 - _dataGridView1.Rows.Count + 1);
            }
            for (int i = 0; i < polygon.Lines.Count; i++)
            {
                _dataGridView1.Rows[i].Cells[cellIndex].Value = polygon.Lines[i].GetPointFrom().x;
                _dataGridView1.Rows[i].Cells[cellIndex + 1].Value = polygon.Lines[i].GetPointFrom().y;
                _dataGridView1.Rows[i + 1].Cells[cellIndex].Value = polygon.Lines[i].GetPointTo().x;
                _dataGridView1.Rows[i + 1].Cells[cellIndex + 1].Value = polygon.Lines[i].GetPointTo().y;
            }
        }

        private void draw_series_chart1(C2DPolygon polygon, string seriesName)
        {
            _chart1.Series[seriesName].Points.Clear();
            foreach (var t in polygon.Lines)
            {
                _chart1.Series[seriesName].Points.AddXY(t.GetPointFrom().x, t.GetPointFrom().y);
                _chart1.Series[seriesName].Points.AddXY(t.GetPointTo().x, t.GetPointTo().y);
            }
        }

        private void draw_series_chart1(C2DHoledPolyBase holedPolygon, string seriesName)
        {
            _chart1.Series[seriesName].Points.Clear();
            C2DPolygon polygon = new C2DPolygon(holedPolygon.Rim);
            draw_series_chart1(polygon, seriesName);
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
            _chart1.Series.Add(series);
        }

        private void chart1_add_series(C2DHoledPolyBase holedPolygon)
        {
            C2DPolygon polygon = new C2DPolygon(holedPolygon.Rim);
            chart1_add_series(polygon);
        }

        private void chart1_draw_holed(List<C2DHoledPolygon> holedPolygons)
        {
            for (int i = 0; i < holedPolygons.Count<C2DHoledPolygon>(); i++)
            {
                chart1_add_series(holedPolygons[i]);
            }
        }

        private void read_from_rows(string seriesName, int firstRowIndex = 0)
        {
            _userPolygonC2DPoints.Clear();
            for (int i = 0; i < _dataGridView1.Rows.Count; i++)
            {
                if (_dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    _userPolygonC2DPoints.Add(new C2DPoint((double)_dataGridView1.Rows[i].Cells[firstRowIndex].Value, (double)_dataGridView1.Rows[i].Cells[firstRowIndex + 1].Value));
                }
            }
            _comparedPolygon = new C2DPolygon(_userPolygonC2DPoints, true);
        }

        private void Calculate(C2DPolygon comparedPolygon)
        {
            _trackBar1.Visible = true;
            _label26.Visible = true;
            _label27.Visible = true;
            _chart1.Series.Clear();
            _chart1.Series.Add(_tempSeries1);
            _chart1.Series.Add(_tempSeries2);
            read_from_rows("Series2", 0);
            _someGrid = new CGrid();
            _comparedPolygon.Grow((double)_trackBar1.Value / 100.0);
            draw_series_chart1(_oryginalPolygon, "Series1");
            if (_comparedPolygon.Lines.Count > 0)
            {
                draw_series_chart1(_comparedPolygon, "Series2");
            }
            _bluePolys = new List<C2DHoledPolygon>();
            _yellowPolys = new List<C2DHoledPolygon>();
            _bluePolysNoOverlap = new List<C2DHoledPolygon>();
            _yellowPolysNoOverlap = new List<C2DHoledPolygon>();
            _tempPolys = new List<C2DHoledPolygon>();
            _comparedArea = _comparedPolygon.GetArea();
            _oryginalPolygon.GetOverlaps(_comparedPolygon, _bluePolys, _someGrid);
            _comparedPolygon.GetOverlaps(_oryginalPolygon, _yellowPolys, _someGrid);
            _oryginalPolygon.GetNonOverlaps(_comparedPolygon, _bluePolysNoOverlap, _someGrid);
            _comparedPolygon.GetNonOverlaps(_oryginalPolygon, _yellowPolysNoOverlap, _someGrid);
            double num = _bluePolys.Sum(current => current.GetArea());
            double num2 = _yellowPolys.Sum(current2 => current2.GetArea());
            double num3 = _bluePolysNoOverlap.Sum(current3 => current3.GetArea());
            double num4 = _yellowPolysNoOverlap.Sum(current4 => current4.GetArea());
            double num5 = num3 + num4;
            _label2.Text = Math.Round(_area, 3).ToString(CultureInfo.InvariantCulture);
            _label3.Text = Math.Round(_comparedArea, 3).ToString(CultureInfo.InvariantCulture);
            _label5.Text = Math.Round(_area - _comparedArea, 3).ToString(CultureInfo.InvariantCulture);
            _label12.Text = Math.Round(_oryginalPolygon.GetPerimeter(), 3).ToString(CultureInfo.InvariantCulture);
            _label14.Text = Math.Round(_comparedPolygon.GetPerimeter(), 3).ToString(CultureInfo.InvariantCulture);
            _label7.Text = Math.Round(num, 3).ToString(CultureInfo.InvariantCulture);
            _label15.Text = Math.Round(num2, 3).ToString(CultureInfo.InvariantCulture);
            _label17.Text = Math.Round(num3, 3).ToString(CultureInfo.InvariantCulture);
            _label9.Text = Math.Round(num4, 3).ToString(CultureInfo.InvariantCulture);
            _label22.Text = Math.Round(num + num2, 3).ToString(CultureInfo.InvariantCulture);
            _label24.Text = Math.Round(num5, 3).ToString(CultureInfo.InvariantCulture);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_comparedArea != 0.0)
            {
                _label19.Text = Math.Round((_area - num5) / _area * 100.0, 5).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                MessageBox.Show(Resources.Form1_Calculate_Figure_area_can_not_be_zero_);
            }
            _tempSeries1 = _chart1.Series[0];
            _tempSeries2 = _chart1.Series[1];
            _checkBox1.Checked = true;
            _checkBox2.Checked = false;
            _checkBox1.Show();
            _checkBox2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Calculate(_comparedPolygon);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.Form1_aboutToolStripMenuItem_Click_);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.Form1_helpToolStripMenuItem_Click_);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }

        private void update_chart()
        {
            _chart1.Series.Clear();
            if (_checkBox1.Checked)
            {
                _chart1.Series.Add(_tempSeries1);
                _chart1.Series.Add(_tempSeries2);
            }
            if (_checkBox2.Checked)
            {
                chart1_draw_holed(_bluePolysNoOverlap);
                chart1_draw_holed(_yellowPolysNoOverlap);
            }
        }

        private void update_figures_grid()
        {
            _dataGridView1.Rows.Clear();
            fill_grid(_comparedPolygon, 0);
            fill_grid(_oryginalPolygon, 2);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            update_chart();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            update_chart();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (_radioButton1.Checked)
            {
                _comparedPolygon = _polygon101Et1;
                update_figures_grid();
                Calculate(_comparedPolygon);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _comparedPolygon = _polygon101Et2;
            update_figures_grid();
            Calculate(_comparedPolygon);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            _comparedPolygon = _polygon101Et3;
            update_figures_grid();
            Calculate(_comparedPolygon);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Calculate(_comparedPolygon);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _trackBar1.Value = 100;
            _comparedArea = _comparedPolygon.GetArea();
            bool flag = false;
            bool flag2 = false;
            while (_comparedArea > _area * (1.0 + _precision) || _comparedArea < _area * (1.0 - _precision))
            {
                if (_comparedArea > _area * (1.0 + _precision))
                {
                    _trackBar1.Value--;
                    Calculate(_comparedPolygon);
                    flag = true;
                }
                else if (_comparedArea < _area * (1.0 - _precision))
                {
                    _trackBar1.Value++;
                    Calculate(_comparedPolygon);
                    flag2 = true;
                }
                if (flag && flag2)
                {
                    break;
                }
            }
            _comparedPolygon = move_to_zero_point(_comparedPolygon);
        }

        private C2DPolygon move_to_zero_point(C2DPolygon polygon)
        {
            List<C2DPoint> list = new List<C2DPoint>();
            polygon.GetPointsCopy(list);
            double num = list.Min((C2DPoint item) => item.x);
            double num2 = list.Min((C2DPoint item) => item.y);
            foreach (C2DPoint current in list.ToList<C2DPoint>())
            {
                current.x -= num;
                current.y -= num2;
            }
            return new C2DPolygon(list, false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            ChartArea chartArea = new ChartArea();
            Legend legend = new Legend();
            Series series = new Series();
            Series series2 = new Series();
            _label1 = new Label();
            _label2 = new Label();
            _button1 = new Button();
            _dataGridView1 = new DataGridView();
            _x = new DataGridViewTextBoxColumn();
            _y = new DataGridViewTextBoxColumn();
            _oryginalX = new DataGridViewTextBoxColumn();
            _oryginalY = new DataGridViewTextBoxColumn();
            _panel1 = new Panel();
            _groupBox2 = new GroupBox();
            _radioButton3 = new RadioButton();
            _radioButton2 = new RadioButton();
            _radioButton1 = new RadioButton();
            _groupBox1 = new GroupBox();
            _label24 = new Label();
            _label25 = new Label();
            _label22 = new Label();
            _label23 = new Label();
            _label21 = new Label();
            _label19 = new Label();
            _label20 = new Label();
            _label17 = new Label();
            _label18 = new Label();
            _label15 = new Label();
            _label16 = new Label();
            _label11 = new Label();
            _label12 = new Label();
            _label13 = new Label();
            _label14 = new Label();
            _label4 = new Label();
            _label3 = new Label();
            _label9 = new Label();
            _label6 = new Label();
            _label10 = new Label();
            _label5 = new Label();
            _label7 = new Label();
            _label8 = new Label();
            _chart1 = new Chart();
            _dataSet1 = new DataSet();
            _dataTable1 = new DataTable();
            _contextMenuStrip1 = new ContextMenuStrip(components);
            _aboutToolStripMenuItem = new ToolStripMenuItem();
            _helpToolStripMenuItem = new ToolStripMenuItem();
            _checkBox2 = new CheckBox();
            _checkBox1 = new CheckBox();
            _trackBar1 = new TrackBar();
            _label26 = new Label();
            _label27 = new Label();
            _button2 = new Button();
            _timer1 = new Timer(components);
            _form1BindingSource = new BindingSource(components);
            ((ISupportInitialize)_dataGridView1).BeginInit();
            _panel1.SuspendLayout();
            _groupBox2.SuspendLayout();
            _groupBox1.SuspendLayout();
            ((ISupportInitialize)_chart1).BeginInit();
            ((ISupportInitialize)_dataSet1).BeginInit();
            ((ISupportInitialize)_dataTable1).BeginInit();
            _contextMenuStrip1.SuspendLayout();
            ((ISupportInitialize)_trackBar1).BeginInit();
            ((ISupportInitialize)_form1BindingSource).BeginInit();
            SuspendLayout();
            _label1.AutoSize = true;
            _label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label1.ForeColor = Color.RoyalBlue;
            _label1.Location = new Point(8, 16);
            _label1.Name = "_label1";
            _label1.Size = new Size(112, 13);
            _label1.TabIndex = 0;
            _label1.Text = "Blue Figure Area =";
            _label2.AutoSize = true;
            _label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label2.Location = new Point(129, 16);
            _label2.Name = "_label2";
            _label2.Size = new Size(14, 13);
            _label2.TabIndex = 1;
            _label2.Text = "0";
            _button1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            _button1.Location = new Point(874, 523);
            _button1.Name = "_button1";
            _button1.Size = new Size(75, 23);
            _button1.TabIndex = 2;
            _button1.Text = "Calculate";
            _button1.UseVisualStyleBackColor = true;
            _button1.Click += new EventHandler(button1_Click);
            _dataGridView1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
            _dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            _dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dataGridView1.Columns.AddRange(new DataGridViewColumn[]
            {
                _x,
                _y,
                _oryginalX,
                _oryginalY
            });
            _dataGridView1.Location = new Point(0, 0);
            _dataGridView1.Name = "_dataGridView1";
            _dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            _dataGridView1.Size = new Size(352, 483);
            _dataGridView1.TabIndex = 3;
            _dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
            _x.FillWeight = 48.89976f;
            _x.HeaderText = "Your X";
            _x.Name = "_x";
            _x.Width = 64;
            _y.FillWeight = 151.1003f;
            _y.HeaderText = "Your Y";
            _y.Name = "_y";
            _y.Width = 64;
            _oryginalX.HeaderText = "Oryginal X";
            _oryginalX.Name = "_oryginalX";
            _oryginalX.Width = 80;
            _oryginalY.HeaderText = "Oryginal Y";
            _oryginalY.Name = "_oryginalY";
            _oryginalY.Width = 80;
            _panel1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            _panel1.Controls.Add(_groupBox2);
            _panel1.Controls.Add(_groupBox1);
            _panel1.Controls.Add(_chart1);
            _panel1.Controls.Add(_dataGridView1);
            _panel1.Location = new Point(15, 25);
            _panel1.Name = "_panel1";
            _panel1.Size = new Size(934, 483);
            _panel1.TabIndex = 5;
            _groupBox2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            _groupBox2.Controls.Add(_radioButton3);
            _groupBox2.Controls.Add(_radioButton2);
            _groupBox2.Controls.Add(_radioButton1);
            _groupBox2.Location = new Point(386, 280);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new Size(230, 203);
            _groupBox2.TabIndex = 30;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Load";
            _radioButton3.AutoSize = true;
            _radioButton3.Location = new Point(6, 65);
            _radioButton3.Name = "_radioButton3";
            _radioButton3.Size = new Size(69, 17);
            _radioButton3.TabIndex = 2;
            _radioButton3.TabStop = true;
            _radioButton3.Text = "101_ET3";
            _radioButton3.UseVisualStyleBackColor = true;
            _radioButton3.CheckedChanged += new EventHandler(radioButton3_CheckedChanged);
            _radioButton2.AutoSize = true;
            _radioButton2.Location = new Point(6, 42);
            _radioButton2.Name = "_radioButton2";
            _radioButton2.Size = new Size(69, 17);
            _radioButton2.TabIndex = 1;
            _radioButton2.TabStop = true;
            _radioButton2.Text = "101_ET2";
            _radioButton2.UseVisualStyleBackColor = true;
            _radioButton2.CheckedChanged += new EventHandler(radioButton2_CheckedChanged);
            _radioButton1.AutoSize = true;
            _radioButton1.Location = new Point(6, 19);
            _radioButton1.Name = "_radioButton1";
            _radioButton1.Size = new Size(69, 17);
            _radioButton1.TabIndex = 0;
            _radioButton1.TabStop = true;
            _radioButton1.Text = "101_ET1";
            _radioButton1.UseVisualStyleBackColor = true;
            _radioButton1.CheckedChanged += new EventHandler(radioButton1_CheckedChanged);
            _groupBox1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            _groupBox1.BackColor = Color.Transparent;
            _groupBox1.Controls.Add(_label24);
            _groupBox1.Controls.Add(_label25);
            _groupBox1.Controls.Add(_label22);
            _groupBox1.Controls.Add(_label23);
            _groupBox1.Controls.Add(_label21);
            _groupBox1.Controls.Add(_label19);
            _groupBox1.Controls.Add(_label20);
            _groupBox1.Controls.Add(_label17);
            _groupBox1.Controls.Add(_label18);
            _groupBox1.Controls.Add(_label15);
            _groupBox1.Controls.Add(_label16);
            _groupBox1.Controls.Add(_label11);
            _groupBox1.Controls.Add(_label12);
            _groupBox1.Controls.Add(_label13);
            _groupBox1.Controls.Add(_label14);
            _groupBox1.Controls.Add(_label1);
            _groupBox1.Controls.Add(_label2);
            _groupBox1.Controls.Add(_label4);
            _groupBox1.Controls.Add(_label3);
            _groupBox1.Controls.Add(_label9);
            _groupBox1.Controls.Add(_label6);
            _groupBox1.Controls.Add(_label10);
            _groupBox1.Controls.Add(_label5);
            _groupBox1.Controls.Add(_label7);
            _groupBox1.Controls.Add(_label8);
            _groupBox1.Location = new Point(614, 280);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new Size(320, 203);
            _groupBox1.TabIndex = 18;
            _groupBox1.TabStop = false;
            _label24.AutoSize = true;
            _label24.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label24.Location = new Point(205, 146);
            _label24.Name = "_label24";
            _label24.Size = new Size(14, 13);
            _label24.TabIndex = 29;
            _label24.Text = "0";
            _label25.AutoSize = true;
            _label25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label25.ForeColor = SystemColors.ControlText;
            _label25.Location = new Point(8, 146);
            _label25.Name = "_label25";
            _label25.Size = new Size(196, 13);
            _label25.TabIndex = 28;
            _label25.Text = "Non-overlapping areas raw sum =";
            _label22.AutoSize = true;
            _label22.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label22.Location = new Point(158, 133);
            _label22.Name = "_label22";
            _label22.Size = new Size(14, 13);
            _label22.TabIndex = 27;
            _label22.Text = "0";
            _label23.AutoSize = true;
            _label23.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            _label23.ForeColor = Color.FromArgb(64, 64, 64);
            _label23.Location = new Point(8, 133);
            _label23.Name = "_label23";
            _label23.Size = new Size(144, 13);
            _label23.TabIndex = 26;
            _label23.Text = "Overlapping areas raw sum =";
            _label21.AutoSize = true;
            _label21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label21.Location = new Point(295, 172);
            _label21.Name = "_label21";
            _label21.Size = new Size(16, 13);
            _label21.TabIndex = 25;
            _label21.Text = "%";
            _label19.AutoSize = true;
            _label19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label19.Location = new Point(245, 172);
            _label19.Name = "_label19";
            _label19.Size = new Size(14, 13);
            _label19.TabIndex = 24;
            _label19.Text = "0";
            _label20.AutoSize = true;
            _label20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label20.ForeColor = Color.DarkRed;
            _label20.Location = new Point(152, 172);
            _label20.Name = "_label20";
            _label20.Size = new Size(87, 13);
            _label20.TabIndex = 23;
            _label20.Text = "Match index =";
            _label17.AutoSize = true;
            _label17.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label17.Location = new Point(152, 107);
            _label17.Name = "_label17";
            _label17.Size = new Size(14, 13);
            _label17.TabIndex = 22;
            _label17.Text = "0";
            _label18.AutoSize = true;
            _label18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            _label18.ForeColor = Color.FromArgb(64, 64, 64);
            _label18.Location = new Point(8, 107);
            _label18.Name = "_label18";
            _label18.Size = new Size(140, 13);
            _label18.TabIndex = 21;
            _label18.Text = "Blue non-overlapping area =";
            _label15.AutoSize = true;
            _label15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label15.Location = new Point(142, 94);
            _label15.Name = "_label15";
            _label15.Size = new Size(14, 13);
            _label15.TabIndex = 20;
            _label15.Text = "0";
            _label16.AutoSize = true;
            _label16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            _label16.ForeColor = Color.FromArgb(64, 64, 64);
            _label16.Location = new Point(8, 94);
            _label16.Name = "_label16";
            _label16.Size = new Size(129, 13);
            _label16.TabIndex = 19;
            _label16.Text = "Yellow overlapping area =";
            _label11.AutoSize = true;
            _label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label11.ForeColor = Color.RoyalBlue;
            _label11.Location = new Point(8, 42);
            _label11.Name = "_label11";
            _label11.Size = new Size(139, 13);
            _label11.TabIndex = 15;
            _label11.Text = "Blue Figure Perimeter =";
            _label12.AutoSize = true;
            _label12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label12.Location = new Point(151, 42);
            _label12.Name = "_label12";
            _label12.Size = new Size(14, 13);
            _label12.TabIndex = 16;
            _label12.Text = "0";
            _label13.AutoSize = true;
            _label13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label13.ForeColor = Color.DarkGoldenrod;
            _label13.Location = new Point(8, 55);
            _label13.Name = "_label13";
            _label13.Size = new Size(148, 13);
            _label13.TabIndex = 17;
            _label13.Text = "Yellow figure Perimeter =";
            _label14.AutoSize = true;
            _label14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label14.Location = new Point(158, 55);
            _label14.Name = "_label14";
            _label14.Size = new Size(14, 13);
            _label14.TabIndex = 18;
            _label14.Text = "0";
            _label4.AutoSize = true;
            _label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label4.ForeColor = Color.DarkGoldenrod;
            _label4.Location = new Point(8, 29);
            _label4.Name = "_label4";
            _label4.Size = new Size(121, 13);
            _label4.TabIndex = 6;
            _label4.Text = "Yellow figure Area =";
            _label3.AutoSize = true;
            _label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label3.Location = new Point(136, 29);
            _label3.Name = "_label3";
            _label3.Size = new Size(14, 13);
            _label3.TabIndex = 7;
            _label3.Text = "0";
            _label9.AutoSize = true;
            _label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label9.Location = new Point(164, 120);
            _label9.Name = "_label9";
            _label9.Size = new Size(14, 13);
            _label9.TabIndex = 14;
            _label9.Text = "0";
            _label6.AutoSize = true;
            _label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            _label6.ForeColor = Color.FromArgb(64, 64, 64);
            _label6.Location = new Point(8, 68);
            _label6.Name = "_label6";
            _label6.Size = new Size(138, 13);
            _label6.TabIndex = 8;
            _label6.Text = "Difference between areas =";
            _label10.AutoSize = true;
            _label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            _label10.ForeColor = Color.FromArgb(64, 64, 64);
            _label10.Location = new Point(8, 120);
            _label10.Name = "_label10";
            _label10.Size = new Size(150, 13);
            _label10.TabIndex = 13;
            _label10.Text = "Yellow non-overlapping area =";
            _label5.AutoSize = true;
            _label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label5.Location = new Point(149, 68);
            _label5.Name = "_label5";
            _label5.Size = new Size(14, 13);
            _label5.TabIndex = 9;
            _label5.Text = "0";
            _label7.AutoSize = true;
            _label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label7.Location = new Point(129, 81);
            _label7.Name = "_label7";
            _label7.Size = new Size(14, 13);
            _label7.TabIndex = 11;
            _label7.Text = "0";
            _label8.AutoSize = true;
            _label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            _label8.ForeColor = Color.FromArgb(64, 64, 64);
            _label8.Location = new Point(8, 81);
            _label8.Name = "_label8";
            _label8.Size = new Size(119, 13);
            _label8.TabIndex = 10;
            _label8.Text = "Blue overlapping area =";
            _chart1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            chartArea.AlignmentOrientation = AreaAlignmentOrientations.All;
            chartArea.Name = "ChartArea1";
            _chart1.ChartAreas.Add(chartArea);
            legend.Name = "Legend1";
            _chart1.Legends.Add(legend);
            _chart1.Location = new Point(358, 0);
            _chart1.Name = "_chart1";
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
            _chart1.Series.Add(series);
            _chart1.Series.Add(series2);
            _chart1.Size = new Size(576, 255);
            _chart1.TabIndex = 4;
            _chart1.Text = "chart1";
            _dataSet1.DataSetName = "NewDataSet";
            _dataSet1.Tables.AddRange(new DataTable[]
            {
                _dataTable1
            });
            _dataTable1.TableName = "Table1";
            _contextMenuStrip1.Items.AddRange(new ToolStripItem[]
            {
                _aboutToolStripMenuItem,
                _helpToolStripMenuItem
            });
            _contextMenuStrip1.Name = "_contextMenuStrip1";
            _contextMenuStrip1.Size = new Size(108, 48);
            _contextMenuStrip1.Opening += new CancelEventHandler(contextMenuStrip1_Opening);
            _aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
            _aboutToolStripMenuItem.Size = new Size(107, 22);
            _aboutToolStripMenuItem.Text = "About";
            _aboutToolStripMenuItem.Click += new EventHandler(aboutToolStripMenuItem_Click);
            _helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            _helpToolStripMenuItem.Size = new Size(107, 22);
            _helpToolStripMenuItem.Text = "Help";
            _helpToolStripMenuItem.Click += new EventHandler(helpToolStripMenuItem_Click);
            _checkBox2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            _checkBox2.AutoSize = true;
            _checkBox2.Location = new Point(720, 542);
            _checkBox2.Name = "_checkBox2";
            _checkBox2.Size = new Size(132, 17);
            _checkBox2.TabIndex = 24;
            _checkBox2.Text = "Show difference areas";
            _checkBox2.UseVisualStyleBackColor = true;
            _checkBox2.Visible = false;
            _checkBox2.CheckedChanged += new EventHandler(checkBox2_CheckedChanged);
            _checkBox1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            _checkBox1.AutoSize = true;
            _checkBox1.Checked = true;
            _checkBox1.CheckState = CheckState.Checked;
            _checkBox1.Location = new Point(720, 521);
            _checkBox1.Name = "_checkBox1";
            _checkBox1.Size = new Size(87, 17);
            _checkBox1.TabIndex = 23;
            _checkBox1.Text = "Show figures";
            _checkBox1.UseVisualStyleBackColor = true;
            _checkBox1.Visible = false;
            _checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
            _trackBar1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            _trackBar1.Location = new Point(60, 514);
            _trackBar1.Maximum = 200;
            _trackBar1.Minimum = 1;
            _trackBar1.Name = "_trackBar1";
            _trackBar1.Size = new Size(307, 45);
            _trackBar1.TabIndex = 31;
            _trackBar1.Value = 100;
            _trackBar1.Visible = false;
            _trackBar1.Scroll += new EventHandler(trackBar1_Scroll);
            _label26.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            _label26.AutoSize = true;
            _label26.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label26.Location = new Point(373, 528);
            _label26.Name = "_label26";
            _label26.Size = new Size(36, 13);
            _label26.TabIndex = 32;
            _label26.Text = "Grow";
            _label26.Visible = false;
            _label27.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            _label27.AutoSize = true;
            _label27.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 238);
            _label27.Location = new Point(11, 528);
            _label27.Name = "_label27";
            _label27.Size = new Size(43, 13);
            _label27.TabIndex = 33;
            _label27.Text = "Shrink";
            _label27.Visible = false;
            _button2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            _button2.Location = new Point(415, 523);
            _button2.Name = "_button2";
            _button2.Size = new Size(91, 23);
            _button2.TabIndex = 34;
            _button2.Text = "Scale To Area";
            _button2.UseVisualStyleBackColor = true;
            _button2.Click += new EventHandler(button2_Click);
            _timer1.Interval = 50;
            _form1BindingSource.DataSource = typeof(Form1);
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(961, 558);
            ContextMenuStrip = _contextMenuStrip1;
            Controls.Add(_button2);
            Controls.Add(_label27);
            Controls.Add(_label26);
            Controls.Add(_trackBar1);
            Controls.Add(_checkBox2);
            Controls.Add(_checkBox1);
            Controls.Add(_panel1);
            Controls.Add(_button1);
            Name = "Form1";
            Text = "5";
            Load += new EventHandler(Form1_Load);
            ((ISupportInitialize)_dataGridView1).EndInit();
            _panel1.ResumeLayout(false);
            _groupBox2.ResumeLayout(false);
            _groupBox2.PerformLayout();
            _groupBox1.ResumeLayout(false);
            _groupBox1.PerformLayout();
            ((ISupportInitialize)_chart1).EndInit();
            ((ISupportInitialize)_dataSet1).EndInit();
            ((ISupportInitialize)_dataTable1).EndInit();
            _contextMenuStrip1.ResumeLayout(false);
            ((ISupportInitialize)_trackBar1).EndInit();
            ((ISupportInitialize)_form1BindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
