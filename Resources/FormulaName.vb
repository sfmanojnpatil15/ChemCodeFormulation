Imports MySql.Data.MySqlClient
Public Class FormulaName

    Dim con As New MySqlConnection
    Dim cmd As New MySqlCommand
    Dim da As MySqlDataAdapter
    Dim ds As New DataSet
    Dim term As New DataTable()
    Public Shared Terms As New Dictionary(Of String, String)

    Private Sub me_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub
    Private Sub FormulaName_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        con.ConnectionString = My.Settings.DBCon

        Try

            con = New MySqlConnection()
            con.ConnectionString = My.Settings.DBCon
            con.Open()
            cmd = New MySqlCommand()
            cmd.CommandType = CommandType.Text
            cmd.Connection = con
            cmd.CommandText = "select * from terms"

            da = New MySqlDataAdapter(cmd)
            da.Fill(term)
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Helper.WriteLog(ex)
            MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        Finally
            con.Close()
        End Try

        AddHandler savebutton.Click, AddressOf save




    End Sub
    Private Sub save()

        'Trims the name of the formula
        FormulanameBox.Text = FormulanameBox.Text.Trim
        If String.IsNullOrEmpty(FormulanameBox.Text) Then
            FormulanameBox.Focus()
            Exit Sub
        End If
        Dim sqlstring As String = "SELECT FormulaName FROM Formulas WHERE FormulaName = @FormulaName1"
        Using selectcommand As New MySqlCommand(sqlstring, con)
            con.Open()
            selectcommand.Parameters.AddWithValue("@FormulaName1", FormulanameBox.Text)
            Dim reader As MySqlDataReader = selectcommand.ExecuteReader()
            If reader.HasRows = True Then
                FormulanameBox.Focus()
                reader.Close()
                con.Close()
                MsgBox("This formula exist already", MessageBoxButtons.OK, "Message")
                Exit Sub
            End If
            reader.Close()
            con.Close()
        End Using


        Dim newFormID As Integer
        If Label1.Text = 0 Then

            Dim query2 As String = "Select @@Identity"
            sqlstring = "INSERT INTO Formulas (FormulaName, FormulaType, EnteredBy, EnteredDate, PackagingFormat, ServingSize,CustomerName, SalesRepId,Contact,ServingWeight, IsInactive,LabTestingCostPolicy,QuoteExpiryDisclaimer,Message,OtherIngredients,FormulaNameDetail) VALUES (@FormulaName1, @FormulaType, " & Dashboard.UserIDLabel.Text & ", @EnteredDate, @PackagingFormat, @ServingSize,@CustomerName,@SalesRepId,@Contact, @ServingWeight, 0,@LabTestingCostPolicy,@QuoteExpiryDisclaimer,@Message,@OtherIngredients,@FormulaNameDetail)"
            Using insertcommand As New MySqlCommand(sqlstring, con)
                con.Open()
                insertcommand.Parameters.AddWithValue("@FormulaName1", FormulanameBox.Text)
                insertcommand.Parameters.AddWithValue("@FormulaType", Label3.Text)
                insertcommand.Parameters.AddWithValue("@EnteredDate", DateTime.Now)
                insertcommand.Parameters.AddWithValue("@PackagingFormat", Label4.Text)
                insertcommand.Parameters.AddWithValue("@ServingSize", Label5.Text)
                insertcommand.Parameters.AddWithValue("@ServingWeight", Label6.Text)
                insertcommand.Parameters.AddWithValue("@CustomerName", lblCustName.Text)
                insertcommand.Parameters.AddWithValue("@Contact", lblContact.Text)
                insertcommand.Parameters.AddWithValue("@SalesRepId", lblSalesRep.Text)
                insertcommand.Parameters.AddWithValue("@FormulaNameDetail", FormulanameBox.Text)
                insertcommand.Parameters.AddWithValue("@LabTestingCostPolicy", term.Rows(0)("LabTestingCostPolicy"))
                insertcommand.Parameters.AddWithValue("@QuoteExpiryDisclaimer", term.Rows(0)("QuoteExpiryDisclaimer"))
                insertcommand.Parameters.AddWithValue("@Message", term.Rows(0)("Message"))

                If Label3.Text = "Powder" Then
                    insertcommand.Parameters.AddWithValue("@OtherIngredients", term.Rows(0)("OtherIngredientsPowder"))
                    'Terms.Add("",)
                    Formulator2.Terms("Other Ingredients") = term.Rows(0)("OtherIngredientsPowder")
                ElseIf Label3.Text = "Capsule" Then
                    insertcommand.Parameters.AddWithValue("@OtherIngredients", term.Rows(0)("OtherIngredientsCapsule"))
                    Formulator2.Terms("Other Ingredients") = term.Rows(0)("OtherIngredientsCapsule")
                Else
                    insertcommand.Parameters.AddWithValue("@OtherIngredients", term.Rows(0)("OtherIngredientsTablet"))
                    Formulator2.Terms("Other Ingredients") = term.Rows(0)("OtherIngredientsTablet")
                End If


                Formulator2.Terms("LabTestingCostPolicy") = term.Rows(0)("LabTestingCostPolicy")
                Formulator2.Terms("QuoteExpiryDisclaimer") = term.Rows(0)("QuoteExpiryDisclaimer")
                Formulator2.Terms("Message") = term.Rows(0)("Message")

                insertcommand.ExecuteNonQuery()
                insertcommand.CommandText = query2
                newFormID = Convert.ToInt32(insertcommand.ExecuteScalar())
                con.Close()

            End Using

            Try
                'Dashboard.ds.Tables(0).Rows.Add(newFormID, FormulanameBox.Text, Label3.Text, DateTime.Now, DateTime.Now)
                Dashboard.PopulateFormulas()
            Catch ex As System.Exception
                Console.WriteLine(ex.Message)

            End Try



            Dim FormulaID As Label

            For Each FRM As Form In System.Windows.Forms.Application.OpenForms
                If FRM.Name = Label2.Text Then

                    FRM.Text = Me.FormulanameBox.Text & "      -       GT Formulator   -   " & DateTime.Now.Year
                    FormulaID = CType(FRM.Controls("FormulaID"), Label)

                    FormulaID.Text = newFormID

                    FRM.Tag = newFormID
                    FRM.Name = Me.FormulanameBox.Text
                End If
            Next
        Else
            'Updates item
            con = New MySqlConnection(My.Settings.DBCon)
            Try
                con.Open()
                Dim command As MySqlCommand = New MySqlCommand("UPDATE Formulas SET 
                FormulaName ='" & FormulanameBox.Text & "' 
                , UpdatedDate ='" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "'
                WHERE FormulaID =" & Label1.Text & "", con)
                command.ExecuteNonQuery()
                Dim rowsToUpdate As DataRow() = Dashboard.ds.Tables(0).Select("FormulaID = " & Label1.Text)
                If rowsToUpdate.Length > 0 Then
                    ' Update the first matching row
                    rowsToUpdate(0)("FormulaName") = FormulanameBox.Text

                Else
                    ' Handle the case where the row is not found
                    MessageBox.Show("Row with FormID " & newFormID & " not found.")
                End If

                command.Dispose()
            Catch ex As Exception
                MsgBox("Can not open connection ! ", MessageBoxButtons.OK, "Error")
            End Try

            For Each FRM As Form In System.Windows.Forms.Application.OpenForms
                If FRM.Tag = Label1.Text Then
                    FRM.Text = Me.FormulanameBox.Text & "      -       GT Formulator   -   " & DateTime.Now.Year
                    FRM.Name = Me.FormulanameBox.Text
                End If
            Next

        End If

        Formulator2.IsSaved = True
        Formulator2.Button9.Enabled = True
        Formulator2.btnDeleteFormulaVersion.Enabled = True
        Me.Close()

    End Sub
    Private Sub FormulanameBox_KeyPress(sender As Object, e As KeyEventArgs)
        If e.KeyData = Keys.Enter Then
            save()
        End If
    End Sub
    Public Sub Me_Close(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
    Private Sub FormulanameBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles FormulanameBox.KeyPress
        If e.KeyChar = "/" Or e.KeyChar = "\" Or e.KeyChar = ":" Or e.KeyChar = "*" Or e.KeyChar = "?" Or e.KeyChar = """" Or e.KeyChar = "'" Or e.KeyChar = "<" Or e.KeyChar = ">" Or e.KeyChar = "|" Then
            Label2.Visible = True
            e.Handled = True
        End If
    End Sub
    Private Sub FormulaName_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Formulator2.ContinueSaving = False
    End Sub

End Class