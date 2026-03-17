Imports System.Data.SqlClient
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class EditMaterial

    Dim helper As New Helper()
    Dim isPriceUpdate As Boolean = False
    Dim DbCon As New MySqlConnection
    Dim dbUp As New MySqlCommand
    Dim da As MySqlDataAdapter
    Dim ds As New DataSet
    Dim dsTierPricing As New DataSet

    Private Sub EditMaterial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAttachmentGrid()
        BindTierPricingDataSource()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim conn As New MySqlConnection()
        conn.ConnectionString = My.Settings.DBCon

        Try
            If String.IsNullOrEmpty(TextBox1.Text) Or String.IsNullOrEmpty(TextBox2.Text) Or String.IsNullOrEmpty(TextBox3.Text) Then
                MsgBox("All fields are required", MessageBoxButtons.OK, "Message")
            Else
                Dim sql As String = "UPDATE Material SET MaterialName=@MaterialName, Supplier=@Supplier, Price=@Price, LatestDate =@LatestDate,ComponentCode=@ComponentCode, Category=@Category,CasePack=@CasePack,VendorCode=@VendorCode WHERE MaterialID = @MaterialID"
                If isPriceUpdate Then
                    sql = "UPDATE Material SET MaterialName=@MaterialName, Supplier=@Supplier, Price=@Price, LatestDate =@LatestDate, ComponentCode=@ComponentCode, Category=@Category,PriceUpdateDate=@PriceUpdateDate,CasePack=@CasePack,VendorCode=@VendorCode WHERE MaterialID = @MaterialID"
                End If


                Dim sqlCom As New MySqlCommand(sql)

                sqlCom.Parameters.AddWithValue("@MaterialName", TextBox1.Text)
                sqlCom.Parameters.AddWithValue("@Supplier", TextBox2.Text)
                sqlCom.Parameters.AddWithValue("@Price", TextBox3.Text)
                sqlCom.Parameters.AddWithValue("@LatestDate", DateTime.Now.ToString("yyyy-MM-dd"))
                sqlCom.Parameters.AddWithValue("@MaterialID", MaterialID.Text)
                sqlCom.Parameters.AddWithValue("@Category", txtcategoreyEditMaterial.Text)
                sqlCom.Parameters.AddWithValue("@ComponentCode", txtComponentCode.Text)
                sqlCom.Parameters.AddWithValue("@CasePack", txtCasePack.Text)
                sqlCom.Parameters.AddWithValue("@VendorCode", txtVendorCode.Text)
                If isPriceUpdate Then
                    sqlCom.Parameters.AddWithValue("@PriceUpdateDate", DateTime.Now.ToString("yyyy-MM-dd"))
                End If

                'Open Database Connection
                sqlCom.Connection = conn
                conn.Open()

                Dim sqlRead As MySqlDataReader = sqlCom.ExecuteReader()
                sqlRead.Close()

                If dsTierPricing.HasChanges Then
                    InsertUpdateTierPricingRecords()
                End If


                For Each row As DataRow In ds.Tables("materialattachments").Rows
                    If row.RowState = DataRowState.Deleted Then
                        Dim DeletedId As Integer = Convert.ToInt32(row("Id", DataRowVersion.Original))
                        Dim deleteQuery As New MySqlCommand("Delete from materialattachments where Id= @Id", conn)
                        deleteQuery.Parameters.AddWithValue("@Id", DeletedId)
                        deleteQuery.ExecuteNonQuery()
                    Else
                        If row.RowState = DataRowState.Added Then
                            Dim insertCmd As New MySqlCommand("INSERT INTO materialattachments (FileName, FileData, FileExtension, UploadedDate, FileType, MaterialId) VALUES (@FileName, @FileData, @FileExtension, @UploadedDate, @FileType, @MaterialId)", conn)
                            insertCmd.Parameters.AddWithValue("@FileName", row("FileName"))
                            insertCmd.Parameters.AddWithValue("@FileData", row("FileData"))
                            insertCmd.Parameters.AddWithValue("@FileExtension", row("FileExtension"))
                            insertCmd.Parameters.AddWithValue("@UploadedDate", row("UploadedDate"))
                            insertCmd.Parameters.AddWithValue("@FileType", row("FileType"))
                            insertCmd.Parameters.AddWithValue("@MaterialId", MaterialID.Text)
                            'insertCmd.Connection = conn
                            insertCmd.ExecuteNonQuery()
                        End If

                    End If

                Next
                Me.Close()

                Dashboard.PopulateMaterial()
                isPriceUpdate = False
            End If

            'Dashboard.DataGridView2.CurrentRow.DataBoundItem("MaterialName") = TextBox1.Text
            'Dashboard.DataGridView2.CurrentRow.DataBoundItem("Supplier") = TextBox2.Text
            'Dashboard.DataGridView2.CurrentRow.DataBoundItem("Price") = TextBox3.Text

        Catch ex As Exception
            MessageBox.Show("Failed to connect to Database..", "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub BtnDownload_Click(sender As Object, e As EventArgs) Handles BtnDownload.Click
        Try
            If dgvAttachments.SelectedRows.Count > 0 Then

                Dim FileName As String = dgvAttachments.SelectedRows(0).Cells("FileName").Value.ToString()
                Dim baseName As String = Path.GetFileNameWithoutExtension(FileName)
                Dim FileData As Byte() = CType(dgvAttachments.SelectedRows(0).Cells("FileData").Value, Byte())
                Dim FileExt As String = dgvAttachments.SelectedRows(0).Cells("FileExtension").Value.ToString()
                Dim FileType As String = dgvAttachments.SelectedRows(0).Cells("FileType").Value.ToString()
                Dim downloadsPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
                Dim FullPath As String = Path.Combine(downloadsPath, FileName)
                Dim Counter As Integer = 1
                While System.IO.File.Exists(FullPath)
                    FullPath = Path.Combine(downloadsPath, baseName & "(" & Counter & ")" & FileExt)

                    Counter += 1
                End While
                '  Dim filePath As String = dataGridView1.SelectedRows(0).Cells("FilePathColumn").Value.ToString()
                File.WriteAllBytes(FullPath, FileData)
            Else
                MessageBox.Show("Please select an attachment to download", "Download", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub dgvAttachments_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAttachments.CellClick
        '  lblAttachmentId.Text = dgvAttachments.CurrentRow.DataBoundItem("Id")
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Try
            If dgvAttachments.SelectedRows.Count > 0 Then
                Dim selectedRowIndex As Integer = dgvAttachments.SelectedRows(0).Index
                If selectedRowIndex >= 0 AndAlso selectedRowIndex < ds.Tables("materialattachments").Rows.Count Then
                    ' Mark the row for deletion
                    ds.Tables("materialattachments").Rows(selectedRowIndex).Delete()
                End If
            Else
                MessageBox.Show("Please select an attachment to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub



    Private Sub btnUploadAttachment_Click(sender As Object, e As EventArgs) Handles btnUploadAttachment.Click
        Using ofd As New OpenFileDialog()
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim FileName As String = Path.GetFileName(ofd.FileName)
                Dim fileBytes As Byte() = File.ReadAllBytes(ofd.FileName)
                Dim fileExt As String = Path.GetExtension(FileName).ToLower()
                Dim mimeType = GetMimeType(FileName)

                Dim NewRow As DataRow = ds.Tables("materialattachments").NewRow
                NewRow("FileName") = FileName
                NewRow("FileData") = fileBytes
                NewRow("FileExtension") = fileExt
                NewRow("Uploadeddate") = DateTime.Now
                NewRow("FileType") = mimeType
                NewRow("MaterialId") = 0
                ds.Tables("materialattachments").Rows.Add(NewRow)
            End If
        End Using
    End Sub
    Private Function GetMimeType(filePath As String) As String
        Dim mimeType As String = "application/octet-stream" ' Default if unknown
        Dim ext As String = Path.GetExtension(filePath).ToLower()

        Dim regKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext)
        If regKey IsNot Nothing AndAlso regKey.GetValue("Content Type") IsNot Nothing Then
            mimeType = regKey.GetValue("Content Type").ToString()
        End If

        Return mimeType
    End Function
    Private Sub dgvAttachments_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAttachments.CellDoubleClick
        Helper.DisplayDocuments(dgvAttachments)
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        isPriceUpdate = True
    End Sub

    Private Sub MaterialTab_TabIndexChanged(sender As Object, e As EventArgs) Handles MaterialTab.TabIndexChanged
        If MaterialTab.SelectedTab.Name = "TabPage2" Then

        End If

    End Sub

    Private Sub LoadAttachmentGrid()
        DbCon.ConnectionString = My.Settings.DBCon
        MaterialTab.SelectedIndex = 0
        Dim attachmentsQuery As String = "SELECT * FROM materialattachments where MaterialId = " & MaterialID.Text
        Dim attachmentsAdapter As New MySqlDataAdapter(attachmentsQuery, DbCon)

        If ds.Tables.Contains("materialattachments") Then
            ds.Tables("materialattachments").Clear()
        End If

        attachmentsAdapter.Fill(ds, "materialattachments")
        dgvAttachments.DataSource = ds.Tables("materialattachments")
    End Sub

    Private Sub BindTierPricingDataSource()
        Try
            DbCon = New MySqlConnection()
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()

            dbUp = New MySqlCommand()
            dbUp.CommandType = CommandType.Text
            dbUp.CommandText = $"Select * from tblMaterialTierPicing where MaterialID={MaterialID.Text}"
            dbUp.Connection = DbCon

            Dim TierPricing As New MySqlDataAdapter(dbUp)

            If dsTierPricing.Tables.Contains("TierPricing") Then
                dsTierPricing.Tables.Remove("TierPricing")
            End If

            TierPricing.Fill(dsTierPricing, "TierPricing")
            dsTierPricing.AcceptChanges()

            grdMaterialTierPricing.DataSource = dsTierPricing.Tables("TierPricing")

        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
            DbCon.Close()
        End Try
    End Sub

    Private Sub txtAddNewLineTierPricing_Click(sender As Object, e As EventArgs) Handles txtAddNewLineTierPricing.Click
        Try
            Dim row = dsTierPricing.Tables("TierPricing").NewRow()
            row("MaterialID") = MaterialID.Text
            row("QTY") = 0
            row("Cost") = 0.00
            row("AddedOn") = DateTime.Now.Date
            row("UpdatedOn") = DateTime.Now.Date

            dsTierPricing.Tables("TierPricing").Rows.Add(row)
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub InsertUpdateTierPricingRecords()
        Try
            DbCon = New MySqlConnection()
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()

            da = New MySqlDataAdapter()

            Dim SelectCommand As New MySqlCommand("SELECT * FROM tblMaterialTierPicing WHERE MaterialID=" & MaterialID.Text, DbCon)
            da.SelectCommand = SelectCommand

            Dim InsertCommand As New MySqlCommand()
            InsertCommand.Connection = DbCon
            InsertCommand.CommandType = CommandType.Text
            InsertCommand.CommandText = "Insert into tblMaterialTierPicing(MaterialID,QTY,Cost,AddedOn,UpdatedOn) Values(
            @MaterialID,@QTY,@Cost,@AddedOn,@UpdatedOn)"

            InsertCommand.Parameters.Add("@MaterialID", MySqlDbType.Int16, 10, "MaterialID")
            InsertCommand.Parameters.Add("@QTY", MySqlDbType.Int16, 10, "QTY")
            InsertCommand.Parameters.Add("@Cost", MySqlDbType.Double, 10, "Cost")
            InsertCommand.Parameters.Add("@AddedOn", MySqlDbType.DateTime, 10, "AddedOn")
            InsertCommand.Parameters.Add("@UpdatedOn", MySqlDbType.DateTime, 10, "UpdatedOn")

            da.InsertCommand = InsertCommand

            Dim UpdateCommand As New MySqlCommand()
            UpdateCommand.Connection = DbCon
            UpdateCommand.CommandType = CommandType.Text
            UpdateCommand.CommandText = "UPDATE tblMaterialTierPicing SET QTY=@QTY,Cost=@Cost,AddedOn=@AddedOn,UpdatedOn=@UpdatedOn where ID=@ID"

            UpdateCommand.Parameters.Add("@QTY", MySqlDbType.Int16, 10, "QTY")
            UpdateCommand.Parameters.Add("@Cost", MySqlDbType.Double, 10, "Cost")
            UpdateCommand.Parameters.Add("@AddedOn", MySqlDbType.DateTime, 10, "AddedOn")
            UpdateCommand.Parameters.Add("@UpdatedOn", MySqlDbType.DateTime, 10, "UpdatedOn")
            UpdateCommand.Parameters.Add("@ID", MySqlDbType.Int16, 10, "ID")

            da.UpdateCommand = UpdateCommand

            Dim DeleteCommand As New MySqlCommand("Delete from tblMaterialTierPicing where ID=@ID")
            DeleteCommand.Connection = DbCon
            DeleteCommand.Parameters.Add("ID", MySqlDbType.Int32, 10, "ID")

            da.DeleteCommand = DeleteCommand

            da.Update(dsTierPricing, "TierPricing")
            dsTierPricing.AcceptChanges()

        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
            DbCon.Close()
        End Try
    End Sub

    Private Sub grdMaterialTierPricing_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles grdMaterialTierPricing.EditingControlShowing
        Dim txt = CType(e.Control, TextBox)

        RemoveHandler txt.KeyPress, AddressOf helper.txtBox_KeyPress_Number
        RemoveHandler txt.KeyPress, AddressOf helper.txtBox_KeyPress
        RemoveHandler txt.LostFocus, AddressOf helper.txtBox_LostFocus

        If grdMaterialTierPricing.CurrentCell.ColumnIndex = 2 Then
            AddHandler txt.KeyPress, AddressOf helper.txtBox_KeyPress_Number
        ElseIf grdMaterialTierPricing.CurrentCell.ColumnIndex = 3 Then
            AddHandler txt.KeyPress, AddressOf helper.txtBox_KeyPress
            AddHandler txt.LostFocus, AddressOf helper.txtBox_LostFocus
        End If

    End Sub

End Class