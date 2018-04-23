Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Drawing
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Base.ViewInfo

Namespace HorizontalMerging
	Public Class MyGridViewHandler
		Protected view_ As GridView
		Public ReadOnly Property View() As GridView
			Get
				Return view_
			End Get
		End Property

		Protected mergedCells As New List(Of MyMergedCellInfo)()

		Public Sub MergeCells(ByVal sValue As String, ByVal iRowHandle As Integer, ByVal gridColumns() As GridColumn)
			Dim myCellInfo As New MyMergedCellInfo(sValue, iRowHandle)
			For Each item As GridColumn In gridColumns
				myCellInfo.Columns.Add(item)
			Next item
			mergedCells.Add(myCellInfo)
		End Sub

		Public Sub New(ByVal someView As GridView)
			view_ = someView
			AddHandler view_.CustomDrawCell, AddressOf view_CustomDrawCell
		End Sub

		Private Sub view_CustomDrawCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs)
			Dim textRect As Rectangle = e.Bounds

			Dim currentInfo As MyMergedCellInfo = Nothing
			For Each item As MyMergedCellInfo In mergedCells
				If item.RowHandle = View.GetDataSourceRowIndex(e.RowHandle) AndAlso item.Columns.Contains(e.Column) Then
					currentInfo = item
					Exit For
				End If
			Next item
			If currentInfo IsNot Nothing Then
				Dim clipBoundsX As Integer = 0
				Dim currentClip As RectangleF = e.Cache.ClipInfo.MaximumBounds
				If currentInfo IsNot Nothing Then
					For Each item As GridColumn In currentInfo.Columns
						If item Is e.Column Then
							Continue For
						End If
						If currentInfo.Columns.IndexOf(item) > currentInfo.Columns.IndexOf(e.Column) Then
							textRect.Width += item.VisibleWidth
						Else
							textRect.X -= item.VisibleWidth
							textRect.Width += item.VisibleWidth
						End If
					Next item
					e.DisplayText = currentInfo.DisplayText
					clipBoundsX = If(CInt(Math.Truncate(currentClip.X)) < e.Bounds.X, e.Bounds.X - 4, CInt(Math.Truncate(currentClip.X)))

					If View.LeftCoord > 0 Then
						e.Cache.ClipInfo.SetClip(New Rectangle(clipBoundsX, CInt(Math.Truncate(currentClip.Y)), textRect.Width, CInt(currentClip.Height)))
					End If

					Dim lines As IndentInfoCollection = (TryCast(e.Cell, GridCellInfo)).RowInfo.Lines
					Dim removedLines As New List(Of IndentInfo)()
					For Each currentLine As IndentInfo In lines
						If textRect.X <= (currentLine.Bounds.X - View.LeftCoord) AndAlso (textRect.Width + textRect.X) >= (currentLine.Bounds.X - View.LeftCoord) AndAlso currentLine.Bounds.Y <= textRect.Y AndAlso (currentLine.Bounds.Y + currentLine.Bounds.Height) >= textRect.Y Then
							currentLine.OffsetContent(-currentLine.Bounds.X, -currentLine.Bounds.Y)
						End If
					Next currentLine
				End If
				e.Appearance.DrawBackground(e.Cache, textRect)
				e.Appearance.DrawString(e.Cache, e.DisplayText, textRect)
				e.Handled = True
				e.Cache.ClipInfo.SetClip(New Rectangle(CInt(Math.Truncate(currentClip.X)), CInt(Math.Truncate(currentClip.Y)), CInt(currentClip.Width), CInt(currentClip.Height)))
			End If
		End Sub
	End Class

	Public Class MyMergedCellInfo
		Private columns_ As List(Of GridColumn)
		Private displayText_ As String
		Private rowHandle_ As Integer

		Public ReadOnly Property Columns() As List(Of GridColumn)
			Get
				Return columns_
			End Get
		End Property

		Public ReadOnly Property DisplayText() As String
			Get
				Return displayText_
			End Get
		End Property

		Public ReadOnly Property RowHandle() As Integer
			Get
				Return rowHandle_
			End Get
		End Property

		Public Sub New(ByVal sDisplayText As String, ByVal iRowHandle As Integer)
			columns_ = New List(Of GridColumn)()
			displayText_ = sDisplayText
			rowHandle_ = iRowHandle
		End Sub
	End Class
End Namespace
