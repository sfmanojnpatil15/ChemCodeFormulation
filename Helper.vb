Imports System.Drawing.Printing
Imports System.Diagnostics
Imports System.IO
Imports MySql.Data.MySqlClient
Imports PdfiumViewer

Public Class Helper

    Shared prev = New Preview()
    Shared FilePath = AppDomain.CurrentDomain.BaseDirectory
    Shared Writer
    Shared IsAdmin As Boolean = False

    'Initialise Log Writer
    Public Shared Sub InitialiseLog()
        Dim FileName As String = FilePath + "Log\" + DateTime.Now.Date.ToString("dd-MM-yyyy")
        'Create FileStream and passed it to StreamWriter so that multiple instances can work with the same file.
        Dim FS = New FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
        Writer = New StreamWriter(FS)
    End Sub

    'Log Exception
    Public Shared Sub WriteLog(Ex As Exception)

        Dim StackTrace As New StackTrace(Ex, True)
        Dim frame = StackTrace.GetFrames().Last()
        Writer.WriteLine("Error:")
        Writer.WriteLine("Message: " + If(String.IsNullOrEmpty(Ex.Message), "", Ex.Message.ToString()))
        Writer.WriteLine("Line Number: " + If(String.IsNullOrEmpty(frame.GetFileLineNumber), "", frame.GetFileLineNumber.ToString()))
        Writer.WriteLine("Method Name: " + If(String.IsNullOrEmpty(frame.GetMethod.Name), "", frame.GetMethod.ToString()))
        Writer.WriteLine("Class Name: " + If(String.IsNullOrEmpty(frame.GetFileName), "", frame.GetFileName.ToString()))
        Writer.WriteLine("Time: " + DateTime.Now.ToString("HH:mm:ss"))
        Writer.WriteLine("------------------------------------------------------------------------------------")
        Writer.WriteLine()
        Writer.Flush()
    End Sub

    'Log Message
    Public Shared Sub WriteMessage(Ex As String)
        Writer.WriteLine("Error:")
        Writer.WriteLine("Message: " + Ex)
        Writer.WriteLine("------------------------------------------------------------------------------------")
        Writer.WriteLine()
        Writer.Flush()
    End Sub

    'Login to the Application
    Public Shared Function login(txtPassword As TextBox, txtUsername As TextBox, Fm As Form) As Boolean

        If txtPassword.Text = "" Or txtUsername.Text = "" Then
            MessageBox.Show("Please complete the required fields..", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Else

            Dim conn As New MySqlConnection()
            conn.ConnectionString = My.Settings.DBCon

            Try

                Dim sql As String = "SELECT * FROM UserList WHERE Username='" & txtUsername.Text & "' AND Password = '" & txtPassword.Text & "'"
                Dim sqlCom As New MySqlCommand(sql)

                'Open Database Connection
                sqlCom.Connection = conn
                conn.Open()

                Dim sqlRead As MySqlDataReader = sqlCom.ExecuteReader()

                If sqlRead.Read() Then
                    Dashboard.UserIDLabel.Text = sqlRead.Item(0)
                    Dashboard.Username.Text = txtUsername.Text
                    Dim IsUserOrAdmin = sqlRead("Type").ToString()
                    If (IsUserOrAdmin = "admin") Then
                        IsAdmin = True
                    End If

                    Dashboard.Show()
                    Fm.Hide()

                Else
                    MessageBox.Show("Username and Password do not match..", "Authentication Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    txtPassword.Text = ""
                    txtUsername.Text = ""

                    txtUsername.Focus()
                End If

            Catch ex As Exception
                MessageBox.Show("Failed to connect to Database..", "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                WriteLog(ex)
            End Try
        End If
        Return IsAdmin

    End Function

    'Get current Assembly version of the Application
    Public Shared Sub GetAssembly(lblVersion As Control, txtPassword As TextBox)
        Dim assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location)
        LoginForm.AssemblyVersion = "Version: " & fvi.FileVersion & ""
        lblVersion.Text = LoginForm.AssemblyVersion

        txtPassword.PasswordChar = "*"
    End Sub

    'Used to create an array of long Sentence or multiple word.
    Public Shared Function Wordwrap(ctl As Control, length As Int16) As List(Of String)
        Dim wordbreak = New List(Of String)
        Dim Text = ctl.Text.Split(" ")

        Dim Line As String

        For Each txt In Text

            Dim Temp As String = Line

            Line += txt
            Line += " "

            If Line.Length >= length Then
                Line = Temp

                wordbreak.Add(Line)
                Line = txt
            End If
        Next
        wordbreak.Add(Line)
        Return wordbreak
    End Function

    'Used to Preview PDF and image documents type
    Public Shared Sub DisplayDocuments(dgvAttachments As DataGridView)
        Try
            Dim FileExtension = dgvAttachments.SelectedRows(0).Cells("FileExtension").Value
            Dim tempDocxPath As String
            If FileExtension = ".pdf" Then

                Dim FileData = CType(dgvAttachments.SelectedRows(0).Cells("FileData").Value, Byte())

                Dim Stream = New MemoryStream(FileData)

                Dim viewer = New PdfiumViewer.PdfViewer()
                viewer.Dock = DockStyle.Fill
                viewer.ZoomMode = PdfViewerZoomMode.FitHeight

                viewer.Document = PdfiumViewer.PdfDocument.Load(Stream)

                prev.Controls.Add(viewer)
                prev.ShowDialog()
                prev.Controls.Remove(viewer)

            ElseIf FileExtension = ".jpeg" Or FileExtension = ".jpg" Or FileExtension = ".png" Then

                Dim ImageData = CType(dgvAttachments.SelectedRows(0).Cells("FileData").Value, Byte())

                Dim ImgPictureBox = New PictureBox()
                Dim LoadImage As Image

                Dim stream As New MemoryStream(ImageData)
                LoadImage = Image.FromStream(stream)

                ImgPictureBox.Image = LoadImage

                ImgPictureBox.Dock = DockStyle.Fill

                ImgPictureBox.SizeMode = PictureBoxSizeMode.Zoom

                prev.Height = ImgPictureBox.Height
                prev.Width = ImgPictureBox.Width
                prev.Controls.Add(ImgPictureBox)
                prev.ShowDialog()
                prev.Controls.Remove(ImgPictureBox)

            ElseIf FileExtension = ".doc" Or FileExtension = ".docx" Then

                Dim WordData As Byte() = CType(dgvAttachments.SelectedRows(0).Cells("FileData").Value, Byte())

                tempDocxPath = Path.Combine(Path.GetTempPath(), dgvAttachments.SelectedRows(0).Cells("filename").Value.ToString())
                File.WriteAllBytes(tempDocxPath, WordData)
                If File.Exists(tempDocxPath) Then
                    Process.Start("explorer.exe", tempDocxPath)
                End If

            Else
                MessageBox.Show($"Preview not available for {FileExtension} file type, Download file to view.", "Message", MessageBoxButtons.OK)
            End If


        Catch ex As Exception
            WriteLog(ex)
        Finally

        End Try
    End Sub

    ' Checkes weather there is any change to the dataset
    Public Shared Function IsSInitialtate(ds As DataSet) As Boolean
        Dim isChanged As Boolean = False
        For Each tbl As DataTable In ds.Tables
            For Each tblrow As DataRow In tbl.Rows
                If tblrow.RowState = DataRowState.Modified Then
                    For Each tblclm As DataColumn In tbl.Columns
                        Dim InitialVal As Object = tblrow(tblclm, DataRowVersion.Original)
                        Dim CurrentVal As Object = tblrow(tblclm, DataRowVersion.Current)

                        If IsDBNull(InitialVal) And IsDBNull(CurrentVal) Then
                            Continue For
                        End If

                        If tblclm.DataType.Equals(GetType(Double)) Or tblclm.DataType.Equals(GetType(Decimal)) Then
                            If (InitialVal - CurrentVal = 0.005) Or (CurrentVal - InitialVal = 0.005) Then
                                Continue For
                            End If
                        End If

                        Dim Type = tblclm.DataType
                        If Type.Name = "Decimal" And (Not IsDBNull(CurrentVal) And Not IsDBNull(InitialVal)) Then
                            CurrentVal = Math.Round(CurrentVal, 2)
                            InitialVal = Math.Round(InitialVal, 2)
                        End If


                        If IsDBNull(InitialVal) And IsDBNull(CurrentVal) Or (tblclm.ColumnName = "EnteredDate" Or tblclm.ColumnName = "UpdatedDate") Then
                            Continue For
                        End If

                        If (IsDBNull(InitialVal) And Not IsDBNull(CurrentVal)) Or (IsDBNull(CurrentVal) And Not IsDBNull(InitialVal)) Then
                            isChanged = True
                            Return isChanged
                            Exit Function
                        End If

                        If (IsDBNull(InitialVal) And Not IsDBNull(CurrentVal)) Or (Not IsDBNull(InitialVal) And IsDBNull(CurrentVal)) Or Not (InitialVal = CurrentVal) Then
                            isChanged = True
                            Return isChanged
                            Exit Function
                        End If
                    Next
                ElseIf tblrow.RowState = DataRowState.Added Or tblrow.RowState = DataRowState.Deleted Then
                    If tbl.TableName = "formulaF" Or tbl.TableName = "formula" Or tbl.TableName = "salesBagsTxns" Or tbl.TableName = "salestxns" Or tbl.TableName = "salesBoxTxns" Then
                        isChanged = True
                        Return isChanged
                        Exit Function

                    Else
                        Continue For
                    End If

                End If
            Next
        Next
        Return isChanged
    End Function
    Public Sub txtBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not (Char.IsDigit(CChar(CStr(e.KeyChar))) Or e.KeyChar = "." Or e.KeyChar = Convert.ToChar(8)) Then
            e.Handled = True
        End If

        Dim Ctl As TextBox = CType(sender, TextBox)
        If Ctl.SelectionStart = 0 AndAlso Ctl.SelectionLength = Ctl.Text.Length Then

        Else
            If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And (e.KeyChar <> "."c Or Ctl.Text.Contains(".")) Then
                e.Handled = True
            End If
        End If
        If e.KeyChar = ChrW(Keys.Enter) Then
            FormatTextBox(sender, e)
        End If
    End Sub
    Public Sub FormatTextBox(sender As Object, e As EventArgs)
        Dim Ctl As Control = CType(sender, Control)
        Dim PrintingCost = Ctl.Text.Split(".")
        If Not Ctl.Text.Contains(".") Then
            Ctl.Text = Ctl.Text + ".00"
        ElseIf Ctl.Text.EndsWith(".") Then
            Ctl.Text = Ctl.Text + "00"
        ElseIf PrintingCost(1).Length = 1 Then
            Ctl.Text = Ctl.Text + "0"
        ElseIf Ctl.Text = "." Then
            Ctl.Text = "0.00"
        End If
        If Ctl.Text.StartsWith(".") Then
            Ctl.Text = "0" + Ctl.Text
        End If
    End Sub
    Public Sub txtBox_LostFocus(sender As Object, e As EventArgs)
        FormatTextBox(sender, e)
    End Sub
    Public Sub txtBox_KeyPress_Number(sender As Object, e As KeyPressEventArgs)
        Try
            Dim Ctl As Control = CType(sender, Control)
            If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) Then
                e.Handled = True
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
End Class
