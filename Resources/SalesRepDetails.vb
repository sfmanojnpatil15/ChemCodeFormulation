Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Public Class SalesRepDetails

    Dim mySqlConnection As MySqlConnection = New MySqlConnection

    Private Sub lblSave_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub SalesRepDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'txtSalesRepId.Text = Dashboard.SaleRepId

            If Not String.IsNullOrEmpty(txtSalesRepId.Text) Then
                mySqlConnection.ConnectionString = My.Settings.DBCon
                mySqlConnection.Open()

                Dim query = String.Format("SELECT * FROM salesreps WHERE ID ='{0}'", txtSalesRepId.Text.Trim())
                Dim mySqlCommand = New MySqlCommand(query, mySqlConnection)

                Dim dataReader As MySqlDataReader = mySqlCommand.ExecuteReader
                If dataReader.Read() Then
                    txtSalesRepId.Enabled = False
                    txtLastName.Text = If(IsDBNull(dataReader("LastName")), "", dataReader("LastName"))
                    txtFirstName.Text = If(IsDBNull(dataReader("FirstName")), "", dataReader("FirstName"))
                    txtPhone.Text = If(IsDBNull(dataReader("Phone")), "", dataReader("Phone"))
                    txtEmail.Text = If(IsDBNull(dataReader("Email")), "", dataReader("Email"))
                End If

                dataReader.Close()


            End If
        Catch ex As Exception

        Finally
            mySqlConnection.Close()
        End Try

    End Sub

    Private Sub SalesRepDetails_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        txtSalesRepId.Enabled = True
        txtSalesRepId.Text = ""
        Me.Text = "New Sales Rep"
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtEmail.Text = ""
        txtPhone.Text = ""
    End Sub

    Private Sub btnSaveSalesRep_Click(sender As Object, e As EventArgs) Handles btnSaveSalesRep.Click
        Dim SalesRepExist As Boolean = False

        Try
            If String.IsNullOrEmpty(txtFirstName.Text) Or String.IsNullOrEmpty(txtLastName.Text) Or String.IsNullOrEmpty(txtEmail.Text) Or String.IsNullOrEmpty(txtPhone.Text) Or String.IsNullOrEmpty(txtSalesRepId.Text) Then
                MessageBox.Show("All fields are required", "Message", MessageBoxButtons.OK)
            Else
                Dim EmailPattern As String = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                Dim isEmail As Boolean = Regex.IsMatch(txtEmail.Text, EmailPattern)
                If Not isEmail Then
                    MessageBox.Show("Enter valid Email ID", "Message", MessageBoxButtons.OK)
                    Exit Sub
                End If
                mySqlConnection.ConnectionString = My.Settings.DBCon
                mySqlConnection.Open()

                Dim query = String.Format("SELECT * FROM salesreps WHERE ID ='{0}'", txtSalesRepId.Text.Trim())
                Dim mySqlCommand = New MySqlCommand(query, mySqlConnection)

                Dim dataReader As MySqlDataReader = mySqlCommand.ExecuteReader
                If dataReader.Read Then
                    SalesRepExist = True
                End If
                dataReader.Close()

                If Dashboard.IsEdit = True Then
                    If SalesRepExist Then

                        Dim UpdateQuery = "UPDATE salesreps SET FirstName=@FirstName,LastName=@LastName,Phone=@Phone,Email=@Email where ID=@ID"
                        mySqlCommand = New MySqlCommand(UpdateQuery, mySqlConnection)
                        mySqlCommand.Parameters.AddWithValue("@FirstName", txtFirstName.Text)
                        mySqlCommand.Parameters.AddWithValue("@LastName", txtLastName.Text)
                        mySqlCommand.Parameters.AddWithValue("@Phone", txtPhone.Text)
                        mySqlCommand.Parameters.AddWithValue("@Email", txtEmail.Text)
                        mySqlCommand.Parameters.AddWithValue("@ID", txtSalesRepId.Text)

                        mySqlCommand.ExecuteNonQuery()
                        Me.Close()

                        Dashboard.PopulateSalesRep()
                    Else
                        MessageBox.Show("User not found!", "Message", MessageBoxButtons.OK)
                    End If


                ElseIf SalesRepExist Then
                    MessageBox.Show("Sales Rep Id already exists", "Message", MessageBoxButtons.OK)
                Else
                    Dim InsertQuery = "INSERT INTO salesreps(ID,FirstName,LastName,Phone,Email) VALUES(@ID,@FirstName,@LastName,@Phone,@Email)"
                    mySqlCommand = New MySqlCommand(InsertQuery)
                    mySqlCommand.Connection = mySqlConnection

                    mySqlCommand.Parameters.AddWithValue("@ID", txtSalesRepId.Text)
                    mySqlCommand.Parameters.AddWithValue("@FirstName", txtFirstName.Text)
                    mySqlCommand.Parameters.AddWithValue("@LastName", txtLastName.Text)
                    mySqlCommand.Parameters.AddWithValue("@Phone", txtPhone.Text)
                    mySqlCommand.Parameters.AddWithValue("@Email", txtEmail.Text)

                    mySqlCommand.ExecuteNonQuery()
                    Me.Close()

                    Dashboard.PopulateSalesRep()
                End If
            End If




        Catch ex As Exception
            MessageBox.Show("Failed to connect to Database!", "Message", MessageBoxButtons.OK)
        Finally
            mySqlConnection.Close()


        End Try
    End Sub
End Class