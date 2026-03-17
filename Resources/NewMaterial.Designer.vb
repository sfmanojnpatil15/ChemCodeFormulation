<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NewMaterial
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
        Me.txtMaterialName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSupplier = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPrice = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cmbUOM = New System.Windows.Forms.ComboBox()
        Me.txtComponentCode = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCategory = New System.Windows.Forms.TextBox()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.MaterialTab = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblCasePack = New System.Windows.Forms.Label()
        Me.txtCasePack = New System.Windows.Forms.TextBox()
        Me.txtVendorCode = New System.Windows.Forms.TextBox()
        Me.lblVendorCode = New System.Windows.Forms.Label()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.grdMaterialTierPricing = New System.Windows.Forms.DataGridView()
        Me.TierID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierMaterialID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierQTY = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierCost = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierAddedOn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TierUpdatedOn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txtAddNewLineTierPricing = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnDownload = New System.Windows.Forms.Button()
        Me.btnUploadAttachment = New System.Windows.Forms.Button()
        Me.dgvAttachments = New System.Windows.Forms.DataGridView()
        Me.FileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MaterialId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Uploadeddate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FileExtension = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FileData = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FileType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MaterialTab.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        CType(Me.grdMaterialTierPricing, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.dgvAttachments, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Material Name"
        '
        'txtMaterialName
        '
        Me.txtMaterialName.Location = New System.Drawing.Point(26, 30)
        Me.txtMaterialName.Name = "txtMaterialName"
        Me.txtMaterialName.Size = New System.Drawing.Size(323, 20)
        Me.txtMaterialName.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Supplier"
        '
        'txtSupplier
        '
        Me.txtSupplier.Location = New System.Drawing.Point(26, 73)
        Me.txtSupplier.Name = "txtSupplier"
        Me.txtSupplier.Size = New System.Drawing.Size(323, 20)
        Me.txtSupplier.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 146)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Price"
        '
        'txtPrice
        '
        Me.txtPrice.Location = New System.Drawing.Point(26, 162)
        Me.txtPrice.Name = "txtPrice"
        Me.txtPrice.Size = New System.Drawing.Size(75, 20)
        Me.txtPrice.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(126, 146)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "UOM"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(239, 146)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(89, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Component Code"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(405, 323)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Save"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cmbUOM
        '
        Me.cmbUOM.FormattingEnabled = True
        Me.cmbUOM.ItemHeight = 13
        Me.cmbUOM.Items.AddRange(New Object() {"KG", "EACH"})
        Me.cmbUOM.Location = New System.Drawing.Point(130, 162)
        Me.cmbUOM.Name = "cmbUOM"
        Me.cmbUOM.Size = New System.Drawing.Size(75, 21)
        Me.cmbUOM.TabIndex = 5
        Me.cmbUOM.Text = "KG"
        '
        'txtComponentCode
        '
        Me.txtComponentCode.Location = New System.Drawing.Point(241, 162)
        Me.txtComponentCode.Name = "txtComponentCode"
        Me.txtComponentCode.Size = New System.Drawing.Size(107, 20)
        Me.txtComponentCode.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(0, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(0, 13)
        Me.Label5.TabIndex = 6
        '
        'txtCategory
        '
        Me.txtCategory.Location = New System.Drawing.Point(26, 117)
        Me.txtCategory.Name = "txtCategory"
        Me.txtCategory.Size = New System.Drawing.Size(323, 20)
        Me.txtCategory.TabIndex = 3
        Me.txtCategory.Text = "RM"
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Location = New System.Drawing.Point(23, 100)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(49, 13)
        Me.lblCategory.TabIndex = 0
        Me.lblCategory.Text = "Category"
        '
        'MaterialTab
        '
        Me.MaterialTab.Controls.Add(Me.TabPage1)
        Me.MaterialTab.Controls.Add(Me.TabPage3)
        Me.MaterialTab.Controls.Add(Me.TabPage2)
        Me.MaterialTab.Location = New System.Drawing.Point(12, 12)
        Me.MaterialTab.Name = "MaterialTab"
        Me.MaterialTab.SelectedIndex = 0
        Me.MaterialTab.Size = New System.Drawing.Size(472, 309)
        Me.MaterialTab.TabIndex = 8
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.lblCasePack)
        Me.TabPage1.Controls.Add(Me.txtCasePack)
        Me.TabPage1.Controls.Add(Me.txtVendorCode)
        Me.TabPage1.Controls.Add(Me.lblVendorCode)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.txtMaterialName)
        Me.TabPage1.Controls.Add(Me.txtCategory)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.lblCategory)
        Me.TabPage1.Controls.Add(Me.txtSupplier)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.txtComponentCode)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.cmbUOM)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.txtPrice)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(464, 283)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Material"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(207, 210)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(32, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "kg/lb"
        '
        'lblCasePack
        '
        Me.lblCasePack.AutoSize = True
        Me.lblCasePack.Location = New System.Drawing.Point(126, 190)
        Me.lblCasePack.Name = "lblCasePack"
        Me.lblCasePack.Size = New System.Drawing.Size(59, 13)
        Me.lblCasePack.TabIndex = 0
        Me.lblCasePack.Text = "Case Pack"
        '
        'txtCasePack
        '
        Me.txtCasePack.Location = New System.Drawing.Point(129, 206)
        Me.txtCasePack.Name = "txtCasePack"
        Me.txtCasePack.Size = New System.Drawing.Size(75, 20)
        Me.txtCasePack.TabIndex = 8
        '
        'txtVendorCode
        '
        Me.txtVendorCode.Location = New System.Drawing.Point(26, 206)
        Me.txtVendorCode.Name = "txtVendorCode"
        Me.txtVendorCode.Size = New System.Drawing.Size(75, 20)
        Me.txtVendorCode.TabIndex = 7
        '
        'lblVendorCode
        '
        Me.lblVendorCode.AutoSize = True
        Me.lblVendorCode.Location = New System.Drawing.Point(23, 190)
        Me.lblVendorCode.Name = "lblVendorCode"
        Me.lblVendorCode.Size = New System.Drawing.Size(69, 13)
        Me.lblVendorCode.TabIndex = 0
        Me.lblVendorCode.Text = "Vendor Code"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.grdMaterialTierPricing)
        Me.TabPage3.Controls.Add(Me.txtAddNewLineTierPricing)
        Me.TabPage3.Controls.Add(Me.Button2)
        Me.TabPage3.Controls.Add(Me.Button3)
        Me.TabPage3.Controls.Add(Me.Button4)
        Me.TabPage3.Controls.Add(Me.Button5)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(464, 283)
        Me.TabPage3.TabIndex = 0
        Me.TabPage3.Text = "Tier Pricing"
        '
        'grdMaterialTierPricing
        '
        Me.grdMaterialTierPricing.AllowUserToAddRows = False
        Me.grdMaterialTierPricing.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.grdMaterialTierPricing.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.grdMaterialTierPricing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdMaterialTierPricing.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TierID, Me.TierMaterialID, Me.TierQTY, Me.TierCost, Me.TierAddedOn, Me.TierUpdatedOn})
        Me.grdMaterialTierPricing.Location = New System.Drawing.Point(13, 41)
        Me.grdMaterialTierPricing.Name = "grdMaterialTierPricing"
        Me.grdMaterialTierPricing.Size = New System.Drawing.Size(433, 227)
        Me.grdMaterialTierPricing.TabIndex = 19
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
        Me.txtAddNewLineTierPricing.Location = New System.Drawing.Point(371, 12)
        Me.txtAddNewLineTierPricing.Name = "txtAddNewLineTierPricing"
        Me.txtAddNewLineTierPricing.Size = New System.Drawing.Size(75, 23)
        Me.txtAddNewLineTierPricing.TabIndex = 17
        Me.txtAddNewLineTierPricing.Text = "Add"
        Me.txtAddNewLineTierPricing.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(357, -23)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 16
        Me.Button2.Text = "Delete"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(276, -23)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 15
        Me.Button3.Text = "Download"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(1, -23)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(117, 23)
        Me.Button4.TabIndex = 14
        Me.Button4.Text = "Upload Attachment"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(389, 283)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(75, 23)
        Me.Button5.TabIndex = 13
        Me.Button5.Text = "Save"
        Me.Button5.UseVisualStyleBackColor = True
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
        Me.TabPage2.Size = New System.Drawing.Size(464, 283)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Attachments"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Location = New System.Drawing.Point(373, 17)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(75, 23)
        Me.BtnDelete.TabIndex = 11
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnDownload
        '
        Me.BtnDownload.Location = New System.Drawing.Point(292, 17)
        Me.BtnDownload.Name = "BtnDownload"
        Me.BtnDownload.Size = New System.Drawing.Size(75, 23)
        Me.BtnDownload.TabIndex = 10
        Me.BtnDownload.Text = "Download"
        Me.BtnDownload.UseVisualStyleBackColor = True
        '
        'btnUploadAttachment
        '
        Me.btnUploadAttachment.Location = New System.Drawing.Point(17, 17)
        Me.btnUploadAttachment.Name = "btnUploadAttachment"
        Me.btnUploadAttachment.Size = New System.Drawing.Size(117, 23)
        Me.btnUploadAttachment.TabIndex = 9
        Me.btnUploadAttachment.Text = "Upload Attachment"
        Me.btnUploadAttachment.UseVisualStyleBackColor = True
        '
        'dgvAttachments
        '
        Me.dgvAttachments.AllowUserToAddRows = False
        Me.dgvAttachments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAttachments.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FileName, Me.MaterialId, Me.Id, Me.Uploadeddate, Me.FileExtension, Me.FileData, Me.FileType})
        Me.dgvAttachments.Location = New System.Drawing.Point(17, 46)
        Me.dgvAttachments.MultiSelect = False
        Me.dgvAttachments.Name = "dgvAttachments"
        Me.dgvAttachments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.dgvAttachments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAttachments.Size = New System.Drawing.Size(430, 218)
        Me.dgvAttachments.TabIndex = 1
        '
        'FileName
        '
        Me.FileName.DataPropertyName = "FileName"
        Me.FileName.HeaderText = "File Name"
        Me.FileName.Name = "FileName"
        Me.FileName.ReadOnly = True
        Me.FileName.Width = 200
        '
        'MaterialId
        '
        Me.MaterialId.DataPropertyName = "MaterialId"
        Me.MaterialId.HeaderText = "MaterialId"
        Me.MaterialId.Name = "MaterialId"
        Me.MaterialId.Visible = False
        '
        'Id
        '
        Me.Id.DataPropertyName = "Id"
        Me.Id.HeaderText = "Id"
        Me.Id.Name = "Id"
        Me.Id.ReadOnly = True
        Me.Id.Visible = False
        '
        'Uploadeddate
        '
        Me.Uploadeddate.DataPropertyName = "Uploadeddate"
        Me.Uploadeddate.HeaderText = "Uploaded Date"
        Me.Uploadeddate.Name = "Uploadeddate"
        Me.Uploadeddate.ReadOnly = True
        Me.Uploadeddate.Width = 185
        '
        'FileExtension
        '
        Me.FileExtension.DataPropertyName = "FileExtension"
        Me.FileExtension.HeaderText = "FileExtension"
        Me.FileExtension.Name = "FileExtension"
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
        'NewMaterial
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(495, 359)
        Me.Controls.Add(Me.MaterialTab)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "NewMaterial"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Material"
        Me.MaterialTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        CType(Me.grdMaterialTierPricing, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.dgvAttachments, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents txtMaterialName As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtSupplier As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtPrice As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents cmbUOM As ComboBox
    Friend WithEvents txtComponentCode As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtCategory As TextBox
    Friend WithEvents lblCategory As Label
    Friend WithEvents MaterialTab As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents dgvAttachments As DataGridView
    Friend WithEvents btnUploadAttachment As Button
    Friend WithEvents BtnDelete As Button
    Friend WithEvents BtnDownload As Button
    Friend WithEvents FileName As DataGridViewTextBoxColumn
    Friend WithEvents MaterialId As DataGridViewTextBoxColumn
    Friend WithEvents Id As DataGridViewTextBoxColumn
    Friend WithEvents Uploadeddate As DataGridViewTextBoxColumn
    Friend WithEvents FileExtension As DataGridViewTextBoxColumn
    Friend WithEvents FileData As DataGridViewTextBoxColumn
    Friend WithEvents FileType As DataGridViewTextBoxColumn
    Friend WithEvents txtVendorCode As TextBox
    Friend WithEvents lblVendorCode As Label
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents txtAddNewLineTierPricing As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents lblCasePack As Label
    Friend WithEvents txtCasePack As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents grdMaterialTierPricing As DataGridView
    Friend WithEvents TierID As DataGridViewTextBoxColumn
    Friend WithEvents TierMaterialID As DataGridViewTextBoxColumn
    Friend WithEvents TierQTY As DataGridViewTextBoxColumn
    Friend WithEvents TierCost As DataGridViewTextBoxColumn
    Friend WithEvents TierAddedOn As DataGridViewTextBoxColumn
    Friend WithEvents TierUpdatedOn As DataGridViewTextBoxColumn
    'Friend WithEvents Id As DataGridViewTextBoxColumn
    'Friend WithEvents MaterialId As DataGridViewTextBoxColumn

End Class
