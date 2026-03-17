<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SalesRepDetails
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblPhone = New System.Windows.Forms.Label()
        Me.lblLastName = New System.Windows.Forms.Label()
        Me.txtPhone = New System.Windows.Forms.TextBox()
        Me.txtLastName = New System.Windows.Forms.TextBox()
        Me.btnSaveSalesRep = New System.Windows.Forms.Button()
        Me.txtFirstName = New System.Windows.Forms.TextBox()
        Me.lblFirstName = New System.Windows.Forms.Label()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.AddGroupBox = New System.Windows.Forms.GroupBox()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.txtSalesRepId = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.AddGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblPhone
        '
        Me.lblPhone.AutoSize = True
        Me.lblPhone.Location = New System.Drawing.Point(14, 100)
        Me.lblPhone.Name = "lblPhone"
        Me.lblPhone.Size = New System.Drawing.Size(38, 13)
        Me.lblPhone.TabIndex = 0
        Me.lblPhone.Text = "Phone"
        '
        'lblLastName
        '
        Me.lblLastName.AutoSize = True
        Me.lblLastName.Location = New System.Drawing.Point(14, 73)
        Me.lblLastName.Name = "lblLastName"
        Me.lblLastName.Size = New System.Drawing.Size(58, 13)
        Me.lblLastName.TabIndex = 0
        Me.lblLastName.Text = "Last Name"
        '
        'txtPhone
        '
        Me.txtPhone.Location = New System.Drawing.Point(88, 100)
        Me.txtPhone.MaxLength = 20
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(242, 20)
        Me.txtPhone.TabIndex = 4
        '
        'txtLastName
        '
        Me.txtLastName.Location = New System.Drawing.Point(88, 72)
        Me.txtLastName.MaxLength = 20
        Me.txtLastName.Name = "txtLastName"
        Me.txtLastName.Size = New System.Drawing.Size(242, 20)
        Me.txtLastName.TabIndex = 3
        '
        'btnSaveSalesRep
        '
        Me.btnSaveSalesRep.Location = New System.Drawing.Point(255, 163)
        Me.btnSaveSalesRep.Name = "btnSaveSalesRep"
        Me.btnSaveSalesRep.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveSalesRep.TabIndex = 6
        Me.btnSaveSalesRep.Text = "Save"
        Me.btnSaveSalesRep.UseVisualStyleBackColor = True
        '
        'txtFirstName
        '
        Me.txtFirstName.Location = New System.Drawing.Point(88, 44)
        Me.txtFirstName.MaxLength = 20
        Me.txtFirstName.Name = "txtFirstName"
        Me.txtFirstName.Size = New System.Drawing.Size(242, 20)
        Me.txtFirstName.TabIndex = 2
        '
        'lblFirstName
        '
        Me.lblFirstName.AutoSize = True
        Me.lblFirstName.Location = New System.Drawing.Point(14, 46)
        Me.lblFirstName.Name = "lblFirstName"
        Me.lblFirstName.Size = New System.Drawing.Size(57, 13)
        Me.lblFirstName.TabIndex = 0
        Me.lblFirstName.Text = "First Name"
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(14, 127)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(32, 13)
        Me.lblEmail.TabIndex = 0
        Me.lblEmail.Text = "Email"
        '
        'AddGroupBox
        '
        Me.AddGroupBox.BackColor = System.Drawing.SystemColors.Control
        Me.AddGroupBox.Controls.Add(Me.txtEmail)
        Me.AddGroupBox.Controls.Add(Me.txtSalesRepId)
        Me.AddGroupBox.Controls.Add(Me.Label1)
        Me.AddGroupBox.Controls.Add(Me.lblPhone)
        Me.AddGroupBox.Controls.Add(Me.lblEmail)
        Me.AddGroupBox.Controls.Add(Me.lblLastName)
        Me.AddGroupBox.Controls.Add(Me.txtFirstName)
        Me.AddGroupBox.Controls.Add(Me.txtPhone)
        Me.AddGroupBox.Controls.Add(Me.lblFirstName)
        Me.AddGroupBox.Controls.Add(Me.txtLastName)
        Me.AddGroupBox.Controls.Add(Me.btnSaveSalesRep)
        Me.AddGroupBox.Location = New System.Drawing.Point(0, 2)
        Me.AddGroupBox.Name = "AddGroupBox"
        Me.AddGroupBox.Size = New System.Drawing.Size(341, 201)
        Me.AddGroupBox.TabIndex = 10
        Me.AddGroupBox.TabStop = False
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(88, 128)
        Me.txtEmail.MaxLength = 100
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(242, 20)
        Me.txtEmail.TabIndex = 5
        '
        'txtSalesRepId
        '
        Me.txtSalesRepId.Location = New System.Drawing.Point(88, 16)
        Me.txtSalesRepId.MaxLength = 2
        Me.txtSalesRepId.Name = "txtSalesRepId"
        Me.txtSalesRepId.Size = New System.Drawing.Size(242, 20)
        Me.txtSalesRepId.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Sales Rep Id"
        '
        'SalesRepDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(342, 200)
        Me.Controls.Add(Me.AddGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SalesRepDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Sales Rep"
        Me.AddGroupBox.ResumeLayout(False)
        Me.AddGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblPhone As Label
    Friend WithEvents lblLastName As Label
    Friend WithEvents txtPhone As TextBox
    Friend WithEvents txtLastName As TextBox
    Friend WithEvents btnSaveSalesRep As Button
    Friend WithEvents txtFirstName As TextBox
    Friend WithEvents lblFirstName As Label
    Friend WithEvents lblEmail As Label
    Friend WithEvents AddGroupBox As GroupBox
    Friend WithEvents txtSalesRepId As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtEmail As TextBox
End Class
