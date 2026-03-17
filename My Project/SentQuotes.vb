Imports MySql.Data.MySqlClient
Public Class SentQuotes
    Dim con As New MySqlConnection
    Dim cmd As New MySqlCommand
    Dim da As New MySqlDataAdapter
    Dim ds As New DataSet
    Public continueemail As Boolean = False
    Private Sub SentQuotes_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        con.ConnectionString = My.Settings.DBCon
        cmd.Connection = con
        cmd.CommandType = CommandType.Text

        con.Open()

        'Gets all sent forms for this formula
        cmd.CommandText = "SELECT * FROM SentForms WHERE FormulaID =" & FormulaID.Text & " ORDER BY SentDate DESC"
        da = New MySqlDataAdapter(cmd.CommandText, con)
        da.Fill(ds, "forms")

        'Gets all sent forms details for this formula
        cmd.CommandText = "SELECT * FROM SentFormDetails WHERE formulaid =" & FormulaID.Text
        da = New MySqlDataAdapter(cmd.CommandText, con)
        da.Fill(ds, "formsdet")

        con.Close()

        If ds.Tables("forms").Rows.Count = 0 Then
            MsgBox("No records to display", MessageBoxButtons.OK, "Message")
            Me.Close()
            Exit Sub
        End If

        ds.Tables("forms").PrimaryKey = New DataColumn(0) {ds.Tables("forms").Columns("SentDate")}

        For Each r As DataRow In ds.Tables("forms").Rows
            ListBox1.Items.Add(r.Item("SentDate"))
        Next


        If DataGridView1.Rows.Count = 0 Then
            DataGridView1.Rows.Add()
            DataGridView1.Rows.Add()
        End If
        DataGridView1.Rows(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Rows(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight



        Me.ListBox1.DrawMode = DrawMode.OwnerDrawFixed
        Me.ListBox1.ItemHeight = 25

    End Sub
    Private Sub ListBox1_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles ListBox1.DrawItem

        ' Draw the background of the ListBox control for each item.
        e.DrawBackground()

        ' Define the default color of the brush as black. 
        Dim myBrush As Brush = Brushes.Black

        ' Determine the color of the brush to draw each item based on    
        ' the index of the item to draw. 


        ' Draw the current item text based on the current  
        ' Font and the custom brush settings.
        If e.Index <> -1 Then
            e.Graphics.DrawString(Me.ListBox1.Items(e.Index).ToString(), New Font("Segoe UI", 10, FontStyle.Regular), myBrush, e.Bounds.Left, e.Bounds.Y + 5, StringFormat.GenericDefault)
        End If
        ' If the ListBox has focus, draw a focus rectangle around  _  
        ' the selected item.
        e.DrawFocusRectangle()
    End Sub

    Private Sub SentQuotes_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged


        'clears sizes
        RemoveHandler ComboBox1.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
        ComboBox1.SelectedIndex = -1
        ComboBox1.Text = String.Empty
        ComboBox1.Items.Clear()

        For j As Integer = 0 To ComboBox1.Items.Count - 1
            ComboBox1.Items.Remove(j)
        Next

        'Clears the cells
        For j As Integer = 0 To DataGridView1.RowCount - 1
            For c As Integer = 0 To DataGridView1.Rows(j).Cells.Count - 1
                DataGridView1.Rows(j).Cells(c).Value = ""
            Next
        Next

        'cleras message
        RichTextBox1.Clear()
        'Changes Version of formula
        Label12.Text = "Not available"
        'Changes sent by label
        Label3.Text = ""




        'clears link to PDF
        urllink.Text = ""

        Dim foundRow As DataRow = ds.Tables("forms").Rows.Find(ListBox1.SelectedItem.ToString)
        If Not foundRow Is Nothing Then
            'Changes Version of formula
            If Not IsDBNull(foundRow("VersionID")) Then
                Label12.Text = foundRow("VersionID").ToString
            End If
            'Changes sent by label
            Label3.Text = foundRow("SentTo").ToString
            'Sets the message
            If Not IsDBNull(foundRow("Message")) Then
                RichTextBox1.Rtf = foundRow("Message")
            End If
            'Sets the link
            If Not IsDBNull(foundRow("Hyperlink")) Then
                urllink.Text = foundRow("Hyperlink")
            End If

            'gets the different sizes on this quote
            Dim query = (From size In ds.Tables("formsdet") Where size.Field(Of Integer)("emailid") = foundRow("emailid") Select size.Field(Of String)("sizename")).Distinct()
            AddHandler ComboBox1.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
            For Each s In query
                ComboBox1.Items.Add(s.ToString)
            Next

            If ComboBox1.Items.Count > 0 Then
                ComboBox1.SelectedIndex = 0

            End If
        End If




    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

        If ComboBox1.Items.Count > 0 Then
            DataGridView1.Rows.Clear()
            DataGridView1.Rows.Add()
            DataGridView1.Rows.Add()

            Dim foundRow As DataRow = ds.Tables("forms").Rows.Find(ListBox1.SelectedItem.ToString)

            Dim dv As DataView
            If Not ComboBox1.Text.Trim = "Bulk" Then
                dv = New DataView(ds.Tables("formsdet"), "emailid= " & foundRow("emailid") & " AND sizename= " & ComboBox1.Text & "", "ind ASC", DataViewRowState.CurrentRows)
            Else
                dv = New DataView(ds.Tables("formsdet"), "emailid= " & foundRow("emailid"), "ind ASC", DataViewRowState.CurrentRows)
            End If


            For j As Integer = 0 To dv.Count - 1
                DataGridView1.Rows(0).Cells(j).Value = dv.Item(j)("qty")
                DataGridView1.Rows(1).Cells(j).Value = dv.Item(j)("price")
            Next

            DataGridView1.Rows(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Rows(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End If

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click

        If String.IsNullOrEmpty(urllink.Text) Then
            MsgBox("The file is not on this computer", MessageBoxButtons.OK, "Message")
            Exit Sub
        End If


        ' Verify that the file exists.
        If System.IO.File.Exists(urllink.Text) = False Then
            MsgBox("File Not Found", MessageBoxButtons.OK, "Message")
        Else
            Process.Start(urllink.Text)
        End If



    End Sub
End Class