Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient


Public Class LoginForm

    Inherits System.Windows.Forms.Form
    Public Shared AssemblyVersion As String

    Private results As String
    Public IsAdmin As Boolean = False

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        IsAdmin = Helper.login(txtPassword, txtUsername, Me)
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            IsAdmin = Helper.login(txtPassword, txtUsername, Me)
        End If
    End Sub

    Private Sub txtUsername_KeyDown(sender As Object, e As KeyEventArgs) Handles txtUsername.KeyDown
        If e.KeyCode = Keys.Enter Then
            IsAdmin = Helper.login(txtPassword, txtUsername, Me)
        End If
    End Sub

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Helper.InitialiseLog()
        Helper.GetAssembly(lblVersion, txtPassword)
    End Sub
End Class