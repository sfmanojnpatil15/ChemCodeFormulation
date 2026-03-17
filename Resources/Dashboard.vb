Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class Dashboard

    Dim dsMl As New DataSet
    Dim dsFS As New DataSet
    Dim DbCon As New MySqlConnection
    Dim dbUp As New MySqlCommand
    Dim da As MySqlDataAdapter

    Dim SSQLDbCon As New SqlConnection
    Dim SSQLdbUp As New SqlCommand
    Dim SSQLda As SqlDataAdapter

    'Material List
    Dim cmdML As New MySqlCommand
    Dim daML As New MySqlDataAdapter

    'Formula Settings
    Dim cmdFS As New MySqlCommand
    Dim daFS As New MySqlDataAdapter

    Dim MaterialDV As DataView
    Dim SalesRepDV As DataView
    Dim FormuDV As DataView
    Dim UserDV As DataView
    Public Shared SaleRepId As String = ""
    Public Shared IsEdit As Boolean = False
    Dim FId As String
    Dim currentRowIndex As Integer = -1

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormulaPanel.BringToFront()

        lblVersion.Text = LoginForm.AssemblyVersion

        AddHandler txtFilter.TextChanged, AddressOf txtFilter_TextChanged
        AddHandler FilterLookUp.Click, AddressOf FilterLookUp_Click
        AddHandler TextBox1.TextChanged, AddressOf TextBox1_TextChanged

        AddHandler FormulaSettingsFreighttxt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler FormulaSettingsFreighttxt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler FormulaSettingsFreighttxt.LostFocus, AddressOf txtBox_LostFocus

        ComboBox1.SelectedIndex = 0

        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = FormuDV

        DataGridView2.AutoGenerateColumns = False
        DataGridView2.DataSource = MaterialDV
        DataGridView2.Sort(DataGridView2.Columns(0), ListSortDirection.Ascending)


        PopulateUser()
        userGrid.AutoGenerateColumns = False
        userGrid.DataSource = UserDV

        DTSalesRep.AutoGenerateColumns = False
        DTSalesRep.DataSource = SalesRepDV
        AddHandler ComboBox1.SelectedIndexChanged, AddressOf PopulateFormulas

        SetFormulaSettings()
        PopulateFormulas()
        PopulateMaterial()
        PopulateSalesRep()
        FormulaSettingsFreighttxt.Text = dsFS.Tables("formulasettings")(0).Item("Freight").ToString()
    End Sub

    Private Sub SetFormulaSettings()
        Try
            dsFS = New DataSet
            cmdFS.Connection = DbCon
            DbCon.Open()
            cmdFS.CommandType = CommandType.Text
            Dim queryString1 As String = "SELECT * FROM tblformualsettings"
            daFS.SelectCommand = New MySqlCommand(queryString1, DbCon)
            daFS.Fill(dsFS, "formulasettings")

        Catch ex As Exception
        Finally
            DbCon.Close()
        End Try
    End Sub
    Public Sub PopulateMaterial()
        'Dim dsMl As New DataSet
        dsMl = New DataSet
        cmdML.Connection = DbCon
        DbCon.Open()
        cmdML.CommandType = CommandType.Text

        Dim queryString1 As String = ""

        queryString1 = "SELECT * FROM Material"

        daML.SelectCommand = New MySqlCommand(queryString1, DbCon)
        daML.Fill(dsMl, "materiallist")

        DbCon.Close()

        MaterialDV = New DataView(dsMl.Tables("materiallist"), "", "MaterialName ASC", DataViewRowState.CurrentRows)
        DataGridView2.DataSource = MaterialDV
    End Sub
    Public Sub PopulateFormulas()

        Dim myConnection = New MySqlConnection


        myConnection.ConnectionString = My.Settings.DBCon
        myConnection.Open()
        dbUp.Connection = myConnection
        dbUp.CommandType = CommandType.Text

        dbUp.CommandText = "SELECT FormulaID, FormulaName, FormulaType, EnteredDate,SalesRepId, EnteredDate FROM Formulas WHERE IsInactive = 0"
        da = New MySqlDataAdapter(dbUp.CommandText, DbCon)

        If ds.Tables.Contains("formulas") Then
            ds.Tables.Remove("formulas")
        End If

        da.Fill(ds, "formulas")

        FormuDV = New DataView(ds.Tables("formulas"), "", "FormulaID DESC", DataViewRowState.CurrentRows)

        'Checks the tiem period to be displayed
        Dim Exp As String
        Dim FilterDate As New DateTime
        If ComboBox1.SelectedIndex = 0 Then
            FilterDate = Date.Today.AddDays(-30)
            Exp = "EnteredDate >= '" & FilterDate & "'"
        Else
            FilterDate = Date.Today.AddDays(+1)
            Exp = ""
        End If

        FormuDV = New DataView(ds.Tables("formulas"), Exp, "FormulaID DESC", DataViewRowState.CurrentRows)

        DataGridView1.DataSource = FormuDV

        myConnection.Close()

    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs)
        GetFilterData()
    End Sub
    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex > -1 Then
            Dim fid As Integer = (DataGridView1.CurrentRow.DataBoundItem("FormulaID"))
            Dim isopen As Boolean = False

            For Each form In My.Application.OpenForms
                If (form.tag = fid) Then
                    If form.Visible Then
                        isopen = True
                        form.activate
                    End If
                End If
            Next
            If isopen = False Then
                Dim frm As New Formulator2
                frm.Tag = fid
                frm.FormulaID.Text = fid
                'frm.txtCustName.Text =
                frm.lblQuote.Visible = True
                frm.lblQuote.Text = "Quote Number: " & 1000 + Convert.ToInt32(fid) & ""
                frm.Show()
            End If
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim frm As New Formulator2
            frm.FormulaID.Text = -1
            'frm.ComboBox3.SelectedIndex = 0
            frm.VersionCmbBox.Text = 0
            frm.Name = DateTime.Now
            frm.Text = "New Formula      -       GT Formulator   -   " & DateTime.Now.Year
            frm.Show()
        Catch ex As Exception
            MsgBox("Unable To Open Form", MessageBoxButtons.OK, "Error")
            Helper.WriteLog(ex)
        End Try

    End Sub
    Private Sub DataGridView2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellDoubleClick

        If e.RowIndex > -1 Then
            Dim fid As Integer = (DataGridView2.CurrentRow.DataBoundItem("MaterialID"))

            EditMaterial.MaterialID.Text = fid
            EditMaterial.txtComponentCode.Text = DataGridView2.CurrentRow.DataBoundItem("ComponentCode")
            EditMaterial.TextBox1.Text = DataGridView2.CurrentRow.DataBoundItem("MaterialName")
            EditMaterial.TextBox2.Text = DataGridView2.CurrentRow.DataBoundItem("Supplier")
            EditMaterial.TextBox3.Text = DataGridView2.CurrentRow.DataBoundItem("Price")
            EditMaterial.txtComponentCode.Text = DataGridView2.CurrentRow.DataBoundItem("ComponentCode")
            EditMaterial.txtcategoreyEditMaterial.Text = DataGridView2.CurrentRow.DataBoundItem("Category")
            EditMaterial.txtCasePack.Text = DataGridView2.CurrentRow.DataBoundItem("CasePack")
            EditMaterial.txtVendorCode.Text = DataGridView2.CurrentRow.DataBoundItem("VendorCode")
            EditMaterial.ShowDialog()

        End If

    End Sub
    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        FilterMaterial()
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        FormulaSettingPnl.BringToFront()
    End Sub
    Private Sub Dashboard_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        LoginForm.Dispose()
    End Sub
    Private Sub btnAddMaterial_Click(sender As Object, e As EventArgs) Handles btnAddMaterial.Click
        NewMaterial.ShowDialog()
        FilterMaterial()
    End Sub
    Private Sub btnDeleteFormula_Click(sender As Object, e As EventArgs) Handles btnDeleteFormula.Click
        If FormulaName.Text = "NA" Then
            MessageBox.Show("Select formula to delete", "Message", MessageBoxButtons.OK)
            Exit Sub
        Else
            Dim IsDelete As DialogResult = MessageBox.Show("Do you want to delete Formula '" & FormulaName.Text & "' ?", "Confirmation", MessageBoxButtons.YesNo)
            Try
                If FormulaName.Text <> "NA" And IsDelete = DialogResult.Yes Then
                    DbCon.ConnectionString = My.Settings.DBCon

                    DbCon.Open()

                    Dim tableName As New List(Of String)
                    tableName.Add("formulas")
                    tableName.Add("blendingdetails")
                    tableName.Add("blisterdetails")
                    tableName.Add("bottlepackagingdetails")
                    tableName.Add("encapsulationdetails")
                    tableName.Add("formuladetails")
                    tableName.Add("formulaversions")
                    tableName.Add("salesdetails")
                    tableName.Add("standupbagdetails")
                    tableName.Add("stickpackdetails")
                    tableName.Add("tabletingdetails")
                    tableName.Add("sachetsdetails")
                    For Each table As String In tableName
                        Dim mySqlCommand = New MySqlCommand()
                        Dim deleteQuery = $"DELETE FROM {table} WHERE FormulaID=@FormulaID"
                        mySqlCommand.CommandText = deleteQuery
                        mySqlCommand.Parameters.AddWithValue("@FormulaID", FId)
                        mySqlCommand.Connection = DbCon

                        mySqlCommand.ExecuteNonQuery()
                    Next
                    MessageBox.Show("Formula Delete Successfully...", "Message", MessageBoxButtons.OK)
                    PopulateFormulas()
                    FormulaName.Text = "NA"
                End If

            Catch ex As Exception
                MessageBox.Show("Cannot Connect to Database", "Message", MessageBoxButtons.OK)
            Finally
                DbCon.Close()
            End Try
        End If
    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick, grdSalesMargin.CellContentClick
        Dim FName As String = DataGridView1.CurrentRow.DataBoundItem("FormulaName")
        FId = DataGridView1.CurrentRow.DataBoundItem("FormulaID")
        FormulaName.Text = FName
    End Sub
    Public Sub PopulateUser()
        Try
            Dim userDataSet As DataSet = New DataSet()

            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            Dim query = "SELECT UserID as ID,Username as Username,FName as `First Name`,LName as `Last Name`,Password FROM userlist WHERE Username <> 'Admin'"
            Dim mySqlCommand = New MySqlCommand(query, DbCon)
            daML.SelectCommand = mySqlCommand
            daML.Fill(userDataSet, "UserList")

            UserDV = New DataView(userDataSet.Tables(0), "", "", DataViewRowState.CurrentRows)
            userGrid.DataSource = UserDV
            userGrid.Columns("ID").Visible = False
            userGrid.Columns("Password").Visible = False
            DbCon.Close()
        Catch ex As Exception
            Dim e = ""
        End Try
    End Sub
    Public Sub PopulateSalesRep()
        Try
            Dim SalesRepDataSet As DataSet = New DataSet()

            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            Dim query = "SELECT * FROM salesreps"
            Dim mySqlCommand = New MySqlCommand(query, DbCon)
            daML.SelectCommand = mySqlCommand
            daML.Fill(SalesRepDataSet, "SalesRepData")
            DbCon.Close()
            'daML.Fill(dsMl, "materiallist")

            SalesRepDV = New DataView(SalesRepDataSet.Tables("SalesRepData"), "", "ID ASC", DataViewRowState.CurrentRows)

            DTSalesRep.DataSource = SalesRepDV



        Catch ex As Exception
            Dim e = ""
        End Try

    End Sub
    Private Sub txtSearchUser_TextChanged(sender As Object, e As EventArgs) Handles txtSearchUser.TextChanged
        UserDV.RowFilter = String.Format("Username like '%{0}%'", txtSearchUser.Text)
    End Sub
    Private Sub lblAddUser_Click(sender As Object, e As EventArgs) Handles lblAddUser.Click
        User.ShowDialog()
    End Sub
    Private Sub btnMaterialList_Click(sender As Object, e As EventArgs) Handles btnMaterialList.Click
        MaterialPanel.BringToFront()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        FormulaPanel.BringToFront()
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        UserPanel.BringToFront()
    End Sub
    Private Sub userGrid_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles userGrid.CellDoubleClick
        Dim UserID = userGrid.CurrentRow.DataBoundItem("ID")
        User.lblUserID.Text = UserID
        User.Text = "Edit User"
        User.txtUserName.Enabled = False

        User.chkIsActive.Enabled = True

        User.ShowDialog()

    End Sub
    Private Sub FilterLookUp_Click(sender As Object, e As EventArgs)

        If ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Formula Id" Then
            Dim FormulaData As DataTable = New DataTable()
            FormulaData.TableName = "FormulaData"
            Dim dv As New DataView
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            Dim query = "SELECT FormulaID FROM Formulas WHERE IsInactive = 0"
            Dim mySqlCommand = New MySqlCommand(query, DbCon)
            Using adapter As New MySqlDataAdapter(mySqlCommand)
                adapter.Fill(FormulaData)
            End Using

            dv.Table = FormulaData
            FilterList.DataSource = Nothing
            FilterList.DisplayMember = "FormulaID"
            FilterList.ValueMember = "FormulaID"
            FilterList.DataSource = dv
            FilterList.Visible = True
            DbCon.Close()


        ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Sales Rep Id" Then
            Dim SalesData As DataTable = New DataTable()
            SalesData.TableName = "SalesData"
            Dim dv As New DataView
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            Dim query = "SELECT ID FROM salesreps"
            Dim mySqlCommand = New MySqlCommand(query, DbCon)
            Using adapter As New MySqlDataAdapter(mySqlCommand)
                adapter.Fill(SalesData)
            End Using

            dv.Table = SalesData
            FilterList.DataSource = Nothing
            FilterList.DisplayMember = "ID"
            FilterList.ValueMember = "ID"
            FilterList.DataSource = dv
            FilterList.Visible = True
            DbCon.Close()
        ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Date Created" Then
            CalenderBox.Visible = True



        End If

    End Sub
    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs) Handles ComboBox1.TextChanged
        If ComboBox1.Text = "Date Created" Then
            calenderlookup.Visible = True
            FilterLookUp.Visible = False
            txtFilter.ReadOnly = True
            txtFilter.Text = ""

        ElseIf ComboBox1.Text = "Formula Id" Or ComboBox1.Text = "Sales Rep Id" Then
            calenderlookup.Visible = False
            FilterLookUp.Visible = True
            txtFilter.ReadOnly = False

        Else

            calenderlookup.Visible = False
            FilterLookUp.Visible = False
            txtFilter.ReadOnly = True

        End If

        txtFilter.Text = ""
        FilterList.Visible = False
        CalenderBox.Visible = False
        txtFilter.ReadOnly = True

    End Sub
    Private Sub FilterList_KeyDown(sender As Object, e As KeyEventArgs) Handles FilterList.KeyDown

    End Sub
    Private Sub FilterList_Click(sender As Object, e As EventArgs) Handles FilterList.Click
        RemoveHandler txtFilter.TextChanged, AddressOf txtFilter_TextChanged
        RemoveHandler FilterLookUp.Click, AddressOf FilterLookUp_Click
        txtFilter.Text = FilterList.Text
        'FilterList.Visible = False
        'txtFilter.Text = FilterList.Text
        'FormuDV = New DataView(ds.Tables("formulas"), "", "FormulaID DESC", DataViewRowState.CurrentRows)
        'If Not String.IsNullOrEmpty(txtFilter.Text) Then
        '    If ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Formula Id" Then
        '        FormuDV.RowFilter = String.Format("FormulaID = {0}", txtFilter.Text)

        '    ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Sales Rep Id" Then
        '        'FormuDV.RowFilter = String.Format("LOWER(SalesRepId) = '{0}'", txtFilter.Text.ToLower())
        '        'FormuDV.RowFilter = String.Format("LOWER(SalesRepId) = '{0}'", txtFilter.Text.ToLower())
        '        FormuDV.RowFilter = String.Format("SalesRepId = '{0}'", txtFilter.Text.Trim().ToLower())

        '    ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Date Created" Then
        '        'FormuDV.RowFilter = String.Format("EnteredDate = {0}", txtFilter.Text)
        '        'FormuDV.RowFilter = String.Format("EnteredDate = #{0}#", CDate(txtFilter.Text).ToString("yyyy-MM-dd"))
        '        Dim filterDate As String = CDate(txtFilter.Text).ToString("yyyy-MM-dd")
        '        FormuDV.RowFilter = "EnteredDate >= #" & filterDate & " 00:00:00# AND EnteredDate < #" & filterDate & " 23:59:59#"
        '        'FormuDV.RowFilter = String.Format("DATE(EnteredDate, 'System.String') LIKE '%{0}%'", filterDate)
        '    End If
        'End If
        GetFilterData()
        AddHandler txtFilter.TextChanged, AddressOf txtFilter_TextChanged
        AddHandler FilterLookUp.Click, AddressOf FilterLookUp_Click
    End Sub
    Private Sub txtFilter_TextChanged(sender As Object, e As EventArgs)
        'Handles txtFilter.TextChanged
        'GetFilterData()
        If ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Formula Id" Then
            Dim FormulaData As DataTable = New DataTable()
            FormulaData.TableName = "FormulaData"
            Dim dv As New DataView
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            'Dim query = "SELECT FormulaID FROM Formulas WHERE IsInactive = 0 and " + ""
            Dim query = "SELECT FormulaID FROM Formulas WHERE IsInactive = 0  "
            Dim mySqlCommand = New MySqlCommand(query, DbCon)
            Using adapter As New MySqlDataAdapter(mySqlCommand)
                adapter.Fill(FormulaData)
            End Using

            dv.Table = FormulaData
            FilterList.DataSource = Nothing
            FilterList.DisplayMember = "FormulaID"
            FilterList.ValueMember = "FormulaID"
            FilterList.DataSource = dv
            FilterList.Visible = True
            DbCon.Close()


        ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Sales Rep Id" Then
            Dim SalesData As DataTable = New DataTable()
            SalesData.TableName = "SalesData"
            Dim dv As New DataView
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            Dim query = "SELECT ID FROM salesreps"
            Dim mySqlCommand = New MySqlCommand(query, DbCon)
            Using adapter As New MySqlDataAdapter(mySqlCommand)
                adapter.Fill(SalesData)
            End Using

            dv.Table = SalesData
            FilterList.DataSource = Nothing
            FilterList.DisplayMember = "ID"
            FilterList.ValueMember = "ID"
            FilterList.DataSource = dv
            FilterList.Visible = True
            DbCon.Close()
        ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Date Created" Then
            CalenderBox.Visible = True



        End If

    End Sub
    Private Sub CalenderBox_DateSelected(sender As Object, e As DateRangeEventArgs) Handles CalenderBox.DateSelected
        Dim selectedDate As Date = CalenderBox.SelectionStart

        txtFilter.Text = selectedDate
        GetFilterData()
        CalenderBox.Visible = False
    End Sub
    Private Sub GetFilterData()
        FilterList.Visible = False

        Dim MainData = New DataView

        FormuDV = New DataView(ds.Tables("formulas"), "", "FormulaID DESC", DataViewRowState.CurrentRows)

        If ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Formula Id" Then

            Dim formulatable As DataTable = FormuDV.Table
            Dim formulatableFilter As DataTable = formulatable.Clone()

            Dim FilteredRows = (From row In formulatable.AsEnumerable() Where (If(String.IsNullOrEmpty(TextBox1.Text), True, row.Field(Of String)("FormulaName").ToLower().Contains(TextBox1.Text.ToLower()))) AndAlso (If(String.IsNullOrEmpty(txtFilter.Text), True, row.Field(Of Int32)("FormulaID") = Convert.ToInt32(txtFilter.Text)))
                                Select row).ToList()

            If (FilteredRows.Count > 0) Then

                formulatableFilter.Clear()
                For Each row As DataRow In FilteredRows
                    formulatableFilter.ImportRow(row)
                Next

            End If
            FormuDV = New DataView(formulatableFilter)

        ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Sales Rep Id" Then

            Dim formulatable As DataTable = FormuDV.Table
            Dim formulatableFilter As DataTable = formulatable.Clone()

            Dim FilteredRows = (From row In formulatable.AsEnumerable()
                                Where (If(String.IsNullOrEmpty(TextBox1.Text), True,
                     (Not row.IsNull("FormulaName") AndAlso row.Field(Of String)("FormulaName").ToLower().Contains(TextBox1.Text.ToLower())))) AndAlso
                  (If(String.IsNullOrEmpty(txtFilter.Text), True,
                      (Not row.IsNull("SalesRepId") AndAlso row.Field(Of String)("SalesRepId").ToLower() = txtFilter.Text.Trim().ToLower())))
                                Select row).ToList()


            If (FilteredRows.Any()) Then

                For Each row As DataRow In FilteredRows
                    formulatableFilter.ImportRow(row)
                Next

            End If
            FormuDV = New DataView(formulatableFilter)

        ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "Date Created" Then
            If Not String.IsNullOrEmpty(txtFilter.Text) Then

                Dim filterDate As String = CDate(txtFilter.Text).ToString("yyyy-MM-dd")

                Dim formulatable As DataTable = FormuDV.Table
                Dim formulatableFilter As DataTable = formulatable.Clone()

                Dim FilteredRows = (From row In formulatable.AsEnumerable()
                                    Where (If(String.IsNullOrEmpty(TextBox1.Text), True, row.Field(Of String)("FormulaName").ToLower().Contains(TextBox1.Text.ToLower()))) AndAlso (If(String.IsNullOrEmpty(filterDate), True, row.Field(Of DateTime)("EnteredDate") >= Convert.ToDateTime(filterDate & " 00:00:00"))) AndAlso row.Field(Of DateTime)("EnteredDate") < Convert.ToDateTime(filterDate & " 23:59:59")
                                    Select row).ToList()

                If (FilteredRows.Count > 0) Then
                    formulatableFilter.Clear()
                    For Each row As DataRow In FilteredRows
                        formulatableFilter.ImportRow(row)
                    Next
                End If
                FormuDV = New DataView(formulatableFilter)

            Else
                MessageBox.Show("Select Date First...", "Message", MessageBoxButtons.OK)
                RemoveHandler TextBox1.TextChanged, AddressOf TextBox1_TextChanged
                TextBox1.Text = ""
                AddHandler TextBox1.TextChanged, AddressOf TextBox1_TextChanged
            End If

        ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.SelectedItem.ToString() = "All" Then

            Dim formulatable As DataTable = FormuDV.Table
            Dim formulatableFilter As DataTable = formulatable.Clone()

            Dim FilteredRows = (From row In formulatable.AsEnumerable() Where (If(String.IsNullOrEmpty(TextBox1.Text), True, row.Field(Of String)("FormulaName").ToLower().Contains(TextBox1.Text.ToLower())))
                                Select row).ToList()


            If (FilteredRows.Count > 0) Then
                formulatableFilter.Clear()
                For Each row As DataRow In FilteredRows
                    formulatableFilter.ImportRow(row)
                Next

            End If
            FormuDV = New DataView(formulatableFilter)
        ElseIf ComboBox1.SelectedItem IsNot Nothing AndAlso ComboBox1.Text = "Last 30 Days" Then
            If Not String.IsNullOrEmpty(TextBox1.Text) Then
                Dim FilterDate = Date.Today.AddDays(-30)
                Dim Exp = "EnteredDate >= '" & FilterDate & "' And FormulaName like '" & If(String.IsNullOrEmpty(TextBox1.Text), "", TextBox1.Text) & "%'"

                FormuDV = New DataView(ds.Tables("formulas"), Exp, "FormulaID DESC", DataViewRowState.CurrentRows)
            End If

        End If
        DataGridView1.DataSource = FormuDV

    End Sub
    Private Sub calenderlookup_Click(sender As Object, e As EventArgs) Handles calenderlookup.Click
        RemoveHandler txtFilter.TextChanged, AddressOf txtFilter_TextChanged
        RemoveHandler FilterLookUp.Click, AddressOf FilterLookUp_Click
        'txtFilter.ReadOnly = True
        CalenderBox.Visible = True
        'GetFilterData()
        AddHandler txtFilter.TextChanged, AddressOf txtFilter_TextChanged
        AddHandler FilterLookUp.Click, AddressOf FilterLookUp_Click
    End Sub
    Private Sub btnDelateMaterial_Click(sender As Object, e As EventArgs) Handles btnDelateMaterial.Click
        Dim IsDelete As DialogResult = MessageBox.Show("Do you want to delete Material '" & MaterialName.Text & "' ?", "Confirmation", MessageBoxButtons.YesNo)

        Dim mySqlCommand = New MySqlCommand()
        Try

            If MaterialName.Text <> "NA" And IsDelete = DialogResult.Yes Then
                DbCon.ConnectionString = My.Settings.DBCon

                DbCon.Open()

                Dim SupplierName = DataGridView2.CurrentRow.DataBoundItem("Supplier")

                Dim deleteQuery = "DELETE mtp from tblmaterialtierpicing mtp inner join material m on m.MaterialID=mtp.MaterialID WHERE m.MaterialName=@MaterialName; DELETE FROM material WHERE MaterialName=@MaterialName and Supplier=@Supplier;"
                mySqlCommand.CommandText = deleteQuery
                mySqlCommand.Parameters.AddWithValue("@MaterialName", MaterialName.Text)
                mySqlCommand.Parameters.AddWithValue("@Supplier", SupplierName)
                mySqlCommand.Connection = DbCon

                mySqlCommand.ExecuteNonQuery()
                DbCon.Close()


                PopulateMaterial()
                FilterMaterial()

                MaterialName.Text = "NA"
            End If

        Catch ex As Exception
            MessageBox.Show("Cannot Connect to Database", "Message", MessageBoxButtons.OK)
        Finally
            DbCon.Close()
        End Try
    End Sub
    Private Sub DataGridView2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        Dim MName As String = DataGridView2.CurrentRow.DataBoundItem("MaterialName")
        MaterialName.Text = MName
    End Sub
    Private Sub btnSaleRep_Click(sender As Object, e As EventArgs) Handles btnSaleRep.Click
        SalesRepPanel.BringToFront()

    End Sub
    Private Sub btnAddSales_Click(sender As Object, e As EventArgs) Handles btnAddSales.Click

        SalesRepDetails.txtSalesRepId.Text = ""
        SalesRepDetails.ShowDialog()

    End Sub
    Private Sub DTSalesRep_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DTSalesRep.CellDoubleClick
        IsEdit = True
        SalesRepDetails.txtSalesRepId.Text = DTSalesRep.CurrentRow.DataBoundItem("ID")
        'txtSalesRep.Text = SaleRepId
        'txtSalesRep.Enabled = False
        SalesRepDetails.Text = "Edit Sales Rep"

        SalesRepDetails.ShowDialog()

    End Sub
    Private Sub txtSalesRep_TextChanged(sender As Object, e As EventArgs) Handles txtSalesRep.TextChanged
        SalesRepDV.RowFilter = String.Format("FirstName like '%{0}%'", txtSalesRep.Text)
    End Sub
    Private Sub DTSalesRep_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DTSalesRep.CellClick
        lblSapesRepoID.Text = DTSalesRep.CurrentRow.DataBoundItem("ID")
    End Sub
    Private Sub btnDeleteSalesRepo_Click(sender As Object, e As EventArgs) Handles btnDeleteSalesRepo.Click
        If lblSapesRepoID.Text = "NA" Then
            MessageBox.Show("Select Salesrepo to delete ", "Message", MessageBoxButtons.OK)
        ElseIf lblSapesRepoID.Text <> "NA" Then
            Dim mySqlConnection As New MySqlConnection()
            Try
                Dim Delete = MessageBox.Show("Do you want to delete '" & lblSapesRepoID.Text & "'", "Message", MessageBoxButtons.YesNo)
                If Delete = DialogResult.Yes Then
                    mySqlConnection.ConnectionString = My.Settings.DBCon
                    mySqlConnection.Open()
                    Dim query = "Delete from salesreps where ID=@ID"
                    Dim mySqlCommand = New MySqlCommand(query, mySqlConnection)
                    mySqlCommand.Parameters.AddWithValue("@ID", lblSapesRepoID.Text)
                    mySqlCommand.ExecuteNonQuery()
                    PopulateSalesRep()
                End If
            Catch ex As Exception
                MessageBox.Show("Could not connect to Database", "Message", MessageBoxButtons.OK)
            Finally
                mySqlConnection.Close()
                lblSapesRepoID.Text = "NA"
            End Try
        End If
    End Sub
    Public Function DeleteFormula() As String
        Return ""
    End Function
    Private Sub btnSalesMargin_Click(sender As Object, e As EventArgs) Handles btnSalesMargin.Click
        SalesMarginPanel.BringToFront()
    End Sub
    Private Sub Dashboard_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If LoginForm.IsAdmin = True Then
            'Button6.Visible = True
            Button6.Enabled = True
            btnSaleRep.Enabled = True
        Else
            'Button6.Visible = False
            Button6.Enabled = False
            btnSaleRep.Enabled = False

        End If
    End Sub

    Private Sub btnUpdateTerms_Click(sender As Object, e As EventArgs) Handles btnUpdateTerms.Click

        Try
            Dim term As New DataTable()
            DbCon = New MySqlConnection()
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            dbUp = New MySqlCommand()
            dbUp.CommandType = CommandType.Text
            dbUp.Connection = DbCon
            dbUp.CommandText = "select * from terms"

            da = New MySqlDataAdapter(dbUp)
            da.Fill(term)

            rtxLabTesting.Text = term.Rows(0)("LabTestingCostPolicy")
            rtxMessage.Text = term.Rows(0)("Message")
            rtxQuoteExpiry.Text = term.Rows(0)("QuoteExpiryDisclaimer")
            rtxOtherIngredientsCapsule.Text = term.Rows(0)("OtherIngredientsCapsule")
            rtxOtherIngredientsPowder.Text = term.Rows(0)("OtherIngredientsPowder")
            rtxOtherIngredientsTablet.Text = term.Rows(0)("OtherIngredientsTablet")


            dbUp.ExecuteNonQuery()

        Catch ex As Exception
            Helper.WriteLog(ex)
            MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        Finally
            DbCon.Close()
        End Try

        TCPanel.BringToFront()
    End Sub

    Private Sub btnUpdateTC_Click(sender As Object, e As EventArgs) Handles btnUpdateTC.Click
        Try
            DbCon = New MySqlConnection()
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            dbUp = New MySqlCommand()
            dbUp.CommandType = CommandType.Text
            dbUp.Connection = DbCon
            dbUp.CommandText = "update terms 
            set QuoteExpiryDisclaimer =@QuoteExpiryDisclaimer  
            ,OtherIngredientsTablet=@OtherIngredientsTablet
            ,OtherIngredientsCapsule =@OtherIngredientsCapsule
            ,OtherIngredientsPowder =@OtherIngredientsPowder 
            ,LabTestingCostPolicy =@LabTestingCostPolicy
            ,Message=@Message"

            dbUp.Parameters.AddWithValue("@QuoteExpiryDisclaimer", rtxQuoteExpiry.Text)
            dbUp.Parameters.AddWithValue("@OtherIngredientsTablet", rtxOtherIngredientsTablet.Text)
            dbUp.Parameters.AddWithValue("@OtherIngredientsCapsule", rtxOtherIngredientsCapsule.Text)
            dbUp.Parameters.AddWithValue("@OtherIngredientsPowder", rtxOtherIngredientsPowder.Text)
            dbUp.Parameters.AddWithValue("@LabTestingCostPolicy", rtxLabTesting.Text)
            dbUp.Parameters.AddWithValue("@Message", rtxMessage.Text)

            dbUp.ExecuteNonQuery()
            MessageBox.Show("Terms & Condition updated...", "Message", MessageBoxButtons.OK)
        Catch ex As Exception
            Helper.WriteLog(ex)
            MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        Finally
            DbCon.Close()
        End Try
    End Sub

    Public Sub FilterMaterial()
        'MaterialDV.RowFilter = String.Format("MaterialName Like '{0}%'", TextBox2.Text)
        MaterialDV = New DataView(dsMl.Tables("materiallist"), "", "MaterialName ASC", DataViewRowState.CurrentRows)
        Dim materialtable As DataTable = MaterialDV.Table
        Dim materialtableFilter As DataTable = materialtable.Clone

        Dim FilteredRows = (From row In materialtable.AsEnumerable()
                            Where (
                                If(String.IsNullOrEmpty(TextBox2.Text),
                                True,
                                row.Field(Of String)("MaterialName").IndexOf(TextBox2.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                            ) Select row).ToList()

        If (FilteredRows.Count > 0) Then
            materialtableFilter.Clear()
            For Each row As DataRow In FilteredRows
                materialtableFilter.ImportRow(row)
            Next
        End If
        MaterialDV = New DataView(materialtableFilter)
        DataGridView2.DataSource = MaterialDV
    End Sub

    Private Sub DataGridView1_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles DataGridView1.MouseDown
        If e.Button = MouseButtons.Right Then

            Dim hit As DataGridView.HitTestInfo = DataGridView1.HitTest(e.X, e.Y)
            If hit.Type = DataGridViewHitTestType.Cell Or hit.Type = DataGridViewHitTestType.RowHeader Then
                currentRowIndex = hit.RowIndex
                DataGridView1.ClearSelection()
                DataGridView1.Rows(currentRowIndex).Selected = True
            Else
                currentRowIndex = -1
            End If

            Dim endPoint As Point = New Point(e.X, e.Y)
            ContextMenuStrip1.Show(DataGridView1, endPoint)
        End If
    End Sub

    Public Sub CopyCurrentFormulaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyCurrentFormulaToolStripMenuItem.Click
        Try
            DbCon = New MySqlConnection()
            DbCon.ConnectionString = My.Settings.DBCon

            If currentRowIndex >= 0 Then

                DbCon.Open()

                Dim ID As String = DataGridView1.Rows(currentRowIndex).Cells(0).Value.ToString()

                Dim Cmd As New MySqlCommand()
                Cmd.CommandType = CommandType.StoredProcedure
                Cmd.CommandText = "CopyFormula"
                Cmd.Parameters.AddWithValue("@CurrentFormulaID", ID)
                Cmd.Connection = DbCon

                Cmd.ExecuteNonQuery()
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
            DbCon.Close()
            PopulateFormulas()
        End Try
    End Sub

    Private Sub RenameCurrentFormulaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenameCurrentFormulaToolStripMenuItem.Click
        Try
            Dim FID As String = DataGridView1.Rows(currentRowIndex).Cells(0).Value.ToString()
            Dim FName As String = DataGridView1.Rows(currentRowIndex).Cells(1).Value.ToString()

            Dim frm2 As New FormulaName
            frm2.FormulanameBox.Text = FName
            frm2.Label2.Text = FName
            frm2.Label1.Text = FID
            frm2.ShowDialog()

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub txtBox_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim Ctl As Control = CType(sender, Control)

            Dim PrintingCost = Ctl.Text.Split(".")
            If Ctl.Text.Contains(".") Then
                If PrintingCost(1).Length > 1 Then
                    Ctl.Text = PrintingCost(0) + "." + PrintingCost(1).Substring(0, 2)
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Public Sub txtBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not (Char.IsDigit(CChar(CStr(e.KeyChar))) Or e.KeyChar = "." Or e.KeyChar = Convert.ToChar(8)) Then
            e.Handled = True
        End If

        Dim Ctl As TextBox = CType(sender, TextBox)
        If Ctl.SelectionStart = 0 AndAlso Ctl.SelectionLength = Ctl.Text.Length Then

        Else
            If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And (e.KeyChar <> "."c Or Ctl.Text.Contains(".")) Then
                e.Handled = True
            End If
        End If
        If e.KeyChar = ChrW(Keys.Enter) Then
            FormatTextBox(sender, e)
        End If
    End Sub
    Public Sub FormatTextBox(sender As Object, e As EventArgs)
        Dim Ctl As Control = CType(sender, Control)
        Dim PrintingCost = Ctl.Text.Split(".")
        If Not Ctl.Text.Contains(".") Then
            Ctl.Text = Ctl.Text + ".00"
        ElseIf Ctl.Text.EndsWith(".") Then
            Ctl.Text = Ctl.Text + "00"
        ElseIf PrintingCost(1).Length = 1 Then
            Ctl.Text = Ctl.Text + "0"
        ElseIf Ctl.Text = "." Then
            Ctl.Text = "0.00"
        End If
        If Ctl.Text.StartsWith(".") Then
            Ctl.Text = "0" + Ctl.Text
        End If
    End Sub
    Public Sub txtBox_LostFocus(sender As Object, e As EventArgs)
        FormatTextBox(sender, e)
    End Sub

    Private Sub FormulaSettingBtn_Click(sender As Object, e As EventArgs) Handles FormulaSettingBtn.Click
        Try
            DbCon.ConnectionString = My.Settings.DBCon
            cmdFS = New MySqlCommand
            cmdFS.Connection = DbCon
            DbCon.Open()
            cmdFS.CommandType = CommandType.Text
            cmdFS.CommandText = "UPDATE tblformualsettings SET Freight=@Freight"
            cmdFS.Parameters.Add("@Freight", MySqlDbType.Decimal).Value = FormulaSettingsFreighttxt.Text
            cmdFS.ExecuteNonQuery()
            MessageBox.Show("Freight updated successfully", "Message", MessageBoxButtons.OK)
        Catch ex As Exception
            MessageBox.Show("Error updating freight cost", "Message", MessageBoxButtons.OK)
        Finally
            DbCon.Close()

            SetFormulaSettings()
            FormulaSettingsFreighttxt.Text = dsFS.Tables("formulasettings")(0).Item("Freight").ToString()
        End Try
    End Sub

End Class