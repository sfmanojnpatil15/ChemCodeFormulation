Public Class UpdateFreightCost


    Public Event passvalue(text As Double)
    Private Sub UpdateFreightCost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Focus()
        TextBox1.SelectionStart = 0
        TextBox1.SelectionLength = TextBox1.Text.Length
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or Asc(e.KeyChar) = 8)
    End Sub
    Private Sub TextBox1_1(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            RaiseEvent passvalue(TextBox1.Text)
            Me.Close()
            Me.Close()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        RaiseEvent passvalue(TextBox1.Text)
        Me.Close()
    End Sub


    Private Sub UpdateFreightCost_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Me.Dispose()
    End Sub



End Class