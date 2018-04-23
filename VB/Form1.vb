Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid

Namespace HorizontalMerging
	Partial Public Class Form1
		Inherits Form
		Private mergedCells As New List(Of MyMergedCellInfo)()
		Private ViewHandler As MyGridViewHandler = Nothing
		Public Sub New()
			InitializeComponent()
			ViewHandler = New MyGridViewHandler(gridView1)
		End Sub

		Private Function CreateTable(ByVal RowCount As Integer) As DataTable
			Dim tbl As New DataTable()
			tbl.Columns.Add("Name1", GetType(String))
			tbl.Columns.Add("Name2", GetType(String))
			tbl.Columns.Add("Name3", GetType(String))
			tbl.Columns.Add("Name4", GetType(String))
			For i As Integer = 0 To RowCount - 1
				If i = 1 Then
					tbl.Rows.Add(New Object() { String.Format("Name{0}", i), "This is a long long string, which is merged for several columns", "", "" })
				ElseIf i = 3 Then
					tbl.Rows.Add(New Object() { String.Format("Name{0}", i), "This is a long long string, which is merged for several columns", "", "" })
				Else
					tbl.Rows.Add(New Object() { String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i) })
				End If
			Next i
			Return tbl
		End Function

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			gridControl1.DataSource = CreateTable(20)
			gridView1.Columns(0).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
			gridView1.Columns(3).Width = 300

			gridControl1.ForceInitialize()
			ViewHandler.MergeCells(gridView1.GetRowCellValue(1, "Name2").ToString(), gridView1.GetDataSourceRowIndex(1), New GridColumn() { gridView1.Columns(1), gridView1.Columns(2), gridView1.Columns(3) })
			ViewHandler.MergeCells(gridView1.GetRowCellValue(3, "Name2").ToString(), gridView1.GetDataSourceRowIndex(3), New GridColumn() { gridView1.Columns(1), gridView1.Columns(2), gridView1.Columns(3) })
		End Sub
	End Class
End Namespace
