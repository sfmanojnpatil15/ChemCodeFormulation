<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EditMaterial
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.MaterialID = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblComponentCode = New System.Windows.Forms.Label()
        Me.txtComponentCode = New System.Windows.Forms.TextBox()
        Me.txtcategoreyEditMaterial = New System.Windows.Forms.TextBox()
        Me.lblcategoreyEditMaterial = New System.Windows.Forms.Label()
        Me.MaterialTab = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblCasePack = New System.Windows.Forms.Label()
        Me.txtCasePack = New System.Windows.Forms.TextBox()
        Me.txtVendorCode = New System.Windows.Forms.TextBox()
        Me.lblVendorCode = New System.Windows.Forms.Label()
        Me.tabTierPricing = New System.Windows.Forms.TabPage()
        Me.grdMaterialTierPricing = New System.Windows.Forms.DataGridView()
        Me.TierID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierMaterialID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierQTY = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierCost = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierAddedOn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierUpdatedOn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txtAddNewLineTierPricing = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnDownload = New System.Windows.Forms.Button()
        Me.btnUploadAttachment = New System.Windows.Forms.Button()
        Me.dgvAttachments = New System.Windows.Forms.DataGridView()
        Me.FileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MaterialIDColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FileExtension = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FileData = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FileType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Uploadeddate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblAttachmentId = New System.Windows.Forms.Label()
        Me.MaterialTab.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.tabTierPricing.SuspendLayout()
        CType(Me.grdMaterialTierPricing, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.dgvAttachments, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 101)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Material Name"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(28, 118)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(374, 20)
        Me.TextBox1.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(25, 144)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Supplier"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(28, 161)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(374, 20)
        Me.TextBox2.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 230)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Price"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(28, 247)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(72, 20)
        Me.TextBox3.TabIndex = 6
        '
        'MaterialID
        '
        Me.MaterialID.AutoSize = True
        Me.MaterialID.Location = New System.Drawing.Point(398, 3)
        Me.MaterialID.Name = "MaterialID"
        Me.MaterialID.Size = New System.Drawing.Size(13, 13)
        Me.MaterialID.TabIndex = 0
        Me.MaterialID.Text = "1"
        Me.MaterialID.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(406, 367)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "Save"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lblComponentCode
        '
        Me.lblComponentCode.AutoSize = True
        Me.lblComponentCode.Location = New System.Drawing.Point(25, 58)
        Me.lblComponentCode.Name = "lblComponentCode"
        Me.lblComponentCode.Size = New System.Drawing.Size(89, 13)
        Me.lblComponentCode.TabIndex = 0
        Me.lblComponentCode.Text = "Component Code"
        '
        'txtComponentCode
        '
        Me.txtComponentCode.Location = New System.Drawing.Point(28, 75)
        Me.txtComponentCode.Name = "txtComponentCode"
        Me.txtComponentCode.Size = New System.Drawing.Size(374, 20)
        Me.txtComponentCode.TabIndex = 2
        '
        'txtcategoreyEditMaterial
        '
        Me.txtcategoreyEditMaterial.Location = New System.Drawing.Point(28, 204)
        Me.txtcategoreyEditMaterial.Name = "txtcategoreyEditMaterial"
        Me.txtcategoreyEditMaterial.Size = New System.Drawing.Size(374, 20)
        Me.txtcategoreyEditMaterial.TabIndex = 5
        '
        'lblcategoreyEditMaterial
        '
        Me.lblcategoreyEditMaterial.AutoSize = True
        Me.lblcategoreyEditMaterial.Location = New System.Drawing.Point(25, 187)
        Me.lblcategoreyEditMaterial.Name = "lblcategoreyEditMaterial"
        Me.lblcategoreyEditMaterial.Size = New System.Drawing.Size(49, 13)
        Me.lblcategoreyEditMaterial.TabIndex = 0
        Me.lblcategoreyEditMaterial.Text = "Category"
        '
        'MaterialTab
        '
        Me.MaterialTab.Controls.Add(Me.TabPage1)
        Me.MaterialTab.Controls.Add(Me.tabTierPricing)
        Me.MaterialTab.Controls.Add(Me.TabPage2)
        Me.MaterialTab.Location = New System.Drawing.Point(12, 12)
        Me.MaterialTab.Name = "MaterialTab"
        Me.MaterialTab.SelectedIndex = 0
        Me.MaterialTab.Size = New System.Drawing.Size(472, 351)
        Me.MaterialTab.TabIndex = 7
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.lblCasePack)
        Me.TabPage1.Controls.Add(Me.txtCasePack)
        Me.TabPage1.Controls.Add(Me.txtVendorCode)
        Me.TabPage1.Controls.Add(Me.lblVendorCode)
        Me.TabPage1.Controls.Add(Me.lblComponentCode)
        Me.TabPage1.Controls.Add(Me.txtcategoreyEditMaterial)
        Me.TabPage1.Controls.Add(Me.MaterialID)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.lblcategoreyEditMaterial)
        Me.TabPage1.Controls.Add(Me.TextBox1)
        Me.TabPage1.Controls.Add(Me.txtComponentCode)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.TextBox2)
        Me.TabPage1.Controls.Add(Me.TextBox3)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(464, 325)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Material"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(106, 293)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(32, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "kg/lb"
        '
        'lblCasePack
        '
        Me.lblCasePack.AutoSize = True
        Me.lblCasePack.Location = New System.Drawing.Point(25, 270)
        Me.lblCasePack.Name = "lblCasePack"
        Me.lblCasePack.Size = New System.Drawing.Size(59, 13)
        Me.lblCasePack.TabIndex = 0
        Me.lblCasePack.Text = "Case Pack"
        '
        'txtCasePack
        '
        Me.txtCasePack.Location = New System.Drawing.Point(28, 289)
        Me.txtCasePack.Name = "txtCasePack"
        Me.txtCasePack.Size = New System.Drawing.Size(75, 20)
        Me.txtCasePack.TabIndex = 7
        '
        'txtVendorCode
        '
        Me.txtVendorCode.Location = New System.Drawing.Point(28, 32)
        Me.txtVendorCode.Name = "txtVendorCode"
        Me.txtVendorCode.Size = New System.Drawing.Size(374, 20)
        Me.txtVendorCode.TabIndex = 1
        '
        'lblVendorCode
        '
        Me.lblVendorCode.AutoSize = True
        Me.lblVendorCode.Location = New System.Drawing.Point(25, 16)
        Me.lblVendorCode.Name = "lblVendorCode"
        Me.lblVendorCode.Size = New System.Drawing.Size(69, 13)
        Me.lblVendorCode.TabIndex = 0
        Me.lblVendorCode.Text = "Vendor Code"
        '
        'tabTierPricing
        '
        Me.tabTierPricing.Controls.Add(Me.grdMaterialTierPricing)
        Me.tabTierPricing.Controls.Add(Me.txtAddNewLineTierPricing)
        Me.tabTierPricing.Location = New System.Drawing.Point(4, 22)
        Me.tabTierPricing.Name = "tabTierPricing"
        Me.tabTierPricing.Size = New System.Drawing.Size(464, 325)
        Me.tabTierPricing.TabIndex = 2
        Me.tabTierPricing.Text = "Tier Pricing"
        Me.tabTierPricing.UseVisualStyleBackColor = True
        '
        'grdMaterialTierPricing
        '
        Me.grdMaterialTierPricing.AllowUserToAddRows = False
        Me.grdMaterialTierPricing.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.grdMaterialTierPricing.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.grdMaterialTierPricing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdMaterialTierPricing.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TierID, Me.TierMaterialID, Me.TierQTY, Me.TierCost, Me.TierAddedOn, Me.TierUpdatedOn})
        Me.grdMaterialTierPricing.Location = New System.Drawing.Point(16, 41)
        Me.grdMaterialTierPricing.Name = "grdMaterialTierPricing"
        Me.grdMaterialTierPricing.Size = New System.Drawing.Size(433, 268)
        Me.grdMaterialTierPricing.TabIndex = 21
        '
        'TierID
        '
        Me.TierID.DataPropertyName = "ID"
        Me.TierID.HeaderText = "ID"
        Me.TierID.Name = "TierID"
        Me.TierID.Visible = False
        '
        'TierMaterialID
        '
        Me.TierMaterialID.DataPropertyName = "MaterialID"
        Me.TierMaterialID.HeaderText = "MaterialID"
        Me.TierMaterialID.Name = "TierMaterialID"
        Me.TierMaterialID.Visible = False
        '
        'TierQTY
        '
        Me.TierQTY.DataPropertyName = "QTY"
        Me.TierQTY.HeaderText = "QTY"
        Me.TierQTY.Name = "TierQTY"
        '
        'TierCost
        '
        Me.TierCost.DataPropertyName = "Cost"
        Me.TierCost.HeaderText = "Cost"
        Me.TierCost.Name = "TierCost"
        '
        'TierAddedOn
        '
        Me.TierAddedOn.DataPropertyName = "AddedOn"
        Me.TierAddedOn.HeaderText = "AddedOn"
        Me.TierAddedOn.Name = "TierAddedOn"
        Me.TierAddedOn.Visible = False
        '
        'TierUpdatedOn
        '
        Me.TierUpdatedOn.DataPropertyName = "UpdatedOn"
        Me.TierUpdatedOn.HeaderText = "UpdatedOn"
        Me.TierUpdatedOn.Name = "TierUpdatedOn"
        Me.TierUpdatedOn.Visible = False
        '
        'txtAddNewLineTierPricing
        '
        Me.txtAddNewLineTierPricing.Cursor = System.Windows.Forms.Cursors.Hand
        Me.txtAddNewLineTierPricing.Location = New System.Drawing.Point(374, 12)
        Me.txtAddNewLineTierPricing.Name = "txtAddNewLineTierPricing"
        Me.txtAddNewLineTierPricing.Size = New System.Drawing.Size(75, 23)
        Me.txtAddNewLineTierPricing.TabIndex = 20
        Me.txtAddNewLineTierPricing.Text = "Add"
        Me.txtAddNewLineTierPricing.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.BtnDelete)
        Me.TabPage2.Controls.Add(Me.BtnDownload)
        Me.TabPage2.Controls.Add(Me.btnUploadAttachment)
        Me.TabPage2.Controls.Add(Me.dgvAttachments)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(464, 325)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Attachments"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Location = New System.Drawing.Point(372, 17)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(75, 23)
        Me.BtnDelete.TabIndex = 16
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnDownload
        '
        Me.BtnDownload.Location = New System.Drawing.Point(291, 17)
        Me.BtnDownload.Name = "BtnDownload"
        Me.BtnDownload.Size = New System.Drawing.Size(75, 23)
        Me.BtnDownload.TabIndex = 15
        Me.BtnDownload.Text = "Download"
        Me.BtnDownload.UseVisualStyleBackColor = True
        '
        'btnUploadAttachment
        '
        Me.btnUploadAttachment.Location = New System.Drawing.Point(17, 17)
        Me.btnUploadAttachment.Name = "btnUploadAttachment"
        Me.btnUploadAttachment.Size = New System.Drawing.Size(117, 23)
        Me.btnUploadAttachment.TabIndex = 14
        Me.btnUploadAttachment.Text = "Upload Attachment"
        Me.btnUploadAttachment.UseVisualStyleBackColor = True
        '
        'dgvAttachments
        '
        Me.dgvAttachments.AllowUserToAddRows = False
        Me.dgvAttachments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAttachments.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FileName, Me.MaterialIDColumn, Me.Id, Me.FileExtension, Me.FileData, Me.FileType, Me.Uploadeddate})
        Me.dgvAttachments.Location = New System.Drawing.Point(17, 46)
        Me.dgvAttachments.MultiSelect = False
        Me.dgvAttachments.Name = "dgvAttachments"
        Me.dgvAttachments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAttachments.Size = New System.Drawing.Size(430, 261)
        Me.dgvAttachments.TabIndex = 12
        '
        'FileName
        '
        Me.FileName.DataPropertyName = "FileName"
        Me.FileName.HeaderText = "File Name"
        Me.FileName.Name = "FileName"
        Me.FileName.ReadOnly = True
        Me.FileName.Width = 200
        '
        'MaterialIDColumn
        '
        Me.MaterialIDColumn.DataPropertyName = "MaterialId"
        Me.MaterialIDColumn.HeaderText = "Material Id"
        Me.MaterialIDColumn.Name = "MaterialIDColumn"
        Me.MaterialIDColumn.ReadOnly = True
        Me.MaterialIDColumn.Visible = False
        '
        'Id
        '
        Me.Id.DataPropertyName = "Id"
        Me.Id.HeaderText = "Id"
        Me.Id.Name = "Id"
        Me.Id.ReadOnly = True
        Me.Id.Visible = False
        '
        'FileExtension
        '
        Me.FileExtension.DataPropertyName = "FileExtension"
        Me.FileExtension.HeaderText = "FileExtension"
        Me.FileExtension.Name = "FileExtension"
        Me.FileExtension.ReadOnly = True
        Me.FileExtension.Visible = False
        '
        'FileData
        '
        Me.FileData.DataPropertyName = "FileData"
        Me.FileData.HeaderText = "FileData"
        Me.FileData.Name = "FileData"
        Me.FileData.ReadOnly = True
        Me.FileData.Visible = False
        '
        'FileType
        '
        Me.FileType.DataPropertyName = "FileType"
        Me.FileType.HeaderText = "FileType"
        Me.FileType.Name = "FileType"
        Me.FileType.ReadOnly = True
        Me.FileType.Visible = False
        '
        'Uploadeddate
        '
        Me.Uploadeddate.DataPropertyName = "Uploadeddate"
        Me.Uploadeddate.HeaderText = "Uploaded Date"
        Me.Uploadeddate.Name = "Uploadeddate"
        Me.Uploadeddate.ReadOnly = True
        Me.Uploadeddate.Width = 185
        '
        'lblAttachmentId
        '
        Me.lblAttachmentId.AutoSize = True
        Me.lblAttachmentId.Location = New System.Drawing.Point(288, 320)
        Me.lblAttachmentId.Name = "lblAttachmentId"
        Me.lblAttachmentId.Size = New System.Drawing.Size(0, 13)
        Me.lblAttachmentId.TabIndex = 17
        Me.lblAttachmentId.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblAttachmentId.Visible = False
        '
        'EditMaterial
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(495, 399)
        Me.Controls.Add(Me.lblAttachmentId)
        Me.Controls.Add(Me.MaterialTab)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "EditMaterial"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Material"
        Me.MaterialTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.tabTierPricing.ResumeLayout(False)
        CType(Me.grdMaterialTierPricing, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.dgvAttachments, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents MaterialID As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents lblComponentCode As Label
    Friend WithEvents txtComponentCode As TextBox
    Friend WithEvents txtcategoreyEditMaterial As TextBox
    Friend WithEvents lblcategoreyEditMaterial As Label
    Friend WithEvents MaterialTab As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents BtnDelete As Button
    Friend WithEvents BtnDownload As Button
    Friend WithEvents btnUploadAttachment As Button
    Friend WithEvents lblAttachmentId As Label
    Friend WithEvents dgvAttachments As DataGridView
    Friend WithEvents FileName As DataGridViewTextBoxColumn
    Friend WithEvents MaterialIDColumn As DataGridViewTextBoxColumn
    Friend WithEvents Id As DataGridViewTextBoxColumn
    Friend WithEvents FileExtension As DataGridViewTextBoxColumn
    Friend WithEvents FileData As DataGridViewTextBoxColumn
    Friend WithEvents FileType As DataGridViewTextBoxColumn
    Friend WithEvents Uploadeddate As DataGridViewTextBoxColumn
    Friend WithEvents Label7 As Label
    Friend WithEvents lblCasePack As Label
    Friend WithEvents txtCasePack As TextBox
    Friend WithEvents txtVendorCode As TextBox
    Friend WithEvents lblVendorCode As Label
    Friend WithEvents tabTierPricing As TabPage
    Friend WithEvents grdMaterialTierPricing As DataGridView
    Friend WithEvents TierID As DataGridViewTextBoxColumn
    Friend WithEvents TierMaterialID As DataGridViewTextBoxColumn
    Friend WithEvents TierQTY As DataGridViewTextBoxColumn
    Friend WithEvents TierCost As DataGridViewTextBoxColumn
    Friend WithEvents TierAddedOn As DataGridViewTextBoxColumn
    Friend WithEvents TierUpdatedOn As DataGridViewTextBoxColumn
    Friend WithEvents txtAddNewLineTierPricing As Button
End Class
