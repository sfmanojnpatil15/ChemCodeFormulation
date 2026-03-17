Imports MySql.Data.MySqlClient

Public Class User

    Dim mySqlConnection As MySqlConnection = New MySqlConnection

    Private Sub lblSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim UserNameExist As Boolean = False

        Try

            If String.IsNullOrEmpty(txtUserName.Text) Or String.IsNullOrEmpty(txtPassword.Text) Or String.IsNullOrEmpty(txtFirstName.Text) Or String.IsNullOrEmpty(txtLastName.Text) Then
                MessageBox.Show("All fields are required", "Message", MessageBoxButtons.OK)
            Else

                mySqlConnection.ConnectionString = My.Settings.DBCon
                mySqlConnection.Open()

                Dim query = String.Format("SELECT * FROM userlist WHERE Username ='{0}'", txtUserName.Text.Trim())
                Dim mySqlCommand = New MySqlCommand(query, mySqlConnection)

                Dim dataReader As MySqlDataReader = mySqlCommand.ExecuteReader
                If dataReader.Read Then
                    UserNameExist = True
                End If
                dataReader.Close()

                If lblUserID.Text <> "-1" Then
                    If UserNameExist Then

                        Dim UpdateQuery = "UPDATE userlist SET FName=@FName,LName=@LName,IsInactive=@IsInactive,Password=@Password,Type=@Type where Username=@Username"
                        mySqlCommand = New MySqlCommand(UpdateQuery, mySqlConnection)
                        mySqlCommand.Parameters.AddWithValue("@Username", txtUserName.Text)
                        mySqlCommand.Parameters.AddWithValue("@FName", txtFirstName.Text)
                        mySqlCommand.Parameters.AddWithValue("@LName", txtLastName.Text)
                        mySqlCommand.Parameters.AddWithValue("@Password", txtPassword.Text)
                        mySqlCommand.Parameters.AddWithValue("@IsInactive", If(chkIsActive.Checked, 0, 1))
                        mySqlCommand.Parameters.AddWithValue("@Type", cmbType.Text)

                        mySqlCommand.ExecuteNonQuery()
                        Me.Close()

                        Dashboard.PopulateUser()
                    Else
                        MessageBox.Show("User not found!", "Message", MessageBoxButtons.OK)
                    End If
                Else
                    If UserNameExist Then
                        MessageBox.Show("User already exists", "Message", MessageBoxButtons.OK)
                    Else
                        Dim InsertQuery = "INSERT INTO userlist(Username,FName,LName,Password,IsInactive,Type) VALUES(@Username,@FName,@LName,@Password,@IsInactive,@Type)"
                        mySqlCommand = New MySqlCommand(InsertQuery)
                        mySqlCommand.Connection = mySqlConnection

                        mySqlCommand.Parameters.AddWithValue("@Username", txtUserName.Text)
                        mySqlCommand.Parameters.AddWithValue("@FName", txtFirstName.Text)
                        mySqlCommand.Parameters.AddWithValue("@LName", txtLastName.Text)
                        mySqlCommand.Parameters.AddWithValue("@Password", txtPassword.Text)
                        mySqlCommand.Parameters.AddWithValue("@IsInactive", 0)
                        mySqlCommand.Parameters.AddWithValue("@Type", cmbType.Text)

                        mySqlCommand.ExecuteNonQuery()
                        Me.Close()

                        Dashboard.PopulateUser()
                    End If
                End If




            End If
        Catch ex As Exception
            MessageBox.Show("Failed to connect to Database!", "Message", MessageBoxButtons.OK)
        Finally
            mySqlConnection.Close()


        End Try
    End Sub

    Private Sub User_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If lblUserID.Text <> "-1" Then
                mySqlConnection.ConnectionString = My.Settings.DBCon
                mySqlConnection.Open()

                Dim query = String.Format("SELECT * FROM userlist WHERE UserID ='{0}'", lblUserID.Text.Trim())
                Dim mySqlCommand = New MySqlCommand(query, mySqlConnection)

                Dim dataReader As MySqlDataReader = mySqlCommand.ExecuteReader
                If dataReader.Read() Then
                    txtFirstName.Text = dataReader("FName")
                    txtLastName.Text = dataReader("LName")
                    txtPassword.Text = dataReader("Password")
                    txtUserName.Text = dataReader("Username")
                    chkIsActive.Checked = If(dataReader("IsInActive"), False, True)
                    cmbType.SelectedItem = dataReader("Type")
                End If

                    dataReader.Close()


            End If
        Catch ex As Exception

        Finally
            mySqlConnection.Close()

        End Try

    End Sub

    Private Sub User_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing


        lblUserID.Text = "-1"
        Me.Text = "New User"
        txtUserName.Enabled = True
        chkIsActive.Checked = True
        chkIsActive.Enabled = False

        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtPassword.Text = ""
        txtUserName.Text = ""
        cmbType.SelectedIndex = 0
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub ContextMenuStrip2_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip2.Opening

    End Sub
End Class