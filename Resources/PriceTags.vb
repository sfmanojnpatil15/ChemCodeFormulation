Public Class PriceTags
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Me.Close()

    End Sub

    Private Sub PriceTags_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Me.Tag = "Packets" Then
            For Each cl As DataGridViewColumn In DataGridView1.Columns
                If cl.Name = "Unit Price" Then
                    cl.HeaderText = "Unit Price"
                    cl.ReadOnly = True
                    cl.CellTemplate = New DataGridViewTextBoxCell()
                    cl.DefaultCellStyle.BackColor = Color.LightGray
                End If
            Next
            DGVCal()
            AddHandler DataGridView1.CellValidated, AddressOf DGVCal
            AddHandler DataGridView1.CellValidating, AddressOf DGVCal
            DataGridView1.Height = 538
            RichTextBox1.Visible = False
        ElseIf Me.Tag = "bottles" Then
            DataGridView1.Height = 391
            RichTextBox1.Visible = True
        End If


    End Sub




    Private Sub DGVCal()
        Try
            With DataGridView1

                For j As Integer = 0 To .Rows.Count - 2
                    Dim PriCost As Double
                    Dim FillCost As Double
                    Dim IngCost As Double
                    Dim UnitCos As Double

                    PriCost = .Rows(j).Cells(1).Value / 1000
                    FillCost = .Rows(j).Cells(2).Value / 1000
                    IngCost = .Rows(j).Cells(3).Value

                    UnitCos = PriCost + FillCost + IngCost

                    .Rows(j).Cells(4).Value = Format(UnitCos, "N3")
                Next
            End With
        Catch ex As System.Exception

        End Try

    End Sub


End Class