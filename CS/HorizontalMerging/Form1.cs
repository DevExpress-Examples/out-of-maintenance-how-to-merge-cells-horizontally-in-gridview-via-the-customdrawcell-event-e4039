using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;

namespace HorizontalMerging
{
    public partial class Form1 : Form
    {
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
            tbl.Columns.Add("Name5", typeof(string));
            for (int i = 0; i < RowCount; i++)
            {
                if (i == 1)
                    tbl.Rows.Add(new object[] { String.Format("Name{0}", i), "This is a long long string, which is merged for several columns", "", "", "" });
                else if (i == 3)
                    tbl.Rows.Add(new object[] { String.Format("Name{0}", i), "This is a long long string, which is merged for several columns", "", "", "Text" });
                else
                    tbl.Rows.Add(new object[] { String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i) });
            }
            return tbl;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = CreateTable(20);
            gridView1.Columns[0].Fixed = FixedStyle.Left;
            gridView1.Columns[4].Width = 300;
            gridControl1.ForceInitialize();
            ViewHandler.MergeCells(gridView1.GetRowCellValue(1, "Name2").ToString(), gridView1.GetDataSourceRowIndex(1), new GridColumn[] { gridView1.Columns[2], gridView1.Columns[3], gridView1.Columns[4] });
            ViewHandler.MergeCells(gridView1.GetRowCellValue(3, "Name2").ToString(), gridView1.GetDataSourceRowIndex(3), new GridColumn[] { gridView1.Columns[2], gridView1.Columns[3] });
        }
    }
 }
