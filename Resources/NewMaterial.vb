Imports System.IO
Imports MySql.Data.MySqlClient
Public Class NewMaterial

    Dim helper As New Helper()
    Dim DbCon As New MySqlConnection
    Dim dbUp As New MySqlCommand
    Dim da As MySqlDataAdapter
    Dim ds As New DataSet
    Dim dsTierPricing As New DataSet


    Private Sub NewMaterial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            cmbUOM.SelectedIndex = 0
            MaterialTab.SelectedIndex = 0

            BindGridWithDataSource()

            AddHandler txtPrice.KeyPress, AddressOf helper.txtBox_KeyPress
            AddHandler txtPrice.LostFocus, AddressOf helper.txtBox_LostFocus

            AddHandler txtCasePack.KeyPress, AddressOf helper.txtBox_KeyPress
            AddHandler txtCasePack.LostFocus, AddressOf helper.txtBox_LostFocus

        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
            DbCon.Close()
        End Try

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim manterialName As String = txtMaterialName.Text.Trim
        Dim sp As String = txtSupplier.Text.Trim

        'checks if material already exist
        Dim materialexist As Boolean = False
        DbCon = New MySqlConnection(My.Settings.DBCon)

        'ADAS-GT
        Try
            DbCon.Open()
            Dim query = "SELECT MaterialName FROM Material WHERE MaterialName=@MaterialName AND Supplier=@Supplier ORDER BY MaterialName"
            Dim sqlCommand As MySqlCommand = New MySqlCommand(query, DbCon)
            sqlCommand.Parameters.AddWithValue("@MaterialName", manterialName)
            sqlCommand.Parameters.AddWithValue("@Supplier", sp)
            Dim sqlReader As MySqlDataReader = sqlCommand.ExecuteReader()
            While sqlReader.Read()
                materialexist = True
            End While
            sqlReader.Close()
            sqlCommand.Dispose()

        Catch ex As Exception
        Finally
            DbCon.Close()
        End Try

        If materialexist = True Then
            MessageBox.Show("Material " + manterialName + " already exist for Supplier " + sp + ".", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If



        Try
            If String.IsNullOrEmpty(txtCategory.Text) Or String.IsNullOrEmpty(txtMaterialName.Text) Or String.IsNullOrEmpty(txtSupplier.Text) Or String.IsNullOrEmpty(txtPrice.Text) Or String.IsNullOrEmpty(cmbUOM.Text) Then
                MessageBox.Show("Please fill all fields in the Material tab", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                MaterialTab.SelectedIndex = 0
            Else
                Dim addMaterial = "INSERT INTO Material(ComponentCode,MaterialName,Supplier,Price,LatestDate,PriceUpdateDate,Category,UOM,IsManual,CasePack,VendorCode) values(@ComponentCode,@MaterialName,@Supplier,@Price,@LatestDate,@PriceUpdateDate,@Category,@UOM,@IsManual,@CasePack,@VendorCode);SELECT LAST_INSERT_ID();"

                Dim mySqlCommand = New MySqlCommand(addMaterial)

                mySqlCommand.Parameters.AddWithValue("@ComponentCode", txtComponentCode.Text)
                mySqlCommand.Parameters.AddWithValue("@MaterialName", txtMaterialName.Text)
                mySqlCommand.Parameters.AddWithValue("@Supplier", txtSupplier.Text)
                mySqlCommand.Parameters.AddWithValue("@Price", txtPrice.Text)
                mySqlCommand.Parameters.AddWithValue("@LatestDate", DateTime.Now.ToString("yyyy-MM-dd"))
                mySqlCommand.Parameters.AddWithValue("@PriceUpdateDate", DateTime.Now.ToString("yyyy-MM-dd"))
                mySqlCommand.Parameters.AddWithValue("@Category", txtCategory.Text)
                mySqlCommand.Parameters.AddWithValue("@UOM", cmbUOM.Text)
                mySqlCommand.Parameters.AddWithValue("@IsManual", 1)
                mySqlCommand.Parameters.AddWithValue("@CasePack", txtCasePack.Text)
                mySqlCommand.Parameters.AddWithValue("@VendorCode", txtVendorCode.Text)

                mySqlCommand.Connection = DbCon
                DbCon.Open()

                'Dim ExecuteQuery = mySqlCommand.ExecuteNonQuery

                Dim MaterialId As Integer = Convert.ToInt32(mySqlCommand.ExecuteScalar())

                If dsTierPricing.HasChanges() Then
                    For Each row As DataRow In dsTierPricing.Tables("TierPricing").Rows
                        row("MaterialID") = MaterialId
                    Next

                    InsertTierPricingRecords(MaterialId)
                End If



                For Each row As DataRow In ds.Tables("materialattachments").Rows
                    If row.RowState = DataRowState.Deleted Then
                        Dim DeletedId As Integer = Convert.ToInt32(row("Id", DataRowVersion.Original))
                        Dim deleteQuery As New MySqlCommand("Delete from materialattachments where Id= @Id")
                        deleteQuery.Parameters.AddWithValue("@Id", DeletedId)
                        deleteQuery.ExecuteNonQuery()

                    ElseIf row.RowState = DataRowState.Added Then

                        Dim insertCmd As New MySqlCommand("INSERT INTO materialattachments (FileName, FileData, FileExtension, UploadedDate, FileType, MaterialId) VALUES (@FileName, @FileData, @FileExtension, @UploadedDate, @FileType, @MaterialId)", DbCon)
                        insertCmd.Parameters.AddWithValue("@FileName", row("FileName"))
                        insertCmd.Parameters.AddWithValue("@FileData", row("FileData"))
                        insertCmd.Parameters.AddWithValue("@FileExtension", row("FileExtension"))
                        insertCmd.Parameters.AddWithValue("@UploadedDate", row("UploadedDate"))
                        insertCmd.Parameters.AddWithValue("@FileType", row("FileType"))
                        insertCmd.Parameters.AddWithValue("@MaterialId", MaterialId)

                        insertCmd.Connection = DbCon
                        insertCmd.ExecuteNonQuery()
                    End If

                Next
                MessageBox.Show("Material Added", "Message", MessageBoxButtons.OK)
                Formulator2.MaterialAdded = True
                Me.Close()

                Dashboard.PopulateMaterial()
                Formulator2.PopulateMaterial()

                DbCon.Close()

            End If



        Catch ex As Exception
            MessageBox.Show("Can not open Connection !")
        Finally

        End Try


    End Sub

    Private Sub NewMaterial_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        txtComponentCode.Text = ""
        txtMaterialName.Text = ""
        txtSupplier.Text = ""
        txtPrice.Text = ""
        txtVendorCode.Text = ""
        txtCasePack.Text = ""
        cmbUOM.SelectedText = ""
        txtCategory.Text = "RM"
    End Sub

    Private Sub btnUploadAttachment_Click(sender As Object, e As EventArgs) Handles btnUploadAttachment.Click
        Try
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
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
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
                MessageBox.Show("Please select an attachment to download.", "Download", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub dgvAttachments_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAttachments.CellClick
        'If e.RowIndex > 0 Then

        '    Dim rowindex = e.RowIndex
        '    Dim row As DataGridViewRow = e.dgvAttachments.Rows[rowindex]
        '    End If
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

        End Try
    End Sub
    Private Sub dgvAttachments_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAttachments.CellDoubleClick
        Helper.DisplayDocuments(dgvAttachments)
    End Sub

    Private Sub BindGridWithDataSource()
        Try
            BindTierPricingDataSource()
            BindAttachmentDataSource()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub BindTierPricingDataSource()
        Try
            DbCon = New MySqlConnection()
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()

            dbUp = New MySqlCommand()
            dbUp.CommandType = CommandType.Text
            dbUp.CommandText = "Select * from tblMaterialTierPicing where 1=2"
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

    Private Sub BindAttachmentDataSource()
        Try
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            dbUp.Connection = DbCon
            dbUp.CommandType = CommandType.Text
            Dim attachmentsQuery As String = "SELECT * FROM materialattachments WHERE 1=0"
            Dim attachmentsAdapter As New MySqlDataAdapter(attachmentsQuery, DbCon)
            If ds.Tables.Contains("materialattachments") Then
                ds.Tables("materialattachments").Clear()
            End If
            attachmentsAdapter.Fill(ds, "materialattachments")

            'Dim emptyAttachmentsTable As DataTable = ds.Tables("materialattachments").Clone()
            attachmentsAdapter.Fill(ds, "materialattachments")
            dgvAttachments.DataSource = ds.Tables("materialattachments")
        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
            DbCon.Close()
        End Try
    End Sub

    Private Sub txtAddNewLineTierPricing_Click(sender As Object, e As EventArgs) Handles txtAddNewLineTierPricing.Click
        Try
            Dim row = dsTierPricing.Tables("TierPricing").NewRow()

            row("QTY") = 0
            row("Cost") = 0.00
            row("AddedOn") = DateTime.Now.Date
            row("UpdatedOn") = DateTime.Now.Date

            dsTierPricing.Tables("TierPricing").Rows.Add(row)
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub InsertTierPricingRecords(ID As Int64)
        Try
            DbCon = New MySqlConnection()
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()

            da = New MySqlDataAdapter()
            Dim InsertCommand As New MySqlCommand()
            Dim SelectCommand As New MySqlCommand("SELECT * FROM tblMaterialTierPicing WHERE MaterialID=" & ID, DbCon)
            da.SelectCommand = SelectCommand

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