Imports System.Threading
Imports System.ComponentModel
Imports System.Drawing.Printing
Imports MySql.Data.MySqlClient
Imports System.Linq
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports BPG_Costing.Dashboard

Public Class Formulator2

    Dim culture As CultureInfo = New CultureInfo("en-IN")

    Public Shared ContinueSaving As Boolean = True
    Public Shared IsSaved As Boolean = False

    Shared MaterialDataTable As New DataTable()

    Dim DbCon As New MySqlConnection
    Dim dbUp As New MySqlCommand
    Dim da As MySqlDataAdapter
    Dim ds As New DataSet

    'FormulaDetails info
    Dim dbUpF As New MySqlCommand
    Dim daF As New MySqlDataAdapter()
    Dim dsF As New DataSet
    Dim BindingSource1 As New BindingSource

    'FormulaVersions info
    Dim dbUpFV As New MySqlCommand
    Dim daFV As New MySqlDataAdapter()
    Dim dsFV As New DataSet

    'Formula info
    Dim dbUpFF As New MySqlCommand
    Dim daFF As New MySqlDataAdapter()
    Public Shared dsFF As New DataSet

    'Formula Blending Info
    Dim cmdBi As New MySqlCommand
    Dim daBi As New MySqlDataAdapter()
    Dim dsBi As New DataSet

    'Formula Tableting Info
    Dim cmdTi As New MySqlCommand
    Dim daTi As New MySqlDataAdapter()
    Dim dsTi As New DataSet

    'Formula Encapsulation Info
    Dim cmdEi As New MySqlCommand
    Dim daEi As New MySqlDataAdapter()
    Dim dsEi As New DataSet

    'Packaging Bottles
    Dim cmdPB As New MySqlCommand
    Dim daPB As New MySqlDataAdapter()
    Dim dsPB As New DataSet

    'Packaging Stick Packs
    Dim cmdPSP As New MySqlCommand
    Dim daPSP As New MySqlDataAdapter()
    Dim dsPSP As New DataSet

    'Packaging Sachets
    Dim cmdPSa As New MySqlCommand
    Dim daPSa As New MySqlDataAdapter()
    Dim dsPSa As New DataSet

    'StandUpBags
    Dim cmdStandupBags As New MySqlCommand
    Dim daStandupBags As New MySqlDataAdapter()
    Dim dsStandupBags As New DataSet

    'Packaging Blisters
    Dim cmdPBlisters As New MySqlCommand
    Dim daPBlisters As New MySqlDataAdapter()
    Dim dsPBlisters As New DataSet

    'Sales info
    Dim cmdsal As New MySqlCommand
    Public Shared daSal As New MySqlDataAdapter()
    Public Shared dsSal As New DataSet

    'Formula Settings
    Dim cmdFS As New MySqlCommand
    Dim daFS As New MySqlDataAdapter()
    Dim dsFS As New DataSet

    Dim cmdsalBox As New MySqlCommand
    Dim daSalBox As New MySqlDataAdapter()
    Public Shared dsSalBox As New DataSet

    Dim cmdsalBags As New MySqlCommand
    Dim daSalBags As New MySqlDataAdapter()
    Public Shared dsSalBags As New DataSet

    Dim SalesDVAuto As DataView

    Public Shared BindingSource2 As New BindingSource
    Dim BindSalesBoxGrid As New BindingSource
    Dim BindSalesBulkStickpacksGrid As New BindingSource

    Dim Read1 As MySqlDataReader
    Dim MySqlDataAdapter As New MySqlDataAdapter()

    Dim sw As Double = 0
    Dim ss As Double = 0

    Dim i As Int16 = 0

    Dim email As String = ""
    Dim phone As String = ""

    Dim PrtngIng As Boolean = True

    Dim RowPIndex As Integer = 0
    Dim RowsPerPage As Integer = 22
    Dim PrintingPage As Integer = 1
    Dim printPack As Boolean = False

    'if bottles, no more than 2 sizes per page
    Dim SizeNo As Integer = 1
    Dim SizePIndex As Integer = -1
    Dim SizesPerPage As Integer = 1

    'Saves Packaging Type 
    Dim PackagingType As String = ""

    'Stores DataRow to be deleted 
    Dim deletedRows() As DataRow

    'Stores Current index of Combobox3
    Dim Combobox3Index As Int16
    Dim Combobox3Text As String

    'Stores Saves Version
    Dim FormulaVersions As New Dictionary(Of String, String)

    ' Y Axis values for Bulk  and Box Headers
    Dim yBulkHeader
    Dim yBoxHeader

    ' X Axis values for Bulk And Box
    Dim xKeys As Int16 = 300
    Dim xValue As Int16 = xKeys + 130
    Dim xUnit As Int16 = xKeys + 160

    ' X Axis values for AdditionalServices
    Dim xAdditionalChargesKeys As Int16 = 800
    Dim xAdditionalChargesValue As Int16 = xAdditionalChargesKeys + 200
    Dim xAdditionalChargesUnit As Int16 = xAdditionalChargesKeys + 150

    Dim Wordbreak = New List(Of String)

    'Stores TotalUnitCost to display correct sales data when saving packaging type
    Dim Temp_ToatlUnitCost As String
    Dim Temp_TextBox8 As String
    Public Shared MaterialAdded As Boolean = False

    'Stores MG  Temp value.
    Dim TempMG As String = ""
    Dim MGIndex As Int16

    'Freight Cost
    Dim FreightCost = ""

    Public Shared Terms As New Dictionary(Of String, String)

    Dim RowIndex As Integer
    Dim IsReplace As Boolean
    Dim IsNewMaterialBelow As Boolean

    Dim FlavourProfile As Double = 0.00
    Dim FlavourProfileBox As Double = 0.00

    WithEvents fr2 As UpdateFreightCost

    Private Sub Formulator2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        lblAdditionalChargesBulk.Text = 0.00

        Dim myForm As New Formulator2()
        Dim myScreen As Screen = Screen.FromControl(myForm)
        Dim area As Rectangle = myScreen.WorkingArea

        Me.Width = area.Width
        Me.Height = area.Height

        If area.Width < 1440 Then
            chkDisplayBag.Location = New Point(1153, 36)
        End If


        lblVersion.Text = LoginForm.AssemblyVersion

        dsF.Namespace = "dsf"
        dsFV.Namespace = "dsfv"
        dsFF.Namespace = "dsFF"
        dsBi.Namespace = "dsBi"
        dsTi.Namespace = "dsTi"
        dsEi.Namespace = "dsEi"
        dsPB.Namespace = "dsPB"
        dsPSP.Namespace = "dsPSP"
        dsPSa.Namespace = "dsPSa"
        dsSal.Namespace = "dsSal"
        dsStandupBags.Namespace = "dsStandupBags"

        'Disable Blisters when FormulaType is Powder, as on formulator load the powder is default selected
        If FormulaTypeCmbBox.Text = "Powder" Then
            BlistersPanel.Visible = False
            If ComboBox3.Items.Contains("Blister") Then
                ComboBox3.Items.Remove("Blister")
            End If
        End If

        PopulateFormulator2()
        BindControls()


        Terms = New Dictionary(Of String, String)

        If FormulaID.Text <> "-1" Then
            Terms.Add("Other Ingredients", dsFF.Tables("formulaF").Rows(0)("OtherIngredients"))
            Terms.Add("LabTestingCostPolicy", dsFF.Tables("formulaF").Rows(0)("LabTestingCostPolicy"))
            Terms.Add("QuoteExpiryDisclaimer", dsFF.Tables("formulaF").Rows(0)("QuoteExpiryDisclaimer"))
            Terms.Add("Message", dsFF.Tables("formulaF").Rows(0)("Message"))
        Else
            Terms.Add("Other Ingredients", "")
            Terms.Add("LabTestingCostPolicy", "")
            Terms.Add("QuoteExpiryDisclaimer", "")
            Terms.Add("Message", "")
        End If


        If FormulaTypeCmbBox.SelectedIndex <> 0 Then
            AddHandler ServingSizeTextBox.KeyPress, AddressOf txtBox_KeyPress_Number

            RemoveHandler TextBox35.TextChanged, AddressOf txtBox_TextChanged
            RemoveHandler TextBox35.KeyPress, AddressOf txtBox_KeyPress
            RemoveHandler TextBox35.LostFocus, AddressOf txtBox_LostFocus
            AddHandler TextBox35.KeyPress, AddressOf txtBox_KeyPress_Number


        Else
            RemoveHandler TextBox35.KeyPress, AddressOf txtBox_KeyPress_Number
            AddHandler TextBox35.TextChanged, AddressOf txtBox_TextChanged
            AddHandler TextBox35.KeyPress, AddressOf txtBox_KeyPress
            AddHandler TextBox35.LostFocus, AddressOf txtBox_LostFocus

        End If

        'txtSachetsShipperCaseCountAmt
        AddHandler txtSachetsShipperCaseCountAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsShipperCaseCountAmt.KeyPress, AddressOf txtBox_KeyPress

        'txtStickPacksShipperCaseCountAmt
        AddHandler txtStickPacksShipperCaseCountAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickPacksShipperCaseCountAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStickPacksShipperCaseCountAmt.LostFocus, AddressOf txtBox_LostFocus

        'txtStickPackBagServings
        'AddHandler txtStickPackBagServings.TextChanged, AddressOf txtBox_TextChanged
        'AddHandler txtStickPackBagServings.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtStickPackBagServings.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSalesRepId.TextChanged, AddressOf txtSalesRepId_TextChanged
        AddHandler SalesRepLookUp.Click, AddressOf SalesRepLookUp_Click

        AddHandler AddMaterial.Click, AddressOf AddMaterial_Click
        'AddHandler txtServing.TextChanged, AddressOf txtBox_TextChanged
        'AddHandler txtServing.KeyPress, AddressOf txtBox_KeyPress_Number


        AddHandler txtStickPackBagServings.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickPackBagServings.KeyPress, AddressOf txtBox_KeyPress_Number

        AddHandler txtSachetsBagQty.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBagQty.KeyPress, AddressOf txtBox_KeyPress_Number

        AddHandler txtStickPackBagQty.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickPackBagQty.KeyPress, AddressOf txtBox_KeyPress_Number

        AddHandler txtStickPackBagQty.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickPackBagQty.KeyPress, AddressOf txtBox_KeyPress_Number

        AddHandler txtBlisterQty.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterQty.KeyPress, AddressOf txtBox_KeyPress_Number

        'AddHandler txtBlisterCount.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterCount.KeyPress, AddressOf txtBox_KeyPress_Number


        'AddHandler txtBlisterCountAmount.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtBlisterCountAmount.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlisterCost.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterCost.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterCost.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlisterPrintingCostAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterPrintingCostAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterPrintingCostAmt.LostFocus, AddressOf txtBox_LostFocus

        'AddHandler txtBlisterBoxCostAmt.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtBlisterBoxCostAmt.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlisterPrintingPlatesAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterPrintingPlatesAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterPrintingPlatesAmt.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetsBagRD.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBagRD.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsBagRD.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlendingFlavourProfileServing.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlendingFlavourProfileServing.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlendingFlavourProfileServing.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlendingFlavourProfileKg.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlendingFlavourProfileKg.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlendingFlavourProfileKg.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtPrintingBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtPrintingBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtPrintingBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlisterPackoutAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterPackoutAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterPackoutAmt.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlisterWaferSeal.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterWaferSeal.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterWaferSeal.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlisterArtPreperationAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterArtPreperationAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterArtPreperationAmt.LostFocus, AddressOf txtBox_LostFocus


        AddHandler txtArtPrepBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtArtPrepBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtArtPrepBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler TextBox6.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox6.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox6.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtlabcost.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtlabcost.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtlabcost.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtStandupBagRD.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStandupBagRD.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStandupBagRD.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlisterBoxRD.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterBoxRD.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterBoxRD.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlisterBulkRD.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterBulkRD.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterBulkRD.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetsBulkRD.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBulkRD.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsBulkRD.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetsBoxRD.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBoxRD.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsBoxRD.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtStickpackBulkRD.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickpackBulkRD.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStickpackBulkRD.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtStickpackBoxRD.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickpackBoxRD.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStickpackBoxRD.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtStickpackBagRD.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickpackBagRD.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStickpackBagRD.LostFocus, AddressOf txtBox_LostFocus


        AddHandler txtBlisterPackoutAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterShrinkWrap.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterWaferSeal.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterOtherAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtOtherCostBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterCaseCount.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShipperCountBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterCaseCountAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShipperCostBulk.TextChanged, AddressOf txtBox_TextChanged

        ' Handlers to Handle Blister Calculation
        AddHandler txtBlisterPackoutAmt.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtBlisterShrinkWrap.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtBlisterWaferSeal.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtBlisterOtherAmt.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtOtherCostBulk.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtBlisterCaseCount.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtShipperCountBulk.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtBlisterCaseCountAmt.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtShipperCostBulk.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtBlisterCount.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtBlisterCountAmount.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtBlisterCost.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtBlisterPrintingCostAmt.TextChanged, AddressOf CalculateBlisterCost
        AddHandler txtAmontDisplaybox.TextChanged, AddressOf CalculateBlisterCost
        'AddHandler TextBox8.TextChanged, AddressOf CalculateBlisterCost


        AddHandler txtBlisterPackoutAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterShrinkWrap.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterWaferSeal.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterOtherAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtOtherCostBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterCaseCountAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtShipperCostBulk.KeyPress, AddressOf txtBox_KeyPress

        AddHandler txtBlisterPackoutAmt.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtBlisterShrinkWrap.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtBlisterWaferSeal.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtBlisterOtherAmt.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtOtherCostBulk.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtBlisterCaseCountAmt.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtShipperCostBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtFillWeight.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtOther.TextChanged, AddressOf txtBox_TextChanged
        'AddHandler txtShipperCaseCount.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShipperCaseAmt.TextChanged, AddressOf txtBox_TextChanged

        AddHandler txtFillWeight.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtOther.LostFocus, AddressOf txtBox_LostFocus
        'AddHandler txtShipperCaseCount.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtShipperCaseAmt.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtFillWeight.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtOther.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtShipperCaseCount.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtShipperCaseAmt.KeyPress, AddressOf txtBox_KeyPress

        AddHandler TextBox1.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox10.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox7.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox2.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox3.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox5.LostFocus, AddressOf txtBox_LostFocus

        AddHandler TextBox1.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox10.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox7.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox2.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox3.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox5.TextChanged, AddressOf txtBox_TextChanged

        AddHandler TextBox1.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox10.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox7.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox2.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox3.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox5.KeyPress, AddressOf txtBox_KeyPress

        AddHandler TextBox27.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickPacksSachets.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtFillWeight.TextChanged, AddressOf txtBox_TextChanged

        AddHandler TextBox28.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox29.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox30.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox31.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox32.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox33.TextChanged, AddressOf txtBox_TextChanged


        AddHandler txtSachetPrintingPlatesBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetPrintingPlatesBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetPrintingPlatesBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtPrintingPlatesStickBag.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtPrintingPlatesStickBag.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtPrintingPlatesStickBag.LostFocus, AddressOf txtBox_LostFocus


        AddHandler txtArtPreStickBag.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtArtPreStickBag.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtArtPreStickBag.LostFocus, AddressOf txtBox_LostFocus


        AddHandler txtSecondaryPackStickBag.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSecondaryPackStickBag.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSecondaryPackStickBag.LostFocus, AddressOf txtBox_LostFocus


        AddHandler txtOtherCostStickBag.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtOtherCostStickBag.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtOtherCostStickBag.LostFocus, AddressOf txtBox_LostFocus


        AddHandler txtShipperCostStickBag.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShipperCostStickBag.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtShipperCostStickBag.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetArtPrepBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetArtPrepBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetArtPrepBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetOtherCost.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetOtherCost.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetOtherCost.LostFocus, AddressOf txtBox_LostFocus


        AddHandler txtSachetShipperCost.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetShipperCost.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetShipperCost.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetShipperCountBulk.KeyPress, AddressOf txtBox_KeyPress_Number

        AddHandler TextBox30.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShrinkWrapSachets.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtWaferSealSachets.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox29.TextChanged, AddressOf txtBox_TextChanged
        'AddHandler txtSachetsShipperCaseCount.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsShipperCaseCountAmt.TextChanged, AddressOf txtBox_TextChanged

        AddHandler TextBox27.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtStickPacksSachets.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtFillWeight.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtBlisterCount.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox28.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox29.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox30.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox31.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox32.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox33.KeyPress, AddressOf txtBox_KeyPress

        AddHandler TextBox30.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtShrinkWrapSachets.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtWaferSealSachets.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox29.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtSachetsShipperCaseCount.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsShipperCaseCountAmt.KeyPress, AddressOf txtBox_KeyPress

        AddHandler TextBox27.LostFocus, AddressOf txtBox_LostFocus
        'AddHandler txtStickPacksSachets.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtFillWeight.LostFocus, AddressOf txtBox_LostFocus
        'AddHandler txtBlisterCount.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox28.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox29.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox30.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox31.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox32.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox33.LostFocus, AddressOf txtBox_LostFocus

        AddHandler TextBox30.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtShrinkWrapSachets.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtWaferSealSachets.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox29.LostFocus, AddressOf txtBox_LostFocus
        'AddHandler txtSachetsShipperCaseCount.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtSachetsShipperCaseCountAmt.LostFocus, AddressOf txtBox_LostFocus

        AddHandler TextBox18.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox19.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox20.TextChanged, AddressOf txtBox_TextChanged

        AddHandler txtStickPackBagPrintingCost.TextChanged, AddressOf txtBox_TextChanged

        'AddHandler txtStickPacks.TextChanged, AddressOf txtBox_TextChanged
        'AddHandler txtShrinkWrapStickPack.TextChanged, AddressOf txtBox_TextChanged
        'AddHandler txtWaferSeal.TextChanged, AddressOf txtBox_TextChanged
        'AddHandler TextBox24.TextChanged, AddressOf txtBox_TextChanged
        ''AddHandler txtStickPacksShipperCaseCountAmt.TextChanged, AddressOf txtBox_TextChanged
        'AddHandler txtStickPacksShipperCaseCount.TextChanged, AddressOf txtBox_TextChanged

        'AddHandler txtStickPacks.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox21.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox22.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox23.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox24.TextChanged, AddressOf txtBox_TextChanged

        AddHandler TextBox18.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox19.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox20.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStickPackBagPrintingCost.KeyPress, AddressOf txtBox_KeyPress

        'AddHandler txtStickPacks.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtShrinkWrapStickPack.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler TextBox23.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtWaferSeal.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler TextBox24.KeyPress, AddressOf txtBox_KeyPress
        ''AddHandler txtStickPacksShipperCaseCountAmt.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtStickPacksShipperCaseCount.KeyPress, AddressOf txtBox_KeyPress

        'AddHandler txtStickPacks.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox21.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox22.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox24.KeyPress, AddressOf txtBox_KeyPress

        AddHandler TextBox18.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox19.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox20.LostFocus, AddressOf txtBox_LostFocus
        AddHandler txtStickPackBagPrintingCost.LostFocus, AddressOf txtBox_LostFocus

        ''AddHandler txtStickPacks.LostFocus, AddressOf txtBox_LostFocus
        'AddHandler txtShrinkWrapStickPack.LostFocus, AddressOf txtBox_LostFocus
        'AddHandler txtWaferSeal.LostFocus, AddressOf txtBox_LostFocus
        'AddHandler TextBox24.LostFocus, AddressOf txtBox_LostFocus
        ''AddHandler txtStickPacksShipperCaseCountAmt.LostFocus, AddressOf txtBox_LostFocus
        'AddHandler txtStickPacksShipperCaseCount.LostFocus, AddressOf txtBox_LostFocus

        'AddHandler txtStickPacks.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox21.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox22.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox23.LostFocus, AddressOf txtBox_LostFocus
        AddHandler TextBox24.LostFocus, AddressOf txtBox_LostFocus

        'AddHandler txtBlisterOtherAmt.TextChanged, AddressOf txtBox_TextChanged
        'AddHandler txtBlisterOtherAmt.KeyPress, AddressOf txtBox_KeyPress
        'AddHandler txtBlisterOtherAmt.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txttoolcostamtblister.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txttoolcostamtblister.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txttoolcostamtblister.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtToolingCostBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtToolingCostBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtToolingCostBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtShipperCostBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShipperCostBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtShipperCostBulk.LostFocus, AddressOf txtBox_LostFocus


        AddHandler txtAmontDisplaybox.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtAmontDisplaybox.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtAmontDisplaybox.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtBlisterCaseCount.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShipperCountBulk.TextChanged, AddressOf txtBox_TextChanged

        'txtQty TextBox
        AddHandler txtQty.KeyPress, AddressOf txtBox_KeyPress_Number
        'txtQty TextBox
        AddHandler txtStickPacksSachets.KeyPress, AddressOf txtBox_KeyPress_Number
        AddHandler txtStickPacks.KeyPress, AddressOf txtBox_KeyPress_Number
        'AddHandler txtSize.KeyPress, AddressOf txtBox_KeyPress_Number
        'To handle only decimal input from user

        'txtlabcost TextBox
        AddHandler txtlabcost.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtlabcost.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtlabcost.LostFocus, AddressOf txtBox_LostFocus
        'txtPrintingCost TextBox
        AddHandler txtPrintingCost.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtPrintingCost.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtPrintingCost.LostFocus, AddressOf txtBox_LostFocus
        'txtWaferSealSachets TextBox
        AddHandler txtWaferSealSachets.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtWaferSealSachets.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtWaferSealSachets.LostFocus, AddressOf txtBox_LostFocus
        'txtWaferSeal TextBox
        AddHandler txtWaferSeal.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtWaferSeal.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtWaferSeal.LostFocus, AddressOf txtBox_LostFocus
        'txtShrinkWrap TextBox
        AddHandler txtShrinkWrapStickPack.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShrinkWrapStickPack.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtShrinkWrapStickPack.LostFocus, AddressOf txtBox_LostFocus
        'txtShrinkWrapSachets Textbox
        AddHandler txtShrinkWrapSachets.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShrinkWrapSachets.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtShrinkWrapSachets.LostFocus, AddressOf txtBox_LostFocus

        'txtBlisterShrinkWrap Textbox
        AddHandler txtBlisterShrinkWrap.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterShrinkWrap.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterShrinkWrap.LostFocus, AddressOf txtBox_LostFocus

        'txtPrintingPlates TextBox
        AddHandler txtPrintingPlates.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtPrintingPlates.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtPrintingPlates.LostFocus, AddressOf txtBox_LostFocus
        'txtArtPreperation TextBox
        AddHandler txtArtPreperation.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtArtPreperation.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtArtPreperation.LostFocus, AddressOf txtBox_LostFocus
        'txtArtPreperation TextBox
        AddHandler txtSecondaryPackoutBag.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSecondaryPackoutBag.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSecondaryPackoutBag.LostFocus, AddressOf txtBox_LostFocus
        ''txtOther TextBox
        AddHandler txtOther.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtOther.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtOther.LostFocus, AddressOf txtBox_LostFocus
        'txtShipperCaseCount TextBox
        AddHandler txtShipperCaseAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtShipperCaseAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtShipperCaseAmt.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtStickPrintingPlateBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickPrintingPlateBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStickPrintingPlateBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtStickArtPrepBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickArtPrepBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStickArtPrepBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtStickOtherCostBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickOtherCostBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStickOtherCostBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtStickShipperCostBulk.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtStickShipperCostBulk.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtStickShipperCostBulk.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtStickShipperCountBulk.KeyPress, AddressOf txtBox_KeyPress_Number


        AddHandler txtBlisterCountAmount.TextChanged, AddressOf txtServing_TextChanged
        AddHandler txtBlisterCountAmount.KeyPress, AddressOf txtBox_KeyPress_Number
        'AddHandler txtBlisterBoxCostAmt.KeyPress, AddressOf txtBox_KeyPress_Number
        'AddHandler txtBlisterBoxCostAmt.TextChanged, AddressOf txtServing_TextChanged
        AddHandler txtBlisterBoxCostAmt.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtBlisterBoxCostAmt.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtBlisterBoxCostAmt.LostFocus, AddressOf txtBox_LostFocus


        'txtServing
        AddHandler txtServing.TextChanged, AddressOf txtServing_TextChanged
        AddHandler txtServing.KeyPress, AddressOf txtBox_KeyPress_Number
        'AddHandler txtServing.LostFocus, AddressOf txtBox_LostFocus

        'TextBox9
        AddHandler TextBox9.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox9.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox9.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetsBagPrintingPlates.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBagPrintingPlates.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsBagPrintingPlates.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetsBagArtPreparation.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBagArtPreparation.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsBagArtPreparation.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetsBagSecondaryPackout.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBagSecondaryPackout.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsBagSecondaryPackout.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetsBagOther.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBagOther.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsBagOther.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetsBagShipperCaseCost.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBagShipperCaseCost.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsBagShipperCaseCost.LostFocus, AddressOf txtBox_LostFocus

        AddHandler txtSachetsBagPrintingCost.TextChanged, AddressOf txtBox_TextChanged
        AddHandler txtSachetsBagPrintingCost.KeyPress, AddressOf txtBox_KeyPress
        AddHandler txtSachetsBagPrintingCost.LostFocus, AddressOf txtBox_LostFocus

        'TextBox12
        AddHandler TextBox12.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox12.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox12.LostFocus, AddressOf txtBox_LostFocus

        'TextBox14
        AddHandler TextBox14.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox14.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox14.LostFocus, AddressOf txtBox_LostFocus

        'TextBox15
        AddHandler TextBox15.TextChanged, AddressOf txtBox_TextChanged
        AddHandler TextBox15.KeyPress, AddressOf txtBox_KeyPress
        AddHandler TextBox15.LostFocus, AddressOf txtBox_LostFocus

        'Check Below		
        AddHandler TextBox39DisplayQunatity.KeyPress, AddressOf txtBox_KeyPress_Number
        AddHandler txtStickPacksShipperCaseCount.KeyPress, AddressOf txtBox_KeyPress_Number
        AddHandler txtSachetsShipperCaseCount.KeyPress, AddressOf txtBox_KeyPress_Number
        AddHandler txtBlisterCaseCount.KeyPress, AddressOf txtBox_KeyPress_Number
        AddHandler txtShipperCountBulk.KeyPress, AddressOf txtBox_KeyPress_Number
        AddHandler txtShipperCountBulk.KeyPress, AddressOf txtBox_KeyPress_Number
        AddHandler txtShipperCaseCount.KeyPress, AddressOf txtBox_KeyPress_Number
        AddHandler txtShipperCountStickBag.KeyPress, AddressOf txtBox_KeyPress_Number

        'Check Below
        AddHandler txtQtyDiplaybox.KeyPress, AddressOf txtBox_KeyPress_Number

        'Sachets Combobox
        AddHandler ComboBox11.TextChanged, AddressOf PritingCostStandardSachets
        AddHandler ComboBox10.TextChanged, AddressOf PritingCostStandardSachets
        AddHandler ComboBox9.TextChanged, AddressOf PritingCostStandardSachets
        AddHandler ComboBox12.TextChanged, AddressOf PritingCostStandardSachets

        'Stickpacks ComboBox
        AddHandler ComboBox6.TextChanged, AddressOf PritingCostStandardStickpaks
        AddHandler ComboBox7.TextChanged, AddressOf PritingCostStandardStickpaks
        AddHandler ComboBox8.TextChanged, AddressOf PritingCostStandardStickpaks
        AddHandler cmbstickpanelColors.TextChanged, AddressOf PritingCostStandardStickpaks

        'Adding Hangler for StickPack Calculation
        AddHandler txtStickPacks.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtShrinkWrapStickPack.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler TextBox23.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtWaferSeal.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler TextBox24.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtStickPacksShipperCaseCountAmt.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtStickPacksShipperCaseCount.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtStickPackBagPrintingCost.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtStickShipperCostBulk.TextChanged, AddressOf CalculateStickPacksCost
        'AddHandler TextBox17.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler TextBox18.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler TextBox19.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler TextBox20.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtSecondaryPackStickBag.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtOtherCostStickBag.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtShipperCountStickBag.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtShipperCostStickBag.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler txtStickShipperCountBulk.TextChanged, AddressOf CalculateStickPacksCost
        'AddHandler TextBox8.TextChanged, AddressOf CalculateStickPacksCost
        'AddHandler ServingSizeTextBox.TextChanged, AddressOf CalculateStickPacksCost
        AddHandler TextBox16.TextChanged, AddressOf CalculateStickPacksCost

        'Sachet
        AddHandler TextBox27.TextChanged, AddressOf CalculateSachetsCost
        AddHandler TextBox28.TextChanged, AddressOf CalculateSachetsCost
        AddHandler TextBox30.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtShrinkWrapSachets.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtWaferSealSachets.TextChanged, AddressOf CalculateSachetsCost
        AddHandler TextBox29.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtSachetsShipperCaseCount.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtSachetsShipperCaseCountAmt.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtSachetShipperCountBulk.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtSachetShipperCost.TextChanged, AddressOf CalculateSachetsCost
        AddHandler TextBox35.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtStickPacksSachets.TextChanged, AddressOf CalculateSachetsCost
        AddHandler TextBox33.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtSachetsBagPrintingCost.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtSachetsBagSecondaryPackout.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtSachetsBagOther.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtSachetsBagShipperCaseCount.TextChanged, AddressOf CalculateSachetsCost
        AddHandler txtSachetsBagShipperCaseCost.TextChanged, AddressOf CalculateSachetsCost

        'bag
        AddHandler txtFillWeight.TextChanged, AddressOf CalculateStandUpBagCost
        AddHandler txtOther.TextChanged, AddressOf CalculateStandUpBagCost
        AddHandler txtShipperCaseAmt.TextChanged, AddressOf CalculateStandUpBagCost
        AddHandler txtShipperCaseCount.TextChanged, AddressOf CalculateStandUpBagCost
        AddHandler txtPrintingCost.TextChanged, AddressOf CalculateStandUpBagCost
        AddHandler txtSecondaryPackoutBag.TextChanged, AddressOf CalculateStandUpBagCost

        AddHandler TextBox8.TextChanged, AddressOf TextBox8_TextChanged
        'Adding Handler to Add currency format to material textboxes
        AddHandler txtQtyDiplaybox.KeyPress, AddressOf txtCurrencyFormat
        AddHandler txtQty.KeyPress, AddressOf txtCurrencyFormat
        AddHandler TextBox39DisplayQunatity.KeyPress, AddressOf txtCurrencyFormat
        AddHandler ComboBox8.KeyPress, AddressOf txtCurrencyFormat
        AddHandler ComboBox8.LostFocus, AddressOf txtCurrencyFormatCMB
        AddHandler txtStickPackBagQty.KeyPress, AddressOf txtCurrencyFormat
        AddHandler ComboBox9.KeyPress, AddressOf txtCurrencyFormat
        AddHandler ComboBox9.LostFocus, AddressOf txtCurrencyFormatCMB
        AddHandler TextBox38DisplayQty.KeyPress, AddressOf txtCurrencyFormat

        AddHandler txtQtyDiplaybox.LostFocus, AddressOf txtCurrencyFormatCMB
        AddHandler txtQty.LostFocus, AddressOf txtCurrencyFormatCMB
        AddHandler TextBox39DisplayQunatity.LostFocus, AddressOf txtCurrencyFormatCMB
        AddHandler txtStickPackBagQty.LostFocus, AddressOf txtCurrencyFormatCMB
        AddHandler TextBox38DisplayQty.LostFocus, AddressOf txtCurrencyFormatCMB

        AddHandler ComboBox9.SelectedIndexChanged, AddressOf txtCurrencyFormatCMB
        AddHandler ComboBox9.SelectedIndexChanged, AddressOf txtCurrencyFormatCMB

        DisplaySalesRepData()
        If FormulaID.Text <> -1 Then
            FormulaTypeCmbBox.Enabled = False

        End If

    End Sub

    Private Sub PopulateFormulator2()
        Try
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            dbUp.Connection = DbCon
            dbUp.CommandType = CommandType.Text
            ds = New DataSet
            dsFF = New DataSet
            dsBi = New DataSet
            dsTi = New DataSet
            dsEi = New DataSet
            dsF = New DataSet
            dsFV = New DataSet
            dsPB = New DataSet
            dsPSP = New DataSet
            dsPSa = New DataSet
            dsSal = New DataSet

            dsSalBox = New DataSet
            dsSalBags = New DataSet

            dsStandupBags = New DataSet
            dsPBlisters = New DataSet
            MaterialDataTable = New DataTable()

            dbUp.CommandText = "SELECT * FROM Material WHERE Category = 'RM' ORDER BY MaterialName"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "ingredients")
            'Changes by Payal P--Start : Added new DataGridTableStyle data into main dataset
            dbUp.CommandText = "SELECT * FROM salesreps"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "salesreps")

            dbUp.CommandText = "SELECT * FROM xls_bottlelist ORDER BY MaterialName"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "xls_bottlelist")

            dbUp.CommandText = "SELECT * FROM xls_stickpackslist"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "xls_stickpackslist")

            dbUp.CommandText = "SELECT * FROM xls_sachetlist"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "xls_sachetlist")

            dbUp.CommandText = "SELECT * FROM xls_lidslist ORDER BY MaterialName"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "xls_lidslist")

            dbUp.CommandText = "SELECT * FROM xls_scooplist ORDER BY ScoopMaterialName"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "xls_scooplist")

            dbUp.CommandText = "SELECT * FROM xls_shipperlist ORDER BY ShipperMaterialName"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "xls_shipperlist")

            'Changes Sail-Adarsh
            dbUp.CommandText = "SELECT * FROM material ORDER BY MaterialName"

            MySqlDataAdapter = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            MySqlDataAdapter.Fill(MaterialDataTable)
            'Changes by Payal P--End :

            dbUp.CommandText = "SELECT * FROM Material WHERE Category <> 'RM' ORDER BY MaterialName"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "material")

            dbUp.CommandText = "SELECT * FROM StickPackPricing"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "stickpackspricing")

            dbUp.CommandText = "SELECT * FROM SachetsPricing"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "sachetspricing")

            dbUp.CommandText = "SELECT * FROM MySettings"
            da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            da.Fill(ds, "settings")

            dbUpFF.Connection = DbCon
            dbUpFF.CommandType = CommandType.Text

            Dim queryString1 As String = "Select  FormulaID,FormulaName,FormulaType,FormulaType,PackagingFormat,ServingSize,ServingWeight,EnteredDate,EnteredBy,UpdatedDate,IsInactive,
          IFNULL(CustomerName, '')  AS CustomerName,IFNULL(SalesRepId, '')AS SalesRepId,IFNULL(Contact, '')  AS Contact ,OtherIngredients,LabTestingCostPolicy,QuoteExpiryDisclaimer,Message FROM Formulas WHERE FormulaID = " & FormulaID.Text
            'Dim queryString1 As String = "SELECT *,IFNULL(CustomerName, ''),IFNULL(SalesRepId, ''),IFNULL(Contact, ''),FROM Formulas WHERE FormulaID = " & FormulaID.Text
            daFF.SelectCommand = New MySqlCommand(queryString1, DbCon)
            daFF.Fill(dsFF, "formulaF")

            cmdBi.Connection = DbCon
            cmdBi.CommandType = CommandType.Text
            Dim queryString5 As String = "SELECT * FROM BlendingDetails WHERE FormulaID = " & FormulaID.Text
            daBi.SelectCommand = New MySqlCommand(queryString5, DbCon)
            daBi.Fill(dsBi, "blending")

            cmdTi.Connection = DbCon
            cmdTi.CommandType = CommandType.Text
            Dim queryString3 As String = "SELECT * FROM TabletingDetails WHERE FormulaID = " & FormulaID.Text
            daTi.SelectCommand = New MySqlCommand(queryString3, DbCon)
            daTi.Fill(dsTi, "tableting")

            cmdEi.Connection = DbCon
            cmdEi.CommandType = CommandType.Text
            Dim queryString4 As String = "SELECT * FROM EncapsulationDetails WHERE FormulaID = " & FormulaID.Text
            daEi.SelectCommand = New MySqlCommand(queryString4, DbCon)
            daEi.Fill(dsEi, "encapsulation")

            dbUpF.Connection = DbCon
            dbUpF.CommandType = CommandType.Text
            Dim queryString As String = "SELECT * FROM FormulaDetails WHERE FormulaID = " & FormulaID.Text & " ORDER BY IngredientIndex ASC"
            daF.SelectCommand = New MySqlCommand(queryString, DbCon)
            daF.Fill(dsF, "formula")

            dbUpFV.Connection = DbCon
            dbUpFV.CommandType = CommandType.Text
            Dim queryString8 As String = "SELECT * FROM FormulaVersions WHERE FormulaID = " & FormulaID.Text & " ORDER BY VersionNumber ASC"
            daFV.SelectCommand = New MySqlCommand(queryString8, DbCon)
            daFV.Fill(dsFV, "versions")

            cmdPB.Connection = DbCon
            cmdPB.CommandType = CommandType.Text
            Dim queryString2 As String = "SELECT * FROM BottlePackagingDetails WHERE FormulaID = " & FormulaID.Text
            daPB.SelectCommand = New MySqlCommand(queryString2, DbCon)
            daPB.Fill(dsPB, "bottles")

            Dim queryStandupBags As String = "SELECT * FROM StandUpBagDetails Where FormulaId =" & FormulaID.Text & " Order by StandUpBagID desc"
            daStandupBags.SelectCommand = New MySqlCommand(queryStandupBags, DbCon)
            daStandupBags.Fill(dsStandupBags, "StandUpBag")

            cmdPSP.Connection = DbCon
            cmdPSP.CommandType = CommandType.Text
            Dim queryString6 As String = "SELECT * FROM StickPackDetails WHERE FormulaID = " & FormulaID.Text
            daPSP.SelectCommand = New MySqlCommand(queryString6, DbCon)
            daPSP.Fill(dsPSP, "stickpacks")

            cmdPBlisters.Connection = DbCon
            cmdPBlisters.CommandType = CommandType.Text
            Dim queryBlister As String = "SELECT * FROM blisterdetails WHERE FormulaID = " & FormulaID.Text
            daPBlisters.SelectCommand = New MySqlCommand(queryBlister, DbCon)
            daPBlisters.Fill(dsPBlisters, "blisters")

            cmdPSa.Connection = DbCon
            cmdPSa.CommandType = CommandType.Text
            Dim queryString7 As String = "SELECT * FROM SachetsDetails WHERE FormulaID = " & FormulaID.Text
            daPSa.SelectCommand = New MySqlCommand(queryString7, DbCon)
            daPSa.Fill(dsPSa, "sachets")

            cmdFS.CommandText = "SELECT * FROM tblformualsettings "
            daFS = New MySqlDataAdapter(cmdFS.CommandText, DbCon)
            daFS.Fill(dsFS, "FormulaSettings")
            FreightCost = dsFS.Tables("FormulaSettings")(0).Item("Freight").ToString()

            cmdsal.CommandText = "SELECT * FROM SalesDetails WHERE TYPE='BULK' AND FormulaID=" & FormulaID.Text
            daSal = New MySqlDataAdapter(cmdsal.CommandText, DbCon)
            daSal.Fill(dsSal, "salestxns")

            cmdsalBox.CommandText = "SELECT * FROM SalesDetails WHERE TYPE='BOX' AND FormulaID=" & FormulaID.Text
            daSalBox = New MySqlDataAdapter(cmdsalBox.CommandText, DbCon)
            daSalBox.Fill(dsSalBox, "salesBoxTxns")


            cmdsalBags.CommandText = "SELECT * FROM SalesDetails WHERE TYPE='BULKBAGS' AND FormulaID=" & FormulaID.Text
            daSalBags = New MySqlDataAdapter(cmdsalBags.CommandText, DbCon)
            daSalBags.Fill(dsSalBags, "salesBagsTxns")
            Dim dataRows As DataRowCollection = dsFV.Tables("versions").Rows
            For Each dataRow As DataRow In dataRows
                FormulaVersions(dataRow("VersionNumber").ToString()) = dataRow("PackagingType").ToString()
            Next

            DbCon.Close()

            lblQuoteNumber.Text = If(FormulaID.Text = "-1", "NA", Convert.ToInt32(FormulaID.Text))

            If lblQuoteNumber.Text = "NA" Then
                Button9.Enabled = False
                btnDeleteFormulaVersion.Enabled = False
            Else
                btnDeleteFormulaVersion.Enabled = True
                Button9.Enabled = True
            End If

            'Checks if sales data exists for existing formula
            If FormulaID.Text <> "-1" Then
                Dim DefaultVersion As DataView = New DataView(dsFV.Tables("versions"), "IsDefault = 1", "", DataViewRowState.CurrentRows)
                Dim Formula As DataView = New DataView(dsFF.Tables("formulaF"), "", "", DataViewRowState.CurrentRows)

                Dim FormulaType = Formula(0)("FormulaType").ToString()
                Dim PackagingFormat = Convert.ToInt16(Formula(0)("PackagingFormat").ToString())
                Dim ServingSize = Convert.ToDecimal(Formula(0)("ServingSize"))

                Dim VersionNumber = Convert.ToInt16(DefaultVersion(0)("VersionNumber").ToString())
                Dim DisplayBox = Convert.ToBoolean(DefaultVersion(0)("IsBox"))
                Dim DisplayBag = Convert.ToBoolean(DefaultVersion(0)("IsBag"))
                Dim PackagingType = DefaultVersion(0)("PackagingType").ToString()

                Dim UnitSizeBottle = 0
                If PackagingType = "Bottles" Then
                    Dim ValidRows = dsPB.Tables("bottles").AsEnumerable().Where(Function(c) c.RowState <> DataRowState.Deleted)
                    Dim UnitSizes = (From c In ValidRows Where c.Item("VersionNumber") = VersionNumber Select c.Item("SizeCount")).Distinct()
                    For Each Unit In UnitSizes
                        UnitSizeBottle = Unit
                    Next
                End If

                CheckSalesData(Convert.ToInt32(FormulaID.Text), PackagingFormat, VersionNumber, DisplayBox, DisplayBag, FormulaType, PackagingType, ServingSize, UnitSizeBottle)

                If dsSal.HasChanges() Then
                    dsSal_Insert()
                    dsSal.Tables.Clear()
                    cmdsal.CommandText = "SELECT * FROM SalesDetails WHERE TYPE='BULK' AND FormulaID=" & FormulaID.Text
                    daSal = New MySqlDataAdapter(cmdsal.CommandText, DbCon)
                    daSal.Fill(dsSal, "salestxns")
                End If

                If DisplayBag Then
                    If dsSalBags.HasChanges() Then
                        daSalBag_Insert()
                        dsSalBags.Tables.Clear()
                        cmdsalBags.CommandText = "SELECT * FROM SalesDetails WHERE TYPE='BULKBAGS' AND FormulaID=" & FormulaID.Text
                        daSalBags = New MySqlDataAdapter(cmdsalBags.CommandText, DbCon)
                        daSalBags.Fill(dsSalBags, "salesBagsTxns")
                    End If
                ElseIf DisplayBox Then
                    If dsSalBox.HasChanges() Then
                        dsSalBox_Insert()
                        dsSalBox.Tables.Clear()
                        cmdsalBox.CommandText = "SELECT * FROM SalesDetails WHERE TYPE='BOX' AND FormulaID=" & FormulaID.Text
                        daSalBox = New MySqlDataAdapter(cmdsalBox.CommandText, DbCon)
                        daSalBox.Fill(dsSalBox, "salesBoxTxns")
                    End If
                End If

            End If


            ContinueSaving = True
            IsSaved = False

            lblQuoteNumber.Text = If(FormulaID.Text = "-1", "NA", Convert.ToInt32(FormulaID.Text))
            ContinueSaving = True
            IsSaved = False

            With dsF.Tables("formula").Columns("FormulaTxnID")
                .AutoIncrement = True
                .AutoIncrementSeed = -1
                .AutoIncrementStep = -1
            End With
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub BindControls(Optional ISMgChanged = False)
        Try
            BindingSource1.DataSource = dsF.Tables("formula")

            DataGridView1.AutoGenerateColumns = False
            For Each col In DataGridView1.Columns
                col.sortmode = DataGridViewColumnSortMode.NotSortable
            Next

            BindingSource2.DataSource = dsSal.Tables("salestxns")
            BindSalesBulkStickpacksGrid.DataSource = dsSalBags.Tables("salesBagsTxns")
            BindSalesBoxGrid.DataSource = dsSalBox.Tables("salesBoxTxns")

            gridBulkSales.AutoGenerateColumns = False
            gridBulkSales.DataSource = BindingSource2

            gridBulkSales.Columns(0).DefaultCellStyle.Format = "N0"
            gridBulkSales.Columns(0).DefaultCellStyle.FormatProvider = culture

            gridBoxSales.AutoGenerateColumns = False
            gridBoxSales.DataSource = BindSalesBoxGrid
            gridBoxSales.Columns(0).DefaultCellStyle.Format = "N0"
            gridBoxSales.Columns(0).DefaultCellStyle.FormatProvider = culture


            gridbBulkSalesBags.AutoGenerateColumns = False
            gridbBulkSalesBags.DataSource = BindSalesBulkStickpacksGrid
            gridbBulkSalesBags.Columns(0).DefaultCellStyle.Format = "N0"
            gridbBulkSalesBags.Columns(0).DefaultCellStyle.FormatProvider = culture

            For Each col In gridBulkSales.Columns
                col.sortmode = DataGridViewColumnSortMode.NotSortable
            Next

            For Each col In gridBoxSales.Columns
                col.sortmode = DataGridViewColumnSortMode.NotSortable
            Next

            For Each col In gridbBulkSalesBags.Columns
                col.sortmode = DataGridViewColumnSortMode.NotSortable
            Next

            If FormulaID.Text = -1 Then
                'New formula
                'AddHandler txtSalesRepId.TextChanged, AddressOf txtSalesRepId_TextChanged
                'AddHandler SalesRepLookUp.Click, AddressOf SalesRepLookUp_Click
            Else
                '*** Opens a formula

                'Determine standard version
                VersionCmbBox.Items.Clear()
                Dim VersionSet As Boolean = False
                For Each v As DataRow In dsFV.Tables("versions").Rows
                    VersionCmbBox.Items.Add(v.Item("VersionNumber"))
                    If v.Item("IsDefault") = 1 Then
                        Dim index As Int16 = VersionCmbBox.FindStringExact(v.Item("VersionNumber").ToString())
                        VersionCmbBox.SelectedIndex = index
                        VersionSet = True
                        VersionDescriptionTxt.Text = v.Item("VersionDescription")
                        If dsFF.Tables("formulaF").Rows(0).Item("FormulaType") <> "Powder" Then
                            ServingSizeTextBox.Text = Format(dsFV.Tables("versions").Rows(0).Item("ServingSize"), "N0")
                        Else
                            ServingSizeTextBox.Text = Format(dsFV.Tables("versions").Rows(0).Item("ServingSize"), "N2")
                        End If
                    End If
                Next

                If dsFV.Tables("versions").Rows.Count = 0 Then
                    Exit Sub
                ElseIf VersionSet = False Then
                    VersionCmbBox.SelectedIndex = VersionCmbBox.Items.Count - 1
                End If

                AddHandler FormulaTypeCmbBox.SelectedIndexChanged, AddressOf FormulaTypeCmbBox_SelectedIndexChanged
                If dsFF.Tables("formulaF").Rows(0).Item("FormulaType") = "powder" Or dsFF.Tables("formulaF").Rows(0).Item("FormulaType") = "Powder" Then
                    FormulaTypeCmbBox.SelectedIndex = 0
                    ServingSizeTextBox.Enabled = False
                ElseIf dsFF.Tables("formulaF").Rows(0).Item("FormulaType") = "Capsule" Then
                    FormulaTypeCmbBox.SelectedIndex = 1
                    ServingSizeTextBox.Enabled = True
                ElseIf dsFF.Tables("formulaF").Rows(0).Item("FormulaType") = "Tablet" Then
                    FormulaTypeCmbBox.SelectedIndex = 2
                    ServingSizeTextBox.Enabled = True
                End If

                Me.Name = dsFF.Tables("formulaF").Rows(0).Item("FormulaName")
                Me.txtCustName.Text = If(String.IsNullOrEmpty(txtCustName.Text), dsFF.Tables("formulaF").Rows(0).Item("CustomerName"), txtCustName.Text)
                Me.txtContact.Text = If(String.IsNullOrEmpty(txtContact.Text), dsFF.Tables("formulaF").Rows(0).Item("Contact"), txtContact.Text)
                Me.txtSalesRepId.Text = If(String.IsNullOrEmpty(txtSalesRepId.Text), dsFF.Tables("formulaF").Rows(0).Item("SalesRepId"), txtSalesRepId.Text)
                Me.Text = dsFF.Tables("formulaF").Rows(0).Item("FormulaName") & "     -   GT Formulator " & DateTime.Now.Year

                Panel2.Enabled = True
                Panel1.Enabled = True
                VersionCmbBox.Enabled = True
                VersionDescriptionTxt.Enabled = True
                Button9.Enabled = True
                btnDeleteFormulaVersion.Enabled = True
                DisplayFormula()
                For j As Integer = 0 To DataGridView1.Rows.Count - 1
                    DisplaySelectedCostValue(j)
                    DisplayCostType(j)
                    DisplayCostColor(j)
                    'FormulaCalculations(j, ISMgChanged)
                    CalculateServingWeight()
                    FormulaCost(j)
                Next

                'AddHandler ComboBox3.SelectedIndexChanged, AddressOf ComboBox3_SelectedIndexChanged
                Dim test As String = dsFF.Tables("formulaF").Rows(0).Item("PackagingFormat")
                'ComboBox3.SelectedIndex = dsFF.Tables("formulaF").Rows(0).Item("PackagingFormat")
                Dim dataView As DataView = New DataView(dsFV.Tables("versions"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), 1, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
                If dataView.Count > 0 Then
                    Dim Index As Int16 = ComboBox3.FindStringExact(If(IsDBNull(dataView(0).Item("PackagingType")), -1, dataView(0).Item("PackagingType")))
                    ComboBox3.SelectedIndex = Index
                End If

                'If ComboBox4.Items.Count() = 0 Then
                '    If dsPB.Tables("bottles").Rows.Count > 0 Then
                '        Dim Sizes = (From c In dsPB.Tables("bottles") Select c.Item("SizeCount")).Distinct()
                '        For Each s In Sizes
                '            ComboBox4.Items.Add(s)
                '        Next
                '        Dim row As DataRow = dsPB.Tables("bottles").Select("IsDefault = " & 1).FirstOrDefault()
                '        If Not row Is Nothing Then
                '            ComboBox4.Text = row.Item("SizeCount")
                '        End If
                '    End If
                'End If
                DisplaySales()

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub Formulator2_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            If FormulaID.Text = -1 Then
                FormulaTypeCmbBox.DroppedDown = True
            End If
            BagsPanel.AutoScroll = True
            StickPacksPanel.AutoScroll = True
            '*** Add Handlers
            AddHandler FormulaTypeCmbBox.SelectedIndexChanged, AddressOf FormulaTypeCmbBox_SelectedIndexChanged
            AddHandler VersionCmbBox.SelectedIndexChanged, AddressOf VersionCmbBox_SelectedIndexChanged

            AddHandler DataGridView1.CellEndEdit, AddressOf DataGridView1_CellEndEdit

            SetComboBox3Index()
            SetPackagingType()
            DisplaySalesData()
            If FormulaTypeCmbBox.SelectedIndex = 0 Then
                DisplayBlending(VersionCmbBox.Text)
            ElseIf FormulaTypeCmbBox.SelectedIndex = 1 Then
                DisplayEncapsulation(VersionCmbBox.Text)
            ElseIf FormulaTypeCmbBox.SelectedIndex = 2 Then
                DisplayTableting(VersionCmbBox.Text)
            End If

            AcceptAllDbChanges()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub DisplayFormula()
        Try
            If VersionCmbBox.SelectedIndex <> -1 Then
                BindingSource1.Filter = "VersionID =" & VersionCmbBox.Text
                BindingSource1.Sort = "IngredientIndex"
                BindingSource1.DataSource = dsF.Tables("formula")
                DataGridView1.DataSource = BindingSource1

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub NewVersion()
        Try
            Dim verRow As DataRow = dsFV.Tables("versions").NewRow()
            verRow("FormulaID") = FormulaID.Text
            If VersionCmbBox.Items.Count > 0 Then
                Dim maxVersionNumber = VersionCmbBox.Items.Cast(Of Integer).Max()
                verRow("VersionNumber") = maxVersionNumber + 1
                verRow("VersionDescription") = "Version " & (maxVersionNumber + 1)
            Else
                verRow("VersionNumber") = VersionCmbBox.Items.Count + 1
                verRow("VersionDescription") = "Version " & VersionCmbBox.Items.Count + 1
            End If

            verRow("ServingSize") = ServingSizeTextBox.Text
            verRow("ServingWeight") = ServingWeightLabel.Text
            verRow("IsDefault") = 1
            verRow("PackagingType") = ComboBox3.Text
            verRow("IsBox") = If(chkDisplayBox.Checked, 1, 0)
            verRow("IsBag") = If(chkDisplayBag.Checked, 1, 0)
            dsFV.Tables("versions").Rows.Add(verRow)


            If VersionCmbBox.Items.Count > 0 Then
                Dim maxVersionNumber = VersionCmbBox.Items.Cast(Of Integer).Max()
                VersionCmbBox.Items.Add(maxVersionNumber + 1)
                Dim index As Int16 = VersionCmbBox.FindStringExact(maxVersionNumber + 1)
                VersionCmbBox.SelectedIndex = index
            Else
                VersionCmbBox.Items.Add(VersionCmbBox.Items.Count + 1)
                VersionCmbBox.SelectedIndex = VersionCmbBox.Items.Count - 1
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub Standup_BagUpdate(sender As Object, e As EventArgs)
        Try
            Dim dataView As DataView = New DataView(dsStandupBags.Tables("StandUpBag"), "AddedFrom ='Bags' AND VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), 1, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then

                If TypeOf sender Is TextBox Then
                    Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)

                    Dim IsInvalidChar = CheckInvalidChar(senderTxtBox, dataView)
                    If IsInvalidChar Then
                        Exit Sub
                    End If

                    If senderTxtBox.Tag = "QuantityBag" Then
                        Dim qty = (If(String.IsNullOrEmpty(senderTxtBox.Text), "0", senderTxtBox.Text))
                        dataView(0).Item(senderTxtBox.Tag) = qty.Replace(",", "")
                    Else
                        dataView(0).Item(senderTxtBox.Tag) = senderTxtBox.Text
                    End If
                ElseIf TypeOf sender Is ComboBox Then
                    Dim SenderComboBox As ComboBox = DirectCast(sender, ComboBox)
                    dataView(0).Item(SenderComboBox.Tag) = If(SenderComboBox.Text = "Yes", 1, 0)
                ElseIf TypeOf sender Is ListBox Then
                    Dim SenderListBox As ListBox = DirectCast(sender, ListBox)
                    dataView(0).Item(SenderListBox.Tag) = SenderListBox.Text.ToString

                End If
            Else
                Dim row As DataRow = dsStandupBags.Tables("StandUpBag").NewRow()
                row("FormulaId") = Convert.ToInt32(FormulaID.Text)
                row("Description_") = txtDexcription.Text
                row("QuantityBag") = If(String.IsNullOrEmpty(txtQty.Text.Replace(",", "")), 0, Convert.ToInt32(txtQty.Text.Replace(",", "")))
                row("MaterialBag") = txtMaterial.Text
                row("SizeBag") = txtSize.Text
                row("PrintColor") = txtPrintColors.Text
                row("TearNotch") = If(cmbTearNotch.Text = "Yes", 1, 0)
                row("Zipper") = If(cmbZipper.Text = "Yes", 1, 0)
                row("HangerHole") = If(cmbHangerHole.Text = "Yes", 1, 0)
                row("ServingWeight") = If(String.IsNullOrEmpty(txtServingWeight.Text), 0, txtServingWeight.Text)
                row("Servings") = If(String.IsNullOrEmpty(txtServing.Text), 0, txtServing.Text)
                row("FillWeight") = If(String.IsNullOrEmpty(txtFillWeight.Text), 0, txtFillWeight.Text)
                row("PrintingPlatesBag") = txtPrintingPlates.Text
                row("ArtPreparationBag") = txtArtPreperation.Text
                row("SecondaryPackout") = txtSecondaryPackoutBag.Text
                row("Other") = txtOther.Text
                row("Other_Description") = txtOtherDesc.Text
                row("Shipper_CaseAmt") = txtShipperCaseAmt.Text
                row("Shipper_CaseCount") = If(String.IsNullOrEmpty(txtShipperCaseCount.Text) Or IsDBNull(txtShipperCaseCount.Text), 0, txtShipperCaseCount.Text)
                row("Research_DevelopmentBag") = If(String.IsNullOrEmpty(txtStandupBagRD.Text), 0.00, Convert.ToDouble(txtStandupBagRD.Text))
                'row("Price") = txtPrice.Text
                row("PrintingCostBag") = txtPrintingCost.Text
                row("AddedFrom") = "Bags"
                row("VersionNumber") = If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text)
                dsStandupBags.Tables("StandUpBag").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
            Exit Sub
        End Try

    End Sub


    Private Sub StickPack_BagUpdate(sender As Object, e As EventArgs)
        Try
            Dim dataView As DataView = New DataView(dsStandupBags.Tables("StandUpBag"), "AddedFrom ='Stick Packs' AND VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), 1, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then

                If TypeOf sender Is TextBox Then
                    Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)

                    Dim IsInvalidChar = CheckInvalidChar(senderTxtBox, dataView)
                    If IsInvalidChar Then
                        Exit Sub
                    End If

                    If senderTxtBox.Tag = "QuantityBag" Then
                        Dim qty = (If(String.IsNullOrEmpty(senderTxtBox.Text), "0", senderTxtBox.Text))
                        dataView(0).Item(senderTxtBox.Tag) = qty.Replace(",", "")
                    Else
                        dataView(0).Item(senderTxtBox.Tag) = senderTxtBox.Text
                    End If
                ElseIf TypeOf sender Is ComboBox Then
                    Dim SenderComboBox As ComboBox = DirectCast(sender, ComboBox)
                    dataView(0).Item(SenderComboBox.Tag) = If(SenderComboBox.Text = "Yes", 1, 0)
                ElseIf TypeOf sender Is ListBox Then
                    Dim SenderListBox As ListBox = DirectCast(sender, ListBox)
                    dataView(0).Item(SenderListBox.Tag) = SenderListBox.Text.ToString

                End If
            Else
                Dim row As DataRow = dsStandupBags.Tables("StandUpBag").NewRow()
                row("FormulaId") = Convert.ToInt32(FormulaID.Text)
                row("Description_") = txtStickPackBagDescription.Text
                row("QuantityBag") = If(String.IsNullOrEmpty(txtStickPackBagQty.Text.Replace(",", "")), 0, Convert.ToInt32(txtStickPackBagQty.Text.Replace(",", "")))
                row("MaterialBag") = txtStickPackBagMaterial.Text
                row("SizeBag") = txtStickPackBagSize.Text
                row("PrintColor") = txtStickPackBagPrintColors.Text
                row("TearNotch") = If(cmbStickPackBagTearNotch.Text = "Yes", 1, 0)
                row("Zipper") = If(cmbStickPackBagZipper.Text = "Yes", 1, 0)
                row("HangerHole") = If(cmbStickPackBagHangerHole.Text = "Yes", 1, 0)
                row("ServingWeight") = If(String.IsNullOrEmpty(txtStickPackBagServingWeight.Text), 0, txtStickPackBagServingWeight.Text)
                row("Servings") = If(String.IsNullOrEmpty(txtStickPackBagServings.Text), 0, txtStickPackBagServings.Text)
                row("FillWeight") = If(String.IsNullOrEmpty(txtStickPackBagFillWeight.Text), 0, txtStickPackBagFillWeight.Text)
                row("PrintingPlatesBag") = txtPrintingPlatesStickBag.Text
                row("ArtPreparationBag") = txtArtPreStickBag.Text
                row("SecondaryPackout") = txtSecondaryPackStickBag.Text
                row("Other") = If(String.IsNullOrEmpty(txtOtherCostStickBag.Text), 0.00, txtOtherCostStickBag.Text)
                row("Other_Description") = txtOtherStickBag.Text
                row("Shipper_CaseAmt") = txtShipperCostStickBag.Text
                row("Shipper_CaseCount") = If(String.IsNullOrEmpty(txtShipperCountStickBag.Text) Or IsDBNull(txtShipperCountStickBag.Text), 0, txtShipperCountStickBag.Text)
                row("Research_DevelopmentBag") = If(String.IsNullOrEmpty(txtStickpackBagRD.Text), 0.00, Convert.ToDouble(txtStickpackBagRD.Text))
                'row("Price") = txtPrice.Text
                row("PrintingCostBag") = txtStickPackBagPrintingCost.Text
                row("AddedFrom") = "Stick Packs"
                row("VersionNumber") = If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text)
                dsStandupBags.Tables("StandUpBag").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
            Exit Sub
        End Try

    End Sub

    Private Sub Sachets_BagUpdate(sender As Object, e As EventArgs)
        Try
            Dim dataView As DataView = New DataView(dsStandupBags.Tables("StandUpBag"), "AddedFrom ='Sachets' AND VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), 1, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then

                If TypeOf sender Is TextBox Then
                    Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)

                    Dim IsInvalidChar = CheckInvalidChar(senderTxtBox, dataView)
                    If IsInvalidChar Then
                        Exit Sub
                    End If

                    If senderTxtBox.Tag = "QuantityBag" Then
                        Dim qty = (If(String.IsNullOrEmpty(senderTxtBox.Text), "0", senderTxtBox.Text))
                        dataView(0).Item(senderTxtBox.Tag) = qty.Replace(",", "")
                    Else
                        dataView(0).Item(senderTxtBox.Tag) = senderTxtBox.Text
                    End If
                ElseIf TypeOf sender Is ComboBox Then
                    Dim SenderComboBox As ComboBox = DirectCast(sender, ComboBox)
                    dataView(0).Item(SenderComboBox.Tag) = If(SenderComboBox.Text = "Yes", 1, 0)
                ElseIf TypeOf sender Is ListBox Then
                    Dim SenderListBox As ListBox = DirectCast(sender, ListBox)
                    dataView(0).Item(SenderListBox.Tag) = SenderListBox.Text.ToString

                End If
            Else
                Dim row As DataRow = dsStandupBags.Tables("StandUpBag").NewRow()
                row("FormulaId") = Convert.ToInt32(FormulaID.Text)
                row("Description_") = txtSachetsBagDescription.Text
                row("QuantityBag") = If(String.IsNullOrEmpty(txtSachetsBagQty.Text.Replace(",", "")), 0, Convert.ToInt32(txtSachetsBagQty.Text.Replace(",", "")))
                row("MaterialBag") = txtSachetsBagMaterial.Text
                row("SizeBag") = txtSachetsBagSize.Text
                row("PrintColor") = txtSachetsBagPrintColors.Text
                row("TearNotch") = If(cmbSachetsBagTearNotch.Text = "Yes", 1, 0)
                row("Zipper") = If(cmbSachetsBagZipper.Text = "Yes", 1, 0)
                row("HangerHole") = If(cmbSachetsBagHangerHole.Text = "Yes", 1, 0)
                row("ServingWeight") = If(String.IsNullOrEmpty(txtSachetsBagServingWeight.Text), 0, txtSachetsBagServingWeight.Text)
                row("Servings") = If(String.IsNullOrEmpty(txtSachetsBagServings.Text), 0, txtSachetsBagServings.Text)
                row("FillWeight") = If(String.IsNullOrEmpty(txtSachetsBagFillWeight.Text), 0, txtSachetsBagFillWeight.Text)
                row("PrintingPlatesBag") = txtSachetsBagPrintingPlates.Text
                row("ArtPreparationBag") = txtSachetsBagArtPreparation.Text
                row("SecondaryPackout") = txtSachetsBagSecondaryPackout.Text
                row("Other") = txtSachetsBagOther.Text
                row("Other_Description") = txtSachetsBagOtherDescription.Text
                row("Shipper_CaseAmt") = txtSachetsBagShipperCaseCost.Text
                row("Shipper_CaseCount") = If(String.IsNullOrEmpty(txtSachetsBagShipperCaseCount.Text) Or IsDBNull(txtSachetsBagShipperCaseCount.Text), 0, txtSachetsBagShipperCaseCount.Text)
                row("Research_DevelopmentBag") = If(String.IsNullOrEmpty(txtSachetsBagRD.Text), 0.00, Convert.ToDouble(txtSachetsBagRD.Text))
                'row("Price") = txtPrice.Text
                row("PrintingCostBag") = txtSachetsBagPrintingCost.Text
                row("AddedFrom") = "Sachets"
                row("VersionNumber") = If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text)
                dsStandupBags.Tables("StandUpBag").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
            Exit Sub
        End Try

    End Sub

    Private Sub FormulaTypeCmbBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            If FormulaTypeCmbBox.Text <> "Powder" And FormulaID.Text = "-1" Then
                ServingSizeTextBox.Text = 1
            End If

            If dsFF.Tables("formulaF").Rows.Count = 0 Then

                NewVersion() '01.10.2025RS move here from bottom or get error in ServingSizeTextBox_TextChanged since VersionCmbBox.Text was NULL
                ComboBox3.SelectedIndex = 0
                Dim R As DataRow = dsFF.Tables("formulaF").NewRow()
                R("FormulaID") = 0
                R("FormulaName") = ""
                R("FormulaType") = FormulaTypeCmbBox.Text
                R("PackagingFormat") = 0


                If FormulaTypeCmbBox.Text <> "Powder" Then
                    R("ServingSize") = 1
                End If

                dsFF.Tables("formulaF").Rows.Add(R)

                Panel2.Enabled = True
                Panel1.Enabled = True

                VersionCmbBox.Enabled = True
                VersionDescriptionTxt.Enabled = True

                'NewVersion()  '01.10.2025RS Move to top (probably need to do same for all for FormulaTypeCmbBox Types in this routine).
            End If

            If FormulaTypeCmbBox.Text = "Powder" Then
                BlistersPanel.Visible = False
                If ComboBox3.Items.Contains("Blister") Then
                    ComboBox3.Items.Remove("Blister")
                End If

                If Not ComboBox3.Items.Contains("Bags") Then
                    ComboBox3.Items.Add("Bags")
                End If
                ServingSizeTextBox.Enabled = False
                ServingSizeUOMLabel.Text = "gm"
                BlendingPanel.Visible = True
                EncapsulationPanel.Visible = False
                TabletingPanel.Visible = False
                DataGridView1.Columns(10).HeaderText = "Per kg Cost"
                DataGridViewTextBoxColumn1.HeaderText = "Qty/kg"
                Label5.Text = "Mix Cost/kg ₹"
                Label105.Text = "Total per kg Cost"
                Label98.Text = "gram(s)"

                If FormulaID.Text = "-1" Then
                    TextBox12.Text = "3.50"
                    TextBox15.Text = "1.00"
                End If


            ElseIf FormulaTypeCmbBox.Text = "Capsule" Then

                If Not ComboBox3.Items.Contains("Blister") Then
                    ComboBox3.Items.Add("Blister")
                End If

                ServingSizeTextBox.Enabled = True
                ServingSizeUOMLabel.Text = "Capsule(s)"
                BlendingPanel.Visible = False
                EncapsulationPanel.Visible = True
                TabletingPanel.Visible = False
                DataGridView1.Columns(10).HeaderText = "Per/1000 Cost"
                DataGridViewTextBoxColumn1.HeaderText = "Qty per/1000"
                Label5.Text = "Mix Cost per/1000 ₹"
                Label105.Text = "Total per/1000 Cost"
                Label98.Text = "Capsule(s)"
                cmbBlisterCountType.SelectedIndex = 0

                If FormulaID.Text = "-1" Then
                    ComboBox5.SelectedIndex = 5
                    TextBox7.Text = "3.50"
                    txtlabcost.Text = "1.00"
                End If

            ElseIf FormulaTypeCmbBox.Text = "Tablet" Then

                If Not ComboBox3.Items.Contains("Blister") Then
                    ComboBox3.Items.Add("Blister")
                End If

                If ComboBox3.Items.Contains("Bags") Then
                    ComboBox3.Items.Remove("Bags")
                End If
                ServingSizeTextBox.Enabled = True
                ServingSizeUOMLabel.Text = "Tablet(s)"
                BlendingPanel.Visible = False
                EncapsulationPanel.Visible = False
                TabletingPanel.Visible = True
                DataGridView1.Columns(10).HeaderText = "Per/1000 Cost"
                DataGridViewTextBoxColumn1.HeaderText = "Qty per/1000"
                Label5.Text = "Mix Cost/1th ₹"
                Label105.Text = "Total per 1000 Cost"
                Label98.Text = "Tablet(s)"
                cmbBlisterCountType.SelectedIndex = 1
                If FormulaID.Text = "-1" Then
                    TextBox3.Text = "3.50"
                End If

            End If

            'Update FormulaDetail Grid as per change in Formula type
            For j As Integer = 0 To DataGridView1.Rows.Count - 1
                'FormulaCalculations(j)
                CalculateServingWeight()
                FormulaCost(j)
            Next

            dsFF.Tables("formulaF").Rows(0).Item("PackagingFormat") = ComboBox3.SelectedIndex
            dsFF.Tables("formulaF").Rows(0).Item("FormulaType") = FormulaTypeCmbBox.Text
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub VersionCmbBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            For Each r As DataRow In dsFV.Tables("versions").Rows
                If r.RowState = DataRowState.Deleted Then
                    Continue For
                End If
                If r.Item("VersionNumber") = VersionCmbBox.Text Then
                    VersionDescriptionTxt.Text = r.Item("VersionDescription")
                    If FormulaTypeCmbBox.SelectedIndex = 0 Then
                        ServingSizeTextBox.Text = Format(r.Item("ServingSize"), "N2")
                    Else
                        ServingSizeTextBox.Text = Format(r.Item("ServingSize"), "N0")
                    End If
                    r.Item("IsDefault") = 1
                Else
                    r.Item("IsDefault") = 0
                End If

                'If r.Item("VersionNumber") = VersionCmbBox.Text Then
                '    VersionDescriptionTxt.Text = r.Item("VersionDescription")
                '    If FormulaTypeCmbBox.SelectedIndex = 0 Then
                '        ServingSizeTextBox.Text = Format(r.Item("ServingSize"), "N2")
                '    Else
                '        ServingSizeTextBox.Text = Format(r.Item("ServingSize"), "N0")
                '    End If
                '    r.Item("IsDefault") = 1
                'Else
                '    r.Item("IsDefault") = 0
                'End If
            Next

            Dim CurrentVersion = New DataView(dsFV.Tables("versions"), "VersionNumber = " & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)
            If CurrentVersion.Count > 0 Then
                If CurrentVersion(0)("IsBox").ToString() = "1" Then
                    chkDisplayBox.Checked = True
                ElseIf CurrentVersion(0)("IsBox").ToString() = "0" Then
                    chkDisplayBox.Checked = False
                End If
                If CurrentVersion(0)("IsBag").ToString() = "1" Then
                    chkDisplayBag.Checked = True
                ElseIf CurrentVersion(0)("IsBag").ToString() = "0" Then
                    chkDisplayBag.Checked = False
                End If
            End If

            Select Case FormulaTypeCmbBox.Text
                Case "Powder"
                    DisplayBlending(VersionCmbBox.Text)
                Case "Capsule"
                    DisplayEncapsulation(VersionCmbBox.Text)
                Case "Tablet"
                    DisplayTableting(VersionCmbBox.Text)
            End Select
            DisplayFormula()
            SetComboBox3Index()
            SetPackagingType()

            For j As Integer = 0 To DataGridView1.Rows.Count - 1
                DisplaySelectedCostValue(j)
                DisplayCostType(j)
                DisplayCostColor(j)
                ' FormulaCalculations(j)
                CalculateServingWeight()
                FormulaCost(j)
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Try
            If VersionCmbBox.SelectedIndex = -1 Then
                Exit Sub
            End If
            Dim result As DialogResult = MessageBox.Show("Copy current version?", "Copy Version", MessageBoxButtons.YesNo)
            If result = DialogResult.No Then
                Exit Sub
            Else
                Me.Cursor = Cursors.WaitCursor

                Saves(True)

                Dim dv As DataView = New DataView(dsF.Tables("formula"), "VersionID = " & VersionCmbBox.Text, "IngredientIndex", DataViewRowState.CurrentRows)
                For Each r As DataRowView In dv

                    Dim workRow As DataRow = dsF.Tables("formula").NewRow()
                    workRow("FormulaID") = FormulaID.Text
                    workRow("VersionID") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                    workRow("IngredientIndex") = r.Item("IngredientIndex")
                    workRow("MaterialName") = r.Item("MaterialName")
                    workRow("ComponentCode") = r.Item("ComponentCode")
                    workRow("VendorName") = r.Item("VendorName")
                    workRow("Actualmg") = r.Item("Actualmg")
                    workRow("Potency") = r.Item("Potency")
                    workRow("Overage") = r.Item("Overage")
                    workRow("mg") = r.Item("mg")
                    workRow("Freight") = r.Item("Freight")
                    workRow("MaterialCost") = r.Item("MaterialCost")
                    workRow("EnteredDate") = DateTime.Now
                    workRow("EnteredBy") = Dashboard.UserIDLabel.Text
                    workRow("IsManual") = r.Item("IsManual")
                    workRow("ManualCost") = r.Item("ManualCost")
                    workRow("ManualCostDate") = r.Item("ManualCostDate")
                    workRow("LatestCost") = r.Item("LatestCost")
                    workRow("LatestCostDate") = r.Item("LatestCostDate")

                    dsF.Tables("formula").Rows.Add(workRow)

                Next
                'daF_commands()
                BindingSource1.DataSource = dsF.Tables("formula")

                Select Case FormulaTypeCmbBox.Text
                    Case "Powder"
                        CopyBlending(VersionCmbBox.Text)
                    Case "Capsule"
                        CopyEncapsulation(VersionCmbBox.Text)
                    Case "Tablet"
                        CopyTableting(VersionCmbBox.Text)
                End Select
                Select Case ComboBox3.Text
                    Case "Bulk"
                    Case "Bottles"
                        CopyBottle(VersionCmbBox.Text)
                    Case "Sachets"
                        CopySachets(VersionCmbBox.Text)
                        CopyBags(VersionCmbBox.Text)
                    Case "Stick Packs"
                        CopyStickPacks(VersionCmbBox.Text)
                        CopyBags(VersionCmbBox.Text)
                    Case "Bags"
                        CopyBags(VersionCmbBox.Text)
                    Case "Blister"
                        CopyBlister(VersionCmbBox.Text)
                End Select

                'If ComboBox3.Text <> "Bottles" Then
                CopySales(VersionCmbBox.Text)
                'End If

                NewVersion()
                If ComboBox3.Text = "Bulk" Then
                    'If ComboBox3.Text = "Bulk" Then
                    '    For j As Integer = 0 To gridBulkSales.Rows.Count - 1
                    '        CalculateSales(j, TextBox8.Text)
                    '    Next
                    'Else
                    '    For j As Integer = 0 To gridBulkSales.Rows.Count - 1
                    '        CalculateSales(j, TotalUnitCost.Text)
                    '    Next
                    'End If
                    CalculateBulkSales(gridBulkSales)
                ElseIf ComboBox3.Text = "Bottles" Then
                    CalculateBottlingCost()
                ElseIf ComboBox3.Text = "Stick Packs" Then
                    CalculateStickPacksCost()
                ElseIf ComboBox3.Text = "Sachets" Then
                    CalculateSachetsCost()
                ElseIf ComboBox3.Text = "Bags" Then
                    CalculateStandUpBagCost()
                ElseIf ComboBox3.Text = "Blister" Then
                    CalculateBlisterCost()
                End If
                Saves(True)
                BindControls()
                Me.Cursor = Cursors.Arrow
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub VersionDescriptionTxt_Leave(sender As Object, e As EventArgs)
        Try
            Dim findtxn() As DataRow = dsFV.Tables("versions").Select("VersionNumber = " & VersionCmbBox.Text)
            If findtxn.Count > 0 Then
                Dim OldS As String = findtxn(0).Item("VersionDescription")
                If OldS <> VersionDescriptionTxt.Text Then
                    findtxn(0).Item("VersionDescription") = VersionDescriptionTxt.Text
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub TextBox37_TextChanged(sender As Object, e As EventArgs) Handles TextBox37.TextChanged
        Try
            'Create a dataview
            Dim dv As New DataView
            If String.IsNullOrEmpty(TextBox37.Text) Then
                'ListBox2.Visible = False
                MaterialDataGrid.Visible = False
                IsNewMaterialBelow = False
                IsReplace = False
            Else

                Dim FilteredText As String = TextBox37.Text
                Dim FilteredMaterialDataTable As DataTable = ds.Tables("ingredients").Clone()
                Dim FilteredRows = From row In ds.Tables("ingredients").AsEnumerable()
                                   Where row.Field(Of String)("MaterialName").StartsWith(FilteredText, StringComparison.OrdinalIgnoreCase)

                For Each row As DataRow In FilteredRows
                    FilteredMaterialDataTable.ImportRow(row)
                Next

                dv.Table = FilteredMaterialDataTable
                MaterialDataGrid.DataSource = dv
                For Each dataColumn As DataGridViewColumn In MaterialDataGrid.Columns
                    If dataColumn.Name <> "MaterialName" And dataColumn.Name <> "Supplier" And dataColumn.Name <> "Price" And dataColumn.Name <> "PriceUpdateDate" Then
                        dataColumn.Visible = False
                    End If
                Next

                If dv.Count > 0 Then
                    MaterialDataGrid.Visible = True
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub AddNewFormulaRow(text1 As String, Optional Supplier As String = "")
        Try

            Dim cultureInfo As New System.Globalization.CultureInfo("en-IN")
            Dim tx As String = cultureInfo.TextInfo.ToTitleCase(text1)
            Dim sp As String
            If Not String.IsNullOrEmpty(Supplier) Then
                sp = cultureInfo.TextInfo.ToTitleCase(Supplier)
            End If

            Dim TempDS As New DataSet

            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()

            dbUp = New MySqlCommand()
            dbUp.Connection = DbCon
            dbUp.CommandType = CommandType.Text

            If Not String.IsNullOrEmpty(Supplier) Then
                dbUp.CommandText = "SELECT * FROM material WHERE MaterialName=@MaterialName and Supplier=@Supplier"
                dbUp.Parameters.AddWithValue("@MaterialName", tx)
                dbUp.Parameters.AddWithValue("@Supplier", sp)
            Else
                dbUp.CommandText = "SELECT * FROM material WHERE MaterialName=@MaterialName"
                dbUp.Parameters.AddWithValue("@MaterialName", tx)
            End If

            Dim workRow As DataRow = dsF.Tables("formula").NewRow()
            da = New MySqlDataAdapter(dbUp)
            da.Fill(TempDS, "Material")

            Dim findtxn() As DataRow = TempDS.Tables("Material").Select()

            'Dim findtxn() As DataRow = ds.Tables("ingredients").Select("MaterialName = '" & tx & "'")

            If findtxn.Count > 0 Then
                workRow("ComponentCode") = findtxn(0).Item("ComponentCode")
                workRow("VendorName") = findtxn(0).Item("Supplier")
                workRow("LatestCost") = findtxn(0).Item("Price")
                workRow("IsManual") = 0
                workRow("LatestCostDate") = findtxn(0).Item("LatestDate")
                workRow("Freight") = FreightCost
                workRow("MaterialCost") = findtxn(0).Item("Price") + 2
            Else
                workRow("ComponentCode") = DBNull.Value
                workRow("VendorName") = DBNull.Value
                workRow("LatestCost") = 0
                workRow("IsManual") = 1
                workRow("ManualCost") = 0
                workRow("ManualCostDate") = DBNull.Value
                workRow("Freight") = FreightCost
                workRow("MaterialCost") = 0
            End If
            workRow("FormulaID") = FormulaID.Text
            workRow("VersionID") = VersionCmbBox.Text

            If IsNewMaterialBelow Then
                workRow("IngredientIndex") = RowIndex + 1
                Dim DataView = New DataView(dsF.Tables("formula"), "VersionID = " & VersionCmbBox.Text & " AND IngredientIndex > " & RowIndex, "IngredientIndex", DataViewRowState.CurrentRows)
                i = RowIndex + 2
                For Each item In DataView
                    item("IngredientIndex") = i
                    i = i + 1
                Next

            Else
                workRow("IngredientIndex") = DataGridView1.Rows.Count + 1
            End If

            workRow("MaterialName") = tx
            workRow("Actualmg") = 0
            workRow("Potency") = DBNull.Value
            workRow("Overage") = DBNull.Value
            workRow("mg") = 0

            workRow("EnteredDate") = Now.Date
            workRow("EnteredBy") = Convert.ToInt16(Dashboard.UserIDLabel.Text)
            dsF.Tables("formula").Rows.Add(workRow)


            BindControls()

            If IsNewMaterialBelow Then
                DisplaySelectedCostValue(RowIndex)
                DisplayCostType(RowIndex)
                DisplayCostColor(RowIndex)
            Else
                DisplaySelectedCostValue(DataGridView1.Rows.Count - 1)
                DisplayCostType(DataGridView1.Rows.Count - 1)
                DisplayCostColor(DataGridView1.Rows.Count - 1)
            End If

        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
            DbCon.Close()
        End Try

    End Sub
    Private Sub DataGridView1_RowValidated(sender As Object, e As DataGridViewCellEventArgs)
        Try
            'FormulaCalculations(DataGridView1.CurrentRow.Index)
            CalculateServingWeight()
            FormulaCost(DataGridView1.CurrentRow.Index)
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    Function FormulaCost(ind As Integer)
        Try
            If sw = 0 Then
                Exit Function
            End If

            Dim mg As Double = DataGridView1.Rows(ind).DataBoundItem("mg")
            Dim mc As Double = DataGridView1.Rows(ind).DataBoundItem("MaterialCost")


            If FormulaTypeCmbBox.Text = "Powder" Then
                'calcualtes percentage of the ingredinet on the total serving weight
                Dim PITSW As Double = Math.Round((mg / sw) * 100, 2)
                DataGridView1.Rows(ind).Cells(10).Value = Math.Round((PITSW * DataGridView1.Rows(ind).DataBoundItem("MaterialCost") / 100), 2)
            Else
                Dim PerGramCost = Math.Round(((mc * mg) / 1000), 2)
                DataGridView1.Rows(ind).Cells(10).Value = PerGramCost
            End If
            Dim tc As Double = 0
            For j As Integer = 0 To DataGridView1.Rows.Count - 1
                tc += DataGridView1.Rows(j).Cells(10).Value
            Next
            CostKiloLabel.Text = (tc).ToString("##,#0.00")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Function


    Private Sub DisplaySelectedCostValue(ind As Integer)
        Try
            If DataGridView1.Rows(ind).DataBoundItem("IsManual") = 1 Then
                DataGridView1.Rows(ind).Cells(8).Value = DataGridView1.Rows(ind).DataBoundItem("ManualCost")
            Else
                DataGridView1.Rows(ind).Cells(8).Value = DataGridView1.Rows(ind).DataBoundItem("LatestCost")
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub
    Private Sub DisplayCostType(ind As Integer)
        Try
            If DataGridView1.Rows(ind).DataBoundItem("IsManual") = 1 Then
                DataGridView1.Rows(ind).Cells(8).Value = DataGridView1.Rows(ind).DataBoundItem("ManualCost")
                DataGridView1.Rows(ind).Cells(9).Value = "M"
            Else
                DataGridView1.Rows(ind).Cells(8).Value = DataGridView1.Rows(ind).DataBoundItem("LatestCost")
                DataGridView1.Rows(ind).Cells(9).Value = "L"
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub DisplayCostColor(ind As Integer)
        Try
            If DataGridView1.Rows(ind).DataBoundItem("MaterialCost") = 0 Then
                DataGridView1.Rows(ind).Cells(8).Style.BackColor = Color.White
                Exit Sub
            End If

            Dim dt1 As DateTime
            Dim dt2 As DateTime = Convert.ToDateTime(DateTime.Now)
            If DataGridView1.Rows(ind).DataBoundItem("IsManual") = 1 Then
                If Not IsDBNull(DataGridView1.Rows(ind).DataBoundItem("ManualCostDate")) Then
                    dt1 = Convert.ToDateTime(DataGridView1.Rows(ind).DataBoundItem("ManualCostDate"))
                Else
                    DataGridView1.Rows(ind).Cells(8).Style.BackColor = Color.White
                    Exit Sub
                End If
            Else
                If Not IsDBNull(DataGridView1.Rows(ind).DataBoundItem("LatestCostDate")) Then
                    dt1 = Convert.ToDateTime(DataGridView1.Rows(ind).DataBoundItem("LatestCostDate"))
                Else
                    DataGridView1.Rows(ind).Cells(8).Style.BackColor = Color.White
                    Exit Sub
                End If
            End If



            Dim ts As TimeSpan = dt2.Subtract(dt1)
            If Convert.ToInt32(ts.Days) >= 0 Then
                Dim df As Integer = Convert.ToInt32(ts.Days)
                If df <= My.Settings.NewPrice Then
                    DataGridView1.Rows(ind).Cells(8).Style.BackColor = Color.LightGreen
                ElseIf df > My.Settings.NewPrice AndAlso df < My.Settings.OldPrice Then
                    DataGridView1.Rows(ind).Cells(8).Style.BackColor = Color.Yellow
                Else
                    DataGridView1.Rows(ind).Cells(8).Style.BackColor = Color.Salmon
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Function FormulaCalculations(ind As Integer, Optional ISMgChanged As Boolean = False)
        Try
            'calculates mg Column
            Dim tt As Double
            Dim Act As Double = 0
            Dim Pot As Double = 0
            Dim Ove As Double = 0
            If Not IsDBNull(DataGridView1.Rows(ind).DataBoundItem("Actualmg")) Then
                tt = DataGridView1.Rows(ind).DataBoundItem("Actualmg")
            End If
            If Not IsDBNull(DataGridView1.Rows(ind).DataBoundItem("Potency")) Then
                Pot = DataGridView1.Rows(ind).DataBoundItem("Potency")
            End If
            If Not IsDBNull(DataGridView1.Rows(ind).DataBoundItem("Overage")) Then
                Ove = DataGridView1.Rows(ind).DataBoundItem("Overage")
            End If

            If Not IsDBNull(tt) Then
                If Pot <> 0 Then
                    tt *= Math.Round((100 / Pot), 5)
                End If
                If Ove <> 0 Then
                    tt += Math.Round((tt * Ove) / 100, 5)
                End If

                If FormulaTypeCmbBox.Text = "Powder" Then
                    If ISMgChanged And MGIndex = ind Then
                        DataGridView1.Rows(ind).DataBoundItem("mg") = TempMG
                    Else
                        DataGridView1.Rows(ind).DataBoundItem("mg") = tt
                    End If

                Else
                    If Double.IsPositiveInfinity(Math.Round(tt / ss, 2)) Then
                        DataGridView1.Rows(ind).DataBoundItem("mg") = 0
                    Else
                        Dim tt1 As Double
                        If ss = 0 Then
                            ' Handle the division by zero case
                            tt1 = 0 ' or any default value you want to assign
                        Else
                            'tt1 = Math.Round(tt / ss, 2)
                            tt1 = Math.Round(tt / ss, 4)
                        End If

                        DataGridView1.Rows(ind).DataBoundItem("mg") = tt1 '01.10.2025RS Odd one sometimes 0 becomes NaN not sure if 0 is correct
                    End If
                End If
            Else
                DataGridView1.Rows(ind).DataBoundItem("mg") = 0
            End If
            CalculateServingWeight()


        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Function


    Function CalculateServingWeight()
        Try
            If dsF.Tables("formula").Rows.Count > 0 Then
                sw = dsF.Tables("formula").Compute("Sum(mg)", "VersionID=" & VersionCmbBox.Text)
            End If
            If FormulaTypeCmbBox.Text = "Powder" Then
                ServingSizeTextBox.Text = Math.Round(sw / 1000, 2)
            End If
            ServingWeightLabel.Text = sw.ToString("#,##0.##")
            If FormulaTypeCmbBox.Text = "Powder" Then
                UpdateTextbox13()
            ElseIf FormulaTypeCmbBox.Text = "Capsule" Then
                UpdateTextbox14()
            ElseIf FormulaTypeCmbBox.Text = "Tablet" Then
                updateTextbox4()
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Function

    Private Sub OnlyNumberBoxes(sender As Object, e As KeyPressEventArgs)
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or Asc(e.KeyChar) = 8)
    End Sub
    Private Sub ServingSizeTextBox_TextChanged(sender As Object, e As EventArgs) Handles ServingSizeTextBox.TextChanged
        Try
            If FormulaTypeCmbBox.Text = "Powder" Then
                Dim ServingDecimal As Decimal
                Double.TryParse(ServingSizeTextBox.Text, ServingDecimal)
                ServingSizeTextBox.Text = ServingDecimal.ToString("N2")
            End If

            If Not String.IsNullOrEmpty(ServingSizeTextBox.Text) Then
                txtServingWeight.Text = ServingSizeTextBox.Text
                txtStickPackBagServingWeight.Text = ServingSizeTextBox.Text
                txtSachetsBagServingWeight.Text = ServingSizeTextBox.Text
                ss = ServingSizeTextBox.Text
                If ComboBox3.SelectedIndex <> -1 Then
                    If ComboBox3.Text = "Stick Packs" Then
                        RemoveHandler TextBox16.TextChanged, AddressOf StickPack_TextBox_TextChanged
                        TextBox16.Text = ServingSizeTextBox.Text
                        AddHandler TextBox16.TextChanged, AddressOf StickPack_TextBox_TextChanged
                    ElseIf ComboBox3.Text = "Sachets" Then
                        RemoveHandler TextBox35.TextChanged, AddressOf Sachets_TextBox_TextChanged
                        TextBox35.Text = ServingSizeTextBox.Text
                        AddHandler TextBox35.TextChanged, AddressOf Sachets_TextBox_TextChanged
                    ElseIf ComboBox3.Text = "Bottles" Then
                        CalculateBottlingCost()
                    End If
                    If dsFF.Tables.Contains("formulaF") AndAlso dsFF.Tables("formulaF").Rows.Count > 0 Then
                        dsFF.Tables("formulaF").Rows(0).Item("ServingSize") = ServingSizeTextBox.Text
                        'Else
                        'MessageBox.Show("The formulaF table is empty or does not exist.")
                    End If
                    'dsFF.Tables("formulaF").Rows(0).Item("ServingSize") = ServingSizeTextBox.Text
                    Dim findtxn() As DataRow = dsFV.Tables("versions").Select("VersionNumber = " & VersionCmbBox.Text) '01.10.2025RS Was getting error since NewVersion() was called too late
                    If findtxn.Count > 0 Then
                        findtxn(0).Item("ServingSize") = ServingSizeTextBox.Text
                    End If

                End If
                If FormulaTypeCmbBox.Text <> "Powder" Then
                    For j As Integer = 0 To DataGridView1.Rows.Count - 1
                        FormulaCalculations(j)
                        FormulaCost(j)
                    Next
                End If
            Else
                Exit Sub
            End If

            If String.IsNullOrEmpty(ComboBox4.Text) Or String.IsNullOrEmpty(ServingSizeTextBox.Text) Then
                Exit Sub
            End If
            txtFillWeight.Text = Double.Parse(ComboBox4.Text) * Double.Parse(ServingSizeTextBox.Text)
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub
    Private Sub btnFormula_Click(sender As Object, e As EventArgs) Handles btnFormula.Click
        Panel2.BringToFront()
    End Sub

    Private Sub btnPackaging_Click(sender As Object, e As EventArgs) Handles btnPackaging.Click
        Try
            Panel3.BringToFront()
            SetPackagingType()
            DisplaySalesData()
            If FormulaTypeCmbBox.SelectedIndex = 0 Then
                DisplayBlending(VersionCmbBox.Text)
            ElseIf FormulaTypeCmbBox.SelectedIndex = 1 Then
                DisplayEncapsulation(VersionCmbBox.Text)
            ElseIf FormulaTypeCmbBox.SelectedIndex = 2 Then
                DisplayTableting(VersionCmbBox.Text)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub dataGridView2_EditingControlShowing(ByVal sender As Object, ByVal e As DataGridViewEditingControlShowingEventArgs) Handles DataGridView2.EditingControlShowing
        Try
            Dim allowedRows As Integer()
            If FormulaTypeCmbBox.Text = "Capsule" Then
                allowedRows = {0, 1}
            Else
                allowedRows = {0, 1, 2}
            End If
            If DataGridView2.Columns(DataGridView2.CurrentCell.ColumnIndex).Name = "Column11" AndAlso allowedRows.Contains(DataGridView2.CurrentCell.RowIndex) Then
                Dim tb As TextBox = CType(e.Control, TextBox)
                If (tb IsNot Nothing) Then
                    RemoveHandler tb.TextChanged, New EventHandler(AddressOf TextBox_TextChanged)
                    AddHandler tb.TextChanged, New EventHandler(AddressOf TextBox_TextChanged)
                End If
            Else

                Dim tb As TextBox = CType(e.Control, TextBox)
                If (tb IsNot Nothing) Then
                    RemoveHandler tb.TextChanged, New EventHandler(AddressOf TextBox_TextChanged)
                End If

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim tb As TextBox = CType(sender, TextBox)
            'ListBox1.Items.Clear()
            Dim table = DirectCast(ListBox1.DataSource, System.Data.DataView).Table.TableName
            Dim custDV As New DataView


            If (table = "xls_bottlelist") Then
                Dim bottledataTable As DataTable = ds.Tables("xls_bottlelist")
                Dim bottledataTableFilter As DataTable = bottledataTable.Clone()
                Dim FilteredRows = From row In bottledataTable.AsEnumerable()
                                   Where row.Field(Of String)("MaterialName").ToLower().StartsWith(tb.Text.ToLower())
                                   Select row
                If (FilteredRows.Count > 0) Then
                    'bottledataTable = New DataTable
                    'bottledataTable.TableName = "xls_bottlelist"
                    bottledataTableFilter.Clear()
                    For Each row As DataRow In FilteredRows
                        bottledataTableFilter.ImportRow(row)
                    Next

                    ListBox1.DataSource = Nothing
                    ListBox1.DisplayMember = "MaterialName"
                    ListBox1.ValueMember = "ComponentCode"
                End If
                custDV = New DataView(bottledataTableFilter, "", "MaterialName", DataViewRowState.CurrentRows)

            ElseIf (table = "xls_lidslist") Then
                Dim lidsdataTable As DataTable = ds.Tables("xls_lidslist")
                Dim lidsdataTableFilter As DataTable = lidsdataTable.Clone()

                Dim FilteredRows = From row In lidsdataTable.AsEnumerable()
                                   Where row.Field(Of String)("MaterialName").ToLower().StartsWith(tb.Text.ToLower())
                                   Select row
                If (FilteredRows.Count > 0) Then
                    'bottledataTable = New DataTable
                    'bottledataTable.TableName = "xls_bottlelist"
                    lidsdataTableFilter.Clear()
                    For Each row As DataRow In FilteredRows
                        lidsdataTableFilter.ImportRow(row)
                    Next

                    ListBox1.DataSource = Nothing
                    ListBox1.DisplayMember = "MaterialName"
                    ListBox1.ValueMember = "ComponentCode"
                End If
                custDV = New DataView(lidsdataTableFilter, "", "MaterialName", DataViewRowState.CurrentRows)
            ElseIf (table = "xls_scooplist") Then
                Dim scoopdataTable As DataTable = ds.Tables("xls_scooplist")
                Dim scoopdataTableFilter As DataTable = scoopdataTable.Clone()

                Dim FilteredRows = From row In scoopdataTable.AsEnumerable()
                                   Where row.Field(Of String)("ScoopMaterialName").ToLower().StartsWith(tb.Text.ToLower())
                                   Select row
                If (FilteredRows.Count > 0) Then
                    'bottledataTable = New DataTable
                    'bottledataTable.TableName = "xls_bottlelist"
                    scoopdataTableFilter.Clear()
                    For Each row As DataRow In FilteredRows
                        scoopdataTableFilter.ImportRow(row)
                    Next

                    ListBox1.DataSource = Nothing
                    ListBox1.DisplayMember = "ScoopMaterialName"
                    ListBox1.ValueMember = "ComponentCode"
                End If
                custDV = New DataView(scoopdataTableFilter, "", "ScoopMaterialName", DataViewRowState.CurrentRows)

            ElseIf (table = "xls_shipperlist") Then
                Dim shipperdataTable As DataTable = ds.Tables("xls_shipperlist")
                Dim shipperdataTableFilter As DataTable = shipperdataTable.Clone()

                Dim FilteredRows = From row In shipperdataTable.AsEnumerable()
                                   Where row.Field(Of String)("ShipperMaterialName").ToLower().StartsWith(tb.Text.ToLower())
                                   Select row
                If (FilteredRows.Count > 0) Then
                    'bottledataTable = New DataTable
                    'bottledataTable.TableName = "xls_bottlelist"
                    shipperdataTableFilter.Clear()
                    For Each row As DataRow In FilteredRows
                        shipperdataTableFilter.ImportRow(row)
                    Next

                    ListBox1.DataSource = Nothing
                    ListBox1.DisplayMember = "ShipperMaterialName"
                    ListBox1.ValueMember = "ComponentCode"
                End If
                custDV = New DataView(shipperdataTableFilter, "", "ShipperMaterialName", DataViewRowState.CurrentRows)
            End If
            If ListBox1.Visible = False Then
                'ListBox1.Items.Clear()
                If DataGridView2.CurrentCell.ColumnIndex = 1 Then
                    ListBox1.Visible = True
                End If
            End If
            'ListBox1.DataSource = Nothing '
            ''ListBox1.Items.Clear() ' Clear existing items

            'For Each dr As DataRowView In custDV
            '    If dr.Item("MaterialName").tolower.ToString.Contains(tb.Text.ToLower) Then
            '        ListBox1.Items.Add(dr.Item("MaterialName"))

            '    End If
            'Next
            ListBox1.DataSource = custDV
            If tb.Focus = False Then
                ListBox1.Visible = False
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView2_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridView2.KeyDown
        Try
            If e.KeyCode = Keys.Escape Or e.KeyCode = Keys.Enter Then
                If ListBox1.Visible = True Then
                    ListBox1.Visible = False
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub



    Private Sub CostKiloLabel_TextChanged(sender As Object, e As EventArgs) Handles CostKiloLabel.TextChanged
        Try
            If FormulaTypeCmbBox.Text = "Tablet" Then
                CalculateTableting()
            ElseIf FormulaTypeCmbBox.Text = "Capsule" Then
                CalculateEncapsulation()
            ElseIf FormulaTypeCmbBox.Text = "Powder" Then
                CalculateBlending()
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Function CalculateTableting()
        Try
            'define val to pass to subtotal panel
            Dim ct As Double = 0
            'gets the current cost per kg
            Dim cpk As Double = CostKiloLabel.Text
            If cpk <> 0 Then
                ct = cpk

                For Each ctl In TabletingPanel.Controls
                    If TypeOf ctl Is TextBox Then
                        Dim tb As TextBox = ctl
                        If Not String.IsNullOrEmpty(ctl.tag) Then
                            'RemoveHandler tb.TextChanged, New EventHandler(AddressOf CalculateTableting)
                            'AddHandler tb.TextChanged, New EventHandler(AddressOf CalculateTableting)
                            Dim v As Double = tb.Text
                            ct += v
                        Else

                            cpk = (TextBox5.Text * cpk) / 100
                            TextBox4.Text = cpk.ToString("##,#0.00")

                        End If
                    End If
                Next
            End If

            CalculateTableting = ct
            TextBox36.Text = ct.ToString("##,#0.00")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Function

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        Try
            If FormulaTypeCmbBox.Text = "Powder" Then
                SubtotalTextBox_TextChanged(sender, e)
            End If

            '***make this combo to change the dataset of packaging picked by the user
            ToggleSalesGrid()
            SetPackagingType()
            If FormulaID.Text <> "-1" Then
                dsFF.Tables("formulaF").Rows(0).Item("PackagingFormat") = ComboBox3.SelectedIndex
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub SetPackagingType()
        Try
            'Toggle display box CheckBox
            If ComboBox3.Text = "Sachets" Or ComboBox3.Text = "Stick Packs" Or ComboBox3.Text = "Blister" Then
                chkDisplayBox.Visible = True

                'Add Bag in Sachets
                If ComboBox3.Text = "Stick Packs" Or ComboBox3.Text = "Sachets" Then
                    chkDisplayBag.Visible = True
                Else
                    chkDisplayBag.Visible = False
                End If
            Else
                chkDisplayBox.Visible = False
                chkDisplayBag.Visible = False

            End If

            'Add Bag in Sachets
            If (ComboBox3.Text = "Stick Packs" Or ComboBox3.Text = "Sachets") And chkDisplayBag.Checked Then
                pnlBagSales.Visible = True

                If pnlBoxSales.Visible = True Then
                    pnlBagSales.Location = New Point(14, 719)
                Else
                    pnlBagSales.Location = New Point(14, 476)
                End If

            Else
                pnlBagSales.Visible = False
            End If


            Dim CurrentVersion = New DataView(dsFV.Tables("versions"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
            If CurrentVersion.Count > 0 Then
                If CurrentVersion(0)("IsBox").ToString() = "1" Then
                    chkDisplayBox.Checked = True
                ElseIf CurrentVersion(0)("IsBox").ToString() = "0" Then
                    chkDisplayBox.Checked = False
                End If
                If CurrentVersion(0)("IsBag").ToString() = "1" Then
                    chkDisplayBag.Checked = True
                ElseIf CurrentVersion(0)("IsBag").ToString() = "0" Then
                    chkDisplayBag.Checked = False
                End If
            End If

            UpdatePackagingType()

            PackagingType = ComboBox3.Text
            StickPacksPanel.Visible = False
            SachetsPanel.Visible = False
            BottlesPanel.Visible = False
            BagsPanel.Visible = False
            BlistersPanel.Visible = False

            BindingSource2.DataSource = dsSal.Tables("salestxns")
            BindSalesBulkStickpacksGrid.DataSource = dsSal.Tables("salestxns")
            BindSalesBoxGrid.DataSource = dsSal.Tables("salestxns")
            If ComboBox3.Text = "Bulk" Then
                TotalUnitPanel.Visible = False
                Label22.Text = "Sales Bulk"

                If FormulaTypeCmbBox.Text = "Powder" Then
                    HideUnhideFlavorProfile(True)
                    CalculateBlending()
                End If

            ElseIf ComboBox3.Text = "Bottles" Then
                AddHandler DataGridView2.CellEndEdit, AddressOf CalculateBottlingCost
                If FormulaTypeCmbBox.Text = "Powder" Then
                    HideUnhideFlavorProfile(False)
                    CalculateBlending()
                End If
                DisplayBottling()
                Label22.Text = "Sales Bulk Bottle"
                BottlesPanel.Visible = True
                TotalUnitPanel.Visible = True
                If ComboBox4.Items.Count > 0 And ComboBox4.SelectedIndex = -1 Then
                    ComboBox4.SelectedIndex = 0
                End If
            ElseIf ComboBox3.Text = "Stick Packs" Then

                'Stick Packs Info
                Label22.Text = "Sales Bulk Stick Packet"
                lblBox.Text = "Sales Display Unit"
                lblBulkBags.Text = "Sales Bag Unit"

                If chkDisplayBox.Checked Then
                    GroupBoxStickpacks.Controls.Add(lblStickPacks)
                    GroupBoxStickpacks.Controls.Add(txtStickPacks)
                    GroupBoxStickpacks.Controls.Add(lblStickPacksUnit)
                    lblStickPacks.Location = New Point(13, 201)
                    txtStickPacks.Location = New Point(91, 197)
                    txtStickPacks.Size = New Size(109, 20)
                    lblStickPacksUnit.Location = New Point(203, 200)
                ElseIf chkDisplayBag.Checked Then
                    grpBoxStickPackBag.Controls.Add(lblStickPacks)
                    grpBoxStickPackBag.Controls.Add(txtStickPacks)
                    grpBoxStickPackBag.Controls.Add(lblStickPacksUnit)
                    lblStickPacks.Location = New Point(24, 312)
                    txtStickPacks.Location = New Point(104, 309)
                    txtStickPacks.Size = New Size(69, 20)
                    lblStickPacksUnit.Location = New Point(181, 311)
                End If
                RemoveHandler TextBox16.TextChanged, AddressOf StickPack_TextBox_TextChanged
                RemoveHandler TextBox16.TextChanged, AddressOf StickPack_TextBox_TextChanged
                If FormulaID.Text = "-1" Then
                    TextBox16.Text = ServingSizeTextBox.Text
                End If

                AddHandler TextBox16.TextChanged, AddressOf StickPack_TextBox_TextChanged
                AddRemoveHandlerStickPacks(True)
                DisplayStickpacks(VersionCmbBox.Text)


                StickPacksPanel.Visible = True
                TotalUnitPanel.Visible = True

                If FormulaTypeCmbBox.Text = "Powder" Then
                    HideUnhideFlavorProfile(False)
                    CalculateBlending()
                End If
                CalculateStickPacksCost()


            ElseIf ComboBox3.Text = "Sachets" Then

                Label22.Text = "Sales Bulk Sachets"
                lblBox.Text = "Sales Display Unit"
                lblBulkBags.Text = "Sales Bag Unit"

                'Add Bag in Sachets
                If chkDisplayBox.Checked Then
                    GroupBoxSachetBox.Controls.Add(lblStickPacksSachets)
                    GroupBoxSachetBox.Controls.Add(txtStickPacksSachets)
                    GroupBoxSachetBox.Controls.Add(Label73)
                    lblStickPacksSachets.Location = New Point(29, 203)
                    txtStickPacksSachets.Location = New Point(106, 202)
                    Label73.Location = New Point(217, 206)
                    Label73.Text = "Per IFC"
                ElseIf chkDisplayBag.Checked Then
                    GroupBoxSachetBag.Controls.Add(lblStickPacksSachets)
                    GroupBoxSachetBag.Controls.Add(txtStickPacksSachets)
                    GroupBoxSachetBag.Controls.Add(Label73)
                    lblStickPacksSachets.Location = New Point(24, 312)
                    txtStickPacksSachets.Location = New Point(104, 309)
                    Label73.Location = New Point(178, 312)
                    Label73.Text = "Per IFC/BAG"
                End If

                'Sachets Packs Info
                RemoveHandler TextBox35.TextChanged, AddressOf Sachets_TextBox_TextChanged
                RemoveHandler TextBox35.TextChanged, AddressOf Sachets_TextBox_TextChanged
                TextBox35.Text = ServingSizeTextBox.Text
                AddHandler TextBox35.TextChanged, AddressOf Sachets_TextBox_TextChanged
                AddRemoveSachets(True)
                DisplaySachets(VersionCmbBox.Text)

                SachetsPanel.Visible = True
                TotalUnitPanel.Visible = True

                If FormulaTypeCmbBox.Text = "Powder" Then
                    HideUnhideFlavorProfile(False)
                    CalculateBlending()
                End If

                CalculateSachetsCost()
            ElseIf ComboBox3.Text = "Bags" Then

                Label22.Text = "Sales Standup Bag"
                AddRemoveHandlerBags(True)
                DisplayStandupBag(VersionCmbBox.Text)

                BagsPanel.Visible = True
                TotalUnitPanel.Visible = True

                If FormulaTypeCmbBox.Text = "Powder" Then
                    HideUnhideFlavorProfile(False)
                    CalculateBlending()
                End If
                CalculateStandUpBagCost()
            ElseIf ComboBox3.Text = "Blister" Then
                'Blisters
                Label22.Text = "Sales Bulk Blister"
                lblBox.Text = "Sales Box Blister Unit"
                AddRemoveHandlerBlister(True)

                DisplayBlister(VersionCmbBox.Text)
                BlistersPanel.Visible = True
                TotalUnitPanel.Visible = True
                CalculateBlisterCost()
            End If

            DisplaySalesData()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DisplaySalesData()
        Try
            If ComboBox3.SelectedIndex = 0 Then
                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat = " & ComboBox3.SelectedIndex & " And VersionNumber = " & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)

                DisplayBulkSalesData()

                BindingSource2.DataSource = dsSal.Tables("salestxns")
                BindingSource2.Filter = "PackagingFormat = " & ComboBox3.SelectedIndex & " AND [Type] = 'BULK' AND VersionNumber = '" & VersionCmbBox.Text & "'"

            ElseIf ComboBox3.SelectedIndex = 1 Then
                If ComboBox4.SelectedIndex <> -1 Then

                    SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & "AND VersionNumber=" & VersionCmbBox.Text & " AND UnitSize =" & ComboBox4.Text, "", DataViewRowState.CurrentRows)

                    If SalesDVAuto.Count = 0 Then
                        For j As Integer = 1 To 5
                            Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                            workRow("FormulaID") = FormulaID.Text
                            workRow("PackagingFormat") = ComboBox3.SelectedIndex
                            workRow("UnitSize") = ComboBox4.Text
                            workRow("Quantity") = 2500 * j
                            workRow("MarginPercentage") = 40 - (j * 5)
                            workRow("SalesPrice") = DBNull.Value
                            workRow("Type") = "BULK"
                            workRow("VersionNumber") = VersionCmbBox.Text
                            workRow("OverriddenSalesPrice") = DBNull.Value
                            workRow("Overridden") = 0
                            dsSal.Tables("salestxns").Rows.Add(workRow)
                        Next
                    End If

                    BindingSource2.DataSource = dsSal.Tables("salestxns")
                    BindingSource2.Filter = "PackagingFormat = " & ComboBox3.SelectedIndex & " AND [Type] = 'BULK' AND VersionNumber='" & VersionCmbBox.Text & "' AND UnitSize =" & ComboBox4.Text
                End If
            ElseIf ComboBox3.SelectedIndex = 4 Then
                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " AND [Type] = 'BULK' AND VersionNumber=" & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)

                If SalesDVAuto.Count = 0 Then
                    For j As Integer = 1 To 5
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                        workRow("FormulaID") = FormulaID.Text
                        workRow("PackagingFormat") = ComboBox3.SelectedIndex
                        workRow("UnitSize") = ServingSizeTextBox.Text
                        workRow("Quantity") = 2500 * j
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("Type") = "BULK"
                        workRow("VersionNumber") = VersionCmbBox.Text
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)
                    Next
                End If
                BindingSource2.DataSource = dsSal.Tables("salestxns")
                BindingSource2.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND [Type] = 'BULK' AND VersionNumber='" & VersionCmbBox.Text & "'"

                If chkDisplayBox.Checked Then
                    SalesDVAuto = New DataView(dsSalBox.Tables("salesBoxTxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " AND [Type] = 'BOX' AND VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text), "", DataViewRowState.CurrentRows)

                    If SalesDVAuto.Count = 0 Then
                        For j As Integer = 1 To 5
                            Dim workRow As DataRow = dsSalBox.Tables("salesBoxTxns").NewRow()
                            workRow("FormulaID") = FormulaID.Text
                            workRow("PackagingFormat") = ComboBox3.SelectedIndex
                            workRow("UnitSize") = ServingSizeTextBox.Text
                            workRow("Quantity") = 2500 * j
                            workRow("MarginPercentage") = 40 - (j * 5)
                            workRow("SalesPrice") = DBNull.Value
                            workRow("Type") = "BOX"
                            workRow("VersionNumber") = VersionCmbBox.Text
                            workRow("OverriddenSalesPrice") = DBNull.Value
                            workRow("Overridden") = 0
                            dsSalBox.Tables("salesBoxTxns").Rows.Add(workRow)
                        Next
                    End If

                    BindSalesBoxGrid.DataSource = dsSalBox.Tables("salesBoxTxns")
                    BindSalesBoxGrid.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND Type='BOX' AND VersionNumber='" & If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text) & "'"
                    CalculateBulkSales(gridBoxSales)
                End If

            ElseIf ComboBox3.SelectedIndex = 2 Or ComboBox3.SelectedIndex = 3 Or ComboBox3.SelectedIndex = 5 Then

                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " AND [Type] = 'BULK' AND VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text), "", DataViewRowState.CurrentRows)

                If SalesDVAuto.Count = 0 Then
                    For j As Integer = 1 To 5
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()

                        workRow("FormulaID") = FormulaID.Text
                        workRow("PackagingFormat") = ComboBox3.SelectedIndex
                        workRow("UnitSize") = ServingSizeTextBox.Text
                        workRow("Quantity") = 25000 * j
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("Type") = "BULK"
                        workRow("VersionNumber") = VersionCmbBox.Text
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)
                    Next
                End If
                BindingSource2.DataSource = dsSal.Tables("salestxns")
                BindingSource2.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND [Type] = 'BULK' AND VersionNumber='" & If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text) & "'"


                If chkDisplayBox.Checked Then
                    SalesDVAuto = New DataView(dsSalBox.Tables("salesBoxTxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " AND [Type] = 'BOX' AND VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text), "", DataViewRowState.CurrentRows)

                    If SalesDVAuto.Count = 0 Then
                        For j As Integer = 1 To 5
                            Dim workRow As DataRow = dsSalBox.Tables("salesBoxTxns").NewRow()
                            workRow("FormulaID") = FormulaID.Text
                            workRow("PackagingFormat") = ComboBox3.SelectedIndex
                            workRow("UnitSize") = ServingSizeTextBox.Text
                            workRow("Quantity") = 2500 * j
                            workRow("MarginPercentage") = 40 - (j * 5)
                            workRow("SalesPrice") = DBNull.Value
                            workRow("Type") = "BOX"
                            workRow("VersionNumber") = VersionCmbBox.Text
                            workRow("OverriddenSalesPrice") = DBNull.Value
                            workRow("Overridden") = 0
                            dsSalBox.Tables("salesBoxTxns").Rows.Add(workRow)
                        Next
                    End If

                    BindSalesBoxGrid.DataSource = dsSalBox.Tables("salesBoxTxns")
                    BindSalesBoxGrid.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND Type='BOX' AND VersionNumber='" & If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text) & "'"
                    CalculateBulkSales(gridBoxSales)
                End If

                If (ComboBox3.Text = "Stick Packs" Or ComboBox3.Text = "Sachets") And chkDisplayBag.Checked Then
                    populateBagSalesGrid()
                    'CalculateBulkSales(gridBoxSales)
                    CalculateBulkSales(gridbBulkSalesBags)
                End If
            End If

            gridBulkSales.AutoGenerateColumns = False
            CalculateBulkSales(gridBulkSales)
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub DisplayBulkSalesData()
        Try
            If FormulaTypeCmbBox.SelectedIndex = 0 Then
                If SalesDVAuto.Count = 0 Then
                    Dim i As Int16 = 1
                    For j As Integer = 1 To 6
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                        workRow("FormulaID") = FormulaID.Text
                        workRow("PackagingFormat") = ComboBox3.SelectedIndex
                        workRow("UnitSize") = 0
                        If j = 1 Then
                            workRow("Quantity") = 300
                        Else
                            workRow("Quantity") = 500 * i
                            i = i + 1
                        End If
                        workRow("Type") = "BULK"
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("VersionNumber") = VersionCmbBox.Text
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)

                    Next
                    i = 1
                End If
            ElseIf FormulaTypeCmbBox.SelectedIndex = 1 Or FormulaTypeCmbBox.SelectedIndex = 2 Then
                If SalesDVAuto.Count = 0 Then
                    For j As Integer = 1 To 5
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                        workRow("FormulaID") = FormulaID.Text
                        workRow("PackagingFormat") = ComboBox3.SelectedIndex
                        workRow("UnitSize") = 0
                        If j = 5 Then
                            workRow("Quantity") = 250000 * 6
                        Else
                            workRow("Quantity") = 250000 * j
                        End If
                        workRow("Type") = "BULK"
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("VersionNumber") = VersionCmbBox.Text
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)
                    Next
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateStickPacksCost()
        Try
            If ComboBox3.Text = "Stick Packs" Then
                lblAdditionalChargesBulk.Text = 0
                lblAdditionalChargesBox.Text = 0
                lblAdditionalChargeBag.Text = 0

                CalculateStickBulkUnitCost()

                If chkDisplayBox.Checked Then
                    CalculateStickBoxUnitCost()
                ElseIf chkDisplayBag.Checked Then
                    CalculateStickBagUnitCost()
                End If


                If chkDisplayBox.Checked Then
                    TotalUnitCost.Text = lblTotalCostBox.Text
                ElseIf chkDisplayBag.Checked Then
                    TotalUnitCost.Text = lblTotalCostBag.Text
                Else
                    TotalUnitCost.Text = lblTotalCostBulk.Text
                End If
            Else
                Exit Sub
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateStickBulkUnitCost()
        Try
            lblAdditionalChargesBulk.Text = 0
            'Step1
            Dim TotalPerKgCost As Double
            Double.TryParse(TextBox8.Text, TotalPerKgCost) ' Total Cost per kg

            Dim ServingSize As Double
            Double.TryParse(TextBox16.Text, ServingSize) 'Serving Size

            Dim PrePackagingCost As Double = (TotalPerKgCost / 1000) * ServingSize

            'Adding flavour Profile cost to Powder
            If FormulaTypeCmbBox.Text = "Powder" Then
                PrePackagingCost += CalculateFlavourProfileCost()
            End If

            lblPreAmtStickPack.Text = PrePackagingCost.ToString("N2")

            Dim PackagingMaterialCost As Double = 0
            'Step2
            Dim AddValue As New Collection
            AddValue.Add(TextBox18) 'Filling Cost
            AddValue.Add(TextBox19)

            For i As Integer = 1 To AddValue.Count
                If AddValue(i).Text = "." Or String.IsNullOrEmpty(AddValue(i).Text) Then
                    Exit Sub
                Else
                    Dim ValueToAdd As Double = (AddValue(i).Text / 1000)
                    PackagingMaterialCost += ValueToAdd
                End If

            Next

            'Calculating lblAdditionalChargesBulk.Text 
            Dim FillingCost As Double
            Double.TryParse(TextBox18.Text, FillingCost)

            'ShipperCase Cost
            Dim ShipperCaseConut As Double
            If txtStickShipperCountBulk.Text = "" Or txtStickShipperCountBulk.Text = "0" Or txtStickShipperCountBulk.Text = "0.00" Then
                ShipperCaseConut = 1
            Else
                ShipperCaseConut = txtStickShipperCountBulk.Text
            End If

            Dim StickShipperCostBulk As Double
            Double.TryParse(txtStickShipperCostBulk.Text, StickShipperCostBulk)

            lblAdditionalChargesBulk.Text += FillingCost / 1000
            lblAdditionalChargesBulk.Text += StickShipperCostBulk / ShipperCaseConut
            PackagingMaterialCost += StickShipperCostBulk / ShipperCaseConut

            lblAmtStickPack.Text = Convert.ToDecimal(PackagingMaterialCost).ToString("N2")
            lblTotalCostBulk.Text = (PackagingMaterialCost + PrePackagingCost).ToString("N2")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateStickBoxUnitCost()
        Try
            'Step 1
            Dim TotalPerKgCost As Double
            Double.TryParse(TextBox8.Text, TotalPerKgCost) ' Total Per Kg

            Dim ServingSize As Double
            Double.TryParse(TextBox16.Text, ServingSize)   ' Serving Weight

            Dim StickPackPerIfc As Double
            Double.TryParse(txtStickPacks.Text, StickPackPerIfc) ' Stickpack



            Dim PrePackagingCost As Double = (TotalPerKgCost / 1000) * ServingSize * StickPackPerIfc  'Label to store Box Text

            'Adding flavour Profile cost to Powder
            If FormulaTypeCmbBox.Text = "Powder" Then
                PrePackagingCost += CalculateFlavourProfileCost(txtStickPacks)
            End If

            lblPreAmtStickPack.Text = PrePackagingCost.ToString("N2")

            Dim PackagingMaterialCost As Decimal = 0
            'Step 2
            Dim AddValue As New Collection
            AddValue.Add(TextBox18) 'Filling Cost
            AddValue.Add(TextBox19) 'Printing Cost


            For i As Integer = 1 To AddValue.Count
                If AddValue(i).Text = "." Or String.IsNullOrEmpty(AddValue(i).Text) Then
                    Exit Sub
                Else
                    Dim ValueToAdd As Double = (AddValue(i).Text / 1000) * StickPackPerIfc
                    PackagingMaterialCost += ValueToAdd
                End If

            Next

            'Step 3
            Dim cost As Double
            Double.TryParse(TextBox20.Text, cost)
            Dim DisplayCost = cost / 1000 'DisplayCost
            PackagingMaterialCost += DisplayCost

            Dim ShrinkwrapCost As Double
            Double.TryParse(txtShrinkWrapStickPack.Text, ShrinkwrapCost) ' ShrinkWrap

            Dim SecondaryPackOutCost As Double
            Double.TryParse(TextBox23.Text, SecondaryPackOutCost) ' Secondary Packout

            Dim WaferSealCost As Double
            Double.TryParse(txtWaferSeal.Text, WaferSealCost) ' WaferSeal

            PackagingMaterialCost += ShrinkwrapCost
            PackagingMaterialCost += SecondaryPackOutCost
            PackagingMaterialCost += WaferSealCost

            'Other
            Dim OtherCost As Double
            Double.TryParse(TextBox24.Text, OtherCost) ' Other

            'Shipper Cost
            Dim ShipperCaseConut
            If txtStickPacksShipperCaseCount.Text = "" Or txtStickPacksShipperCaseCount.Text = "0" Or txtStickPacksShipperCaseCount.Text = "0.00" Then
                ShipperCaseConut = 1
            Else
                ShipperCaseConut = txtStickPacksShipperCaseCount.Text
            End If
            Dim StickpackShipperCaseCountAmt As Double
            Double.TryParse(txtStickPacksShipperCaseCountAmt.Text, StickpackShipperCaseCountAmt)

            'Filling Cost
            Dim FillingCost As Double
            Double.TryParse(TextBox18.Text, FillingCost)

            lblAdditionalChargesBox.Text += (FillingCost / 1000) * StickPackPerIfc
            lblAdditionalChargesBox.Text += StickpackShipperCaseCountAmt / ShipperCaseConut
            lblAdditionalChargesBox.Text += OtherCost

            PackagingMaterialCost += StickpackShipperCaseCountAmt / ShipperCaseConut
            PackagingMaterialCost += OtherCost

            lblAmtStickPack.Text = Convert.ToDecimal(PackagingMaterialCost).ToString("N2")
            lblTotalCostBox.Text = (PackagingMaterialCost + PrePackagingCost).ToString("N2")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
        'lblTotalCostBag.Text = TotalUnitCostBox
    End Sub

    Private Sub CalculateStickBagUnitCost()
        Try
            'Step 1
            Dim TotalPerKgCost As Double
            Double.TryParse(TextBox8.Text, TotalPerKgCost) ' Total Per kg

            Dim ServingSize As Double
            Double.TryParse(TextBox16.Text, ServingSize) ' Serving Weight

            Dim StickPackPerIfc As Double
            Double.TryParse(txtStickPacks.Text, StickPackPerIfc) ' Stickpack per IFC


            Dim PrePackagingCost As Double = (TotalPerKgCost / 1000) * ServingSize * StickPackPerIfc

            'Adding flavour Profile cost to Powder
            If FormulaTypeCmbBox.Text = "Powder" Then
                PrePackagingCost += CalculateFlavourProfileCost(txtStickPacks)
            End If

            lblPreAmtStickPack.Text = PrePackagingCost.ToString("N2")

            Dim PackagingMaterialCost As Decimal = 0
            'Step 2
            Dim AddValue As New Collection
            AddValue.Add(TextBox18) 'Filling Cost
            AddValue.Add(TextBox19) 'Printing Cost

            For i As Integer = 1 To AddValue.Count
                If AddValue(i).Text = "." Or String.IsNullOrEmpty(AddValue(i).Text) Then
                    Exit Sub
                Else
                    Dim ValueToAdd As Double = (AddValue(i).Text / 1000) * StickPackPerIfc
                    PackagingMaterialCost += ValueToAdd
                End If
            Next
            Dim PrintingCost As Double
            Double.TryParse(txtStickPackBagPrintingCost.Text, PrintingCost)
            Dim DisplayCost = PrintingCost / 1000 'DisplayCost
            PackagingMaterialCost += DisplayCost


            'Step 3 
            Dim SecondaryPackOutCost As Double = If(Double.TryParse(txtSecondaryPackStickBag.Text, SecondaryPackOutCost), SecondaryPackOutCost, 0) 'Secondary Packout Bag

            PackagingMaterialCost += SecondaryPackOutCost

            'Filling Cost 
            Dim FillingCost As Double
            Double.TryParse(TextBox18.Text, FillingCost)
            lblAdditionalChargeBag.Text += (FillingCost / 1000) * StickPackPerIfc

            'Other
            Dim OtherCost As Double
            Double.TryParse(txtOtherCostStickBag.Text, OtherCost)
            lblAdditionalChargeBag.Text += OtherCost
            PackagingMaterialCost += OtherCost

            'Shipper Cost
            Dim ShipperCaseConut As Double
            If txtShipperCountStickBag.Text = "" Or txtShipperCountStickBag.Text = "0" Or txtShipperCountStickBag.Text = "0.00" Then
                ShipperCaseConut = 1
            Else
                ShipperCaseConut = txtShipperCountStickBag.Text
            End If

            Dim ShipperCost As Double
            Double.TryParse(txtShipperCostStickBag.Text, ShipperCost)

            lblAdditionalChargeBag.Text += ShipperCost / ShipperCaseConut
            PackagingMaterialCost += ShipperCost / ShipperCaseConut

            lblTotalCostBag_TextChanged(lblTotalCostBag, EventArgs.Empty)

            lblAmtStickPack.Text = Convert.ToDecimal(PackagingMaterialCost).ToString("N2")
            lblTotalCostBag.Text = (PackagingMaterialCost + PrePackagingCost).ToString("N2")

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub


    Private Sub CalculateSachetsCost()
        If ComboBox3.Text = "Sachets" Then

            lblAdditionalChargesBulk.Text = 0
            lblAdditionalChargesBox.Text = 0
            lblAdditionalChargeBag.Text = 0

            CalculateSachetsBulk()

            If chkDisplayBox.Checked Then
                CalculateSachetsbox()
            ElseIf chkDisplayBag.Checked Then
                CalculateSachetsBagCost()
            End If

            If chkDisplayBox.Checked Then
                TotalUnitCost.Text = lblTotalCostBox.Text
            ElseIf chkDisplayBag.Checked Then
                TotalUnitCost.Text = lblTotalCostBag.Text
            Else
                TotalUnitCost.Text = lblTotalCostBulk.Text
            End If
        Else
            Exit Sub
        End If
    End Sub


    Private Sub CalculateSachetsBulk()
        Try
            lblAdditionalChargesBulk.Text = 0
            'Step 1
            Dim TotalperkgCost As Double
            Double.TryParse(TextBox8.Text, TotalperkgCost)

            Dim ServingSize As Double
            Double.TryParse(TextBox35.Text, ServingSize)

            Dim PrePackagingCost As Double = (TotalperkgCost / 1000) * ServingSize

            'Adding Flavour Profile cost when Powder 
            If FormulaTypeCmbBox.Text = "Powder" Then
                PrePackagingCost += CalculateFlavourProfileCost()
            End If

            lblPreAmtSachets.Text = PrePackagingCost.ToString("N2")

            Dim PackagingMaterialCost As Double = 0
            'Step 2
            Dim AddValue As New Collection
            AddValue.Add(TextBox33) ' Filling Cost
            AddValue.Add(TextBox28)
            For i As Integer = 1 To AddValue.Count
                If AddValue(i).Text = "." Or String.IsNullOrEmpty(AddValue(i).Text) Then
                    Exit Sub
                Else
                    Dim ValueToAdd As Double = (AddValue(i).Text / 1000)
                    PackagingMaterialCost += ValueToAdd
                End If
            Next

            'Calculating Additional Charges
            Dim ShipperCaseConut As Double
            If txtSachetShipperCountBulk.Text = "" Or txtSachetShipperCountBulk.Text = "0" Or txtSachetShipperCountBulk.Text = "0.00" Then
                ShipperCaseConut = 1
            Else
                ShipperCaseConut = txtSachetShipperCountBulk.Text
            End If

            'ShipperCaseCount
            Dim ShipperCaseCost As Double
            Double.TryParse(txtSachetShipperCost.Text, ShipperCaseCost)

            'Filling Cost 
            Dim FillingCost As Double
            Double.TryParse(TextBox33.Text, FillingCost)

            lblAdditionalChargesBulk.Text += FillingCost / 1000
            lblAdditionalChargesBulk.Text += ShipperCaseCost / ShipperCaseConut
            PackagingMaterialCost += ShipperCaseCost / ShipperCaseConut

            lblAmtSachets.Text = (PackagingMaterialCost).ToString("N2")

            lblTotalCostBulk.Text = (PackagingMaterialCost + PrePackagingCost).ToString("N2")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub CalculateSachetsbox()
        Try
            'Step 1
            Dim TotalperkgCost As Double
            Double.TryParse(TextBox8.Text, TotalperkgCost)

            Dim ServingSize As Double
            Double.TryParse(TextBox35.Text, ServingSize)

            Dim SachetsPerIFC As Double
            Double.TryParse(txtStickPacksSachets.Text, SachetsPerIFC)

            Dim PrePackagingCost As Double = (TotalperkgCost / 1000) * ServingSize * SachetsPerIFC

            'Adding Flavour Profile cost when Powder 
            If FormulaTypeCmbBox.Text = "Powder" Then
                PrePackagingCost += CalculateFlavourProfileCost(txtStickPacksSachets)
            End If

            lblPreAmtSachets.Text = PrePackagingCost.ToString("N2")

            Dim PackagingMaterialCost As Double = 0
            'Step 2
            Dim AddValue As New Collection
            AddValue.Add(TextBox33) 'Filling Cost
            AddValue.Add(TextBox28)
            For i As Integer = 1 To AddValue.Count
                If AddValue(i).Text = "." Or String.IsNullOrEmpty(AddValue(i).Text) Then
                    Exit Sub
                Else
                    Dim ValueToAdd As Double = (AddValue(i).Text / 1000) * SachetsPerIFC
                    PackagingMaterialCost += ValueToAdd
                End If
            Next

            'Step 3 
            Dim DisplayCost As Double
            Double.TryParse(TextBox27.Text, DisplayCost)

            PackagingMaterialCost += DisplayCost / 1000

            'Step4
            Dim ShrinkWrap As Double
            Double.TryParse(TextBox30.Text, ShrinkWrap)
            Dim SecondaryPackout As Double
            Double.TryParse(txtShrinkWrapSachets.Text, SecondaryPackout)
            Dim WaferSeal As Double
            Double.TryParse(txtWaferSealSachets.Text, WaferSeal)

            PackagingMaterialCost += ShrinkWrap
            PackagingMaterialCost += SecondaryPackout
            PackagingMaterialCost += WaferSeal

            'Shipper Cost
            Dim ShipperCaseConut
            If txtSachetsShipperCaseCount.Text = "" Or txtSachetsShipperCaseCount.Text = "0" Or txtSachetsShipperCaseCount.Text = "0.00" Then
                ShipperCaseConut = 1
            Else
                ShipperCaseConut = If(String.IsNullOrEmpty(txtSachetsShipperCaseCount.Text), 0, txtSachetsShipperCaseCount.Text)
            End If

            Dim SachetsShipperCaseCountAmt As Double
            Double.TryParse(txtSachetsShipperCaseCountAmt.Text, SachetsShipperCaseCountAmt)

            'Filling Cost
            Dim FillingCost As Double
            Double.TryParse(TextBox33.Text, FillingCost)

            'Other
            Dim Other As Double
            Double.TryParse(TextBox29.Text, Other)

            lblAdditionalChargesBox.Text += (FillingCost / 1000) * SachetsPerIFC
            lblAdditionalChargesBox.Text += SachetsShipperCaseCountAmt / ShipperCaseConut
            lblAdditionalChargesBox.Text += Other

            PackagingMaterialCost += Other
            PackagingMaterialCost += SachetsShipperCaseCountAmt / ShipperCaseConut

            lblAmtSachets.Text = PackagingMaterialCost.ToString("N2")
            lblTotalCostBox.Text = (PackagingMaterialCost + PrePackagingCost).ToString("N2")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateSachetsBagCost()
        Try
            'Step 1
            Dim TotalPerKgCost As Double
            Double.TryParse(TextBox8.Text, TotalPerKgCost) ' Total Per kg

            Dim ServingSize As Double
            Double.TryParse(TextBox35.Text, ServingSize) ' Serving Weight

            Dim StickPackPerIfc As Double
            Double.TryParse(txtStickPacksSachets.Text, StickPackPerIfc) ' Stickpack per IFC


            Dim PrePackagingCost As Double = (TotalPerKgCost / 1000) * ServingSize * StickPackPerIfc

            'Adding flavour Profile cost to Powder
            If FormulaTypeCmbBox.Text = "Powder" Then
                PrePackagingCost += CalculateFlavourProfileCost(txtStickPacksSachets)
            End If

            lblPreAmtSachets.Text = PrePackagingCost.ToString("N2")

            Dim PackagingMaterialCost As Decimal = 0
            'Step 2
            Dim AddValue As New Collection
            AddValue.Add(TextBox33) 'Filling Cost
            AddValue.Add(TextBox28) 'Printing Cost

            For i As Integer = 1 To AddValue.Count
                If AddValue(i).Text = "." Or String.IsNullOrEmpty(AddValue(i).Text) Then
                    Exit Sub
                Else
                    Dim ValueToAdd As Double = (AddValue(i).Text / 1000) * StickPackPerIfc
                    PackagingMaterialCost += ValueToAdd
                End If
            Next
            Dim PrintingCost As Double
            Double.TryParse(txtSachetsBagPrintingCost.Text, PrintingCost)
            Dim DisplayCost = PrintingCost / 1000 'DisplayCost
            PackagingMaterialCost += DisplayCost


            'Step 3 
            Dim SecondaryPackOutCost As Double = If(Double.TryParse(txtSachetsBagSecondaryPackout.Text, SecondaryPackOutCost), SecondaryPackOutCost, 0) 'Secondary Packout Bag

            PackagingMaterialCost += SecondaryPackOutCost

            'Filling Cost 
            Dim FillingCost As Double
            Double.TryParse(TextBox33.Text, FillingCost)
            lblAdditionalChargeBag.Text += (FillingCost / 1000) * StickPackPerIfc

            'Other
            Dim OtherCost As Double
            Double.TryParse(txtSachetsBagOther.Text, OtherCost)
            lblAdditionalChargeBag.Text += OtherCost
            PackagingMaterialCost += OtherCost

            'Shipper Cost
            Dim ShipperCaseConut As Double
            If txtSachetsBagShipperCaseCount.Text = "" Or txtSachetsBagShipperCaseCount.Text = "0" Or txtSachetsBagShipperCaseCount.Text = "0.00" Then
                ShipperCaseConut = 1
            Else
                ShipperCaseConut = txtSachetsBagShipperCaseCount.Text
            End If

            Dim ShipperCost As Double
            Double.TryParse(txtSachetsBagShipperCaseCost.Text, ShipperCost)

            lblAdditionalChargeBag.Text += ShipperCost / ShipperCaseConut
            PackagingMaterialCost += ShipperCost / ShipperCaseConut

            lblTotalCostBag_TextChanged(lblTotalCostBag, EventArgs.Empty)

            lblAmtSachets.Text = Convert.ToDecimal(PackagingMaterialCost).ToString("N2")
            lblTotalCostBag.Text = (PackagingMaterialCost + PrePackagingCost).ToString("N2")

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateBlisterCost()
        If ComboBox3.Text = "Blister" Then
            lblAdditionalChargesBulk.Text = 0
            lblAdditionalChargesBox.Text = 0
            CalculateBlisterBulk()
            If chkDisplayBox.Checked Then
                CalculateBlisterBox()
            End If
            If chkDisplayBox.Checked Then
                TotalUnitCost.Text = lblTotalCostBox.Text
            Else
                TotalUnitCost.Text = lblTotalCostBulk.Text
            End If
        Else
            Exit Sub
        End If
    End Sub
    Private Sub CalculateBlisterBulk()
        Try
            lblAdditionalChargesBulk.Text = 0
            'Step 1
            Dim TotalPerkgCost As Double
            Double.TryParse(TextBox8.Text, TotalPerkgCost) 'Total per 1000
            Dim BlisterCount As Double
            Double.TryParse(txtBlisterCount.Text, BlisterCount) 'Blister Count

            Dim PrePackagingCost As Double = Double.Parse(TotalPerkgCost / 1000) * If(String.IsNullOrEmpty(BlisterCount), "0", BlisterCount)

            ''Adding flavour Profile cost to Powder
            'If FormulaTypeCmbBox.Text = "Powder" Then
            '    PrePackagingCost += CalculateFlavourProfileCost(ServingSizeTextBox)
            'End If

            lblPreAmtBlister.Text = PrePackagingCost.ToString("N2")

            Dim PackagingMaterialCost As Double = 0

            'Blister Cost
            Dim BlisterCost As Double
            Double.TryParse(txtBlisterCost.Text, BlisterCost) 'Blister Cost
            lblAdditionalChargesBulk.Text += BlisterCost
            PackagingMaterialCost += BlisterCost

            'Printing Cost
            Dim PrintingCost As Double
            Double.TryParse(txtBlisterPrintingCostAmt.Text, PrintingCost)
            lblAdditionalChargesBulk.Text += PrintingCost / 1000
            PackagingMaterialCost += PrintingCost / 1000

            'Shipper Cost
            Dim ShipperCaseConut As Double
            If txtShipperCountBulk.Text = "" Or txtShipperCountBulk.Text = "0" Or txtShipperCountBulk.Text = "0.00" Then
                ShipperCaseConut = 1
            Else
                ShipperCaseConut = txtShipperCountBulk.Text
            End If
            Dim ShipperCost As Double
            Double.TryParse(txtShipperCostBulk.Text, ShipperCost)
            lblAdditionalChargesBulk.Text += ShipperCost / ShipperCaseConut
            PackagingMaterialCost += ShipperCost / ShipperCaseConut

            lblAmtBlister.Text = PackagingMaterialCost.ToString("N2")
            lblTotalCostBulk.Text = (PackagingMaterialCost + PrePackagingCost).ToString("N2")

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub CalculateBlisterBox()
        Try
            'Step 1
            Dim TotalPerkgCost As Double
            Double.TryParse(TextBox8.Text, TotalPerkgCost) 'Total per 1000

            Dim BlisterCount As Double
            Double.TryParse(txtBlisterCount.Text, BlisterCount) 'Blister Count

            Dim PrePackagingCost As Double = Double.Parse(TotalPerkgCost / 1000) * BlisterCount


            lblPreAmtBlister.Text = PrePackagingCost.ToString("N2")

            Dim PackagingMaterialCost As Double = 0

            'Step 4
            Dim DisplayBoxAmount As Double
            Double.TryParse(txtAmontDisplaybox.Text, DisplayBoxAmount)
            Dim DisplayCost = DisplayBoxAmount / 1000

            PackagingMaterialCost += DisplayCost

            'Step 5
            Dim ShrinkWrapCost As Double
            Double.TryParse(txtBlisterShrinkWrap.Text, ShrinkWrapCost)
            PackagingMaterialCost += ShrinkWrapCost

            Dim PackoutCost As Double
            Double.TryParse(txtBlisterPackoutAmt.Text, PackoutCost)
            PackagingMaterialCost += PackoutCost

            Dim WaferSeal As Double
            Double.TryParse(txtBlisterWaferSeal.Text, WaferSeal)
            PackagingMaterialCost += WaferSeal

            'Blister Per Box Cost
            Dim BlisterPerBox As Double
            Double.TryParse(txtBlisterCountAmount.Text, BlisterPerBox)

            Dim BlisterCost As Double
            Double.TryParse(txtBlisterCost.Text, BlisterCost)

            Dim BlisterPerBoxCost = BlisterPerBox * BlisterCost

            'Other Cost
            Dim OtherCost As Double
            Double.TryParse(txtBlisterOtherAmt.Text, OtherCost)

            'Shipper Cost
            Dim ShipperCaseConut As Double
            If txtBlisterCaseCount.Text = "" Or txtBlisterCaseCount.Text = "0" Or txtBlisterCaseCount.Text = "0.00" Then
                ShipperCaseConut = 1
            Else
                ShipperCaseConut = txtBlisterCaseCount.Text
            End If
            Dim ShipperCaseCountCost As Double
            Double.TryParse(txtBlisterCaseCountAmt.Text, ShipperCaseCountCost)

            'Printing Cost
            Dim AddValue As New Collection
            AddValue.Add(txtBlisterPrintingCostAmt)

            For i As Integer = 1 To AddValue.Count
                If AddValue(i).Text = "." Or String.IsNullOrEmpty(AddValue(i).Text) Then
                    Exit Sub
                Else
                    Dim ValueToAdd As Double = (AddValue(i).Text / 1000) * BlisterPerBox
                    lblAdditionalChargesBox.Text += ValueToAdd
                    PackagingMaterialCost += ValueToAdd
                End If
            Next

            lblAdditionalChargesBox.Text += ShipperCaseCountCost / ShipperCaseConut
            lblAdditionalChargesBox.Text += OtherCost
            lblAdditionalChargesBox.Text += BlisterPerBoxCost

            PackagingMaterialCost += ShipperCaseCountCost / ShipperCaseConut
            PackagingMaterialCost += OtherCost
            PackagingMaterialCost += BlisterPerBoxCost

            lblAmtBlister.Text = PackagingMaterialCost.ToString("N2")
            lblTotalCostBox.Text = (PackagingMaterialCost + PrePackagingCost).ToString("N2")

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateStandUpBagCost()
        Try
            If ComboBox3.Text = "Bags" Then

                lblAdditionalChargesBulk.Text = 0

                Dim TotalPerKgCost As Double
                Double.TryParse(TextBox8.Text, TotalPerKgCost)

                Dim FillWeight As Double
                Double.TryParse(txtFillWeight.Text, FillWeight)

                Dim Other As Double
                Double.TryParse(txtOther.Text, Other)

                Dim FillingCost As Double
                Double.TryParse(txtSecondaryPackoutBag.Text, FillingCost)

                Dim PrePackagingCost As Double = (TotalPerKgCost / 1000) * FillWeight

                'Adding flavour Profile cost to Powder
                If FormulaTypeCmbBox.Text = "Powder" Then
                    PrePackagingCost += CalculateFlavourProfileCost(txtServing)
                End If

                lblPreAmtStandupBag.Text = PrePackagingCost.ToString("N2")

                Dim PackagingMaterialCost As Double = 0
                Dim ShipperCaseConut
                If txtShipperCaseCount.Text = "" Or txtShipperCaseCount.Text = "0" Or txtShipperCaseCount.Text = "0.00" Then
                    ShipperCaseConut = 1
                Else
                    ShipperCaseConut = If(String.IsNullOrEmpty(txtShipperCaseCount.Text), 0, txtShipperCaseCount.Text)
                End If
                Dim ShipperCaseCount As Double
                Double.TryParse(txtShipperCaseAmt.Text, ShipperCaseCount)
                Dim ShipperCost = ShipperCaseCount / ShipperCaseConut

                Dim PrintingCost As Double
                Double.TryParse(txtPrintingCost.Text, PrintingCost)
                PackagingMaterialCost += (PrintingCost / 1000)
                PackagingMaterialCost += ShipperCost
                PackagingMaterialCost += Other
                PackagingMaterialCost += FillingCost

                lblAmtStandupBag.Text = PackagingMaterialCost.ToString("N2")
                lblTotalCostBulk.Text = (PackagingMaterialCost + PrePackagingCost).ToString("##,#0.###")

                TotalUnitCost.Text = lblTotalCostBulk.Text

                'Calculating lblAdditionalChargesBulk.Text
                lblAdditionalChargesBulk.Text += ShipperCost
                lblAdditionalChargesBulk.Text += Other
                lblAdditionalChargesBulk.Text += FillingCost
            Else
                Exit Sub
            End If
        Catch ex As System.Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateBottlingCost()
        Try
            If ComboBox3.Text = "Bottles" Then
                lblAdditionalChargesBulk.Text = 0

                If ComboBox4.SelectedIndex = -1 Then
                    Exit Sub
                End If
                If IsNothing(DataGridView2.DataSource) Then
                    Exit Sub
                End If

                Dim cst As Double = TextBox8.Text
                'total cost per unit
                Dim tcu As Double = (cst / 1000) * ComboBox4.Text

                If FormulaTypeCmbBox.Text = "Powder" Then

                    Dim FillWeight As Double
                    FillWeight = Math.Round(ComboBox4.Text * ServingSizeTextBox.Text)
                    tcu = (cst / 1000) * FillWeight

                    'Adding Flavour Profile cost when Powder 
                    If FormulaTypeCmbBox.Text = "Powder" Then
                        tcu += CalculateFlavourProfileCost(ComboBox4)
                    End If

                    Label26.Text = tcu.ToString("##,#0.00")
                    'Label27.Text = "Fill Weight:"
                    Label28.Text = FillWeight
                Else
                    tcu = (cst / 1000) * ComboBox4.Text
                    Label26.Text = tcu.ToString("##,#0.00")
                    'Label27.Text = "Servings/Container:"
                    Label28.Text = Math.Ceiling(ComboBox4.Text / ServingSizeTextBox.Text)
                End If

                Dim ci As Integer = DataGridView2.Columns.Item("Column13").Index

                Dim Material As Integer = DataGridView2.Columns.Item("Column4").Index

                Dim tm As Double
                For j As Integer = 0 To DataGridView2.Rows.Count - 1

                    'Dim skipList As String() = {"Desiccant", "Cotton", "Neckband", "Shippers", "Labor", "Other"}

                    'If skipList.Contains(DataGridView2.Rows(j).Cells(Material).Value.ToString()) Then
                    '    Continue For
                    'End If

                    If Not IsDBNull(DataGridView2.Rows(j).Cells(ci).Value) Then
                        tm += DataGridView2.Rows(j).Cells(ci).Value
                    End If
                Next
                tcu += tm

                TotalUnitCost.Text = tcu.ToString("N2")

                lblTotalCostBulk.Text = TotalUnitCost.Text

                lblAdditionalChargesBulk.Text = CalculateBottleSizeAdditionalCategories()

            Else
                Exit Sub
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DisplayBottling()

        DataGridView2.DataSource = Nothing

        Try
            If ComboBox4.Items.Count >= 0 Then
                Dim Combobox4Index = ComboBox4.SelectedIndex
                ComboBox4.Items.Clear()
                ComboBox4.Text = ""
                Dim ValidRows = dsPB.Tables("bottles").AsEnumerable().Where(Function(c) c.RowState <> DataRowState.Deleted)
                Dim UnitSizes = (From c In ValidRows Where c.Item("VersionNumber") = VersionCmbBox.Text Select c.Item("SizeCount")).Distinct()
                For Each Unit In UnitSizes
                    ComboBox4.Items.Add(Unit)
                Next
                ComboBox4.SelectedIndex = Combobox4Index
            End If

            If ComboBox4.Items.Count > 0 And ComboBox4.SelectedIndex = -1 Then
                ComboBox4.SelectedIndex = 0
            End If
            If ComboBox4.SelectedIndex <> -1 Then

                Dim custDV As DataView = New DataView(dsPB.Tables("bottles"), "SizeCount = " & ComboBox4.Text & " AND VersionNumber=" & VersionCmbBox.Text, "BindingIndex ASC", DataViewRowState.CurrentRows)
                DataGridView2.Enabled = True

                'checks if dataset exist or one needs to be created
                If IsNothing(DataGridView2.DataSource) Then
                    DataGridView2.AutoGenerateColumns = False
                    DataGridView2.DataSource = custDV
                    For Each col In DataGridView2.Columns
                        col.sortmode = DataGridViewColumnSortMode.NotSortable
                    Next
                End If

                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " AND VersionNumber=" & VersionCmbBox.Text & " AND UnitSize =" & ComboBox4.Text, "", DataViewRowState.CurrentRows)
                BindingSource2.DataSource = dsSal.Tables("salestxns")
                BindingSource2.Filter = "PackagingFormat =" & ComboBox3.SelectedIndex & " AND VersionNumber=" & VersionCmbBox.Text & " AND UnitSize =" & ComboBox4.Text

                CalculateBottlingCost()

                'lblBottleUnitCostSumAMT.Text = CalculateBottleSizeCount()
                lblBottleUnitCostSumAMT.Text = CalculateBottleSizeTotalPackagingCost()
                CalculateBulkSales(gridBulkSales)
            Else
                Label26.Text = 0.00
                TotalUnitCost.Text = 0.00
                Label28.Text = 0
                DataGridView2.DataSource = Nothing
                DataGridView2.Enabled = False
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
        End Try
    End Sub
    Private Sub ComboBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox4.KeyPress
        Try
            Dim KeyAscii As Integer = Asc(e.KeyChar)
            Select Case KeyAscii
                Case 8, 27, 48 To 57, 9
                Case Else
                    KeyAscii = 0
            End Select

            If KeyAscii = 0 Then
                e.Handled = True
            Else
                e.Handled = False
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub ComboBox4_KeyDown(sender As Object, e As KeyEventArgs) Handles ComboBox4.KeyDown
        Try
            If e.KeyCode = Keys.Enter And ComboBox4.Items.Count = 0 Then
                If String.IsNullOrEmpty(ComboBox4.Text) Then
                    Exit Sub
                End If
                Dim result As DialogResult = MessageBox.Show("Add new size?", "NewSize", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    ComboBox4.Text = ""
                    ComboBox4.SelectedIndex = -1
                ElseIf result = DialogResult.Yes Then
                    'Checks if size exist
                    For j As Integer = 0 To ComboBox4.Items.Count - 1
                        If ComboBox4.Items(j).ToString = ComboBox4.Text Then
                            ComboBox4.SelectedIndex = j
                            Exit Sub
                        End If
                    Next


                    For j As Integer = 0 To 9
                        Dim nr As DataRow = dsPB.Tables(ComboBox3.Text).NewRow()
                        nr("BindingIndex") = j
                        nr("FormulaID") = FormulaID.Text
                        nr("SizeCount") = ComboBox4.Text
                        If j = 0 Then
                            nr("Category") = "Bottles"
                            'nr("MaterialName") =
                        ElseIf j = 1 Then
                            nr("Category") = "Lids"
                        ElseIf j = 2 Then
                            nr("Category") = "Scoops"
                        ElseIf j = 3 Then
                            nr("Category") = "Desiccant"
                            nr("MaterialName") = "Yes"
                        ElseIf j = 4 Then
                            nr("Category") = "Cotton"
                            If FormulaTypeCmbBox.Text = "Powder" Then
                                nr("MaterialName") = "No"
                            Else
                                nr("MaterialName") = "Yes"
                            End If
                        ElseIf j = 5 Then
                            nr("Category") = "Neckband"
                            nr("MaterialName") = "Yes"
                        ElseIf j = 6 Then
                            nr("Category") = "Labels"
                            nr("MaterialName") = "Customer Supplied"
                        ElseIf j = 7 Then
                            nr("Category") = "Shippers"
                            nr("MaterialName") = "24 Pack"

                        ElseIf j = 8 Then
                            nr("Category") = "Labor"
                        ElseIf j = 9 Then
                            nr("Category") = "Other"
                        End If
                        'nr("MaterialName") = DBNull.Value
                        nr("VendorName") = DBNull.Value

                        If j = 3 Then
                            nr("UnitCost") = 0.05
                        ElseIf j = 4 Then
                            If FormulaTypeCmbBox.Text = "Powder" Then
                                nr("UnitCost") = 0.00
                            Else
                                nr("UnitCost") = 0.05
                            End If
                        ElseIf j = 5 Then
                            nr("UnitCost") = 0.05
                        Else
                            nr("UnitCost") = DBNull.Value
                        End If

                        If j = 8 Then
                            nr("UnitCost") = "0.65"
                        End If

                        nr("ComponentCode") = DBNull.Value
                        nr("IsDefault") = True
                        nr("EnteredBy") = Dashboard.UserIDLabel.Text
                        nr("EnteredDate") = DateTime.Now
                        nr("VersionNumber") = VersionCmbBox.Text
                        dsPB.Tables(ComboBox3.Text).Rows.Add(nr)
                    Next
                    'ComboBox4.Items.Add(ComboBox4.Text)
                    'ComboBox4.SelectedIndex = ComboBox4.Items.Count - 1

                    For j As Integer = 1 To 5
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                        workRow("FormulaID") = FormulaID.Text
                        workRow("PackagingFormat") = ComboBox3.SelectedIndex
                        workRow("UnitSize") = Convert.ToDecimal(ComboBox4.Text)
                        workRow("Quantity") = 2500 * j
                        workRow("VersionNumber") = VersionCmbBox.Text
                        workRow("Type") = "BULK"
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)
                    Next

                    SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat = " & ComboBox3.SelectedIndex & " AND UnitSize = " & ComboBox4.Text & " AND VersionNumber = " & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)
                    BindingSource2.Filter = "PackagingFormat =" & ComboBox3.SelectedIndex & " AND UnitSize = " & ComboBox4.Text & " AND VersionNumber = " & VersionCmbBox.Text
                    ComboBox4.Items.Add(ComboBox4.Text)
                    ComboBox4.SelectedIndex = ComboBox4.Items.Count - 1
                    'BindingSource2.DataSource = SalesDVAuto
                    'If ComboBox3.Text = "Bulk" Then
                    '    For j As Integer = 0 To gridBulkSales.Rows.Count - 1
                    '        CalculateSales(j, TextBox8.Text)

                    '    Next
                    'Else
                    '    For j As Integer = 0 To gridBulkSales.Rows.Count - 1
                    '        CalculateSales(j, TotalUnitCost.Text)
                    '    Next

                    'End If
                    CalculateBulkSales(gridBulkSales)
                    DisplaySalesData()
                    'lblBottleUnitCostSumAMT.Text = CalculateBottleSizeCount()
                    lblBottleUnitCostSumAMT.Text = CalculateBottleSizeTotalPackagingCost()
                End If

            ElseIf e.KeyCode = Keys.Enter And ComboBox4.Items.Count >= 1 Then

                Dim response As DialogResult = MessageBox.Show("Update current size to " & ComboBox4.Text & "?", "Update current size", MessageBoxButtons.YesNo)

                If response = DialogResult.Yes Then
                    Dim Size = ComboBox4.Items(0)
                    Dim filter As String

                    'Updating Bottle Size Count
                    Dim Bottle As New DataView
                    filter = " VersionNumber = " & VersionCmbBox.Text
                    Bottle = New DataView(dsPB.Tables("bottles"), filter, "", DataViewRowState.CurrentRows)
                    For Each row As DataRowView In Bottle
                        row("SizeCount") = ComboBox4.Text
                    Next

                    'Updating Sales Table values 

                    BindingSource2.EndEdit()
                    BindingSource2.SuspendBinding()

                    Dim SalesDV As DataView
                    filter = "PackagingFormat = " & ComboBox3.SelectedIndex & " AND UnitSize =" & Size & " AND [Type] = 'BULK' AND VersionNumber = " & VersionCmbBox.Text
                    SalesDV = New DataView(dsSal.Tables("salestxns"), filter, "", DataViewRowState.CurrentRows)

                    'For Each row As DataRowView In SalesDV
                    '    row("UnitSize") = ComboBox4.Text
                    'Next

                    'BindingSource2.DataSource = dsSal.Tables("salestxns")

                    'SalesDV = New DataView(dsSal.Tables("salestxns"), "PackagingFormat = " & ComboBox3.SelectedIndex & " AND UnitSize =" & ComboBox4.Text & " AND [Type] = 'BULK' AND VersionNumber = " & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)

                    'BindingSource2.Filter = "PackagingFormat = " & ComboBox3.SelectedIndex & " AND UnitSize =" & ComboBox4.Text & " AND [Type] = 'BULK' AND VersionNumber='" & VersionCmbBox.Text & "'"


                    'Dim rows() As DataRow
                    'rows = dsSal.Tables("salestxns").Select("PackagingFormat = " & ComboBox3.SelectedIndex &
                    '                   " AND UnitSize =" & Size &
                    '                   " AND [Type] = 'BULK' AND VersionNumber = " & VersionCmbBox.Text)

                    'For Each row As DataRow In rows
                    '    row("UnitSize") = ComboBox4.Text
                    'Next


                    Dim rowsToUpdate As New List(Of DataRowView)

                    For Each row As DataRowView In SalesDV
                        rowsToUpdate.Add(row)
                    Next

                    For Each row As DataRowView In rowsToUpdate
                        row.BeginEdit()
                        row("UnitSize") = ComboBox4.Text
                        row.EndEdit()
                    Next
                    BindingSource2.ResumeBinding()





                    ComboBox4.Items.Clear()
                    ComboBox4.Items.Add(ComboBox4.Text)
                End If

                ComboBox4.Text = ComboBox4.Items(0)

            End If
            If e.KeyCode = Keys.Enter Then
                DisplayBottling()
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    Private Sub ComboBox4_LostFocus(sender As Object, e As EventArgs) Handles ComboBox4.LostFocus
        Try
            If String.IsNullOrEmpty(ComboBox4.Text) Then
                ComboBox4.SelectedIndex = ComboBox4.Items.Count - 1
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            If ComboBox4.SelectedIndex = -1 Then
                Exit Sub
            End If

            Dim result As DialogResult = MessageBox.Show("Remove this size?", "Remove Size", MessageBoxButtons.YesNo)
            If result = DialogResult.No Then
                Exit Sub
            ElseIf result = DialogResult.Yes Then
                Dim expression As String
                expression = "SizeCount = " & ComboBox4.Text & " AND VersionNumber=" & VersionCmbBox.Text
                Dim foundRows() As DataRow
                foundRows = dsPB.Tables("bottles").Select(expression)
                For Each r As DataRow In foundRows
                    r.Delete()
                Next

                If Not IsNothing(dsSal.Tables("salestxns")) Then
                    Dim expression1 As String
                    expression1 = "PackagingFormat = 1 AND UnitSize = " & ComboBox4.Text & " AND VersionNumber=" & VersionCmbBox.Text
                    Dim foundRows1() As DataRow
                    foundRows1 = dsSal.Tables("salestxns").Select(expression1)
                    For Each r As DataRow In foundRows1
                        r.Delete()
                    Next
                    BindingSource2.Filter = "PackagingFormat =" & ComboBox3.SelectedIndex & " AND UnitSize = " & ComboBox4.Text & " AND VersionNumber=" & VersionCmbBox.Text

                End If

                If ComboBox4.SelectedIndex >= 0 Then ' Ensure an item is selected
                    Dim selectedIndex As Integer = ComboBox4.SelectedIndex
                    ComboBox4.Items.RemoveAt(selectedIndex)

                    If ComboBox4.Items.Count > 0 Then
                        ' Select the last item in the ComboBox
                        ComboBox4.SelectedIndex = ComboBox4.Items.Count - 1
                    Else
                        ' No items left, set SelectedIndex to -1
                        ComboBox4.SelectedIndex = -1
                        ComboBox4.Text = ""
                    End If
                End If

                If ComboBox4.SelectedIndex = -1 Then
                    Label26.Text = 0.00
                    TotalUnitCost.Text = 0.00
                    Label28.Text = 0
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        Try
            If ComboBox3.Text = "Bulk" Then
                'If ComboBox3.Text = "Bulk" Then
                '    For j As Integer = 0 To gridBulkSales.Rows.Count - 1
                '        CalculateSales(j, TextBox8.Text)
                '    Next
                'Else
                '    For j As Integer = 0 To gridBulkSales.Rows.Count - 1
                '        CalculateSales(j, TotalUnitCost.Text)
                '    Next
                'End If
                CalculateBulkSales(gridBulkSales)
            ElseIf ComboBox3.Text = "Bottles" Then
                CalculateBottlingCost()
            ElseIf ComboBox3.Text = "Stick Packs" Then
                CalculateStickPacksCost()
            ElseIf ComboBox3.Text = "Sachets" Then
                CalculateSachetsCost()
            ElseIf ComboBox3.Text = "Bags" Then
                CalculateStandUpBagCost()
            ElseIf ComboBox3.Text = "Blister" Then
                CalculateBlisterCost()
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub SubtotalTextBox_TextChanged(sender As Object, e As EventArgs) Handles TextBox36.TextChanged, TextBox9.TextChanged, txtBlendingFlavourProfileKg.TextChanged
        Try
            Dim sttb As Double
            Dim labCost As Double

            If TextBox9.Text = "." Or String.IsNullOrEmpty(TextBox9.Text) Then
                Exit Sub
            Else
                If Double.TryParse(TextBox36.Text, sttb) Then
                    Dim tt As Double
                    If Not sttb = 0 Then
                        tt = sttb
                    End If
                    tt += (tt * TextBox9.Text) / 100
                    'If Double.TryParse(txtlabcost.Text, labCost) Then
                    '    tt += labCost
                    'End If

                    If FormulaTypeCmbBox.Text = "Powder" And ComboBox3.Text = "Bulk" Then
                        Dim FP As Double
                        Double.TryParse(txtBlendingFlavourProfileKg.Text, FP)
                        tt += FP
                    End If

                    TextBox8.Text = tt.ToString("##,#0.00")

                Else
                    Exit Sub
                End If

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Function CalculateFlavourProfileCost(Optional ctl As Control = Nothing) As Decimal
        Try
            Dim FlavourProfileCost As Double
            Dim PFCost As Double

            Double.TryParse(txtBlendingFlavourProfileServing.Text, FlavourProfileCost)

            If ctl IsNot Nothing Then
                Dim Serving As Double
                Double.TryParse(ctl.Text, Serving)

                PFCost = Serving * FlavourProfileCost

                If Double.IsNaN(FlavourProfile) Then
                    Return 0.0
                Else
                    Return PFCost
                End If
            Else
                Return FlavourProfileCost
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
            Return 0.00
        End Try
    End Function
    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        updateTextbox4()
    End Sub
    Private Sub updateTextbox4()
        Try
            If String.IsNullOrEmpty(CostKiloLabel.Text) Or String.IsNullOrEmpty(TextBox5.Text) Or TextBox5.Text = "." Then
                Exit Sub
            End If
            Dim cpk As Double = CostKiloLabel.Text
            cpk = (TextBox5.Text * cpk) / 100
            TextBox4.Text = cpk.ToString("##,#0.00")
            'CalculateTableting()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Try
            Dim dataView As DataView = New DataView(dsTi.Tables("tableting"), "VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
            If ComboBox2.SelectedIndex = 0 Then
                TextBox2.Text = 2.5
            Else
                TextBox2.Text = 4.0
            End If
            Try
                dataView(0).Item("TypeofCoating") = ComboBox2.Text
            Catch ex As System.Exception
                Exit Sub
            End Try
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        'RemoveHandler TextBox1.TextChanged, AddressOf Encapsulation_TextBox_TextChanged
        Try
            Dim dataView As DataView = New DataView(dsEi.Tables("encapsulation"), "VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
            If ComboBox5.SelectedIndex = 0 Then
                TextBox1.Text = "3.00"
            ElseIf ComboBox5.SelectedIndex = 1 Then
                TextBox1.Text = "3.00"
            ElseIf ComboBox5.SelectedIndex = 2 Then
                TextBox1.Text = "4.00"
            ElseIf ComboBox5.SelectedIndex = 3 Then
                TextBox1.Text = "5.00"
            ElseIf ComboBox5.SelectedIndex = 4 Then
                TextBox1.Text = "5.00"
            ElseIf ComboBox5.SelectedIndex = 5 Then
                TextBox1.Text = "7.00"
            ElseIf ComboBox5.SelectedIndex = 6 Then
                TextBox1.Text = "7.20"
            ElseIf ComboBox5.SelectedIndex = 7 Then
                TextBox1.Text = "6.50"
            ElseIf ComboBox5.SelectedIndex = 8 Then
                TextBox1.Text = "7.00"
            ElseIf ComboBox5.SelectedIndex = 9 Then
                TextBox1.Text = "4.20"
            ElseIf ComboBox5.SelectedIndex = 10 Then
                TextBox1.Text = "3.00"
            ElseIf ComboBox5.SelectedIndex = 11 Then
                TextBox1.Text = "5.00"
            ElseIf ComboBox5.SelectedIndex = 12 Then
                TextBox1.Text = "3.00"
            ElseIf ComboBox5.SelectedIndex = 13 Then
                TextBox1.Text = "5.00"
            ElseIf ComboBox5.SelectedIndex = 14 Then
                TextBox1.Text = "6.00"
            ElseIf ComboBox5.SelectedIndex = 15 Then
                TextBox1.Text = "4.00"
            ElseIf ComboBox5.SelectedIndex = 16 Then
                TextBox1.Text = "8.50"
            End If

            'AddHandler TextBox1.TextChanged, AddressOf Encapsulation_TextBox_TextChanged

            dataView(0).Item("TypeofCapsule") = ComboBox5.Text
            'ComboBox5.DataBindings(0).BindingManagerBase.EndCurrentEdit()
        Catch ex As Exception
            Helper.WriteLog(ex)
            Exit Sub
        End Try
    End Sub
    Private Sub Encapsulation_TextBox_TextChanged(sender As Object, e As EventArgs)

        If TypeOf sender Is TextBox Or TypeOf sender Is ComboBox Then
            Dim dataView As DataView = New DataView(dsEi.Tables("encapsulation"), "VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
            Try
                If dataView.Count > 0 Then
                    If TypeOf sender Is TextBox Then
                        Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)

                        Dim IsInvalidChar = CheckInvalidChar(senderTxtBox, dataView)
                        If IsInvalidChar Then
                            Exit Sub
                        End If

                        dataView(0).Item(senderTxtBox.Tag) = senderTxtBox.Text
                        dataView(0).Item("EnteredDate") = DateTime.Now
                    End If
                    If TypeOf sender Is ComboBox Then
                        Dim senderCmbBox = CType(sender, ComboBox)
                        dataView(0).Item(senderCmbBox.Tag) = senderCmbBox.Text
                        dataView(0).Item("EnteredDate") = DateTime.Now
                    End If
                Else
                    Dim R As DataRow = dsEi.Tables("encapsulation").NewRow()
                    R("FormulaID") = FormulaID.Text
                    R("TypeofCapsule") = ComboBox5.Text
                    R("CapsuleCost") = TextBox1.Text
                    R("EncapsulationCost") = TextBox7.Text
                    R("WastagePercentage") = TextBox11.Text
                    R("LabCost") = txtlabcost.Text
                    R("WastageValue") = TextBox10.Text
                    R("EnteredBy") = Dashboard.UserIDLabel.Text
                    R("EnteredDate") = DateTime.Now
                    R("VersionNumber") = VersionCmbBox.Text
                    dsEi.Tables("encapsulation").Rows.Add(R)
                End If
                'senderTxtBox.DataBindings(0).BindingManagerBase.EndCurrentEdit()
                CalculateEncapsulation()
            Catch ex As Exception
                Helper.WriteLog(ex)
                Exit Sub
            End Try
        End If
    End Sub
    Private Sub StickPack_TextBox_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim dataView As DataView = New DataView(dsPSP.Tables("stickpacks"), "VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "StickPackDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                If TypeOf sender Is TextBox Then
                    Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)

                    Dim IsInvalidChar = CheckInvalidChar(senderTxtBox, dataView)
                    If IsInvalidChar Then
                        Exit Sub
                    End If


                    Dim columnName As String = Convert.ToString(senderTxtBox.Tag)
                    If dataView.Table.Columns(columnName).DataType Is GetType(Int32) Then
                        Dim parsedValue As Integer
                        If Integer.TryParse(senderTxtBox.Text, parsedValue) Then
                            dataView(0)(columnName) = parsedValue
                        Else
                            dataView(0)(columnName) = If(String.IsNullOrEmpty(senderTxtBox.Text), 0, senderTxtBox.Text.Replace(",", "")) ' or handle invalid input appropriately
                        End If
                    Else
                        ' For string or other types
                        dataView(0)(columnName) = senderTxtBox.Text
                    End If
                    'dataView(0).Item(senderTxtBox.Tag) = senderTxtBox.Text
                End If
                If TypeOf sender Is ComboBox Then
                    Dim senderTxtBox As ComboBox = DirectCast(sender, ComboBox)
                    dataView(0).Item(senderTxtBox.Tag) = senderTxtBox.Text
                ElseIf TypeOf sender Is GroupBox Then
                    Dim grp As GroupBox = DirectCast(sender, GroupBox)
                    For Each ctrl As Control In grp.Controls
                        If TypeOf ctrl Is TextBox Then
                            Dim txtBox As TextBox = DirectCast(ctrl, TextBox)
                            If txtBox.Tag IsNot Nothing Then
                                dataView(0).Item(txtBox.Tag) = txtBox.Text
                            End If
                        End If
                    Next
                End If
            Else
                Dim R As DataRow = dsPSP.Tables("stickpacks").NewRow()
                R("FormulaID") = FormulaID.Text
                R("SizeCount") = ComboBox6.Text
                R("Material") = ComboBox7.Text
                R("Quantity") = ComboBox8.Text.Replace(",", "")
                R("VersionNumber") = VersionCmbBox.Text
                R("Colors") = cmbstickpanelColors.Text
                R("PacketContents") = TextBox16.Text

                R("FillingCost") = TextBox18.Text
                R("PrintingCost") = TextBox19.Text
                R("PrintingPlates") = TextBox21.Text
                R("ArtPreparation") = TextBox22.Text
                R("PackOut") = TextBox23.Text
                R("OtherCostValue") = TextBox24.Text
                R("OtherCostDesc") = TextBox25.Text
                R("WaferSeal") = txtWaferSeal.Text
                R("ShrinkWrap") = txtShrinkWrapStickPack.Text
                'Changes by Payal P
                R("StickPacks") = If(String.IsNullOrEmpty(txtStickPacks.Text), 0, txtStickPacks.Text)
                'Display Box
                R("DisplayQuantity") = If(TextBox38DisplayQty.Text.Replace(",", "") = "", 0, Convert.ToInt32(TextBox38DisplayQty.Text.Replace(",", "")))
                R("DisplayCost") = TextBox20.Text
                R("DisplayDescription") = If(TextBox38DisplayDesc.Text = "", "", TextBox38DisplayDesc.Text)
                R("DisplaySpec") = If(TextBox43DisplaySpec.Text = "", "", TextBox43DisplaySpec.Text)
                R("DisplayBoardGrade") = If(TextBox42DisplayBoardGrade.Text = "", ".018 SBS C1S", TextBox42DisplayBoardGrade.Text)
                R("DisplayDimension") = If(TextBox41DisplayDimension.Text = "", "", TextBox41DisplayDimension.Text)
                R("DisplayProductStyle") = If(TextBox40DisplayProductStyle.Text = "", "TT AGB-T Style", TextBox40DisplayProductStyle.Text)
                'BULK
                R("PrintingPlatesBulk") = If(txtStickPrintingPlateBulk.Text = "", 0, Convert.ToDouble(txtStickPrintingPlateBulk.Text))
                R("ArtPreparationBulk") = If(txtStickArtPrepBulk.Text = "", 0, Convert.ToDouble(txtStickArtPrepBulk.Text))
                R("OtherCostBulk") = If(txtStickOtherCostBulk.Text = "", 0, Convert.ToDouble(txtStickOtherCostBulk.Text))
                R("OtherCostDescBulk") = If(txtStickOtherDescBulk.Text = "", "", txtStickOtherDescBulk.Text)
                R("ShipperCaseCountBulk") = If(txtStickShipperCountBulk.Text = "", "", txtStickShipperCountBulk.Text)
                R("ShipperCaseCostBulk") = If(txtStickShipperCostBulk.Text = "", 0, Convert.ToDouble(txtStickShipperCostBulk.Text))

                R("Research_DevelopmentBulk") = If(txtStickpackBulkRD.Text = "", 0.00, Convert.ToDouble(txtStickpackBulkRD.Text))
                R("Research_DevelopmentBox") = If(txtStickpackBoxRD.Text = "", 0.00, Convert.ToDouble(txtStickpackBoxRD.Text))

                dsPSP.Tables("stickpacks").Rows.Add(R)

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
            Exit Sub
        End Try

    End Sub
    Private Sub Sachets_TextBox_TextChanged(sender As Object, e As EventArgs)

        Try
            Dim dataView As DataView
            dataView = New DataView(dsPSa.Tables("sachets"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "StickPackDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                If TypeOf sender Is TextBox Then
                    Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)
                    Dim columnName As String = Convert.ToString(senderTxtBox.Tag)

                    Dim IsInvalidChar = CheckInvalidChar(senderTxtBox, dataView)
                    If IsInvalidChar Then
                        Exit Sub
                    End If

                    If dataView.Table.Columns(columnName).DataType Is GetType(Int32) Then
                        Dim parsedValue As Integer
                        If Integer.TryParse(senderTxtBox.Text, parsedValue) Then
                            dataView(0)(columnName) = parsedValue
                        Else
                            dataView(0)(columnName) = If(String.IsNullOrEmpty(senderTxtBox.Text), "0", senderTxtBox.Text.Replace(",", ""))  ' or handle invalid input appropriately
                        End If
                    Else
                        ' For string or other types
                        dataView(0)(columnName) = senderTxtBox.Text
                    End If
                ElseIf TypeOf sender Is ComboBox Then
                    Dim senderCmb As ComboBox = DirectCast(sender, ComboBox)
                    dataView(0).Item(senderCmb.Tag) = senderCmb.Text
                ElseIf TypeOf sender Is GroupBox Then
                    Dim grp As GroupBox = DirectCast(sender, GroupBox)
                    For Each ctrl As Control In grp.Controls
                        If TypeOf ctrl Is TextBox Then
                            Dim txtBox As TextBox = DirectCast(ctrl, TextBox)
                            If txtBox.Tag IsNot Nothing Then
                                dataView(0).Item(txtBox.Tag) = txtBox.Text
                            End If
                        End If
                    Next
                End If
            Else
                Dim R As DataRow = dsPSa.Tables("sachets").NewRow()
                R("FormulaID") = FormulaID.Text
                R("SizeCount") = ComboBox11.Text
                R("Material") = ComboBox10.Text
                R("Quantity") = ComboBox9.Text.Replace(",", "")
                R("Colors") = ComboBox12.Text
                R("PacketContents") = TextBox35.Text

                R("FillingCost") = TextBox33.Text
                R("PrintingCost") = TextBox28.Text
                R("DisplayCost") = TextBox27.Text
                R("PrintingPlates") = TextBox32.Text
                R("ArtPreparation") = TextBox31.Text
                R("PackOut") = TextBox30.Text
                R("OtherCostValue") = TextBox29.Text
                R("OtherCostDesc") = TextBox26.Text
                R("ShrinkWrap") = txtShrinkWrapSachets.Text
                R("WaferSeal") = txtWaferSealSachets.Text
                R("Sachets") = txtStickPacksSachets.Text
                R("VersionNumber") = VersionCmbBox.Text
                'Display Box
                R("DisplayQuantity") = If(TextBox38DisplayQty.Text.Replace(",", "") = "", 0, Convert.ToInt32(TextBox38DisplayQty.Text.Replace(",", "")))
                R("DisplayDescription") = If(TextBox39Desc.Text = "", "", TextBox39Desc.Text)
                R("DisplaySpec") = If(TextBox44Spec.Text = "", "", TextBox44Spec.Text)
                R("DisplayBoardGrade") = If(TextBox43BoardGrade.Text = "", ".018 SBS C1S", TextBox43BoardGrade.Text)
                R("DisplayDimension") = If(TextBox42Dimension.Text = "", "", TextBox42Dimension.Text)
                R("DisplayProductStyle") = If(TextBox41ProductStyle.Text = "", "TT AGB-T Style", TextBox41ProductStyle.Text)
                'BULK
                R("PrintingPlatesBulk") = If(txtSachetPrintingPlatesBulk.Text = "", 0, Convert.ToDouble(txtSachetPrintingPlatesBulk.Text))
                R("ArtPreparationBulk") = If(txtSachetArtPrepBulk.Text = "", 0, Convert.ToDouble(txtSachetArtPrepBulk.Text))
                R("OtherCostDescBulk") = If(txtSachetOtherCostDesc.Text = "", "", txtSachetOtherCostDesc.Text)
                R("OtherCostBulk") = If(txtSachetOtherCost.Text = "", 0, Convert.ToDouble(txtSachetOtherCost.Text))
                R("ShipperCaseCountBulk") = If(txtSachetShipperCountBulk.Text = "", "", txtSachetShipperCountBulk.Text)
                R("ShipperCaseCostBulk") = If(txtSachetShipperCost.Text = "", 0, Convert.ToDouble(txtSachetShipperCost.Text))
                R("Research_Development") = If(txtSachetsBulkRD.Text = "", 0.00, Convert.ToDouble(txtSachetsBulkRD.Text))
                R("Research_DevelopmentBulk") = If(txtSachetsBoxRD.Text = "", 0.00, Convert.ToDouble(txtSachetsBoxRD.Text))
                dsPSa.Tables("sachets").Rows.Add(R)

            End If
            'senderTxtBox.DataBindings(0).BindingManagerBase.EndCurrentEdit()
        Catch ex As System.Exception
            Helper.WriteLog(ex)
            Exit Sub
        End Try

    End Sub
    Private Sub Tableting_TextBox_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim dataView As DataView = New DataView(dsTi.Tables("tableting"), "VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
            If TypeOf sender Is TextBox Or TypeOf sender Is ComboBox Then
                'Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)
                Try
                    If dataView.Count > 0 Then
                        If TypeOf sender Is TextBox Then
                            Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)

                            Dim IsInvalidChar = CheckInvalidChar(senderTxtBox, dataView)
                            If IsInvalidChar Then
                                Exit Sub
                            End If

                            dataView(0).Item(senderTxtBox.Tag) = senderTxtBox.Text
                            dataView(0).Item("EnteredDate") = DateTime.Now
                        End If
                        If TypeOf sender Is ComboBox Then
                            Dim senderCmbBox As ComboBox = DirectCast(sender, ComboBox)
                            dataView(0).Item(senderCmbBox.Tag) = senderCmbBox.Text
                            dataView(0).Item("EnteredDate") = DateTime.Now
                        End If
                    Else
                        Dim R As DataRow = dsTi.Tables("tableting").NewRow()
                        R("FormulaID") = FormulaID.Text
                        R("TypeofCoating") = ComboBox12.Text
                        R("CoatingCost") = TextBox2.Text
                        R("CompressionCost") = TextBox3.Text
                        R("WastagePercentage") = TextBox5.Text
                        R("WastageValue") = TextBox4.Text
                        R("LabCost") = TextBox6.Text
                        R("EnteredBy") = Dashboard.UserIDLabel.Text
                        R("EnteredDate") = DateTime.Now
                        R("VersionNumber") = VersionCmbBox.Text
                        dsTi.Tables("tableting").Rows.Add(R)
                    End If
                    'senderTxtBox.DataBindings(0).BindingManagerBase.EndCurrentEdit()
                    CalculateTableting()
                Catch ex As System.Exception
                    Exit Sub
                End Try
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub Blending_TextBox_TextChanged(sender As Object, e As EventArgs)
        Try
            If TypeOf sender Is TextBox Then
                Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)
                Dim dataView = New DataView(dsBi.Tables("blending"), "VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "BlendingDetailID", DataViewRowState.CurrentRows)
                Try
                    If dataView.Count > 0 Then
                        If TypeOf sender Is TextBox Then
                            Dim IsInvalidChar = CheckInvalidChar(senderTxtBox, dataView)
                            If IsInvalidChar Then
                                Exit Sub
                            End If
                            dataView(0).Item(senderTxtBox.Tag) = If(String.IsNullOrEmpty(senderTxtBox.Text) Or senderTxtBox.Text = ".", "0.00", senderTxtBox.Text)
                            dataView(0).Item("EnteredDate") = DateTime.Now
                        End If
                    Else
                        Dim R As DataRow = dsBi.Tables("blending").NewRow()
                        R("FormulaID") = FormulaID.Text
                        R("BlendingCost") = TextBox12.Text
                        R("WastagePercentage") = TextBox13.Text
                        R("WastageValue") = TextBox14.Text
                        R("LabCost") = TextBox15.Text
                        R("EnteredBy") = Dashboard.UserIDLabel.Text
                        R("EnteredDate") = DateTime.Now
                        If ComboBox3.Text = "Bulk" And FormulaTypeCmbBox.Text = "Powder" Then
                            R("FlavorProfile") = txtBlendingFlavourProfileKg.Text
                        Else
                            R("FlavorProfile") = txtBlendingFlavourProfileServing.Text
                        End If
                        R("VersionNumber") = VersionCmbBox.Text
                        dsBi.Tables("blending").Rows.Add(R)
                    End If
                    'senderTxtBox.DataBindings(0).BindingManagerBase.EndCurrentEdit()
                    CalculateBlending()
                Catch ex As System.Exception

                    Exit Sub
                End Try
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Function CalculateBlending()
        Try
            'define val to pass to subtotal panel
            Dim ct As Double = 0
            'gets the current cost per kg
            Dim cpk As Double = CostKiloLabel.Text
            If cpk <> 0 Then
                ct = cpk

                For Each ctl In BlendingPanel.Controls
                    If TypeOf ctl Is TextBox Then
                        Dim tb As TextBox = ctl

                        If String.IsNullOrEmpty(tb.Text) Or tb.Text = "." Or tb.Tag = "FlavorProfile" Then
                            Continue For
                        End If

                        Dim ss As String = tb.Name
                        If Not String.IsNullOrEmpty(ctl.tag) Then

                            Dim v As Double
                            If Not String.IsNullOrEmpty(tb.Text) Then
                                v = tb.Text
                            End If

                            ct += v
                        Else
                            cpk = (TextBox14.Text * cpk) / 100
                            TextBox13.Text = cpk.ToString("##,#0.00")

                        End If
                    End If
                Next
            End If

            CalculateBlending = ct
            TextBox36.Text = ct.ToString("##,#0.00")

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Function
    Private Sub TextBox14_TextChanged(sender As Object, e As EventArgs) Handles TextBox14.TextChanged
        If TextBox14.Text = "." Then
            Exit Sub
        Else
            UpdateTextbox13()
        End If

    End Sub

    Private Sub UpdateTextbox13()
        Try
            If String.IsNullOrEmpty(CostKiloLabel.Text) Then
                Exit Sub
            End If

            Dim cpk As Double = CostKiloLabel.Text
            If Not String.IsNullOrEmpty(TextBox14.Text) Then
                cpk = (TextBox14.Text * cpk) / 100
                TextBox13.Text = cpk.ToString("##,#0.00")
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Function CalculateEncapsulation()
        Try
            'define val to pass to subtotal panel
            Dim ct As Double = 0
            'gets the current cost per kg
            Dim cpk As Double = CostKiloLabel.Text
            If cpk <> 0 Then
                ct = cpk

                For Each ctl In EncapsulationPanel.Controls
                    If TypeOf ctl Is TextBox Then
                        Dim tb As TextBox = ctl
                        If tb.Tag = "color" Then
                            Continue For
                        End If
                        If Not String.IsNullOrEmpty(ctl.tag) Then
                            Dim v As Double = tb.Text
                            ct += v
                        Else
                            cpk = (TextBox10.Text * cpk) / 100
                            TextBox11.Text = cpk.ToString("##,#0.00")
                        End If
                    End If
                Next
            End If

            CalculateEncapsulation = ct
            TextBox36.Text = ct.ToString("##,#0.00")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Function

    Private Sub TextBox10_TextChanged(sender As Object, e As EventArgs) Handles TextBox10.TextChanged
        UpdateTextbox14()
    End Sub

    Private Sub UpdateTextbox14()
        Try
            If String.IsNullOrEmpty(CostKiloLabel.Text) Or String.IsNullOrEmpty(TextBox10.Text) Or TextBox10.Text = "." Then
                Exit Sub
            End If
            Dim cpk As Double = CostKiloLabel.Text
            cpk = (TextBox10.Text * cpk) / 100


            TextBox11.Text = cpk.ToString("##,#0.00")
            CalculateEncapsulation()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub PritingCostStandardSticks(sender As Object, e As EventArgs) Handles ComboBox6.SelectedIndexChanged, ComboBox8.SelectedIndexChanged
        Try
            Dim dr() As System.Data.DataRow
            If ComboBox6.SelectedIndex <> -1 AndAlso ComboBox8.SelectedIndex <> -1 Then
                Dim qt As Double = ComboBox8.Text.Replace(",", "")
                dr = ds.Tables("stickpackspricing").Select("Size='" & ComboBox6.Text & "' AND Quantity = " & qt & "")
                If dr.Length > 0 Then
                    TextBox19.Text = dr(0)("Cost").ToString()
                End If
            End If
            If TypeOf sender Is ComboBox Then
                Dim senderCmbBox As ComboBox = DirectCast(sender, ComboBox)
                Try
                    Dim dataView As DataView = New DataView(dsPSP.Tables("stickpacks"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "StickPackDetailID", DataViewRowState.CurrentRows)
                    If dataView.Count > 0 Then
                        dataView(0)(senderCmbBox.Tag) = senderCmbBox.Text
                    End If
                    'If dsPSP.Tables("stickpacks").Rows.Count > 0 Then
                    '    dsPSP.Tables("stickpacks").Rows(0).Item(senderCmbBox.Tag) = senderCmbBox.Text
                    'End If
                    'senderCmbBox.DataBindings(0).BindingManagerBase.EndCurrentEdit()
                Catch ex As System.Exception
                    Exit Sub
                End Try
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub PritingCostStandardSachets(sender As Object, e As EventArgs) 'Handles ComboBox11.SelectedIndexChanged, ComboBox10.SelectedIndexChanged, ComboBox9.SelectedIndexChanged, ComboBox12.SelectedIndexChanged
        Try
            Dim dr() As System.Data.DataRow
            Dim number = New Integer() {3, 4, 5, 6}
            If ComboBox11.Text <> "" AndAlso ComboBox10.Text <> "" AndAlso ComboBox9.Text <> "" AndAlso ComboBox12.Text <> "" Then

                Dim qt As Double = ComboBox9.Text.Replace(",", "")
                dr = ds.Tables("xls_sachetlist").Select("Size='" & ComboBox11.Text & "' AND Material = '" & ComboBox10.Text & "' AND QTY = " & qt & "")
                If dr.Length > 0 Then
                    If number.Contains(ComboBox12.Text) Then
                        TextBox28.Text = dr(0)(ComboBox12.Text).ToString()
                    Else
                        TextBox28.Text = "0.00"
                    End If

                ElseIf dr.Length = 0 Then
                    TextBox28.Text = "0.00"
                End If
            Else
                TextBox28.Text = "0.00"
            End If
            If TypeOf sender Is ComboBox Then

                Dim senderCmbBox As ComboBox = DirectCast(sender, ComboBox)

                Dim dataView As DataView = New DataView(dsPSa.Tables("sachets"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
                If dataView.Count > 0 Then
                    dataView(0)(senderCmbBox.Tag) = senderCmbBox.Text
                End If
                'If dsPSa.Tables("sachets").Rows.Count > 0 Then
                '    dsPSa.Tables("sachets").Rows(0).Item(senderCmbBox.Tag) = senderCmbBox.Text
                'End If
                'senderCmbBox.DataBindings(0).BindingManagerBase.EndCurrentEdit()

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub PritingCostStandardStickpaks(sender As Object, e As EventArgs)
        Try
            Dim dr() As System.Data.DataRow
            Dim number = New Integer() {3, 4, 5, 6}
            If ComboBox6.Text <> "" AndAlso ComboBox7.Text <> "" AndAlso ComboBox8.Text <> "" AndAlso cmbstickpanelColors.Text <> "" Then

                dr = ds.Tables("xls_stickpackslist").Select("Size='" & ComboBox6.Text & "' AND Material = '" & ComboBox7.Text & "' AND QTY = " & ComboBox8.Text.Replace(",", "") & "")
                If dr.Length > 0 Then
                    If number.Contains(cmbstickpanelColors.Text) Then
                        TextBox19.Text = dr(0)(cmbstickpanelColors.Text).ToString()
                    Else
                        TextBox19.Text = "0.00"
                    End If
                ElseIf dr.Length = 0 Then
                    TextBox19.Text = "0.00"
                End If
            Else
                TextBox19.Text = "0.00"
            End If
            If TypeOf sender Is ComboBox Then

                Dim senderCmbBox As ComboBox = DirectCast(sender, ComboBox)
                Dim dataView As DataView = New DataView(dsPSP.Tables("stickpacks"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "StickPackDetailID", DataViewRowState.CurrentRows)
                If dataView.Count > 0 Then
                    dataView(0)(senderCmbBox.Tag) = senderCmbBox.Text
                End If
                'If dsPSP.Tables("stickpacks").Rows.Count > 0 Then
                '    dsPSP.Tables("stickpacks").Rows(0).Item(senderCmbBox.Tag) = senderCmbBox.Text
                'End If
                'senderCmbBox.DataBindings(0).BindingManagerBase.EndCurrentEdit()

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub BlisterUpdate(sender As Object, e As EventArgs)
        Try
            Dim dataview As DataView = New DataView(dsPBlisters.Tables("blisters"), "VersionNumber=" & VersionCmbBox.Text, "BlisterDetailsID", DataViewRowState.CurrentRows)
            If dataview.Count > 0 Then
                If TypeOf sender Is TextBox Then
                    Dim senderTxtBox As TextBox = DirectCast(sender, TextBox)
                    Dim columnName As String = Convert.ToString(senderTxtBox.Tag)

                    Dim IsInvalidChar = CheckInvalidChar(senderTxtBox, dataview)
                    If IsInvalidChar Then
                        Exit Sub
                    End If

                    If dataview.Table.Columns(columnName).DataType Is GetType(Int32) Then
                        Dim parsedValue As Integer
                        If Integer.TryParse(senderTxtBox.Text, parsedValue) Then
                            dataview(0)(columnName) = parsedValue
                        Else
                            dataview(0)(columnName) = If(String.IsNullOrEmpty(senderTxtBox.Text), "0", senderTxtBox.Text.Replace(",", ""))  ' or handle invalid input appropriately
                        End If
                    Else
                        ' For string or other types
                        dataview(0)(columnName) = senderTxtBox.Text
                    End If
                    'dataview(0).Item(senderTxtBox.Tag) = senderTxtBox.Text
                ElseIf TypeOf sender Is ComboBox Then
                    Dim SenderComboBox As ComboBox = DirectCast(sender, ComboBox)
                    dataview(0).Item(SenderComboBox.Tag) = If(SenderComboBox.Text = "Yes", 1, 0)
                    If SenderComboBox.Tag.ToString() = "BlisterType" Then
                        dataview(0).Item(SenderComboBox.Tag) = If(SenderComboBox.Text = "Capsule(s)", "Capsule(s)", "Tablets(s)")
                    End If
                    If SenderComboBox.Tag.ToString() = "Material" Then
                        dataview(0).Item(SenderComboBox.Tag) = txtboxBlisterMaterial.Text
                    End If
                    If SenderComboBox.Tag.ToString() = "ShrinkWrapFlag" Then
                        dataview(0).Item(SenderComboBox.Tag) = If(SenderComboBox.Text.ToLower() = "yes", 1, 0)
                    End If
                    If SenderComboBox.Tag.ToString() = "WaferSealsFlag" Then
                        dataview(0).Item(SenderComboBox.Tag) = If(SenderComboBox.Text.ToLower() = "yes", 1, 0)
                    End If
                ElseIf TypeOf sender Is GroupBox Then
                    Dim grp As GroupBox = DirectCast(sender, GroupBox)
                    For Each ctrl As Control In grp.Controls
                        If TypeOf ctrl Is TextBox Then
                            Dim txtBox As TextBox = DirectCast(ctrl, TextBox)
                            If txtBox.Tag IsNot Nothing Then
                                dataview(0).Item(txtBox.Tag) = txtBox.Text
                            End If
                        End If
                    Next
                End If
            Else

                Dim row As DataRow = dsPBlisters.Tables("blisters").NewRow()
                row("FormulaID") = Convert.ToInt32(FormulaID.Text)
                row("BlisterFormat") = txtBlisterFormat.Text
                row("BlisterSize") = If(txtBlisterSize.Text = "", 0, txtBlisterSize.Text)
                row("Material") = txtboxBlisterMaterial.Text
                row("Quantity") = If(txtBlisterQty.Text = "", 0, Convert.ToInt32(txtBlisterQty.Text))
                row("BlisterCount") = If(txtBlisterCount.Text = "", 0, Convert.ToInt32(txtBlisterCount.Text))
                row("BlisterType") = If(cmbBlisterCountType.Text = "Capsule(s)", "Capsule(s)", "Tablets(s)")
                row("BlisterAmount") = If(txtBlisterCountAmount.Text = "", 0, Double.Parse(txtBlisterCountAmount.Text))
                row("BlisterCost") = If(txtBlisterCost.Text = "", 0, Double.Parse(txtBlisterCost.Text))
                row("PrintingCost") = If(txtBlisterPrintingCostAmt.Text = "", 0, Double.Parse(txtBlisterPrintingCostAmt.Text))
                'row("Quantity") = Convert.ToInt32(txtBlisterQty.Text) 
                row("FoldingCartonCost") = If(txtBlisterBoxCostAmt.Text = "", "0.00", Double.Parse(txtBlisterBoxCostAmt.Text))
                row("PrintingPlatesCost") = If(txtBlisterPrintingPlatesAmt.Text = "", 0, Double.Parse(txtBlisterPrintingPlatesAmt.Text))
                row("PrintingPlatesCostBulk") = If(txtPrintingBulk.Text = "", 0, Double.Parse(txtPrintingBulk.Text))
                row("ShrinkWrapFlag") = If(cmbBlisterShrinkWrap.Text = "Yes", 1, 0)
                row("WaferSealsFlag") = If(cmbBlisterWaferSeal.Text = "Yes", 1, 0)
                row("SecondaryPackoutCost") = If(txtBlisterPackoutAmt.Text = "", 0, Double.Parse(txtBlisterPackoutAmt.Text))
                row("ShrinkWrap") = If(txtBlisterShrinkWrap.Text = "", 0, Double.Parse(txtBlisterShrinkWrap.Text))
                row("WaferSealsCost") = If(txtBlisterWaferSeal.Text = "", 0, Double.Parse(txtBlisterWaferSeal.Text))
                row("ArtPreparation") = If(txtBlisterArtPreperationAmt.Text = "", 0, Double.Parse(txtBlisterArtPreperationAmt.Text))
                row("ArtPreparationBulk") = If(txtArtPrepBulk.Text = "", 0, Double.Parse(txtArtPrepBulk.Text))
                row("OtherCost") = If(txtBlisterOtherAmt.Text = "", 0, Double.Parse(txtBlisterOtherAmt.Text))
                row("OtherCostBulk") = If(txtOtherCostBulk.Text = "", 0, Double.Parse(txtOtherCostBulk.Text))
                row("Other_Description") = txtOtherDescBlister.Text
                row("Other_DescriptionBulk") = txtOtherBulk.Text

                row("ShipperCaseCount") = If(txtBlisterCaseCount.Text = "", 0, txtBlisterCaseCount.Text)
                row("ShipperCaseCountBulk") = If(txtShipperCountBulk.Text = "", 0, txtShipperCountBulk.Text)
                row("ShipperCaseCost") = If(txtBlisterCaseCountAmt.Text = "", 0, Double.Parse(txtBlisterCaseCountAmt.Text))
                row("ShipperCaseCost") = If(txtShipperCostBulk.Text = "", 0, Double.Parse(txtShipperCostBulk.Text))
                row("VersionNumber") = If(VersionCmbBox.Text = "", 0, Convert.ToInt32(VersionCmbBox.Text))
                'Chnages by Payal P
                row("ToolingCost") = If(txttoolcostamtblister.Text = "", 0, Double.Parse(txttoolcostamtblister.Text))
                row("ToolingCostBulk") = If(txtToolingCostBulk.Text = "", 0, Double.Parse(txtToolingCostBulk.Text))
                'Display Box
                row("Quantity_DisplayBox") = If(txtQtyDiplaybox.Text.Replace(",", "") = "", 0, Convert.ToInt32(txtQtyDiplaybox.Text.Replace(",", "")))
                row("Amount_DisplayBox") = If(txtAmontDisplaybox.Text = "", 0, Double.Parse(txtAmontDisplaybox.Text))
                row("Description_DisplayBox") = If(txtDescDiplaybox.Text = "", "", txtDescDiplaybox.Text)
                row("Spec_DisplayBox") = If(txtSepcDisplaybox.Text = "", "", txtSepcDisplaybox.Text)
                row("BoardGrade_DisplayBox") = If(txtBoradGradeDiplaybox.Text = "", ".018 SBS C1S", txtBoradGradeDiplaybox.Text)
                row("Dimension_DisplayBox") = If(txtDimensionDisplaybox.Text = "", "", txtDimensionDisplaybox.Text)
                row("ProductStyle_DisplayBox") = If(txtProductStyle.Text = "", "TT AGB-T Style", txtProductStyle.Text)
                row("Research_DevelopmentBulk") = If(txtBlisterBulkRD.Text = "", 0.00, Convert.ToDouble(txtBlisterBulkRD.Text))
                row("Research_Development") = If(txtBlisterBoxRD.Text = "", 0.00, Convert.ToDouble(txtBlisterBoxRD.Text))
                dsPBlisters.Tables("blisters").Rows.Add(row)
            End If


        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub Saves(Optional isCopy As Boolean = False, Optional isFromLoad As Boolean = False)
        Try

            'Perform calculation before save
            CalculateBulkSales(gridBulkSales)
            If chkDisplayBag.Checked Then
                CalculateBulkSales(gridbBulkSalesBags)
            ElseIf chkDisplayBox.Checked Then
                CalculateBulkSales(gridBoxSales)
            End If

            'Updates Formula Table

            Dim dataViewF As DataView = New DataView(dsFF.Tables("formulaF"), "", "", DataViewRowState.CurrentRows)
            dataViewF(0).Item("ServingSize") = If(String.IsNullOrEmpty(ServingSizeTextBox.Text), "", ServingSizeTextBox.Text)
            dataViewF(0).Item("CustomerName") = If(String.IsNullOrEmpty(txtCustName.Text), "", txtCustName.Text)
            dataViewF(0).Item("Contact") = If(String.IsNullOrEmpty(txtContact.Text), "", txtContact.Text)
            dataViewF(0).Item("SalesRepId") = If(String.IsNullOrEmpty(txtSalesRepId.Text), "", txtSalesRepId.Text)

            Dim IsNew As Boolean = False

            Combobox3Index = ComboBox3.SelectedIndex

#Region "Delete Box Sales Data If any When Check Box Is Not clicked"
            'If chkDisplayBox.Checked = False Then
            '    DeleteSales(Combobox3Index, VersionCmbBox.Text, "BOX")

            '    Dim boxData As DataView = New DataView(dsPSP.Tables("stickpacks"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "StickPackDetailID", DataViewRowState.CurrentRows)
            '    If boxData.Count > 0 Then
            '        boxData(0)("DisplayQuantity") = 0
            '        boxData(0)("DisplayDescription") = ""
            '        boxData(0)("DisplaySpec") = ""
            '        boxData(0)("DisplayBoardGrade") = ".018 SBS C1S"
            '        boxData(0)("DisplayDimension") = ""
            '        boxData(0)("DisplayProductStyle") = "TT AGB-T Style"
            '    End If

            '    boxData = New DataView(dsPSa.Tables("sachets"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "", DataViewRowState.CurrentRows)
            '    If boxData.Count > 0 Then
            '        boxData(0)("DisplayQuantity") = 0
            '        boxData(0)("DisplayDescription") = ""
            '        boxData(0)("DisplaySpec") = ""
            '        boxData(0)("DisplayBoardGrade") = ".018 SBS C1S"
            '        boxData(0)("DisplayDimension") = ""
            '        boxData(0)("DisplayProductStyle") = "TT AGB-T Style"
            '    End If
            '    boxData = New DataView(dsPBlisters.Tables("blisters"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), 0, VersionCmbBox.Text), "BlisterDetailsID", DataViewRowState.CurrentRows)
            '    If boxData.Count > 0 Then
            '        boxData(0)("Quantity_DisplayBox") = 0
            '        boxData(0)("Amount_DisplayBox") = "0.00"
            '        boxData(0)("Description_DisplayBox") = ""
            '        boxData(0)("Spec_DisplayBox") = ""
            '        boxData(0)("BoardGrade_DisplayBox") = ".018 SBS C1S"
            '        boxData(0)("Dimension_DisplayBox") = ""
            '        boxData(0)("ProductStyle_DisplayBox") = "TT AGB-T Style"
            '    End If

            'End If
#End Region
            Dim FVersions As DataView = New DataView(dsFV.Tables("versions"), "", "", DataViewRowState.CurrentRows)

            For Each rows As DataRowView In FVersions
                Select Case rows.Item("PackagingType").ToString()
                    Case "Bulk"
                        DeleteSachets(rows.Item("VersionNumber").ToString())
                        DeleteStickPacks(rows.Item("VersionNumber").ToString())
                        DeleteBags(rows.Item("VersionNumber").ToString())
                        DeleteBlister(rows.Item("VersionNumber").ToString())
                        DeleteBottle(rows.Item("VersionNumber").ToString())
                        DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString())
                    Case "Bottles"
                        DeleteSachets(rows.Item("VersionNumber").ToString())
                        DeleteStickPacks(rows.Item("VersionNumber").ToString())
                        DeleteBags(rows.Item("VersionNumber").ToString())
                        DeleteBlister(rows.Item("VersionNumber").ToString())
                        DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString())
                        'DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString(), "BOX")
                    Case "Sachets"
                        DeleteBottle(rows.Item("VersionNumber").ToString())
                        DeleteStickPacks(rows.Item("VersionNumber").ToString())
                        DeleteBags(rows.Item("VersionNumber").ToString(), "Sachets")
                        DeleteBlister(rows.Item("VersionNumber").ToString())
                        DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString())
                        'DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString(), "BOX")
                    Case "Stick Packs"
                        DeleteSachets(rows.Item("VersionNumber").ToString())
                        DeleteBottle(rows.Item("VersionNumber").ToString())
                        DeleteBags(rows.Item("VersionNumber").ToString(), "Stick Packs")
                        DeleteBlister(rows.Item("VersionNumber").ToString())
                        DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString())
                        'DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString(), "BOX")
                    Case "Bags"
                        DeleteSachets(rows.Item("VersionNumber").ToString())
                        DeleteStickPacks(rows.Item("VersionNumber").ToString())
                        DeleteBottle(rows.Item("VersionNumber").ToString())
                        DeleteBlister(rows.Item("VersionNumber").ToString())
                        DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString())
                        'DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString(), "BOX")
                    Case "Blister"
                        DeleteSachets(rows.Item("VersionNumber").ToString())
                        DeleteStickPacks(rows.Item("VersionNumber").ToString())
                        DeleteBags(rows.Item("VersionNumber").ToString())
                        DeleteBottle(rows.Item("VersionNumber").ToString())
                        DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString())
                        'DeleteSales(Combobox3Index, rows.Item("VersionNumber").ToString(), "BOX")
                End Select
            Next

            Select Case FormulaTypeCmbBox.Text
                Case "Powder"
                    If dsBi.Tables("blending").Rows.Count > 0 Then
                        DeleteEncapsulation(VersionCmbBox.Text)
                        DeleteTableting(VersionCmbBox.Text)
                    End If
                Case "Capsule"
                    If dsEi.Tables("encapsulation").Rows.Count > 0 Then
                        DeleteBlending(VersionCmbBox.Text)
                        DeleteTableting(VersionCmbBox.Text)
                    End If
                Case "Tablet"
                    If dsTi.Tables("tableting").Rows.Count > 0 Then
                        DeleteBlending(VersionCmbBox.Text)
                        DeleteEncapsulation(VersionCmbBox.Text)
                    End If
            End Select


            If FormulaTypeCmbBox.SelectedIndex = -1 Then
                FormulaTypeCmbBox.DroppedDown = True
                Exit Sub
            End If



            'Check if it is a new formula to save
            If FormulaID.Text = -1 Then


                FormulaName.Label1.Text = 0
                FormulaName.Label2.Text = Me.Name
                FormulaName.Label3.Text = FormulaTypeCmbBox.Text
                FormulaName.Label4.Text = ComboBox3.SelectedIndex
                FormulaName.Label5.Text = ServingSizeTextBox.Text
                FormulaName.lblCustName.Text = txtCustName.Text
                FormulaName.lblContact.Text = txtContact.Text
                FormulaName.lblSalesRep.Text = txtSalesRepId.Text
                Dim sttb As Double
                If Double.TryParse(ServingWeightLabel.Text, sttb) Then
                    FormulaName.Label6.Text = sttb
                End If


                FormulaName.ShowDialog()
                'if for any reason user cancels saving
                If ContinueSaving = False And IsSaved = False Then
                    lblQuoteNumber.Text = "NA"
                    Exit Sub
                End If



                IsNew = True
            End If

            For Each r As DataRow In dsF.Tables("formula").Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each r As DataRow In dsBi.Tables(0).Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each r As DataRow In dsFV.Tables(0).Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each r As DataRow In dsTi.Tables(0).Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each r As DataRow In dsEi.Tables(0).Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each r As DataRow In dsPB.Tables(0).Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each r As DataRow In dsPSa.Tables(0).Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each r As DataRow In dsPSP.Tables(0).Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each r As DataRow In dsPSa.Tables(0).Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each row As DataRow In dsStandupBags.Tables(0).Rows
                If row.RowState <> DataRowState.Deleted Then
                    row.Item("FormulaID") = FormulaID.Text
                End If
            Next
            For Each r As DataRow In dsPBlisters.Tables(0).Rows
                If r.RowState <> DataRowState.Deleted Then
                    r.Item("FormulaID") = FormulaID.Text
                End If
            Next
            'Saves blending part
            If dsBi.HasChanges = True Then
                daBi_commands()
            End If


            'Saves tableting part
            If dsTi.HasChanges = True Then
                daTi_commands()
            End If


            'Saves encapsulation part
            If dsEi.HasChanges = True Then
                daEi_commands()
            End If


            'Saves bottles part
            If dsPB.HasChanges = True Then
                daPB_commands()
            End If


            'Saves sachets part
            If dsPSa.HasChanges = True Then
                daPSa_commands()
            End If

            'Saves blisters part
            If dsPBlisters.HasChanges = True Then
                daPBlisters_commands()
            End If

            'saves sales
            If dsSal.HasChanges = True Then
                dasal_commands()
            End If

            'saves Bag And Box Sales Grid

            If dsSalBox.HasChanges = True Then
                dasalBox_commands()
            End If
            If dsSalBags.HasChanges = True Then
                dasalBag_commands()
            End If

            'Saves formula details
            If dsF.HasChanges = True Then
                daF_commands()
            End If
            'Saves stick pack part
            If dsPSP.HasChanges = True Then
                daPSP_commands()
            End If
            'Saves Standup Bag part
            If dsStandupBags.HasChanges = True Then
                dsStandupBags_commands()
            End If

            'Saves Version info
            If dsFV.HasChanges = True Then
                dsFV_commands()
            End If

            'Saves formula info
            If dsFF.HasChanges = True Then
                dsFF_commands()
            End If

            Thread.Sleep(1000)
            If Not isFromLoad Then
                If Not FormulaID.Text = -1 Then
                    If IsNew = True Then
                        MsgBox("Formula Saved Successfully", MessageBoxButtons.OK, "Message")
                        FormulaTypeCmbBox.Enabled = False
                    Else
                        If isCopy = False Then
                            MsgBox("Formula Updated Successfully", MessageBoxButtons.OK, "Message")
                        End If
                    End If
                End If

                PopulateFormulator2()
                BindControls()
            End If

            Select Case ComboBox3.Text
                Case "Bulk"
                    DisplaySachets(VersionCmbBox.Text)
                    DisplayStickpacks(VersionCmbBox.Text)
                    DisplayStandupBag(VersionCmbBox.Text)
                    DisplayBlister(VersionCmbBox.Text)
                Case "Bottles"
                    DisplaySachets(VersionCmbBox.Text)
                    DisplayStickpacks(VersionCmbBox.Text)
                    DisplayStandupBag(VersionCmbBox.Text)
                    DisplayBlister(VersionCmbBox.Text)
                    DisplayBottling()
                    CalculateBottlingCost()
                Case "Sachets"
                    DisplayStickpacks(VersionCmbBox.Text)
                    DisplayStandupBag(VersionCmbBox.Text)
                    DisplayBlister(VersionCmbBox.Text)
                    DisplaySachets(VersionCmbBox.Text)
                    CalculateSachetsCost()
                Case "Stick Packs"
                    DisplaySachets(VersionCmbBox.Text)
                    DisplayStandupBag(VersionCmbBox.Text)
                    DisplayBlister(VersionCmbBox.Text)
                    DisplayStickpacks(VersionCmbBox.Text)
                    CalculateStickPacksCost()
                Case "Bags"
                    DisplaySachets(VersionCmbBox.Text)
                    DisplayStickpacks(VersionCmbBox.Text)
                    DisplayBlister(VersionCmbBox.Text)
                    DisplayStandupBag(VersionCmbBox.Text)
                    CalculateStandUpBagCost()
                Case "Blister"
                    DisplaySachets(VersionCmbBox.Text)
                    DisplayStickpacks(VersionCmbBox.Text)
                    DisplayStandupBag(VersionCmbBox.Text)
                    DisplayBlister(VersionCmbBox.Text)
                    CalculateBlisterCost()
            End Select
            DisplayFormula()
            Select Case FormulaTypeCmbBox.Text
                Case "Powder"
                    DisplayBlending(VersionCmbBox.Text)
                Case "Capsule"
                    DisplayEncapsulation(VersionCmbBox.Text)
                Case "Tablet"
                    DisplayTableting(VersionCmbBox.Text)
            End Select

            If ComboBox3.Text = "Bottles" Then
                SetPackagingType()
            End If

            'Unsaves unwanted blending part
            If dsBi.HasChanges = True Then
                dsBi.RejectChanges()
            End If

            'Unsaves unwanted tableting part
            If dsTi.HasChanges = True Then
                dsTi.RejectChanges()
            End If

            'Unsaves unwanted encapsulation part
            If dsEi.HasChanges = True Then
                dsEi.RejectChanges()
            End If

            'Unsaves unwanted bottles part
            If dsPB.HasChanges = True Then
                dsPB.RejectChanges()
            End If

            'Unsaves unwanted sachets part
            If dsPSa.HasChanges = True Then
                dsPSa.RejectChanges()
            End If

            'Unsaves unwanted blisters part
            If dsPBlisters.HasChanges = True Then
                dsPBlisters.RejectChanges()
            End If

            'Unsaves unwanted sales
            If dsSal.HasChanges = True Then
                dsSal.RejectChanges()
            End If

            'Unsaves unwanted formula details
            If dsF.HasChanges = True Then
                dsF.RejectChanges()
            End If

            'Unsaves unwanted stick pack part
            If dsPSP.HasChanges = True Then
                dsPSP.RejectChanges()
            End If

            'Unsaves unwanted Standup Bag part
            If dsStandupBags.HasChanges = True Then
                dsStandupBags.RejectChanges()
            End If

            'Unsaves unwanted Version info
            If dsFV.HasChanges = True Then
                dsFV.RejectChanges()
            End If

            'Unsaves unwanted formula info
            If dsFF.HasChanges = True Then
                dsFF.RejectChanges()
            End If

            'If ComboBox3.Text = "Bulk" Then
            '    For j As Integer = 0 To DataGridView3.Rows.Count - 1
            '        CalculateSales(j, Temp_TextBox8)
            '    Next
            'Else
            '    For j As Integer = 0 To DataGridView3.Rows.Count - 1
            '        CalculateSales(j, Temp_ToatlUnitCost)
            '    Next
            'End If
            'Dim DataView = dsSal.Tables(0)
            ' BindControls()
            'For j As Integer = 0 To DataGridView1.Rows.Count - 1
            '    DisplaySelectedCostValue(j)
            '    DisplayCostType(j)
            '    DisplayCostColor(j)
            'Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub btnSaveFormula_Click(sender As Object, e As EventArgs) Handles btnSaveFormula.Click
        Saves()
    End Sub
#Region " All About Saving"

    Public Sub dasal_commands()
        Dim SelectCommand As New MySqlCommand("SELECT * FROM SalesDetails WHERE Type = 'BULK' AND FormulaID=" & FormulaID.Text, DbCon)
        daSal.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO SalesDetails
        (FormulaID
        , PackagingFormat
        , UnitSize
        , Quantity
        , MarginPercentage
        , SalesPrice
        , VersionNumber
        , Type
        , OverriddenSalesPrice
        , Overridden) 
        VALUES
        (" & FormulaID.Text & "
        , @PackagingFormat        
        , @UnitSize 
        , @Quantity
        , @MarginPercentage
        , @SalesPrice
        , @VersionNumber
        , @Type
        , @OverriddenSalesPrice
        , @Overridden) ", DbCon)

        InsertCommand.Parameters.Add("@PackagingFormat", MySqlDbType.Int32, 10, "PackagingFormat")
        InsertCommand.Parameters.Add("@UnitSize", MySqlDbType.Decimal, 5, "UnitSize")
        InsertCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 11, "Quantity")
        InsertCommand.Parameters.Add("@MarginPercentage", MySqlDbType.Decimal, 4, "MarginPercentage")
        InsertCommand.Parameters.Add("@SalesPrice", MySqlDbType.Decimal, 11, "SalesPrice")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        InsertCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 20, "Type")
        InsertCommand.Parameters.Add("@OverriddenSalesPrice", MySqlDbType.Decimal, 8, "OverriddenSalesPrice")
        InsertCommand.Parameters.Add("@Overridden", MySqlDbType.Int32, 1, "Overridden")
        daSal.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE SalesDetails
        SET UnitSize = @UnitSize
        , Quantity = @Quantity 
        , MarginPercentage = @MarginPercentage
        , SalesPrice = @SalesPrice
		, VersionNumber=@VersionNumber, Type=@Type
        , OverriddenSalesPrice=@OverriddenSalesPrice ,Overridden=@Overridden 
        WHERE SalesDetailID = @SalesDetailID", DbCon)


        Dim p As MySqlParameter = UpdateCommand.Parameters.Add("@SalesDetailID", MySqlDbType.Int32, 10, "SalesDetailID")
        p.SourceVersion = DataRowVersion.Original
        'UpdateCommand.Parameters.Add("@SalesDetailID", MySqlDbType.Int32, 10, "SalesDetailID")
        UpdateCommand.Parameters.Add("@UnitSize", MySqlDbType.Decimal, 5, "UnitSize")
        UpdateCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 11, "Quantity")
        UpdateCommand.Parameters.Add("@MarginPercentage", MySqlDbType.Decimal, 4, "MarginPercentage")
        UpdateCommand.Parameters.Add("@SalesPrice", MySqlDbType.Decimal, 11, "SalesPrice")
        'UpdateCommand.Parameters.Add("@SalesDetailID", MySqlDbType.Int32, 10, "SalesDetailID")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        UpdateCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 20, "Type")
        UpdateCommand.Parameters.Add("@OverriddenSalesPrice", MySqlDbType.Decimal, 8, "OverriddenSalesPrice")
        UpdateCommand.Parameters.Add("@Overridden", MySqlDbType.Int32, 1, "Overridden")
        daSal.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM SalesDetails
        WHERE SalesDetailID = @SalesDetailID", DbCon)

        DeleteCommand.Parameters.Add("@SalesDetailID", MySqlDbType.Int32, 10, "SalesDetailID")
        daSal.DeleteCommand = DeleteCommand

        Dim bs As BindingSource = DirectCast(gridBulkSales.DataSource, BindingSource)
        bs.EndEdit()
        BindingSource2.EndEdit()
        Try
            bs.RaiseListChangedEvents = False
            daSal.Update(dsSal, "salestxns")
            bs.RaiseListChangedEvents = True
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

        dsSal.AcceptChanges()
    End Sub

    Public Sub dasalBag_commands()

        Dim SelectCommand As New MySqlCommand("SELECT * FROM SalesDetails WHERE  Type = 'BULKBAGS' AND  FormulaID=" & FormulaID.Text, DbCon)
        daSalBags.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO SalesDetails
        (FormulaID
        , PackagingFormat
        , UnitSize
        , Quantity
        , MarginPercentage
        , SalesPrice
        , VersionNumber
        , Type
        , OverriddenSalesPrice
        , Overridden) 
        VALUES
        (" & FormulaID.Text & "
        , @PackagingFormat        
        , @UnitSize 
        , @Quantity
        , @MarginPercentage
        , @SalesPrice
        , @VersionNumber
        , @Type
        , @OverriddenSalesPrice
        , @Overridden) ", DbCon)

        InsertCommand.Parameters.Add("@PackagingFormat", MySqlDbType.Int32, 10, "PackagingFormat")
        InsertCommand.Parameters.Add("@UnitSize", MySqlDbType.Decimal, 5, "UnitSize")
        InsertCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 11, "Quantity")
        InsertCommand.Parameters.Add("@MarginPercentage", MySqlDbType.Decimal, 4, "MarginPercentage")
        InsertCommand.Parameters.Add("@SalesPrice", MySqlDbType.Decimal, 11, "SalesPrice")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        InsertCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 20, "Type")
        InsertCommand.Parameters.Add("@OverriddenSalesPrice", MySqlDbType.Decimal, 8, "OverriddenSalesPrice")
        InsertCommand.Parameters.Add("@Overridden", MySqlDbType.Int32, 1, "Overridden")
        daSalBags.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE SalesDetails
        SET UnitSize = @UnitSize
        , Quantity = @Quantity 
        , MarginPercentage = @MarginPercentage
        , SalesPrice = @SalesPrice
		, VersionNumber=@VersionNumber, Type=@Type
        , OverriddenSalesPrice=@OverriddenSalesPrice ,Overridden=@Overridden 
        WHERE SalesDetailID = @SalesDetailID", DbCon)

        UpdateCommand.Parameters.Add("@SalesDetailID", MySqlDbType.Int32, 10, "SalesDetailID")
        UpdateCommand.Parameters.Add("@UnitSize", MySqlDbType.Decimal, 5, "UnitSize")
        UpdateCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 11, "Quantity")
        UpdateCommand.Parameters.Add("@MarginPercentage", MySqlDbType.Decimal, 4, "MarginPercentage")
        UpdateCommand.Parameters.Add("@SalesPrice", MySqlDbType.Decimal, 11, "SalesPrice")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        UpdateCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 20, "Type")
        UpdateCommand.Parameters.Add("@OverriddenSalesPrice", MySqlDbType.Decimal, 8, "OverriddenSalesPrice")
        UpdateCommand.Parameters.Add("@Overridden", MySqlDbType.Int32, 1, "Overridden")
        daSalBags.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM SalesDetails
        WHERE SalesDetailID = @SalesDetailID", DbCon)

        DeleteCommand.Parameters.Add("@SalesDetailID", MySqlDbType.Int32, 10, "SalesDetailID")
        daSalBags.DeleteCommand = DeleteCommand

        Dim bs As BindingSource = DirectCast(gridbBulkSalesBags.DataSource, BindingSource)
        bs.EndEdit()
        BindSalesBulkStickpacksGrid.EndEdit()
        Try
            bs.RaiseListChangedEvents = False

            daSalBags.Update(dsSalBags, "salesBagsTxns")
            bs.RaiseListChangedEvents = True
        Catch ex As Exception
            Helper.WriteLog(ex)
            'MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        End Try
        dsSalBags.AcceptChanges()
    End Sub

    Public Sub dasalBox_commands()
        Dim SelectCommand As New MySqlCommand("SELECT * FROM SalesDetails WHERE  Type = 'BOX' AND FormulaID=" & FormulaID.Text, DbCon)
        daSalBox.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO SalesDetails
        (FormulaID
        , PackagingFormat
        , UnitSize
        , Quantity
        , MarginPercentage
        , SalesPrice
        ,VersionNumber
        ,Type
        , OverriddenSalesPrice
        , Overridden) 
        VALUES
        (" & FormulaID.Text & "
        , @PackagingFormat        
        , @UnitSize 
        , @Quantity
        , @MarginPercentage
        , @SalesPrice
        , @VersionNumber
        , @Type
        , @OverriddenSalesPrice
        , @Overridden)", DbCon)

        InsertCommand.Parameters.Add("@PackagingFormat", MySqlDbType.Int32, 10, "PackagingFormat")
        InsertCommand.Parameters.Add("@UnitSize", MySqlDbType.Decimal, 5, "UnitSize")
        InsertCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 11, "Quantity")
        InsertCommand.Parameters.Add("@MarginPercentage", MySqlDbType.Decimal, 4, "MarginPercentage")
        InsertCommand.Parameters.Add("@SalesPrice", MySqlDbType.Decimal, 11, "SalesPrice")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        InsertCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 20, "Type")
        InsertCommand.Parameters.Add("@OverriddenSalesPrice", MySqlDbType.Decimal, 8, "OverriddenSalesPrice")
        InsertCommand.Parameters.Add("@Overridden", MySqlDbType.Int32, 1, "Overridden")
        daSalBox.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE SalesDetails
        SET UnitSize = @UnitSize
        , Quantity = @Quantity 
        , MarginPercentage = @MarginPercentage
        , SalesPrice = @SalesPrice
		, VersionNumber=@VersionNumber, Type=@Type
        , OverriddenSalesPrice=@OverriddenSalesPrice ,Overridden=@Overridden 
        WHERE SalesDetailID = @SalesDetailID", DbCon)

        UpdateCommand.Parameters.Add("@SalesDetailID", MySqlDbType.Int32, 10, "SalesDetailID")
        UpdateCommand.Parameters.Add("@UnitSize", MySqlDbType.Decimal, 5, "UnitSize")
        UpdateCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 11, "Quantity")
        UpdateCommand.Parameters.Add("@MarginPercentage", MySqlDbType.Decimal, 4, "MarginPercentage")
        UpdateCommand.Parameters.Add("@SalesPrice", MySqlDbType.Decimal, 11, "SalesPrice")
        'UpdateCommand.Parameters.Add("@SalesDetailID", MySqlDbType.Int32, 10, "SalesDetailID")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        UpdateCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 20, "Type")
        UpdateCommand.Parameters.Add("@OverriddenSalesPrice", MySqlDbType.Decimal, 8, "OverriddenSalesPrice")
        UpdateCommand.Parameters.Add("@Overridden", MySqlDbType.Int32, 1, "Overridden")
        daSalBox.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM SalesDetails
        WHERE SalesDetailID = @SalesDetailID", DbCon)

        DeleteCommand.Parameters.Add("@SalesDetailID", MySqlDbType.Int32, 10, "SalesDetailID")
        daSalBox.DeleteCommand = DeleteCommand


        Dim bs As BindingSource = DirectCast(gridBoxSales.DataSource, BindingSource)
        bs.EndEdit()
        BindSalesBoxGrid.EndEdit()
        Try
            bs.RaiseListChangedEvents = False
            daSalBox.Update(dsSalBox, "salesBoxTxns")
            bs.RaiseListChangedEvents = True
        Catch ex As Exception
            Helper.WriteLog(ex)
            'MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        End Try
        dsSalBox.AcceptChanges()
    End Sub

    Private Sub dsFV_commands()

        Dim SelectCommand As New MySqlCommand("SELECT * FROM FormulaVersions WHERE FormulaID = " & FormulaID.Text & " ORDER BY VersionNumber ASC", DbCon)
        daFV.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO FormulaVersions
        (FormulaID
        , VersionNumber        
        , VersionDescription
        , ServingSize
        , ServingWeight
        , IsDefault
        ,PackagingType
        ,IsBox, IsBag
        ) 
        VALUES
        (" & FormulaID.Text & "
        , @VersionNumber        
        , @VersionDescription
        , @ServingSize
        , @ServingWeight
        , @IsDefault
        , @PackagingType
        , @IsBox, @IsBag)", DbCon)

        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 2, "VersionNumber")
        InsertCommand.Parameters.Add("@VersionDescription", MySqlDbType.VarChar, 40, "VersionDescription")
        InsertCommand.Parameters.Add("@ServingSize", MySqlDbType.Double, 5, "ServingSize")
        InsertCommand.Parameters.Add("@ServingWeight", MySqlDbType.Double, 11, "ServingWeight")
        InsertCommand.Parameters.Add("@IsDefault", MySqlDbType.Int32, 4, "IsDefault")
        InsertCommand.Parameters.Add("@PackagingType", MySqlDbType.VarChar, 20, "PackagingType")
        InsertCommand.Parameters.Add("@IsBox", MySqlDbType.Int32, 4, "IsBox")
        InsertCommand.Parameters.Add("@IsBag", MySqlDbType.Int32, 4, "IsBag")

        daFV.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE FormulaVersions
        SET VersionDescription = @VersionDescription
        , ServingSize = @ServingSize
        , ServingWeight = @ServingWeight 
        , IsDefault = @IsDefault
        ,PackagingType = @PackagingType , IsBox=@IsBox , IsBag=@IsBag
        WHERE FormulaVersionID = @FormulaVersionID ", DbCon)

        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 2, "VersionNumber")
        UpdateCommand.Parameters.Add("@VersionDescription", MySqlDbType.VarChar, 40, "VersionDescription")
        UpdateCommand.Parameters.Add("@ServingSize", MySqlDbType.Double, 5, "ServingSize")
        UpdateCommand.Parameters.Add("@ServingWeight", MySqlDbType.Double, 11, "ServingWeight")
        UpdateCommand.Parameters.Add("@IsDefault", MySqlDbType.Int32, 4, "IsDefault")
        UpdateCommand.Parameters.Add("@FormulaVersionID", MySqlDbType.Int32, 10, "FormulaVersionID")
        UpdateCommand.Parameters.Add("@PackagingType", MySqlDbType.VarChar, 20, "PackagingType")
        UpdateCommand.Parameters.Add("@IsBox", MySqlDbType.Int32, 4, "IsBox")
        UpdateCommand.Parameters.Add("@IsBag", MySqlDbType.Int32, 4, "IsBag")

        daFV.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM formulaversions WHERE FormulaVersionID = @FormulaVersionID", DbCon)
        DeleteCommand.Parameters.Add("@FormulaVersionID", MySqlDbType.Int32, 10, "FormulaVersionID")
        daFV.DeleteCommand = DeleteCommand

        Try
            daFV.Update(dsFV, "versions")
        Catch ex As Exception
            Helper.WriteLog(ex)
            'MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        End Try


        dsFV.AcceptChanges()

    End Sub
    Private Sub dsFF_commands()

        Dim sqlstring As String
        sqlstring = "UPDATE Formulas
        SET FormulaType = @FormulaType
        , PackagingFormat = @PackagingFormat
        , ServingSize = @ServingSize 
        , ServingWeight = @ServingWeight
        , UpdatedDate = NOW() 
        ,CustomerName=@CustomerName
        ,SalesRepId=@SalesRepId
        ,Contact=@Contact      
        WHERE FormulaID = " & FormulaID.Text


        ',OtherIngredients=@OtherIngredients
        ',LabTestingCostPolicy=@LabTestingCostPolicy
        ',QuoteExpiryDisclaimer=@QuoteExpiryDisclaimer
        ',Message=@Message

        Using UpdateCommand As New MySqlCommand(sqlstring, DbCon)
            DbCon.Open()
            UpdateCommand.Parameters.AddWithValue("@FormulaType", FormulaTypeCmbBox.Text)
            UpdateCommand.Parameters.AddWithValue("@PackagingFormat", ComboBox3.SelectedIndex)
            UpdateCommand.Parameters.AddWithValue("@ServingSize", ServingSizeTextBox.Text)
            UpdateCommand.Parameters.AddWithValue("@CustomerName", txtCustName.Text)
            UpdateCommand.Parameters.AddWithValue("@SalesRepId", txtSalesRepId.Text)
            UpdateCommand.Parameters.AddWithValue("@Contact", txtContact.Text)
            'UpdateCommand.Parameters.AddWithValue("@OtherIngredients", Terms("Other Ingredients"))
            'UpdateCommand.Parameters.AddWithValue("@LabTestingCostPolicy", Terms("LabTestingCostPolicy"))
            'UpdateCommand.Parameters.AddWithValue("@QuoteExpiryDisclaimer", Terms("QuoteExpiryDisclaimer"))
            'UpdateCommand.Parameters.AddWithValue("@Message", Terms("Message"))

            'UpdateCommand.Parameters.AddWithValue("@ServingWeight", ServingWeightLabel.Text)
            'Changes by Payal P on 22-01-2025 -- 
            Dim sttb As Double
            If Double.TryParse(ServingWeightLabel.Text, sttb) Then
                UpdateCommand.Parameters.AddWithValue("@ServingWeight", sttb)
            End If
            UpdateCommand.ExecuteNonQuery()
            DbCon.Close()

        End Using

        dsFF.AcceptChanges()

    End Sub
    Private Sub dsStandupBags_commands()

        Dim SelectCommand As New MySqlCommand("SELECT * FROM StandUpBagDetails WHERE FORMULAID=" & FormulaID.Text, DbCon)
        daStandupBags.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO StandUpBagDetails (
        FormulaId ,
        Description_ ,
        QuantityBag  ,
        MaterialBag ,
        SizeBag ,
        PrintColor ,
        TearNotch ,
        Zipper ,
        HangerHole ,
        ServingWeight ,
        Servings ,
        FillWeight ,
        PrintingPlatesBag ,
        ArtPreparationBag ,
        SecondaryPackout ,
        Other ,
        Other_Description,
        Shipper_CaseCount,
        Shipper_CaseAmt,
        Price,
        PrintingCostBag,
        Research_DevelopmentBag,
        VersionNumber,
        AddedFrom) VALUE (
        @FormulaId ,
        @Description_ ,
        @QuantityBag  ,
        @MaterialBag ,
        @SizeBag ,
        @PrintColor ,
        @TearNotch ,
        @Zipper ,
        @HangerHole ,
        @ServingWeight ,
        @Servings ,
        @FillWeight ,
        @PrintingPlatesBag ,
        @ArtPreparationBag ,
        @SecondaryPackout,
        @Other ,
        @Other_Description, 
        @Shipper_CaseCount,
        @Shipper_CaseAmt,
        @Price,
        @PrintingCostBag,
        @Research_DevelopmentBag,
        @VersionNumber,
        @AddedFrom)", DbCon)

        InsertCommand.Parameters.Add("@FormulaId", MySqlDbType.Int32, 10, "FormulaId")
        InsertCommand.Parameters.Add("@Description_", MySqlDbType.VarChar, 100, "Description_")
        InsertCommand.Parameters.Add("@QuantityBag", MySqlDbType.Int32, 10, "QuantityBag")
        InsertCommand.Parameters.Add("@MaterialBag", MySqlDbType.VarChar, 200, "MaterialBag")
        InsertCommand.Parameters.Add("@SizeBag", MySqlDbType.VarChar, 30, "SizeBag")
        InsertCommand.Parameters.Add("@PrintColor", MySqlDbType.VarChar, 50, "PrintColor")
        InsertCommand.Parameters.Add("@TearNotch", MySqlDbType.Byte, 1, "TearNotch")
        InsertCommand.Parameters.Add("@Zipper", MySqlDbType.Byte, 1, "Zipper")
        InsertCommand.Parameters.Add("@HangerHole", MySqlDbType.Byte, 1, "HangerHole")
        InsertCommand.Parameters.Add("@ServingWeight", MySqlDbType.Double, 15, "ServingWeight")
        InsertCommand.Parameters.Add("@Servings", MySqlDbType.VarChar, 45, "Servings")
        InsertCommand.Parameters.Add("@FillWeight", MySqlDbType.Double, 15, "FillWeight")
        InsertCommand.Parameters.Add("@PrintingPlatesBag", MySqlDbType.Double, 15, "PrintingPlatesBag")
        InsertCommand.Parameters.Add("@ArtPreparationBag", MySqlDbType.Double, 15, "ArtPreparationBag")
        InsertCommand.Parameters.Add("@SecondaryPackout", MySqlDbType.Double, 15, "SecondaryPackout")
        InsertCommand.Parameters.Add("@Other", MySqlDbType.Double, 15, "Other")
        InsertCommand.Parameters.Add("@Shipper_CaseCount", MySqlDbType.VarChar, 45, "Shipper_CaseCount")
        InsertCommand.Parameters.Add("@AddedFrom", MySqlDbType.VarChar, 20, "AddedFrom")
        InsertCommand.Parameters.Add("@Shipper_CaseAmt", MySqlDbType.Double, 15, "Shipper_CaseAmt")
        InsertCommand.Parameters.Add("@Price", MySqlDbType.Double, 15, "Price")
        InsertCommand.Parameters.Add("@PrintingCostBag", MySqlDbType.Double, 15, "PrintingCostBag")
        InsertCommand.Parameters.Add("@Research_DevelopmentBag", MySqlDbType.Double, 15, "Research_DevelopmentBag")
        InsertCommand.Parameters.Add("@Other_Description", MySqlDbType.VarChar, 100, "Other_Description")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        daStandupBags.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE StandUpBagDetails SET 
        FormulaId= @FormulaId,
        Description_= @Description_,
        QuantityBag= @QuantityBag,
        MaterialBag= @MaterialBag,
        SizeBag= @SizeBag,
        PrintColor= @PrintColor,
        TearNotch= @TearNotch,
        Zipper= @Zipper,
        HangerHole= @HangerHole,
        ServingWeight= @ServingWeight,
        Servings= @Servings,
        FillWeight= @FillWeight,
        PrintingPlatesBag= @PrintingPlatesBag,
        ArtPreparationBag= @ArtPreparationBag,
        SecondaryPackout= @SecondaryPackout,
        Other= @Other,
        Shipper_CaseCount= @Shipper_CaseCount,
        Shipper_CaseAmt= @Shipper_CaseAmt,
        Price= @Price,
        PrintingCostBag=@PrintingCostBag ,
        Other_Description=@Other_Description,
        VersionNumber=@VersionNumber,
        Research_DevelopmentBag=@Research_DevelopmentBag,
        AddedFrom=@AddedFrom
        WHERE StandUpBagID = @StandUpBagID", DbCon)

        UpdateCommand.Parameters.Add("@FormulaId", MySqlDbType.Int32, 10, "FormulaId")
        UpdateCommand.Parameters.Add("@Description_", MySqlDbType.VarChar, 100, "Description_")
        UpdateCommand.Parameters.Add("@QuantityBag", MySqlDbType.Int32, 10, "QuantityBag")
        UpdateCommand.Parameters.Add("@MaterialBag", MySqlDbType.VarChar, 200, "MaterialBag")
        UpdateCommand.Parameters.Add("@SizeBag", MySqlDbType.VarChar, 30, "SizeBag")
        UpdateCommand.Parameters.Add("@PrintColor", MySqlDbType.VarChar, 50, "PrintColor")
        UpdateCommand.Parameters.Add("@TearNotch", MySqlDbType.Byte, 1, "TearNotch")
        UpdateCommand.Parameters.Add("@Zipper", MySqlDbType.Byte, 1, "Zipper")
        UpdateCommand.Parameters.Add("@HangerHole", MySqlDbType.Byte, 1, "HangerHole")
        UpdateCommand.Parameters.Add("@ServingWeight", MySqlDbType.Double, 15, "ServingWeight")
        UpdateCommand.Parameters.Add("@Servings", MySqlDbType.VarChar, 45, "Servings")
        UpdateCommand.Parameters.Add("@FillWeight", MySqlDbType.Double, 15, "FillWeight")
        UpdateCommand.Parameters.Add("@PrintingPlatesBag", MySqlDbType.Double, 15, "PrintingPlatesBag")
        UpdateCommand.Parameters.Add("@ArtPreparationBag", MySqlDbType.Double, 15, "ArtPreparationBag")
        UpdateCommand.Parameters.Add("@SecondaryPackout", MySqlDbType.Double, 15, "SecondaryPackout")
        UpdateCommand.Parameters.Add("@Other", MySqlDbType.Double, 15, "Other")
        UpdateCommand.Parameters.Add("@Shipper_CaseCount", MySqlDbType.VarChar, 45, "Shipper_CaseCount")
        UpdateCommand.Parameters.Add("@Shipper_CaseAmt", MySqlDbType.Double, 15, "Shipper_CaseAmt")
        UpdateCommand.Parameters.Add("@Price", MySqlDbType.Double, 15, "Price")
        UpdateCommand.Parameters.Add("@Other_Description", MySqlDbType.VarChar, 100, "Other_Description")
        UpdateCommand.Parameters.Add("@Research_DevelopmentBag", MySqlDbType.Double, 15, "Research_DevelopmentBag")
        UpdateCommand.Parameters.Add("@PrintingCostBag", MySqlDbType.Double, 15, "PrintingCostBag")
        UpdateCommand.Parameters.Add("@StandUpBagID", MySqlDbType.Int32, 10, "StandUpBagID")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        UpdateCommand.Parameters.Add("@AddedFrom", MySqlDbType.VarChar, 20, "AddedFrom")
        daStandupBags.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM standupbagdetails WHERE StandUpBagID=@StandUpBagID", DbCon)
        DeleteCommand.Parameters.Add("@StandUpBagID", MySqlDbType.Int16, 10, "StandUpBagID")
        daStandupBags.DeleteCommand = DeleteCommand

        Try
            daStandupBags.Update(dsStandupBags, "StandUpBag")
        Catch ex As Exception
            Helper.WriteLog(ex)
            'MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        End Try
        dsStandupBags.AcceptChanges()
    End Sub
    Private Sub daPSP_commands()

        Dim SelectCommand As New MySqlCommand("SELECT * FROM StickPackDetails WHERE FormulaID=" & FormulaID.Text, DbCon)
        daPSP.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO StickPackDetails
        (FormulaID
        , SizeCount        
        , Material
        , Quantity
        , PacketContents
        , CostContent
        , FillingCost
        , PrintingCost
        , DisplayCost
        , PrintingPlates
        , ArtPreparation
        , PackOut
        , OtherCostDesc
        , OtherCostValue
		,ShrinkWrap
        ,WaferSeal
		,Colors
        ,StickPacks
		,ShipperCaseCountValue
        ,ShipperCaseCountDesc
		,VersionNumber 
        ,DisplayQuantity    
        ,DisplayDescription     
        ,DisplaySpec 
        ,DisplayBoardGrade  
        ,DisplayDimension   
        ,DisplayProductStyle
        ,PrintingPlatesBulk
        ,ArtPreparationBulk
        ,OtherCostBulk
        ,OtherCostDescBulk
        ,ShipperCaseCountBulk
        ,ShipperCaseCostBulk
        ,Research_DevelopmentBulk
        ,Research_DevelopmentBox
        ) 


        VALUES
        (" & FormulaID.Text & "
        , @SizeCount        
        , @Material
        , @Quantity
        , @PacketContents
        , @CostContent
        , @FillingCost
        , @PrintingCost
        , @DisplayCost
        , @PrintingPlates
        , @ArtPreparation
        , @PackOut
        , @OtherCostDesc
        , @OtherCostValue
		,@ShrinkWrap
        ,@WaferSeal
        ,@Colors
        ,@StickPacks
		,@ShipperCaseCountValue
        ,@ShipperCaseCountDesc
        ,@VersionNumber
        ,@DisplayQuantity 
        ,@DisplayDescription 
        ,@DisplaySpec 
        ,@DisplayBoardGrade 
        ,@DisplayDimension 
        ,@DisplayProductStyle
        ,@PrintingPlatesBulk
        ,@ArtPreparationBulk
        ,@OtherCostBulk
        ,@OtherCostDescBulk
        ,@ShipperCaseCountBulk
        ,@ShipperCaseCostBulk
        ,@Research_DevelopmentBulk
        ,@Research_DevelopmentBox)", DbCon)


        InsertCommand.Parameters.Add("@SizeCount", MySqlDbType.VarChar, 200, "SizeCount")
        InsertCommand.Parameters.Add("@Material", MySqlDbType.VarChar, 200, "Material")
        InsertCommand.Parameters.Add("@Quantity", MySqlDbType.VarChar, 200, "Quantity")
        InsertCommand.Parameters.Add("@PacketContents", MySqlDbType.Double, 15, "PacketContents")
        InsertCommand.Parameters.Add("@CostContent", MySqlDbType.Double, 15, "CostContent")
        InsertCommand.Parameters.Add("@FillingCost", MySqlDbType.Double, 15, "FillingCost")
        InsertCommand.Parameters.Add("@PrintingCost", MySqlDbType.Double, 15, "PrintingCost")
        InsertCommand.Parameters.Add("@DisplayCost", MySqlDbType.Double, 15, "DisplayCost")
        InsertCommand.Parameters.Add("@PrintingPlates", MySqlDbType.Double, 15, "PrintingPlates")
        InsertCommand.Parameters.Add("@ArtPreparation", MySqlDbType.Double, 15, "ArtPreparation")
        InsertCommand.Parameters.Add("@PackOut", MySqlDbType.Double, 15, "PackOut")
        InsertCommand.Parameters.Add("@OtherCostDesc", MySqlDbType.VarChar, 200, "OtherCostDesc")
        InsertCommand.Parameters.Add("@OtherCostValue", MySqlDbType.Double, 15, "OtherCostValue")
        InsertCommand.Parameters.Add("@ShrinkWrap", MySqlDbType.Double, 15, "ShrinkWrap")
        InsertCommand.Parameters.Add("@WaferSeal", MySqlDbType.Double, 15, "WaferSeal")
        InsertCommand.Parameters.Add("@StickPacks", MySqlDbType.Double, 15, "StickPacks")
        InsertCommand.Parameters.Add("@Colors", MySqlDbType.VarChar, 200, "Colors")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        InsertCommand.Parameters.Add("@DisplayQuantity", MySqlDbType.Int32, 15, "DisplayQuantity")
        InsertCommand.Parameters.Add("@DisplayDescription", MySqlDbType.VarChar, 100, "DisplayDescription")
        InsertCommand.Parameters.Add("@DisplaySpec", MySqlDbType.VarChar, 100, "DisplaySpec")
        InsertCommand.Parameters.Add("@DisplayBoardGrade", MySqlDbType.VarChar, 100, "DisplayBoardGrade")
        InsertCommand.Parameters.Add("@DisplayDimension", MySqlDbType.VarChar, 100, "DisplayDimension")
        InsertCommand.Parameters.Add("@DisplayProductStyle", MySqlDbType.VarChar, 100, "DisplayProductStyle")
        InsertCommand.Parameters.Add("@ShipperCaseCountValue", MySqlDbType.Double, 15, "ShipperCaseCountValue")
        InsertCommand.Parameters.Add("@ShipperCaseCountDesc", MySqlDbType.VarChar, 20, "ShipperCaseCountDesc")

        InsertCommand.Parameters.Add("@PrintingPlatesBulk", MySqlDbType.Double, 15, "PrintingPlatesBulk")
        InsertCommand.Parameters.Add("@ArtPreparationBulk", MySqlDbType.Double, 15, "ArtPreparationBulk")
        InsertCommand.Parameters.Add("@OtherCostBulk", MySqlDbType.Double, 15, "OtherCostBulk")
        InsertCommand.Parameters.Add("@ShipperCaseCostBulk", MySqlDbType.Double, 15, "ShipperCaseCostBulk")
        InsertCommand.Parameters.Add("@OtherCostDescBulk", MySqlDbType.VarChar, 45, "OtherCostDescBulk")
        InsertCommand.Parameters.Add("@ShipperCaseCountBulk", MySqlDbType.VarChar, 45, "ShipperCaseCountBulk")
        InsertCommand.Parameters.Add("@Research_DevelopmentBox", MySqlDbType.Double, 15, "Research_DevelopmentBox")
        InsertCommand.Parameters.Add("@Research_DevelopmentBulk", MySqlDbType.Double, 15, "Research_DevelopmentBulk")

        daPSP.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE StickPackDetails
        SET SizeCount = @SizeCount
        , Material = @Material
        , Quantity = @Quantity 
        , PacketContents = @PacketContents
        , CostContent = @CostContent
        , FillingCost = @FillingCost
        , PrintingCost = @PrintingCost
        , DisplayCost = @DisplayCost
        , PrintingPlates = @PrintingPlates
        , ArtPreparation = @ArtPreparation
        , PackOut = @PackOut
        , OtherCostDesc = @OtherCostDesc
        , OtherCostValue = @OtherCostValue
		,ShrinkWrap=@ShrinkWrap
        ,WaferSeal=@WaferSeal
        ,StickPacks=@StickPacks
		,Colors=@Colors
		,VersionNumber=@VersionNumber		
        ,DisplayQuantity=@DisplayQuantity	
        ,DisplayDescription=@DisplayDescription		
        ,DisplaySpec=@DisplaySpec		
        ,DisplayBoardGrade=@DisplayBoardGrade		
        ,DisplayDimension=@DisplayDimension	
        ,DisplayProductStyle=@DisplayProductStyle
		,ShipperCaseCountValue=@ShipperCaseCountValue
        ,ShipperCaseCountDesc=@ShipperCaseCountDesc
		,VersionNumber=@VersionNumber
        ,PrintingPlatesBulk   = @PrintingPlatesBulk
        ,ArtPreparationBulk   = @ArtPreparationBulk
        ,OtherCostBulk        = @OtherCostBulk
        ,OtherCostDescBulk    = @OtherCostDescBulk
        ,ShipperCaseCountBulk = @ShipperCaseCountBulk
        ,ShipperCaseCostBulk	=@ShipperCaseCostBulk
        ,Research_DevelopmentBox=@Research_DevelopmentBox
        ,Research_DevelopmentBulk=@Research_DevelopmentBulk
        WHERE StickPackDetailID = @StickPackDetailID", DbCon)

        UpdateCommand.Parameters.Add("@SizeCount", MySqlDbType.VarChar, 200, "SizeCount")
        UpdateCommand.Parameters.Add("@Material", MySqlDbType.VarChar, 200, "Material")
        UpdateCommand.Parameters.Add("@Quantity", MySqlDbType.VarChar, 200, "Quantity")
        UpdateCommand.Parameters.Add("@PacketContents", MySqlDbType.Double, 15, "PacketContents")
        UpdateCommand.Parameters.Add("@CostContent", MySqlDbType.Double, 15, "CostContent")
        UpdateCommand.Parameters.Add("@FillingCost", MySqlDbType.Double, 15, "FillingCost")
        UpdateCommand.Parameters.Add("@PrintingCost", MySqlDbType.Double, 15, "PrintingCost")
        UpdateCommand.Parameters.Add("@DisplayCost", MySqlDbType.Double, 15, "DisplayCost")
        UpdateCommand.Parameters.Add("@PrintingPlates", MySqlDbType.Double, 15, "PrintingPlates")
        UpdateCommand.Parameters.Add("@ArtPreparation", MySqlDbType.Double, 15, "ArtPreparation")
        UpdateCommand.Parameters.Add("@PackOut", MySqlDbType.Double, 15, "PackOut")
        UpdateCommand.Parameters.Add("@OtherCostDesc", MySqlDbType.VarChar, 200, "OtherCostDesc")
        UpdateCommand.Parameters.Add("@OtherCostValue", MySqlDbType.Double, 15, "OtherCostValue")
        UpdateCommand.Parameters.Add("@StickPackDetailID", MySqlDbType.Int32, 10, "StickPackDetailID")
        UpdateCommand.Parameters.Add("@ShrinkWrap", MySqlDbType.Double, 15, "ShrinkWrap")
        UpdateCommand.Parameters.Add("@WaferSeal", MySqlDbType.Double, 15, "WaferSeal")
        UpdateCommand.Parameters.Add("@StickPacks", MySqlDbType.Double, 15, "StickPacks")
        UpdateCommand.Parameters.Add("@Colors", MySqlDbType.VarChar, 200, "Colors")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        UpdateCommand.Parameters.Add("@DisplayQuantity", MySqlDbType.Int32, 15, "DisplayQuantity")
        UpdateCommand.Parameters.Add("@DisplayDescription", MySqlDbType.VarChar, 100, "DisplayDescription")
        UpdateCommand.Parameters.Add("@DisplaySpec", MySqlDbType.VarChar, 100, "DisplaySpec")
        UpdateCommand.Parameters.Add("@DisplayBoardGrade", MySqlDbType.VarChar, 100, "DisplayBoardGrade")
        UpdateCommand.Parameters.Add("@DisplayDimension", MySqlDbType.VarChar, 100, "DisplayDimension")
        UpdateCommand.Parameters.Add("@DisplayProductStyle", MySqlDbType.VarChar, 100, "DisplayProductStyle")
        UpdateCommand.Parameters.Add("@ShipperCaseCountValue", MySqlDbType.Double, 15, "ShipperCaseCountValue")
        UpdateCommand.Parameters.Add("@ShipperCaseCountDesc", MySqlDbType.VarChar, 20, "ShipperCaseCountDesc")

        UpdateCommand.Parameters.Add("@PrintingPlatesBulk", MySqlDbType.Double, 15, "PrintingPlatesBulk")
        UpdateCommand.Parameters.Add("@ArtPreparationBulk", MySqlDbType.Double, 15, "ArtPreparationBulk")
        UpdateCommand.Parameters.Add("@OtherCostBulk", MySqlDbType.Double, 15, "OtherCostBulk")
        UpdateCommand.Parameters.Add("@ShipperCaseCostBulk", MySqlDbType.Double, 15, "ShipperCaseCostBulk")
        UpdateCommand.Parameters.Add("@OtherCostDescBulk", MySqlDbType.VarChar, 45, "OtherCostDescBulk")
        UpdateCommand.Parameters.Add("@ShipperCaseCountBulk", MySqlDbType.VarChar, 45, "ShipperCaseCountBulk")

        UpdateCommand.Parameters.Add("@Research_DevelopmentBox", MySqlDbType.Double, 15, "Research_DevelopmentBox")
        UpdateCommand.Parameters.Add("@Research_DevelopmentBulk", MySqlDbType.Double, 15, "Research_DevelopmentBulk")


        daPSP.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM StickPackDetails WHERE StickPackDetailID=@StickPackDetailID", DbCon)
        DeleteCommand.Parameters.Add("@StickPackDetailID", MySqlDbType.Int32, 10, "StickPackDetailID")
        daPSP.DeleteCommand = DeleteCommand

        Try
            daPSP.Update(dsPSP, "stickpacks")
        Catch ex As Exception
            Helper.WriteLog(ex)
            'MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        End Try



        'dsBi.Tables("blending").Clear()
        'daBi.Fill(dsBi, "blending")
        dsPSP.AcceptChanges()


    End Sub

    Private Sub daPBlisters_commands()
        Dim SelectCommand As New MySqlCommand("SELECT * FROM blisterdetails WHERE FormulaID=" & FormulaID.Text, DbCon)
        daPBlisters.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO blisterdetails
        (FormulaID
        , BlisterFormat        
        , BlisterSize
        , Material
        , Quantity
        , BlisterCount
        , BlisterType
        , BlisterAmount
        , BlisterCost
        , PrintingCost
        , FoldingCartonCost
        , PrintingPlatesCost
        , ArtPreparation
        , SecondaryPackoutCost
        , ShrinkWrap
        , WaferSealsCost
        , OtherCost
        , Other_Description
        , ShipperCaseCount
        , ShipperCaseCost
        , ShrinkWrapFlag
        , WaferSealsFlag
        , VersionNumber
        , ToolingCost
        , Quantity_DisplayBox 
        , Amount_DisplayBox 
        , Description_DisplayBox 
        , Spec_DisplayBox
        , BoardGrade_DisplayBox
        , Dimension_DisplayBox
        , ProductStyle_DisplayBox
        , PrintingPlatesCostBulk
        , ArtPreparationBulk
        , ShipperCaseCostBulk
        , ShipperCaseCountBulk
        , OtherCostBulk
        , ToolingCostBulk
        , Other_DescriptionBulk
        , Research_Development
        , Research_DevelopmentBulk
        ) 
        VALUES
        (" & FormulaID.Text & "
        , @BlisterFormat        
        , @BlisterSize
        , @Material
        , @Quantity
        , @BlisterCount
        , @BlisterType
        , @BlisterAmount
        , @BlisterCost
        , @PrintingCost
        , @FoldingCartonCost
        , @PrintingPlatesCost
        , @ArtPreparation
        , @SecondaryPackoutCost
        , @ShrinkWrap
        , @WaferSealsCost
        , @OtherCost
        , @Other_Description
        , @ShipperCaseCount
        , @ShipperCaseCost
        , @ShrinkWrapFlag
        , @WaferSealsFlag
        , @VersionNumber
        , @ToolingCost
        , @Quantity_DisplayBox
        , @Amount_DisplayBox
        , @Description_DisplayBox
        , @Spec_DisplayBox
        , @BoardGrade_DisplayBox
        , @Dimension_DisplayBox
        , @ProductStyle_DisplayBox
        , @PrintingPlatesCostBulk
        , @ArtPreparationBulk
        , @ShipperCaseCostBulk
        , @ShipperCaseCountBulk
        , @OtherCostBulk
        , @ToolingCostBulk
        , @Other_DescriptionBulk
        , @Research_Development
        , @Research_DevelopmentBulk)", DbCon)

        InsertCommand.Parameters.Add("@BlisterFormat", MySqlDbType.VarChar, 45, "BlisterFormat")
        InsertCommand.Parameters.Add("@BlisterSize", MySqlDbType.VarChar, 45, "BlisterSize")
        InsertCommand.Parameters.Add("@Material", MySqlDbType.VarChar, 45, "Material")
        InsertCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 10, "Quantity")
        InsertCommand.Parameters.Add("@BlisterCount", MySqlDbType.Double, 15, "BlisterCount")
        InsertCommand.Parameters.Add("@BlisterType", MySqlDbType.VarChar, 45, "BlisterType")
        InsertCommand.Parameters.Add("@BlisterAmount", MySqlDbType.Double, 15, "BlisterAmount")
        InsertCommand.Parameters.Add("@BlisterCost", MySqlDbType.Double, 15, "BlisterCost")
        InsertCommand.Parameters.Add("@PrintingCost", MySqlDbType.Double, 15, "PrintingCost")
        InsertCommand.Parameters.Add("@FoldingCartonCost", MySqlDbType.Double, 15, "FoldingCartonCost")
        InsertCommand.Parameters.Add("@PrintingPlatesCost", MySqlDbType.Double, 15, "PrintingPlatesCost")
        InsertCommand.Parameters.Add("@ArtPreparation", MySqlDbType.Double, 15, "ArtPreparation")
        InsertCommand.Parameters.Add("@SecondaryPackoutCost", MySqlDbType.Double, 15, "SecondaryPackoutCost")
        InsertCommand.Parameters.Add("@ShrinkWrap", MySqlDbType.Double, 15, "ShrinkWrap")
        InsertCommand.Parameters.Add("@WaferSealsCost", MySqlDbType.Double, 15, "WaferSealsCost")
        InsertCommand.Parameters.Add("@OtherCost", MySqlDbType.Double, 15, "OtherCost")
        InsertCommand.Parameters.Add("@ShipperCaseCount", MySqlDbType.VarChar, 45, "ShipperCaseCount")
        InsertCommand.Parameters.Add("@ShipperCaseCost", MySqlDbType.Double, 15, "ShipperCaseCost")
        InsertCommand.Parameters.Add("@ShrinkWrapFlag", MySqlDbType.Int32, 2, "ShrinkWrapFlag")
        InsertCommand.Parameters.Add("@Other_Description", MySqlDbType.VarChar, 100, "Other_Description")
        InsertCommand.Parameters.Add("@WaferSealsFlag", MySqlDbType.Int32, 2, "WaferSealsFlag")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        InsertCommand.Parameters.Add("@ToolingCost", MySqlDbType.Double, 2, "ToolingCost")
        InsertCommand.Parameters.Add("@Quantity_DisplayBox", MySqlDbType.Int32, 2, "Quantity_DisplayBox")
        InsertCommand.Parameters.Add("@Amount_DisplayBox", MySqlDbType.Double, 2, "Amount_DisplayBox")
        InsertCommand.Parameters.Add("@Description_DisplayBox", MySqlDbType.VarChar, 100, "Description_DisplayBox")
        InsertCommand.Parameters.Add("@Spec_DisplayBox", MySqlDbType.VarChar, 100, "Spec_DisplayBox")
        InsertCommand.Parameters.Add("@BoardGrade_DisplayBox", MySqlDbType.VarChar, 100, "BoardGrade_DisplayBox")
        InsertCommand.Parameters.Add("@Dimension_DisplayBox", MySqlDbType.VarChar, 100, "Dimension_DisplayBox")
        InsertCommand.Parameters.Add("@ProductStyle_DisplayBox", MySqlDbType.VarChar, 100, "ProductStyle_DisplayBox")
        InsertCommand.Parameters.Add("@PrintingPlatesCostBulk", MySqlDbType.Double, 2, "PrintingPlatesCostBulk")
        InsertCommand.Parameters.Add("@ArtPreparationBulk", MySqlDbType.Double, 2, "ArtPreparationBulk")
        InsertCommand.Parameters.Add("@ToolingCostBulk", MySqlDbType.Double, 2, "ToolingCostBulk")
        InsertCommand.Parameters.Add("@OtherCostBulk", MySqlDbType.Double, 2, "OtherCostBulk")
        InsertCommand.Parameters.Add("@ShipperCaseCostBulk", MySqlDbType.Double, 2, "ShipperCaseCostBulk")
        InsertCommand.Parameters.Add("@Research_DevelopmentBulk", MySqlDbType.Double, 15, "Research_DevelopmentBulk")
        InsertCommand.Parameters.Add("@Research_Development", MySqlDbType.Double, 15, "Research_Development")
        InsertCommand.Parameters.Add("@ShipperCaseCountBulk", MySqlDbType.VarChar, 45, "ShipperCaseCountBulk")
        InsertCommand.Parameters.Add("@Other_DescriptionBulk", MySqlDbType.VarChar, 45, "Other_DescriptionBulk")
        daPBlisters.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE blisterdetails
        SET BlisterFormat = @BlisterFormat
        , BlisterSize = @BlisterSize
        , Quantity = @Quantity 
        , Material = @Material
        , BlisterCount = @BlisterCount
        , BlisterType = @BlisterType
        , BlisterAmount = @BlisterAmount
        , PrintingCost = @PrintingCost
        , BlisterCost = @BlisterCost
        , FoldingCartonCost = @FoldingCartonCost
        , ArtPreparation = @ArtPreparation
        , PrintingPlatesCost = @PrintingPlatesCost
        , SecondaryPackoutCost = @SecondaryPackoutCost
        , ShrinkWrap = @ShrinkWrap
        , WaferSealsCost = @WaferSealsCost 
        , OtherCost = @OtherCost
        , ShipperCaseCount = @ShipperCaseCount
        , ShipperCaseCost = @ShipperCaseCost
        , ShrinkWrapFlag = @ShrinkWrapFlag
        , WaferSealsFlag = @WaferSealsFlag
        , Other_Description=@Other_Description
        ,VersionNumber=@VersionNumber
        ,ToolingCost = @ToolingCost
        ,Quantity_DisplayBox =@Quantity_DisplayBox
        ,Amount_DisplayBox =@Amount_DisplayBox
        ,Description_DisplayBox =@Description_DisplayBox
        ,Spec_DisplayBox =@Spec_DisplayBox
        ,BoardGrade_DisplayBox =@BoardGrade_DisplayBox
        ,Dimension_DisplayBox =@Dimension_DisplayBox
        ,ProductStyle_DisplayBox =@ProductStyle_DisplayBox
        ,PrintingPlatesCostBulk= @PrintingPlatesCostBulk
        ,ArtPreparationBulk    = @ArtPreparationBulk    
        ,ShipperCaseCostBulk   = @ShipperCaseCostBulk   
        ,OtherCostBulk         = @OtherCostBulk         
        ,ShipperCaseCountBulk  = @ShipperCaseCountBulk  
        ,ToolingCostBulk       = @ToolingCostBulk       
        ,Other_DescriptionBulk       = @Other_DescriptionBulk       
        ,Research_DevelopmentBulk =@Research_DevelopmentBulk
        ,Research_Development=@Research_Development
        WHERE BlisterDetailsID = @BlisterDetailsID", DbCon)

        UpdateCommand.Parameters.Add("@BlisterFormat", MySqlDbType.VarChar, 45, "BlisterFormat")
        UpdateCommand.Parameters.Add("@BlisterSize", MySqlDbType.VarChar, 45, "BlisterSize")
        UpdateCommand.Parameters.Add("@Material", MySqlDbType.VarChar, 45, "Material")
        UpdateCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 10, "Quantity")
        UpdateCommand.Parameters.Add("@BlisterCount", MySqlDbType.Double, 15, "BlisterCount")
        UpdateCommand.Parameters.Add("@BlisterType", MySqlDbType.VarChar, 45, "BlisterType")
        UpdateCommand.Parameters.Add("@BlisterAmount", MySqlDbType.Double, 15, "BlisterAmount")
        UpdateCommand.Parameters.Add("@BlisterCost", MySqlDbType.Double, 15, "BlisterCost")
        UpdateCommand.Parameters.Add("@PrintingCost", MySqlDbType.Double, 15, "PrintingCost")
        UpdateCommand.Parameters.Add("@FoldingCartonCost", MySqlDbType.Double, 15, "FoldingCartonCost")
        UpdateCommand.Parameters.Add("@PrintingPlatesCost", MySqlDbType.Double, 15, "PrintingPlatesCost")
        UpdateCommand.Parameters.Add("@ArtPreparation", MySqlDbType.Double, 15, "ArtPreparation")
        UpdateCommand.Parameters.Add("@SecondaryPackoutCost", MySqlDbType.Double, 15, "SecondaryPackoutCost")
        UpdateCommand.Parameters.Add("@ShrinkWrap", MySqlDbType.Double, 15, "ShrinkWrap")
        UpdateCommand.Parameters.Add("@WaferSealsCost", MySqlDbType.Double, 15, "WaferSealsCost")
        UpdateCommand.Parameters.Add("@OtherCost", MySqlDbType.VarChar, 45, "OtherCost")
        UpdateCommand.Parameters.Add("@Other_Description", MySqlDbType.VarChar, 100, "Other_Description")
        UpdateCommand.Parameters.Add("@ShipperCaseCount", MySqlDbType.VarChar, 45, "ShipperCaseCount")
        UpdateCommand.Parameters.Add("@ShipperCaseCost", MySqlDbType.Double, 15, "ShipperCaseCost")
        UpdateCommand.Parameters.Add("@ShrinkWrapFlag", MySqlDbType.Int32, 2, "ShrinkWrapFlag")
        UpdateCommand.Parameters.Add("@WaferSealsFlag", MySqlDbType.Int32, 2, "WaferSealsFlag")
        UpdateCommand.Parameters.Add("@BlisterDetailsID", MySqlDbType.Int32, 10, "BlisterDetailsID")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        'Changes by Payal P
        UpdateCommand.Parameters.Add("@ToolingCost", MySqlDbType.Double, 2, "ToolingCost")
        'Changes for Display box
        UpdateCommand.Parameters.Add("@Quantity_DisplayBox", MySqlDbType.Int32, 2, "Quantity_DisplayBox")
        UpdateCommand.Parameters.Add("@Amount_DisplayBox", MySqlDbType.Double, 2, "Amount_DisplayBox")
        UpdateCommand.Parameters.Add("@Description_DisplayBox", MySqlDbType.VarChar, 100, "Description_DisplayBox")
        UpdateCommand.Parameters.Add("@Spec_DisplayBox", MySqlDbType.VarChar, 100, "Spec_DisplayBox")
        UpdateCommand.Parameters.Add("@BoardGrade_DisplayBox", MySqlDbType.VarChar, 100, "BoardGrade_DisplayBox")
        UpdateCommand.Parameters.Add("@Dimension_DisplayBox", MySqlDbType.VarChar, 100, "Dimension_DisplayBox")
        UpdateCommand.Parameters.Add("@ProductStyle_DisplayBox", MySqlDbType.VarChar, 100, "ProductStyle_DisplayBox")

        UpdateCommand.Parameters.Add("@PrintingPlatesCostBulk", MySqlDbType.Double, 2, "PrintingPlatesCostBulk")
        UpdateCommand.Parameters.Add("@ArtPreparationBulk", MySqlDbType.Double, 2, "ArtPreparationBulk")
        UpdateCommand.Parameters.Add("@ToolingCostBulk", MySqlDbType.Double, 2, "ToolingCostBulk")
        UpdateCommand.Parameters.Add("@OtherCostBulk", MySqlDbType.Double, 2, "OtherCostBulk")
        UpdateCommand.Parameters.Add("@ShipperCaseCostBulk", MySqlDbType.Double, 2, "ShipperCaseCostBulk")
        UpdateCommand.Parameters.Add("@ShipperCaseCountBulk", MySqlDbType.VarChar, 45, "ShipperCaseCountBulk")
        UpdateCommand.Parameters.Add("@Other_DescriptionBulk", MySqlDbType.VarChar, 45, "Other_DescriptionBulk")

        UpdateCommand.Parameters.Add("@Research_DevelopmentBulk", MySqlDbType.Double, 15, "Research_DevelopmentBulk")
        UpdateCommand.Parameters.Add("@Research_Development", MySqlDbType.Double, 15, "Research_Development")

        daPBlisters.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM blisterdetails WHERE BlisterDetailsID=@BlisterDetailsID", DbCon)
        DeleteCommand.Parameters.Add("@BlisterDetailsID", MySqlDbType.Int16, 10, "BlisterDetailsID")
        daPBlisters.DeleteCommand = DeleteCommand

        Try
            daPBlisters.Update(dsPBlisters, "blisters")
        Catch ex As Exception
            Helper.WriteLog(ex)
            'MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        End Try
        dsPBlisters.AcceptChanges()
    End Sub
    Private Sub daPSa_commands()

        Dim SelectCommand As New MySqlCommand("SELECT * FROM SachetsDetails WHERE FormulaID=" & FormulaID.Text, DbCon)
        daPSa.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO SachetsDetails
        (FormulaID
        , SizeCount        
        , Material
        , Quantity
        , Colors
        , PacketContents
        , CostContent
        , FillingCost
        , PrintingCost
        , DisplayCost
        , PrintingPlates
        , ArtPreparation
        , PackOut
        , OtherCostDesc
        , OtherCostValue
		, ShrinkWrap
        , WaferSeal
        , Sachets
        , ShipperCaseCountValue
        , ShipperCaseCountDesc
        , VersionNumber
        , DisplayQuantity 
        , DisplayDescription 
        , DisplaySpec 
        , DisplayBoardGrade 
        , DisplayDimension 
        , DisplayProductStyle
        , PrintingPlatesBulk
        , ArtPreparationBulk
        , OtherCostDescBulk
        , OtherCostBulk
        , ShipperCaseCountBulk
        , ShipperCaseCostBulk
        , Research_Development
        , Research_DevelopmentBulk
) 
							   
        VALUES
        (" & FormulaID.Text & "
        , @SizeCount        
        , @Material
        , @Quantity
        , @Colors
        , @PacketContents
        , @CostContent
        , @FillingCost
        , @PrintingCost
        , @DisplayCost
        , @PrintingPlates
        , @ArtPreparation
        , @PackOut
        , @OtherCostDesc
        , @OtherCostValue
		, @ShrinkWrap
        , @WaferSeal
        , @Sachets,@ShipperCaseCountValue
        , @ShipperCaseCountDesc
        , @VersionNumber 
        , @DisplayQuantity 
        , @DisplayDescription 
        , @DisplaySpec 
        , @DisplayBoardGrade 
        , @DisplayDimension 
        , @DisplayProductStyle
        , @PrintingPlatesBulk
        , @ArtPreparationBulk
        , @OtherCostDescBulk
        , @OtherCostBulk
        , @ShipperCaseCountBulk
        , @ShipperCaseCostBulk
        , @Research_Development
        , @Research_DevelopmentBulk
        )", DbCon)


        InsertCommand.Parameters.Add("@SizeCount", MySqlDbType.VarChar, 200, "SizeCount")
        InsertCommand.Parameters.Add("@Material", MySqlDbType.VarChar, 200, "Material")
        InsertCommand.Parameters.Add("@Quantity", MySqlDbType.VarChar, 200, "Quantity")
        InsertCommand.Parameters.Add("@Colors", MySqlDbType.VarChar, 200, "Colors")
        InsertCommand.Parameters.Add("@PacketContents", MySqlDbType.Double, 15, "PacketContents")
        InsertCommand.Parameters.Add("@CostContent", MySqlDbType.Double, 15, "CostContent")
        InsertCommand.Parameters.Add("@FillingCost", MySqlDbType.Double, 15, "FillingCost")
        InsertCommand.Parameters.Add("@PrintingCost", MySqlDbType.Double, 15, "PrintingCost")
        InsertCommand.Parameters.Add("@DisplayCost", MySqlDbType.Double, 15, "DisplayCost")
        InsertCommand.Parameters.Add("@PrintingPlates", MySqlDbType.Double, 15, "PrintingPlates")
        InsertCommand.Parameters.Add("@ArtPreparation", MySqlDbType.Double, 15, "ArtPreparation")
        InsertCommand.Parameters.Add("@PackOut", MySqlDbType.Double, 15, "PackOut")
        InsertCommand.Parameters.Add("@OtherCostDesc", MySqlDbType.VarChar, 200, "OtherCostDesc")
        InsertCommand.Parameters.Add("@OtherCostValue", MySqlDbType.Double, 15, "OtherCostValue")
        InsertCommand.Parameters.Add("@ShrinkWrap", MySqlDbType.Double, 15, "ShrinkWrap")
        InsertCommand.Parameters.Add("@WaferSeal", MySqlDbType.Double, 15, "WaferSeal")
        InsertCommand.Parameters.Add("@Sachets", MySqlDbType.Double, 15, "Sachets")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")
        InsertCommand.Parameters.Add("@DisplayQuantity", MySqlDbType.Int32, 15, "DisplayQuantity")
        InsertCommand.Parameters.Add("@DisplayDescription", MySqlDbType.VarChar, 100, "DisplayDescription")
        InsertCommand.Parameters.Add("@DisplaySpec", MySqlDbType.VarChar, 100, "DisplaySpec")
        InsertCommand.Parameters.Add("@DisplayBoardGrade", MySqlDbType.VarChar, 100, "DisplayBoardGrade")
        InsertCommand.Parameters.Add("@DisplayDimension", MySqlDbType.VarChar, 100, "DisplayDimension")
        InsertCommand.Parameters.Add("@DisplayProductStyle", MySqlDbType.VarChar, 100, "DisplayProductStyle")
        InsertCommand.Parameters.Add("@ShipperCaseCountValue", MySqlDbType.Double, 15, "ShipperCaseCountValue")
        InsertCommand.Parameters.Add("@ShipperCaseCountDesc", MySqlDbType.VarChar, 20, "ShipperCaseCountDesc")
        'BULK
        InsertCommand.Parameters.Add("@PrintingPlatesBulk", MySqlDbType.Double, 15, "PrintingPlatesBulk")
        InsertCommand.Parameters.Add("@ArtPreparationBulk", MySqlDbType.Double, 15, "ArtPreparationBulk")
        InsertCommand.Parameters.Add("@OtherCostDescBulk", MySqlDbType.VarChar, 45, "OtherCostDescBulk")
        InsertCommand.Parameters.Add("@OtherCostBulk", MySqlDbType.Double, 15, "OtherCostBulk")
        InsertCommand.Parameters.Add("@ShipperCaseCountBulk", MySqlDbType.VarChar, 45, "ShipperCaseCountBulk")
        InsertCommand.Parameters.Add("@ShipperCaseCostBulk", MySqlDbType.Double, 15, "ShipperCaseCostBulk")

        InsertCommand.Parameters.Add("@Research_Development", MySqlDbType.Double, 15, "Research_Development")
        InsertCommand.Parameters.Add("@Research_DevelopmentBulk", MySqlDbType.Double, 15, "Research_DevelopmentBulk")

        daPSa.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE SachetsDetails
        SET SizeCount = @SizeCount
        , Material = @Material
        , Quantity = @Quantity 
        , Colors = @Colors
        , PacketContents = @PacketContents
        , CostContent = @CostContent
        , FillingCost = @FillingCost
        , PrintingCost = @PrintingCost
        , DisplayCost = @DisplayCost
        , PrintingPlates = @PrintingPlates
        , ArtPreparation = @ArtPreparation
        , PackOut = @PackOut
        , OtherCostDesc = @OtherCostDesc
        , OtherCostValue = @OtherCostValue
		, ShrinkWrap  = @ShrinkWrap
        , WaferSeal  = @WaferSeal
        , Sachets  = @Sachets
		,VersionNumber=@VersionNumber	 
        ,DisplayQuantity=@DisplayQuantity	
        ,DisplayDescription=@DisplayDescription		
        ,DisplaySpec=@DisplaySpec		
        ,DisplayBoardGrade=@DisplayBoardGrade		
        ,DisplayDimension=@DisplayDimension	
        ,DisplayProductStyle=@DisplayProductStyle	
		, ShipperCaseCountValue=@ShipperCaseCountValue
        , ShipperCaseCountDesc=@ShipperCaseCountDesc
        , PrintingPlatesBulk =     @PrintingPlatesBulk
        , ArtPreparationBulk =     @ArtPreparationBulk
        , OtherCostDescBulk  =   @OtherCostDescBulk
        , OtherCostBulk      =   @OtherCostBulk
        , ShipperCaseCountBulk   =     @ShipperCaseCountBulk
        , ShipperCaseCostBulk    =  @ShipperCaseCostBulk
        , Research_DevelopmentBulk= @Research_DevelopmentBulk
        , Research_Development = @Research_Development
        WHERE StickPackDetailID = @StickPackDetailID", DbCon)

        UpdateCommand.Parameters.Add("@SizeCount", MySqlDbType.VarChar, 200, "SizeCount")
        UpdateCommand.Parameters.Add("@Material", MySqlDbType.VarChar, 200, "Material")
        UpdateCommand.Parameters.Add("@Quantity", MySqlDbType.VarChar, 200, "Quantity")
        UpdateCommand.Parameters.Add("@Colors", MySqlDbType.VarChar, 200, "Colors")
        UpdateCommand.Parameters.Add("@PacketContents", MySqlDbType.Double, 15, "PacketContents")
        UpdateCommand.Parameters.Add("@CostContent", MySqlDbType.Double, 15, "CostContent")
        UpdateCommand.Parameters.Add("@FillingCost", MySqlDbType.Double, 15, "FillingCost")
        UpdateCommand.Parameters.Add("@PrintingCost", MySqlDbType.Double, 15, "PrintingCost")
        UpdateCommand.Parameters.Add("@DisplayCost", MySqlDbType.Double, 15, "DisplayCost")
        UpdateCommand.Parameters.Add("@PrintingPlates", MySqlDbType.Double, 15, "PrintingPlates")
        UpdateCommand.Parameters.Add("@ArtPreparation", MySqlDbType.Double, 15, "ArtPreparation")
        UpdateCommand.Parameters.Add("@PackOut", MySqlDbType.Double, 15, "PackOut")
        UpdateCommand.Parameters.Add("@OtherCostDesc", MySqlDbType.VarChar, 200, "OtherCostDesc")
        UpdateCommand.Parameters.Add("@OtherCostValue", MySqlDbType.Double, 15, "OtherCostValue")
        UpdateCommand.Parameters.Add("@StickPackDetailID", MySqlDbType.Int32, 10, "StickPackDetailID")
        UpdateCommand.Parameters.Add("@ShrinkWrap", MySqlDbType.Double, 15, "ShrinkWrap")
        UpdateCommand.Parameters.Add("@WaferSeal", MySqlDbType.Double, 15, "WaferSeal")
        UpdateCommand.Parameters.Add("@Sachets", MySqlDbType.Int32, 10, "Sachets")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")
        UpdateCommand.Parameters.Add("@DisplayQuantity", MySqlDbType.Int32, 15, "DisplayQuantity")
        UpdateCommand.Parameters.Add("@DisplayDescription", MySqlDbType.VarChar, 100, "DisplayDescription")
        UpdateCommand.Parameters.Add("@DisplaySpec", MySqlDbType.VarChar, 100, "DisplaySpec")
        UpdateCommand.Parameters.Add("@DisplayBoardGrade", MySqlDbType.VarChar, 100, "DisplayBoardGrade")
        UpdateCommand.Parameters.Add("@DisplayDimension", MySqlDbType.VarChar, 100, "DisplayDimension")
        UpdateCommand.Parameters.Add("@DisplayProductStyle", MySqlDbType.VarChar, 100, "DisplayProductStyle")
        UpdateCommand.Parameters.Add("@ShipperCaseCountValue", MySqlDbType.Double, 15, "ShipperCaseCountValue")
        UpdateCommand.Parameters.Add("@ShipperCaseCountDesc", MySqlDbType.VarChar, 20, "ShipperCaseCountDesc")
        'BULK
        UpdateCommand.Parameters.Add("@PrintingPlatesBulk", MySqlDbType.Double, 15, "PrintingPlatesBulk")
        UpdateCommand.Parameters.Add("@ArtPreparationBulk", MySqlDbType.Double, 15, "ArtPreparationBulk")
        UpdateCommand.Parameters.Add("@OtherCostDescBulk", MySqlDbType.VarChar, 45, "OtherCostDescBulk")
        UpdateCommand.Parameters.Add("@OtherCostBulk", MySqlDbType.Double, 15, "OtherCostBulk")
        UpdateCommand.Parameters.Add("@ShipperCaseCountBulk", MySqlDbType.VarChar, 45, "ShipperCaseCountBulk")
        UpdateCommand.Parameters.Add("@ShipperCaseCostBulk", MySqlDbType.Double, 15, "ShipperCaseCostBulk")

        UpdateCommand.Parameters.Add("@Research_DevelopmentBulk", MySqlDbType.Double, 15, "Research_DevelopmentBulk")
        UpdateCommand.Parameters.Add("@Research_Development", MySqlDbType.Double, 15, "Research_Development")

        daPSa.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM sachetsdetails WHERE StickPackDetailID=@StickPackDetailID", DbCon)
        DeleteCommand.Parameters.Add("@StickPackDetailID", MySqlDbType.Int16, 10, "StickPackDetailID")
        daPSa.DeleteCommand = DeleteCommand

        Try
            daPSa.Update(dsPSa, "sachets")
        Catch ex As Exception
            Helper.WriteLog(ex)
            'MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        End Try


        'dsBi.Tables("blending").Clear()
        'daBi.Fill(dsBi, "blending")
        dsPSa.AcceptChanges()


    End Sub
    Private Sub daPB_commands()


        If dsPB.Tables("bottles").Rows.Count > 0 Then
            For Each r As DataRow In dsPB.Tables("bottles").Rows
                If r.RowState <> DataRowState.Deleted Then
                    If ComboBox4.SelectedIndex <> -1 Then
                        If r.Item("SizeCount") = ComboBox4.Text Then
                            r.Item("IsDefault") = True
                        Else
                            r.Item("IsDefault") = False
                        End If
                    End If
                End If
            Next
        End If


        Dim SelectCommand As New MySqlCommand("SELECT * FROM BottlePackagingDetails WHERE FormulaID=" & FormulaID.Text, DbCon)
        daPB.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO BottlePackagingDetails
        (BindingIndex
        , FormulaID
        , SizeCount        
        , Category
        , MaterialName
        , VendorName
        , UnitCost
        , ComponentCode
        , IsDefault
        , EnteredBy
        , EnteredDate
        , VersionNumber) 
        VALUES
        (@BindingIndex
        , " & FormulaID.Text & "
        , @SizeCount        
        , @Category
        , @MaterialName
        , @VendorName
        , @UnitCost
        , @ComponentCode
        , 1
        , @EnteredBy
        , @EnteredDate
        , @VersionNumber)", DbCon)

        InsertCommand.Parameters.Add("@BindingIndex", MySqlDbType.Int32, 3, "BindingIndex")
        InsertCommand.Parameters.Add("@SizeCount", MySqlDbType.Double, 15, "SizeCount")
        InsertCommand.Parameters.Add("@Category", MySqlDbType.VarChar, 200, "Category")
        InsertCommand.Parameters.Add("@MaterialName", MySqlDbType.VarChar, 200, "MaterialName")
        InsertCommand.Parameters.Add("@VendorName", MySqlDbType.VarChar, 200, "VendorName")
        InsertCommand.Parameters.Add("@UnitCost", MySqlDbType.Double, 15, "UnitCost")
        InsertCommand.Parameters.Add("@ComponentCode", MySqlDbType.VarChar, 200, "ComponentCode")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")
        InsertCommand.Parameters.Add("@EnteredBy", MySqlDbType.Int32, 3, "EnteredBy")
        InsertCommand.Parameters.Add("@EnteredDate", MySqlDbType.Date, 10, "EnteredDate")

        daPB.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE BottlePackagingDetails
        SET BindingIndex = @BindingIndex
        , SizeCount = @SizeCount
        , Category = @Category
        , MaterialName = @MaterialName
        , VendorName = @VendorName
        , UnitCost = @UnitCost
        , ComponentCode = @ComponentCode
        , IsDefault = @IsDefault , VersionNumber= @VersionNumber
        WHERE BottlePackagingDetailsID = @BottlePackagingDetailsID", DbCon)

        UpdateCommand.Parameters.Add("@BindingIndex", MySqlDbType.Int32, 3, "BindingIndex")
        UpdateCommand.Parameters.Add("@SizeCount", MySqlDbType.Double, 15, "SizeCount")
        UpdateCommand.Parameters.Add("@Category", MySqlDbType.VarChar, 200, "Category")
        UpdateCommand.Parameters.Add("@MaterialName", MySqlDbType.VarChar, 200, "MaterialName")
        UpdateCommand.Parameters.Add("@VendorName", MySqlDbType.VarChar, 200, "VendorName")
        UpdateCommand.Parameters.Add("@UnitCost", MySqlDbType.Double, 15, "UnitCost")
        UpdateCommand.Parameters.Add("@ComponentCode", MySqlDbType.VarChar, 200, "ComponentCode")
        UpdateCommand.Parameters.Add("@IsDefault", MySqlDbType.Int32, 2, "IsDefault")
        UpdateCommand.Parameters.Add("@BottlePackagingDetailsID", MySqlDbType.Int32, 10, "BottlePackagingDetailsID")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")
        daPB.UpdateCommand = UpdateCommand


        Dim DeleteCommand As New MySqlCommand("DELETE FROM BottlePackagingDetails
        WHERE BottlePackagingDetailsID = @BottlePackagingDetailsID", DbCon)
        DeleteCommand.Parameters.Add("@BottlePackagingDetailsID", MySqlDbType.Int32, 10, "BottlePackagingDetailsID")
        daPB.DeleteCommand = DeleteCommand


        Try
            daPB.Update(dsPB, "bottles")
        Catch ex As Exception
            Helper.WriteLog(ex)
            'MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        End Try



        'dsBi.Tables("blending").Clear()
        'daBi.Fill(dsBi, "blending")
        dsPB.AcceptChanges()

    End Sub
    Private Sub daEi_commands()
        'If dsEi.Tables("encapsulation").Rows.Count > 0 Then
        '    dsEi.Tables("encapsulation").Rows(0).Item("TypeofCapsule") = ComboBox5.Text
        '    dsEi.Tables("encapsulation").Rows(0).Item("CapsuleCost") = TextBox1.Text
        '    dsEi.Tables("encapsulation").Rows(0).Item("EncapsulationCost") = TextBox7.Text
        '    dsEi.Tables("encapsulation").Rows(0).Item("WastageValue") = TextBox11.Text
        '    dsEi.Tables("encapsulation").Rows(0).Item("LabCost") = txtlabcost.Text
        '    dsEi.Tables("encapsulation").Rows(0).Item("WastagePercentage") = TextBox10.Text
        '    dsEi.Tables("encapsulation").Rows(0).Item("VersionNumber") = VersionCmbBox.Text
        'Else
        '    Exit Sub
        'End If
        DisplayEncapsulation(VersionCmbBox.Text)
        Dim SelectCommand As New MySqlCommand("SELECT * FROM EncapsulationDetails WHERE FormulaID=" & FormulaID.Text, DbCon)
        daEi.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO EncapsulationDetails
        (FormulaID
        , TypeofCapsule        
        , CapsuleCost
        , EncapsulationCost
        , WastagePercentage
        , WastageValue
        , LabCost
        , VersionNumber
        , color
        , EnteredBy
        , EnteredDate) 
        VALUES
        (" & FormulaID.Text & "
        , @TypeofCapsule        
        , @CapsuleCost
        , @EncapsulationCost
        , @WastagePercentage
        , @WastageValue
        , @LabCost
        , @VersionNumber     
        , @color
        , @EnteredBy
        , @EnteredDate) ", DbCon)

        InsertCommand.Parameters.Add("@color", MySqlDbType.VarChar, 20, "color")
        InsertCommand.Parameters.Add("@TypeofCapsule", MySqlDbType.VarChar, 200, "TypeofCapsule")
        InsertCommand.Parameters.Add("@CapsuleCost", MySqlDbType.Double, 15, "CapsuleCost")
        InsertCommand.Parameters.Add("@EncapsulationCost", MySqlDbType.Double, 15, "EncapsulationCost")
        InsertCommand.Parameters.Add("@WastagePercentage", MySqlDbType.Double, 15, "WastagePercentage")
        InsertCommand.Parameters.Add("@WastageValue", MySqlDbType.Double, 15, "WastageValue")
        InsertCommand.Parameters.Add("@EnteredBy", MySqlDbType.Int32, 3, "EnteredBy")
        InsertCommand.Parameters.Add("@EnteredDate", MySqlDbType.Date, 10, "EnteredDate")
        InsertCommand.Parameters.Add("@LabCost", MySqlDbType.Double, 15, "LabCost")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")
        daEi.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE EncapsulationDetails
		SET TypeofCapsule = @TypeofCapsule
        , CapsuleCost = @CapsuleCost
        , EncapsulationCost= @EncapsulationCost
        , WastagePercentage = @WastagePercentage
        , WastageValue = @WastageValue
        , LabCost = @LabCost
        , VersionNumber=@VersionNumber
        , color=@color
        WHERE EncapsulationDetailID=@EncapsulationDetailID", DbCon)

        UpdateCommand.Parameters.Add("@color", MySqlDbType.VarChar, 20, "color")
        UpdateCommand.Parameters.Add("@TypeofCapsule", MySqlDbType.VarChar, 200, "TypeofCapsule")
        UpdateCommand.Parameters.Add("@CapsuleCost", MySqlDbType.Double, 15, "CapsuleCost")
        UpdateCommand.Parameters.Add("@EncapsulationCost", MySqlDbType.Double, 15, "EncapsulationCost")
        UpdateCommand.Parameters.Add("@WastagePercentage", MySqlDbType.Double, 15, "WastagePercentage")
        UpdateCommand.Parameters.Add("@WastageValue", MySqlDbType.Double, 15, "WastageValue")
        UpdateCommand.Parameters.Add("@LabCost", MySqlDbType.Double, 15, "LabCost")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")
        UpdateCommand.Parameters.Add("@EncapsulationDetailID", MySqlDbType.Int32, 3, "EncapsulationDetailID")
        daEi.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM encapsulationdetails WHERE EncapsulationDetailID=@EncapsulationDetailID", DbCon)
        DeleteCommand.Parameters.Add("@EncapsulationDetailID", MySqlDbType.Int16, 10, "EncapsulationDetailID")
        daEi.DeleteCommand = DeleteCommand

        Try

            daEi.Update(dsEi, "encapsulation")
        Catch ex As Exception
            Helper.WriteLog(ex)
            'MessageBox.Show("Error occured...", "Error", MessageBoxButtons.OK)
        End Try


        ' dsBi.Tables("blending").Clear()
        'daBi.Fill(dsBi, "blending")
        dsEi.AcceptChanges()

    End Sub
    Private Sub daTi_commands()
        'If dsTi.Tables("tableting").Rows.Count > 0 Then
        '    dsTi.Tables("tableting").Rows(0).Item("TypeofCoating") = ComboBox2.Text
        '    dsTi.Tables("tableting").Rows(0).Item("CoatingCost") = TextBox2.Text
        '    dsTi.Tables("tableting").Rows(0).Item("CompressionCost") = TextBox3.Text
        '    dsTi.Tables("tableting").Rows(0).Item("WastageValue") = TextBox4.Text
        '    dsTi.Tables("tableting").Rows(0).Item("WastagePercentage") = TextBox5.Text
        '    dsTi.Tables("tableting").Rows(0).Item("LabCost") = TextBox6.Text
        '    dsTi.Tables("tableting").Rows(0).Item("VersionNumber") = VersionCmbBox.Text
        'Else
        '    Exit Sub
        'End If
        DisplayTableting(VersionCmbBox.Text)
        Dim SelectCommand As New MySqlCommand("SELECT * FROM TabletingDetails WHERE FormulaID=" & FormulaID.Text, DbCon)
        daBi.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO TabletingDetails
                    (FormulaID
        , TypeofCoating        
        , CoatingCost
        , CompressionCost
        , WastagePercentage
        , WastageValue
        , LabCost
        , EnteredBy
        , EnteredDate
        ,VersionNumber) 
        VALUES
        (" & FormulaID.Text & "
        , @TypeofCoating        
        , @CoatingCost
        , @CompressionCost
        , @WastagePercentage
        , @WastageValue
        , @LabCost
        , @EnteredBy
        , @EnteredDate
        ,@VersionNumber) ", DbCon)


        InsertCommand.Parameters.Add("@TypeofCoating", MySqlDbType.VarChar, 200, "TypeofCoating")
        InsertCommand.Parameters.Add("@CoatingCost", MySqlDbType.Double, 15, "CoatingCost")
        InsertCommand.Parameters.Add("@CompressionCost", MySqlDbType.Double, 15, "CompressionCost")
        InsertCommand.Parameters.Add("@WastagePercentage", MySqlDbType.Double, 15, "WastagePercentage")
        InsertCommand.Parameters.Add("@WastageValue", MySqlDbType.Double, 15, "WastageValue")
        InsertCommand.Parameters.Add("@LabCost", MySqlDbType.Double, 15, "LabCost")
        InsertCommand.Parameters.Add("@EnteredBy", MySqlDbType.Int32, 3, "EnteredBy")
        InsertCommand.Parameters.Add("@EnteredDate", MySqlDbType.Date, 10, "EnteredDate")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")

        daTi.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE TabletingDetails
		SET TypeofCoating = @TypeofCoating
        , CoatingCost = @CoatingCost
        , CompressionCost= @CompressionCost
        , WastagePercentage = @WastagePercentage
        , WastageValue = @WastageValue
        , LabCost = @LabCost
        ,VersionNumber=@VersionNumber
        WHERE TabletingDetailID=@TabletingDetailID", DbCon)

        UpdateCommand.Parameters.Add("@TypeofCoating", MySqlDbType.VarChar, 200, "TypeofCoating")
        UpdateCommand.Parameters.Add("@CoatingCost", MySqlDbType.Double, 15, "CoatingCost")
        UpdateCommand.Parameters.Add("@CompressionCost", MySqlDbType.Double, 15, "CompressionCost")
        UpdateCommand.Parameters.Add("@WastagePercentage", MySqlDbType.Double, 15, "WastagePercentage")
        UpdateCommand.Parameters.Add("@WastageValue", MySqlDbType.Double, 15, "WastageValue")
        UpdateCommand.Parameters.Add("@LabCost", MySqlDbType.Double, 15, "LabCost")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")
        UpdateCommand.Parameters.Add("@TabletingDetailID", MySqlDbType.Int32, 3, "TabletingDetailID")
        daTi.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM tabletingdetails WHERE TabletingDetailID=@TabletingDetailID", DbCon)
        DeleteCommand.Parameters.Add("@TabletingDetailID", MySqlDbType.Int16, 10, "TabletingDetailID")
        daTi.DeleteCommand = DeleteCommand

        Try
            daTi.Update(dsTi, "tableting")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

        dsTi.AcceptChanges()
    End Sub

    Private Sub daBi_commands()


        DisplayBlending(VersionCmbBox.Text)

        Dim SelectCommand As New MySqlCommand("SELECT * FROM BlendingDetails WHERE FormulaID=" & FormulaID.Text, DbCon)
        daBi.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO BlendingDetails
        (FormulaID
        , BlendingCost
        , WastagePercentage
        , WastageValue
        , LabCost
        , EnteredBy
        , EnteredDate
        , VersionNumber
        ,FlavorProfile) 
        VALUES
        (" & FormulaID.Text & "
        , @BlendingCost
        , @WastagePercentage
        , @WastageValue
        , @LabCost
        , @EnteredBy
        , @EnteredDate
        , @VersionNumber
        , @FlavorProfile) ", DbCon)


        InsertCommand.Parameters.Add("@BlendingCost", MySqlDbType.Double, 15, "BlendingCost")
        InsertCommand.Parameters.Add("@WastagePercentage", MySqlDbType.Double, 15, "WastagePercentage")
        InsertCommand.Parameters.Add("@WastageValue", MySqlDbType.Double, 15, "WastageValue")
        InsertCommand.Parameters.Add("@LabCost", MySqlDbType.Double, 15, "LabCost")
        InsertCommand.Parameters.Add("@EnteredBy", MySqlDbType.Int32, 3, "EnteredBy")
        InsertCommand.Parameters.Add("@EnteredDate", MySqlDbType.Date, 10, "EnteredDate")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")
        InsertCommand.Parameters.Add("@FlavorProfile", MySqlDbType.Double, 8, "FlavorProfile")

        daBi.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE BlendingDetails
        SET BlendingCost = @BlendingCost
        , WastagePercentage = @WastagePercentage
        , WastageValue = @WastageValue
        , LabCost = @LabCost
        , VersionNumber=@VersionNumber
        , FlavorProfile=@FlavorProfile
        WHERE BlendingDetailID=@BlendingDetailID", DbCon)
        UpdateCommand.Parameters.Add("@BlendingCost", MySqlDbType.Double, 15, "BlendingCost")
        UpdateCommand.Parameters.Add("@WastagePercentage", MySqlDbType.Double, 15, "WastagePercentage")
        UpdateCommand.Parameters.Add("@WastageValue", MySqlDbType.Double, 15, "WastageValue")
        UpdateCommand.Parameters.Add("@LabCost", MySqlDbType.Double, 15, "LabCost")
        UpdateCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 3, "VersionNumber")
        UpdateCommand.Parameters.Add("@BlendingDetailID", MySqlDbType.Int32, 3, "BlendingDetailID")
        UpdateCommand.Parameters.Add("@FlavorProfile", MySqlDbType.Double, 8, "FlavorProfile")

        daBi.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM BlendingDetails WHERE BlendingDetailID=@BlendingDetailID", DbCon)
        DeleteCommand.Parameters.Add("@BlendingDetailID", MySqlDbType.Int16, 10, "BlendingDetailID")
        daBi.DeleteCommand = DeleteCommand

        Try
            daBi.Update(dsBi, "blending")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
        dsBi.AcceptChanges()

    End Sub

    Private Sub daF_commands()

        Dim SelectCommand As New MySqlCommand("SELECT * FROM FormulaDetails WHERE FormulaID=" & FormulaID.Text & " ORDER BY IngredientIndex ASC", DbCon)
        daF.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO FormulaDetails
        (FormulaID
        , VersionID
        , IngredientIndex
        , MaterialName
        , ComponentCode
        , VendorName
        , Actualmg
        , Potency
        , Overage
        , mg
        , Freight
        , MaterialCost
        , EnteredDate
        , EnteredBy
        , IsManual
        , ManualCost
        , ManualCostDate
        , LatestCost
        , LatestCostDate) 
        VALUES
        (" & FormulaID.Text & "
        , @VersionID
        , @IngredientIndex
        , @MaterialName
        , @ComponentCode
        , @VendorName
        , @Actualmg
        , @Potency
        , @Overage
        , @mg
        , @Freight
        , @MaterialCost
        , @EnteredDate
        , @EnteredBy
        , @IsManual
        , @ManualCost
        , @ManualCostDate
        , @LatestCost
        , @LatestCostDate) ", DbCon)


        InsertCommand.Parameters.Add("@VersionID", MySqlDbType.Int32, 3, "VersionID")
        InsertCommand.Parameters.Add("@IngredientIndex", MySqlDbType.Int32, 3, "IngredientIndex")
        InsertCommand.Parameters.Add("@MaterialName", MySqlDbType.VarChar, 200, "MaterialName")
        InsertCommand.Parameters.Add("@ComponentCode", MySqlDbType.VarChar, 200, "ComponentCode")
        InsertCommand.Parameters.Add("@VendorName", MySqlDbType.VarChar, 200, "VendorName")
        InsertCommand.Parameters.Add("@Actualmg", MySqlDbType.Double, 18, "Actualmg")
        InsertCommand.Parameters.Add("@Potency", MySqlDbType.Double, 5, "Potency")
        InsertCommand.Parameters.Add("@Overage", MySqlDbType.Int32, 3, "Overage")
        InsertCommand.Parameters.Add("@mg", MySqlDbType.Double, 18, "mg")
        InsertCommand.Parameters.Add("@Freight", MySqlDbType.Double, 15, "Freight")
        InsertCommand.Parameters.Add("@MaterialCost", MySqlDbType.Double, 15, "MaterialCost")
        InsertCommand.Parameters.Add("@EnteredDate", MySqlDbType.Date, 10, "EnteredDate")
        InsertCommand.Parameters.Add("@EnteredBy", MySqlDbType.Int32, 3, "EnteredBy")
        InsertCommand.Parameters.Add("@IsManual", MySqlDbType.Int32, 1, "IsManual")
        InsertCommand.Parameters.Add("@ManualCost", MySqlDbType.Double, 15, "ManualCost")
        InsertCommand.Parameters.Add("@ManualCostDate", MySqlDbType.Date, 10, "ManualCostDate")
        InsertCommand.Parameters.Add("@LatestCost", MySqlDbType.Double, 15, "LatestCost")
        InsertCommand.Parameters.Add("@LatestCostDate", MySqlDbType.Date, 10, "LatestCostDate")

        daF.InsertCommand = InsertCommand

        Dim UpdateCommand As New MySqlCommand("UPDATE FormulaDetails
            SET IngredientIndex = @IngredientIndex
        , MaterialName = @MaterialName
        , ComponentCode = @ComponentCode
        , VendorName = @VendorName
        , Actualmg = @Actualmg
        , Potency = @Potency
        , Overage = @Overage
        , mg = @mg
        , Freight = @Freight
        , MaterialCost = @MaterialCost
        , IsManual = @IsManual
        , ManualCost = @ManualCost
        , ManualCostDate = @ManualCostDate
        , LatestCost = @LatestCost
        , LatestCostDate = @LatestCostDate
        WHERE FormulaTxnID = @FormulaTxnID", DbCon)

        UpdateCommand.Parameters.Add("@IngredientIndex", MySqlDbType.Int32, 3, "IngredientIndex")
        UpdateCommand.Parameters.Add("@MaterialName", MySqlDbType.VarChar, 200, "MaterialName")
        UpdateCommand.Parameters.Add("@ComponentCode", MySqlDbType.VarChar, 200, "ComponentCode")
        UpdateCommand.Parameters.Add("@VendorName", MySqlDbType.VarChar, 200, "VendorName")
        UpdateCommand.Parameters.Add("@Actualmg", MySqlDbType.Double, 18, "Actualmg")
        UpdateCommand.Parameters.Add("@Potency", MySqlDbType.Double, 5, "Potency")
        UpdateCommand.Parameters.Add("@Overage", MySqlDbType.Int32, 3, "Overage")
        UpdateCommand.Parameters.Add("@mg", MySqlDbType.Double, 18, "mg")
        UpdateCommand.Parameters.Add("@Freight", MySqlDbType.Double, 15, "Freight")
        UpdateCommand.Parameters.Add("@MaterialCost", MySqlDbType.Double, 15, "MaterialCost")
        UpdateCommand.Parameters.Add("@IsManual", MySqlDbType.Int32, 1, "IsManual")
        UpdateCommand.Parameters.Add("@ManualCost", MySqlDbType.Double, 15, "ManualCost")
        UpdateCommand.Parameters.Add("@ManualCostDate", MySqlDbType.Date, 10, "ManualCostDate")
        UpdateCommand.Parameters.Add("@LatestCost", MySqlDbType.Double, 15, "LatestCost")
        UpdateCommand.Parameters.Add("@LatestCostDate", MySqlDbType.Date, 10, "LatestCostDate")
        UpdateCommand.Parameters.Add("@FormulaTxnID", MySqlDbType.Int32, 10, "FormulaTxnID")

        daF.UpdateCommand = UpdateCommand

        Dim DeleteCommand As New MySqlCommand("DELETE FROM FormulaDetails WHERE FormulaTxnID = @FormulaTxnID", DbCon)

        DeleteCommand.Parameters.Add("@FormulaTxnID", MySqlDbType.Int32, 10, "FormulaTxnID")
        daF.DeleteCommand = DeleteCommand

        Try
            daF.Update(dsF, "formula")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

        Dim bs As BindingSource = DirectCast(DataGridView1.DataSource, BindingSource)
        bs.RaiseListChangedEvents = False
        dsF.AcceptChanges()
        bs.RaiseListChangedEvents = True

    End Sub
#End Region
    Private Sub btnRenameFormula_Click(sender As Object, e As EventArgs) Handles btnRenameFormula.Click
        Try
            If FormulaID.Text = -1 Then
                Dim result As DialogResult = MessageBox.Show("Save formula first", "Message", MessageBoxButtons.YesNo)

                Exit Sub
            End If

            Dim oldname As String = Me.Name
            Dim frm2 As New FormulaName
            frm2.FormulanameBox.Text = oldname
            frm2.Label2.Text = oldname
            frm2.Label1.Text = FormulaID.Text
            frm2.ShowDialog()

            If Me.Name = oldname Then
                Exit Sub
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub btnPrintWorkSheet_Click(sender As Object, e As EventArgs) Handles btnPrintWorkSheet.Click
        Try
            RowPIndex = 0
            PrtngIng = True

            PrintingPage = 1
            printPack = False

            SizeNo = 1
            SizePIndex = -1

            Dim PD As New PrintDocument
            PD = PrintDocument1

            PrintDocument1.DefaultPageSettings.Landscape = True
            If DataGridView1.RowCount > 0 Then
                Dim pp As New PrintDialog
                pp.Document = PD

                If pp.ShowDialog() = DialogResult.OK Then
                    PrintDocument1.PrinterSettings = pp.PrinterSettings
                    PrintDocument1.Print()
                End If
            Else
                MessageBox.Show("Add formula detail to print...", "Message", MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    'Changes by Payal P ---- Start 26-02-2025--- For Print Report
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Try
            Dim right As StringFormat = New StringFormat(StringFormatFlags.NoClip)
            right.Alignment = StringAlignment.Far

            Dim center As StringFormat = New StringFormat(StringFormatFlags.NoClip)
            center.Alignment = StringAlignment.Center

            Dim PrintedPerPage As Integer = 0

            Dim format1 As New StringFormat
            format1.Alignment = StringAlignment.Far

            Dim x As Integer = 50
            Dim y As Integer = 50

            Dim Temp As Int16

            'e.Graphics.DrawImage(My.Resources.GTLogo, x, y - 20, 156, 52)
            e.Graphics.DrawImage(My.Resources.brand_nutra_green_logo, x, y - 20, 156, 52)
            e.Graphics.DrawString("Formula Pricing Worksheet", New Font("Arial", 16, FontStyle.Bold), Brushes.Black, x + 325, y)
            e.Graphics.DrawString(DateTime.Now, New Font("Arial", 10, FontStyle.Bold), Brushes.Black, x + 850, y)

            Dim header_y As Integer = y + 50
            e.Graphics.DrawString("CUSTOMER NAME:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 40, header_y)
            e.Graphics.DrawString(Me.txtCustName.Text, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 150, header_y)

            'e.DrawString(Me.lblCustomerName.Text, New XFont("Arial", 8, XFontStyle.Bold), XBrushes.Black, 120, header_y + 10)

            e.Graphics.DrawString("CONTACT:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 540, header_y)
            e.Graphics.DrawString(Me.txtContact.Text, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 605, header_y)

            'e.DrawString(Me.lblContact.Text, New XFont("Arial", 8, XFontStyle.Bold), XBrushes.Black, 255, header_y + 10)


            e.Graphics.DrawString("FORMULA NAME:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 40, header_y + 20)
            If FormulaID.Text = -1 Then
                e.Graphics.DrawString("Untitled", New Font("Arial", 10, FontStyle.Bold), Brushes.Black, 140, header_y - 5)
            Else
                If Me.Name.Length > 45 Then
                    e.Graphics.DrawString(Me.Name.Substring(0, 40) & "...", New Font("Arial", 10, FontStyle.Bold), Brushes.Black, 140, header_y + 15)
                Else
                    e.Graphics.DrawString(Me.Name, New Font("Arial", 10, FontStyle.Bold), Brushes.Black, 140, header_y + 15)
                End If
            End If


            e.Graphics.DrawString("FORMULA TYPE:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 540, header_y + 20)
            e.Graphics.DrawString(Me.FormulaTypeCmbBox.SelectedItem, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 640, header_y + 20)
            e.Graphics.DrawString("FORMULA ID:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 740, header_y + 20)
            e.Graphics.DrawString(Me.FormulaID.Text, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 820, header_y + 20)
            e.Graphics.DrawString("VERSION:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 40, header_y + 40)
            e.Graphics.DrawString(VersionCmbBox.Text, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 100, header_y + 40)
            e.Graphics.DrawString("DESCRIPTION:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 130, header_y + 40)
            e.Graphics.DrawString(VersionDescriptionTxt.Text.ToString, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 215, header_y + 40)

            e.Graphics.DrawString("SERVING SIZE:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 540, header_y + 40)
            e.Graphics.DrawString(Me.ServingSizeTextBox.Text & " " & Me.ServingSizeUOMLabel.Text, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 630, header_y + 40)
            e.Graphics.DrawString("SALES REP:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 40, header_y + 60)
            e.Graphics.DrawString(Me.txtSalesRepId.Text, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 115, header_y + 60)

            e.Graphics.DrawString("PHONE:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 540, header_y + 60)
            e.Graphics.DrawString(phone, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 590, header_y + 60)

            e.Graphics.DrawString("EMAIL:", New Font("Arial", 8, FontStyle.Bold), Brushes.Gray, 740, header_y + 60)
            e.Graphics.DrawString(email, New Font("Arial", 8, FontStyle.Bold), Brushes.Black, 785, header_y + 60)

            If PrtngIng = True Then

                header_y += 60
                y = header_y + 30

                Dim RectHeight As Int16 = 30
                Dim TblHeader As New List(Of String)
                TblHeader.Add("Index")
                TblHeader.Add("Ingredient")
                TblHeader.Add("Component Code")
                TblHeader.Add("Vendor Code")
                TblHeader.Add("Supplier")
                TblHeader.Add("Actual WT")
                TblHeader.Add("Potency")
                TblHeader.Add("Overage")
                TblHeader.Add("mg")
                TblHeader.Add("Freight")
                TblHeader.Add("Cost")
                TblHeader.Add("Case Pack")
                TblHeader.Add(DataGridView1.Columns(10).HeaderText)

                Dim Font8Regular = New Font("Arial", 8, FontStyle.Regular)
                Dim Font8Bold = New Font("Arial", 8, FontStyle.Bold)
                Dim BlackBrush = Brushes.Black
                Dim BlackTransparent = Brushes.Transparent
                Dim BlackPen = Pens.Black
                Dim format As New StringFormat()
                format.Alignment = StringAlignment.Center
                format.LineAlignment = StringAlignment.Center

                Dim Rect = New Rectangle(x - 10, y, 40, RectHeight)
                e.Graphics.DrawRectangle(BlackPen, Rect)
                e.Graphics.DrawString(TblHeader(0), Font8Bold, BlackBrush, Rect, format)
                Rect = New Rectangle(x + 30, y, 220, RectHeight)
                e.Graphics.DrawRectangle(BlackPen, Rect)
                e.Graphics.DrawRectangle(BlackPen, Rect)
                e.Graphics.DrawString(TblHeader(1), Font8Bold, BlackBrush, Rect, format)
                Rect = New Rectangle(x + 250, y, 100, RectHeight)
                e.Graphics.DrawRectangle(BlackPen, Rect)
                e.Graphics.DrawString(TblHeader(2), Font8Bold, BlackBrush, Rect, format)
                Rect = New Rectangle(x + 350, y, 100, RectHeight)
                e.Graphics.DrawRectangle(BlackPen, Rect)
                e.Graphics.DrawString(TblHeader(3), Font8Bold, BlackBrush, Rect, format)
                Rect = New Rectangle(x + 450, y, 90, RectHeight)
                e.Graphics.DrawRectangle(BlackPen, Rect)
                e.Graphics.DrawString(TblHeader(4), Font8Bold, BlackBrush, Rect, format)

                Dim SizePerColumn As Double = 400 / 7
                x = 590
                For i As Integer = 0 To TblHeader.Count - 2
                    If i > 4 Then
                        Rect = New Rectangle(x, y, SizePerColumn, RectHeight)
                        e.Graphics.DrawRectangle(BlackPen, Rect)
                        e.Graphics.DrawString(TblHeader(i), Font8Bold, BlackBrush, Rect, format)
                        x = x + SizePerColumn
                    End If
                Next

                Rect = New Rectangle(x, y, 80, RectHeight)
                e.Graphics.DrawRectangle(BlackPen, Rect)
                e.Graphics.DrawString(TblHeader(12), Font8Bold, BlackBrush, Rect, format)

                y += 30

                With DataGridView1
                    Do While Me.RowPIndex < .Rows.Count

                        lblValues.Text = .Rows(Me.RowPIndex).Cells(2).Value.ToString
                        Dim Supplierlist As List(Of String) = Helper.Wordwrap(lblValues, 14)


                        lblValues.Text = .Rows(Me.RowPIndex).Cells(1).Value.ToString()
                        Dim Ingredientlist As List(Of String) = Helper.Wordwrap(lblValues, 39)

                        Dim Lines As Integer
                        If Supplierlist.Count > Ingredientlist.Count Then
                            Lines = Supplierlist.Count
                        ElseIf Supplierlist.Count < Ingredientlist.Count Then
                            Lines = Ingredientlist.Count
                        Else
                            Lines = Ingredientlist.Count
                        End If


                        x = 50
                        Rect = New Rectangle(x - 10, y, 40, Lines * 20)
                        e.Graphics.DrawRectangle(BlackPen, Rect)
                        e.Graphics.DrawString(.Rows(Me.RowPIndex).Cells(0).Value.ToString, Font8Regular, BlackBrush, Rect, format)


                        If Not IsDBNull(.Rows(Me.RowPIndex).Cells(1).Value) Then
                            Rect = New Rectangle(x + 30, y, 220, Lines * 20)
                            e.Graphics.DrawRectangle(BlackPen, Rect)
                            Dim Yindex As Integer = y + 3
                            For Each item As String In Ingredientlist
                                e.Graphics.DrawString(item, Font8Regular, BlackBrush, 90, Yindex)
                                Yindex += 14
                            Next
                        End If
                        Dim MaterialName = .Rows(Me.RowPIndex).Cells(1).Value.ToString()
                        Dim Code = (From tbl In ds.Tables("ingredients").AsEnumerable()
                                    Where tbl.Field(Of String)("MaterialName").ToLower() = MaterialName.ToLower()
                                    Select tbl.Field(Of String)("ComponentCode")).FirstOrDefault()

                        Rect = New Rectangle(x + 250, y, 100, Lines * 20)
                        e.Graphics.DrawRectangle(BlackPen, Rect)
                        e.Graphics.DrawString(If(IsDBNull(Code), "", Code), Font8Regular, BlackBrush, Rect, format)

                        Dim vn As String = ""
                        If Not IsNothing(.Rows(Me.RowPIndex).Cells(2).Value) Then
                            vn = .Rows(Me.RowPIndex).Cells(2).Value.ToString
                        End If

                        Dim VendorCode = (From tbl In ds.Tables("ingredients").AsEnumerable()
                                          Where tbl.Field(Of String)("MaterialName").ToLower() = MaterialName.ToLower()
                                          Select tbl.Field(Of String)("VendorCode")).FirstOrDefault()

                        Rect = New Rectangle(x + 350, y, 100, Lines * 20)
                        e.Graphics.DrawRectangle(BlackPen, Rect)
                        e.Graphics.DrawString(If(IsDBNull(VendorCode), "", VendorCode), Font8Regular, BlackBrush, Rect, format)

                        If Not String.IsNullOrEmpty(vn) Then
                            Dim YIndex As Integer = y + 3
                            Rect = New Rectangle(x + 450, y, 90, Lines * 20)
                            e.Graphics.DrawRectangle(BlackPen, Rect)
                            For Each line In Supplierlist
                                e.Graphics.DrawString(line, Font8Regular, BlackBrush, 510, YIndex)
                                YIndex += 14
                            Next
                        End If



                        x = 590
                        'Dim Ccount As Integer = DataGridView1.Columns.Cast(Of DataGridViewColumn)().Count(Function(c) c.Visible)
                        Dim Ccount As Integer = TblHeader.Count
                        For i As Integer = 5 To Ccount - 1

                            If i = 11 Then
                                Dim CasePack = (From tbl In ds.Tables("ingredients").AsEnumerable()
                                                Where tbl.Field(Of String)("MaterialName").ToLower() = MaterialName.ToLower()
                                                Select tbl.Field(Of Decimal)("CasePack")).FirstOrDefault()

                                Rect = New Rectangle(x, y, SizePerColumn, Lines * 20)
                                e.Graphics.DrawRectangle(BlackPen, Rect)
                                e.Graphics.DrawString(If(IsDBNull(CasePack), "", CasePack), Font8Regular, BlackBrush, Rect, format)
                            ElseIf i = 12 Then

                                Dim DecimalVal As Double = 0
                                If Not IsDBNull(.Rows(Me.RowPIndex).Cells(10).Value) Then
                                    Double.TryParse(.Rows(Me.RowPIndex).Cells(10).Value, DecimalVal)
                                End If
                                Rect = New Rectangle(x, y, 80, Lines * 20)

                                e.Graphics.DrawRectangle(BlackPen, Rect)
                                If Not DecimalVal = 0 Then
                                    e.Graphics.DrawString(DecimalVal.ToString("N2"), Font8Regular, BlackBrush, Rect, format)
                                Else
                                    e.Graphics.DrawString("0.00", Font8Regular, BlackBrush, Rect, format)
                                End If

                            ElseIf i = 5 Then

                                Dim DecimalVal As Double = 0
                                If Not IsDBNull(.Rows(Me.RowPIndex).Cells(3).Value) Then
                                    Double.TryParse(.Rows(Me.RowPIndex).Cells(3).Value, DecimalVal)
                                End If
                                Rect = New Rectangle(x, y, SizePerColumn, Lines * 20)

                                e.Graphics.DrawRectangle(BlackPen, Rect)
                                If Not DecimalVal = 0 Then
                                    e.Graphics.DrawString(DecimalVal.ToString("N2"), Font8Regular, BlackBrush, Rect, format)
                                Else
                                    e.Graphics.DrawString("0.00", Font8Regular, BlackBrush, Rect, format)
                                End If

                            ElseIf i = 6 Then

                                Dim DecimalVal As Double = 0
                                If Not IsDBNull(.Rows(Me.RowPIndex).Cells(4).Value) Then
                                    Double.TryParse(.Rows(Me.RowPIndex).Cells(4).Value, DecimalVal)
                                End If
                                Rect = New Rectangle(x, y, SizePerColumn, Lines * 20)

                                e.Graphics.DrawRectangle(BlackPen, Rect)
                                If Not DecimalVal = 0 Then
                                    e.Graphics.DrawString(DecimalVal.ToString("N2"), Font8Regular, BlackBrush, Rect, format)
                                Else
                                    e.Graphics.DrawString("0", Font8Regular, BlackBrush, Rect, format)
                                End If

                            ElseIf i = 7 Then
                                Dim DecimalVal As Double = 0
                                If Not IsDBNull(.Rows(Me.RowPIndex).Cells(5).Value) Then
                                    Double.TryParse(.Rows(Me.RowPIndex).Cells(5).Value, DecimalVal)
                                End If
                                Rect = New Rectangle(x, y, SizePerColumn, Lines * 20)

                                e.Graphics.DrawRectangle(BlackPen, Rect)
                                If Not DecimalVal = 0 Then
                                    e.Graphics.DrawString(DecimalVal.ToString("N2"), Font8Regular, BlackBrush, Rect, format)
                                Else
                                    e.Graphics.DrawString("0", Font8Regular, BlackBrush, Rect, format)
                                End If
                            ElseIf i = 8 Then

                                Dim DecimalVal As Double = 0
                                If Not IsDBNull(.Rows(Me.RowPIndex).Cells(6).Value) Then
                                    Double.TryParse(.Rows(Me.RowPIndex).Cells(6).Value, DecimalVal)
                                End If
                                Rect = New Rectangle(x, y, SizePerColumn, Lines * 20)

                                e.Graphics.DrawRectangle(BlackPen, Rect)
                                If Not DecimalVal = 0 Then
                                    e.Graphics.DrawString(DecimalVal.ToString("N2"), Font8Regular, BlackBrush, Rect, format)
                                Else
                                    e.Graphics.DrawString("0.00", Font8Regular, BlackBrush, Rect, format)
                                End If
                            ElseIf i = 9 Then

                                Dim DecimalVal As Double = 0
                                If Not IsDBNull(.Rows(Me.RowPIndex).Cells(7).Value) Then
                                    Double.TryParse(.Rows(Me.RowPIndex).Cells(7).Value, DecimalVal)
                                End If
                                Rect = New Rectangle(x, y, SizePerColumn, Lines * 20)

                                e.Graphics.DrawRectangle(BlackPen, Rect)
                                If Not DecimalVal = 0 Then
                                    e.Graphics.DrawString(DecimalVal.ToString("N2"), Font8Regular, BlackBrush, Rect, format)
                                Else
                                    e.Graphics.DrawString("0.00", Font8Regular, BlackBrush, Rect, format)
                                End If
                            ElseIf i = 10 Then

                                Dim DecimalVal As Double = 0
                                If Not IsDBNull(.Rows(Me.RowPIndex).Cells(8).Value) Then
                                    Double.TryParse(.Rows(Me.RowPIndex).Cells(8).Value, DecimalVal)
                                End If
                                Rect = New Rectangle(x, y, SizePerColumn, Lines * 20)

                                e.Graphics.DrawRectangle(BlackPen, Rect)
                                If Not DecimalVal = 0 Then
                                    e.Graphics.DrawString(DecimalVal.ToString("N2"), Font8Regular, BlackBrush, Rect, format)
                                Else
                                    e.Graphics.DrawString("0.00", Font8Regular, BlackBrush, Rect, format)
                                End If
                            End If


                            x = x + SizePerColumn
                        Next

                        y += Lines * 20

                        PrintedPerPage += 1
                        RowPIndex += 1

                        If PrintedPerPage = DataGridView1.Rows.Count Then
                            Continue Do
                        End If

                        If PrintedPerPage = RowsPerPage Then

                            Me.PrintingPage += 1
                            y = 50
                            e.HasMorePages = True
                            Exit Sub
                        End If

                    Loop
                End With

                x = 50
                e.Graphics.DrawString("Serving Weight:    " & Me.ServingWeightLabel.Text & " mg", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 496, y + 20, right)
                e.Graphics.DrawString("Mix Cost/kg:   ₹" & CostKiloLabel.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 1000, y + 20, right)


                PrtngIng = False
                If SizePIndex = -1 Then
                    e.HasMorePages = True
                    SizePIndex = 0
                    Exit Sub
                End If

            End If

            y = 200

            'Manufacturing profit
            e.Graphics.DrawString("Manufacturing", New Font("Arial", 12, FontStyle.Bold), Brushes.Black, x + 10, y)

            e.Graphics.DrawString("Packaging info", New Font("Arial", 12, FontStyle.Bold), Brushes.Black, x + 510, y)
            e.Graphics.DrawLine(New Pen(Brushes.DarkGray, 1), x + 220, y + 20, x + 220, y + 580)

            y += 60

            Temp = y

            If FormulaTypeCmbBox.Text = "Capsule" Then
                Preview.PrintCapsule(y, x, e, right, Me)
                y += 100
            ElseIf FormulaTypeCmbBox.Text = "Tablet" Then
                Preview.PrintTablet(y, x, e, right, Me)
                y += 100
            Else
                Preview.PrintPowder(y, x, e, right, Me)
                y += 100
            End If

            'Print subtotal panel
            e.Graphics.DrawString("Sub Total", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y + 20)
            e.Graphics.DrawString("₹" & TextBox36.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y + 20, right)
            y += 20

            e.Graphics.DrawString("Overhead %", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y + 20)
            e.Graphics.DrawString(TextBox9.Text & "% ", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y + 20, right)
            y += 20

            e.Graphics.DrawString("Total:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y + 20)
            e.Graphics.DrawString("₹ " & TextBox8.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y + 20, right)
            y += 20

            y = Temp

            If ComboBox3.Text = "Sachets" Then
                Preview.PrintSachets(y, e, Me)

            ElseIf ComboBox3.Text = "Stick Packs" Then
                Preview.PrintStickPacks(y, e, Me)

            ElseIf ComboBox3.Text = "Bottles" Then
                Preview.PrintBottle(y, e, Me, dsPB)

            ElseIf ComboBox3.Text = "Bags" Then
                Preview.PrintBag(y, e, Me)

            ElseIf ComboBox3.Text = "Blister" Then
                Preview.PrintBlister(y, e, Me)

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub btnPDFQuote_Click(sender As Object, e As EventArgs) Handles btnPDFQuote.Click
        Try
            If FormulaTypeCmbBox.SelectedIndex = -1 Then
                FormulaTypeCmbBox.DroppedDown = True
                Exit Sub
            End If

            If FormulaID.Text = -1 Then
                MessageBox.Show("Save formula first", "Message", MessageBoxButtons.OK)
                Exit Sub
            End If

            Dim pre As Boolean = False
            pre = CheckUnsavedChanges(sender, pre)

            If pre = True Then
                Exit Sub
            End If

            Dim frm As New SpecificationsForm

            frm.lblFormulaName.Text = Me.Name

            frm.chkCalculate.Checked = chkCalculate.Checked

            If chkDisplayBox.Checked Then
                frm.lblIsBox.Text = 1
            Else
                frm.lblIsBox.Text = 0
            End If

            If ComboBox3.Text = "Stick Packs" Or ComboBox3.Text = "Sachets" Then
                If chkDisplayBag.Checked Then
                    frm.chkDisplayBag.Checked = True
                Else
                    frm.chkDisplayBag.Checked = False
                End If
            End If

            frm.lblServingsContainer.Text = Label28.Text
            frm.lblCapsuleType.Text = ComboBox5.Text
            frm.lblCapsuleColor.Text = txtCapsuleColor.Text

            frm.lblVersionNumber.Text = If(String.IsNullOrEmpty(VersionCmbBox.Text), "", VersionCmbBox.Text)
            frm.lblVersionDescription.Text = If(String.IsNullOrEmpty(VersionDescriptionTxt.Text), "", VersionDescriptionTxt.Text)
            With DataGridView1
                For j As Integer = 0 To .RowCount - 1

                    Dim ing As String = .Rows(j).Cells(1).Value.ToString
                    Dim lc As Double = 0
                    If Not IsDBNull(.Rows(j).Cells(6).Value) Then
                        lc = Format(.Rows(j).Cells(6).Value, "N3")
                    End If

                    ing = ing.Trim
                    ing = StrConv(ing, VbStrConv.ProperCase)

                    frm.DataGridView1.Rows.Add(.Rows(j).Cells(0).Value, ing, .Rows(j).Cells(3).Value, .Rows(j).Cells(4).Value, .Rows(j).Cells(5).Value, lc)
                Next
            End With

            frm.Tag = Me.Name
            frm.FormulaID.Text = FormulaID.Text
            frm.Text += " for " & Name

            If FormulaTypeCmbBox.Text = "Capsule" Then
                frm.Label12.Visible = True
                frm.Label13.Visible = True
                frm.Label13.Text = ComboBox5.Text
            End If
            frm.PackagingFormat = ComboBox3.SelectedIndex
            frm.FormulaType.Text = FormulaTypeCmbBox.Text
            frm.ServingSizeTextBox.Text = Me.ServingSizeTextBox.Text
            frm.ServingSizeUOMLabel.Text = Me.ServingSizeUOMLabel.Text
            frm.lblCustomerName.Text = Me.txtCustName.Text
            frm.lblContact.Text = Me.txtContact.Text
            frm.lblSalesRep.Text = Me.txtSalesRepId.Text
            frm.ServingWeightLabel.Text = Me.ServingWeightLabel.Text
            frm.FormulaDT = dsFF.Tables("formulaF")
            If ComboBox3.Text <> "Bags" Then
                frm.chkDisplayBox.Checked = Me.chkDisplayBox.Checked
            End If

            'frm.Label1.Text = Me.Label8.Text
            frm.Label11.Text = ComboBox3.Text

            'For j As Integer = 0 To gridBulkSales.Rows.Count - 1
            '    frm.gridBulkSales.Rows.Add(gridBulkSales.Rows(j).Cells(0).Value, gridBulkSales.Rows(j).Cells(2).Value)
            'Next

            If ComboBox3.Text = "Bulk" Then
                frm.BulkPanel.Visible = True
            ElseIf ComboBox3.Text = "Sachets" Then
                'For j As Integer = 0 To gridBoxSales.Rows.Count - 1
                '    frm.gridBoxSales.Rows.Add(gridBoxSales.Rows(j).Cells(0).Value, gridBoxSales.Rows(j).Cells(2).Value)
                'Next
                'For j As Integer = 0 To gridbBulkSalesBags.Rows.Count - 1
                '    frm.gridbBulkSalesBags.Rows.Add(gridbBulkSalesBags.Rows(j).Cells(0).Value, gridbBulkSalesBags.Rows(j).Cells(2).Value)
                'Next
                'Dim ctrl As Control
                'For Each ctrl In frm.PacketsPanel.Controls
                '    If (ctrl.GetType() Is GetType(TextBox)) Or (ctrl.GetType() Is GetType(ComboBox)) Then
                '        For Each cmbbx1 As Control In SachetsPanel.Controls
                '            If ctrl.Name = cmbbx1.Name Then
                '                ctrl.Text = cmbbx1.Text
                '            End If
                '        Next

                '    End If
                'Next
                Dim allSourceControls As New List(Of Control)
                For Each ctrl As Control In SachetsPanel.Controls
                    allSourceControls.Add(ctrl)
                    If TypeOf ctrl Is GroupBox Then
                        For Each innerCtrl As Control In ctrl.Controls
                            allSourceControls.Add(innerCtrl)
                        Next
                    End If
                Next

                ' Now loop through target controls in frm.StickPackPanel
                For Each ctrl As Control In frm.PacketsPanel.Controls
                    If TypeOf ctrl Is TextBox OrElse TypeOf ctrl Is ComboBox Then
                        For Each matchCtrl As Control In allSourceControls
                            If ctrl.Tag IsNot Nothing AndAlso matchCtrl.Tag IsNot Nothing Then
                                If ctrl.Tag.ToString() = matchCtrl.Tag.ToString() Then
                                    ctrl.Text = matchCtrl.Text
                                End If
                            End If
                        Next

                    ElseIf TypeOf ctrl Is GroupBox Then
                        For Each childCtrl As Control In ctrl.Controls
                            If TypeOf childCtrl Is TextBox OrElse TypeOf childCtrl Is ComboBox Then
                                For Each matchCtrl As Control In allSourceControls
                                    If childCtrl.Tag IsNot Nothing AndAlso matchCtrl.Tag IsNot Nothing Then
                                        If childCtrl.Tag.ToString() = matchCtrl.Tag.ToString() Then
                                            childCtrl.Text = matchCtrl.Text
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    End If
                Next
                frm.PacketsPanel.Visible = True
            ElseIf ComboBox3.Text = "Stick Packs" Then
                'For j As Integer = 0 To gridBoxSales.Rows.Count - 1
                '    frm.gridBoxSales.Rows.Add(gridBoxSales.Rows(j).Cells(0).Value, gridBoxSales.Rows(j).Cells(2).Value)
                'Next
                'For j As Integer = 0 To gridbBulkSalesBags.Rows.Count - 1
                '    frm.gridbBulkSalesBags.Rows.Add(gridbBulkSalesBags.Rows(j).Cells(0).Value, gridbBulkSalesBags.Rows(j).Cells(2).Value)
                'Next
                Dim allSourceControls As New List(Of Control)
                For Each ctrl As Control In StickPacksPanel.Controls
                    allSourceControls.Add(ctrl)
                    If TypeOf ctrl Is GroupBox Then
                        For Each innerCtrl As Control In ctrl.Controls
                            allSourceControls.Add(innerCtrl)
                        Next
                    End If
                Next

                ' Now loop through target controls in frm.StickPackPanel
                For Each ctrl As Control In frm.StickPackPanel.Controls

                    If TypeOf ctrl Is TextBox OrElse TypeOf ctrl Is ComboBox Then

                        For Each matchCtrl As Control In allSourceControls

                            If ctrl.Tag IsNot Nothing AndAlso matchCtrl.Tag IsNot Nothing Then

                                If ctrl.Tag.ToString() = matchCtrl.Tag.ToString() Then

                                    ctrl.Text = matchCtrl.Text

                                End If

                            End If

                        Next

                    ElseIf TypeOf ctrl Is GroupBox Then

                        For Each childCtrl As Control In ctrl.Controls

                            If TypeOf childCtrl Is TextBox OrElse TypeOf childCtrl Is ComboBox Then

                                For Each lstmatchCtrl As Control In allSourceControls.Where(Function(c) TypeOf c Is GroupBox)

                                    For Each matchCtrl As Control In lstmatchCtrl.Controls

                                        If childCtrl.Tag IsNot Nothing AndAlso matchCtrl.Tag IsNot Nothing Then

                                            If childCtrl.Tag.ToString() = matchCtrl.Tag.ToString() Then

                                                childCtrl.Text = matchCtrl.Text

                                            End If

                                        End If

                                    Next

                                Next

                            End If

                        Next

                    End If

                Next

                frm.StickPackPanel.Visible = True
            ElseIf ComboBox3.Text = "Bottles" Then

                ' MessageBox.Show(frm.dsSpec.Tables.Count)

                Dim dt_copy As New DataTable
                dt_copy = dsPB.Tables("bottles").Copy
                frm.dsSpec.Tables.Add(dt_copy)

                'Dim dt_copy1 As New DataTable
                'dt_copy1 = dsSal.Tables("salestxns").Copy
                'frm.dsSpec.Tables.Add(dt_copy1)

                For i = 0 To ComboBox4.Items.Count - 1
                    frm.ComboBox4.Items.Add(ComboBox4.Items(i).ToString)
                Next
                frm.ComboBox4.SelectedIndex = ComboBox4.SelectedIndex
                frm.BottlePanel.Visible = True
            ElseIf ComboBox3.Text = "Bags" Then
                Dim ctrl As Control
                For Each ctrl In frm.BagsPanel.Controls
                    If (ctrl.GetType() Is GetType(TextBox)) Or (ctrl.GetType() Is GetType(ComboBox)) Then
                        For Each cmbbx1 As Control In BagsPanel.Controls
                            If ctrl.Tag = cmbbx1.Tag Then
                                ctrl.Text = cmbbx1.Text
                            End If
                        Next

                    End If
                Next
                frm.BagsPanel.Visible = True
            ElseIf ComboBox3.Text = "Blister" Then
                'For j As Integer = 0 To gridBoxSales.Rows.Count - 1
                '    frm.gridBoxSales.Rows.Add(gridBoxSales.Rows(j).Cells(0).Value, gridBoxSales.Rows(j).Cells(2).Value)
                'Next
                'For j As Integer = 0 To gridbBulkSalesBags.Rows.Count - 1
                '    frm.gridbBulkSalesBags.Rows.Add(gridbBulkSalesBags.Rows(j).Cells(0).Value, gridbBulkSalesBags.Rows(j).Cells(2).Value)
                'Next
                Dim ctrl As Control
                Dim allSourceControls As New List(Of Control)
                For Each ctrl2 As Control In BlistersPanel.Controls
                    allSourceControls.Add(ctrl2)
                    If TypeOf ctrl2 Is GroupBox Then
                        For Each innerCtrl As Control In ctrl2.Controls
                            allSourceControls.Add(innerCtrl)
                        Next
                    End If
                Next
                For Each ctrl In frm.BlisterPanel.Controls
                    If (ctrl.GetType() Is GetType(TextBox)) Or (ctrl.GetType() Is GetType(ComboBox)) Then
                        For Each cmbbx1 As Control In BlistersPanel.Controls
                            If ctrl.Tag = cmbbx1.Tag Then
                                ctrl.Text = cmbbx1.Text
                            End If
                        Next
                    ElseIf TypeOf ctrl Is GroupBox Then

                        For Each childCtrl As Control In ctrl.Controls
                            If TypeOf childCtrl Is TextBox OrElse TypeOf childCtrl Is ComboBox Then
                                For Each matchCtrl As Control In allSourceControls
                                    If childCtrl.Tag IsNot Nothing AndAlso matchCtrl.Tag IsNot Nothing Then
                                        If childCtrl.Tag.ToString() = matchCtrl.Tag.ToString() Then
                                            childCtrl.Text = matchCtrl.Text
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    End If

                Next
                frm.BlisterPanel.Visible = True
            End If

            Dim BulkSales As New DataTable
            BulkSales = dsSal.Tables("salestxns").Copy
            frm.dsSpec.Tables.Add(BulkSales)

            Dim BoxSales As New DataTable
            BoxSales = dsSalBox.Tables("salesBoxTxns").Copy
            frm.dsSpec.Tables.Add(BoxSales)

            Dim BagSales As New DataTable
            BagSales = dsSalBags.Tables("salesBagsTxns").Copy
            frm.dsSpec.Tables.Add(BagSales)

            frm.ShowDialog()

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            If e.RowIndex <> -1 AndAlso e.ColumnIndex = 9 Then
                If e.ColumnIndex = 9 Then
                    If DataGridView1.Rows(e.RowIndex).DataBoundItem("IsManual") = 1 Then
                        DataGridView1.Rows(e.RowIndex).DataBoundItem("IsManual") = 0
                        If Not IsDBNull(DataGridView1.Rows(e.RowIndex).DataBoundItem("LatestCost")) Then
                            If DataGridView1.Rows(e.RowIndex).DataBoundItem("LatestCost") <> 0 Then
                                If Not IsDBNull(DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")) Then
                                    DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("LatestCost") + DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")
                                Else
                                    DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("LatestCost")
                                End If
                            Else
                                DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                            End If
                        Else
                            DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                        End If
                    Else
                        DataGridView1.Rows(e.RowIndex).DataBoundItem("IsManual") = 1

                        If Not IsDBNull(DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost")) Then
                            If DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost") <> 0 Then
                                If Not IsDBNull(DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")) Then
                                    DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost") + DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")
                                Else
                                    DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost")
                                End If
                            Else
                                DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                            End If
                        Else
                            DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                        End If
                    End If
                    DisplaySelectedCostValue(e.RowIndex)
                    DisplayCostType(e.RowIndex)
                    DisplayCostColor(e.RowIndex)
                    FormulaCost(e.RowIndex)
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub ColorCode()
        Try
            Dim Ind As Integer
            DataGridView1.Rows(Ind).Cells(8).Style.BackColor = Color.Green
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub TextBox37_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox37.KeyDown
        Try
            If Not String.IsNullOrEmpty(TextBox37.Text) Then
                If e.KeyCode = Keys.Enter Then

                    If MaterialDataGrid.Rows.Count > 0 Then
                        Dim MaterialName = MaterialDataGrid.CurrentRow.DataBoundItem("MaterialName")
                        Dim Supplier = MaterialDataGrid.CurrentRow.DataBoundItem("Supplier")
                        If IsReplace Then
                            ReplaceMaterial(MaterialName, Supplier)
                        Else
                            AddNewFormulaRow(MaterialName, Supplier)
                        End If
                    Else
                        If IsReplace Then
                            ReplaceMaterial(TextBox37.Text, "")
                        Else
                            AddNewFormulaRow(TextBox37.Text)
                        End If

                    End If


                    TextBox37.Clear()
                    TextBox37.Focus()
                ElseIf e.KeyCode = Keys.Down Then
                    If MaterialDataGrid.Rows.Count > 0 Then
                        MaterialDataGrid.Focus()
                    End If
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        NewMaterial.ShowDialog()
    End Sub

    Private Sub ReplaceMaterialToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReplaceMaterialToolStripMenuItem.Click
        Try
            Dim dv As New DataView()

            MaterialDataGrid.Visible = True
            PopulateAllMaterial(dv)
            IsReplace = True
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub AddNewMaterialToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNewMaterialToolStripMenuItem.Click
        Dim dv As New DataView()
        Try
            MaterialDataGrid.Visible = True
            PopulateAllMaterial(dv)
            IsNewMaterialBelow = True
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub UpdateCostToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateCostToolStripMenuItem.Click
        Try
            Dim mn As String = DataGridView1.CurrentRow.DataBoundItem("MaterialName").ToString
            Dim sp As String = DataGridView1.CurrentRow.DataBoundItem("VendorName").ToString
            Dim price As Double = DataGridView1.CurrentRow.Cells(8).Value.ToString


            Dim result As DialogResult = MessageBox.Show("Confirm you want to update this item", "Confirm", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then

                'Updates item
                DbCon = New MySqlConnection(My.Settings.DBCon)
                Try
                    DbCon.Open()
                    Dim command As MySqlCommand = New MySqlCommand("UPDATE Material SET 
                MaterialName ='" & mn & "' 
                , Supplier ='" & sp & "'
                , Price =" & price & "
                , LatestDate ='" & Date.Now.Date.ToString("MM-dd-yyyy") & "'
                , IsManual= 1 
                WHERE MaterialName ='" & mn & "'", DbCon)
                    command.ExecuteNonQuery()
                    command.Dispose()
                Catch ex As Exception
                    MsgBox("Can not open connection ! ", MessageBoxButtons.OK, "Error")
                End Try

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    Private Sub DataGridView1_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles DataGridView1.MouseDown
        Try
            If e.Button = MouseButtons.Right Then
                'If Dashboard.UserIDLabel.Text > 2 Then
                '    Exit Sub
                'End If

                Dim endPoint As Point = CType(sender, Control).PointToScreen(New Point(e.X, e.Y))
                ContextMenuStrip1.Show(Me, endPoint)
            End If
            If MaterialDataGrid.Visible Or SalesRepList.Visible Then
                MaterialDataGrid.Visible = False
                SalesRepList.Visible = False
                IsNewMaterialBelow = False
                IsReplace = False
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Try
            If e.ColumnIndex = 0 Then
                DataGridView1.CurrentCell.ReadOnly = False
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        Try
            If e.ColumnIndex = 0 Then
                Dim OldInd As Integer = (e.RowIndex) + 1
                Dim NewInd As Integer
                If DataGridView1.CurrentCell.Value > DataGridView1.Rows.Count Then
                    NewInd = DataGridView1.Rows.Count
                Else
                    NewInd = DataGridView1.CurrentCell.Value
                End If
                DataGridView1.Rows(e.RowIndex).DataBoundItem("IngredientIndex") = NewInd
                Dim txnid As String = DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialName")

                For Each r As DataRow In dsF.Tables("formula").Rows
                    If OldInd > NewInd Then
                        'subir
                        If r.Item("IngredientIndex") = NewInd Then
                            If r.Item("MaterialName") <> txnid Then
                                r.Item("IngredientIndex") = NewInd + 1
                            End If
                        ElseIf r.Item("IngredientIndex") > NewInd Then
                            If r.Item("IngredientIndex") < OldInd Then
                                r.Item("IngredientIndex") = r.Item("IngredientIndex") + 1
                            End If
                        End If
                    Else
                        'bajar
                        'if newval is higher than old
                        If r.Item("IngredientIndex") = NewInd Then
                            If r.Item("MaterialName") <> txnid Then
                                r.Item("IngredientIndex") = NewInd - 1
                            End If
                        ElseIf r.Item("IngredientIndex") < NewInd Then
                            If r.Item("IngredientIndex") > OldInd Then
                                r.Item("IngredientIndex") = r.Item("IngredientIndex") - 1
                            End If
                        End If
                    End If
                Next
            End If

            If e.ColumnIndex = 1 Then
                DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialName") = DataGridView1.CurrentCell.Value
            End If

            If e.ColumnIndex = 2 Then
                DataGridView1.Rows(e.RowIndex).DataBoundItem("VendorName") = DataGridView1.CurrentCell.Value
            End If

            If e.ColumnIndex = 7 Then
                DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight") = DataGridView1.CurrentCell.Value
                If DataGridView1.Rows(e.RowIndex).DataBoundItem("IsManual") = 1 Then
                    If Not IsDBNull(DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost")) Then
                        If DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost") <> 0 Then
                            If Not IsDBNull(DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")) Then
                                DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost") + DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")
                            Else
                                DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost")
                            End If
                        Else
                            DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                        End If
                    Else
                        DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                    End If
                Else
                    If Not IsDBNull(DataGridView1.Rows(e.RowIndex).DataBoundItem("LatestCost")) Then
                        If DataGridView1.Rows(e.RowIndex).DataBoundItem("LatestCost") <> 0 Then
                            If Not IsDBNull(DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")) Then
                                DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("LatestCost") + DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")
                            Else
                                DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("LatestCost")
                            End If
                        Else
                            DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                        End If
                    Else
                        DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                    End If
                End If
                FormulaCost(e.RowIndex)
            End If

            If e.ColumnIndex = 8 Then
                If String.IsNullOrEmpty(DataGridView1.CurrentCell.Value) Then
                    DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost") = DBNull.Value
                    DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                Else
                    DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost") = DataGridView1.CurrentCell.Value

                    If DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost") <> 0 Then
                        If Not IsDBNull(DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")) Then
                            DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost") + DataGridView1.Rows(e.RowIndex).DataBoundItem("Freight")
                        Else
                            DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCost")
                        End If
                    Else
                        DataGridView1.Rows(e.RowIndex).DataBoundItem("MaterialCost") = 0
                    End If
                End If

                DataGridView1.Rows(e.RowIndex).DataBoundItem("IsManual") = 1
                DataGridView1.Rows(e.RowIndex).DataBoundItem("ManualCostDate") = Now.Date

            End If
            If e.ColumnIndex = 3 Or e.ColumnIndex = 4 Or e.ColumnIndex = 5 Then
                FormulaCalculations(e.RowIndex)
                FormulaCost(e.RowIndex)
            End If
            If e.ColumnIndex = 6 Then
                DataGridView1.Rows(e.RowIndex).DataBoundItem("mg") = DataGridView1.CurrentCell.Value
                TempMG = DataGridView1.CurrentCell.Value
                MGIndex = e.RowIndex
                FormulaCost(e.RowIndex)
            End If

            DisplaySelectedCostValue(e.RowIndex)
            DisplayCostType(e.RowIndex)
            DisplayCostColor(e.RowIndex)
            FormulaCost(e.RowIndex)

            'BindControls()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


#Region "Sales Panel"
    Public Sub CalculateSales(ind As Integer, unitCost As Double, dataGrid As DataGridView, Optional AditionalCharges As Double = 0)
        Try
            If dataGrid.DataSource.Count > 0 Then
                Dim qty As Double
                Dim margin As Double
                Dim SP As Double
                Dim TotalUnitCost As Double = unitCost
                Dim IsUpdated As Integer



                If IsDBNull(dataGrid.Rows(ind).DataBoundItem("Quantity")) Then
                    dataGrid.Rows(ind).DataBoundItem("MarginPercentage") = 0
                    dataGrid.Rows(ind).DataBoundItem("SalesPrice") = 0
                    dataGrid.Rows(ind).Cells(3).Value = 0
                    dataGrid.Rows(ind).Cells(4).Value = 0
                    Exit Sub
                End If

                If Not IsDBNull(dataGrid.Rows(ind).DataBoundItem("Quantity")) Then
                    qty = dataGrid.Rows(ind).DataBoundItem("Quantity")
                End If
                If Not IsDBNull(dataGrid.Rows(ind).DataBoundItem("MarginPercentage")) Then
                    margin = dataGrid.Rows(ind).DataBoundItem("MarginPercentage")
                End If
                If Not IsDBNull(dataGrid.Rows(ind).DataBoundItem("Overridden")) Then
                    IsUpdated = dataGrid.Rows(ind).DataBoundItem("Overridden")
                End If

                Dim markup As Double
                markup = 1 - (((1 * margin)) / 100)

                'Additional Cost Calculation directly in Sales grid
                If dataGrid.Name = "gridBoxSales" Then
                    If lblAdditionalChargesBox.Text <> 0 Then
                        unitCost -= lblAdditionalChargesBox.Text
                    End If
                ElseIf dataGrid.Name = "gridBulkSales" Then
                    If lblAdditionalChargesBulk.Text <> 0 Then
                        unitCost -= lblAdditionalChargesBulk.Text
                    End If
                ElseIf dataGrid.Name = "gridbBulkSalesBags" Then
                    If lblAdditionalChargeBag.Text <> 0 Then
                        unitCost -= lblAdditionalChargeBag.Text
                    End If
                End If
                SP = Math.Round(unitCost / markup, 2)

                'Additional Cost Calculation directly in Sales grid
                If dataGrid.Name = "gridBoxSales" Then
                    If lblAdditionalChargesBox.Text <> 0 Then
                        SP += lblAdditionalChargesBox.Text
                    End If
                ElseIf dataGrid.Name = "gridBulkSales" Then
                    If lblAdditionalChargesBulk.Text <> 0 Then
                        SP += lblAdditionalChargesBulk.Text
                    End If
                ElseIf dataGrid.Name = "gridbBulkSalesBags" Then
                    If lblAdditionalChargeBag.Text <> 0 Then
                        SP += lblAdditionalChargeBag.Text
                    End If
                End If

                dataGrid.Rows(ind).DataBoundItem("SalesPrice") = SP

                dataGrid.Rows(ind).Cells(2).Value = SP

                If IsUpdated = 0 Then
                    ''Assigning values in OveriddenColumn
                    'If dataGrid.Name = "gridBulkSales" Then
                    '    If dsSal.Tables("salestxns").Rows.Count > 0 Then
                    '        If dsSal.Tables("salestxns").Rows(ind).RowState <> DataRowState.Deleted Then
                    '            dsSal.Tables("salestxns").Rows(ind)("OverriddenSalesPrice") = SP
                    '        End If
                    '    End If
                    'ElseIf dataGrid.Name = "gridBoxSales" Then
                    '    If dsSalBox.Tables("salesBoxTxns").Rows.Count > 0 Then
                    '        If dsSal.Tables("salestxns").Rows(ind).RowState <> DataRowState.Deleted Then
                    '            dsSalBox.Tables("salesBoxTxns").Rows(ind)("OverriddenSalesPrice") = SP
                    '        End If
                    '    End If
                    'ElseIf dataGrid.Name = "gridbBulkSalesBags" Then
                    '    If dsSalBags.Tables("salesBagsTxns").Rows.Count > 0 Then
                    '        If dsSal.Tables("salestxns").Rows(ind).RowState <> DataRowState.Deleted Then
                    '            dsSalBags.Tables("salesBagsTxns").Rows(ind)("OverriddenSalesPrice") = SP
                    '        End If
                    '    End If
                    'End If
                    dataGrid.Rows(ind).DataBoundItem("OverriddenSalesPrice") = SP
                End If

                'Dim Up As Double = SP - unitCost

                Dim Up As Double = SP - TotalUnitCost

                dataGrid.Rows(ind).Cells(3).Value = Math.Round(Up, 2)

                'Dim UpEx As Double = SP - unitCost
                dataGrid.Rows(ind).Cells(4).Value = Math.Round(Up, 2) * Math.Round(dataGrid.Rows(ind).DataBoundItem("Quantity"), 2)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView3_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridBulkSales.EditingControlShowing, gridbBulkSalesBags.EditingControlShowing, gridBoxSales.EditingControlShowing
        Try
            If gridBulkSales.CurrentCell.ColumnIndex = 0 Then
                AddHandler CType(e.Control, TextBox).KeyPress, AddressOf TextBox_keyPress
            ElseIf gridBulkSales.CurrentCell.ColumnIndex = 1 Then
                AddHandler CType(e.Control, TextBox).KeyPress, AddressOf TextBox_keyPress1
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub TextBox_keyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If Not (Char.IsDigit(CChar(CStr(e.KeyChar))) Or e.KeyChar = Convert.ToChar(8)) Then e.Handled = True
    End Sub
    Private Sub TextBox_keyPress1(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If Not (Char.IsDigit(CChar(CStr(e.KeyChar))) Or e.KeyChar = "." Or e.KeyChar = Convert.ToChar(8)) Then e.Handled = True
    End Sub
    Private Sub DataGridView3_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles gridBulkSales.CellEndEdit
        Try
            If e.ColumnIndex = 0 Or e.ColumnIndex = 1 Then
                CalculateBulkSales(gridBulkSales)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub TotalUnitCost_TextChanged(sender As Object, e As EventArgs) Handles TotalUnitCost.TextChanged
        Try
            TotalUnitCost.Text = Math.Round(Double.Parse(If(String.IsNullOrEmpty(TotalUnitCost.Text), "0", TotalUnitCost.Text)), 2).ToString("F2")
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

#End Region
    Private Sub DisplaySales()
        Try
            'displays Sales Data
            If ComboBox3.SelectedIndex = 0 Then
                'SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " AND UnitSize = 0 AND VersionNumber=" & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)
                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " AND VersionNumber=" & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)

            ElseIf ComboBox3.SelectedIndex = 1 Then
                If ComboBox4.SelectedIndex <> -1 Then
                    SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " And UnitSize = " & ComboBox4.Text & " And VersionNumber=" & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)
                End If
            ElseIf ComboBox3.SelectedIndex = 2 Or ComboBox3.SelectedIndex = 3 Or ComboBox3.SelectedIndex = 4 Or ComboBox3.SelectedIndex = 5 Then
                'SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & ServingSizeTextBox.Text & " And UnitSize = 0 And VersionNumber=" & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)
                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " And VersionNumber=" & If(String.IsNullOrEmpty(VersionCmbBox.Text), "0", VersionCmbBox.Text), "", DataViewRowState.CurrentRows)

            End If

            gridBulkSales.AutoGenerateColumns = False

            CalculateBulkSales(gridBulkSales)
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Function CheckUnsavedChanges(sender As Object, pre1 As Boolean) As Boolean
        Try
            Dim listofds As New List(Of DataSet)
            listofds.Add(dsF) 'FormualDetail    
            listofds.Add(dsFF) 'FormulaInfo

            If FormulaTypeCmbBox.Text = "Powder" Then
                listofds.Add(dsBi)  'Blending
            ElseIf FormulaTypeCmbBox.Text = "Tablet" Then
                listofds.Add(dsTi) 'Tableting
            ElseIf FormulaTypeCmbBox.Text = "Capsule" Then
                listofds.Add(dsEi) 'Encapsulation
            End If

            listofds.Add(dsPB)
            listofds.Add(dsPSP)
            listofds.Add(dsPSa)
            listofds.Add(dsSal)
            listofds.Add(dsSalBox)
            listofds.Add(dsSalBags)
            listofds.Add(dsStandupBags)
            listofds.Add(dsPBlisters)

            For Each ds As DataSet In listofds
                pre1 = Helper.IsSInitialtate(ds)
                If pre1 Then
                    Exit For
                End If

            Next
            If pre1 Then
                Dim response As DialogResult = MessageBox.Show("Save unsaved changes?", "Message", MessageBoxButtons.YesNo)
                If response = DialogResult.Yes Then
                    Saves()
                End If
            End If
            Return pre1
        Catch ex As Exception
            Helper.WriteLog(ex)
            Return False
        End Try

    End Function

    Private Sub Formulator2_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            Dim pre As Boolean
            CheckUnsavedChanges(sender, pre)
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub fr2_passvalue(text As String) Handles fr2.passvalue
        Try
            Dim currend As Double = text
            For j As Integer = 0 To DataGridView1.Rows.Count - 1
                DataGridView1.Rows(j).DataBoundItem("Freight") = currend
                If DataGridView1.Rows(j).DataBoundItem("IsManual") = 1 Then
                    If Not IsDBNull(DataGridView1.Rows(j).DataBoundItem("ManualCost")) Then
                        If DataGridView1.Rows(j).DataBoundItem("ManualCost") <> 0 Then
                            If Not IsDBNull(DataGridView1.Rows(j).DataBoundItem("Freight")) Then
                                DataGridView1.Rows(j).DataBoundItem("MaterialCost") = DataGridView1.Rows(j).DataBoundItem("ManualCost") + DataGridView1.Rows(j).DataBoundItem("Freight")
                            Else
                                DataGridView1.Rows(j).DataBoundItem("MaterialCost") = DataGridView1.Rows(j).DataBoundItem("ManualCost")
                            End If
                        Else
                            DataGridView1.Rows(j).DataBoundItem("MaterialCost") = 0
                        End If
                    Else
                        DataGridView1.Rows(j).DataBoundItem("MaterialCost") = 0
                    End If
                Else
                    If Not IsDBNull(DataGridView1.Rows(j).DataBoundItem("LatestCost")) Then
                        If DataGridView1.Rows(j).DataBoundItem("LatestCost") <> 0 Then
                            If Not IsDBNull(DataGridView1.Rows(j).DataBoundItem("Freight")) Then
                                DataGridView1.Rows(j).DataBoundItem("MaterialCost") = DataGridView1.Rows(j).DataBoundItem("LatestCost") + DataGridView1.Rows(j).DataBoundItem("Freight")
                            Else
                                DataGridView1.Rows(j).DataBoundItem("MaterialCost") = DataGridView1.Rows(j).DataBoundItem("LatestCost")
                            End If
                        Else
                            DataGridView1.Rows(j).DataBoundItem("MaterialCost") = 0
                        End If
                    Else
                        DataGridView1.Rows(j).DataBoundItem("MaterialCost") = 0
                    End If
                End If
                FormulaCost(j)
            Next
            DataGridView1.Refresh()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub UpdateFreightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateFreightToolStripMenuItem.Click
        Try
            fr2 = New UpdateFreightCost()
            fr2.ShowDialog()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub lookupicon_Click(sender As Object, e As EventArgs) Handles lookupicon.Click
        Try
            If MaterialDataGrid.Visible = True Then
                MaterialDataGrid.Visible = False
                IsNewMaterialBelow = False
                IsReplace = False
                Exit Sub
            End If
            If SalesRepList.Visible = True Then
                SalesRepList.Visible = False
            End If

            Dim dv As New DataView
            If String.IsNullOrEmpty(TextBox37.Text) Then
                PopulateAllMaterial(dv)
            Else
                PopulateFilteredMaterial(dv)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Public Sub PopulateAllMaterial(dv As DataView)
        Try
            dv.Table = ds.Tables("ingredients")
            MaterialDataGrid.DataSource = dv
            For Each dataColumn As DataGridViewColumn In MaterialDataGrid.Columns
                If dataColumn.Name <> "MaterialName" And dataColumn.Name <> "Supplier" And dataColumn.Name <> "Price" And dataColumn.Name <> "PriceUpdateDate" Then
                    dataColumn.Visible = False
                End If
            Next
            If dv.Count > 0 Then
                MaterialDataGrid.Visible = True
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Public Sub PopulateFilteredMaterial(dv As DataView)
        Try
            Dim FilteredText As String = TextBox37.Text
            Dim FilteredMaterialDataTable As DataTable = ds.Tables("ingredients").Clone()
            ' Filter rows based on MaterialName
            Dim FilteredRows = From row In ds.Tables("ingredients").AsEnumerable()
                               Where row.Field(Of String)("MaterialName").StartsWith(FilteredText, StringComparison.OrdinalIgnoreCase)

            ' Add filtered data to the new DataTable
            For Each row As DataRow In FilteredRows
                FilteredMaterialDataTable.ImportRow(row)
            Next

            dv.Table = FilteredMaterialDataTable
            MaterialDataGrid.DataSource = dv

            For Each dataColumn As DataGridViewColumn In MaterialDataGrid.Columns
                If dataColumn.Name <> "MaterialName" And dataColumn.Name <> "Supplier" And dataColumn.Name <> "Price" And dataColumn.Name <> "PriceUpdateDate" Then
                    dataColumn.Visible = False
                End If
            Next
            If dv.Count > 0 Then
                MaterialDataGrid.Visible = True
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub ComboBox3_TextChanged(sender As Object, e As EventArgs) Handles ComboBox3.TextChanged
        'txtServing.Text = ComboBox4.Text
        If String.IsNullOrEmpty(ComboBox4.Text) Or String.IsNullOrEmpty(ServingSizeTextBox.Text) Then
            Exit Sub
        End If
    End Sub
    Private Sub txtServing_TextChanged(sender As Object, e As EventArgs) Handles txtServing.TextChanged, ServingSizeTextBox.TextChanged
        Try
            Dim serving = If(String.IsNullOrEmpty(txtServing.Text), 0, txtServing.Text)
            Dim servingsize = If(String.IsNullOrEmpty(ServingSizeTextBox.Text), 0, ServingSizeTextBox.Text)
            txtFillWeight.Text = Double.Parse(serving) * Double.Parse(servingsize)
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub txtBox_KeyPress_Number(sender As Object, e As KeyPressEventArgs)
        Try
            Dim Ctl As Control = CType(sender, Control)
            If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) Then
                e.Handled = True
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub txtBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        Try
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

    Private Sub txtBox_LostFocus(sender As Object, e As EventArgs)
        FormatTextBox(sender, e)
    End Sub

    Private Sub FormatTextBox(sender As Object, e As EventArgs)
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

    Private Sub FormulaTypeCmbBox_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles FormulaTypeCmbBox.SelectedIndexChanged
        Try
            If FormulaTypeCmbBox.SelectedIndex = 0 Then
                AddHandler ServingSizeTextBox.KeyPress, AddressOf txtBox_KeyPress_Number

                'Adding Decimal To Textbox 35 and Textbox 16
                AddHandler TextBox35.TextChanged, AddressOf txtBox_TextChanged
                AddHandler TextBox35.KeyPress, AddressOf txtBox_KeyPress
                AddHandler TextBox35.LostFocus, AddressOf txtBox_LostFocus

                AddHandler TextBox16.TextChanged, AddressOf txtBox_TextChanged
                AddHandler TextBox16.KeyPress, AddressOf txtBox_KeyPress
                AddHandler TextBox16.LostFocus, AddressOf txtBox_LostFocus

                'Removing Numbers From Textbox 35 and Textbox 16
                RemoveHandler TextBox35.KeyPress, AddressOf txtBox_KeyPress_Number
                RemoveHandler TextBox16.KeyPress, AddressOf txtBox_KeyPress_Number
            Else
                'Adding Decimal To Textbox 35 and Textbox 16

                RemoveHandler TextBox35.TextChanged, AddressOf txtBox_TextChanged
                RemoveHandler TextBox35.KeyPress, AddressOf txtBox_KeyPress
                RemoveHandler TextBox35.LostFocus, AddressOf txtBox_LostFocus

                RemoveHandler TextBox16.TextChanged, AddressOf txtBox_TextChanged
                RemoveHandler TextBox16.KeyPress, AddressOf txtBox_KeyPress
                RemoveHandler TextBox16.LostFocus, AddressOf txtBox_LostFocus

                'Adding Numbers From Textbox 35 and Textbox 16
                AddHandler TextBox35.KeyPress, AddressOf txtBox_KeyPress_Number
                AddHandler TextBox16.KeyPress, AddressOf txtBox_KeyPress_Number

            End If
            If AddMaterial.Enabled = False Then
                AddMaterial.Enabled = True
            End If
            If FormulaTypeCmbBox.SelectedIndex = 0 Then
                'Blending Info
                AddRemoveBlending(True)
                DisplayBlending(VersionCmbBox.Text)
                Label98.Text = "gram(s)"
                Label51.Text = "gram(s)"
                If Not ComboBox3.Items.Contains("Stick Packs") Then
                    ComboBox3.Items.Add("Stick Packs")
                End If

                If Not ComboBox3.Items.Contains("Bags") Then
                    ComboBox3.Items.Add("Bags")
                End If

                'Change sales grid data for Bulk if Formula is not saved
                If ComboBox3.SelectedIndex = 0 And FormulaID.Text = "-1" Then
                    If dsSal.HasChanges Then
                        dsSal.RejectChanges()
                    End If
                    DisplayBulkSalesData()
                    For j As Integer = 0 To gridBulkSales.Rows.Count - 1
                        CalculateSales(j, TextBox8.Text, gridBulkSales)
                    Next
                End If
                Label27.Text = "Fill Weight:"
                lblBottleFillWeightUnit.Visible = True
            ElseIf FormulaTypeCmbBox.SelectedIndex = 1 Then
                'Encapsulation Info
                AddRemoveEncapsulation(True)
                DisplayEncapsulation(VersionCmbBox.Text)
                Label98.Text = "Capsule(s)"
                Label51.Text = "Capsule(s)"
                ComboBox3.Items.Remove("Stick Packs")
                ComboBox3.Items.Remove("Bags")
                'Change sales grid data for Bulk if Formula is not saved
                If ComboBox3.SelectedIndex = 0 And FormulaID.Text = "-1" Then
                    If dsSal.HasChanges Then
                        dsSal.RejectChanges()
                    End If
                    DisplayBulkSalesData()
                    For j As Integer = 0 To gridBulkSales.Rows.Count - 1
                        CalculateSales(j, TextBox8.Text, gridBulkSales)
                    Next
                End If
                Label27.Text = "Servings/Container:"
                lblBottleFillWeightUnit.Visible = False
            ElseIf FormulaTypeCmbBox.SelectedIndex = 2 Then
                'Tableting Info
                AddRemoveTableting(True)
                DisplayTableting(VersionCmbBox.Text)
                Label98.Text = "Tablet(s)"
                Label51.Text = "Tablet(s)"
                If Not ComboBox3.Items.Contains("Stick Packs") Then
                    ComboBox3.Items.Add("Stick Packs")
                End If

                If Not ComboBox3.Items.Contains("Bags") Then
                    ComboBox3.Items.Add("Bags")
                End If

                'Change sales grid data for Bulk if Formula is not saved
                If ComboBox3.SelectedIndex = 0 And FormulaID.Text = "-1" Then
                    If dsSal.HasChanges Then
                        dsSal.RejectChanges()
                    End If
                    DisplayBulkSalesData()
                    For j As Integer = 0 To gridBulkSales.Rows.Count - 1
                        CalculateSales(j, TextBox8.Text, gridBulkSales)
                    Next
                End If
                Label27.Text = "Servings/Container:"
                lblBottleFillWeightUnit.Visible = False
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub AddMaterial_Click(sender As Object, e As EventArgs)
        Try
            MaterialAdded = False
            NewMaterial.ShowDialog()
            If MaterialAdded Then
                If ds.Tables.Contains("ingredients") Then
                    ds.Tables.Remove("ingredients")
                End If
                Dim MySqlCon As New MySqlConnection()
                Try
                    MySqlCon.ConnectionString = My.Settings.DBCon
                    dbUp.CommandText = "SELECT * FROM Material WHERE Category = 'RM' ORDER BY MaterialName"
                    MySqlCon.Open()
                    da = New MySqlDataAdapter(dbUp.CommandText, DbCon)
                    MySqlCon.Close()
                    da.Fill(ds, "ingredients")
                Catch ex As Exception
                Finally
                    MySqlCon.Close()
                End Try

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub SalesRepLookUp_Click(sender As Object, e As EventArgs)
        Try
            If MaterialDataGrid.Visible = True Then
                MaterialDataGrid.Visible = False
                IsReplace = False
                IsNewMaterialBelow = False
            End If
            Dim dv As New DataView
            If String.IsNullOrEmpty(txtSalesRepId.Text) Then
                dv.Table = ds.Tables("salesreps")
                'Filter based on a text box value selected
                'dv.RowFilter = "MaterialName LIKE '*" & TextBox37.Text & "*'"
                SalesRepList.DisplayMember = "ID"
                SalesRepList.ValueMember = "ID"
                SalesRepList.DataSource = dv
                If dv.Count > 0 Then
                    SalesRepList.Visible = True
                End If
            Else
                'Associate the dataview to dataset(Dataset table)
                dv.Table = ds.Tables("salesreps")
                'Filter based on a text box value selected
                dv.RowFilter = "ID LIKE '*" & txtSalesRepId.Text & "*'"
                SalesRepList.DisplayMember = "ID"
                SalesRepList.ValueMember = "ID"
                SalesRepList.DataSource = dv
                If dv.Count > 0 Then
                    SalesRepList.Visible = True
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub SalesRepList_KeyDown(sender As Object, e As KeyEventArgs) Handles SalesRepList.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                If SalesRepList.SelectedIndex <> -1 Then
                    txtSalesRepId.Text = SalesRepList.Text

                End If
            ElseIf e.KeyCode = Keys.Escape Then

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub txtSalesRepId_TextChanged(sender As Object, e As EventArgs)
        Try
            'Create a dataview
            Dim dv As New DataView
            If String.IsNullOrEmpty(txtSalesRepId.Text) Then
                SalesRepList.Visible = False
            Else
                'Associate the dataview to dataset(Dataset table)
                dv.Table = ds.Tables("salesreps")
                'Filter based on a text box value selected
                dv.RowFilter = "ID LIKE '*" & txtSalesRepId.Text & "*'"
                SalesRepList.DisplayMember = "ID"
                SalesRepList.ValueMember = "ID"
                SalesRepList.DataSource = dv
                If dv.Count > 0 Then
                    SalesRepList.Visible = True
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub SalesRepList_Click(sender As Object, e As EventArgs) Handles SalesRepList.Click
        Try
            If SalesRepList.SelectedIndex <> -1 Then

                SalesRepList.Visible = False
                RemoveHandler txtSalesRepId.TextChanged, AddressOf txtSalesRepId_TextChanged
                RemoveHandler SalesRepLookUp.Click, AddressOf SalesRepLookUp_Click
                txtSalesRepId.Text = SalesRepList.Text
                AddHandler txtSalesRepId.TextChanged, AddressOf txtSalesRepId_TextChanged
                AddHandler SalesRepLookUp.Click, AddressOf SalesRepLookUp_Click

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DataGridView2_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles DataGridView2.CellPainting
        Try
            ' Ensure we are painting only the Material Name column and not the header
            Dim allowedRows As Integer()
            If FormulaTypeCmbBox.Text = "Capsule" Then
                allowedRows = {0, 1}
            Else
                allowedRows = {0, 1, 2}
            End If

            If DataGridView2.Columns(e.ColumnIndex).Name = "Column11" AndAlso allowedRows.Contains(e.RowIndex) Then

                'If e.ColumnIndex = DataGridView2.Columns("Column11").Index AndAlso e.RowIndex >= 0 Then
                e.Handled = True
                e.PaintBackground(e.ClipBounds, True)
                e.PaintContent(e.ClipBounds)

                ' Load the search icon (Ensure you have added it to project Resources)
                Dim searchIcon As Image = My.Resources.search
                ' Use your icon here
                Dim iconSize As Integer = 16 ' Size of the icon
                Dim padding As Integer = 5 ' Padding from right side

                ' Calculate position to draw the icon at the right side of the text
                Dim iconX As Integer = e.CellBounds.Right - iconSize - padding
                Dim iconY As Integer = e.CellBounds.Y + (e.CellBounds.Height - iconSize) / 2

                ' Draw the search icon
                e.Graphics.DrawImage(searchIcon, New Rectangle(iconX, iconY, iconSize, iconSize))
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        Try
            Dim allowedRows As Integer()
            If FormulaTypeCmbBox.Text = "Capsule" Then
                allowedRows = {0, 1}
            Else
                allowedRows = {0, 1, 2}
            End If

            If DataGridView2.Columns(e.ColumnIndex).Name = "Column11" AndAlso allowedRows.Contains(e.RowIndex) Then
                ' Get the bounds of the cell where the icon is drawn
                Dim cellBounds As Rectangle = DataGridView2.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                ' Define the icon's position and size
                Dim iconSize As Integer = 16
                Dim padding As Integer = 5
                Dim iconX As Integer = cellBounds.Right - iconSize - padding
                Dim iconY As Integer = cellBounds.Y + (cellBounds.Height - iconSize) / 2
                Dim iconArea As New Rectangle(iconX, iconY, iconSize, iconSize)
                If DataGridView2.CurrentCell.ColumnIndex = 1 Then
                    Dim RowHeight1 As Integer = DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex).Height
                    Dim CellRectangle1 As Rectangle = DataGridView2.GetCellDisplayRectangle(DataGridView2.CurrentCell.ColumnIndex, DataGridView2.CurrentCell.RowIndex, True)

                    CellRectangle1.X += DataGridView2.Left - 5
                    CellRectangle1.Y += DataGridView2.Top

                    Dim DisplayPoint1 As Point = PointToScreen(New Point(CellRectangle1.X, CellRectangle1.Y))
                    ListBox1.Location = DisplayPoint1

                    DataGridView2.CurrentCell = DataGridView2(1, DataGridView2.CurrentCell.RowIndex)


                    Dim tx As String = DataGridView2.CurrentCell.Value.ToString

                    If (e.RowIndex = 0) Then
                        Dim dv As New DataView
                        dv.Table = ds.Tables("xls_bottlelist")
                        ListBox1.DataSource = Nothing
                        ListBox1.DisplayMember = "MaterialName"
                        ListBox1.ValueMember = "ComponentCode"

                        ListBox1.DataSource = dv
                        If dv.Count > 0 Then
                            ListBox1.Visible = True
                            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                                DataGridView2.CurrentCell = DataGridView2.Rows(e.RowIndex).Cells(e.ColumnIndex)
                                ' Select cell      
                                DataGridView2.BeginEdit(True) ' Start editing modeEnd If
                            End If
                        End If
                    End If
                    If (e.RowIndex = 1) Then
                        Dim dv As New DataView

                        dv.Table = ds.Tables("xls_lidslist")
                        ListBox1.DataSource = Nothing
                        ListBox1.DisplayMember = "MaterialName"
                        ListBox1.ValueMember = "ComponentCode"
                        ListBox1.DataSource = dv
                        If dv.Count > 0 Then
                            ListBox1.Visible = True
                            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                                DataGridView2.CurrentCell = DataGridView2.Rows(e.RowIndex).Cells(e.ColumnIndex)
                                ' Select cell      
                                DataGridView2.BeginEdit(True) ' Start editing modeEnd If
                            End If
                        End If
                    End If
                    If (e.RowIndex = 2) Then
                        Dim dv As New DataView

                        dv.Table = ds.Tables("xls_scooplist")
                        ListBox1.DataSource = Nothing

                        ListBox1.DisplayMember = "ScoopMaterialName"
                        ListBox1.ValueMember = "ComponentCode"
                        ListBox1.DataSource = dv
                        If dv.Count > 0 Then
                            ListBox1.Visible = True
                            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                                DataGridView2.CurrentCell = DataGridView2.Rows(e.RowIndex).Cells(e.ColumnIndex)
                                ' Select cell      
                                DataGridView2.BeginEdit(True) ' Start editing modeEnd If
                            End If
                        End If
                    End If
                    If (e.RowIndex = 7) Then
                        Dim dv As New DataView

                        dv.Table = ds.Tables("xls_shipperlist")
                        ListBox1.DataSource = Nothing
                        ListBox1.DisplayMember = "ShipperMaterialName"
                        ListBox1.ValueMember = "ComponentCode"
                        ListBox1.DataSource = dv
                        If dv.Count > 0 Then
                            ListBox1.Visible = True
                            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                                DataGridView2.CurrentCell = DataGridView2.Rows(e.RowIndex).Cells(e.ColumnIndex)
                                ' Select cell      
                                DataGridView2.BeginEdit(True) ' Start editing modeEnd If
                            End If
                        End If
                    End If
                End If
            Else
                ListBox1.Visible = False
                'AddHandler tb.TextChanged, New EventHandler(AddressOf TextBox_TextChanged)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click
        Try
            Dim table = DirectCast(ListBox1.DataSource, System.Data.DataView).Table.TableName

            Dim findtxn() As DataRow
            If (table = "xls_bottlelist") Then
                Dim drv As DataRowView = CType(ListBox1.SelectedItem, DataRowView)
                Dim selectedValue As String = drv("MaterialName").ToString()
                'Dim selectedItem As String = ListBox1.SelectedItem.ToString()
                DataGridView2.CurrentCell.Value = selectedValue
                findtxn = ds.Tables("xls_bottlelist").Select("MaterialName = '" & selectedValue & "'")
            End If
            If (table = "xls_lidslist") Then
                Dim drv As DataRowView = CType(ListBox1.SelectedItem, DataRowView)
                Dim selectedValue As String = drv("MaterialName").ToString()
                'Dim selectedItem As String = ListBox1.SelectedItem.ToString()
                DataGridView2.CurrentCell.Value = selectedValue
                findtxn = ds.Tables("xls_lidslist").Select("MaterialName = '" & selectedValue & "'")
            End If
            If (table = "xls_scooplist") Then
                Dim drv As DataRowView = CType(ListBox1.SelectedItem, DataRowView)
                Dim selectedValue As String = drv("ScoopMaterialName").ToString()
                'Dim selectedItem As String = ListBox1.SelectedItem.ToString()
                DataGridView2.CurrentCell.Value = selectedValue
                findtxn = ds.Tables("xls_scooplist").Select("ScoopMaterialName = '" & selectedValue & "'")
            End If
            If (table = "xls_shipperlist") Then
                Dim drv As DataRowView = CType(ListBox1.SelectedItem, DataRowView)
                Dim selectedValue As String = drv("ShipperMaterialName").ToString()
                'Dim selectedItem As String = ListBox1.SelectedItem.ToString()
                DataGridView2.CurrentCell.Value = selectedValue
                findtxn = ds.Tables("xls_shipperlist").Select("ShipperMaterialName = '" & selectedValue & "'")
            End If
            If findtxn.Count > 0 Then
                DataGridView2.CurrentRow.DataBoundItem("UnitCost") = findtxn(0).Item("Price")
                DataGridView2.CurrentRow.DataBoundItem("VendorName") = findtxn(0).Item("Supplier")
            Else
                DataGridView2.CurrentRow.DataBoundItem("UnitCost") = DBNull.Value
                DataGridView2.CurrentRow.DataBoundItem("VendorName") = DBNull.Value
            End If
            ListBox1.Visible = False
            CalculateBottlingCost()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub SalesRepList_Leave(sender As Object, e As EventArgs) Handles SalesRepList.Leave
        SalesRepList.Visible = False
    End Sub

    Private Sub ListBox2_Leave(sender As Object, e As EventArgs)
        'ListBox2.Visible = False
    End Sub

    Private Sub Panel2_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel2.MouseDown
        If MaterialDataGrid.Visible Or SalesRepList.Visible Then
            MaterialDataGrid.Visible = False
            SalesRepList.Visible = False
            IsReplace = False
            IsNewMaterialBelow = False
        End If
    End Sub

    Public Shared Sub PopulateMaterial()
        Try
            Dim MaterialView = New DataView
            Dim DbCon As New MySqlConnection
            Dim dbUp As New MySqlCommand
            Dim MySqlDataAdapter As MySqlDataAdapter
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            dbUp.Connection = DbCon
            dbUp.CommandType = CommandType.Text
            dbUp.CommandText = "SELECT * FROM material ORDER BY MaterialName"

            MaterialDataTable = New DataTable
            MaterialDataTable.TableName = "Material List"
            MySqlDataAdapter = New MySqlDataAdapter(dbUp.CommandText, DbCon)
            MySqlDataAdapter.Fill(MaterialDataTable)

            DbCon.Close()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub btnDeleteFormulaVersion_Click(sender As Object, e As EventArgs) Handles btnDeleteFormulaVersion.Click

        Try
            If Not String.IsNullOrEmpty(VersionCmbBox.Text) Then
                Dim response As DialogResult = MessageBox.Show("Delete version " & VersionCmbBox.Text & " for current formula", "Message", MessageBoxButtons.YesNo)
                If response = DialogResult.Yes Then
                    Me.Cursor = Cursors.WaitCursor

                    Dim indexBeforDelete As Int16 = VersionCmbBox.SelectedIndex

                    Dim rowsFV() As DataRow = dsFV.Tables("versions").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsFV
                        row.Delete()
                    Next

                    Dim rowsBld() As DataRow = dsBi.Tables("blending").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsBld
                        row.Delete()
                    Next

                    Dim rowsBlst() As DataRow = dsPBlisters.Tables("blisters").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsBlst
                        row.Delete()
                    Next

                    Dim rowsBtl() As DataRow = dsPB.Tables("bottles").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsBtl
                        row.Delete()
                    Next

                    Dim rowsEnp() As DataRow = dsEi.Tables("encapsulation").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsEnp
                        row.Delete()
                    Next

                    Dim rowsStp() As DataRow = dsPSP.Tables("stickpacks").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsStp
                        row.Delete()
                    Next

                    Dim rowstbl() As DataRow = dsTi.Tables("tableting").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowstbl
                        row.Delete()
                    Next

                    Dim rowsSUB() As DataRow = dsStandupBags.Tables("StandUpBag").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsSUB
                        row.Delete()
                    Next

                    Dim rowsSal() As DataRow = dsSal.Tables("salestxns").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsSal
                        row.Delete()
                    Next

                    Dim rowsBoxSal() As DataRow = dsSalBox.Tables("salesBoxTxns").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsBoxSal
                        row.Delete()
                    Next

                    Dim rowsBagSal() As DataRow = dsSalBags.Tables("salesBagsTxns").Select("VersionNumber=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rowsBagSal
                        row.Delete()
                    Next

                    Dim rows() As DataRow = dsF.Tables("formula").Select("VersionID=" & VersionCmbBox.Text & " AND FormulaID=" & FormulaID.Text)
                    For Each row As DataRow In rows
                        row.Delete()
                    Next

                    Saves(True)

                    'Dim index As Int16 = VersionCmbBox.SelectedIndex
                    'If index <> 0 AndAlso index <> -1 Then
                    'VersionCmbBox.Items.RemoveAt(indexBeforDelete)
                    'End If

                    'If index <> 0 AndAlso VersionCmbBox.Items.Count = index Then
                    '    VersionCmbBox.SelectedIndex = index - 1
                    'ElseIf index <> -1 Then
                    '    VersionCmbBox.SelectedIndex = index
                    'End If

                    MessageBox.Show("Version Deleted Successfully..", "Message", MessageBoxButtons.OK)

                    'Below code is to add a new version when user deletes all existing version from his particuldar formula.
                    If VersionCmbBox.Items.Count = 0 Then
                        NewVersion()
                        BindControls()
                        ServingWeightLabel.Text = "0.00"
                        CostKiloLabel.Text = "0.00"

                        If FormulaTypeCmbBox.Text <> "Powder" And FormulaID.Text = "-1" Then
                            ServingSizeTextBox.Text = 1
                        Else
                            ServingSizeTextBox.Text = 0
                        End If
                        'Saving New version created imidiately after its creation because user can select not to save the formula while clossing the form that will leed to in appropriate working.

                    End If
                    Saves(True)
                    Me.Cursor = Cursors.Arrow
                End If
            End If
        Catch Ex As Exception
            MessageBox.Show("Failed to connect to Database", "Error", MessageBoxButtons.OK)
            Helper.WriteLog(Ex)
        Finally
            'mySqlConnection.Close()
        End Try
    End Sub

    Private Sub DisplaySalesRepData()
        Try
            Dim DbCon As New MySqlConnection
            Dim dbUp As New MySqlCommand
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            dbUp.Connection = DbCon
            dbUp.CommandType = CommandType.Text
            dbUp.CommandText = "SELECT * FROM salesreps where ID ='" + txtSalesRepId.Text + "'"
            Dim dataReader As MySqlDataReader = dbUp.ExecuteReader()
            While dataReader.Read()
                email = dataReader.Item("Email")
                phone = dataReader.Item("Phone")
            End While

        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
            DbCon.Close()
        End Try

    End Sub

    Private Sub CopyBlister(selectedVersion As String)
        Try
            Dim row As DataRow
            Dim dataView As DataView
            'Blister
            dataView = New DataView(dsPBlisters.Tables("blisters"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "BlisterDetailsID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                row = dsPBlisters.Tables("blisters").NewRow
                row("FormulaID") = FormulaID.Text
                row("BlisterFormat") = dataView(0).Item("BlisterFormat")
                row("BlisterSize") = dataView(0).Item("BlisterSize")
                row("Material") = dataView(0).Item("Material")
                row("Quantity") = dataView(0).Item("Quantity")
                row("BlisterCount") = dataView(0).Item("BlisterCount")
                row("BlisterType") = dataView(0).Item("BlisterType")
                row("BlisterAmount") = dataView(0).Item("BlisterAmount")
                row("BlisterCost") = dataView(0).Item("BlisterCost")
                row("PrintingCost") = dataView(0).Item("PrintingCost")
                row("FoldingCartonCost") = dataView(0).Item("FoldingCartonCost")
                row("PrintingPlatesCost") = dataView(0).Item("PrintingPlatesCost")
                row("ShrinkWrapFlag") = dataView(0).Item("ShrinkWrapFlag")
                row("WaferSealsFlag") = dataView(0).Item("WaferSealsFlag")
                row("SecondaryPackoutCost") = dataView(0).Item("SecondaryPackoutCost")
                row("ShrinkWrap") = dataView(0).Item("ShrinkWrap")
                row("WaferSealsCost") = dataView(0).Item("WaferSealsCost")
                row("ArtPreparation") = dataView(0).Item("ArtPreparation")
                row("OtherCost") = dataView(0).Item("OtherCost")
                row("Other_Description") = dataView(0).Item("Other_Description")
                row("ShipperCaseCount") = dataView(0).Item("ShipperCaseCount")
                row("ShipperCaseCost") = dataView(0).Item("ShipperCaseCost")
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                row("PrintingPlatesCostBulk") = dataView(0).Item("PrintingPlatesCostBulk")
                row("ArtPreparationBulk") = dataView(0).Item("ArtPreparationBulk")
                row("OtherCostBulk") = dataView(0).Item("OtherCostBulk")
                row("ToolingCostBulk") = dataView(0).Item("ToolingCostBulk")
                row("ShipperCaseCostBulk") = dataView(0).Item("ShipperCaseCostBulk")
                row("ShipperCaseCountBulk") = dataView(0).Item("ShipperCaseCountBulk")
                row("Other_DescriptionBulk") = dataView(0).Item("Other_DescriptionBulk")
                'Display Box Group ,Quantity_DisplayBox 
                row("Quantity_DisplayBox") = dataView(0).Item("Quantity_DisplayBox")
                row("ToolingCost") = dataView(0).Item("ToolingCost")
                row("Amount_DisplayBox") = dataView(0).Item("Amount_DisplayBox")
                row("Description_DisplayBox") = dataView(0).Item("Description_DisplayBox")
                row("Spec_DisplayBox") = dataView(0).Item("Spec_DisplayBox")
                row("BoardGrade_DisplayBox") = dataView(0).Item("BoardGrade_DisplayBox")
                row("Dimension_DisplayBox") = dataView(0).Item("Dimension_DisplayBox")
                row("ProductStyle_DisplayBox") = dataView(0).Item("ProductStyle_DisplayBox")
                row("Research_DevelopmentBulk") = dataView(0).Item("Research_DevelopmentBulk")
                row("Research_Development") = dataView(0).Item("Research_Development")
                dsPBlisters.Tables("blisters").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub
    Private Sub CopyBlending(selectedVersion As String)
        Try
            Dim row As DataRow
            Dim dataView As DataView
            'Blending
            dataView = New DataView(dsBi.Tables("blending"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "BlendingDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                row = dsBi.Tables("blending").NewRow
                row("FormulaID") = FormulaID.Text
                row("EnteredDate") = DateTime.Now
                row("BlendingCost") = dataView(0).Item("BlendingCost")
                row("WastagePercentage") = dataView(0).Item("WastagePercentage")
                row("WastageValue") = dataView(0).Item("WastageValue")
                row("LabCost") = dataView(0).Item("LabCost")
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                row("EnteredBy") = dataView(0).Item("EnteredBy")
                row("FlavorProfile") = dataView(0).Item("FlavorProfile")
                dsBi.Tables("blending").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub
    Private Sub CopySachets(selectedVersion As String)
        Try
            Dim row As DataRow
            Dim dataView As DataView

            'Sachets
            dataView = New DataView(dsPSa.Tables("sachets"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "StickPackDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                row = dsPSa.Tables("sachets").NewRow
                row("FormulaID") = FormulaID.Text
                row("SizeCount") = dataView(0).Item("SizeCount")
                row("Material") = dataView(0).Item("Material")
                row("Quantity") = dataView(0).Item("Quantity")
                row("Colors") = dataView(0).Item("Colors")
                row("PacketContents") = dataView(0).Item("PacketContents")
                row("CostContent") = dataView(0).Item("CostContent")
                row("FillingCost") = dataView(0).Item("FillingCost")
                row("PrintingCost") = dataView(0).Item("PrintingCost")
                row("DisplayCost") = dataView(0).Item("DisplayCost")
                row("PrintingPlates") = dataView(0).Item("PrintingPlates")
                row("ArtPreparation") = dataView(0).Item("ArtPreparation")
                row("PackOut") = dataView(0).Item("PackOut")
                row("OtherCostDesc") = dataView(0).Item("OtherCostDesc")
                row("ShipperCaseCountDesc") = dataView(0).Item("ShipperCaseCountDesc")
                row("ShipperCaseCountValue") = dataView(0).Item("ShipperCaseCountValue")
                row("OtherCostValue") = dataView(0).Item("OtherCostValue")
                row("Sachets") = dataView(0).Item("Sachets")
                row("WaferSeal") = dataView(0).Item("WaferSeal")
                row("ShrinkWrap") = dataView(0).Item("ShrinkWrap")
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                row("PrintingPlatesBulk") = dataView(0).Item("PrintingPlatesBulk")
                row("ArtPreparationBulk") = dataView(0).Item("ArtPreparationBulk")
                row("OtherCostDescBulk") = dataView(0).Item("OtherCostDescBulk")
                row("OtherCostBulk") = dataView(0).Item("OtherCostBulk")
                row("ShipperCaseCostBulk") = dataView(0).Item("ShipperCaseCostBulk")
                row("ShipperCaseCountBulk") = dataView(0).Item("ShipperCaseCountBulk")
                'Display Box Group 
                row("DisplayQuantity") = dataView(0).Item("DisplayQuantity")
                row("DisplayDescription") = dataView(0).Item("DisplayDescription")
                row("DisplaySpec") = dataView(0).Item("DisplaySpec")
                row("DisplayBoardGrade") = dataView(0).Item("DisplayBoardGrade")
                row("DisplayDimension") = dataView(0).Item("DisplayDimension")
                row("DisplayProductStyle") = dataView(0).Item("DisplayProductStyle")

                row("Research_Development") = dataView(0).Item("Research_Development")
                row("Research_DevelopmentBulk") = dataView(0).Item("Research_DevelopmentBulk")

                dsPSa.Tables("sachets").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub CopyEncapsulation(selectedVersion As String)
        Try
            Dim row As DataRow
            Dim dataView As DataView
            'Encapsulation
            dataView = New DataView(dsEi.Tables("encapsulation"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "EncapsulationDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                row = dsEi.Tables("encapsulation").NewRow
                row("FormulaID") = FormulaID.Text
                row("TypeofCapsule") = dataView(0).Item("TypeofCapsule")
                row("CapsuleCost") = dataView(0).Item("CapsuleCost")
                row("EncapsulationCost") = dataView(0).Item("EncapsulationCost")
                row("WastageValue") = dataView(0).Item("WastageValue")
                row("LabCost") = dataView(0).Item("LabCost")
                row("WastagePercentage") = dataView(0).Item("WastagePercentage")
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                row("EnteredDate") = DateTime.Now
                row("EnteredBy") = dataView(0).Item("EnteredBy")
                dsEi.Tables("encapsulation").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub CopyTableting(selectedVersion As String)
        Try
            Dim row As DataRow
            Dim dataView As DataView
            'Tableting
            dataView = New DataView(dsTi.Tables("tableting"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "TabletingDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                row = dsTi.Tables("tableting").NewRow
                row("FormulaID") = FormulaID.Text
                row("TypeofCoating") = dataView(0).Item("TypeofCoating")
                row("CoatingCost") = dataView(0).Item("CoatingCost")
                row("CompressionCost") = dataView(0).Item("CompressionCost")
                row("WastageValue") = dataView(0).Item("WastageValue")
                row("WastagePercentage") = dataView(0).Item("WastagePercentage")
                row("WastagePercentage") = dataView(0).Item("WastagePercentage")
                row("EnteredBy") = dataView(0).Item("EnteredBy")
                row("LabCost") = dataView(0).Item("LabCost")
                row("EnteredDate") = DateTime.Now
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                dsTi.Tables("tableting").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub CopyStickPacks(selectedVersion As String)
        Try
            Dim row As DataRow
            Dim dataView As DataView

            'Stickpacks
            dataView = New DataView(dsPSP.Tables("stickpacks"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "StickPackDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                row = dsPSP.Tables("stickpacks").NewRow
                row("FormulaID") = FormulaID.Text
                row("SizeCount") = dataView(0).Item("SizeCount")
                row("Material") = dataView(0).Item("Material")
                row("Quantity") = dataView(0).Item("Quantity")
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                row("PacketContents") = dataView(0).Item("PacketContents")
                row("CostContent") = dataView(0).Item("CostContent")
                row("FillingCost") = dataView(0).Item("FillingCost")
                row("PrintingCost") = dataView(0).Item("PrintingCost")
                row("DisplayCost") = dataView(0).Item("DisplayCost")
                row("PrintingPlates") = dataView(0).Item("PrintingPlates")
                row("ArtPreparation") = dataView(0).Item("ArtPreparation")
                row("PackOut") = dataView(0).Item("PackOut")
                row("OtherCostValue") = dataView(0).Item("OtherCostValue")
                row("OtherCostDesc") = dataView(0).Item("OtherCostDesc")
                row("ShipperCaseCountDesc") = dataView(0).Item("ShipperCaseCountDesc")
                row("ShipperCaseCountValue") = dataView(0).Item("ShipperCaseCountValue")
                row("StickPacks") = dataView(0).Item("StickPacks")
                row("WaferSeal") = dataView(0).Item("WaferSeal")
                row("ShrinkWrap") = dataView(0).Item("ShrinkWrap")
                row("Colors") = dataView(0).Item("Colors")
                row("PrintingPlatesBulk") = dataView(0).Item("PrintingPlatesBulk")
                row("ArtPreparationBulk") = dataView(0).Item("ArtPreparationBulk")
                row("OtherCostDescBulk") = dataView(0).Item("OtherCostDescBulk")
                row("OtherCostBulk") = dataView(0).Item("OtherCostBulk")
                row("ShipperCaseCostBulk") = dataView(0).Item("ShipperCaseCostBulk")
                row("ShipperCaseCountBulk") = dataView(0).Item("ShipperCaseCountBulk")
                'Display Box Group 
                row("DisplayQuantity") = dataView(0).Item("DisplayQuantity")
                row("DisplayDescription") = dataView(0).Item("DisplayDescription")
                row("DisplaySpec") = dataView(0).Item("DisplaySpec")
                row("DisplayBoardGrade") = dataView(0).Item("DisplayBoardGrade")
                row("DisplayDimension") = dataView(0).Item("DisplayDimension")
                row("DisplayProductStyle") = dataView(0).Item("DisplayProductStyle")

                row("Research_DevelopmentBulk") = dataView(0).Item("Research_DevelopmentBulk")
                row("Research_DevelopmentBox") = dataView(0).Item("Research_DevelopmentBox")

                dsPSP.Tables("stickpacks").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub CopySales(selectedVersion As String)
        Try
            Dim row As DataRow
            Dim dataView As DataView
            'Sales Bulk Details
            dataView = New DataView(dsSal.Tables("salestxns"), " [Type]='BULK' AND VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "SalesDetailID", DataViewRowState.CurrentRows)
            For Each DataRow As DataRowView In dataView
                row = dsSal.Tables("salestxns").NewRow
                row("FormulaID") = FormulaID.Text
                row("PackagingFormat") = DataRow.Item("PackagingFormat")
                row("UnitSize") = DataRow.Item("UnitSize")
                row("Quantity") = DataRow.Item("Quantity")
                row("MarginPercentage") = DataRow.Item("MarginPercentage")
                row("Type") = DataRow.Item("Type")
                row("SalesPrice") = DataRow.Item("SalesPrice")
                row("OverriddenSalesPrice") = DataRow.Item("OverriddenSalesPrice")
                row("Overridden") = DataRow.Item("Overridden")
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                dsSal.Tables("salestxns").Rows.Add(row)
            Next
            BindingSource2.DataSource = dsSal.Tables("salestxns")
            BindingSource2.Filter = " [Type]='BULK' AND VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion)

            'If chkDisplayBox.Checked Then
            'Sales Box Details
            dataView = New DataView(dsSalBox.Tables("salesBoxTxns"), " [Type]='BOX' AND VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "SalesDetailID", DataViewRowState.CurrentRows)
            For Each DataRow As DataRowView In dataView
                row = dsSalBox.Tables("salesBoxTxns").NewRow
                row("FormulaID") = FormulaID.Text
                row("PackagingFormat") = DataRow.Item("PackagingFormat")
                row("UnitSize") = DataRow.Item("UnitSize")
                row("Quantity") = DataRow.Item("Quantity")
                row("MarginPercentage") = DataRow.Item("MarginPercentage")
                row("Type") = DataRow.Item("Type")
                row("SalesPrice") = DataRow.Item("SalesPrice")
                row("OverriddenSalesPrice") = DataRow.Item("OverriddenSalesPrice")
                row("Overridden") = DataRow.Item("Overridden")
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                dsSalBox.Tables("salesBoxTxns").Rows.Add(row)
            Next
            BindSalesBoxGrid.DataSource = dsSal.Tables("salestxns")
            BindSalesBoxGrid.Filter = " [Type]='BOX' AND VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion)
            'Else
            'Sales Box Details
            dataView = New DataView(dsSalBags.Tables("salesBagsTxns"), " [Type]='BULKBAGS' AND VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "SalesDetailID", DataViewRowState.CurrentRows)
            For Each DataRow As DataRowView In dataView
                row = dsSalBags.Tables("salesBagsTxns").NewRow
                row("FormulaID") = FormulaID.Text
                row("PackagingFormat") = DataRow.Item("PackagingFormat")
                row("UnitSize") = DataRow.Item("UnitSize")
                row("Quantity") = DataRow.Item("Quantity")
                row("MarginPercentage") = DataRow.Item("MarginPercentage")
                row("Type") = DataRow.Item("Type")
                row("SalesPrice") = DataRow.Item("SalesPrice")
                row("OverriddenSalesPrice") = DataRow.Item("OverriddenSalesPrice")
                row("Overridden") = DataRow.Item("Overridden")
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                dsSalBags.Tables("salesBagsTxns").Rows.Add(row)
            Next
            BindSalesBulkStickpacksGrid.DataSource = dsSalBags.Tables("salesBagsTxns")
            BindSalesBulkStickpacksGrid.Filter = " [Type]='BULKBAGS' AND VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion)
            'End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub CopyBags(selectedVersion As String)
        Try
            Dim row As DataRow
            Dim dataView As DataView

            'Bags
            dataView = New DataView(dsStandupBags.Tables("StandUpBag"), "AddedFrom ='" & ComboBox3.Text & "' AND VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "StandUpBagID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                row = dsStandupBags.Tables("StandUpBag").NewRow()
                row("FormulaId") = Convert.ToInt32(FormulaID.Text)
                row("Description_") = dataView(0).Item("Description_")
                row("QuantityBag") = dataView(0).Item("QuantityBag")
                row("MaterialBag") = dataView(0).Item("MaterialBag")
                row("SizeBag") = dataView(0).Item("SizeBag")
                row("PrintColor") = dataView(0).Item("PrintColor")
                row("TearNotch") = dataView(0).Item("TearNotch")
                row("Zipper") = dataView(0).Item("Zipper")
                row("HangerHole") = dataView(0).Item("HangerHole")
                row("ServingWeight") = dataView(0).Item("ServingWeight")
                row("Servings") = dataView(0).Item("Servings")
                row("FillWeight") = dataView(0).Item("FillWeight")
                row("PrintingPlatesBag") = dataView(0).Item("PrintingPlatesBag")
                row("ArtPreparationBag") = dataView(0).Item("ArtPreparationBag")
                row("Other") = dataView(0).Item("Other")
                row("Other_Description") = dataView(0).Item("Other_Description")
                row("Shipper_CaseAmt") = dataView(0).Item("Shipper_CaseAmt")
                row("Shipper_CaseCount") = dataView(0).Item("Shipper_CaseCount")
                row("Price") = dataView(0).Item("Price")
                row("PrintingCostBag") = dataView(0).Item("PrintingCostBag")
                row("SecondaryPackout") = dataView(0).Item("SecondaryPackout")
                row("Research_DevelopmentBag") = dataView(0).Item("Research_DevelopmentBag")
                row("VersionNumber") = VersionCmbBox.Items.Cast(Of Integer).Max() + 1
                row("AddedFrom") = ComboBox3.Text
                dsStandupBags.Tables("StandUpBag").Rows.Add(row)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub CopyBottle(selectedVersion As String)
        Try
            Dim row As DataRow
            Dim dataView As DataView

            'Bottle
            dataView = New DataView(dsPB.Tables("bottles"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "", DataViewRowState.CurrentRows)
            For Each DataRow As DataRowView In dataView
                row = dsPB.Tables("bottles").NewRow
                row("BindingIndex") = DataRow.Item("BindingIndex")
                row("FormulaID") = DataRow.Item("FormulaID")
                row("SizeCount") = DataRow.Item("SizeCount")
                row("Category") = DataRow.Item("Category")
                row("MaterialName") = DataRow.Item("MaterialName")
                row("VendorName") = DataRow.Item("VendorName")
                row("UnitCost") = DataRow.Item("UnitCost")
                row("ComponentCode") = DataRow.Item("ComponentCode")
                row("IsDefault") = DataRow.Item("IsDefault")
                row("EnteredBy") = DataRow.Item("EnteredBy")
                row("EnteredDate") = DateTime.Now
                row("VersionNumber") = VersionCmbBox.Items.Count + 1
                dsPB.Tables("bottles").Rows.Add(row)
            Next
            ComboBox4.Items.Clear()
            ComboBox4.Text = ""
            DataGridView2.DataSource = dsPB.Tables("bottles").Select("VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Items.Cast(Of Integer).Max() + 1), 0, VersionCmbBox.Items.Cast(Of Integer).Max() + 1))

            lblBottleUnitCostSumAMT.Text = "0"
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DisplayBlister(selectedVersion As String)
        Try
            'Blisters
            Dim dataView As DataView = New DataView(dsPBlisters.Tables("blisters"), "VersionNumber=" & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "BlisterDetailsID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                txtBlisterFormat.Text = If(String.IsNullOrEmpty(dataView(0).Item("BlisterFormat")) OrElse IsDBNull(dataView(0).Item("BlisterFormat")), "", dataView(0).Item("BlisterFormat"))
                txtBlisterQty.Text = If(String.IsNullOrEmpty(dataView(0).Item("Quantity")) OrElse IsDBNull(dataView(0).Item("Quantity")), "", dataView(0).Item("Quantity"))
                txtboxBlisterMaterial.Text = If(String.IsNullOrEmpty(dataView(0).Item("Material")) OrElse IsDBNull(dataView(0).Item("Material")), "", dataView(0).Item("Material").ToString())
                txtBlisterSize.Text = If(String.IsNullOrEmpty(dataView(0).Item("BlisterSize")) OrElse IsDBNull(dataView(0).Item("BlisterSize")), "", dataView(0).Item("BlisterSize"))
                txtBlisterCount.Text = If(String.IsNullOrEmpty(dataView(0).Item("BlisterCount")) OrElse IsDBNull(dataView(0).Item("BlisterCount")), "", dataView(0).Item("BlisterCount"))
                cmbBlisterShrinkWrap.SelectedIndex = If(dataView(0).Item("ShrinkWrapFlag").ToString() = "0", 1, 0)
                cmbBlisterWaferSeal.SelectedIndex = If(dataView(0).Item("WaferSealsFlag").ToString() = "0", 1, 0)
                'cmbBlisterCountType.SelectedIndex = If(dataView(0).Item("BlisterType").ToString() = "Tablets(s)", 1, 0)
                If FormulaTypeCmbBox.Text = "Capsule" Then
                    cmbBlisterCountType.SelectedIndex = 0
                ElseIf FormulaTypeCmbBox.Text = "Tablet" Then
                    cmbBlisterCountType.SelectedIndex = 1
                End If
                txtBlisterCountAmount.Text = If(String.IsNullOrEmpty(dataView(0).Item("BlisterAmount")) OrElse IsDBNull(dataView(0).Item("BlisterAmount")), "", dataView(0).Item("BlisterAmount"))
                txtBlisterCost.Text = If(String.IsNullOrEmpty(dataView(0).Item("BlisterCost")) OrElse IsDBNull(dataView(0).Item("BlisterCost")), "", dataView(0).Item("BlisterCost"))
                txtBlisterPrintingCostAmt.Text = If(String.IsNullOrEmpty(dataView(0).Item("PrintingCost")) OrElse IsDBNull(dataView(0).Item("PrintingCost")), "", dataView(0).Item("PrintingCost"))
                txtBlisterBoxCostAmt.Text = If(String.IsNullOrEmpty(dataView(0).Item("FoldingCartonCost")) OrElse IsDBNull(dataView(0).Item("FoldingCartonCost")), "", dataView(0).Item("FoldingCartonCost"))
                txtBlisterPrintingPlatesAmt.Text = If(String.IsNullOrEmpty(dataView(0).Item("PrintingPlatesCost")) OrElse IsDBNull(dataView(0).Item("PrintingPlatesCost")), "", dataView(0).Item("PrintingPlatesCost"))
                txtPrintingBulk.Text = If(IsDBNull(dataView(0).Item("PrintingPlatesCostBulk")) OrElse IsDBNull(dataView(0).Item("PrintingPlatesCostBulk")), 0, dataView(0).Item("PrintingPlatesCostBulk"))
                txtBlisterArtPreperationAmt.Text = If(String.IsNullOrEmpty(dataView(0).Item("ArtPreparation")) OrElse IsDBNull(dataView(0).Item("ArtPreparation")), "", dataView(0).Item("ArtPreparation"))
                txtArtPrepBulk.Text = If(IsDBNull(dataView(0).Item("ArtPreparationBulk")) OrElse IsDBNull(dataView(0).Item("ArtPreparationBulk")), 0, dataView(0).Item("ArtPreparationBulk"))
                txtBlisterPackoutAmt.Text = If(String.IsNullOrEmpty(dataView(0).Item("SecondaryPackoutCost")) OrElse IsDBNull(dataView(0).Item("SecondaryPackoutCost")), "", dataView(0).Item("SecondaryPackoutCost"))
                txtBlisterShrinkWrap.Text = If(String.IsNullOrEmpty(dataView(0).Item("ShrinkWrap")) OrElse IsDBNull(dataView(0).Item("ShrinkWrap")), "", dataView(0).Item("ShrinkWrap"))
                txtBlisterWaferSeal.Text = If(String.IsNullOrEmpty(dataView(0).Item("WaferSealsCost")) OrElse IsDBNull(dataView(0).Item("WaferSealsCost")), "", dataView(0).Item("WaferSealsCost"))
                txtBlisterOtherAmt.Text = If(String.IsNullOrEmpty(dataView(0).Item("OtherCost")) OrElse IsDBNull(dataView(0).Item("OtherCost")), "", dataView(0).Item("OtherCost"))
                txtOtherCostBulk.Text = If(IsDBNull(dataView(0).Item("OtherCostBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("OtherCostBulk")), 0, dataView(0).Item("OtherCostBulk"))
                txtOtherDescBlister.Text = If(String.IsNullOrEmpty(dataView(0).Item("Other_Description")) OrElse IsDBNull(dataView(0).Item("Other_Description")), "", dataView(0).Item("Other_Description"))
                txtOtherBulk.Text = If(IsDBNull(dataView(0).Item("Other_DescriptionBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("Other_DescriptionBulk")), 0, dataView(0).Item("Other_DescriptionBulk"))
                txtBlisterCaseCount.Text = If(String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCount")) OrElse IsDBNull(dataView(0).Item("ShipperCaseCount")), "", dataView(0).Item("ShipperCaseCount"))
                txtShipperCountBulk.Text = If(dataView(0).Item("ShipperCaseCountBulk").ToString() = "", "", dataView(0).Item("ShipperCaseCountBulk"))
                txtBlisterCaseCountAmt.Text = If(String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCost")) OrElse IsDBNull(dataView(0).Item("ShipperCaseCost")), "", dataView(0).Item("ShipperCaseCost"))
                txtShipperCostBulk.Text = If(IsDBNull(dataView(0).Item("ShipperCaseCostBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCostBulk")), 0, dataView(0).Item("ShipperCaseCostBulk"))
                txttoolcostamtblister.Text = If(String.IsNullOrEmpty(dataView(0).Item("ToolingCost")) OrElse IsDBNull(dataView(0).Item("ToolingCost")), "", dataView(0).Item("ToolingCost"))
                txtToolingCostBulk.Text = If(IsDBNull(dataView(0).Item("ToolingCostBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("ToolingCostBulk")), 0, dataView(0).Item("ToolingCostBulk"))
                txtQtyDiplaybox.Text = If(IsDBNull(dataView(0).Item("Quantity_DisplayBox")) OrElse String.IsNullOrEmpty(dataView(0).Item("Quantity_DisplayBox")), 0, Convert.ToInt32(dataView(0).Item("Quantity_DisplayBox")).ToString("N0", culture))
                txtAmontDisplaybox.Text = If(String.IsNullOrEmpty(dataView(0).Item("Amount_DisplayBox")) OrElse IsDBNull(dataView(0).Item("Amount_DisplayBox")), "", dataView(0).Item("Amount_DisplayBox"))
                txtDescDiplaybox.Text = If(String.IsNullOrEmpty(dataView(0).Item("Description_DisplayBox")) OrElse IsDBNull(dataView(0).Item("Description_DisplayBox")), "", dataView(0).Item("Description_DisplayBox"))
                txtSepcDisplaybox.Text = If(String.IsNullOrEmpty(dataView(0).Item("Spec_DisplayBox")) OrElse IsDBNull(dataView(0).Item("Spec_DisplayBox")), "", dataView(0).Item("Spec_DisplayBox"))
                txtBoradGradeDiplaybox.Text = If(String.IsNullOrEmpty(dataView(0).Item("BoardGrade_DisplayBox")) OrElse IsDBNull(dataView(0).Item("BoardGrade_DisplayBox")), "", dataView(0).Item("BoardGrade_DisplayBox"))
                txtDimensionDisplaybox.Text = If(String.IsNullOrEmpty(dataView(0).Item("Dimension_DisplayBox")) OrElse IsDBNull(dataView(0).Item("Dimension_DisplayBox")), "", dataView(0).Item("Dimension_DisplayBox"))
                txtProductStyle.Text = If(String.IsNullOrEmpty(dataView(0).Item("ProductStyle_DisplayBox")) OrElse IsDBNull(dataView(0).Item("ProductStyle_DisplayBox")), "", dataView(0).Item("ProductStyle_DisplayBox"))
                txtBlisterBulkRD.Text = If(IsDBNull(dataView(0).Item("Research_DevelopmentBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("Research_DevelopmentBulk")), 0, dataView(0).Item("Research_DevelopmentBulk"))
                txtBlisterBoxRD.Text = If(IsDBNull(dataView(0).Item("Research_Development")) OrElse String.IsNullOrEmpty(dataView(0).Item("Research_Development")), 0, dataView(0).Item("Research_Development"))
            Else
                AddRemoveHandlerBlister(False)
                RemoveHandler TextBox8.TextChanged, AddressOf TextBox8_TextChanged
                txtBlisterFormat.Text = ""
                txtBlisterQty.Text = ""
                txtBlisterSize.Text = "0"
                txtBlisterCount.Text = "0"
                txtboxBlisterMaterial.SelectedIndex = 0
                cmbBlisterShrinkWrap.SelectedIndex = 1
                cmbBlisterWaferSeal.SelectedIndex = 1
                If FormulaTypeCmbBox.Text = "Capsule" Then
                    cmbBlisterCountType.SelectedIndex = 0
                ElseIf FormulaTypeCmbBox.Text = "Tablet" Then
                    cmbBlisterCountType.SelectedIndex = 1
                End If
                txtBlisterCountAmount.Text = "0"
                txtBlisterCost.Text = "0.00"
                txtBlisterPrintingCostAmt.Text = "0.00"
                txtBlisterBoxCostAmt.Text = "0"
                txtBlisterPrintingPlatesAmt.Text = "0.00"
                txtPrintingBulk.Text = "0.00"
                txtBlisterArtPreperationAmt.Text = "0.00"
                txtArtPrepBulk.Text = "0.00"
                txtBlisterPackoutAmt.Text = "0.00"
                txtBlisterShrinkWrap.Text = "0.00"
                txtBlisterWaferSeal.Text = "0.00"
                txtBlisterOtherAmt.Text = "0.00"
                txtOtherCostBulk.Text = "0.00"
                txtOtherDescBlister.Text = ""
                txtOtherBulk.Text = ""
                txtBlisterCaseCount.Text = ""
                txtShipperCountBulk.Text = ""
                txtBlisterCaseCountAmt.Text = "0.00"
                txtShipperCostBulk.Text = "0.00"
                'Changes by Payal P
                txttoolcostamtblister.Text = "0.00"
                txtToolingCostBulk.Text = "0.00"
                'Chnage for display box
                txtQtyDiplaybox.Text = ""
                txtAmontDisplaybox.Text = "0.00"
                txtDescDiplaybox.Text = ""
                txtSepcDisplaybox.Text = ""
                txtBoradGradeDiplaybox.Text = ".018 SBS C1S"
                txtDimensionDisplaybox.Text = ""
                txtProductStyle.Text = "TT AGB-T Style"
                txtBlisterBulkRD.Text = "0.00"
                txtBlisterBoxRD.Text = "0.00"
                AddHandler TextBox8.TextChanged, AddressOf TextBox8_TextChanged
                AddRemoveHandlerBlister(True)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DisplayBlending(selectedVersion As String)
        Try
            Dim dataView As DataView = New DataView(dsBi.Tables("blending"), "VersionNumber=" & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "BlendingDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                AddRemoveBlending(False)
                TextBox12.Text = SetSafeValue(dataView, "BlendingCost")
                TextBox13.Text = SetSafeValue(dataView, "WastageValue")
                TextBox14.Text = SetSafeValue(dataView, "WastagePercentage")
                TextBox15.Text = SetSafeValue(dataView, "LabCost")
                txtBlendingFlavourProfileServing.Text = SetSafeValue(dataView, "FlavorProfile")
                txtBlendingFlavourProfileKg.Text = SetSafeValue(dataView, "FlavorProfile")
                AddRemoveBlending(True)
            Else
                AddRemoveBlending(False)
                For Each ctl In BlendingPanel.Controls
                    If TypeOf ctl Is TextBox Then
                        ctl.Text = "0.00"
                    End If
                Next
                AddRemoveBlending(True)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DisplayStandupBag(selectedVersion As String)
        Try
            Dim dataView As DataView = New DataView(dsStandupBags.Tables("StandUpBag"), "AddedFrom = '" & ComboBox3.Text & "'AND VersionNumber=" & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "StandUpBagID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                AddRemoveHandlerBags(False)
                txtDexcription.Text = If(IsDBNull(dataView(0).Item("Description_")) OrElse String.IsNullOrEmpty(dataView(0).Item("Description_")), 0, dataView(0).Item("Description_"))
                txtQty.Text = If(IsDBNull(dataView(0).Item("QuantityBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("QuantityBag")), 0, Convert.ToInt32(dataView(0).Item("QuantityBag").ToString().Replace(",", "")).ToString("N0", culture))
                txtMaterial.Text = If(IsDBNull(dataView(0).Item("MaterialBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("MaterialBag")), 0, dataView(0).Item("MaterialBag"))
                txtSize.Text = If(IsDBNull(dataView(0).Item("SizeBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("SizeBag")), 0, dataView(0).Item("SizeBag"))
                txtPrintColors.Text = If(IsDBNull(dataView(0).Item("PrintColor")) OrElse String.IsNullOrEmpty(dataView(0).Item("PrintColor")), 0, dataView(0).Item("PrintColor"))

                cmbTearNotch.SelectedIndex = If(dataView(0).Item("TearNotch").ToString() = "False", 1, 0)
                cmbHangerHole.SelectedIndex = If(dataView(0).Item("HangerHole").ToString() = "False", 1, 0)
                cmbZipper.SelectedIndex = If(dataView(0).Item("Zipper").ToString() = "False", 1, 0)

                txtPrintingCost.Text = If(IsDBNull(dataView(0).Item("PrintingCostBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("PrintingCostBag")), 0, dataView(0).Item("PrintingCostBag"))
                txtPrintingPlates.Text = If(IsDBNull(dataView(0).Item("PrintingPlatesBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("PrintingPlatesBag")), "", dataView(0).Item("PrintingPlatesBag"))
                txtArtPreperation.Text = If(IsDBNull(dataView(0).Item("ArtPreparationBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("ArtPreparationBag")), 0, dataView(0).Item("ArtPreparationBag"))
                txtSecondaryPackoutBag.Text = If(IsDBNull(dataView(0).Item("SecondaryPackout")) OrElse String.IsNullOrEmpty(dataView(0).Item("SecondaryPackout")), 0, dataView(0).Item("SecondaryPackout"))
                txtOther.Text = If(IsDBNull(dataView(0).Item("Other")) OrElse String.IsNullOrEmpty(dataView(0).Item("Other")), "", dataView(0).Item("Other"))
                txtOtherDesc.Text = If(IsDBNull(dataView(0).Item("Other_Description")) OrElse String.IsNullOrEmpty(dataView(0).Item("Other_Description")), "", dataView(0).Item("Other_Description"))

                txtShipperCaseCount.Text = If(IsDBNull(dataView(0).Item("Shipper_CaseCount")) OrElse String.IsNullOrEmpty(dataView(0).Item("Shipper_CaseCount")), "", dataView(0).Item("Shipper_CaseCount"))
                txtShipperCaseAmt.Text = If(IsDBNull(dataView(0).Item("Shipper_CaseAmt")) OrElse String.IsNullOrEmpty(dataView(0).Item("Shipper_CaseAmt")), 0, dataView(0).Item("Shipper_CaseAmt"))
                'txtPrice.Text = dataView(0).Item("Price")

                AddHandler txtServing.TextChanged, AddressOf txtServing_TextChanged
                txtServing.Text = If(IsDBNull(dataView(0).Item("Servings")) OrElse String.IsNullOrEmpty(dataView(0).Item("Servings")), 0, dataView(0).Item("Servings"))
                txtServingWeight.Text = If(IsDBNull(dataView(0).Item("ServingWeight")) OrElse String.IsNullOrEmpty(dataView(0).Item("ServingWeight")), 0, dataView(0).Item("ServingWeight"))
                'txtFillWeight.Text = If(IsDBNull(dataView(0).Item("FillWeight")), 0, dataView(0).Item("FillWeight"))
                txtStandupBagRD.Text = If(IsDBNull(dataView(0).Item("Research_DevelopmentBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("Research_DevelopmentBag")), 0, dataView(0).Item("Research_DevelopmentBag"))
                AddRemoveHandlerBags(True)
            Else
                AddRemoveHandlerBags(False)
                RemoveHandler TextBox8.TextChanged, AddressOf TextBox8_TextChanged
                txtDexcription.Text = "Standup Bag"
                txtQty.Text = "0"
                txtMaterial.Text = "1 MIL Matte/48ga MET PET/3.5mil PE"
                txtSize.Text = ""
                txtPrintColors.Text = "4 Color Process & White"
                cmbTearNotch.SelectedIndex = 0
                cmbHangerHole.SelectedIndex = 1
                cmbZipper.SelectedIndex = 0
                txtPrintingCost.Text = "0.00"
                txtPrintingPlates.Text = "0.00"
                txtArtPreperation.Text = "0.00"
                txtSecondaryPackoutBag.Text = "0.00"
                txtOther.Text = "0.00"
                txtOtherDesc.Text = ""
                txtShipperCaseCount.Text = ""
                txtShipperCaseAmt.Text = "0.00"
                'txtPrice.Text = "0.00"
                txtStandupBagRD.Text = "0.00"
                txtServing.Text = ""
                txtServingWeight.Text = ServingSizeTextBox.Text
                txtFillWeight.Text = "0.00"
                AddHandler TextBox8.TextChanged, AddressOf TextBox8_TextChanged
                AddRemoveHandlerBags(True)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DisplayStickpacks(selectedVersion As String)
        Try
            Dim dataView As DataView = New DataView(dsPSP.Tables("stickpacks"), "VersionNumber=" & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "StickPackDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                ComboBox6.Text = dataView(0).Item("SizeCount")
                ComboBox7.Text = dataView(0).Item("Material")
                ComboBox8.Text = If(IsDBNull(dataView(0).Item("Quantity")) OrElse String.IsNullOrEmpty(dataView(0).Item("Quantity")), "", Convert.ToInt32(dataView(0).Item("Quantity").ToString().Replace(",", "")).ToString("N0", culture))
                cmbstickpanelColors.Text = If(IsDBNull(dataView(0).Item("Colors")) OrElse String.IsNullOrEmpty(dataView(0).Item("Colors")), "", dataView(0).Item("Colors"))
                If FormulaTypeCmbBox.SelectedIndex <> 0 Then
                    TextBox16.Text = If(IsDBNull(dataView(0).Item("PacketContents")) OrElse String.IsNullOrEmpty(dataView(0).Item("PacketContents")), "", Convert.ToInt16(dataView(0).Item("PacketContents")))
                Else
                    TextBox16.Text = dataView(0).Item("PacketContents")
                End If

                TextBox18.Text = dataView(0).Item("FillingCost")
                TextBox19.Text = dataView(0).Item("PrintingCost")
                TextBox20.Text = dataView(0).Item("DisplayCost")
                TextBox21.Text = dataView(0).Item("PrintingPlates")
                TextBox22.Text = dataView(0).Item("ArtPreparation")
                TextBox23.Text = dataView(0).Item("PackOut")
                TextBox24.Text = dataView(0).Item("OtherCostValue")
                TextBox25.Text = If(IsDBNull(dataView(0).Item("OtherCostDesc")) OrElse String.IsNullOrEmpty(dataView(0).Item("OtherCostDesc")), "", dataView(0).Item("OtherCostDesc"))
                txtShrinkWrapStickPack.Text = If(IsDBNull(dataView(0).Item("ShrinkWrap")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShrinkWrap")), "0.00", dataView(0).Item("ShrinkWrap"))
                txtWaferSeal.Text = If(IsDBNull(dataView(0).Item("WaferSeal")) OrElse String.IsNullOrEmpty(dataView(0).Item("WaferSeal")), "0.00", dataView(0).Item("WaferSeal"))
                txtStickPacks.Text = If(IsDBNull(dataView(0).Item("StickPacks")) OrElse String.IsNullOrEmpty(dataView(0).Item("StickPacks")), "", dataView(0).Item("StickPacks"))
                txtStickPacksShipperCaseCount.Text = If(IsDBNull(dataView(0).Item("ShipperCaseCountDesc")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCountDesc")), "", dataView(0).Item("ShipperCaseCountDesc"))
                txtStickPacksShipperCaseCountAmt.Text = If(IsDBNull(dataView(0).Item("ShipperCaseCountValue")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCountValue")), "0.00", dataView(0).Item("ShipperCaseCountValue"))
                'Changes by Payal P
                'Display Box
                TextBox39DisplayQunatity.Text = If(IsDBNull(dataView(0).Item("DisplayQuantity")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayQuantity")), 0, Convert.ToInt32(dataView(0).Item("DisplayQuantity")).ToString("N0", culture))
                TextBox38DisplayDesc.Text = If(IsDBNull(dataView(0).Item("DisplayDescription")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayDescription")), "", dataView(0).Item("DisplayDescription"))
                TextBox43DisplaySpec.Text = If(IsDBNull(dataView(0).Item("DisplaySpec")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplaySpec")), "", dataView(0).Item("DisplaySpec"))
                TextBox42DisplayBoardGrade.Text = If(IsDBNull(dataView(0).Item("DisplayBoardGrade")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayBoardGrade")), ".018 SBS C1S", dataView(0).Item("DisplayBoardGrade"))
                TextBox41DisplayDimension.Text = If(IsDBNull(dataView(0).Item("DisplayDimension")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayDimension")), "", dataView(0).Item("DisplayDimension"))
                TextBox40DisplayProductStyle.Text = If(IsDBNull(dataView(0).Item("DisplayProductStyle")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayProductStyle")), "TT AGB-T Style", dataView(0).Item("DisplayProductStyle"))
                'Bulk
                txtStickPrintingPlateBulk.Text = If(IsDBNull(dataView(0).Item("PrintingPlatesBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("PrintingPlatesBulk")), "0.00", dataView(0).Item("PrintingPlatesBulk"))
                txtStickArtPrepBulk.Text = If(IsDBNull(dataView(0).Item("ArtPreparationBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("ArtPreparationBulk")), "0.00", dataView(0).Item("ArtPreparationBulk"))
                txtStickOtherCostBulk.Text = If(IsDBNull(dataView(0).Item("OtherCostBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("OtherCostBulk")), "0.00", dataView(0).Item("OtherCostBulk"))
                txtStickShipperCostBulk.Text = If(IsDBNull(dataView(0).Item("ShipperCaseCostBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCostBulk")), "0.00", dataView(0).Item("ShipperCaseCostBulk"))
                txtStickOtherDescBulk.Text = If(IsDBNull(dataView(0).Item("OtherCostDescBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("OtherCostDescBulk")), "", dataView(0).Item("OtherCostDescBulk"))
                txtStickShipperCountBulk.Text = If(IsDBNull(dataView(0).Item("ShipperCaseCountBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCountBulk")), "", dataView(0).Item("ShipperCaseCountBulk"))
                txtStickpackBulkRD.Text = If(IsDBNull(dataView(0).Item("Research_DevelopmentBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("Research_DevelopmentBulk")), "0.00", dataView(0).Item("Research_DevelopmentBulk"))
                txtStickpackBoxRD.Text = If(IsDBNull(dataView(0).Item("Research_DevelopmentBox")) OrElse String.IsNullOrEmpty(dataView(0).Item("Research_DevelopmentBox")), "0.00", dataView(0).Item("Research_DevelopmentBox"))
            Else
                AddRemoveHandlerStickPacks(False)
                RemoveHandler TextBox8.TextChanged, AddressOf TextBox8_TextChanged
                RemoveHandler ComboBox6.TextChanged, AddressOf PritingCostStandardStickpaks
                RemoveHandler ComboBox7.TextChanged, AddressOf PritingCostStandardStickpaks
                RemoveHandler ComboBox8.TextChanged, AddressOf PritingCostStandardStickpaks
                RemoveHandler cmbstickpanelColors.TextChanged, AddressOf PritingCostStandardStickpaks
                ComboBox6.SelectedIndex = -1
                ComboBox7.SelectedIndex = -1
                ComboBox8.SelectedIndex = -1
                cmbstickpanelColors.SelectedIndex = -1
                ComboBox6.Text = ""
                ComboBox7.Text = ""
                ComboBox8.Text = ""
                cmbstickpanelColors.Text = ""

                TextBox16.Text = ServingSizeTextBox.Text

                txtStickpackBoxRD.Text = "0.00"
                txtStickpackBulkRD.Text = "0.00"

                TextBox18.Text = "0.00"
                TextBox19.Text = "0.00"
                TextBox20.Text = "0.00"
                TextBox21.Text = "0.00"
                TextBox22.Text = "0.00"
                TextBox23.Text = "0.00"
                TextBox24.Text = "0.00"
                TextBox25.Text = ""
                txtShrinkWrapStickPack.Text = "0.00"
                txtWaferSeal.Text = "0.00"
                txtStickPacks.Text = ""
                txtStickPacksShipperCaseCount.Text = ""
                txtStickPacksShipperCaseCountAmt.Text = "0.00"
                'Changes by Payal P
                'Display Box
                TextBox39DisplayQunatity.Text = ""
                TextBox38DisplayDesc.Text = ""
                TextBox43DisplaySpec.Text = ""
                TextBox42DisplayBoardGrade.Text = ".018 SBS C1S"
                TextBox41DisplayDimension.Text = ""
                TextBox40DisplayProductStyle.Text = "TT AGB-T Style"
                'Bulk
                txtStickPrintingPlateBulk.Text = "0.00"
                txtStickArtPrepBulk.Text = "0.00"
                txtStickOtherCostBulk.Text = "0.00"
                txtStickShipperCostBulk.Text = "0.00"
                txtStickOtherDescBulk.Text = ""
                txtStickShipperCountBulk.Text = ""


                AddHandler TextBox8.TextChanged, AddressOf TextBox8_TextChanged
                AddHandler ComboBox6.TextChanged, AddressOf PritingCostStandardStickpaks
                AddHandler ComboBox7.TextChanged, AddressOf PritingCostStandardStickpaks
                AddHandler ComboBox8.TextChanged, AddressOf PritingCostStandardStickpaks
                AddHandler cmbstickpanelColors.TextChanged, AddressOf PritingCostStandardStickpaks
                AddRemoveHandlerStickPacks(True)
            End If

            dataView = New DataView(dsStandupBags.Tables("StandUpBag"), "AddedFrom ='Stick Packs' AND VersionNumber=" & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "StandUpBagID", DataViewRowState.CurrentRows)

            If dataView.Count > 0 Then
                AddRemoveHandlerStickPacks(False)
                txtStickPackBagDescription.Text = If(String.IsNullOrEmpty(dataView(0).Item("Description_")) OrElse IsDBNull(dataView(0).Item("Description_")), "", dataView(0).Item("Description_"))
                txtStickPackBagQty.Text = If(IsDBNull(dataView(0).Item("QuantityBag")), "0", Convert.ToInt32(dataView(0).Item("QuantityBag")).ToString("N0", culture))
                txtStickPackBagMaterial.Text = If(String.IsNullOrEmpty(dataView(0).Item("MaterialBag")) OrElse IsDBNull(dataView(0).Item("MaterialBag")), "", dataView(0).Item("MaterialBag"))
                txtStickPackBagSize.Text = If(String.IsNullOrEmpty(dataView(0).Item("SizeBag")) OrElse IsDBNull(dataView(0).Item("SizeBag")), "", dataView(0).Item("SizeBag"))
                txtStickPackBagPrintColors.Text = If(String.IsNullOrEmpty(dataView(0).Item("PrintColor")) OrElse IsDBNull(dataView(0).Item("PrintColor")), "", dataView(0).Item("PrintColor"))

                cmbStickPackBagTearNotch.SelectedIndex = If(dataView(0).Item("TearNotch").ToString() = "False", 1, 0)
                cmbStickPackBagHangerHole.SelectedIndex = If(dataView(0).Item("HangerHole").ToString() = "False", 1, 0)
                cmbStickPackBagZipper.SelectedIndex = If(dataView(0).Item("Zipper").ToString() = "False", 1, 0)

                txtStickPackBagPrintingCost.Text = dataView(0).Item("PrintingCostBag")

                txtStickPackBagServings.Text = dataView(0).Item("Servings")
                txtStickPackBagServingWeight.Text = dataView(0).Item("ServingWeight")
                txtStickPackBagFillWeight.Text = dataView(0).Item("FillWeight")
                'Additional Charges
                txtPrintingPlatesStickBag.Text = If(IsDBNull(dataView(0).Item("PrintingPlatesBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("PrintingPlatesBag")), "0.00", dataView(0).Item("PrintingPlatesBag"))
                txtArtPreStickBag.Text = If(IsDBNull(dataView(0).Item("ArtPreparationBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("ArtPreparationBag")), "0.00", dataView(0).Item("ArtPreparationBag"))
                txtSecondaryPackStickBag.Text = If(IsDBNull(dataView(0).Item("SecondaryPackout")) OrElse String.IsNullOrEmpty(dataView(0).Item("SecondaryPackout")), "0.00", dataView(0).Item("SecondaryPackout"))
                txtOtherCostStickBag.Text = If(IsDBNull(dataView(0).Item("Other")) OrElse String.IsNullOrEmpty(dataView(0).Item("Other")), "0.00", dataView(0).Item("Other"))
                txtShipperCostStickBag.Text = If(IsDBNull(dataView(0).Item("Shipper_CaseAmt")) OrElse String.IsNullOrEmpty(dataView(0).Item("Shipper_CaseAmt")), "0.00", dataView(0).Item("Shipper_CaseAmt"))
                txtOtherStickBag.Text = If(IsDBNull(dataView(0).Item("Other_Description")) OrElse String.IsNullOrEmpty(dataView(0).Item("Other_Description")), "", dataView(0).Item("Other_Description"))
                txtShipperCountStickBag.Text = If(IsDBNull(dataView(0).Item("Shipper_CaseCount")) OrElse String.IsNullOrEmpty(dataView(0).Item("Shipper_CaseCount")), "", dataView(0).Item("Shipper_CaseCount"))
                txtStickpackBagRD.Text = If(IsDBNull(dataView(0).Item("Research_DevelopmentBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("Research_DevelopmentBag")), "", dataView(0).Item("Research_DevelopmentBag"))
                AddRemoveHandlerStickPacks(True)
            Else
                AddRemoveHandlerStickPacks(False)

                txtStickPackBagDescription.Text = "Standup Bag"
                txtStickPackBagQty.Text = "0"
                txtStickPackBagMaterial.Text = "1 MIL Matte/48ga MET PET/3.5mil PE"
                txtStickPackBagSize.Text = ""
                txtStickPackBagPrintColors.Text = "4 Color Process & White"
                cmbStickPackBagTearNotch.SelectedIndex = 0
                cmbStickPackBagHangerHole.SelectedIndex = 1
                cmbStickPackBagZipper.SelectedIndex = 0
                txtStickPackBagPrintingCost.Text = "0.00"

                txtStickPackBagServings.Text = ""
                txtStickPackBagServingWeight.Text = ServingSizeTextBox.Text
                txtStickPackBagFillWeight.Text = "0.00"
                'Additional Charges
                txtPrintingPlatesStickBag.Text = "0.00"
                txtArtPreStickBag.Text = "0.00"
                txtSecondaryPackStickBag.Text = "0.00"
                txtOtherCostStickBag.Text = "0.00"
                txtShipperCostStickBag.Text = "0.00"
                txtOtherStickBag.Text = ""
                txtShipperCountStickBag.Text = ""
                txtStickpackBagRD.Text = "0.00"
                AddRemoveHandlerStickPacks(True)
            End If
            txtStickPackBagFillWeight.Text = If(String.IsNullOrEmpty(txtStickPackBagServingWeight.Text), "0", Convert.ToDecimal(txtStickPackBagServingWeight.Text)) * If(String.IsNullOrEmpty(txtStickPackBagServings.Text), "0", Convert.ToInt16(txtStickPackBagServings.Text))

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DisplaySachets(selectedVersion As String)
        Try
            Dim dataView As DataView
            dataView = New DataView(dsPSa.Tables("sachets"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "StickPackDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                txtStickPacksSachets.Text = If(IsDBNull(dataView(0).Item("Sachets")) OrElse String.IsNullOrEmpty(dataView(0).Item("Sachets")), "", dataView(0).Item("Sachets"))
                txtWaferSealSachets.Text = If(IsDBNull(dataView(0).Item("WaferSeal")) OrElse String.IsNullOrEmpty(dataView(0).Item("WaferSeal")), "", dataView(0).Item("WaferSeal"))
                txtShrinkWrapSachets.Text = If(IsDBNull(dataView(0).Item("ShrinkWrap")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShrinkWrap")), "", dataView(0).Item("ShrinkWrap"))
                ComboBox11.Text = dataView(0).Item("SizeCount")
                ComboBox10.Text = dataView(0).Item("Material")
                ComboBox9.Text = If(IsDBNull(dataView(0).Item("Quantity")) Or String.IsNullOrEmpty(dataView(0).Item("Quantity")), "", Convert.ToInt32(dataView(0).Item("Quantity").ToString().Replace(",", "")).ToString("N0", culture))
                ComboBox12.Text = dataView(0).Item("Colors")

                If FormulaTypeCmbBox.SelectedIndex <> 0 Then
                    TextBox35.Text = If(IsDBNull(dataView(0).Item("PacketContents")) OrElse String.IsNullOrEmpty(dataView(0).Item("PacketContents")), "1", Convert.ToInt16(dataView(0).Item("PacketContents")))
                Else
                    TextBox35.Text = dataView(0).Item("PacketContents")
                End If
                TextBox33.Text = If(IsDBNull(dataView(0).Item("FillingCost")) OrElse String.IsNullOrEmpty(dataView(0).Item("FillingCost")), 0, dataView(0).Item("FillingCost"))
                TextBox28.Text = If(IsDBNull(dataView(0).Item("PrintingCost")) OrElse String.IsNullOrEmpty(dataView(0).Item("PrintingCost")), 0, dataView(0).Item("PrintingCost"))
                TextBox27.Text = If(IsDBNull(dataView(0).Item("DisplayCost")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayCost")), 0, dataView(0).Item("DisplayCost"))
                TextBox32.Text = If(IsDBNull(dataView(0).Item("PrintingPlates")) OrElse String.IsNullOrEmpty(dataView(0).Item("PrintingPlates")), 0, dataView(0).Item("PrintingPlates"))
                TextBox31.Text = If(IsDBNull(dataView(0).Item("ArtPreparation")) OrElse String.IsNullOrEmpty(dataView(0).Item("ArtPreparation")), 0, dataView(0).Item("ArtPreparation"))
                TextBox30.Text = If(IsDBNull(dataView(0).Item("PackOut")) OrElse String.IsNullOrEmpty(dataView(0).Item("PackOut")), 0, dataView(0).Item("PackOut"))
                TextBox29.Text = If(IsDBNull(dataView(0).Item("OtherCostValue")) OrElse String.IsNullOrEmpty(dataView(0).Item("OtherCostValue")), 0, dataView(0).Item("OtherCostValue"))
                TextBox26.Text = If(IsDBNull(dataView(0).Item("OtherCostDesc")) OrElse String.IsNullOrEmpty(dataView(0).Item("OtherCostDesc")), "", dataView(0).Item("OtherCostDesc"))
                txtSachetsShipperCaseCount.Text = If(IsDBNull(dataView(0).Item("ShipperCaseCountDesc")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCountDesc")), "", dataView(0).Item("ShipperCaseCountDesc"))
                txtSachetsShipperCaseCountAmt.Text = If(IsDBNull(dataView(0).Item("ShipperCaseCountValue")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCountValue")), "0.00", dataView(0).Item("ShipperCaseCountValue"))
                'Changes by Payal P
                'Display Box  
                TextBox38DisplayQty.Text = If(IsDBNull(dataView(0).Item("DisplayQuantity")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayQuantity")), 0, Convert.ToInt32(dataView(0).Item("DisplayQuantity")).ToString("N0", culture))
                TextBox39Desc.Text = If(IsDBNull(dataView(0).Item("DisplayDescription")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayDescription")), "", dataView(0).Item("DisplayDescription"))
                TextBox44Spec.Text = If(IsDBNull(dataView(0).Item("DisplaySpec")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplaySpec")), "", dataView(0).Item("DisplaySpec"))
                TextBox43BoardGrade.Text = If(IsDBNull(dataView(0).Item("DisplayBoardGrade")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayBoardGrade")), ".018 SBS C1S", dataView(0).Item("DisplayBoardGrade"))
                TextBox42Dimension.Text = If(IsDBNull(dataView(0).Item("DisplayDimension")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayDimension")), "", dataView(0).Item("DisplayDimension"))
                TextBox41ProductStyle.Text = If(IsDBNull(dataView(0).Item("DisplayProductStyle")) OrElse String.IsNullOrEmpty(dataView(0).Item("DisplayProductStyle")), "TT AGB-T Style", dataView(0).Item("DisplayProductStyle"))
                'BULK
                txtSachetPrintingPlatesBulk.Text = If(IsDBNull(dataView(0).Item("PrintingPlatesBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("PrintingPlatesBulk")), 0, dataView(0).Item("PrintingPlatesBulk"))
                txtSachetArtPrepBulk.Text = If(IsDBNull(dataView(0).Item("ArtPreparationBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("ArtPreparationBulk")), 0, dataView(0).Item("ArtPreparationBulk"))
                txtSachetOtherCostDesc.Text = If(IsDBNull(dataView(0).Item("OtherCostDescBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("OtherCostDescBulk")), "", dataView(0).Item("OtherCostDescBulk"))
                txtSachetOtherCost.Text = If(IsDBNull(dataView(0).Item("OtherCostBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("OtherCostBulk")), 0, dataView(0).Item("OtherCostBulk"))
                txtSachetShipperCountBulk.Text = If(IsDBNull(dataView(0).Item("ShipperCaseCountBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCountBulk")), "", dataView(0).Item("ShipperCaseCountBulk"))
                txtSachetShipperCost.Text = If(IsDBNull(dataView(0).Item("ShipperCaseCostBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("ShipperCaseCostBulk")), 0, dataView(0).Item("ShipperCaseCostBulk"))
                txtSachetsBoxRD.Text = If(IsDBNull(dataView(0).Item("Research_Development")) OrElse String.IsNullOrEmpty(dataView(0).Item("Research_Development")), 0, dataView(0).Item("Research_Development"))
                txtSachetsBulkRD.Text = If(IsDBNull(dataView(0).Item("Research_DevelopmentBulk")) OrElse String.IsNullOrEmpty(dataView(0).Item("Research_DevelopmentBulk")), 0, dataView(0).Item("Research_DevelopmentBulk"))

            Else
                AddRemoveSachets(False)
                RemoveHandler TextBox8.TextChanged, AddressOf TextBox8_TextChanged
                RemoveHandler ComboBox11.TextChanged, AddressOf PritingCostStandardSachets
                RemoveHandler ComboBox10.TextChanged, AddressOf PritingCostStandardSachets
                RemoveHandler ComboBox9.TextChanged, AddressOf PritingCostStandardSachets
                RemoveHandler ComboBox12.TextChanged, AddressOf PritingCostStandardSachets
                ComboBox11.Text = ""
                ComboBox10.Text = ""
                ComboBox9.Text = ""
                ComboBox12.Text = ""
                txtStickPacksSachets.Text = "0"
                txtWaferSealSachets.Text = "0.00"
                txtShrinkWrapSachets.Text = "0.00"
                ComboBox11.SelectedIndex = -1
                ComboBox10.SelectedIndex = -1
                ComboBox9.SelectedIndex = -1
                ComboBox12.SelectedIndex = -1
                RemoveHandler TextBox35.TextChanged, AddressOf Sachets_TextBox_TextChanged
                TextBox35.Text = ServingSizeTextBox.Text
                AddHandler TextBox35.TextChanged, AddressOf Sachets_TextBox_TextChanged
                txtSachetsBoxRD.Text = "0.00"
                txtSachetsBulkRD.Text = "0.00"
                TextBox33.Text = "0.00"
                TextBox28.Text = "0.00"
                TextBox27.Text = "0.00"
                TextBox32.Text = "0.00"
                TextBox31.Text = "0.00"
                TextBox30.Text = "0.00"
                TextBox29.Text = "0.00"
                TextBox26.Text = ""
                txtSachetsShipperCaseCount.Text = ""
                txtSachetsShipperCaseCountAmt.Text = "0.00"
                TextBox38DisplayQty.Text = ""
                TextBox39Desc.Text = ""
                TextBox44Spec.Text = ""
                TextBox43BoardGrade.Text = ".018 SBS C1S "
                TextBox42Dimension.Text = ""
                TextBox41ProductStyle.Text = "TT AGB-T Style"
                'BULK
                txtSachetPrintingPlatesBulk.Text = "0.00"
                txtSachetArtPrepBulk.Text = "0.00"
                txtSachetOtherCostDesc.Text = ""
                txtSachetOtherCost.Text = "0.00"
                txtSachetShipperCountBulk.Text = ""
                txtSachetShipperCost.Text = "0.00"

                AddHandler ComboBox11.TextChanged, AddressOf PritingCostStandardSachets
                AddHandler ComboBox10.TextChanged, AddressOf PritingCostStandardSachets
                AddHandler ComboBox9.TextChanged, AddressOf PritingCostStandardSachets
                AddHandler ComboBox12.TextChanged, AddressOf PritingCostStandardSachets
                AddHandler TextBox8.TextChanged, AddressOf TextBox8_TextChanged
                AddRemoveSachets(True)
            End If

            dataView = New DataView(dsStandupBags.Tables("StandUpBag"), "AddedFrom ='Sachets' AND VersionNumber=" & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "StandUpBagID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                AddRemoveSachets(False)

                txtSachetsBagDescription.Text = If(String.IsNullOrEmpty(dataView(0).Item("Description_")) OrElse IsDBNull(dataView(0).Item("Description_")), "", dataView(0).Item("Description_"))
                txtSachetsBagQty.Text = If(IsDBNull(dataView(0).Item("QuantityBag")), "0", Convert.ToInt32(dataView(0).Item("QuantityBag")).ToString("N0", culture))
                txtSachetsBagMaterial.Text = If(String.IsNullOrEmpty(dataView(0).Item("MaterialBag")) OrElse IsDBNull(dataView(0).Item("MaterialBag")), "", dataView(0).Item("MaterialBag"))
                txtSachetsBagSize.Text = If(String.IsNullOrEmpty(dataView(0).Item("SizeBag")) OrElse IsDBNull(dataView(0).Item("SizeBag")), "", dataView(0).Item("SizeBag"))
                txtSachetsBagPrintColors.Text = If(String.IsNullOrEmpty(dataView(0).Item("PrintColor")) OrElse IsDBNull(dataView(0).Item("PrintColor")), "", dataView(0).Item("PrintColor"))

                cmbSachetsBagTearNotch.SelectedIndex = If(dataView(0).Item("TearNotch").ToString() = "False", 1, 0)
                cmbSachetsBagHangerHole.SelectedIndex = If(dataView(0).Item("HangerHole").ToString() = "False", 1, 0)
                cmbSachetsBagZipper.SelectedIndex = If(dataView(0).Item("Zipper").ToString() = "False", 1, 0)

                txtSachetsBagPrintingCost.Text = dataView(0).Item("PrintingCostBag")

                txtSachetsBagServings.Text = dataView(0).Item("Servings")
                txtSachetsBagServingWeight.Text = dataView(0).Item("ServingWeight")
                txtSachetsBagFillWeight.Text = dataView(0).Item("FillWeight")
                'Additional Charges
                txtSachetsBagPrintingPlates.Text = If(IsDBNull(dataView(0).Item("PrintingPlatesBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("PrintingPlatesBag")), "0.00", dataView(0).Item("PrintingPlatesBag"))
                txtSachetsBagArtPreparation.Text = If(IsDBNull(dataView(0).Item("ArtPreparationBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("ArtPreparationBag")), "0.00", dataView(0).Item("ArtPreparationBag"))
                txtSachetsBagSecondaryPackout.Text = If(IsDBNull(dataView(0).Item("SecondaryPackout")) OrElse String.IsNullOrEmpty(dataView(0).Item("SecondaryPackout")), "0.00", dataView(0).Item("SecondaryPackout"))
                txtSachetsBagOther.Text = If(IsDBNull(dataView(0).Item("Other")) OrElse String.IsNullOrEmpty(dataView(0).Item("Other")), "0.00", dataView(0).Item("Other"))
                txtSachetsBagShipperCaseCost.Text = If(IsDBNull(dataView(0).Item("Shipper_CaseAmt")) OrElse String.IsNullOrEmpty(dataView(0).Item("Shipper_CaseAmt")), "0.00", dataView(0).Item("Shipper_CaseAmt"))
                txtSachetsBagOtherDescription.Text = If(IsDBNull(dataView(0).Item("Other_Description")) OrElse String.IsNullOrEmpty(dataView(0).Item("Other_Description")), "", dataView(0).Item("Other_Description"))
                txtSachetsBagShipperCaseCount.Text = If(IsDBNull(dataView(0).Item("Shipper_CaseCount")) OrElse String.IsNullOrEmpty(dataView(0).Item("Shipper_CaseCount")), "", dataView(0).Item("Shipper_CaseCount"))
                txtSachetsBagRD.Text = If(IsDBNull(dataView(0).Item("Research_DevelopmentBag")) OrElse String.IsNullOrEmpty(dataView(0).Item("Research_DevelopmentBag")), "", dataView(0).Item("Research_DevelopmentBag"))
                AddRemoveSachets(True)
            Else
                AddRemoveSachets(False)
                txtSachetsBagDescription.Text = "Standup Bag"
                txtSachetsBagQty.Text = "0"
                txtSachetsBagMaterial.Text = "1 MIL Matte/48ga MET PET/3.5mil PE"
                txtSachetsBagSize.Text = ""
                txtSachetsBagPrintColors.Text = "4 Color Process & White"
                cmbSachetsBagTearNotch.SelectedIndex = 0
                cmbSachetsBagHangerHole.SelectedIndex = 1
                cmbSachetsBagZipper.SelectedIndex = 0
                txtSachetsBagPrintingCost.Text = "0.00"

                txtSachetsBagServings.Text = ""
                txtSachetsBagServingWeight.Text = ServingSizeTextBox.Text
                txtSachetsBagFillWeight.Text = "0.00"
                'Additional Charges
                txtSachetsBagPrintingPlates.Text = "0.00"
                txtSachetsBagArtPreparation.Text = "0.00"
                txtSachetsBagSecondaryPackout.Text = "0.00"
                txtSachetsBagOther.Text = "0.00"
                txtSachetsBagShipperCaseCost.Text = "0.00"
                txtSachetsBagOtherDescription.Text = ""
                txtSachetsBagShipperCaseCount.Text = ""
                txtSachetsBagRD.Text = "0.00"
                AddRemoveSachets(True)
            End If
            txtSachetsBagFillWeight.Text = If(String.IsNullOrEmpty(txtSachetsBagServingWeight.Text), "0", Convert.ToDecimal(txtSachetsBagServingWeight.Text)) * If(String.IsNullOrEmpty(txtSachetsBagServings.Text), "0", Convert.ToInt16(txtSachetsBagServings.Text))

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DisplayEncapsulation(selectedVersion As String)
        Try
            Dim dataView As DataView
            'Encapsulation
            dataView = New DataView(dsEi.Tables("encapsulation"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "EncapsulationDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                AddRemoveEncapsulation(False)
                ComboBox5.Text = If(String.IsNullOrEmpty(dataView(0).Item("TypeofCapsule")) OrElse IsDBNull(dataView(0).Item("TypeofCapsule")), "", dataView(0).Item("TypeofCapsule"))
                TextBox1.Text = If(String.IsNullOrEmpty(dataView(0).Item("CapsuleCost")) OrElse IsDBNull(dataView(0).Item("CapsuleCost")), "", dataView(0).Item("CapsuleCost"))
                TextBox7.Text = If(String.IsNullOrEmpty(dataView(0).Item("EncapsulationCost")) OrElse IsDBNull(dataView(0).Item("EncapsulationCost")), "", dataView(0).Item("EncapsulationCost"))
                TextBox11.Text = If(String.IsNullOrEmpty(dataView(0).Item("WastageValue")) OrElse IsDBNull(dataView(0).Item("WastageValue")), "", dataView(0).Item("WastageValue"))
                TextBox10.Text = If(String.IsNullOrEmpty(dataView(0).Item("WastagePercentage")) OrElse IsDBNull(dataView(0).Item("WastagePercentage")), "", dataView(0).Item("WastagePercentage"))
                txtlabcost.Text = If(IsDBNull(dataView(0).Item("LabCost")) OrElse String.IsNullOrEmpty(dataView(0).Item("LabCost")), "0.00", dataView(0).Item("LabCost"))
                txtCapsuleColor.Text = If(IsDBNull(dataView(0).Item("color")) OrElse String.IsNullOrEmpty(dataView(0).Item("color")), "Clear", dataView(0).Item("color"))
                AddRemoveEncapsulation(True)
            Else
                AddRemoveEncapsulation(False)
                ComboBox5.SelectedIndex = -1
                txtCapsuleColor.Text = "Clear"
                TextBox1.Text = "0.00"
                TextBox7.Text = "0.00"
                TextBox11.Text = "0.00"
                TextBox10.Text = "0.00"
                txtlabcost.Text = "0.00"
                AddRemoveEncapsulation(True)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DisplayTableting(selectedVersion As String)
        Try
            Dim dataView As DataView
            'Tableting
            dataView = New DataView(dsTi.Tables("tableting"), "VersionNumber = " & If(String.IsNullOrEmpty(selectedVersion), 0, selectedVersion), "TabletingDetailID", DataViewRowState.CurrentRows)
            If dataView.Count > 0 Then
                AddRemoveTableting(False)
                ComboBox2.Text = If(String.IsNullOrEmpty(dataView(0).Item("TypeofCoating")) OrElse IsDBNull(dataView(0).Item("TypeofCoating")), "", dataView(0).Item("TypeofCoating"))
                TextBox2.Text = If(String.IsNullOrEmpty(dataView(0).Item("CoatingCost")) OrElse IsDBNull(dataView(0).Item("CoatingCost")), "", dataView(0).Item("CoatingCost"))
                TextBox3.Text = If(String.IsNullOrEmpty(dataView(0).Item("CompressionCost")) OrElse IsDBNull(dataView(0).Item("CompressionCost")), "", dataView(0).Item("CompressionCost"))
                TextBox4.Text = If(String.IsNullOrEmpty(dataView(0).Item("WastageValue")) OrElse IsDBNull(dataView(0).Item("WastageValue")), "", dataView(0).Item("WastageValue"))
                TextBox5.Text = If(String.IsNullOrEmpty(dataView(0).Item("WastagePercentage")) OrElse IsDBNull(dataView(0).Item("WastagePercentage")), "", dataView(0).Item("WastagePercentage"))
                TextBox6.Text = If(String.IsNullOrEmpty(dataView(0).Item("LabCost")) OrElse IsDBNull(dataView(0).Item("LabCost")), "", dataView(0).Item("LabCost"))
                AddRemoveTableting(True)
            Else
                AddRemoveTableting(False)
                ComboBox2.SelectedIndex = -1
                TextBox2.Text = "0.00"
                TextBox3.Text = "0.00"
                TextBox4.Text = "0.00"
                TextBox5.Text = "0.00"
                TextBox6.Text = "0.00"
                AddRemoveTableting(True)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DeleteSales(packagingTypeIndex As Int16, VersionNumber As String, Optional type As String = "")
        Try
            If type = "" Then
                deletedRows = dsSal.Tables("salestxns").Select("VersionNumber =" & VersionNumber & " AND PackagingFormat <>" & packagingTypeIndex)
                For Each dataRow As DataRow In deletedRows
                    dataRow.Delete()
                Next
            Else
                'Delete Box Data when Display Box Check box is not checked
                deletedRows = dsSalBox.Tables("salesBoxTxns").Select("VersionNumber =" & VersionNumber & " AND `TYPE`='BOX' AND PackagingFormat <>" & packagingTypeIndex)
                For Each dataRow As DataRow In deletedRows
                    dataRow.Delete()
                Next
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DeleteBottle(VersionNumber As String)
        Try
            deletedRows = dsPB.Tables("bottles").Select("VersionNumber =" & If(String.IsNullOrEmpty(VersionNumber), "", VersionNumber))
            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next
            ComboBox4.Items.Clear()
            ComboBox4.Text = ""
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DeleteBags(VersionNumber As String, Optional PackagingType As String = "")
        Try
            If String.IsNullOrEmpty(PackagingType) Then
                deletedRows = dsStandupBags.Tables("StandUpBag").Select("VersionNumber =" & VersionNumber)
            Else
                deletedRows = dsStandupBags.Tables("StandUpBag").Select("AddedFrom <> '" & PackagingType & "' AND VersionNumber =" & VersionNumber)
            End If

            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DeleteBlister(VersionNumber As String)
        Try
            deletedRows = dsPBlisters.Tables("blisters").Select("VersionNumber =" & VersionNumber)
            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DeleteStickPacks(VersionNumber As String)
        Try

            deletedRows = dsStandupBags.Tables("StandUpBag").Select("AddedFrom = 'Stick Packs' AND VersionNumber =" & VersionNumber)

            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next

            deletedRows = dsPSP.Tables("stickpacks").Select("VersionNumber =" & VersionNumber)
            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DeleteSachets(VersionNumber As String)
        Try
            deletedRows = dsStandupBags.Tables("StandUpBag").Select("AddedFrom = 'Sachets' AND VersionNumber =" & VersionNumber)

            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next

            deletedRows = dsPSa.Tables("sachets").Select("VersionNumber =" & VersionNumber)
            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DeleteEncapsulation(VersionNumber As String)
        Try
            deletedRows = dsEi.Tables("encapsulation").Select("VersionNumber =" & VersionNumber)
            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DeleteBlending(VersionNumber As String)
        Try
            deletedRows = dsBi.Tables("blending").Select("VersionNumber =" & VersionNumber)
            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub DeleteTableting(VersionNumber As String)
        Try
            deletedRows = dsTi.Tables("tableting").Select("VersionNumber =" & VersionNumber)
            For Each dataRow As DataRow In deletedRows
                dataRow.Delete()
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub
    Private Sub SetComboBox3Index()
        Try
            If FormulaID.Text <> -1 Then
                Dim dataRow() As DataRow = dsFV.Tables("versions").Select("VersionNumber =" & If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text))

                If dataRow.Count > 0 Then
                    Dim PackagingType As String = If(IsDBNull(dataRow(0).Item("PackagingType")), "Bulk", dataRow(0).Item("PackagingType"))
                    Dim PackagingTypeIndex As Int16 = ComboBox3.FindStringExact(PackagingType)
                    ComboBox3.SelectedIndex = PackagingTypeIndex
                    Combobox3Text = ComboBox3.Text

                    'Enable Disable Display Box CheckBox
                    If dataRow(0).Item("IsBox").ToString() = "1" Then
                        chkDisplayBox.Checked = True
                    ElseIf dataRow(0).Item("IsBox").ToString() = "0" Then
                        chkDisplayBox.Checked = False
                    End If
                    If dataRow(0).Item("IsBag").ToString() = "1" Then
                        chkDisplayBag.Checked = True
                    ElseIf dataRow(0).Item("IsBag").ToString() = "0" Then
                        chkDisplayBag.Checked = False
                    End If
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub UpdatePackagingType()
        Try
            If dsFV.Tables.Contains("versions") Then
                If dsFV.Tables("versions").Rows.Count > 0 Then
                    Dim dataView As DataView = New DataView(dsFV.Tables("versions"), "VersionNumber=" & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)
                    If dataView.Count > 0 Then
                        dataView(0).Item("PackagingType") = ComboBox3.Text
                    End If
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub AddRemoveHandlerBags(isAddHandler As Boolean)
        Try
            For Each ctl In BagsPanel.Controls
                If TypeOf ctl Is TextBox Or TypeOf ctl Is ComboBox Or TypeOf ctl Is ListBox Then
                    If TypeOf ctl Is TextBox Then
                        Dim tb As TextBox = ctl
                        If isAddHandler Then
                            AddHandler tb.TextChanged, AddressOf Standup_BagUpdate
                        Else
                            RemoveHandler tb.TextChanged, AddressOf Standup_BagUpdate
                        End If
                    End If
                    If TypeOf ctl Is ListBox Then
                        Dim lb As ListBox = ctl
                        If isAddHandler Then
                            AddHandler lb.Click, AddressOf Standup_BagUpdate
                        Else
                            RemoveHandler lb.Click, AddressOf Standup_BagUpdate
                        End If
                    End If
                    If TypeOf ctl Is ComboBox Then
                        Dim tb As ComboBox = ctl
                        If isAddHandler Then
                            AddHandler tb.SelectedIndexChanged, AddressOf Standup_BagUpdate
                        Else
                            RemoveHandler tb.SelectedIndexChanged, AddressOf Standup_BagUpdate
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub


    Private Sub AddRemoveHandlerStickPacks(isAddHandler As Boolean)
        Try
            For Each ctl In StickPacksPanel.Controls
                If TypeOf ctl Is TextBox Then
                    Dim tb As TextBox = ctl
                    If isAddHandler Then
                        AddHandler tb.TextChanged, AddressOf StickPack_TextBox_TextChanged
                    Else
                        RemoveHandler tb.TextChanged, AddressOf StickPack_TextBox_TextChanged
                    End If
                End If
                If TypeOf ctl Is ComboBox Then
                    Dim cmb As ComboBox = ctl
                    If isAddHandler Then
                        AddHandler cmb.TextChanged, AddressOf StickPack_TextBox_TextChanged
                    Else
                        RemoveHandler cmb.TextChanged, AddressOf StickPack_TextBox_TextChanged
                    End If
                ElseIf TypeOf ctl Is GroupBox Then

                    Dim grp As GroupBox = DirectCast(ctl, GroupBox)

                    If grp.Name = "grpBoxStickPackBag" Then
                        For Each Child As Control In grpBoxStickPackBag.Controls
                            If TypeOf Child Is TextBox Or TypeOf Child Is ComboBox Or TypeOf Child Is ListBox Then
                                If TypeOf Child Is TextBox Then
                                    Dim tb As TextBox = Child
                                    If isAddHandler Then
                                        'AddHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                                        If tb.Tag = "StickPacks" Then
                                            AddHandler tb.TextChanged, AddressOf StickPack_TextBox_TextChanged
                                        Else
                                            AddHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                                        End If

                                    Else
                                        'RemoveHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                                        If tb.Tag = "StickPacks" Then
                                            AddHandler tb.TextChanged, AddressOf StickPack_TextBox_TextChanged
                                        Else
                                            RemoveHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                                        End If

                                    End If
                                End If
                                If TypeOf Child Is ListBox Then
                                    Dim lb As ListBox = Child
                                    If isAddHandler Then
                                        'AddHandler lb.Click, AddressOf StickPack_BagUpdate
                                        AddHandler lb.Click, AddressOf StickPack_BagUpdate
                                    Else
                                        'RemoveHandler lb.Click, AddressOf StickPack_BagUpdate
                                        RemoveHandler lb.Click, AddressOf StickPack_BagUpdate
                                    End If
                                End If
                                If TypeOf Child Is ComboBox Then
                                    Dim tb As ComboBox = Child
                                    If isAddHandler Then
                                        'AddHandler tb.SelectedIndexChanged, AddressOf StickPack_BagUpdate
                                        AddHandler tb.SelectedIndexChanged, AddressOf StickPack_BagUpdate
                                    Else
                                        'RemoveHandler tb.SelectedIndexChanged, AddressOf StickPack_BagUpdate
                                        RemoveHandler tb.SelectedIndexChanged, AddressOf StickPack_BagUpdate
                                    End If
                                End If
                            End If
                        Next
                    Else
                        For Each child As Control In grp.Controls
                            If TypeOf child Is TextBox Then
                                Dim txt As TextBox = DirectCast(child, TextBox)
                                If isAddHandler Then
                                    AddHandler txt.TextChanged, AddressOf StickPack_TextBox_TextChanged
                                Else
                                    RemoveHandler txt.TextChanged, AddressOf StickPack_TextBox_TextChanged
                                End If
                            End If
                        Next
                    End If

                End If
            Next
            If isAddHandler Then
                'AddHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                AddHandler txtStickPacks.TextChanged, AddressOf StickPack_TextBox_TextChanged
            Else
                'RemoveHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                RemoveHandler txtStickPacks.TextChanged, AddressOf StickPack_TextBox_TextChanged
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub AddRemoveHandlerBlister(isAddHandler As Boolean)
        Try
            For Each ctl In BlistersPanel.Controls
                If TypeOf ctl Is TextBox Or TypeOf ctl Is ComboBox Or TypeOf ctl Is ListBox Or TypeOf ctl Is GroupBox Then
                    If TypeOf ctl Is TextBox Then
                        Dim tb As TextBox = ctl
                        If isAddHandler Then
                            AddHandler tb.LostFocus, AddressOf BlisterUpdate
                        Else
                            RemoveHandler tb.LostFocus, AddressOf BlisterUpdate
                        End If
                    End If
                    If TypeOf ctl Is ListBox Then
                        Dim lb As ListBox = ctl
                        If isAddHandler Then
                            AddHandler lb.Click, AddressOf BlisterUpdate
                        Else
                            RemoveHandler lb.Click, AddressOf BlisterUpdate
                        End If
                    End If
                    If TypeOf ctl Is ComboBox Then
                        Dim tb As ComboBox = ctl
                        If isAddHandler Then
                            AddHandler tb.SelectedIndexChanged, AddressOf BlisterUpdate
                            AddHandler tb.TextChanged, AddressOf BlisterUpdate
                        Else
                            RemoveHandler tb.SelectedIndexChanged, AddressOf BlisterUpdate
                            AddHandler tb.TextChanged, AddressOf BlisterUpdate
                        End If

                    End If
                    If TypeOf ctl Is GroupBox Then
                        Dim grp As GroupBox = DirectCast(ctl, GroupBox)
                        For Each child As Control In grp.Controls
                            If TypeOf child Is TextBox Then
                                Dim txt As TextBox = DirectCast(child, TextBox)
                                If isAddHandler Then
                                    AddHandler txt.TextChanged, AddressOf BlisterUpdate
                                Else
                                    RemoveHandler txt.TextChanged, AddressOf BlisterUpdate
                                End If
                            End If
                            If TypeOf child Is ComboBox Then
                                Dim tb As ComboBox = child
                                If isAddHandler Then
                                    AddHandler tb.SelectedIndexChanged, AddressOf BlisterUpdate
                                    AddHandler tb.TextChanged, AddressOf BlisterUpdate
                                Else
                                    RemoveHandler tb.SelectedIndexChanged, AddressOf BlisterUpdate
                                    AddHandler tb.TextChanged, AddressOf BlisterUpdate
                                End If

                            End If
                        Next
                    End If
                End If
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub AddRemoveSachets(isAddHandler As Boolean)
        Try
            For Each ctl In SachetsPanel.Controls
                If TypeOf ctl Is TextBox Then
                    Dim tb As TextBox = ctl
                    If isAddHandler Then
                        AddHandler tb.TextChanged, AddressOf Sachets_TextBox_TextChanged
                    Else
                        RemoveHandler tb.TextChanged, AddressOf Sachets_TextBox_TextChanged
                    End If
                ElseIf TypeOf ctl Is ComboBox Then
                    Dim cb As ComboBox = ctl
                    If isAddHandler Then
                        AddHandler cb.SelectedIndexChanged, AddressOf Sachets_TextBox_TextChanged
                        AddHandler cb.TextChanged, AddressOf Sachets_TextBox_TextChanged
                    Else
                        RemoveHandler cb.SelectedIndexChanged, AddressOf Sachets_TextBox_TextChanged
                        RemoveHandler cb.TextChanged, AddressOf Sachets_TextBox_TextChanged
                    End If
                ElseIf TypeOf ctl Is GroupBox Then
                    Dim grp As GroupBox = DirectCast(ctl, GroupBox)

                    If grp.Name = "GroupBoxSachetBag" Then
                        For Each Child As Control In GroupBoxSachetBag.Controls
                            If TypeOf Child Is TextBox Or TypeOf Child Is ComboBox Or TypeOf Child Is ListBox Then
                                If TypeOf Child Is TextBox Then
                                    Dim tb As TextBox = Child
                                    If isAddHandler Then
                                        'AddHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                                        If tb.Tag = "Sachets" Then
                                            AddHandler tb.TextChanged, AddressOf Sachets_TextBox_TextChanged
                                        Else
                                            AddHandler tb.TextChanged, AddressOf Sachets_BagUpdate
                                        End If

                                    Else
                                        'RemoveHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                                        If tb.Tag = "Sachets" Then
                                            AddHandler tb.TextChanged, AddressOf Sachets_TextBox_TextChanged
                                        Else
                                            RemoveHandler tb.TextChanged, AddressOf Sachets_BagUpdate
                                        End If

                                    End If
                                End If
                                If TypeOf Child Is ListBox Then
                                    Dim lb As ListBox = Child
                                    If isAddHandler Then
                                        'AddHandler lb.Click, AddressOf StickPack_BagUpdate
                                        AddHandler lb.Click, AddressOf Sachets_BagUpdate
                                    Else
                                        'RemoveHandler lb.Click, AddressOf StickPack_BagUpdate
                                        RemoveHandler lb.Click, AddressOf Sachets_BagUpdate
                                    End If
                                End If
                                If TypeOf Child Is ComboBox Then
                                    Dim tb As ComboBox = Child
                                    If isAddHandler Then
                                        'AddHandler tb.SelectedIndexChanged, AddressOf StickPack_BagUpdate
                                        AddHandler tb.SelectedIndexChanged, AddressOf Sachets_BagUpdate
                                    Else
                                        'RemoveHandler tb.SelectedIndexChanged, AddressOf StickPack_BagUpdate
                                        RemoveHandler tb.SelectedIndexChanged, AddressOf Sachets_BagUpdate
                                    End If
                                End If
                            End If
                        Next
                    Else
                        For Each child As Control In grp.Controls
                            If TypeOf child Is TextBox Then
                                Dim txt As TextBox = DirectCast(child, TextBox)
                                If isAddHandler Then
                                    AddHandler txt.TextChanged, AddressOf Sachets_TextBox_TextChanged
                                Else
                                    RemoveHandler txt.TextChanged, AddressOf Sachets_TextBox_TextChanged
                                End If
                            End If
                        Next
                    End If

                End If
            Next

            If isAddHandler Then
                'AddHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                AddHandler txtStickPacksSachets.TextChanged, AddressOf Sachets_TextBox_TextChanged
            Else
                'RemoveHandler tb.TextChanged, AddressOf StickPack_BagUpdate
                RemoveHandler txtStickPacksSachets.TextChanged, AddressOf Sachets_TextBox_TextChanged
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub AddRemoveBlending(isAddHandler As Boolean)
        Try
            For Each ctl In BlendingPanel.Controls
                If TypeOf ctl Is TextBox Then
                    Dim tb As TextBox = ctl
                    If isAddHandler Then
                        AddHandler tb.TextChanged, AddressOf Blending_TextBox_TextChanged
                    Else
                        RemoveHandler tb.TextChanged, AddressOf Blending_TextBox_TextChanged
                    End If
                End If
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub AddRemoveTableting(isAddHandler As Boolean)
        Try
            For Each ctl In TabletingPanel.Controls
                If TypeOf ctl Is TextBox Then
                    Dim tb As TextBox = ctl
                    If isAddHandler Then
                        AddHandler tb.TextChanged, AddressOf Tableting_TextBox_TextChanged
                    Else
                        RemoveHandler tb.TextChanged, AddressOf Tableting_TextBox_TextChanged
                    End If
                End If
                If TypeOf ctl Is ComboBox Then
                    Dim tb As ComboBox = ctl
                    If isAddHandler Then
                        AddHandler tb.TextChanged, AddressOf Tableting_TextBox_TextChanged
                    Else
                        RemoveHandler tb.TextChanged, AddressOf Tableting_TextBox_TextChanged
                    End If
                End If
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub AddRemoveEncapsulation(isAddHandler As Boolean)
        Try
            For Each ctl In EncapsulationPanel.Controls
                If TypeOf ctl Is TextBox Then
                    Dim tb As TextBox = ctl
                    If isAddHandler Then
                        AddHandler tb.TextChanged, AddressOf Encapsulation_TextBox_TextChanged
                    Else
                        RemoveHandler tb.TextChanged, AddressOf Encapsulation_TextBox_TextChanged
                    End If
                End If
                If TypeOf ctl Is ComboBox Then
                    Dim tb As ComboBox = ctl
                    If isAddHandler Then
                        AddHandler tb.TextChanged, AddressOf Encapsulation_TextBox_TextChanged
                    Else
                        RemoveHandler tb.TextChanged, AddressOf Encapsulation_TextBox_TextChanged
                    End If
                End If
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub MaterialDataGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles MaterialDataGrid.CellClick
        Try
            If MaterialDataGrid.Rows.Count > 0 Then
                Dim MaterialName As String = MaterialDataGrid.CurrentRow.DataBoundItem("MaterialName")
                Dim Supplier = MaterialDataGrid.CurrentRow.DataBoundItem("Supplier")
                If IsReplace Then
                    ReplaceMaterial(MaterialName, Supplier)
                Else
                    AddNewFormulaRow(MaterialName, Supplier)
                End If

                If String.IsNullOrEmpty(TextBox37.Text) Then
                    MaterialDataGrid.Visible = False
                    IsReplace = False
                    IsNewMaterialBelow = False
                End If
                TextBox37.Clear()
                TextBox37.Focus()
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub MaterialDataGrid_KeyDown(sender As Object, e As KeyEventArgs) Handles MaterialDataGrid.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                If MaterialDataGrid.Rows.Count > 0 Then
                    Dim MaterialName As String = MaterialDataGrid.CurrentRow.DataBoundItem("MaterialName")
                    Dim Supplier = MaterialDataGrid.CurrentRow.DataBoundItem("Supplier")
                    If IsReplace Then
                        ReplaceMaterial(MaterialName, Supplier)
                    Else
                        AddNewFormulaRow(MaterialName, Supplier)
                    End If

                    MaterialDataGrid.Visible = False
                    IsNewMaterialBelow = False
                    IsReplace = False
                    TextBox37.Clear()
                    TextBox37.Focus()
                End If
            ElseIf e.KeyCode = Keys.Escape Then
                TextBox37.Clear()
                TextBox37.Focus()
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub ServingWeightLabel_TextChanged(sender As Object, e As EventArgs) Handles ServingWeightLabel.TextChanged
        Try
            If dsFV.Tables.Contains("versions") Then
                Dim dataView As DataView = New DataView(dsFV.Tables("versions"), "VersionNumber=" & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)
                dataView(0).Item("ServingWeight") = ServingWeightLabel.Text
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    Private Sub btnAddSalesRow_Click(sender As Object, e As EventArgs) Handles btnAddBulkSalesRow.Click, btnAddBoxSalesRow.Click, btnAddBulkSalesBag.Click
        Try

            If DirectCast(sender, Button).Name = "btnAddBulkSalesRow" Then

                Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                workRow("FormulaID") = FormulaID.Text
                workRow("PackagingFormat") = ComboBox3.SelectedIndex

                If ComboBox3.SelectedIndex = 0 Then
                    workRow("UnitSize") = 0
                ElseIf ComboBox3.SelectedIndex = 1 Then
                    If ComboBox4.Text = "" Then
                        MessageBox.Show("Please Enter Serving/CT...", "Warning", MessageBoxButtons.OK)
                        Exit Sub
                    Else
                        workRow("UnitSize") = ComboBox4.Text
                    End If

                Else
                    workRow("UnitSize") = ServingSizeTextBox.Text
                End If
                workRow("Type") = "BULK"
                workRow("Quantity") = 0
                workRow("MarginPercentage") = 0
                workRow("SalesPrice") = DBNull.Value
                workRow("OverriddenSalesPrice") = DBNull.Value
                workRow("Overridden") = 0
                workRow("VersionNumber") = VersionCmbBox.Text
                dsSal.Tables("salestxns").Rows.Add(workRow)

                BindingSource2.DataSource = dsSal.Tables("salestxns")
                If ComboBox3.SelectedIndex = 0 Then
                    BindingSource2.Filter = "PackagingFormat =" & ComboBox3.SelectedIndex & " AND [TYPE]='BULK' AND VersionNumber=" & VersionCmbBox.Text
                ElseIf ComboBox3.SelectedIndex = 1 Then
                    BindingSource2.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND [TYPE]='BULK' AND  UnitSize = '" & ComboBox4.Text & "' AND VersionNumber='" & VersionCmbBox.Text & "'"


                Else
                    BindingSource2.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND [TYPE]='BULK' AND VersionNumber='" & VersionCmbBox.Text & "'"
                End If
                CalculateBulkSales(gridBulkSales)

            ElseIf DirectCast(sender, Button).Name = "btnAddBoxSalesRow" Then

                Dim workRow As DataRow = dsSalBox.Tables("salesBoxTxns").NewRow()
                workRow("FormulaID") = FormulaID.Text
                workRow("PackagingFormat") = ComboBox3.SelectedIndex

                If ComboBox3.SelectedIndex = 0 Then
                    workRow("UnitSize") = 0
                ElseIf ComboBox3.SelectedIndex = 1 Then
                    If ComboBox4.Text = "" Then
                        MessageBox.Show("Please Enter Serving/CT...", "Warning", MessageBoxButtons.OK)
                        Exit Sub
                    Else
                        workRow("UnitSize") = ComboBox4.Text
                    End If

                Else
                    workRow("UnitSize") = ServingSizeTextBox.Text
                End If
                workRow("Type") = "BOX"
                workRow("Quantity") = 0
                workRow("MarginPercentage") = 0
                workRow("SalesPrice") = DBNull.Value
                workRow("VersionNumber") = VersionCmbBox.Text
                workRow("OverriddenSalesPrice") = DBNull.Value
                workRow("Overridden") = 0
                dsSalBox.Tables("salesBoxTxns").Rows.Add(workRow)

                BindSalesBoxGrid.DataSource = dsSalBox.Tables("salesBoxTxns")
                BindSalesBoxGrid.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND [TYPE]='BOX' AND VersionNumber='" & VersionCmbBox.Text & "'"
                CalculateBulkSales(gridBoxSales)
            ElseIf DirectCast(sender, Button).Name = "btnAddBulkSalesBag" Then

                Dim workRow As DataRow = dsSalBags.Tables("salesBagsTxns").NewRow()
                workRow("FormulaID") = FormulaID.Text
                workRow("PackagingFormat") = ComboBox3.SelectedIndex
                If ComboBox3.SelectedIndex = 0 Then
                    workRow("UnitSize") = 0
                ElseIf ComboBox3.SelectedIndex = 1 Then
                    If ComboBox4.Text = "" Then
                        MessageBox.Show("Please Enter Serving/CT...", "Warning", MessageBoxButtons.OK)
                        Exit Sub
                    Else
                        workRow("UnitSize") = ComboBox4.Text
                    End If
                Else
                    workRow("UnitSize") = ServingSizeTextBox.Text
                End If
                workRow("Type") = "BULKBAGS"
                workRow("Quantity") = 0
                workRow("MarginPercentage") = 0
                workRow("SalesPrice") = DBNull.Value
                workRow("VersionNumber") = VersionCmbBox.Text
                workRow("OverriddenSalesPrice") = DBNull.Value
                workRow("Overridden") = 0
                dsSalBags.Tables("salesBagsTxns").Rows.Add(workRow)

                BindSalesBulkStickpacksGrid.DataSource = dsSalBags.Tables("salesBagsTxns")
                BindSalesBulkStickpacksGrid.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND [TYPE]='BULKBAGS' AND VersionNumber='" & VersionCmbBox.Text & "'"
                CalculateBulkSales(gridbBulkSalesBags)
            End If

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub chkDisplayBox_CheckedChanged(sender As Object, e As EventArgs) Handles chkDisplayBox.CheckedChanged
        Try
            Dim CurrentVersion = New DataView(dsFV.Tables("versions"), "VersionNumber = " & If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text), "", DataViewRowState.CurrentRows)

            'If chkDisplayBox.Checked And (ComboBox3.Text = "Sachets" Or ComboBox3.Text = "Stick Packs" Or ComboBox3.Text = "Blister") Then
            If chkDisplayBox.Checked Then
                chkDisplayBag.Checked = False
                pnlBagSales.Visible = False
                TotalUnitCost.Text = lblTotalCostBox.Text

                If CurrentVersion.Count > 0 Then
                    CurrentVersion(0)("IsBox") = 1
                End If

                'Calculates Sales Panel
                'Add Bag in Sachets
                If ComboBox3.Text = "Stick Packs" Then
                    GroupBoxStickpacks.Controls.Add(lblStickPacks)
                    GroupBoxStickpacks.Controls.Add(txtStickPacks)
                    GroupBoxStickpacks.Controls.Add(lblStickPacksUnit)
                    lblStickPacks.Location = New Point(13, 201)
                    txtStickPacks.Location = New Point(91, 197)
                    txtStickPacks.Size = New Size(109, 20)
                    lblStickPacksUnit.Location = New Point(203, 200)
                ElseIf ComboBox3.Text = "Sachets" Then
                    GroupBoxSachetBox.Controls.Add(lblStickPacksSachets)
                    GroupBoxSachetBox.Controls.Add(txtStickPacksSachets)
                    GroupBoxSachetBox.Controls.Add(Label73)
                    lblStickPacksSachets.Location = New Point(29, 203)
                    txtStickPacksSachets.Location = New Point(106, 202)
                    txtStickPacksSachets.Size = New Point(109, 20)
                    Label73.Location = New Point(217, 206)
                    Label73.Text = "Per IFC"
                End If
            Else
                If CurrentVersion.Count > 0 Then
                    CurrentVersion(0)("IsBox") = 0
                End If

                If Not chkDisplayBag.Checked Then
                    TotalUnitCost.Text = lblTotalCostBulk.Text
                End If
            End If

            ToggleSalesGrid()

            'Add Bag in Sachets
            If ComboBox3.Text = "Stick Packs" Or ComboBox3.Text = "Sachets" Then
                If pnlBoxSales.Visible = True Then
                    pnlBagSales.Location = New Point(14, 719)
                Else
                    pnlBagSales.Location = New Point(14, 476)
                End If
            Else
                pnlBagSales.Visible = False
            End If

            If ComboBox3.Text = "Stick Packs" Then
                CalculateStickPacksCost()
            ElseIf ComboBox3.Text = "Sachets" Then
                CalculateSachetsCost()
            ElseIf ComboBox3.Text = "Bags" Then
                CalculateStandUpBagCost()
            ElseIf ComboBox3.Text = "Blister" Then
                CalculateBlisterCost()
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub ToggleSalesGrid()
        Try
            If chkDisplayBox.Checked And (ComboBox3.Text = "Sachets" Or ComboBox3.Text = "Stick Packs" Or ComboBox3.Text = "Blister") Then
                pnlBoxSales.Visible = True
                GroupBoxSachetBox.Visible = True
                'GroupBoxSachetAdditionalCharges.Visible = True
                GroupBox1BlisterBox.Visible = True
                GroupBoxStickpacks.Visible = True
                'grpboxadditionalbox.Visible = True

                DisplaySalesData()
            Else
                'TextBox27.Text = "0.00"
                'TextBox20.Text = "0.00"
                'txtAmontDisplaybox.Text = "0.00"
                pnlBoxSales.Visible = False
                GroupBoxSachetBox.Visible = False
                'GroupBoxSachetAdditionalCharges.Visible = False

                GroupBox1BlisterBox.Visible = False
                GroupBoxStickpacks.Visible = False
                'grpboxadditionalbox.Visible = False

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Public Sub CalculateBulkSales(dataGrid As DataGridView)
        Try
            If ComboBox3.Text = "Bulk" Then
                lblAdditionalChargesBulk.Text = 0
                For j As Integer = 0 To dataGrid.Rows.Count - 1
                    CalculateSales(j, TextBox8.Text, dataGrid)
                Next
            Else
                Dim UnitCost = TotalUnitCost.Text

                If dataGrid.Name = "gridBoxSales" Then
                    UnitCost = lblTotalCostBox.Text
                ElseIf dataGrid.Name = "gridBulkSales" Then
                    UnitCost = lblTotalCostBulk.Text
                ElseIf dataGrid.Name = "gridbBulkSalesBags" Then
                    UnitCost = lblTotalCostBag.Text
                End If

                For j As Integer = 0 To dataGrid.Rows.Count - 1
                    CalculateSales(j, UnitCost, dataGrid)
                Next
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView2_RowValidated(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.RowValidated
        CalculateBulkSales(gridBulkSales)
    End Sub
    Private Sub gridBoxSales_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles gridBoxSales.CellEndEdit
        CalculateBulkSales(gridBoxSales)
    End Sub
    Private Sub gridbBulkSalesBags_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles gridbBulkSalesBags.CellEndEdit, DataGridView1.RowValidated
        If chkDisplayBag.Checked Then
            CalculateBulkSales(gridbBulkSalesBags)
        End If
    End Sub
    Private Sub txtStickPackBagServings_TextChanged(sender As Object, e As EventArgs) Handles txtStickPackBagServings.TextChanged
        Try
            Dim serving As Double
            Double.TryParse(txtStickPackBagServings.Text, serving)
            Dim servingsize As Double
            Double.TryParse(ServingSizeTextBox.Text, servingsize)
            txtStickPackBagFillWeight.Text = Double.Parse(serving) * Double.Parse(servingsize)
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub lblTotalCostBulk_TextChanged(sender As Object, e As EventArgs) Handles lblTotalCostBulk.TextChanged
        CalculateBulkSales(gridBulkSales)
    End Sub

    Private Sub lblTotalCostBox_TextChanged(sender As Object, e As EventArgs) Handles lblTotalCostBox.TextChanged
        CalculateBulkSales(gridBoxSales)

    End Sub

    Private Sub lblTotalCostBag_TextChanged(sender As Object, e As EventArgs) Handles lblTotalCostBag.TextChanged
        CalculateBulkSales(gridbBulkSalesBags)
    End Sub

    Private Sub chkDisplayBag_CheckedChanged(sender As Object, e As EventArgs) Handles chkDisplayBag.CheckedChanged
        Try
            If ComboBox3.Text = "Stick Packs" Then
                DisplayStickpacks(VersionCmbBox.Text)
            ElseIf ComboBox3.Text = "Sachets" Then
                DisplaySachets(VersionCmbBox.Text)
            End If

            Dim CurrentVersion = New DataView(dsFV.Tables("versions"), "VersionNumber = " & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)

            Dim DataView = New DataView(dsSalBags.Tables("salesBagsTxns"), "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND Type='BULKBAGS' AND VersionNumber='" & VersionCmbBox.Text & "'", "", DataViewRowState.CurrentRows)

            If chkDisplayBag.Checked And DataView.Count = 0 Then
                populateBagSalesGrid()
            End If

            ' Binding Bag Sales Data to display Data in sales grid
            BindSalesBulkStickpacksGrid.DataSource = dsSalBags.Tables("salesBagsTxns")
            BindSalesBulkStickpacksGrid.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND Type='BULKBAGS' AND VersionNumber='" & VersionCmbBox.Text & "'"

            If chkDisplayBag.Checked Then
                TotalUnitCost.Text = lblTotalCostBag.Text

                If CurrentVersion.Count > 0 Then
                    CurrentVersion(0)("IsBag") = 1
                End If

                chkDisplayBox.Checked = False

                pnlBagSales.Visible = True
                pnlBoxSales.Visible = False
                If pnlBoxSales.Visible = True Then
                    pnlBagSales.Location = New Point(14, 719)
                Else
                    pnlBagSales.Location = New Point(14, 476)
                End If

                'StickPacks
                grpBoxStickPackBag.Visible = True
                grpBoxStickPackBag.Controls.Add(lblStickPacks)
                grpBoxStickPackBag.Controls.Add(txtStickPacks)
                grpBoxStickPackBag.Controls.Add(lblStickPacksUnit)

                lblStickPacks.Location = New Point(24, 312)
                txtStickPacks.Location = New Point(104, 309)
                txtStickPacks.Size = New Size(69, 20)
                lblStickPacksUnit.Location = New Point(181, 311)

                'Add Bag in Sachets
                'Sachets
                GroupBoxSachetBag.Visible = True

                GroupBoxSachetBag.Controls.Add(lblStickPacksSachets)
                GroupBoxSachetBag.Controls.Add(txtStickPacksSachets)
                GroupBoxSachetBag.Controls.Add(Label73)

                lblStickPacksSachets.Location = New Point(24, 312)
                txtStickPacksSachets.Location = New Point(104, 309)
                txtStickPacksSachets.Size = New Size(69, 20)
                Label73.Location = New Point(178, 312)
                Label73.Text = "Per IFC/BAG"
            Else
                If Not chkDisplayBox.Checked Then
                    TotalUnitCost.Text = lblTotalCostBulk.Text
                End If
                pnlBagSales.Visible = False

                'Add Bag in Sachets
                'StickPacks
                grpBoxStickPackBag.Visible = False
                'Sachets
                GroupBoxSachetBag.Visible = False

            End If

            If Not chkDisplayBag.Checked Then
                If CurrentVersion.Count > 0 Then
                    CurrentVersion(0)("IsBag") = 0
                End If
            End If

            ToggleSalesGrid()

            'Add Bag in Sachets
            If ComboBox3.Text = "Stick Packs" Then
                CalculateStickPacksCost()
            ElseIf ComboBox3.Text = "Sachets" Then
                CalculateSachetsCost()
            End If

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub TextBox14_Click(sender As Object, e As EventArgs) Handles TextBox14.TextChanged
        UpdateTextbox13()
    End Sub

    Private Sub TextBox10_Click(sender As Object, e As EventArgs) Handles TextBox10.TextChanged
        UpdateTextbox14()
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        updateTextbox4()
    End Sub

    Private Sub txtCurrencyFormat(sender As Object, e As KeyPressEventArgs)
        Try
            If e.KeyChar = ChrW(Keys.Enter) Then
                Dim Ctl As Control
                If TypeOf sender Is TextBox Then
                    Ctl = CType(sender, TextBox)
                ElseIf TypeOf sender Is ComboBox Then
                    Ctl = CType(sender, ComboBox)
                End If

                Ctl.Text = If(String.IsNullOrEmpty(Ctl.Text), Ctl.Text, Convert.ToInt32(Ctl.Text.Replace(",", "")).ToString("N0", culture))

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub txtCurrencyFormatCMB(sender As Object, e As EventArgs)
        Try
            Dim Ctl As Control
            If TypeOf sender Is TextBox Then
                Ctl = CType(sender, TextBox)
            ElseIf TypeOf sender Is ComboBox Then
                Ctl = CType(sender, ComboBox)
            End If

            Dim Val As Double
            Double.TryParse(Ctl.Text, Val)
            Ctl.Text = If(String.IsNullOrEmpty(Val), Val, Convert.ToInt32(Val.ToString().Replace(",", "")).ToString("N0", culture))
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub txtCustName_TextChanged(sender As Object, e As EventArgs) Handles txtCustName.TextChanged
        Try
            If FormulaID.Text <> "-1" Then
                dsFF.Tables("formulaF")(0).Item("CustomerName") = txtCustName.Text
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub txtContact_TextChanged(sender As Object, e As EventArgs) Handles txtContact.TextChanged
        Try
            If FormulaID.Text <> "-1" Then
                dsFF.Tables("formulaF")(0).Item("Contact") = txtContact.Text
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub txtSalesRepId_TextChanged_1(sender As Object, e As EventArgs) Handles txtSalesRepId.TextChanged
        Try
            If FormulaID.Text <> "-1" Then
                dsFF.Tables("formulaF")(0).Item("SalesRepId") = txtSalesRepId.Text
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub
    'Accept all db changes-Sail 25-06-2025
    Private Sub AcceptAllDbChanges()
        Try
            Dim listofds As New List(Of DataSet)
            listofds.Add(dsF)
            listofds.Add(dsFF)
            listofds.Add(dsBi)
            listofds.Add(dsTi)
            listofds.Add(dsEi)
            listofds.Add(dsPB)
            listofds.Add(dsPSP)
            listofds.Add(dsPSa)

            listofds.Add(dsSal)
            listofds.Add(dsSalBags)
            listofds.Add(dsSalBox)
            listofds.Add(dsStandupBags)

            For Each ds As DataSet In listofds
                If ds.HasChanges = True Then
                    ds.AcceptChanges()
                End If
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView2_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellValueChanged
        Try
            If dsPB.Tables.Count > 0 Then
                If dsPB.Tables("bottles").Rows.Count > 0 Then
                    'lblBottleUnitCostSumAMT.Text = CalculateBottleSizeCount()
                    lblBottleUnitCostSumAMT.Text = CalculateBottleSizeTotalPackagingCost()
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub DataGridView2_Click(sender As Object, e As EventArgs) Handles DataGridView2.Click
        Try
            If dsPB.Tables.Count > 0 Then
                If dsPB.Tables("bottles").Rows.Count > 0 Then
                    'lblBottleUnitCostSumAMT.Text = CalculateBottleSizeCount()
                    lblBottleUnitCostSumAMT.Text = CalculateBottleSizeTotalPackagingCost()
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Function CalculateBottleSizeCount() As String
        Try
            Dim UnitCost As String
            UnitCost = (From tbl In dsPB.Tables("bottles").AsEnumerable().Where(Function(tbl) tbl.RowState <> DataRowState.Deleted).Where(Function(tbl) Not tbl.IsNull("UnitCost") And Not tbl.Item("Category") = "Desiccant" And Not tbl.Item("Category") = "Cotton" And Not tbl.Item("Category") = "Neckband" And Not tbl.Item("Category") = "Shippers" And Not tbl.Item("Category") = "Labor" And Not tbl.Item("Category") = "Other" And tbl.Item("VersionNumber") = If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text) And tbl.Item("SizeCount") = If(String.IsNullOrEmpty(ComboBox4.Text), "", ComboBox4.Text)).Select(Function(tbl) tbl.Field(Of Double)("UnitCost"))).Sum()

            Return UnitCost
        Catch ex As Exception
            Helper.WriteLog(ex)
            Return ""
        End Try
    End Function
    Function CalculateBottleSizeTotalPackagingCost() As String
        Try
            Dim UnitCost As String
            UnitCost = (From tbl In dsPB.Tables("bottles").AsEnumerable().Where(Function(tbl) tbl.RowState <> DataRowState.Deleted).Where(Function(tbl) Not tbl.IsNull("UnitCost") And tbl.Item("VersionNumber") = If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text) And tbl.Item("SizeCount") = If(String.IsNullOrEmpty(ComboBox4.Text), "", ComboBox4.Text)).Select(Function(tbl) tbl.Field(Of Double)("UnitCost"))).Sum()

            Return UnitCost
        Catch ex As Exception
            Helper.WriteLog(ex)
            Return ""
        End Try
    End Function

    Function CalculateBottleSizeAdditionalCategories() As Double
        Try
            Dim UnitCost As Double
            UnitCost = (
                From tbl In dsPB.Tables("bottles").AsEnumerable()
                Where tbl.RowState <> DataRowState.Deleted _
                      AndAlso Not tbl.IsNull("UnitCost") _
                      AndAlso (tbl.Item("Category") = "Desiccant" _
                           OrElse tbl.Item("Category") = "Cotton" _
                           OrElse tbl.Item("Category") = "Neckband" _
                           OrElse tbl.Item("Category") = "Shippers" _
                           OrElse tbl.Item("Category") = "Labor" _
                           OrElse tbl.Item("Category") = "Other") _
              AndAlso tbl.Item("VersionNumber") = If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text) _
              AndAlso tbl.Item("SizeCount") = If(String.IsNullOrEmpty(ComboBox4.Text), "", ComboBox4.Text)
                Select tbl.Field(Of Double)("UnitCost")).Sum()

            Return UnitCost
        Catch ex As Exception
            Helper.WriteLog(ex)
            Return 0.00
        End Try

    End Function

    Private Sub VersionDescriptionTxt_TextChanged(sender As Object, e As EventArgs) Handles VersionDescriptionTxt.TextChanged
        Try
            Dim version = From tbl In dsFV.Tables("versions").AsEnumerable() Where tbl.Field(Of Int32)("VersionNumber") = If(String.IsNullOrEmpty(VersionCmbBox.Text), "1", VersionCmbBox.Text)
            If version.Count > 0 Then
                version(0).Item("VersionDescription") = VersionDescriptionTxt.Text
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    Private Sub lblBottleUnitCostSumAMT_TexChanged(sender As Object, e As EventArgs) Handles lblBottleUnitCostSumAMT.TextChanged
        Try
            If Not String.IsNullOrEmpty(lblBottleUnitCostSumAMT.Text) Then
                Dim cost As Double = Double.Parse(lblBottleUnitCostSumAMT.Text)
                lblBottleUnitCostSumAMT.Text = cost.ToString("N2")
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub populateBagSalesGrid()
        Try
            SalesDVAuto = New DataView(dsSalBags.Tables("salesBagsTxns"), "PackagingFormat =" & ComboBox3.SelectedIndex & " AND [Type] = 'BULKBAGS' AND VersionNumber=" & VersionCmbBox.Text, "", DataViewRowState.CurrentRows)

            If SalesDVAuto.Count = 0 Then
                For j As Integer = 1 To 5
                    Dim workRow As DataRow = dsSalBags.Tables("salesBagsTxns").NewRow()
                    workRow("FormulaID") = FormulaID.Text
                    workRow("PackagingFormat") = ComboBox3.SelectedIndex
                    workRow("UnitSize") = ServingSizeTextBox.Text
                    workRow("Quantity") = 2500 * j
                    workRow("MarginPercentage") = 40 - (j * 5)
                    workRow("SalesPrice") = DBNull.Value
                    workRow("Type") = "BULKBAGS"
                    workRow("VersionNumber") = VersionCmbBox.Text
                    workRow("OverriddenSalesPrice") = DBNull.Value
                    workRow("Overridden") = 0
                    dsSalBags.Tables("salesBagsTxns").Rows.Add(workRow)
                Next
            End If

            BindSalesBulkStickpacksGrid.DataSource = dsSalBags.Tables("salesBagsTxns")
            BindSalesBulkStickpacksGrid.Filter = "PackagingFormat ='" & ComboBox3.SelectedIndex & "' AND Type='BULKBAGS' AND VersionNumber='" & VersionCmbBox.Text & "'"
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    Private Sub DataGridView1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DataGridView1.EditingControlShowing
        Try
            Dim txt As TextBox = CType(e.Control, TextBox)

            RemoveHandler txt.KeyPress, AddressOf txtBox_KeyPress
            RemoveHandler txt.LostFocus, AddressOf txtBox_LostFocus
            RemoveHandler txt.KeyPress, AddressOf txtBox_KeyPress_Number

            If DataGridView1.CurrentCell.ColumnIndex = 3 OrElse DataGridView1.CurrentCell.ColumnIndex = 6 OrElse DataGridView1.CurrentCell.ColumnIndex = 7 OrElse DataGridView1.CurrentCell.ColumnIndex = 8 OrElse DataGridView1.CurrentCell.ColumnIndex = 4 Then
                AddHandler txt.KeyPress, AddressOf txtBox_KeyPress
                AddHandler txt.LostFocus, AddressOf txtBox_LostFocus
            ElseIf DataGridView1.CurrentCell.ColumnIndex = 5 Then
                AddHandler txt.KeyPress, AddressOf txtBox_KeyPress_Number
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Function SetSafeValue(DView As DataView, ColumnName As String) As String
        Try
            Dim value = DView(0).Item(ColumnName)

            If String.IsNullOrEmpty(ColumnName) OrElse IsDBNull(ColumnName) Then
                Return ""
            End If

            Return value.ToString()
        Catch ex As Exception
            Helper.WriteLog(ex)
            Return ""
        End Try

    End Function

    Private Sub FormulaTable_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved
        'UpdateIndex()
    End Sub

    Private Sub UpdateIndex()
        Try
            Dim DataView = New DataView(dsF.Tables("formula"), "VersionID = " & VersionCmbBox.Text, "IngredientIndex", DataViewRowState.CurrentRows)
            i = 1
            For Each item As DataRowView In DataView
                If item.Row.RowState = DataRowState.Deleted Then
                    Continue For
                Else
                    item("IngredientIndex") = i
                    i = i + 1
                End If

            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView1_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDown
        Try
            If DataGridView1.Rows.Count > 0 Then
                RowIndex = DataGridView1.CurrentRow.Cells("Column1").Value
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub ReplaceMaterial(Material As String, Supplier As String)
        Try
            Dim filter = "VersionID = " & VersionCmbBox.Text & " AND IngredientIndex = " & RowIndex
            Dim DataView = New DataView(dsF.Tables("formula"), filter, "IngredientIndex", DataViewRowState.CurrentRows)
            If Not String.IsNullOrEmpty(Supplier) Then
                Dim Mt = ds.Tables("ingredients").AsEnumerable().Where(Function(mtl) mtl.Field(Of String)("MaterialName") = Material And mtl.Field(Of String)("Supplier") = Supplier).FirstOrDefault
                DataView.Item(0)("MaterialName") = Mt.Item("MaterialName")
                DataView.Item(0)("ComponentCode") = Mt.Item("ComponentCode")
                DataView.Item(0)("VendorName") = Mt.Item("Supplier")
                DataView.Item(0)("MaterialCost") = Mt.Item("Price")
                DataView.Item(0)("Manualcost") = Mt.Item("Price")
                DataView.Item(0)("IsManual") = Mt.Item("IsManual")
                DataView.Item(0)("LatestCostDate") = Mt.Item("LatestDate")
                If Mt.Item("IsManual") = 1 Then
                    DataView.Item(0)("ManualCostDate") = DateTime.Now
                End If
                DataView.Item(0)("EnteredDate") = DateTime.Now
            Else
                DataView.Item(0)("MaterialName") = Material
                DataView.Item(0)("ComponentCode") = DBNull.Value
                DataView.Item(0)("VendorName") = DBNull.Value
                DataView.Item(0)("LatestCost") = 0
                DataView.Item(0)("MaterialCost") = 0
                DataView.Item(0)("Manualcost") = 0
                DataView.Item(0)("IsManual") = 1
                DataView.Item(0)("ManualCostDate") = DateTime.Now
                DataView.Item(0)("EnteredDate") = DateTime.Now
            End If

            DisplayFormula()
            For j As Integer = 0 To DataGridView1.Rows.Count - 1
                DisplaySelectedCostValue(j)
                DisplayCostType(j)
                DisplayCostColor(j)
                'FormulaCalculations(j, ISMgChanged)
                CalculateServingWeight()
                FormulaCost(j)
            Next
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateSalesPRiceWithFlavorCost(sender As Object, e As EventArgs) Handles txtBlendingFlavourProfileServing.TextChanged, ComboBox4.SelectedIndexChanged, txtStickPackBagServings.TextChanged
        Try
            If FormulaTypeCmbBox.Text = "Powder" Then
                If ComboBox3.Text = "Bulk" Then

                ElseIf ComboBox3.Text = "Bottles" Then
                    CalculateBottlingCost()
                ElseIf ComboBox3.Text = "Stick Packs" Then
                    CalculateStickPacksCost()
                ElseIf ComboBox3.Text = "Sachets" Then
                    CalculateSachetsCost()
                ElseIf ComboBox3.Text = "Bags" Then
                    CalculateStandUpBagCost()
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try


    End Sub

    Private Sub HideUnhideFlavorProfile(IsBulk As Boolean)
        Try
            If Not IsBulk Then
                lblBlendingFlavourProfileKg.Visible = False
                Label63.Visible = False
                txtBlendingFlavourProfileKg.Visible = False
                Label53.Visible = False

                lblBlendingFlavorProfileServing.Visible = True
                Label115.Visible = True
                txtBlendingFlavourProfileServing.Visible = True
                txtBlendingFlavourProfileUnit.Visible = True
            Else
                lblBlendingFlavourProfileKg.Visible = True
                Label63.Visible = True
                txtBlendingFlavourProfileKg.Visible = True
                Label53.Visible = True

                lblBlendingFlavorProfileServing.Visible = False
                Label115.Visible = False
                txtBlendingFlavourProfileServing.Visible = False
                txtBlendingFlavourProfileUnit.Visible = False
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView1_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles DataGridView1.UserDeletedRow
        UpdateIndex()
    End Sub

    Private Function CheckInvalidChar(senderTxtBox As Control, DataView As DataView) As Boolean
        Try
            Dim columnName As String = senderTxtBox.Tag.ToString()
            ' Check if the column exists and its type
            Dim colType As Type = DataView.Table.Columns(columnName).DataType

            If (colType Is GetType(Double) OrElse colType Is GetType(Decimal)) AndAlso (senderTxtBox.Text.Trim() = "." OrElse senderTxtBox.Text.Trim() = "") Then
                ' Skip assignment if it's a Double type column and Text="."
                Return True
            End If
            Return False
        Catch ex As Exception
            Helper.WriteLog(ex)
            Return False
        End Try
    End Function

    Private Sub Formulator2_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control And e.KeyCode = Keys.S Then
            Saves()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub lblAdditionalChargesBulk_TextChanged(sender As Object, e As EventArgs) Handles lblAdditionalChargesBulk.TextChanged
        CalculateBulkSales(gridBulkSales)
    End Sub
    Private Sub lblAdditionalChargesBox_TextChanged(sender As Object, e As EventArgs) Handles lblAdditionalChargesBox.TextChanged
        CalculateBulkSales(gridBoxSales)
    End Sub
    Private Sub lblAdditionalChargeBag_TextChanged(sender As Object, e As EventArgs) Handles lblAdditionalChargeBag.TextChanged
        CalculateBulkSales(gridbBulkSalesBags)
    End Sub

    Public Sub dsSal_Insert()
        Dim SelectCommand As New MySqlCommand("SELECT * FROM SalesDetails WHERE Type = 'BULK' AND FormulaID=" & FormulaID.Text, DbCon)
        daSal.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO SalesDetails
        (FormulaID
        , PackagingFormat
        , UnitSize
        , Quantity
        , MarginPercentage
        , SalesPrice
        , VersionNumber
        , Type
        , OverriddenSalesPrice
        , Overridden) 
        VALUES
        (" & FormulaID.Text & "
        , @PackagingFormat        
        , @UnitSize 
        , @Quantity
        , @MarginPercentage
        , @SalesPrice
        , @VersionNumber
        , @Type
        , @OverriddenSalesPrice
        , @Overridden) ", DbCon)

        InsertCommand.Parameters.Add("@PackagingFormat", MySqlDbType.Int32, 10, "PackagingFormat")
        InsertCommand.Parameters.Add("@UnitSize", MySqlDbType.Decimal, 5, "UnitSize")
        InsertCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 11, "Quantity")
        InsertCommand.Parameters.Add("@MarginPercentage", MySqlDbType.Decimal, 4, "MarginPercentage")
        InsertCommand.Parameters.Add("@SalesPrice", MySqlDbType.Decimal, 11, "SalesPrice")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        InsertCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 20, "Type")
        InsertCommand.Parameters.Add("@OverriddenSalesPrice", MySqlDbType.Decimal, 8, "OverriddenSalesPrice")
        InsertCommand.Parameters.Add("@Overridden", MySqlDbType.Int32, 1, "Overridden")
        daSal.InsertCommand = InsertCommand

        Try
            daSal.Update(dsSal, "salestxns")
            dsSal.AcceptChanges()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Public Sub daSalBag_Insert()

        Dim SelectCommand As New MySqlCommand("SELECT * FROM SalesDetails WHERE  Type = 'BULKBAGS' AND  FormulaID=" & FormulaID.Text, DbCon)
        daSalBags.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO SalesDetails
        (FormulaID
        , PackagingFormat
        , UnitSize
        , Quantity
        , MarginPercentage
        , SalesPrice
        , VersionNumber
        , Type
        , OverriddenSalesPrice
        , Overridden) 
        VALUES
        (" & FormulaID.Text & "
        , @PackagingFormat        
        , @UnitSize 
        , @Quantity
        , @MarginPercentage
        , @SalesPrice
        , @VersionNumber
        , @Type
        , @OverriddenSalesPrice
        , @Overridden) ", DbCon)

        InsertCommand.Parameters.Add("@PackagingFormat", MySqlDbType.Int32, 10, "PackagingFormat")
        InsertCommand.Parameters.Add("@UnitSize", MySqlDbType.Decimal, 5, "UnitSize")
        InsertCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 11, "Quantity")
        InsertCommand.Parameters.Add("@MarginPercentage", MySqlDbType.Decimal, 4, "MarginPercentage")
        InsertCommand.Parameters.Add("@SalesPrice", MySqlDbType.Decimal, 11, "SalesPrice")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        InsertCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 20, "Type")
        InsertCommand.Parameters.Add("@OverriddenSalesPrice", MySqlDbType.Decimal, 8, "OverriddenSalesPrice")
        InsertCommand.Parameters.Add("@Overridden", MySqlDbType.Int32, 1, "Overridden")
        daSalBags.InsertCommand = InsertCommand

        Try
            daSalBags.Update(dsSalBags, "salesBagsTxns")
            dsSalBags.AcceptChanges()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Public Sub dsSalBox_Insert()
        Dim SelectCommand As New MySqlCommand("SELECT * FROM SalesDetails WHERE  Type = 'BOX' AND FormulaID=" & FormulaID.Text, DbCon)
        daSalBox.SelectCommand = SelectCommand

        Dim InsertCommand As New MySqlCommand("INSERT INTO SalesDetails
        (FormulaID
        , PackagingFormat
        , UnitSize
        , Quantity
        , MarginPercentage
        , SalesPrice
        ,VersionNumber
        ,Type
        , OverriddenSalesPrice
        , Overridden) 
        VALUES
        (" & FormulaID.Text & "
        , @PackagingFormat        
        , @UnitSize 
        , @Quantity
        , @MarginPercentage
        , @SalesPrice
        , @VersionNumber
        , @Type
        , @OverriddenSalesPrice
        , @Overridden)", DbCon)

        InsertCommand.Parameters.Add("@PackagingFormat", MySqlDbType.Int32, 10, "PackagingFormat")
        InsertCommand.Parameters.Add("@UnitSize", MySqlDbType.Decimal, 5, "UnitSize")
        InsertCommand.Parameters.Add("@Quantity", MySqlDbType.Int32, 11, "Quantity")
        InsertCommand.Parameters.Add("@MarginPercentage", MySqlDbType.Decimal, 4, "MarginPercentage")
        InsertCommand.Parameters.Add("@SalesPrice", MySqlDbType.Decimal, 11, "SalesPrice")
        InsertCommand.Parameters.Add("@VersionNumber", MySqlDbType.Int32, 10, "VersionNumber")
        InsertCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 20, "Type")
        InsertCommand.Parameters.Add("@OverriddenSalesPrice", MySqlDbType.Decimal, 8, "OverriddenSalesPrice")
        InsertCommand.Parameters.Add("@Overridden", MySqlDbType.Int32, 1, "Overridden")
        daSalBox.InsertCommand = InsertCommand

        Try
            daSalBox.Update(dsSalBox, "salesBoxTxns")
            dsSalBox.AcceptChanges()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub


    Private Sub CheckSalesData(FormulaID As Int16, PackagingFormat As Int16, VersionNumber As Int16, DisplayBox As Boolean, DisplayBag As Boolean, FormulaType As String, PackagingType As String, ServingSize As Decimal, UnitSizeBottle As Int16)
        Try
            If PackagingType = "Bulk" Then
                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat = " & PackagingFormat & " And VersionNumber = " & VersionNumber, "", DataViewRowState.CurrentRows)
                BulkSalesDataCheck(FormulaType, FormulaID, VersionNumber, PackagingFormat)

            ElseIf PackagingType = "Bottles" Then

                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & PackagingFormat & "AND VersionNumber=" & VersionNumber, "", DataViewRowState.CurrentRows)

                If SalesDVAuto.Count = 0 Then
                    For j As Integer = 1 To 5
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                        workRow("FormulaID") = FormulaID
                        workRow("PackagingFormat") = PackagingFormat
                        workRow("UnitSize") = UnitSizeBottle
                        workRow("Quantity") = 2500 * j
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("Type") = "BULK"
                        workRow("VersionNumber") = VersionNumber
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)
                    Next
                End If


            ElseIf PackagingFormat = 4 Then
                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & PackagingFormat & " AND [Type] = 'BULK' AND VersionNumber=" & VersionNumber, "", DataViewRowState.CurrentRows)

                If SalesDVAuto.Count = 0 Then
                    For j As Integer = 1 To 5
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                        workRow("FormulaID") = FormulaID
                        workRow("PackagingFormat") = PackagingFormat
                        workRow("UnitSize") = ServingSize
                        workRow("Quantity") = 2500 * j
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("Type") = "BULK"
                        workRow("VersionNumber") = VersionNumber
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)
                    Next
                End If

                If DisplayBox Then
                    SalesDVAuto = New DataView(dsSalBox.Tables("salesBoxTxns"), "PackagingFormat =" & PackagingFormat & " AND [Type] = 'BOX' AND VersionNumber=" & If(String.IsNullOrEmpty(VersionNumber), "1", VersionNumber), "", DataViewRowState.CurrentRows)

                    If SalesDVAuto.Count = 0 Then
                        For j As Integer = 1 To 5
                            Dim workRow As DataRow = dsSalBox.Tables("salesBoxTxns").NewRow()
                            workRow("FormulaID") = FormulaID
                            workRow("PackagingFormat") = PackagingFormat
                            workRow("UnitSize") = ServingSize
                            workRow("Quantity") = 2500 * j
                            workRow("MarginPercentage") = 40 - (j * 5)
                            workRow("SalesPrice") = DBNull.Value
                            workRow("Type") = "BOX"
                            workRow("VersionNumber") = VersionNumber
                            workRow("OverriddenSalesPrice") = DBNull.Value
                            workRow("Overridden") = 0
                            dsSalBox.Tables("salesBoxTxns").Rows.Add(workRow)
                        Next
                    End If
                End If

            ElseIf PackagingFormat = 2 Or PackagingFormat = 3 Or PackagingFormat = 5 Then

                SalesDVAuto = New DataView(dsSal.Tables("salestxns"), "PackagingFormat =" & PackagingFormat & " AND [Type] = 'BULK' AND VersionNumber=" & If(String.IsNullOrEmpty(VersionNumber), "1", VersionNumber), "", DataViewRowState.CurrentRows)

                If SalesDVAuto.Count = 0 Then
                    For j As Integer = 1 To 5
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()

                        workRow("FormulaID") = FormulaID
                        workRow("PackagingFormat") = PackagingFormat
                        workRow("UnitSize") = ServingSize
                        workRow("Quantity") = 25000 * j
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("Type") = "BULK"
                        workRow("VersionNumber") = VersionNumber
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)
                    Next
                End If

                If DisplayBox Then
                    SalesDVAuto = New DataView(dsSalBox.Tables("salesBoxTxns"), "PackagingFormat =" & PackagingFormat & " AND [Type] = 'BOX' AND VersionNumber=" & If(String.IsNullOrEmpty(VersionNumber), "1", VersionNumber), "", DataViewRowState.CurrentRows)

                    If SalesDVAuto.Count = 0 Then
                        For j As Integer = 1 To 5
                            Dim workRow As DataRow = dsSalBox.Tables("salesBoxTxns").NewRow()
                            workRow("FormulaID") = FormulaID
                            workRow("PackagingFormat") = PackagingFormat
                            workRow("UnitSize") = ServingSize
                            workRow("Quantity") = 2500 * j
                            workRow("MarginPercentage") = 40 - (j * 5)
                            workRow("SalesPrice") = DBNull.Value
                            workRow("Type") = "BOX"
                            workRow("VersionNumber") = VersionNumber
                            workRow("OverriddenSalesPrice") = DBNull.Value
                            workRow("Overridden") = 0
                            dsSalBox.Tables("salesBoxTxns").Rows.Add(workRow)
                        Next
                    End If
                End If

                If PackagingType = "Stick Packs" And DisplayBag Then
                    BagSalesDataCheck(FormulaID, PackagingFormat, VersionNumber, ServingSize)
                End If
            End If

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub BulkSalesDataCheck(FormulaType As String, FormulaID As Int16, VersionNumber As Int16, PackagingFormat As Int16)
        Try
            If FormulaType = "Powder" Then
                If SalesDVAuto.Count = 0 Then
                    Dim i As Int16 = 1
                    For j As Integer = 1 To 6
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                        workRow("FormulaID") = FormulaID
                        workRow("PackagingFormat") = 0
                        workRow("UnitSize") = 0
                        If j = 1 Then
                            workRow("Quantity") = 300
                        Else
                            workRow("Quantity") = 500 * i
                            i = i + 1
                        End If
                        workRow("Type") = "BULK"
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("VersionNumber") = VersionNumber
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)

                    Next
                    i = 1
                End If
            Else
                If SalesDVAuto.Count = 0 Then
                    For j As Integer = 1 To 5
                        Dim workRow As DataRow = dsSal.Tables("salestxns").NewRow()
                        workRow("FormulaID") = FormulaID
                        workRow("PackagingFormat") = 0
                        workRow("UnitSize") = 0
                        If j = 5 Then
                            workRow("Quantity") = 250000 * 6
                        Else
                            workRow("Quantity") = 250000 * j
                        End If
                        workRow("Type") = "BULK"
                        workRow("MarginPercentage") = 40 - (j * 5)
                        workRow("SalesPrice") = DBNull.Value
                        workRow("VersionNumber") = VersionNumber
                        workRow("OverriddenSalesPrice") = DBNull.Value
                        workRow("Overridden") = 0
                        dsSal.Tables("salestxns").Rows.Add(workRow)
                    Next
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub BagSalesDataCheck(FormulaID As Int16, PackagingFormat As Int16, VersionNumber As Int16, ServingSize As Decimal)
        Try
            SalesDVAuto = New DataView(dsSalBags.Tables("salesBagsTxns"), "PackagingFormat =" & PackagingFormat & " AND [Type] = 'BULKBAGS' AND VersionNumber=" & VersionNumber, "", DataViewRowState.CurrentRows)

            If SalesDVAuto.Count = 0 Then
                For j As Integer = 1 To 5
                    Dim workRow As DataRow = dsSalBags.Tables("salesBagsTxns").NewRow()
                    workRow("FormulaID") = FormulaID
                    workRow("PackagingFormat") = PackagingFormat
                    workRow("UnitSize") = ServingSize
                    workRow("Quantity") = 2500 * j
                    workRow("MarginPercentage") = 40 - (j * 5)
                    workRow("SalesPrice") = DBNull.Value
                    workRow("Type") = "BULKBAGS"
                    workRow("VersionNumber") = VersionNumber
                    workRow("OverriddenSalesPrice") = DBNull.Value
                    workRow("Overridden") = 0
                    dsSalBags.Tables("salesBagsTxns").Rows.Add(workRow)
                Next
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub txtSachetsBagFillWeightCalculate(sender As Object, e As EventArgs) Handles txtSachetsBagServings.TextChanged, txtSachetsBagServingWeight.TextChanged
        txtSachetsBagFillWeight.Text = If(String.IsNullOrEmpty(txtSachetsBagServingWeight.Text), 0, txtSachetsBagServingWeight.Text) * If(String.IsNullOrEmpty(txtSachetsBagServings.Text), 0, txtSachetsBagServings.Text)
    End Sub

End Class