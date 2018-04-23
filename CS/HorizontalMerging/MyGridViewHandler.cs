using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base.ViewInfo;

namespace HorizontalMerging
{
    public class MyGridViewHandler
    {
        protected GridView view_;
        public GridView View { get { return view_; } }

        protected List<MyMergedCellInfo> mergedCells = new List<MyMergedCellInfo>();

        public void MergeCells(string sValue, int iRowHandle, GridColumn[] gridColumns)
        {
            MyMergedCellInfo myCellInfo = new MyMergedCellInfo(sValue, iRowHandle);
            foreach (GridColumn item in gridColumns)
            {
                myCellInfo.Columns.Add(item);    
            }
            mergedCells.Add(myCellInfo);        
        }

        public MyGridViewHandler(GridView someView)
        {
            view_ = someView;
            view_.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(view_CustomDrawCell);
        }

        void view_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            Rectangle textRect = e.Bounds;

            MyMergedCellInfo currentInfo = null;
            foreach (MyMergedCellInfo item in mergedCells)
            {
                if (item.RowHandle == View.GetDataSourceRowIndex(e.RowHandle) && item.Columns.Contains(e.Column))
                {
                    currentInfo = item;
                    break;
                }
            }

            if (currentInfo != null)
            {
                int clipBoundsX = 0;
                RectangleF currentClip = e.Graphics.ClipBounds;
                if (currentInfo != null)
                {
                    foreach (GridColumn item in currentInfo.Columns)
                    {
                        if (item == e.Column) continue;
                        if (currentInfo.Columns.IndexOf(item) > currentInfo.Columns.IndexOf(e.Column))
                        {
                            textRect.Width += item.Width;
                        }
                        else
                        {
                            textRect.X -= item.Width;
                            textRect.Width += item.Width;
                        }
                    }
                    e.DisplayText = currentInfo.DisplayText;
                    clipBoundsX = (int)currentClip.X < e.Bounds.X ? e.Bounds.X - 4 : (int)currentClip.X;

                    if (View.LeftCoord > 0)
                        e.Cache.ClipInfo.SetClip(new Rectangle(clipBoundsX, (int)currentClip.Y, textRect.Width, (int)currentClip.Height));


                    IndentInfoCollection lines = (e.Cell as GridCellInfo).RowInfo.Lines;
                    List<IndentInfo> removedLines = new List<IndentInfo>();
                    foreach (IndentInfo currentLine in lines) {
                        if (textRect.X <= (currentLine.Bounds.X - View.LeftCoord) && (textRect.Width + textRect.X) >= (currentLine.Bounds.X - View.LeftCoord) && currentLine.Bounds.Y <= textRect.Y && (currentLine.Bounds.Y + currentLine.Bounds.Height) >= textRect.Y) {
                            currentLine.OffsetContent(-currentLine.Bounds.X, -currentLine.Bounds.Y); }
			        }
                }

                e.Appearance.DrawBackground(e.Cache, textRect);
                e.Appearance.DrawString(e.Cache, e.DisplayText, textRect);
                e.Handled = true;

                e.Cache.ClipInfo.SetClip(new Rectangle((int)currentClip.X, (int)currentClip.Y, (int)currentClip.Width, (int)currentClip.Height));
            }            
        }
    }

    public class MyMergedCellInfo
    {
        List<GridColumn> columns_;
        string displayText_;
        int rowHandle_;

        public List<GridColumn> Columns
        {
            get { return columns_; }
        }

        public string DisplayText
        {
            get { return displayText_; }
        }

        public int RowHandle
        {
            get { return rowHandle_; }
        }

        public MyMergedCellInfo(string sDisplayText, int iRowHandle)
        {
            columns_ = new List<GridColumn>();
            displayText_ = sDisplayText;
            rowHandle_ = iRowHandle;
        }
    }
}
