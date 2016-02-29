﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GeoLib;
using SpatialMapsCompare.Properties;

namespace SpatialMapsCompare
{
    public class Form1 : Form
    {
        private Dictionary<string, Polygon> Polygons = new Dictionary<string, Polygon>();

        private C2DPointSet _chartTempBlue = new C2DPointSet();

        private C2DPointSet _chartTempYellow = new C2DPointSet();

        private Series _tempSeries1 = new Series();

        private Series _tempSeries2 = new Series();

        private CGrid _someGrid = new CGrid();

        private C2DPolygon _oryginalPolygon;

        private C2DPolygon _comparedPolygon;
        
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
            //_oryginalPolygon = new C2DPolygon(_oryginalPolygonC2DPoints, true);
            //_comparedPolygon = new C2DPolygon(_userPolygonC2DPoints, true);
            //_area = _oryginalPolygon.GetArea();
            //fill_grid(_comparedPolygon, 0);
            //fill_grid(_oryginalPolygon, 2);
            //_tempSeries1 = _chart1.Series[0];
            //_tempSeries2 = _chart1.Series[1];
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
            List<C2DPoint> tempPoints = new C2DPointSet();;
            for (int i = 0; i < _dataGridView1.Rows.Count; i++)
            {
                if (_dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    tempPoints.Add(new C2DPoint((double)_dataGridView1.Rows[i].Cells[firstRowIndex].Value, (double)_dataGridView1.Rows[i].Cells[firstRowIndex + 1].Value));
                }
            }
            _comparedPolygon = new C2DPolygon(tempPoints, true);
        }

        public override void Refresh()
        {
            if (_oryginalPolygon == null || _comparedPolygon == null) return;
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
            update_figures_grid();
            base.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Refresh();
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
                    Refresh();
                    flag = true;
                }
                else if (_comparedArea < _area * (1.0 - _precision))
                {
                    _trackBar1.Value++;
                    Refresh();
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this._label1 = new System.Windows.Forms.Label();
            this._label2 = new System.Windows.Forms.Label();
            this._button1 = new System.Windows.Forms.Button();
            this._dataGridView1 = new System.Windows.Forms.DataGridView();
            this._x = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._oryginalX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._oryginalY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._panel1 = new System.Windows.Forms.Panel();
            this._groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this._groupBox1 = new System.Windows.Forms.GroupBox();
            this._label24 = new System.Windows.Forms.Label();
            this._label25 = new System.Windows.Forms.Label();
            this._label22 = new System.Windows.Forms.Label();
            this._label23 = new System.Windows.Forms.Label();
            this._label21 = new System.Windows.Forms.Label();
            this._label19 = new System.Windows.Forms.Label();
            this._label20 = new System.Windows.Forms.Label();
            this._label17 = new System.Windows.Forms.Label();
            this._label18 = new System.Windows.Forms.Label();
            this._label15 = new System.Windows.Forms.Label();
            this._label16 = new System.Windows.Forms.Label();
            this._label11 = new System.Windows.Forms.Label();
            this._label12 = new System.Windows.Forms.Label();
            this._label13 = new System.Windows.Forms.Label();
            this._label14 = new System.Windows.Forms.Label();
            this._label4 = new System.Windows.Forms.Label();
            this._label3 = new System.Windows.Forms.Label();
            this._label9 = new System.Windows.Forms.Label();
            this._label6 = new System.Windows.Forms.Label();
            this._label10 = new System.Windows.Forms.Label();
            this._label5 = new System.Windows.Forms.Label();
            this._label7 = new System.Windows.Forms.Label();
            this._label8 = new System.Windows.Forms.Label();
            this._chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this._dataSet1 = new System.Data.DataSet();
            this._dataTable1 = new System.Data.DataTable();
            this._contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._checkBox2 = new System.Windows.Forms.CheckBox();
            this._checkBox1 = new System.Windows.Forms.CheckBox();
            this._trackBar1 = new System.Windows.Forms.TrackBar();
            this._label26 = new System.Windows.Forms.Label();
            this._label27 = new System.Windows.Forms.Label();
            this._button2 = new System.Windows.Forms.Button();
            this._timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this._form1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView1)).BeginInit();
            this._panel1.SuspendLayout();
            this._groupBox2.SuspendLayout();
            this._groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataTable1)).BeginInit();
            this._contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._form1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this._label1.Location = new System.Drawing.Point(8, 16);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(112, 13);
            this._label1.TabIndex = 0;
            this._label1.Text = "Blue Figure Area =";
            // 
            // _label2
            // 
            this._label2.AutoSize = true;
            this._label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label2.Location = new System.Drawing.Point(129, 16);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(14, 13);
            this._label2.TabIndex = 1;
            this._label2.Text = "0";
            // 
            // _button1
            // 
            this._button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button1.Location = new System.Drawing.Point(874, 523);
            this._button1.Name = "_button1";
            this._button1.Size = new System.Drawing.Size(75, 23);
            this._button1.TabIndex = 2;
            this._button1.Text = "Calculate";
            this._button1.UseVisualStyleBackColor = true;
            this._button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _dataGridView1
            // 
            this._dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this._dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._x,
            this._y,
            this._oryginalX,
            this._oryginalY});
            this._dataGridView1.Location = new System.Drawing.Point(0, 0);
            this._dataGridView1.Name = "_dataGridView1";
            this._dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._dataGridView1.Size = new System.Drawing.Size(352, 483);
            this._dataGridView1.TabIndex = 3;
            this._dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // _x
            // 
            this._x.FillWeight = 48.89976F;
            this._x.HeaderText = "Your X";
            this._x.Name = "_x";
            this._x.Width = 64;
            // 
            // _y
            // 
            this._y.FillWeight = 151.1003F;
            this._y.HeaderText = "Your Y";
            this._y.Name = "_y";
            this._y.Width = 64;
            // 
            // _oryginalX
            // 
            this._oryginalX.HeaderText = "Oryginal X";
            this._oryginalX.Name = "_oryginalX";
            this._oryginalX.Width = 80;
            // 
            // _oryginalY
            // 
            this._oryginalY.HeaderText = "Oryginal Y";
            this._oryginalY.Name = "_oryginalY";
            this._oryginalY.Width = 80;
            // 
            // _panel1
            // 
            this._panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panel1.Controls.Add(this._groupBox2);
            this._panel1.Controls.Add(this._groupBox1);
            this._panel1.Controls.Add(this._chart1);
            this._panel1.Controls.Add(this._dataGridView1);
            this._panel1.Location = new System.Drawing.Point(15, 25);
            this._panel1.Name = "_panel1";
            this._panel1.Size = new System.Drawing.Size(934, 483);
            this._panel1.TabIndex = 5;
            // 
            // _groupBox2
            // 
            this._groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._groupBox2.Controls.Add(this.checkedListBox1);
            this._groupBox2.Controls.Add(this.button2);
            this._groupBox2.Controls.Add(this.button1);
            this._groupBox2.Location = new System.Drawing.Point(386, 280);
            this._groupBox2.Name = "_groupBox2";
            this._groupBox2.Size = new System.Drawing.Size(230, 203);
            this._groupBox2.TabIndex = 30;
            this._groupBox2.TabStop = false;
            this._groupBox2.Text = "Load";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(9, 91);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(213, 109);
            this.checkedListBox1.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(99, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(123, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Open Polygon 2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(99, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Open Polygon 1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // _groupBox1
            // 
            this._groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._groupBox1.BackColor = System.Drawing.Color.Transparent;
            this._groupBox1.Controls.Add(this._label24);
            this._groupBox1.Controls.Add(this._label25);
            this._groupBox1.Controls.Add(this._label22);
            this._groupBox1.Controls.Add(this._label23);
            this._groupBox1.Controls.Add(this._label21);
            this._groupBox1.Controls.Add(this._label19);
            this._groupBox1.Controls.Add(this._label20);
            this._groupBox1.Controls.Add(this._label17);
            this._groupBox1.Controls.Add(this._label18);
            this._groupBox1.Controls.Add(this._label15);
            this._groupBox1.Controls.Add(this._label16);
            this._groupBox1.Controls.Add(this._label11);
            this._groupBox1.Controls.Add(this._label12);
            this._groupBox1.Controls.Add(this._label13);
            this._groupBox1.Controls.Add(this._label14);
            this._groupBox1.Controls.Add(this._label1);
            this._groupBox1.Controls.Add(this._label2);
            this._groupBox1.Controls.Add(this._label4);
            this._groupBox1.Controls.Add(this._label3);
            this._groupBox1.Controls.Add(this._label9);
            this._groupBox1.Controls.Add(this._label6);
            this._groupBox1.Controls.Add(this._label10);
            this._groupBox1.Controls.Add(this._label5);
            this._groupBox1.Controls.Add(this._label7);
            this._groupBox1.Controls.Add(this._label8);
            this._groupBox1.Location = new System.Drawing.Point(614, 280);
            this._groupBox1.Name = "_groupBox1";
            this._groupBox1.Size = new System.Drawing.Size(320, 203);
            this._groupBox1.TabIndex = 18;
            this._groupBox1.TabStop = false;
            // 
            // _label24
            // 
            this._label24.AutoSize = true;
            this._label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label24.Location = new System.Drawing.Point(205, 146);
            this._label24.Name = "_label24";
            this._label24.Size = new System.Drawing.Size(14, 13);
            this._label24.TabIndex = 29;
            this._label24.Text = "0";
            // 
            // _label25
            // 
            this._label25.AutoSize = true;
            this._label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label25.ForeColor = System.Drawing.SystemColors.ControlText;
            this._label25.Location = new System.Drawing.Point(8, 146);
            this._label25.Name = "_label25";
            this._label25.Size = new System.Drawing.Size(196, 13);
            this._label25.TabIndex = 28;
            this._label25.Text = "Non-overlapping areas raw sum =";
            // 
            // _label22
            // 
            this._label22.AutoSize = true;
            this._label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label22.Location = new System.Drawing.Point(158, 133);
            this._label22.Name = "_label22";
            this._label22.Size = new System.Drawing.Size(14, 13);
            this._label22.TabIndex = 27;
            this._label22.Text = "0";
            // 
            // _label23
            // 
            this._label23.AutoSize = true;
            this._label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._label23.Location = new System.Drawing.Point(8, 133);
            this._label23.Name = "_label23";
            this._label23.Size = new System.Drawing.Size(144, 13);
            this._label23.TabIndex = 26;
            this._label23.Text = "Overlapping areas raw sum =";
            // 
            // _label21
            // 
            this._label21.AutoSize = true;
            this._label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label21.Location = new System.Drawing.Point(295, 172);
            this._label21.Name = "_label21";
            this._label21.Size = new System.Drawing.Size(16, 13);
            this._label21.TabIndex = 25;
            this._label21.Text = "%";
            // 
            // _label19
            // 
            this._label19.AutoSize = true;
            this._label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label19.Location = new System.Drawing.Point(245, 172);
            this._label19.Name = "_label19";
            this._label19.Size = new System.Drawing.Size(14, 13);
            this._label19.TabIndex = 24;
            this._label19.Text = "0";
            // 
            // _label20
            // 
            this._label20.AutoSize = true;
            this._label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label20.ForeColor = System.Drawing.Color.DarkRed;
            this._label20.Location = new System.Drawing.Point(152, 172);
            this._label20.Name = "_label20";
            this._label20.Size = new System.Drawing.Size(87, 13);
            this._label20.TabIndex = 23;
            this._label20.Text = "Match index =";
            // 
            // _label17
            // 
            this._label17.AutoSize = true;
            this._label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label17.Location = new System.Drawing.Point(152, 107);
            this._label17.Name = "_label17";
            this._label17.Size = new System.Drawing.Size(14, 13);
            this._label17.TabIndex = 22;
            this._label17.Text = "0";
            // 
            // _label18
            // 
            this._label18.AutoSize = true;
            this._label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._label18.Location = new System.Drawing.Point(8, 107);
            this._label18.Name = "_label18";
            this._label18.Size = new System.Drawing.Size(140, 13);
            this._label18.TabIndex = 21;
            this._label18.Text = "Blue non-overlapping area =";
            // 
            // _label15
            // 
            this._label15.AutoSize = true;
            this._label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label15.Location = new System.Drawing.Point(142, 94);
            this._label15.Name = "_label15";
            this._label15.Size = new System.Drawing.Size(14, 13);
            this._label15.TabIndex = 20;
            this._label15.Text = "0";
            // 
            // _label16
            // 
            this._label16.AutoSize = true;
            this._label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._label16.Location = new System.Drawing.Point(8, 94);
            this._label16.Name = "_label16";
            this._label16.Size = new System.Drawing.Size(129, 13);
            this._label16.TabIndex = 19;
            this._label16.Text = "Yellow overlapping area =";
            // 
            // _label11
            // 
            this._label11.AutoSize = true;
            this._label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label11.ForeColor = System.Drawing.Color.RoyalBlue;
            this._label11.Location = new System.Drawing.Point(8, 42);
            this._label11.Name = "_label11";
            this._label11.Size = new System.Drawing.Size(139, 13);
            this._label11.TabIndex = 15;
            this._label11.Text = "Blue Figure Perimeter =";
            // 
            // _label12
            // 
            this._label12.AutoSize = true;
            this._label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label12.Location = new System.Drawing.Point(151, 42);
            this._label12.Name = "_label12";
            this._label12.Size = new System.Drawing.Size(14, 13);
            this._label12.TabIndex = 16;
            this._label12.Text = "0";
            // 
            // _label13
            // 
            this._label13.AutoSize = true;
            this._label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label13.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this._label13.Location = new System.Drawing.Point(8, 55);
            this._label13.Name = "_label13";
            this._label13.Size = new System.Drawing.Size(148, 13);
            this._label13.TabIndex = 17;
            this._label13.Text = "Yellow figure Perimeter =";
            // 
            // _label14
            // 
            this._label14.AutoSize = true;
            this._label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label14.Location = new System.Drawing.Point(158, 55);
            this._label14.Name = "_label14";
            this._label14.Size = new System.Drawing.Size(14, 13);
            this._label14.TabIndex = 18;
            this._label14.Text = "0";
            // 
            // _label4
            // 
            this._label4.AutoSize = true;
            this._label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label4.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this._label4.Location = new System.Drawing.Point(8, 29);
            this._label4.Name = "_label4";
            this._label4.Size = new System.Drawing.Size(121, 13);
            this._label4.TabIndex = 6;
            this._label4.Text = "Yellow figure Area =";
            // 
            // _label3
            // 
            this._label3.AutoSize = true;
            this._label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label3.Location = new System.Drawing.Point(136, 29);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(14, 13);
            this._label3.TabIndex = 7;
            this._label3.Text = "0";
            // 
            // _label9
            // 
            this._label9.AutoSize = true;
            this._label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label9.Location = new System.Drawing.Point(164, 120);
            this._label9.Name = "_label9";
            this._label9.Size = new System.Drawing.Size(14, 13);
            this._label9.TabIndex = 14;
            this._label9.Text = "0";
            // 
            // _label6
            // 
            this._label6.AutoSize = true;
            this._label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._label6.Location = new System.Drawing.Point(8, 68);
            this._label6.Name = "_label6";
            this._label6.Size = new System.Drawing.Size(138, 13);
            this._label6.TabIndex = 8;
            this._label6.Text = "Difference between areas =";
            // 
            // _label10
            // 
            this._label10.AutoSize = true;
            this._label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._label10.Location = new System.Drawing.Point(8, 120);
            this._label10.Name = "_label10";
            this._label10.Size = new System.Drawing.Size(150, 13);
            this._label10.TabIndex = 13;
            this._label10.Text = "Yellow non-overlapping area =";
            // 
            // _label5
            // 
            this._label5.AutoSize = true;
            this._label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label5.Location = new System.Drawing.Point(149, 68);
            this._label5.Name = "_label5";
            this._label5.Size = new System.Drawing.Size(14, 13);
            this._label5.TabIndex = 9;
            this._label5.Text = "0";
            // 
            // _label7
            // 
            this._label7.AutoSize = true;
            this._label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label7.Location = new System.Drawing.Point(129, 81);
            this._label7.Name = "_label7";
            this._label7.Size = new System.Drawing.Size(14, 13);
            this._label7.TabIndex = 11;
            this._label7.Text = "0";
            // 
            // _label8
            // 
            this._label8.AutoSize = true;
            this._label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._label8.Location = new System.Drawing.Point(8, 81);
            this._label8.Name = "_label8";
            this._label8.Size = new System.Drawing.Size(119, 13);
            this._label8.TabIndex = 10;
            this._label8.Text = "Blue overlapping area =";
            // 
            // _chart1
            // 
            this._chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea1.Name = "ChartArea1";
            this._chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this._chart1.Legends.Add(legend1);
            this._chart1.Location = new System.Drawing.Point(358, 0);
            this._chart1.Name = "_chart1";
            series1.BorderWidth = 9;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series2.BorderWidth = 9;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            this._chart1.Series.Add(series1);
            this._chart1.Series.Add(series2);
            this._chart1.Size = new System.Drawing.Size(576, 255);
            this._chart1.TabIndex = 4;
            this._chart1.Text = "chart1";
            // 
            // _dataSet1
            // 
            this._dataSet1.DataSetName = "NewDataSet";
            this._dataSet1.Tables.AddRange(new System.Data.DataTable[] {
            this._dataTable1});
            // 
            // _dataTable1
            // 
            this._dataTable1.TableName = "Table1";
            // 
            // _contextMenuStrip1
            // 
            this._contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._aboutToolStripMenuItem,
            this._helpToolStripMenuItem});
            this._contextMenuStrip1.Name = "_contextMenuStrip1";
            this._contextMenuStrip1.Size = new System.Drawing.Size(108, 48);
            this._contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // _aboutToolStripMenuItem
            // 
            this._aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
            this._aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this._aboutToolStripMenuItem.Text = "About";
            this._aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // _helpToolStripMenuItem
            // 
            this._helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            this._helpToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this._helpToolStripMenuItem.Text = "Help";
            this._helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // _checkBox2
            // 
            this._checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._checkBox2.AutoSize = true;
            this._checkBox2.Location = new System.Drawing.Point(720, 542);
            this._checkBox2.Name = "_checkBox2";
            this._checkBox2.Size = new System.Drawing.Size(132, 17);
            this._checkBox2.TabIndex = 24;
            this._checkBox2.Text = "Show difference areas";
            this._checkBox2.UseVisualStyleBackColor = true;
            this._checkBox2.Visible = false;
            this._checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // _checkBox1
            // 
            this._checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._checkBox1.AutoSize = true;
            this._checkBox1.Checked = true;
            this._checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkBox1.Location = new System.Drawing.Point(720, 521);
            this._checkBox1.Name = "_checkBox1";
            this._checkBox1.Size = new System.Drawing.Size(87, 17);
            this._checkBox1.TabIndex = 23;
            this._checkBox1.Text = "Show figures";
            this._checkBox1.UseVisualStyleBackColor = true;
            this._checkBox1.Visible = false;
            this._checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // _trackBar1
            // 
            this._trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._trackBar1.Location = new System.Drawing.Point(60, 514);
            this._trackBar1.Maximum = 200;
            this._trackBar1.Minimum = 1;
            this._trackBar1.Name = "_trackBar1";
            this._trackBar1.Size = new System.Drawing.Size(307, 45);
            this._trackBar1.TabIndex = 31;
            this._trackBar1.Value = 100;
            this._trackBar1.Visible = false;
            this._trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // _label26
            // 
            this._label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._label26.AutoSize = true;
            this._label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label26.Location = new System.Drawing.Point(373, 528);
            this._label26.Name = "_label26";
            this._label26.Size = new System.Drawing.Size(36, 13);
            this._label26.TabIndex = 32;
            this._label26.Text = "Grow";
            this._label26.Visible = false;
            // 
            // _label27
            // 
            this._label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._label27.AutoSize = true;
            this._label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._label27.Location = new System.Drawing.Point(11, 528);
            this._label27.Name = "_label27";
            this._label27.Size = new System.Drawing.Size(43, 13);
            this._label27.TabIndex = 33;
            this._label27.Text = "Shrink";
            this._label27.Visible = false;
            // 
            // _button2
            // 
            this._button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button2.Location = new System.Drawing.Point(415, 523);
            this._button2.Name = "_button2";
            this._button2.Size = new System.Drawing.Size(91, 23);
            this._button2.TabIndex = 34;
            this._button2.Text = "Scale To Area";
            this._button2.UseVisualStyleBackColor = true;
            this._button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // _timer1
            // 
            this._timer1.Interval = 50;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // _form1BindingSource
            // 
            this._form1BindingSource.DataSource = typeof(SpatialMapsCompare.Form1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 558);
            this.ContextMenuStrip = this._contextMenuStrip1;
            this.Controls.Add(this._button2);
            this.Controls.Add(this._label27);
            this.Controls.Add(this._label26);
            this.Controls.Add(this._trackBar1);
            this.Controls.Add(this._checkBox2);
            this.Controls.Add(this._checkBox1);
            this.Controls.Add(this._panel1);
            this.Controls.Add(this._button1);
            this.Name = "Form1";
            this.Text = "5";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView1)).EndInit();
            this._panel1.ResumeLayout(false);
            this._groupBox2.ResumeLayout(false);
            this._groupBox1.ResumeLayout(false);
            this._groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataTable1)).EndInit();
            this._contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._form1BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Button button1;
        private Button button2;
        private OpenFileDialog openFileDialog1;
        private CheckedListBox checkedListBox1;

        private Polygon ReadPolygonFromFile(string fileName)
        {
            Polygon tempPoly = null;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrWhiteSpace(fileNameWithoutExtension))
            {
                MessageBox.Show($"The path choosen (\"{fileName}\") does not contain valid file name", "File name empty",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Polygons.ContainsKey(fileNameWithoutExtension))
            {
                MessageBox.Show($"File \"{fileNameWithoutExtension}\" or file with the same name already opened.", $"File \"{fileNameWithoutExtension}\" already open",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                try
                {
                    tempPoly = Helper.DeserializeFromXml<Polygon>(fileName);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, $"Error opening file \"{fileNameWithoutExtension}\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidOperationException iop)
                {
                    MessageBox.Show($"The file \"{fileName}\" is not a valid xml file with polygon points data.", $"Error opening file \"{fileNameWithoutExtension}\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return tempPoly;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var tempPoly = ReadPolygonFromFile(openFileDialog1.FileName);
                if (tempPoly == null) return;
                Polygons.Add(Path.GetFileNameWithoutExtension(openFileDialog1.FileName), tempPoly);
                _oryginalPolygon = tempPoly;

                Refresh();
            }
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var tempPoly = ReadPolygonFromFile(openFileDialog1.FileName);
                if (tempPoly == null) return;
                Polygons.Add(Path.GetFileNameWithoutExtension(openFileDialog1.FileName), tempPoly);
                _comparedPolygon = tempPoly;
                
                Refresh();
            }
        }

    }
}
