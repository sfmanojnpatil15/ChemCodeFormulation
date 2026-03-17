Imports outlook = Microsoft.Office.Interop.Outlook
Imports System.Runtime.InteropServices

Public Class SendForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim OutlookMessage As outlook.MailItem
        Dim AppOutlook As New outlook.Application

        Dim mySignature As String

        Try
            OutlookMessage = AppOutlook.CreateItem(outlook.OlItemType.olMailItem)
            Dim Recipents As outlook.Recipients = OutlookMessage.Recipients
            Recipents.Add("pipelian@hotmail.com")
            OutlookMessage.Subject = "Sending through Outlook"
            OutlookMessage.Body = "Testing outlook Mail"

            OutlookMessage.BodyFormat = outlook.OlBodyFormat.olFormatHTML
            mySignature = OutlookMessage.HTMLBody
            OutlookMessage.HTMLBody = "Here " & mySignature
            OutlookMessage.Display()
        Catch ex As system.Exception
            MessageBox.Show("Mail could not be sent") 'if you dont want this message, simply delete this line 
        Finally
            OutlookMessage = Nothing
            AppOutlook = Nothing
        End Try




    End Sub

    Private Sub SendForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        DataGridView1.Rows.Add(5000, 0)
        DataGridView1.Rows.Add(10000, 0)
        DataGridView1.Rows.Add(15000, 0)
        DataGridView1.Rows.Add(20000, 0)
        DataGridView1.Rows.Add(25000, 0)
        DataGridView1.Rows.Add(50000, 0)
        DataGridView1.Rows.Add(75000, 0)
        DataGridView1.Rows.Add(100000, 0)
        DataGridView1.Rows.Add(150000, 0)
        DataGridView1.Rows.Add(200000, 0)
        DataGridView1.Rows.Add(250000, 0)
        DataGridView1.Rows.Add(500000, 0)
        DataGridView1.Rows.Add(750000, 0)
        DataGridView1.Rows.Add(1000000, 0)


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim fI As Integer
        For j As Integer = 0 To DataGridView1.Rows.Count - 2
            If DataGridView1.Rows(j).Cells(0).Value = TextBox1.Text Then
                fI = j
                DataGridView1.Rows(j).Cells(1).Value = TextBox2.Text
            End If
        Next

        For j As Integer = fI + 1 To DataGridView1.Rows.Count - 2
            DataGridView1.Rows(j).Cells(1).Value = Format(DataGridView1.Rows(j - 1).Cells(1).Value - ((DataGridView1.Rows(j - 1).Cells(1).Value * 8.13) / 100), "N2")
        Next
        While fI > 0
            DataGridView1.Rows(fI - 1).Cells(1).Value = Format(DataGridView1.Rows(fI + 1).Cells(1).Value + ((DataGridView1.Rows(fI + 1).Cells(1).Value * 9.2) / 100), "N2")
            fI = fI - 1
        End While


    End Sub
End Class