using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;

namespace HorizontalMerging
{
    public partial class Form1 : Form
    {
        List<MyMergedCellInfo> mergedCells = new List<MyMergedCellInfo>();
        MyGridViewHandler ViewHandler = null;
        public Form1()
        {
            InitializeComponent();
            ViewHandler = new MyGridViewHandler(gridView1);
        }

        private DataTable CreateTable(int RowCount)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name1", typeof(string));
            tbl.Columns.Add("Name2", typeof(string));
            tbl.Columns.Add("Name3", typeof(string));
            tbl.Columns.Add("Name4", typeof(string));
            for (int i = 0; i < RowCount; i++)
            {
                if (i == 1)
                    tbl.Rows.Add(new object[] { String.Format("Name{0}", i), "This is a long long string, which is merged for several columns", "", "" });
                else if (i == 3)
                    tbl.Rows.Add(new object[] { String.Format("Name{0}", i), "This is a long long string, which is merged for several columns", "", "" });
                else
                    tbl.Rows.Add(new object[] { String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i) });
            }
            return tbl;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = CreateTable(20);
            gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView1.Columns[3].Width = 300;

            gridControl1.ForceInitialize();
            ViewHandler.MergeCells(gridView1.GetRowCellValue(1, "Name2").ToString(), gridView1.GetDataSourceRowIndex(1), new GridColumn[] { gridView1.Columns[1], gridView1.Columns[2], gridView1.Columns[3] });
            ViewHandler.MergeCells(gridView1.GetRowCellValue(3, "Name2").ToString(), gridView1.GetDataSourceRowIndex(3), new GridColumn[] { gridView1.Columns[1], gridView1.Columns[2], gridView1.Columns[3] });
        }
    }
 }
