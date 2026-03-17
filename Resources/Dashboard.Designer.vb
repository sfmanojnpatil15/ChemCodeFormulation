<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Dashboard
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dashboard))
        Me.btnMaterialList = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.FormulaPanel = New System.Windows.Forms.Panel()
        Me.calenderlookup = New System.Windows.Forms.PictureBox()
        Me.CalenderBox = New System.Windows.Forms.MonthCalendar()
        Me.FilterLookUp = New System.Windows.Forms.PictureBox()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.FilterList = New System.Windows.Forms.ListBox()
        Me.btnDeleteFormula = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SalesRep = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateCretaed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnSalesMargin = New System.Windows.Forms.Button()
        Me.MaterialPanel = New System.Windows.Forms.Panel()
        Me.btnDelateMaterial = New System.Windows.Forms.Button()
        Me.btnAddMaterial = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Cost = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserIDLabel = New System.Windows.Forms.Label()
        Me.ds = New System.Data.DataSet()
        Me.Username = New System.Windows.Forms.Label()
        Me.FormulaName = New System.Windows.Forms.Label()
        Me.MaterialName = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.UserPanel = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSearchUser = New System.Windows.Forms.TextBox()
        Me.lblAddUser = New System.Windows.Forms.Button()
        Me.userGrid = New System.Windows.Forms.DataGridView()
        Me.SalesMarginPanel = New System.Windows.Forms.Panel()
        Me.txtPackagingType = New System.Windows.Forms.TextBox()
        Me.lblPackagingType = New System.Windows.Forms.Label()
        Me.btnAddMargin = New System.Windows.Forms.Button()
        Me.lblSalesMargin = New System.Windows.Forms.Label()
        Me.grdSalesMargin = New System.Windows.Forms.DataGridView()
        Me.TCPanel = New System.Windows.Forms.Panel()
        Me.btnUpdateTC = New System.Windows.Forms.Button()
        Me.rtxMessage = New System.Windows.Forms.RichTextBox()
        Me.rtxLabTesting = New System.Windows.Forms.RichTextBox()
        Me.rtxQuoteExpiry = New System.Windows.Forms.RichTextBox()
        Me.rtxOtherIngredientsTablet = New System.Windows.Forms.RichTextBox()
        Me.rtxOtherIngredientsCapsule = New System.Windows.Forms.RichTextBox()
        Me.rtxOtherIngredientsPowder = New System.Windows.Forms.RichTextBox()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.lblLabTestPolicy = New System.Windows.Forms.Label()
        Me.lblIngredientPowder = New System.Windows.Forms.Label()
        Me.lblQuoteExpiry = New System.Windows.Forms.Label()
        Me.lblIngredientCapsule = New System.Windows.Forms.Label()
        Me.lblIngredientsTablet = New System.Windows.Forms.Label()
        Me.TCHeader = New System.Windows.Forms.Label()
        Me.SalesRepPanel = New System.Windows.Forms.Panel()
        Me.btnDeleteSalesRepo = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtSalesRep = New System.Windows.Forms.TextBox()
        Me.btnAddSales = New System.Windows.Forms.Button()
        Me.DTSalesRep = New System.Windows.Forms.DataGridView()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Phone = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Email = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblSalesRep = New System.Windows.Forms.Label()
        Me.btnSaleRep = New System.Windows.Forms.Button()
        Me.lblSapesRepoID = New System.Windows.Forms.Label()
        Me.btnUpdateTerms = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RenameCurrentFormulaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyCurrentFormulaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FormulaSettingPnl = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.FormulaSettingFreightUnitlbl = New System.Windows.Forms.Label()
        Me.FormulaSettingFreightlbl = New System.Windows.Forms.Label()
        Me.FormulaSettingsFreighttxt = New System.Windows.Forms.TextBox()
        Me.FormulaSettingBtn = New System.Windows.Forms.Button()
        Me.FormulaPanel.SuspendLayout()
        CType(Me.calenderlookup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FilterLookUp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MaterialPanel.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ds, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UserPanel.SuspendLayout()
        CType(Me.userGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SalesMarginPanel.SuspendLayout()
        CType(Me.grdSalesMargin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TCPanel.SuspendLayout()
        Me.SalesRepPanel.SuspendLayout()
        CType(Me.DTSalesRep, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.FormulaSettingPnl.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnMaterialList
        '
        Me.btnMaterialList.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnMaterialList.Location = New System.Drawing.Point(21, 54)
        Me.btnMaterialList.Name = "btnMaterialList"
        Me.btnMaterialList.Size = New System.Drawing.Size(115, 25)
        Me.btnMaterialList.TabIndex = 0
        Me.btnMaterialList.Text = "Material List"
        Me.btnMaterialList.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button2.Location = New System.Drawing.Point(21, 85)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(115, 25)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "New Formula"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button3.Location = New System.Drawing.Point(21, 116)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(115, 25)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Open Formula"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button4.Location = New System.Drawing.Point(21, 252)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(115, 25)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "My Account"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button5.Location = New System.Drawing.Point(21, 283)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(115, 25)
        Me.Button5.TabIndex = 4
        Me.Button5.Text = "Settings"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button6.Location = New System.Drawing.Point(21, 370)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(115, 25)
        Me.Button6.TabIndex = 5
        Me.Button6.Text = "Add/Edit User"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'FormulaPanel
        '
        Me.FormulaPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FormulaPanel.Controls.Add(Me.calenderlookup)
        Me.FormulaPanel.Controls.Add(Me.CalenderBox)
        Me.FormulaPanel.Controls.Add(Me.FilterLookUp)
        Me.FormulaPanel.Controls.Add(Me.txtFilter)
        Me.FormulaPanel.Controls.Add(Me.FilterList)
        Me.FormulaPanel.Controls.Add(Me.btnDeleteFormula)
        Me.FormulaPanel.Controls.Add(Me.Label2)
        Me.FormulaPanel.Controls.Add(Me.ComboBox1)
        Me.FormulaPanel.Controls.Add(Me.TextBox1)
        Me.FormulaPanel.Controls.Add(Me.Label1)
        Me.FormulaPanel.Controls.Add(Me.DataGridView1)
        Me.FormulaPanel.Location = New System.Drawing.Point(156, 43)
        Me.FormulaPanel.Name = "FormulaPanel"
        Me.FormulaPanel.Size = New System.Drawing.Size(693, 430)
        Me.FormulaPanel.TabIndex = 1
        '
        'calenderlookup
        '
        Me.calenderlookup.Image = Global.BPG_Costing.My.Resources.Resources.calender
        Me.calenderlookup.Location = New System.Drawing.Point(572, 37)
        Me.calenderlookup.Name = "calenderlookup"
        Me.calenderlookup.Size = New System.Drawing.Size(27, 20)
        Me.calenderlookup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.calenderlookup.TabIndex = 27
        Me.calenderlookup.TabStop = False
        '
        'CalenderBox
        '
        Me.CalenderBox.Location = New System.Drawing.Point(445, 63)
        Me.CalenderBox.Name = "CalenderBox"
        Me.CalenderBox.TabIndex = 26
        Me.CalenderBox.Visible = False
        '
        'FilterLookUp
        '
        Me.FilterLookUp.Image = Global.BPG_Costing.My.Resources.Resources.search
        Me.FilterLookUp.Location = New System.Drawing.Point(577, 39)
        Me.FilterLookUp.Name = "FilterLookUp"
        Me.FilterLookUp.Size = New System.Drawing.Size(20, 20)
        Me.FilterLookUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.FilterLookUp.TabIndex = 25
        Me.FilterLookUp.TabStop = False
        '
        'txtFilter
        '
        Me.txtFilter.Location = New System.Drawing.Point(445, 38)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(121, 20)
        Me.txtFilter.TabIndex = 24
        '
        'FilterList
        '
        Me.FilterList.FormattingEnabled = True
        Me.FilterList.Location = New System.Drawing.Point(462, 63)
        Me.FilterList.Name = "FilterList"
        Me.FilterList.Size = New System.Drawing.Size(132, 82)
        Me.FilterList.Sorted = True
        Me.FilterList.TabIndex = 23
        Me.FilterList.Visible = False
        '
        'btnDeleteFormula
        '
        Me.btnDeleteFormula.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDeleteFormula.Location = New System.Drawing.Point(606, 36)
        Me.btnDeleteFormula.Name = "btnDeleteFormula"
        Me.btnDeleteFormula.Size = New System.Drawing.Size(75, 24)
        Me.btnDeleteFormula.TabIndex = 10
        Me.btnDeleteFormula.Text = "Delete Formula"
        Me.btnDeleteFormula.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(11, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 15)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Open Formula"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Last 30 Days", "Formula Id", "Sales Rep Id", "Date Created", "All"})
        Me.ComboBox1.Location = New System.Drawing.Point(348, 37)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(95, 21)
        Me.ComboBox1.TabIndex = 3
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(87, 38)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(253, 20)
        Me.TextBox1.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Enter keyword"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.SalesRep, Me.DateCretaed})
        Me.DataGridView1.Location = New System.Drawing.Point(11, 66)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowTemplate.Height = 25
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(670, 348)
        Me.DataGridView1.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "FormulaID"
        Me.Column1.FillWeight = 132.3655!
        Me.Column1.HeaderText = "FormulaID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "FormulaName"
        Me.Column2.FillWeight = 349.4733!
        Me.Column2.HeaderText = "Formula Name"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "FormulaType"
        Me.Column3.FillWeight = 160.3867!
        Me.Column3.HeaderText = "Formula Type"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'SalesRep
        '
        Me.SalesRep.DataPropertyName = "SalesRepId"
        Me.SalesRep.FillWeight = 152.2842!
        Me.SalesRep.HeaderText = "Sales Rep"
        Me.SalesRep.Name = "SalesRep"
        '
        'DateCretaed
        '
        Me.DateCretaed.DataPropertyName = "EnteredDate"
        Me.DateCretaed.FillWeight = 205.4902!
        Me.DateCretaed.HeaderText = "Date Cretaed"
        Me.DateCretaed.Name = "DateCretaed"
        '
        'btnSalesMargin
        '
        Me.btnSalesMargin.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSalesMargin.Location = New System.Drawing.Point(21, 147)
        Me.btnSalesMargin.Name = "btnSalesMargin"
        Me.btnSalesMargin.Size = New System.Drawing.Size(115, 25)
        Me.btnSalesMargin.TabIndex = 2
        Me.btnSalesMargin.Text = "Sales Margin"
        Me.btnSalesMargin.UseVisualStyleBackColor = True
        Me.btnSalesMargin.Visible = False
        '
        'MaterialPanel
        '
        Me.MaterialPanel.Controls.Add(Me.btnDelateMaterial)
        Me.MaterialPanel.Controls.Add(Me.btnAddMaterial)
        Me.MaterialPanel.Controls.Add(Me.Label3)
        Me.MaterialPanel.Controls.Add(Me.TextBox2)
        Me.MaterialPanel.Controls.Add(Me.Label4)
        Me.MaterialPanel.Controls.Add(Me.DataGridView2)
        Me.MaterialPanel.Location = New System.Drawing.Point(156, 43)
        Me.MaterialPanel.Name = "MaterialPanel"
        Me.MaterialPanel.Size = New System.Drawing.Size(693, 430)
        Me.MaterialPanel.TabIndex = 3
        '
        'btnDelateMaterial
        '
        Me.btnDelateMaterial.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDelateMaterial.Location = New System.Drawing.Point(559, 42)
        Me.btnDelateMaterial.Name = "btnDelateMaterial"
        Me.btnDelateMaterial.Size = New System.Drawing.Size(121, 20)
        Me.btnDelateMaterial.TabIndex = 11
        Me.btnDelateMaterial.Text = "Delete Material"
        Me.btnDelateMaterial.UseVisualStyleBackColor = True
        '
        'btnAddMaterial
        '
        Me.btnAddMaterial.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAddMaterial.Location = New System.Drawing.Point(446, 42)
        Me.btnAddMaterial.Name = "btnAddMaterial"
        Me.btnAddMaterial.Size = New System.Drawing.Size(107, 20)
        Me.btnAddMaterial.TabIndex = 9
        Me.btnAddMaterial.Text = "New Material"
        Me.btnAddMaterial.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(11, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 15)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Material List"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(87, 42)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(301, 20)
        Me.TextBox2.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 45)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Enter keyword"
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.Cost})
        Me.DataGridView2.Location = New System.Drawing.Point(17, 69)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.RowTemplate.Height = 25
        Me.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView2.Size = New System.Drawing.Size(662, 348)
        Me.DataGridView2.TabIndex = 5
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "MaterialID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "MaterialID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "MaterialName"
        Me.DataGridViewTextBoxColumn2.FillWeight = 500.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Material Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "Supplier"
        Me.DataGridViewTextBoxColumn3.FillWeight = 200.0!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Supplier"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'Cost
        '
        Me.Cost.DataPropertyName = "Price"
        Me.Cost.HeaderText = "Cost"
        Me.Cost.Name = "Cost"
        Me.Cost.ReadOnly = True
        '
        'UserIDLabel
        '
        Me.UserIDLabel.AutoSize = True
        Me.UserIDLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.UserIDLabel.Location = New System.Drawing.Point(820, 43)
        Me.UserIDLabel.Name = "UserIDLabel"
        Me.UserIDLabel.Size = New System.Drawing.Size(15, 17)
        Me.UserIDLabel.TabIndex = 7
        Me.UserIDLabel.Text = "1"
        '
        'ds
        '
        Me.ds.DataSetName = "NewDataSet"
        '
        'Username
        '
        Me.Username.AutoSize = True
        Me.Username.Location = New System.Drawing.Point(821, 10)
        Me.Username.Name = "Username"
        Me.Username.Size = New System.Drawing.Size(36, 13)
        Me.Username.TabIndex = 8
        Me.Username.Text = "Admin"
        '
        'FormulaName
        '
        Me.FormulaName.AutoSize = True
        Me.FormulaName.Location = New System.Drawing.Point(164, 10)
        Me.FormulaName.Name = "FormulaName"
        Me.FormulaName.Size = New System.Drawing.Size(22, 13)
        Me.FormulaName.TabIndex = 9
        Me.FormulaName.Text = "NA"
        Me.FormulaName.Visible = False
        '
        'MaterialName
        '
        Me.MaterialName.AutoSize = True
        Me.MaterialName.Location = New System.Drawing.Point(164, 10)
        Me.MaterialName.Name = "MaterialName"
        Me.MaterialName.Size = New System.Drawing.Size(22, 13)
        Me.MaterialName.TabIndex = 9
        Me.MaterialName.Text = "NA"
        Me.MaterialName.Visible = False
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(777, 484)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(81, 13)
        Me.lblVersion.TabIndex = 10
        Me.lblVersion.Text = "Version: 2.0.0.0"
        '
        'UserPanel
        '
        Me.UserPanel.Controls.Add(Me.Label5)
        Me.UserPanel.Controls.Add(Me.Label6)
        Me.UserPanel.Controls.Add(Me.txtSearchUser)
        Me.UserPanel.Controls.Add(Me.lblAddUser)
        Me.UserPanel.Controls.Add(Me.userGrid)
        Me.UserPanel.Location = New System.Drawing.Point(156, 43)
        Me.UserPanel.Name = "UserPanel"
        Me.UserPanel.Size = New System.Drawing.Size(693, 430)
        Me.UserPanel.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.Location = New System.Drawing.Point(11, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 15)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "User List"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(13, 42)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(75, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Enter keyword"
        '
        'txtSearchUser
        '
        Me.txtSearchUser.Location = New System.Drawing.Point(92, 39)
        Me.txtSearchUser.Name = "txtSearchUser"
        Me.txtSearchUser.Size = New System.Drawing.Size(301, 20)
        Me.txtSearchUser.TabIndex = 5
        '
        'lblAddUser
        '
        Me.lblAddUser.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblAddUser.Location = New System.Drawing.Point(605, 37)
        Me.lblAddUser.Name = "lblAddUser"
        Me.lblAddUser.Size = New System.Drawing.Size(75, 23)
        Me.lblAddUser.TabIndex = 4
        Me.lblAddUser.Text = "Add User"
        Me.lblAddUser.UseVisualStyleBackColor = True
        '
        'userGrid
        '
        Me.userGrid.AllowUserToAddRows = False
        Me.userGrid.AllowUserToDeleteRows = False
        Me.userGrid.AllowUserToResizeColumns = False
        Me.userGrid.AllowUserToResizeRows = False
        Me.userGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.userGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.userGrid.Location = New System.Drawing.Point(11, 68)
        Me.userGrid.Name = "userGrid"
        Me.userGrid.ReadOnly = True
        Me.userGrid.RowHeadersVisible = False
        Me.userGrid.RowTemplate.Height = 25
        Me.userGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.userGrid.Size = New System.Drawing.Size(669, 349)
        Me.userGrid.TabIndex = 0
        '
        'SalesMarginPanel
        '
        Me.SalesMarginPanel.Controls.Add(Me.txtPackagingType)
        Me.SalesMarginPanel.Controls.Add(Me.lblPackagingType)
        Me.SalesMarginPanel.Controls.Add(Me.btnAddMargin)
        Me.SalesMarginPanel.Controls.Add(Me.lblSalesMargin)
        Me.SalesMarginPanel.Controls.Add(Me.grdSalesMargin)
        Me.SalesMarginPanel.Location = New System.Drawing.Point(156, 43)
        Me.SalesMarginPanel.Name = "SalesMarginPanel"
        Me.SalesMarginPanel.Size = New System.Drawing.Size(693, 430)
        Me.SalesMarginPanel.TabIndex = 8
        Me.SalesMarginPanel.Visible = False
        '
        'txtPackagingType
        '
        Me.txtPackagingType.Location = New System.Drawing.Point(102, 39)
        Me.txtPackagingType.Name = "txtPackagingType"
        Me.txtPackagingType.Size = New System.Drawing.Size(301, 20)
        Me.txtPackagingType.TabIndex = 11
        '
        'lblPackagingType
        '
        Me.lblPackagingType.AutoSize = True
        Me.lblPackagingType.Location = New System.Drawing.Point(13, 43)
        Me.lblPackagingType.Name = "lblPackagingType"
        Me.lblPackagingType.Size = New System.Drawing.Size(85, 13)
        Me.lblPackagingType.TabIndex = 10
        Me.lblPackagingType.Text = "Packaging Type"
        '
        'btnAddMargin
        '
        Me.btnAddMargin.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAddMargin.Location = New System.Drawing.Point(605, 37)
        Me.btnAddMargin.Name = "btnAddMargin"
        Me.btnAddMargin.Size = New System.Drawing.Size(75, 23)
        Me.btnAddMargin.TabIndex = 9
        Me.btnAddMargin.Text = "Add Margin"
        Me.btnAddMargin.UseVisualStyleBackColor = True
        '
        'lblSalesMargin
        '
        Me.lblSalesMargin.AutoSize = True
        Me.lblSalesMargin.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblSalesMargin.Location = New System.Drawing.Point(14, 9)
        Me.lblSalesMargin.Name = "lblSalesMargin"
        Me.lblSalesMargin.Size = New System.Drawing.Size(82, 15)
        Me.lblSalesMargin.TabIndex = 8
        Me.lblSalesMargin.Text = "Sales Margins"
        '
        'grdSalesMargin
        '
        Me.grdSalesMargin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdSalesMargin.Location = New System.Drawing.Point(7, 68)
        Me.grdSalesMargin.Name = "grdSalesMargin"
        Me.grdSalesMargin.Size = New System.Drawing.Size(673, 349)
        Me.grdSalesMargin.TabIndex = 0
        '
        'TCPanel
        '
        Me.TCPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TCPanel.Controls.Add(Me.btnUpdateTC)
        Me.TCPanel.Controls.Add(Me.rtxMessage)
        Me.TCPanel.Controls.Add(Me.rtxLabTesting)
        Me.TCPanel.Controls.Add(Me.rtxQuoteExpiry)
        Me.TCPanel.Controls.Add(Me.rtxOtherIngredientsTablet)
        Me.TCPanel.Controls.Add(Me.rtxOtherIngredientsCapsule)
        Me.TCPanel.Controls.Add(Me.rtxOtherIngredientsPowder)
        Me.TCPanel.Controls.Add(Me.lblMessage)
        Me.TCPanel.Controls.Add(Me.lblLabTestPolicy)
        Me.TCPanel.Controls.Add(Me.lblIngredientPowder)
        Me.TCPanel.Controls.Add(Me.lblQuoteExpiry)
        Me.TCPanel.Controls.Add(Me.lblIngredientCapsule)
        Me.TCPanel.Controls.Add(Me.lblIngredientsTablet)
        Me.TCPanel.Controls.Add(Me.TCHeader)
        Me.TCPanel.Location = New System.Drawing.Point(156, 43)
        Me.TCPanel.Name = "TCPanel"
        Me.TCPanel.Size = New System.Drawing.Size(693, 430)
        Me.TCPanel.TabIndex = 12
        '
        'btnUpdateTC
        '
        Me.btnUpdateTC.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUpdateTC.Location = New System.Drawing.Point(564, 10)
        Me.btnUpdateTC.Name = "btnUpdateTC"
        Me.btnUpdateTC.Size = New System.Drawing.Size(115, 25)
        Me.btnUpdateTC.TabIndex = 161
        Me.btnUpdateTC.Text = "Update T&&C"
        Me.btnUpdateTC.UseVisualStyleBackColor = True
        '
        'rtxMessage
        '
        Me.rtxMessage.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtxMessage.Location = New System.Drawing.Point(160, 311)
        Me.rtxMessage.MaxLength = 800
        Me.rtxMessage.Name = "rtxMessage"
        Me.rtxMessage.Size = New System.Drawing.Size(518, 99)
        Me.rtxMessage.TabIndex = 160
        Me.rtxMessage.Text = ""
        '
        'rtxLabTesting
        '
        Me.rtxLabTesting.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtxLabTesting.Location = New System.Drawing.Point(160, 256)
        Me.rtxLabTesting.MaxLength = 100
        Me.rtxLabTesting.Name = "rtxLabTesting"
        Me.rtxLabTesting.Size = New System.Drawing.Size(518, 50)
        Me.rtxLabTesting.TabIndex = 159
        Me.rtxLabTesting.TabStop = False
        Me.rtxLabTesting.Text = ""
        '
        'rtxQuoteExpiry
        '
        Me.rtxQuoteExpiry.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtxQuoteExpiry.Location = New System.Drawing.Point(160, 203)
        Me.rtxQuoteExpiry.MaxLength = 150
        Me.rtxQuoteExpiry.Name = "rtxQuoteExpiry"
        Me.rtxQuoteExpiry.Size = New System.Drawing.Size(518, 48)
        Me.rtxQuoteExpiry.TabIndex = 158
        Me.rtxQuoteExpiry.TabStop = False
        Me.rtxQuoteExpiry.Text = ""
        '
        'rtxOtherIngredientsTablet
        '
        Me.rtxOtherIngredientsTablet.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtxOtherIngredientsTablet.Location = New System.Drawing.Point(160, 150)
        Me.rtxOtherIngredientsTablet.MaxLength = 200
        Me.rtxOtherIngredientsTablet.Name = "rtxOtherIngredientsTablet"
        Me.rtxOtherIngredientsTablet.Size = New System.Drawing.Size(518, 48)
        Me.rtxOtherIngredientsTablet.TabIndex = 157
        Me.rtxOtherIngredientsTablet.TabStop = False
        Me.rtxOtherIngredientsTablet.Text = ""
        '
        'rtxOtherIngredientsCapsule
        '
        Me.rtxOtherIngredientsCapsule.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtxOtherIngredientsCapsule.Location = New System.Drawing.Point(160, 96)
        Me.rtxOtherIngredientsCapsule.MaxLength = 200
        Me.rtxOtherIngredientsCapsule.Name = "rtxOtherIngredientsCapsule"
        Me.rtxOtherIngredientsCapsule.Size = New System.Drawing.Size(518, 48)
        Me.rtxOtherIngredientsCapsule.TabIndex = 156
        Me.rtxOtherIngredientsCapsule.TabStop = False
        Me.rtxOtherIngredientsCapsule.Text = ""
        '
        'rtxOtherIngredientsPowder
        '
        Me.rtxOtherIngredientsPowder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtxOtherIngredientsPowder.Location = New System.Drawing.Point(160, 44)
        Me.rtxOtherIngredientsPowder.MaxLength = 200
        Me.rtxOtherIngredientsPowder.Name = "rtxOtherIngredientsPowder"
        Me.rtxOtherIngredientsPowder.Size = New System.Drawing.Size(520, 48)
        Me.rtxOtherIngredientsPowder.TabIndex = 155
        Me.rtxOtherIngredientsPowder.TabStop = False
        Me.rtxOtherIngredientsPowder.Text = ""
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.Location = New System.Drawing.Point(21, 311)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(53, 13)
        Me.lblMessage.TabIndex = 6
        Me.lblMessage.Text = "Message:"
        '
        'lblLabTestPolicy
        '
        Me.lblLabTestPolicy.AutoSize = True
        Me.lblLabTestPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLabTestPolicy.Location = New System.Drawing.Point(21, 256)
        Me.lblLabTestPolicy.Name = "lblLabTestPolicy"
        Me.lblLabTestPolicy.Size = New System.Drawing.Size(121, 13)
        Me.lblLabTestPolicy.TabIndex = 5
        Me.lblLabTestPolicy.Text = "Lab Testing Cost Policy:" & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lblIngredientPowder
        '
        Me.lblIngredientPowder.AutoSize = True
        Me.lblIngredientPowder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIngredientPowder.Location = New System.Drawing.Point(21, 44)
        Me.lblIngredientPowder.Name = "lblIngredientPowder"
        Me.lblIngredientPowder.Size = New System.Drawing.Size(133, 13)
        Me.lblIngredientPowder.TabIndex = 4
        Me.lblIngredientPowder.Text = "Other Ingredients(Powder):"
        '
        'lblQuoteExpiry
        '
        Me.lblQuoteExpiry.AutoSize = True
        Me.lblQuoteExpiry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQuoteExpiry.Location = New System.Drawing.Point(21, 203)
        Me.lblQuoteExpiry.Name = "lblQuoteExpiry"
        Me.lblQuoteExpiry.Size = New System.Drawing.Size(121, 13)
        Me.lblQuoteExpiry.TabIndex = 3
        Me.lblQuoteExpiry.Text = "Quote Expiry Disclaimer:"
        '
        'lblIngredientCapsule
        '
        Me.lblIngredientCapsule.AutoSize = True
        Me.lblIngredientCapsule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIngredientCapsule.Location = New System.Drawing.Point(21, 96)
        Me.lblIngredientCapsule.Name = "lblIngredientCapsule"
        Me.lblIngredientCapsule.Size = New System.Drawing.Size(135, 13)
        Me.lblIngredientCapsule.TabIndex = 2
        Me.lblIngredientCapsule.Text = "Other Ingredients(Capsule):"
        '
        'lblIngredientsTablet
        '
        Me.lblIngredientsTablet.AutoSize = True
        Me.lblIngredientsTablet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIngredientsTablet.Location = New System.Drawing.Point(21, 150)
        Me.lblIngredientsTablet.Name = "lblIngredientsTablet"
        Me.lblIngredientsTablet.Size = New System.Drawing.Size(127, 13)
        Me.lblIngredientsTablet.TabIndex = 1
        Me.lblIngredientsTablet.Text = "Other Ingredients(Tablet):"
        '
        'TCHeader
        '
        Me.TCHeader.AutoSize = True
        Me.TCHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TCHeader.Location = New System.Drawing.Point(5, 5)
        Me.TCHeader.Name = "TCHeader"
        Me.TCHeader.Size = New System.Drawing.Size(149, 17)
        Me.TCHeader.TabIndex = 0
        Me.TCHeader.Text = "Terms && Conditions"
        '
        'SalesRepPanel
        '
        Me.SalesRepPanel.Controls.Add(Me.btnDeleteSalesRepo)
        Me.SalesRepPanel.Controls.Add(Me.Label7)
        Me.SalesRepPanel.Controls.Add(Me.txtSalesRep)
        Me.SalesRepPanel.Controls.Add(Me.btnAddSales)
        Me.SalesRepPanel.Controls.Add(Me.DTSalesRep)
        Me.SalesRepPanel.Controls.Add(Me.lblSalesRep)
        Me.SalesRepPanel.Location = New System.Drawing.Point(156, 43)
        Me.SalesRepPanel.Name = "SalesRepPanel"
        Me.SalesRepPanel.Size = New System.Drawing.Size(693, 430)
        Me.SalesRepPanel.TabIndex = 8
        '
        'btnDeleteSalesRepo
        '
        Me.btnDeleteSalesRepo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDeleteSalesRepo.Location = New System.Drawing.Point(478, 35)
        Me.btnDeleteSalesRepo.Name = "btnDeleteSalesRepo"
        Me.btnDeleteSalesRepo.Size = New System.Drawing.Size(102, 23)
        Me.btnDeleteSalesRepo.TabIndex = 13
        Me.btnDeleteSalesRepo.Text = "Delete Sales Rep"
        Me.btnDeleteSalesRepo.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 40)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(75, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Enter keyword"
        '
        'txtSalesRep
        '
        Me.txtSalesRep.Location = New System.Drawing.Point(85, 37)
        Me.txtSalesRep.Name = "txtSalesRep"
        Me.txtSalesRep.Size = New System.Drawing.Size(301, 20)
        Me.txtSalesRep.TabIndex = 11
        '
        'btnAddSales
        '
        Me.btnAddSales.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAddSales.Location = New System.Drawing.Point(587, 35)
        Me.btnAddSales.Name = "btnAddSales"
        Me.btnAddSales.Size = New System.Drawing.Size(92, 23)
        Me.btnAddSales.TabIndex = 10
        Me.btnAddSales.Text = "Add Sales Rep"
        Me.btnAddSales.UseVisualStyleBackColor = True
        '
        'DTSalesRep
        '
        Me.DTSalesRep.AllowUserToAddRows = False
        Me.DTSalesRep.AllowUserToDeleteRows = False
        Me.DTSalesRep.AllowUserToResizeColumns = False
        Me.DTSalesRep.AllowUserToResizeRows = False
        Me.DTSalesRep.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DTSalesRep.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DTSalesRep.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.FirstName, Me.LastName, Me.Phone, Me.Email})
        Me.DTSalesRep.Location = New System.Drawing.Point(7, 65)
        Me.DTSalesRep.Name = "DTSalesRep"
        Me.DTSalesRep.ReadOnly = True
        Me.DTSalesRep.RowHeadersVisible = False
        Me.DTSalesRep.RowTemplate.Height = 25
        Me.DTSalesRep.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DTSalesRep.Size = New System.Drawing.Size(673, 349)
        Me.DTSalesRep.TabIndex = 9
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.FillWeight = 126.9036!
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.ReadOnly = True
        '
        'FirstName
        '
        Me.FirstName.DataPropertyName = "FirstName"
        Me.FirstName.FillWeight = 93.27411!
        Me.FirstName.HeaderText = "First Name"
        Me.FirstName.Name = "FirstName"
        Me.FirstName.ReadOnly = True
        '
        'LastName
        '
        Me.LastName.DataPropertyName = "LastName"
        Me.LastName.FillWeight = 93.27411!
        Me.LastName.HeaderText = "Last Name"
        Me.LastName.Name = "LastName"
        Me.LastName.ReadOnly = True
        '
        'Phone
        '
        Me.Phone.DataPropertyName = "Phone"
        Me.Phone.FillWeight = 93.27411!
        Me.Phone.HeaderText = "Phone"
        Me.Phone.Name = "Phone"
        Me.Phone.ReadOnly = True
        '
        'Email
        '
        Me.Email.DataPropertyName = "Email"
        Me.Email.FillWeight = 93.27411!
        Me.Email.HeaderText = "Email"
        Me.Email.Name = "Email"
        Me.Email.ReadOnly = True
        '
        'lblSalesRep
        '
        Me.lblSalesRep.AutoSize = True
        Me.lblSalesRep.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblSalesRep.Location = New System.Drawing.Point(8, 5)
        Me.lblSalesRep.Name = "lblSalesRep"
        Me.lblSalesRep.Size = New System.Drawing.Size(85, 15)
        Me.lblSalesRep.TabIndex = 8
        Me.lblSalesRep.Text = "Sales Rep  List"
        '
        'btnSaleRep
        '
        Me.btnSaleRep.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSaleRep.Location = New System.Drawing.Point(21, 401)
        Me.btnSaleRep.Name = "btnSaleRep"
        Me.btnSaleRep.Size = New System.Drawing.Size(115, 25)
        Me.btnSaleRep.TabIndex = 11
        Me.btnSaleRep.Text = "Add/Edit Sales Rep"
        Me.btnSaleRep.UseVisualStyleBackColor = True
        '
        'lblSapesRepoID
        '
        Me.lblSapesRepoID.AutoSize = True
        Me.lblSapesRepoID.Location = New System.Drawing.Point(193, 10)
        Me.lblSapesRepoID.Name = "lblSapesRepoID"
        Me.lblSapesRepoID.Size = New System.Drawing.Size(22, 13)
        Me.lblSapesRepoID.TabIndex = 12
        Me.lblSapesRepoID.Text = "NA"
        Me.lblSapesRepoID.Visible = False
        '
        'btnUpdateTerms
        '
        Me.btnUpdateTerms.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUpdateTerms.Location = New System.Drawing.Point(21, 432)
        Me.btnUpdateTerms.Name = "btnUpdateTerms"
        Me.btnUpdateTerms.Size = New System.Drawing.Size(115, 25)
        Me.btnUpdateTerms.TabIndex = 13
        Me.btnUpdateTerms.Text = "Terms && Conditions"
        Me.btnUpdateTerms.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RenameCurrentFormulaToolStripMenuItem, Me.CopyCurrentFormulaToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(208, 48)
        '
        'RenameCurrentFormulaToolStripMenuItem
        '
        Me.RenameCurrentFormulaToolStripMenuItem.Name = "RenameCurrentFormulaToolStripMenuItem"
        Me.RenameCurrentFormulaToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.RenameCurrentFormulaToolStripMenuItem.Text = "Rename Current Formula"
        '
        'CopyCurrentFormulaToolStripMenuItem
        '
        Me.CopyCurrentFormulaToolStripMenuItem.Name = "CopyCurrentFormulaToolStripMenuItem"
        Me.CopyCurrentFormulaToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.CopyCurrentFormulaToolStripMenuItem.Text = "Copy Current Formula"
        '
        'FormulaSettingPnl
        '
        Me.FormulaSettingPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FormulaSettingPnl.Controls.Add(Me.Label8)
        Me.FormulaSettingPnl.Controls.Add(Me.FormulaSettingFreightUnitlbl)
        Me.FormulaSettingPnl.Controls.Add(Me.FormulaSettingFreightlbl)
        Me.FormulaSettingPnl.Controls.Add(Me.FormulaSettingsFreighttxt)
        Me.FormulaSettingPnl.Controls.Add(Me.FormulaSettingBtn)
        Me.FormulaSettingPnl.Location = New System.Drawing.Point(156, 43)
        Me.FormulaSettingPnl.Name = "FormulaSettingPnl"
        Me.FormulaSettingPnl.Size = New System.Drawing.Size(693, 430)
        Me.FormulaSettingPnl.TabIndex = 162
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(13, 12)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(130, 17)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Formula Settings"
        '
        'FormulaSettingFreightUnitlbl
        '
        Me.FormulaSettingFreightUnitlbl.AutoSize = True
        Me.FormulaSettingFreightUnitlbl.Location = New System.Drawing.Point(63, 46)
        Me.FormulaSettingFreightUnitlbl.Name = "FormulaSettingFreightUnitlbl"
        Me.FormulaSettingFreightUnitlbl.Size = New System.Drawing.Size(13, 13)
        Me.FormulaSettingFreightUnitlbl.TabIndex = 3
        Me.FormulaSettingFreightUnitlbl.Text = "₹"
        '
        'FormulaSettingFreightlbl
        '
        Me.FormulaSettingFreightlbl.AutoSize = True
        Me.FormulaSettingFreightlbl.Location = New System.Drawing.Point(21, 46)
        Me.FormulaSettingFreightlbl.Name = "FormulaSettingFreightlbl"
        Me.FormulaSettingFreightlbl.Size = New System.Drawing.Size(39, 13)
        Me.FormulaSettingFreightlbl.TabIndex = 2
        Me.FormulaSettingFreightlbl.Text = "Freight"
        '
        'FormulaSettingsFreighttxt
        '
        Me.FormulaSettingsFreighttxt.Location = New System.Drawing.Point(79, 43)
        Me.FormulaSettingsFreighttxt.Name = "FormulaSettingsFreighttxt"
        Me.FormulaSettingsFreighttxt.Size = New System.Drawing.Size(51, 20)
        Me.FormulaSettingsFreighttxt.TabIndex = 1
        Me.FormulaSettingsFreighttxt.Text = "0.00"
        '
        'FormulaSettingBtn
        '
        Me.FormulaSettingBtn.Cursor = System.Windows.Forms.Cursors.Hand
        Me.FormulaSettingBtn.Location = New System.Drawing.Point(605, 9)
        Me.FormulaSettingBtn.Name = "FormulaSettingBtn"
        Me.FormulaSettingBtn.Size = New System.Drawing.Size(75, 23)
        Me.FormulaSettingBtn.TabIndex = 0
        Me.FormulaSettingBtn.Text = "Update"
        Me.FormulaSettingBtn.UseVisualStyleBackColor = True
        '
        'Dashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(880, 501)
        Me.Controls.Add(Me.FormulaSettingPnl)
        Me.Controls.Add(Me.TCPanel)
        Me.Controls.Add(Me.btnUpdateTerms)
        Me.Controls.Add(Me.SalesMarginPanel)
        Me.Controls.Add(Me.lblSapesRepoID)
        Me.Controls.Add(Me.btnSaleRep)
        Me.Controls.Add(Me.UserPanel)
        Me.Controls.Add(Me.SalesRepPanel)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.FormulaName)
        Me.Controls.Add(Me.MaterialName)
        Me.Controls.Add(Me.Username)
        Me.Controls.Add(Me.btnSalesMargin)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.btnMaterialList)
        Me.Controls.Add(Me.FormulaPanel)
        Me.Controls.Add(Me.MaterialPanel)
        Me.Controls.Add(Me.UserIDLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Dashboard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dashboard"
        Me.FormulaPanel.ResumeLayout(False)
        Me.FormulaPanel.PerformLayout()
        CType(Me.calenderlookup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FilterLookUp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MaterialPanel.ResumeLayout(False)
        Me.MaterialPanel.PerformLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ds, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UserPanel.ResumeLayout(False)
        Me.UserPanel.PerformLayout()
        CType(Me.userGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SalesMarginPanel.ResumeLayout(False)
        Me.SalesMarginPanel.PerformLayout()
        CType(Me.grdSalesMargin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TCPanel.ResumeLayout(False)
        Me.TCPanel.PerformLayout()
        Me.SalesRepPanel.ResumeLayout(False)
        Me.SalesRepPanel.PerformLayout()
        CType(Me.DTSalesRep, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.FormulaSettingPnl.ResumeLayout(False)
        Me.FormulaSettingPnl.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnMaterialList As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents FormulaPanel As Panel
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnSalesMargin As Button
    Friend WithEvents MaterialPanel As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents btnAddMaterial As Button
    Friend WithEvents UserIDLabel As Label
    Friend WithEvents ds As DataSet
    Friend WithEvents Username As Label
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents Cost As DataGridViewTextBoxColumn
    Friend WithEvents btnDeleteFormula As Button
    Friend WithEvents FormulaName As Label
    Friend WithEvents lblVersion As Label
    Friend WithEvents UserPanel As Panel
    Friend WithEvents txtSearchUser As TextBox
    Friend WithEvents lblAddUser As Button
    Friend WithEvents userGrid As DataGridView
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents FilterLookUp As PictureBox
    Friend WithEvents txtFilter As TextBox
    Friend WithEvents FilterList As ListBox
    Friend WithEvents CalenderBox As MonthCalendar
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents SalesRep As DataGridViewTextBoxColumn
    Friend WithEvents DateCretaed As DataGridViewTextBoxColumn
    Friend WithEvents calenderlookup As PictureBox
    Friend WithEvents btnDelateMaterial As Button
    Friend WithEvents MaterialName As Label
    Friend WithEvents btnSaleRep As Button
    Friend WithEvents SalesRepPanel As Panel
    Friend WithEvents lblSalesRep As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents txtSalesRep As TextBox
    Friend WithEvents btnAddSales As Button
    Friend WithEvents DTSalesRep As DataGridView
    Friend WithEvents ID As DataGridViewTextBoxColumn
    Friend WithEvents FirstName As DataGridViewTextBoxColumn
    Friend WithEvents LastName As DataGridViewTextBoxColumn
    Friend WithEvents Phone As DataGridViewTextBoxColumn
    Friend WithEvents Email As DataGridViewTextBoxColumn
    Friend WithEvents lblSapesRepoID As Label
    Friend WithEvents btnDeleteSalesRepo As Button
    Friend WithEvents SalesMarginPanel As Panel
    Friend WithEvents txtPackagingType As TextBox
    Friend WithEvents lblPackagingType As Label
    Friend WithEvents btnAddMargin As Button
    Friend WithEvents lblSalesMargin As Label
    Friend WithEvents grdSalesMargin As DataGridView
    Friend WithEvents TCPanel As Panel
    Friend WithEvents lblLabTestPolicy As Label
    Friend WithEvents lblIngredientPowder As Label
    Friend WithEvents lblQuoteExpiry As Label
    Friend WithEvents lblIngredientCapsule As Label
    Friend WithEvents lblIngredientsTablet As Label
    Friend WithEvents TCHeader As Label
    Friend WithEvents btnUpdateTerms As Button
    Friend WithEvents lblMessage As Label
    Friend WithEvents rtxOtherIngredientsTablet As RichTextBox
    Friend WithEvents rtxOtherIngredientsCapsule As RichTextBox
    Friend WithEvents rtxOtherIngredientsPowder As RichTextBox
    Friend WithEvents rtxQuoteExpiry As RichTextBox
    Friend WithEvents rtxLabTesting As RichTextBox
    Friend WithEvents rtxMessage As RichTextBox
    Friend WithEvents btnUpdateTC As Button
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents CopyCurrentFormulaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RenameCurrentFormulaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FormulaSettingPnl As Panel
    Friend WithEvents FormulaSettingBtn As Button
    Friend WithEvents FormulaSettingFreightlbl As Label
    Friend WithEvents FormulaSettingsFreighttxt As TextBox
    Friend WithEvents FormulaSettingFreightUnitlbl As Label
    Friend WithEvents Label8 As Label
End Class
