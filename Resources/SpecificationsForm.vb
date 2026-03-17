Imports MySql.Data.MySqlClient
Imports PdfSharp.Drawing
Imports PdfSharp.Drawing.Layout
Imports PdfSharp.Pdf
Imports System.IO
Imports System.Globalization
Imports BPG_Costing.Formulator2

Public Class SpecificationsForm

    Dim frm As Formulator2 = Application.OpenForms().OfType(Of Formulator2)().FirstOrDefault()

    Dim PageNumber As Integer = 0
    Dim YTermCondition As Integer = 0
    Public PackagingFormat As Integer = 0

    Public FormulaDT = New DataTable()

    Dim pen As XPen = XPens.Black
    Dim penTransparent As XPen = XPens.Transparent

    Dim brushLightGray As XBrush = New XSolidBrush(XColor.FromArgb(217, 217, 217))
    Dim brushGreen As XBrush = New XSolidBrush(XColor.FromArgb(142, 217, 115))
    Dim brushRed As XBrush = New XSolidBrush(XColor.FromArgb(225, 141, 116))
    Dim brushBlack As XBrush = XBrushes.Black
    Dim brushTransparent As XBrush = XBrushes.Transparent

    Dim font9Bold = New XFont("Arial Narrow", 9, XFontStyle.Bold)
    Dim font9Regular = New XFont("Arial Narrow", 9, XFontStyle.Regular)

    Dim font12Bold = New XFont("Arial Narrow", 12, XFontStyle.Bold)
    Dim font12Regular = New XFont("Arial Narrow", 12, XFontStyle.Regular)

    Dim font10Bold = New XFont("Arial Narrow", 10, XFontStyle.Bold)
    Dim font10Regular = New XFont("Arial Narrow", 10, XFontStyle.Regular)

    Dim font6Regular = New XFont("Arial Narrow", 6, XFontStyle.Regular)
    Dim font6Bold = New XFont("Arial Narrow", 6, XFontStyle.Bold)

    Dim font8Bold = New XFont("Arial Narrow", 8, XFontStyle.Bold)
    Dim font8Regular = New XFont("Arial Narrow", 8, XFontStyle.Regular)

    Dim RKey As Integer = 400
    Dim RValue As Integer = 485

    Dim LKey As Integer = 45
    Dim LValue As Integer = 150

    Dim format As New XStringFormat()

    Dim culture As CultureInfo = New CultureInfo("en-IN")

    Dim WrapWord = New List(Of String)
    Dim i As Int16 = 0

    Dim DbCon As New MySqlConnection
    Dim dbUp As New MySqlCommand
    Dim da As MySqlDataAdapter
    Dim ds1 As New DataSet

    Dim PrintingPage As Integer = 1
    Dim printPack As Boolean = False

    Dim dc As New DataGridViewCellStyle

    Private fromIndex As Integer
    Private dragIndex As Integer
    Private dragRect As Rectangle

    Dim pdf As PdfDocument
    Dim pdfPage As PdfPage
    Dim graph As XGraphics

    Dim PrintedPerPage As Integer = 0
    Dim Rowsperpage As Integer = 27
    Dim RowsPrinted As Integer = 0

    Dim custDV As DataView
    Dim SalesDVAuto As DataView

    Dim email As String = ""
    Dim Phone As String = ""

    Dim SalesRepData As New DataSet()
    Dim YIndex As Integer
    Dim Xlength As Integer

    Dim UpdatedIndex As New Dictionary(Of Integer, Decimal)
    Dim UpdatedBoxIndex As New Dictionary(Of Integer, Decimal)
    Dim UpdatedBagIndex As New Dictionary(Of Integer, Decimal)

    Dim Form2 As New Formulator2()

    Dim Formula_CustomerInformation As New Dictionary(Of String, String)
    Dim QuoteNumber_Date As New Dictionary(Of String, String)
    Dim ServingSize As New Dictionary(Of String, String)
    Dim Contactinformation As New Dictionary(Of String, String)
    Dim Capsule_Type As New Dictionary(Of String, String)
    Dim CompanyInfo As New List(Of String)

    Private Sub SpecificationsForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
        Dim frmFormulator2 As Formulator2 = Application.OpenForms().OfType(Of Formulator2)().FirstOrDefault()
        frmFormulator2.chkCalculate.Checked = Not chkCalculate.Checked
    End Sub
    Private Sub SpecificationsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            format.Alignment = XStringAlignment.Center
            format.LineAlignment = XLineAlignment.Center

            pnlBagSales.Location = New Point(StickPackPanel.Width + 30, 14)

            If chkDisplayBag.Checked Then
                GroupBox1.Visible = True
                GroupBoxSachetBag.Visible = True
            Else
                GroupBox1.Visible = False
                GroupBoxSachetBag.Visible = False
            End If
            If Label11.Text = "Bottles" Or Label11.Text = "Bulk" Then
                chkDisplayBox.Checked = False
                chkDisplayBag.Checked = False
                lblIsBox.Text = "0"
            ElseIf Label11.Text = "Bags" Then
                chkDisplayBox.Checked = False
                lblIsBox.Text = "0"
            End If

            DataGridView4.Rows.Add("Shipping Method", "FOB Brand")
            DataGridView4.Rows.Add("Payment Terms", "50% Downs, 50% Due prior shipping")
            DataGridView4.Rows.Add("Freight Terms", "FOB Collect")

            'lblSalesRep =
            If Label11.Text = "Sachets" Or Label11.Text = "Stick Packs" Or Label11.Text = "Blister" Then
                chkDisplayBox.Visible = True
                If Label11.Text = "Stick Packs" Or Label11.Text = "Sachets" Then
                    chkDisplayBag.Visible = True
                End If
            Else
                chkDisplayBox.Visible = False
                chkDisplayBag.Visible = False
            End If

            If chkDisplayBox.Checked Then
                GroupBoxStickPacksDisplaybox.Visible = True
                GroupBoxSachetDisplaybox.Visible = True
            Else
                GroupBoxStickPacksDisplaybox.Visible = False
                GroupBoxSachetDisplaybox.Visible = False
            End If

            If Label11.Text = "Bottles" Then

                custDV = New DataView(dsSpec.Tables(0), "SizeCount = " & If(String.IsNullOrEmpty(ComboBox4.Text), "0", ComboBox4.Text) & " AND VersionNumber=" & lblVersionNumber.Text, "", DataViewRowState.CurrentRows)
                DataGridView2.AutoGenerateColumns = False
                DataGridView2.DataSource = custDV

                custDV = New DataView(dsSpec.Tables(1), "PackagingFormat = 1 AND UnitSize = " & If(String.IsNullOrEmpty(ComboBox4.Text), "0", ComboBox4.Text) & " AND VersionNumber=" & lblVersionNumber.Text, "", DataViewRowState.CurrentRows)
                gridBulkSales.AutoGenerateColumns = False
                gridBulkSales.DataSource = custDV

            End If

            If lblIsBox.Text = "1" And Not chkDisplayBag.Checked Then

                GroupBoxStickPacksDisplaybox.Visible = True
                GroupBoxSachetDisplaybox.Visible = True
                GroupBox1BlisterBox.Visible = True
                pnlBoxSales.Visible = True


                GroupBoxStickPacksDisplaybox.Controls.Add(Label76)
                GroupBoxStickPacksDisplaybox.Controls.Add(TextBox38)
                GroupBoxStickPacksDisplaybox.Controls.Add(Label77)

                Label76.Location = New Point(32, 209)
                TextBox38.Location = New Point(165, 206)
                Label77.Location = New Point(226, 210)
                'grpboxadditionalbox.Visible = True
            End If

            If lblIsBox.Text = "0" And chkDisplayBag.Checked And Label11.Text = "Stick Packs" Then
                pnlBagSales.Visible = True
                pnlBagSales.Location = New Point(783, 220)
                GroupBox1.Controls.Add(Label76)
                GroupBox1.Controls.Add(TextBox38)
                GroupBox1.Controls.Add(Label77)

                Label76.Location = New Point(23, 286)
                TextBox38.Location = New Point(250, 279)
                Label77.Location = New Point(311, 282)
            ElseIf lblIsBox.Text = "0" And chkDisplayBag.Checked And Label11.Text = "Sachets" Then
                pnlBagSales.Visible = True
                pnlBagSales.Location = New Point(783, 220)

                GroupBoxSachetBag.Controls.Add(lblStickPacksSachets)
                GroupBoxSachetBag.Controls.Add(txtStickPacksSachets)
                GroupBoxSachetBag.Controls.Add(Label73)

                lblStickPacksSachets.Location = New Point(21, 279)
                txtStickPacksSachets.Location = New Point(250, 278)
                txtStickPacksSachets.Size = New Point(55, 20)
                Label73.Location = New Point(311, 279)

            End If

            If FormulaType.Text = "Powder" Then
                Label135.Text = "gm"
                Label98.Text = "gm"
                ServingSizeUOMLabel.Text = "gm"

                Dim result As Double
                Dim ServingSize = Double.TryParse(ServingSizeTextBox.Text, result)
                ServingSizeTextBox.Text = result.ToString("N2", culture)

                Dim Stickpack As Double
                Dim SickpackContent = Double.TryParse(TextBox48.Text, Stickpack)
                TextBox48.Text = Stickpack.ToString("N2", culture)

                Dim Sachets As Double
                Dim SachetsContent = Double.TryParse(TextBox35.Text, Sachets)
                TextBox35.Text = Sachets.ToString("N2", culture)

                'rtxtOtherIngredients.Text = "Natural and Artificial Flavors, Citric Acid, Malic Acid, Sucralose, Silicon Dioxide, Maltodextrin, Acesulfame Potassium"
                rtxtOtherIngredients.Text = If(IsDBNull(FormulaDT.Rows(0)("OtherIngredients")), "", FormulaDT.Rows(0)("OtherIngredients"))
            ElseIf FormulaType.Text = "Capsule" Then
                Label135.Text = "Capsule(s)"
                Label98.Text = "Capsule(s)"
                ServingSizeUOMLabel.Text = ""
                'rtxtOtherIngredients.Text = "Vegetable Cellulose (Capsule), Microcrystalline Cellulose, Magnesium Stearate, Silicon Dioxide"
                rtxtOtherIngredients.Text = If(IsDBNull(FormulaDT.Rows(0)("OtherIngredients")), "", FormulaDT.Rows(0)("OtherIngredients"))
            Else
                Label135.Text = "Tablet(s)"
                Label98.Text = "Tablet(s)"
                ServingSizeUOMLabel.Text = ""
                'rtxtOtherIngredients.Text = "Microcrystalline Cellulose, Stearic Acid, Croscarmellose Sodium, Magnesium Stearate, Silicon Dioxide, Dicalcium Phosphate"
                rtxtOtherIngredients.Text = If(IsDBNull(FormulaDT.Rows(0)("OtherIngredients")), "", FormulaDT.Rows(0)("OtherIngredients"))
            End If

            If lblIsBox.Text = "0" And Not chkDisplayBag.Checked Then
                Panel6.Location = New Point(783, 220)
            End If

            If (Label11.Text = "Stick Packs" Or Label11.Text = "Sachets") And chkDisplayBag.Checked And Not chkDisplayBox.Checked Then
                pnlBagSales.Visible = True
                pnlBagSales.Location = New Point(StickPackPanel.Width + 30, 219)
                Panel6.Location = New Point(StickPackPanel.Width + 30, 422)
            End If

            For Each col In gridBulkSales.Columns
                col.sortmode = DataGridViewColumnSortMode.NotSortable
            Next
            For Each col In DataGridView4.Columns
                col.sortmode = DataGridViewColumnSortMode.NotSortable
            Next
            DisplaySalesRepData()

            'Fill Formula And Customer Data
            Formula_CustomerInformation.Add("Formula Type", If(String.IsNullOrEmpty(FormulaType.Text), "", FormulaType.Text))
            Formula_CustomerInformation.Add("Formula Name", If(String.IsNullOrEmpty(lblFormulaName.Text), "", lblFormulaName.Text))
            Formula_CustomerInformation.Add("Version", If(String.IsNullOrEmpty(lblVersionDescription.Text), "", lblVersionDescription.Text))
            Formula_CustomerInformation.Add("Customer Name", If(String.IsNullOrEmpty(lblCustomerName.Text), "", lblCustomerName.Text))
            Formula_CustomerInformation.Add("Contact", If(String.IsNullOrEmpty(lblContact.Text), "", lblContact.Text))

            Dim ServingPerContainer
            If Label11.Text = "Bulk" Then
                'Fill Serving Data
                ServingSize.Add("Serving Size", If(String.IsNullOrEmpty(ServingSizeTextBox.Text), "", ServingSizeTextBox.Text) & " " & ServingSizeUOMLabel.Text)
                ServingSize.Add("Servings per Container", "")
                ServingSize.Add("Fill Weight", "")

            ElseIf Label11.Text = "Bottles" Then
                'Fill Serving Data
                ServingSize.Add("Serving Size", If(String.IsNullOrEmpty(ServingSizeTextBox.Text), "", ServingSizeTextBox.Text) & " " & ServingSizeUOMLabel.Text)
                If FormulaType.Text = "Powder" Then
                    ServingPerContainer = ComboBox4.Text
                Else
                    ServingPerContainer = lblServingsContainer.Text
                End If

            ElseIf Label11.Text = "Blister" Then
                CalculateBlisterSize()

            ElseIf Label11.Text = "Bags" Then
                'Fill Serving Data
                ServingSize.Add("Serving Size", If(String.IsNullOrEmpty(ServingSizeTextBox.Text), "", ServingSizeTextBox.Text) & " " & ServingSizeUOMLabel.Text)
                ServingPerContainer = txtServing.Text
            ElseIf Label11.Text = "Stick Packs" Then
                CalculateStickPackSize()
            ElseIf Label11.Text = "Sachets" Then
                CalculateSachetsSize()
            End If

            ServingPerContainer = If(String.IsNullOrEmpty(ServingPerContainer), 0, ServingPerContainer)

            If FormulaType.Text = "Powder" Then
                If Label11.Text = "Bags" Then
                    ServingSize.Add("Servings per Container", ServingPerContainer)
                    Dim Serving As Double
                    Dim ServingPeContainer As Double
                    Double.TryParse(ServingSizeTextBox.Text, Serving)
                    Double.TryParse(ServingPerContainer, ServingPeContainer)
                    Dim FillWeight = Serving * ServingPeContainer
                    ServingSize.Add("Fill Weight", FillWeight.ToString("N2") & " gm")
                End If

            ElseIf FormulaType.Text = "Capsule" Then
                If Label11.Text = "Bags" Then
                    ServingSize.Add("Servings per Container", ServingPerContainer)
                    ServingSize.Add("Fill Weight", Convert.ToInt16(If(String.IsNullOrEmpty(ServingSizeTextBox.Text), 0, ServingSizeTextBox.Text)) * Convert.ToInt16(If(String.IsNullOrEmpty(ServingPerContainer), 0, ServingPerContainer)) & " Capsule(s)")
                ElseIf Label11.Text = "Bottles" Then
                    ServingSize.Add("Servings per Container", ServingPerContainer)
                    ServingSize.Add("Fill Weight", ComboBox4.Text & " Capsule(s)")
                End If

            Else
                If Label11.Text = "Bags" Then
                    ServingSize.Add("Servings per Container", ServingPerContainer)
                    ServingSize.Add("Fill Weight", Convert.ToInt16(If(String.IsNullOrEmpty(ServingSizeTextBox.Text), 0, ServingSizeTextBox.Text)) * Convert.ToInt16(If(String.IsNullOrEmpty(ServingPerContainer), 0, ServingPerContainer)) & " Tablet(s)")
                ElseIf Label11.Text = "Bottles" Then
                    ServingSize.Add("Servings per Container", ServingPerContainer)
                    ServingSize.Add("Fill Weight", ComboBox4.Text & " Tablet(s)")
                End If
            End If

            'Fill Quote Number and Date
            QuoteNumber_Date.Add("Date Quoted", Date.Now.Date.ToString("MM-dd-yyyy"))
            QuoteNumber_Date.Add("Quote Number", If(String.IsNullOrEmpty(FormulaID.Text), "", FormulaID.Text))


            GetSalesRepData()
            'Fill Contact Information 
            Dim FirstName As String = (From tbl In SalesRepData.Tables("SalesRepData").AsEnumerable() Select tbl.Field(Of String)("FirstName")).FirstOrDefault()
            Dim LastName As String = (From tbl In SalesRepData.Tables("SalesRepData").AsEnumerable() Select tbl.Field(Of String)("LastName")).FirstOrDefault()
            Dim Phone As String = (From tbl In SalesRepData.Tables("SalesRepData").AsEnumerable() Select tbl.Field(Of String)("Phone")).FirstOrDefault()
            Dim Email As String = (From tbl In SalesRepData.Tables("SalesRepData").AsEnumerable() Select tbl.Field(Of String)("Email")).FirstOrDefault()
            Contactinformation.Add("Account Manager", If(String.IsNullOrEmpty(FirstName), "", FirstName) & " " & If(String.IsNullOrEmpty(LastName), "", LastName))
            Contactinformation.Add("Phone", If(String.IsNullOrEmpty(Phone), "", Phone))
            Contactinformation.Add("Email", If(String.IsNullOrEmpty(Email), "", Email))

            'Fill Capsule Information
            Capsule_Type.Add("Capsule Type", lblCapsuleType.Text)
            Capsule_Type.Add("Color", lblCapsuleColor.Text)

            'FillCompany Info 
            CompanyInfo.Add("B301, Everest Nivara Infotech Park,  Turbhe,")
            CompanyInfo.Add("Navi Mumbai 400 705. India")
            'CompanyInfo.Add("Ph: 631-249-4811 www.brandnutra.com")

            txtExpiry.Text = If(IsDBNull(FormulaDT.Rows(0)("QuoteExpiryDisclaimer")), "", FormulaDT.Rows(0)("QuoteExpiryDisclaimer"))
            txtTestingCost.Text = If(IsDBNull(FormulaDT.Rows(0)("LabTestingCostPolicy")), "", FormulaDT.Rows(0)("LabTestingCostPolicy"))
            RichTextBox1.Text = If(IsDBNull(FormulaDT.Rows(0)("Message")), "", FormulaDT.Rows(0)("Message"))


            Dim dataView = New DataView(dsSpec.Tables("salestxns"), "PackagingFormat ='" & PackagingFormat & "' AND VersionNumber='" & lblVersionNumber.Text & "'", "", DataViewRowState.CurrentRows)
            gridBulkSales.AutoGenerateColumns = False
            gridBulkSales.DataSource = dataView

            If chkDisplayBag.Checked Then
                dataView = New DataView(dsSpec.Tables("salesBagsTxns"), "PackagingFormat ='" & PackagingFormat & "' AND VersionNumber='" & lblVersionNumber.Text & "'", "", DataViewRowState.CurrentRows)
                gridbBulkSalesBags.AutoGenerateColumns = False
                gridbBulkSalesBags.DataSource = dataView
            ElseIf chkDisplayBox.Checked Then
                dataView = New DataView(dsSpec.Tables("salesBoxTxns"), "PackagingFormat ='" & PackagingFormat & "' AND VersionNumber='" & lblVersionNumber.Text & "'", "", DataViewRowState.CurrentRows)
                gridBoxSales.AutoGenerateColumns = False
                gridBoxSales.DataSource = dataView
            End If

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Panel2.BringToFront()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Panel3.BringToFront()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            PageNumber = 0

            Dim FileToDelete As String = Path.GetTempPath() & FormulaID.Text & "myimg.png"
            Dim FileToDelete2 As String = Path.GetTempPath() & FormulaID.Text & "myimg2.png"

            If System.IO.File.Exists(FileToDelete) = True Then
                Try
                    System.IO.File.Delete(FileToDelete)
                    System.IO.File.Delete(FileToDelete2)
                Catch ex As System.Exception

                End Try

            End If

            Dim Image1 As New Bitmap(My.Resources.brand_nutra_green_logo)
            Image1.Save(System.IO.Path.GetTempPath() & FormulaID.Text & "myimg.png")

            ''Generates the check mark used for "Approved by Management"
            Dim Image2 As New Bitmap(My.Resources.footer_icons)
            Image2.Save(System.IO.Path.GetTempPath() & FormulaID.Text & "myimg2.png")

            GenerateQuote()
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub
    Private Sub GenerateQuote()
        Try
            If DataGridView1.RowCount = 0 Then
                Exit Sub
            End If
            pdf = New PdfDocument
            pdf.Info.Title = "Formula Pricer"
            Dim pdfFilename As String = Me.Tag & " Specification.pdf"

            Dim filestring As String
            filestring = System.IO.Path.GetTempPath() & pdfFilename

            If System.IO.File.Exists(filestring) = True Then
                Try
                    Dim fOpen As FileStream = File.Open(System.IO.Path.GetTempPath() & pdfFilename, FileMode.Open, FileAccess.Read, FileShare.None)

                    fOpen.Close()
                    fOpen.Dispose()
                    fOpen = Nothing
                Catch e1 As IOException
                    MessageBox.Show("File is already open..")
                    Exit Sub
                Catch e2 As System.Exception
                    MessageBox.Show("This file is open")
                    Exit Sub
                End Try
            End If


            pdfPage = pdf.AddPage
            PageNumber += 1
            graph = XGraphics.FromPdfPage(pdfPage)
            Xlength = pdfPage.Height
            PrintPageNumber()
            PrintedPerPage = 0

            RowsPrinted = 0

            'Prints the ingredients
            Dim y As Integer
            y = IngredientsPrinting()

            If Label11.Text = "Bulk" Then
                y += 20
            End If

            Dim grid As DataGridView = GetCurrentSalesGrid()


            'Prints Bottles Data
            If Label11.Text = "Bottles" Then
                Dim custDV As DataView = New DataView(dsSpec.Tables(0), "SizeCount = " & ComboBox4.Items(0).ToString() & " AND VersionNumber=" & lblVersionNumber.Text, "BindingIndex ASC", DataViewRowState.CurrentRows)
                If (Xlength - y) >= custDV.Count * 20 Then
                    y = PrintBottle(pdfPage, y)
                Else
                    y = PrintBottle(pdfPage, y, False)
                End If
            End If

            'Print sachets Data
            If Label11.Text = "Sachets" Then

                If chkDisplayBox.Checked Then
                    If (Xlength - y) >= 200 Then
                        y = PrintSachets(pdfPage, y)
                    ElseIf (Xlength - y) >= 90 Then
                        y = PrintSachets(pdfPage, y, True, True)
                    Else
                        y = PrintSachets(pdfPage, y, False)
                    End If
                ElseIf chkDisplayBag.Checked Then
                    If (Xlength - y) >= 250 Then
                        y = PrintSachets(pdfPage, y)
                    ElseIf (Xlength - y) >= 90 Then
                        y = PrintSachets(pdfPage, y, True, True)
                    Else
                        y = PrintSachets(pdfPage, y, False)
                    End If
                Else
                    If (Xlength - y) >= 90 Then
                        y = PrintSachets(pdfPage, y)
                    Else
                        y = PrintSachets(pdfPage, y, False)
                    End If

                End If

            End If

            ''Print Bag Data
            If Label11.Text = "Bags" Then
                If (Xlength - y) >= 155 Then
                    y = PrintBag(pdfPage, y)
                Else
                    pdfPage = pdf.AddPage()
                    PageNumber += 1
                    PrintPageNumber()
                    y = PrintBag(pdfPage, y, False)
                End If

            End If

            ''Print StickPack Data
            If Label11.Text = "Stick Packs" Then
                If chkDisplayBox.Checked Then
                    If (Xlength - y) >= 250 Then
                        y = PrintStickPack(pdfPage, y)
                    ElseIf (Xlength - y) >= 90 Then
                        y = PrintStickPack(pdfPage, y, True, True)
                    Else
                        y = PrintStickPack(pdfPage, y, False)
                    End If
                ElseIf chkDisplayBag.Checked Then
                    If (Xlength - y) >= 250 Then
                        y = PrintStickPack(pdfPage, y)
                    ElseIf (Xlength - y) >= 90 Then
                        y = PrintStickPack(pdfPage, y, True, True)
                    Else
                        y = PrintStickPack(pdfPage, y, False)
                    End If
                Else
                    If (Xlength - y) >= 90 Then
                        y = PrintStickPack(pdfPage, y)
                    Else
                        y = PrintStickPack(pdfPage, y, False)
                    End If
                End If
            End If

            'print Blister Data
            If Label11.Text = "Blister" Then
                If chkDisplayBox.Checked Then
                    If (Xlength - y) >= 200 Then
                        y = PrintBlister(pdfPage, y)
                    ElseIf (Xlength - y) >= 90 Then
                        y = PrintBlister(pdfPage, y, True, True)
                    Else
                        y = PrintBlister(pdfPage, y, False)
                    End If
                Else
                    If (Xlength - y) >= 90 Then
                        y = PrintBlister(pdfPage, y)
                    Else
                        y = PrintBlister(pdfPage, y, False)
                    End If
                End If
            End If

            If (Xlength - (y + 20)) >= 40 Then
                y = PrintDisclaimer_Policy(pdfPage, y, True)
            Else
                y = PrintDisclaimer_Policy(pdfPage, y, False)
            End If


            Dim TextLine As List(Of String) = Helper.Wordwrap(RichTextBox1, 80)
            Dim YLength As Integer
            If TextLine.Count = 1 Or TextLine.Count = 2 Or TextLine.Count = 0 Then
                YLength = 3 * 15
            Else
                YLength = TextLine.Count * 15
            End If

            YTermCondition = YLength + 10

            'Prints Terms And Condition for the firm
            If (Xlength - (y + 20)) >= YTermCondition Then
                TermsConditions(pdfPage, y + 20, True)
            Else
                TermsConditions(pdfPage, y + 20, False)
            End If

            pdf.Save(System.IO.Path.GetTempPath() & pdfFilename)

            Process.Start(New ProcessStartInfo(System.IO.Path.GetTempPath() & pdfFilename) With {.UseShellExecute = True})

        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    Private Function PrintHeader() As Integer
        Try
            Dim x As Integer = 30
            Dim y As Integer = 30

            Dim font12Bold = New XFont("Arial", 12, XFontStyle.Bold)
            Dim font16Bold = New XFont("Arial", 16, XFontStyle.Bold)
            Dim font14Bold = New XFont("Arial", 14, XFontStyle.Bold)
            Dim font8Bold = New XFont("Arial", 8, XFontStyle.Bold)

            Dim font12Regular = New XFont("Arial", 12, XFontStyle.Regular)
            Dim font14Regular = New XFont("Arial", 14, XFontStyle.Regular)
            Dim font8Regular = New XFont("Arial", 8, XFontStyle.Regular)

            'Print Heading
            Dim Rect = New XRect(30, y, 540, 18)
            graph.DrawRectangle(brushTransparent, Rect)
            graph.DrawString("MANUFACTURING QUOTE", font16Bold, brushBlack, Rect, format)

            y += 30
            'HeaderImage of Formula Specificitacions
            Dim ApplicationLogo As XImage = XImage.FromFile(AppDomain.CurrentDomain.BaseDirectory & "\Images\GTLogo.png")
            graph.DrawImage(ApplicationLogo, x, y, 160, 50)

            PrintComapanyInfo(y)

            Dim Temp_y As Integer
            Dim Temp_TableStrt As Integer

            y = 78
            'Print Formula Quote Number And Date   
            y = PrintRightData(QuoteNumber_Date, y)
            Temp_y = y
            'Data Second Table - Start
            'Print Serving    

            y = Temp_y + 10
            Temp_TableStrt = y

            y = PrintLeftData(Formula_CustomerInformation, y)
            Temp_y = y

            'Print contact Information
            y = Temp_TableStrt
            y = PrintRightData(Contactinformation, y)

            'Data Second Table - End


            'Data Third Table - Start
            'Print Capsule Type Informtaion
            y = Temp_y + 10
            Temp_TableStrt = y
            y = PrintLeftData(ServingSize, y)

            Temp_y = y

            If FormulaType.Text = "Capsule" Then
                y = Temp_TableStrt
                y = PrintRightData(Capsule_Type, y)
            End If

            'Data Second Table - End

            'PrintFooterImage()
            Return Temp_y
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
        Return 0
    End Function
    Private Sub PrintComapanyInfo(y As Integer)
        'Print Company Info 
        y = 62
        Dim yIncrement = 11
        Dim Rect
        For Each Info In CompanyInfo
            Rect = New XRect(200, y, 170, 15)
            'graph.DrawRectangle(pen, Rect)
            graph.DrawString(Info, font9Regular, brushBlack, 210, y + yIncrement)
            y += 15
        Next

        Dim imgi2 As XImage = XImage.FromFile(AppDomain.CurrentDomain.BaseDirectory & "\Images\Phone_Logo.PNG")
        graph.DrawImage(imgi2, 210, y + 3, 10, 10)
        graph.DrawString("888-221-6661", font9Regular, brushBlack, 223, y + yIncrement)

        imgi2 = XImage.FromFile(AppDomain.CurrentDomain.BaseDirectory & "\Images\Globe_Logo.PNG")
        graph.DrawImage(imgi2, 280, y + 3, 10, 10)
        graph.DrawString("www.greytrix.com", font9Regular, brushBlack, 293, y + yIncrement)
    End Sub
    Private Function IngredientsPrinting(Optional ByVal RowPIndex As Integer = 0) As Integer
        Dim IngrediantHeaderPrinted As Boolean = False
        Dim x As Integer = 30
        Dim header_y As Integer
        Dim y As Integer

        Dim isMaterialAvailable As Boolean

        Dim scale As Double = 0.269
        Dim scale1 As Double = 0.28

        Dim IncrementY As Integer = 12
        Dim Rect

        Rowsperpage = 27

        Dim lastRowIndex As Integer = If(DataGridView1.AllowUserToAddRows, DataGridView1.Rows.Count - 2, DataGridView1.Rows.Count - 1)
        'HeaderPrintingFooter()
        header_y = PrintHeader()

        header_y = PrintSalesData(30, header_y + 15, header_y)

        Dim list As New List(Of String)
        list.Add("Ingredients")
        list.Add("Label Claim (mg)")
        list.Add("Potency %")
        list.Add("Overage %")
        list.Add("Dose (mg)")

        Do While RowPIndex <= lastRowIndex
            ' Draw Header for Each Page
            isMaterialAvailable = True
            If Not IngrediantHeaderPrinted Then
                y = header_y + 6
                Rect = New XRect(30, y, 250, 16)
                graph.DrawRectangle(pen, brushRed, Rect)
                graph.DrawString("Ingredient", font10Bold, brushBlack, 45, y + IncrementY)
                Dim SizePerColumn = 290 / (list.Count - 1)

                Dim StrtColumn As Double = 280
                Dim i As Int16 = 1
                For Each txt In list
                    If i = 1 Then
                        i += 1
                        Continue For
                    End If
                    Rect = New XRect(StrtColumn, y, SizePerColumn, 16)
                    graph.DrawRectangle(pen, brushRed, Rect)
                    graph.DrawString(txt, font10Bold, brushBlack, Rect, format)
                    StrtColumn += SizePerColumn
                    i += 1
                Next
                i = 0
            End If

            y += 16

            Dim rowsOnPage As Integer = 0
            Dim YEndIndex As Integer = 0
            Dim YStrtIndex As Integer = 0
            While RowPIndex <= lastRowIndex AndAlso rowsOnPage < Rowsperpage
                With DataGridView1

                    Dim FormulaDetail_List As New List(Of String)

                    ' Print Ingredient Name (truncate if needed)
                    FormulaDetail_List.Add((If(IsDBNull(.Rows(RowPIndex).Cells(1).Value), "", .Rows(RowPIndex).Cells(1).Value)))
                    FormulaDetail_List.Add((If(IsDBNull(.Rows(RowPIndex).Cells(2).Value), "", Convert.ToDecimal(.Rows(RowPIndex).Cells(2).Value).ToString("N2"))))
                    FormulaDetail_List.Add((If(IsDBNull(.Rows(RowPIndex).Cells(3).Value), "", Convert.ToDecimal(.Rows(RowPIndex).Cells(3).Value).ToString("N2"))))
                    FormulaDetail_List.Add((If(IsDBNull(.Rows(RowPIndex).Cells(4).Value), "", Convert.ToDecimal(.Rows(RowPIndex).Cells(4).Value).ToString("N0"))))
                    FormulaDetail_List.Add((If(IsDBNull(.Rows(RowPIndex).Cells(5).Value), "", Convert.ToDecimal(.Rows(RowPIndex).Cells(5).Value).ToString("N2"))))

                    Dim Lst = New List(Of String)
                    lblIngredientsDetail.Text = FormulaDetail_List(0)

                    Lst = Helper.Wordwrap(lblIngredientsDetail, 55)

                    Rect = New XRect(30, y, 250, Lst.Count * 16)
                    YStrtIndex = y
                    graph.DrawRectangle(pen, brushTransparent, Rect)
                    For Each l In Lst
                        graph.DrawString(l, font10Regular, brushBlack, 45, y + IncrementY)
                        y += 16
                        YEndIndex = y
                    Next

                    y = YStrtIndex


                    Dim SizePerColumn = 290 / (list.Count - 1)

                    Dim StrtColumn As Double = 280
                    Dim i As Int16 = 1
                    For Each txt In FormulaDetail_List
                        If i = 1 Then
                            i += 1
                            Continue For
                        End If
                        Rect = New XRect(StrtColumn, y, SizePerColumn, Lst.Count * 16)
                        graph.DrawRectangle(pen, brushTransparent, Rect)
                        graph.DrawString(txt, font10Regular, brushBlack, Rect, format)
                        StrtColumn += SizePerColumn

                    Next
                    i = 0
                    If YEndIndex = 0 Then
                        y += 16
                    Else
                        y = YEndIndex
                    End If

                    RowsPrinted += 1
                    RowPIndex += 1
                    rowsOnPage += 1
                End With
            End While

            ' Setup for next page if needed
            If RowPIndex <= lastRowIndex Then
                Rowsperpage = 38
                pdfPage = pdf.AddPage()
                PageNumber += 1
                graph = XGraphics.FromPdfPage(pdfPage)
                PrintPageNumber()
                'PrintFooterImage()
                y = 30
                RowsPrinted = 0
                IngrediantHeaderPrinted = True
            End If
        Loop

        If Not isMaterialAvailable Then
            y = header_y
        End If

        Dim Line As List(Of String) = Helper.Wordwrap(rtxtOtherIngredients, 100)
        'Dim Line As List(Of String) = Helper.Wordwrap(rtxtOtherIngredients, 95)
        Rect = New XRect(30, y, 140, Line.Count * 16)
        graph.DrawRectangle(pen, brushLightGray, Rect)
        graph.DrawString("Expected Other Ingredients:", font10Bold, brushBlack, 45, y + IncrementY)

        Rect = New XRect(170, y, 400, Line.Count * 16)
        graph.DrawRectangle(pen, brushTransparent, Rect)

        For Each txt In Line
            graph.DrawString(txt, font10Regular, brushBlack, 180, y + IncrementY)
            y += 12
        Next


        YIndex = y + 6

        Return YIndex
    End Function

    Private Sub PrintFooterImage()
        Dim imgi2 As XImage = XImage.FromFile(AppDomain.CurrentDomain.BaseDirectory & "\Images\footer_icons.jpg")
        graph.DrawImage(imgi2, 60, 800, 486, 18)
    End Sub
    Private Function PrintBottle(ei As PdfPage, y As Integer, Optional IsSamePage As Boolean = True) As Integer

        y += 20
        If Not IsSamePage Then
            pdfPage = pdf.AddPage
            PageNumber += 1
            graph = XGraphics.FromPdfPage(pdfPage)
            PrintPageNumber()
            y = 30
        End If

        Dim Rect = New XRect(30, y, 540, 16)
        graph.DrawRectangle(pen, brushRed, Rect)
        graph.DrawString(ComboBox4.Items(0).ToString() & " CT", font10Bold, brushBlack, 40, y + 11)
        y += 16

        Dim custDV As DataView = New DataView(dsSpec.Tables(0), "SizeCount = " & ComboBox4.Items(0).ToString() & " AND VersionNumber=" & lblVersionNumber.Text, "BindingIndex ASC", DataViewRowState.CurrentRows)

        For Each r As DataRowView In custDV
            If Not IsDBNull(r.Item("Category")) Then
                If FormulaType.Text = "Powder" Then
                    If Not IsDBNull(r("Category")) Then
                        Dim category As String = r("Category").ToString()
                        If category = "Labor" OrElse category = "Cotton" Then
                            Continue For
                        End If
                    End If
                ElseIf FormulaType.Text = "Capsule" Or FormulaType.Text = "Tablet" Then
                    If Not IsDBNull(r("Category")) Then
                        Dim category As String = r("Category").ToString()
                        If category = "Labor" OrElse category = "Scoops" Then
                            Continue For
                        End If
                    End If
                End If

                Rect = New XRect(30, y, 130, 16)
                graph.DrawRectangle(pen, brushLightGray, Rect)
                graph.DrawString(r.Item("Category") & ":", font10Bold, XBrushes.Black, 40, y + 11)


                Rect = New XRect(160, y, 410, 16)
                graph.DrawRectangle(pen, brushTransparent, Rect)
                graph.DrawString(r.Item("MaterialName").ToString(), font10Regular, XBrushes.Black, 170, y + 11)

                y += 16
            End If
        Next

        Return y + 20
    End Function
    Private Function PrintSalesData(X As Integer, y As Integer, header_y As Integer) As Integer
        Dim PageCenter As Integer = 250
        Dim IncrementY As Integer = 11


        Dim maxRows As Integer = 0
        If Not chkDisplayBox.Checked And Not chkDisplayBag.Checked Then
            maxRows = gridBulkSales.Rows.Count
        ElseIf chkDisplayBag.Checked And (Label11.Text = "Stick Packs" Or Label11.Text = "Sachets") Then
            maxRows = gridbBulkSalesBags.Rows.Count
        ElseIf chkDisplayBox.Checked Then
            maxRows = gridBoxSales.Rows.Count
        End If

        Dim Rect = New XRect(30, y, 540, 16)
        graph.DrawRectangle(pen, brushRed, Rect)
        y += 11
        If chkDisplayBox.Checked Then
            If Label11.Text = "Sachets" Then
                graph.DrawString("SALES PRICES: SACHET DISPLAY/BOX", font10Bold, brushBlack, Rect, format)
            ElseIf Label11.Text = "Blister" Then
                graph.DrawString("SALES PRICES BLISTER BOX", font10Bold, brushBlack, Rect, format)
            ElseIf Label11.Text = "Stick Packs" Then
                graph.DrawString("SALES PRICES STICK PACKETS DISPLAY BOX", font10Bold, brushBlack, Rect, format)
            End If

        ElseIf chkDisplayBag.Checked And Label11.Text = "Stick Packs" Then
            graph.DrawString("SALES PRICES STICK PACKET BAG", font10Bold, brushBlack, Rect, format)
        ElseIf chkDisplayBag.Checked And Label11.Text = "Sachets" Then
            graph.DrawString("SALES PRICES SACHETS BAG", font10Bold, brushBlack, Rect, format)
        Else
            If Label11.Text = "Sachets" Then
                graph.DrawString("SALES PRICES BULK SACHETS", font10Bold, brushBlack, Rect, format)
            ElseIf Label11.Text = "Blister" Then
                graph.DrawString("SALES PRICES BULK BLISTER", font10Bold, brushBlack, Rect, format)
            ElseIf Label11.Text = "Stick Packs" Then
                graph.DrawString("SALES PRICES BULK STICK PACKETS", font10Bold, brushBlack, Rect, format)
            ElseIf Label11.Text = "Bags" Then
                graph.DrawString("SALES PRICES STANDUP BAGS/POUCHES", font10Bold, brushBlack, Rect, format)
            ElseIf Label11.Text = "Bulk" Then
                graph.DrawString("SALES PRICES", font10Bold, brushBlack, Rect, format)
            ElseIf Label11.Text = "Bottles" Then
                graph.DrawString("SALES PRICES", font10Bold, brushBlack, Rect, format)
            End If

        End If

        y += 5

        Dim grid As DataGridView = GetCurrentSalesGrid()

        Dim SizeperColumn = 390 / (grid.Rows.Count)
        Dim StrtSalesData As Integer
        For i As Integer = 1 To 2
            Rect = New XRect(30, y, 150, 16)
            graph.DrawRectangle(pen, brushLightGray, Rect)
            StrtSalesData = 180

            If i = 1 Then
                If Label11.Text = "Bulk" Then
                    If FormulaType.Text = "Capsule" Then
                        graph.DrawString("Quantity Capsules (per/m)", font10Bold, brushBlack, Rect, format)
                    ElseIf FormulaType.Text = "Powder" Then
                        graph.DrawString("Quantity (kg)", font10Bold, brushBlack, Rect, format)
                    Else
                        graph.DrawString("Quantity Tablets (per/m)", font10Bold, brushBlack, Rect, format)
                    End If
                Else
                    graph.DrawString("Quantity (units)", font10Bold, brushBlack, Rect, format)
                End If
            Else
                If Label11.Text = "Bulk" Then
                    If FormulaType.Text = "Capsule" Then
                        graph.DrawString("Price/m (Rs)", font10Bold, brushBlack, Rect, format)
                    ElseIf FormulaType.Text = "Powder" Then
                        graph.DrawString("Price/kg (Rs)", font10Bold, brushBlack, Rect, format)
                    Else
                        graph.DrawString("Price/m (Rs)", font10Bold, brushBlack, Rect, format)
                    End If
                Else
                    graph.DrawString("Price/unit (Rs)", font10Bold, brushBlack, Rect, format)
                End If
            End If

            For Each row In grid.Rows
                If Not String.IsNullOrEmpty(row.Cells(0).Value) Then
                    Rect = New XRect(StrtSalesData, y, SizeperColumn, 16)
                    graph.DrawRectangle(pen, brushTransparent, Rect)
                    If i = 1 Then
                        graph.DrawString(String.Format(culture, "{0:N0}", row.Cells(0).Value), font10Regular, brushBlack, Rect, format)
                    Else
                        Dim DecimalValue = Convert.ToDecimal(If(String.IsNullOrEmpty(row.Cells(1).Value) Or IsDBNull(row.Cells(1).Value), "0.00", row.Cells(1).Value))
                        graph.DrawString(DecimalValue.ToString("N2"), font10Regular, brushBlack, Rect, format)
                    End If

                    StrtSalesData += SizeperColumn - 0.1

                End If
            Next


            y += 16
        Next

        Return y + 20
    End Function

    Private Function PrintBag(ei As PdfPage, y As Integer, Optional IsSamePage As Boolean = True) As Integer
        y += 20
        If Not IsSamePage Then
            graph = XGraphics.FromPdfPage(ei)
            PrintPageNumber()
            'HeaderPrintingFooter()
            y = 30
        End If

        Dim BagBulkDict As New Dictionary(Of String, String)

        BagBulkDict.Add(TextUpper(lblDescription), CheckEmptyNull(txtDexcription))
        BagBulkDict.Add(TextUpper(lblQty), CheckEmptyNull(txtQty))
        BagBulkDict.Add(TextUpper(lblMaterial), CheckEmptyNull(txtMaterial))
        BagBulkDict.Add(TextUpper(lblSize), CheckEmptyNull(txtSize))
        BagBulkDict.Add(TextUpper(lblColor), CheckEmptyNull(txtPrintColors))
        BagBulkDict.Add(TextUpper(lblTearNotch), CheckEmptyNull(cmbTearNotch))
        BagBulkDict.Add(TextUpper(lblZipper), CheckEmptyNull(cmbZipper))
        BagBulkDict.Add(TextUpper(lblHanger), CheckEmptyNull(cmbHangerHole))

        Dim BagBulkACDict As New Dictionary(Of String, String)
        BagBulkACDict.Add(TextUpper(lblPrintingPlates), CheckEmptyNull(txtPrintingPlates))
        BagBulkACDict.Add(TextUpper(lblArtPreperation), CheckEmptyNull(txtArtPreperation))
        If Not String.IsNullOrEmpty(txtOtherDesc.Text) Then
            BagBulkACDict.Add(TextUpper(txtOtherDesc), CheckEmptyNull(txtOther))
        End If
        BagBulkACDict.Add(TextUpper(lblShipperCaseCount), CheckEmptyNull(txtShipperCaseCount))

        Dim HeaderYIndex As Integer = y

        'Print Bulk Data
        y = PrintPackagingHeaderLeft(y, Label16)
        y = PrintLeftData(BagBulkDict, y, True)

        Dim TermAndConditionYIndex As Integer = y + 20
        y = HeaderYIndex

        'Print Bulk AC Data
        y = PrintPackagingHeaderRight(y, lblASStandupBag)
        y = PrintRightData(BagBulkACDict, y, True)

        Return TermAndConditionYIndex
    End Function

    Private Function PrintBlister(ei As PdfPage, y As Integer, Optional IsSamePage As Boolean = True, Optional IsBoxNextPage As Boolean = False) As Integer
        Dim ParseNumber As Double
        y += 20
        If Not IsSamePage Then
            pdfPage = pdf.AddPage
            PageNumber += 1
            graph = XGraphics.FromPdfPage(pdfPage)
            PrintPageNumber()
            y = 30
        End If

        Dim BlisterBulkDict As New Dictionary(Of String, String)
        BlisterBulkDict.Add(TextUpper(lblBlisterFormat), CheckEmptyNull(txtBlisterFormat))
        BlisterBulkDict.Add(TextUpper(lblBlisterSize), CheckEmptyNull(txtBlisterSize))
        BlisterBulkDict.Add(TextUpper(Label76Blister), CheckEmptyNull(txtboxBlisterMaterial))
        BlisterBulkDict.Add(TextUpper(Label106Blister), CheckEmptyNull(txtBlisterQty))
        BlisterBulkDict.Add(TextUpper(Label107Blister), CheckEmptyNull(txtBlisterCount) & " " & cmbBlisterCountType.Text)

        Dim BlisterBulkACDict As New Dictionary(Of String, String)
        BlisterBulkACDict.Add(TextUpper(Label29), CheckEmptyNull(txtPrintingBulk))
        BlisterBulkACDict.Add(TextUpper(Label32), CheckEmptyNull(txtArtPrepBulk))

        If Not String.IsNullOrEmpty(txtOtherBulk.Text) Then
            BlisterBulkACDict.Add(TextUpper(txtOtherBulk), CheckEmptyNull(txtOtherCostBulk))
        End If

        If Not chkDisplayBox.Checked Then
            BlisterBulkACDict.Add(TextUpper(Label33), CheckEmptyNull(txtToolingCostBulk))
            BlisterBulkACDict.Add(TextUpper(Label34), CheckEmptyNull(txtShipperCountBulk))
        End If

        Dim BlisterBoxDict As New Dictionary(Of String, String)
        BlisterBoxDict.Add(TextUpper(Label114QtyDiplaybox), CheckEmptyNull(txtQtyDiplaybox))
        BlisterBoxDict.Add(TextUpper(Label117DescDiplayBox), CheckEmptyNull(txtDescDiplaybox))
        BlisterBoxDict.Add(TextUpper(Label117SpecDiplaybox), CheckEmptyNull(txtSepcDisplaybox))
        BlisterBoxDict.Add(TextUpper(Label117BoardGrade), CheckEmptyNull(txtBoradGradeDiplaybox))
        BlisterBoxDict.Add(TextUpper(Label117Dimension), CheckEmptyNull(txtDimensionDisplaybox))
        BlisterBoxDict.Add(TextUpper(Label117ProductStyle), CheckEmptyNull(txtProductStyle))
        BlisterBoxDict.Add(TextUpper(Label22), CheckEmptyNull(txtBlisterCountAmount))
        BlisterBoxDict.Add("CONTAINER/UNIT COUNT", Convert.ToInt16(txtBlisterCount.Text) * Convert.ToInt16(txtBlisterCountAmount.Text))

        Dim BlisterBoxACDict As New Dictionary(Of String, String)
        BlisterBoxACDict.Add(TextUpper(Label116Blister), CheckEmptyNull(txtBlisterPrintingPlatesAmt))
        BlisterBoxACDict.Add(TextUpper(Label121Blister), CheckEmptyNull(txtBlisterArtPreperationAmt))
        'Double.TryParse(txtBlisterPackoutAmt.Text, ParseNumber)
        'BlisterBoxACDict.Add(TextUpper(Label122Blister), If(ParseNumber > 0, "YES", "NO"))
        Double.TryParse(txtBlisterShrinkWrap.Text, ParseNumber)
        BlisterBoxACDict.Add(TextUpper(Label123Blister), If(ParseNumber > 0, "YES", "NO"))
        Double.TryParse(txtBlisterWaferSeal.Text, ParseNumber)
        BlisterBoxACDict.Add(TextUpper(Label124Blister), If(ParseNumber > 0, "YES", "NO"))
        BlisterBoxACDict.Add(TextUpper(lblblistertoolcost), CheckEmptyNull(txttoolcostamtblister))
        If Not String.IsNullOrEmpty(txtOtherDescBlister.Text) Then
            BlisterBoxACDict.Add(TextUpper(txtOtherDescBlister), CheckEmptyNull(txtBlisterOtherAmt))
        End If

        BlisterBoxACDict.Add(TextUpper(Label126Blister), CheckEmptyNull(txtBlisterCaseCountAmt))

        Dim HeaderYIndex = y
        y = PrintPackagingHeaderLeft(y, Label18)
        y = PrintLeftData(BlisterBulkDict, y, True)

        Dim RightData As Integer = y

        y = HeaderYIndex
        y = PrintPackagingHeaderRight(y, lblASBlisters)
        y = PrintRightData(BlisterBulkACDict, y, True)
        Dim LeftData As Integer = y


        If RightData > LeftData Then
            y = RightData + 20
        ElseIf LeftData > RightData Then
            y = LeftData + 20
        Else
            y = RightData + 20
        End If

        If chkDisplayBox.Checked Then
            If IsBoxNextPage Then
                pdfPage = pdf.AddPage
                PageNumber += 1
                graph = XGraphics.FromPdfPage(pdfPage)
                PrintPageNumber()
                y = 30
            End If

            HeaderYIndex = y
            y = PrintPackagingHeaderLeft(y, Label114Displaybox)
            y = PrintLeftData(BlisterBoxDict, y, True)

            y = HeaderYIndex
            y = PrintPackagingHeaderRight(y, lblASDBSBlister)
            y = PrintRightData(BlisterBoxACDict, y, True)

            Return y + 20
        Else
            Return y
        End If
    End Function

    Private Function PrintStickPack(ei As PdfPage, y As Integer, Optional IsSamePage As Boolean = True, Optional IsBox_Bag_NextPage As Boolean = False) As Integer
        y += 20
        Dim ParseDouble As Double
        If Not IsSamePage Then
            pdfPage = pdf.AddPage
            PageNumber += 1
            graph = XGraphics.FromPdfPage(pdfPage)
            PrintPageNumber()
            y = 30
        End If

        Dim StickPackBulkDict As New Dictionary(Of String, String)
        StickPackBulkDict.Add(TextUpper(Label140), CheckEmptyNull(ComboBox13))
        StickPackBulkDict.Add(TextUpper(Label139), CheckEmptyNull(ComboBox6))
        StickPackBulkDict.Add(TextUpper(Label132), CheckEmptyNull(ComboBox8))
        StickPackBulkDict.Add(TextUpper(Label113), CheckEmptyNull(ComboBox7))
        StickPackBulkDict.Add(TextUpper(Label120), CheckEmptyNull(TextBox48) & " " & Label135.Text)

        Dim StickPackBulkACDict As New Dictionary(Of String, String)
        StickPackBulkACDict.Add(TextUpper(Label138), CheckEmptyNull(TextBox46))
        StickPackBulkACDict.Add(TextUpper(Label137), CheckEmptyNull(TextBox45))
        If Not String.IsNullOrEmpty(TextBox40.Text) Then
            StickPackBulkACDict.Add(TextUpper(TextBox40), CheckEmptyNull(TextBox43))
        End If

        If Not chkDisplayBox.Checked And Not chkDisplayBag.Checked Then
            StickPackBulkACDict.Add(TextUpper(lblStickPacksShipperCaseCount), CheckEmptyNull(txtStickPacksShipperCaseCount))
        End If

        Dim StickPackBoxDict As New Dictionary(Of String, String)
        StickPackBoxDict.Add(TextUpper(Label67), CheckEmptyNull(TextBox25))
        StickPackBoxDict.Add(TextUpper(Label65), CheckEmptyNull(TextBox24))
        StickPackBoxDict.Add(TextUpper(Label64), CheckEmptyNull(TextBox23))
        StickPackBoxDict.Add(TextUpper(Label63), CheckEmptyNull(TextBox22))
        StickPackBoxDict.Add(TextUpper(Label62), CheckEmptyNull(TextBox21))
        StickPackBoxDict.Add(TextUpper(Label61), CheckEmptyNull(TextBox20))

        Dim StickPackBoxACDict As New Dictionary(Of String, String)
        StickPackBoxACDict.Add(TextUpper(Label42), CheckEmptyNull(TextBox7))
        StickPackBoxACDict.Add(TextUpper(Label43), CheckEmptyNull(TextBox6))
        'Double.TryParse(TextBox5.Text, ParseDouble)
        'StickPackBoxACDict.Add(TextUpper(Label44), If(ParseDouble > 0, "YES", "NO"))
        Double.TryParse(txtShrinkWrapStickPack.Text, ParseDouble)
        StickPackBoxACDict.Add(TextUpper(lblShrinkWrap), If(ParseDouble > 0, "YES", "NO"))
        Double.TryParse(txtWaferSeal.Text, ParseDouble)
        StickPackBoxACDict.Add(TextUpper(lblWaferSeal), If(ParseDouble > 0, "YES", "NO"))
        If Not String.IsNullOrEmpty(TextBox3.Text) Then
            StickPackBoxACDict.Add(TextUpper(TextBox3), CheckEmptyNull(TextBox4))
        End If
        StickPackBoxACDict.Add(TextUpper(Label40), CheckEmptyNull(TextBox1))


        Dim StickPackbagDict As New Dictionary(Of String, String)
        StickPackbagDict.Add(TextUpper(lblStickPackBagDescription), CheckEmptyNull(txtStickPackBagDescription))
        StickPackbagDict.Add(TextUpper(lblStickPackBagQty), CheckEmptyNull(txtStickPackBagQty))
        StickPackbagDict.Add(TextUpper(lblStickPackBagMaterial), CheckEmptyNull(txtStickPackBagMaterial))
        StickPackbagDict.Add(TextUpper(lblStickPackBagSize), CheckEmptyNull(txtStickPackBagSize))
        StickPackbagDict.Add(TextUpper(lblStickPackBagPrintColor), CheckEmptyNull(txtStickPackBagPrintColors))
        StickPackbagDict.Add(TextUpper(lblStickPackBagTearNotch), CheckEmptyNull(cmbStickPackBagTearNotch))
        StickPackbagDict.Add(TextUpper(lblStickPackBagZipper), CheckEmptyNull(cmbStickPackBagZipper))
        StickPackbagDict.Add(TextUpper(lblStickPackBagHangerHole), CheckEmptyNull(cmbStickPackBagHangerHole))


        Dim StickPackbagACDict As New Dictionary(Of String, String)
        StickPackbagACDict.Add(TextUpper(Label169), CheckEmptyNull(txtPrintingPlatesStickBag))
        StickPackbagACDict.Add(TextUpper(Label168), CheckEmptyNull(txtArtPreStickBag))
        'StickPackbagACDict.Add(TextUpper(Label156),  CheckEmptyNull(txtSecondaryPackStickBag))
        If Not String.IsNullOrEmpty(txtOtherStickBag.Text) Then
            StickPackbagACDict.Add(TextUpper(txtOtherStickBag), CheckEmptyNull(txtOtherCostStickBag))
        End If
        StickPackbagACDict.Add(TextUpper(Label166), CheckEmptyNull(txtShipperCountStickBag))

        Dim HeaderYIndex = y
        y = PrintPackagingHeaderLeft(y, Label141)
        y = PrintLeftData(StickPackBulkDict, y, True)

        Dim RightData As Integer = y

        y = HeaderYIndex
        y = PrintPackagingHeaderRight(y, lblASStickPacks)
        y = PrintRightData(StickPackBulkACDict, y, True)

        Dim leftData As Integer = y

        If RightData > leftData Then
            y = RightData + 20
        ElseIf leftData > RightData Then
            y = leftData + 20
        Else
            y = RightData + 20
        End If
        If chkDisplayBox.Checked Then
            If IsBox_Bag_NextPage Then
                pdfPage = pdf.AddPage
                PageNumber += 1
                graph = XGraphics.FromPdfPage(pdfPage)
                PrintPageNumber()
                y = 30
            End If

            HeaderYIndex = y
            y = PrintPackagingHeaderLeft(y, Label66)
            y = PrintLeftData(StickPackBoxDict, y, True)
            leftData = y
            y = HeaderYIndex
            y = PrintPackagingHeaderRight(y, lblASDBStickPack)
            y = PrintRightData(StickPackBoxACDict, y, True)
            RightData = y
            If RightData > leftData Then
                y = RightData + 20
            ElseIf leftData > RightData Then
                y = leftData + 20
            Else
                y = RightData + 20
            End If
            Return y + 20
        ElseIf chkDisplayBag.Checked Then
            If IsBox_Bag_NextPage Then
                pdfPage = pdf.AddPage
                PageNumber += 1
                graph = XGraphics.FromPdfPage(pdfPage)
                PrintPageNumber()
                y = 30
            End If

            HeaderYIndex = y
            y = PrintPackagingHeaderLeft(y, Label145)
            y = PrintLeftData(StickPackbagDict, y, True)
            leftData = y
            y = HeaderYIndex
            y = PrintPackagingHeaderRight(y, lblASDisplayBag)
            y = PrintRightData(StickPackbagACDict, y, True)
            RightData = y
            If RightData > leftData Then
                y = RightData
            ElseIf leftData > RightData Then
                y = leftData
            Else
                y = RightData
            End If

            Return y + 20
        End If
        Return y
    End Function

    Private Function PrintSachets(ei As PdfPage, y As Integer, Optional IsSamePage As Boolean = True, Optional IsBoxNextPage As Boolean = False) As Integer
        y += 20
        Dim parseDouble As Double
        If Not IsSamePage Then
            pdfPage = pdf.AddPage
            PageNumber += 1
            graph = XGraphics.FromPdfPage(pdfPage)
            PrintPageNumber()
            y = 30
        End If

        Dim SachetsBulkDict As New Dictionary(Of String, String)
        SachetsBulkDict.Add(TextUpper(Label103), CheckEmptyNull(ComboBox11))
        SachetsBulkDict.Add(TextUpper(Label104), CheckEmptyNull(ComboBox12))
        SachetsBulkDict.Add(TextUpper(Label95), CheckEmptyNull(ComboBox10))
        SachetsBulkDict.Add(TextUpper(Label82), CheckEmptyNull(ComboBox9))
        SachetsBulkDict.Add(TextUpper(Label90), CheckEmptyNull(TextBox35) & " " & Label98.Text)

        Dim SachetsBulkACDict As New Dictionary(Of String, String)
        SachetsBulkACDict.Add(TextUpper(Label152), CheckEmptyNull(txtSachetPrintingPlatesBulk))
        SachetsBulkACDict.Add(TextUpper(Label153), CheckEmptyNull(txtSachetArtPrepBulk))
        If Not String.IsNullOrEmpty(txtSachetOtherCostDesc.Text) Then
            SachetsBulkACDict.Add(TextUpper(txtSachetOtherCostDesc), CheckEmptyNull(txtSachetOtherCost))
        End If

        If Not chkDisplayBox.Checked Then
            SachetsBulkACDict.Add(TextUpper(Label157), CheckEmptyNull(txtSachetShipperCountBulk))
        End If

        Dim SachetsBoxDict As New Dictionary(Of String, String)
        SachetsBoxDict.Add(TextUpper(Label123), CheckEmptyNull(TextBox38DisplayQty))
        SachetsBoxDict.Add(TextUpper(Label125), CheckEmptyNull(TextBox39Desc))
        SachetsBoxDict.Add(TextUpper(Label129), CheckEmptyNull(TextBox44Spec))
        SachetsBoxDict.Add(TextUpper(Label128), CheckEmptyNull(TextBox43BoardGrade))
        SachetsBoxDict.Add(TextUpper(Label127), CheckEmptyNull(TextBox42Dimension))
        SachetsBoxDict.Add(TextUpper(Label126), CheckEmptyNull(TextBox41ProductStyle))

        Dim SachetsBoxACDict As New Dictionary(Of String, String)
        SachetsBoxACDict.Add(TextUpper(Label51), CheckEmptyNull(TextBox11))
        SachetsBoxACDict.Add(TextUpper(Label52), CheckEmptyNull(TextBox13))
        Double.TryParse(TextBox14.Text, parseDouble)
        SachetsBoxACDict.Add(TextUpper(Label146), If(parseDouble > 0, "YES", "NO"))
        'Double.TryParse(TextBox12.Text, parseDouble)
        'SachetsBoxACDict.Add(TextUpper(Label54), If(parseDouble > 0, "YES", "NO"))
        Double.TryParse(TextBox10.Text, parseDouble)
        SachetsBoxACDict.Add(TextUpper(Label143), If(parseDouble > 0, "YES", "NO"))
        If Not String.IsNullOrEmpty(TextBox16.Text) Then
            SachetsBoxACDict.Add(TextUpper(TextBox16), CheckEmptyNull(TextBox15))
        End If

        SachetsBoxACDict.Add(TextUpper(Label55), CheckEmptyNull(TextBox8))

        Dim SachetbagDict As New Dictionary(Of String, String)
        SachetbagDict.Add(TextUpper(lblSachetsBagDescription), CheckEmptyNull(txtSachetsBagDescription))
        SachetbagDict.Add(TextUpper(lblSachetsBagQTY), CheckEmptyNull(txtSachetsBagQTY))
        SachetbagDict.Add(TextUpper(lblSachetsBagMaterial), CheckEmptyNull(txtSachetsBagMaterial))
        SachetbagDict.Add(TextUpper(lblSachetsBagSize), CheckEmptyNull(txtSachetsBagSize))
        SachetbagDict.Add(TextUpper(lblSachetsBagPrintColor), CheckEmptyNull(txtSachetsBagPrintColors))
        SachetbagDict.Add(TextUpper(lblSachetsBagTearNotch), CheckEmptyNull(cmbSachetsBagTearNotch))
        SachetbagDict.Add(TextUpper(lblSachetsBagZipper), CheckEmptyNull(cmbSachetsBagZipper))
        SachetbagDict.Add(TextUpper(lblSachetsBagHangerHole), CheckEmptyNull(cmbSachetsBagHangerHole))


        Dim SachetbagACDict As New Dictionary(Of String, String)
        SachetbagACDict.Add(TextUpper(lblSachetsBagPrintingPlates), CheckEmptyNull(txtSachetsBagPrintingPlates))
        SachetbagACDict.Add(TextUpper(lblSachetsBagArtPreparation), CheckEmptyNull(txtSachetsBagArtPreparation))
        'StickPackbagACDict.Add(TextUpper(Label156),  CheckEmptyNull(txtSecondaryPackStickBag))
        If Not String.IsNullOrEmpty(txtSachetsBagOtherDescription.Text) Then
            SachetbagACDict.Add(TextUpper(txtSachetsBagOtherDescription), CheckEmptyNull(txtSachetsBagOther))
        End If
        SachetbagACDict.Add(TextUpper(lblSachetsBagShipperCaseCount), CheckEmptyNull(txtSachetsBagShipperCaseCount))

        Dim HeaderYIndex = y
        y = PrintPackagingHeaderLeft(y, Label75)
        y = PrintLeftData(SachetsBulkDict, y, True)

        Dim leftData As Integer = y

        y = HeaderYIndex
        y = PrintPackagingHeaderRight(y, lblASSachets)
        y = PrintRightData(SachetsBulkACDict, y, True)

        Dim RightData As Integer = y

        If RightData > leftData Then
            y = RightData + 20
        ElseIf leftData > RightData Then
            y = leftData + 20
        Else
            y = RightData + 20
        End If

        If chkDisplayBox.Checked Then
            If IsBoxNextPage Then
                pdfPage = pdf.AddPage
                PageNumber += 1
                graph = XGraphics.FromPdfPage(pdfPage)
                PrintPageNumber()
                y = 30
            End If

            HeaderYIndex = y
            y = PrintPackagingHeaderLeft(y, Label124)
            y = PrintLeftData(SachetsBoxDict, y, True)

            y = HeaderYIndex
            y = PrintPackagingHeaderRight(y, lblASDBSachets)
            y = PrintRightData(SachetsBoxACDict, y, True)

            Return y + 20
        ElseIf chkDisplayBag.Checked Then
            If IsBoxNextPage Then
                pdfPage = pdf.AddPage
                PageNumber += 1
                graph = XGraphics.FromPdfPage(pdfPage)
                PrintPageNumber()
                y = 30
            End If

            HeaderYIndex = y
            y = PrintPackagingHeaderLeft(y, Label145)
            y = PrintLeftData(SachetbagDict, y, True)
            leftData = y
            y = HeaderYIndex
            y = PrintPackagingHeaderRight(y, lblASDisplayBag)
            y = PrintRightData(SachetbagACDict, y, True)
            RightData = y
            If RightData > leftData Then
                y = RightData
            ElseIf leftData > RightData Then
                y = leftData
            Else
                y = RightData
            End If

            Return y + 20
        End If
        Return y
    End Function

    Private Sub TermsConditions(ei As PdfPage, y As Integer, Optional ISSamePage As Boolean = False)
        Dim TextLine As List(Of String) = Helper.Wordwrap(RichTextBox1, 80)

        Dim format As XStringFormat = XStringFormats.Center
        If Not ISSamePage Then
            pdfPage = pdf.AddPage
            PageNumber += 1
            graph = XGraphics.FromPdfPage(pdfPage)
            PrintPageNumber()
            y = 30
        End If

        Dim greenPen As XPen = New XPen(XColor.FromArgb(60, 151, 61))
        Dim greenBrush As XBrush = New XSolidBrush(XColor.FromArgb(60, 151, 61))

        Dim TermCondition As New Dictionary(Of String, String)

        TermCondition.Add("SHIPPING METHOD", If(String.IsNullOrEmpty(DataGridView4.Rows(0).Cells(1).Value), "", DataGridView4.Rows(0).Cells(1).Value.ToString()))
        TermCondition.Add("PAYMENT TERMS", If(String.IsNullOrEmpty(DataGridView4.Rows(1).Cells(1).Value), "", DataGridView4.Rows(1).Cells(1).Value.ToString()))
        TermCondition.Add("FREIGHT TERMS", If(String.IsNullOrEmpty(DataGridView4.Rows(2).Cells(1).Value), "", DataGridView4.Rows(2).Cells(1).Value.ToString()))

        Dim YHeader As Integer
        YHeader = y
        Dim YLength As Double
        If TextLine.Count = 1 Or TextLine.Count = 2 Or TextLine.Count = 0 Then
            YLength = 3 * 15
        Else
            YLength = TextLine.Count * 15
        End If

        Dim Height As Double = Math.Round(YLength / 3, 2)

        Dim Rect
        LKey = 55
        LValue = 150
        For Each key In TermCondition.Keys
            Rect = New XRect(30, y, 110, Height)

            graph.DrawRectangle(pen, brushLightGray, Rect)
            graph.DrawString(key.ToString(), font9Bold, brushBlack, Rect, format)


            Rect = New XRect(140, y, 150, Height)
            LValue = 150

            graph.DrawRectangle(pen, brushTransparent, Rect)
            graph.DrawString(TermCondition(key).ToString(), font9Regular, brushBlack, Rect, format)
            y += Height

        Next


        y = YHeader

        Rect = New XRect(290, y, 280, Height * 3)
        graph.DrawRectangle(pen, brushTransparent, Rect)
        For Each line In TextLine
            graph.DrawString(line, font9Regular, brushBlack, 300, y + 12)
            y += 15
        Next

        'PrintFooterImage()

    End Sub

    Private Function PrintDisclaimer_Policy(ei As PdfPage, y As Integer, Optional ISSamePage As Boolean = False) As Integer

        If Not ISSamePage Then
            pdfPage = pdf.AddPage
            PageNumber += 1
            graph = XGraphics.FromPdfPage(pdfPage)
            PrintPageNumber()
            y = 30
        End If

        Dim Rect = New XRect(30, y, 540, 16)
        graph.DrawRectangle(pen, brushRed, Rect)
        graph.DrawString(CheckEmptyNull(txtExpiry), font9Bold, brushBlack, Rect, format)

        y += 20
        Rect = New XRect(30, y, 540, 16)
        graph.DrawRectangle(pen, brushRed, Rect)
        graph.DrawString(CheckEmptyNull(txtTestingCost), font9Bold, brushBlack, Rect, format)

        Return y + 20
    End Function

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        Try
            DataGridView2.DataSource = Nothing
            gridBulkSales.DataSource = Nothing

            If ComboBox4.SelectedIndex <> -1 Then
                Dim custDV As DataView = New DataView(dsSpec.Tables(0), "SizeCount = " & ComboBox4.Text & " AND VersionNumber=" & lblVersionNumber.Text, "BindingIndex ASC", DataViewRowState.CurrentRows)
                DataGridView2.Enabled = True

                DataGridView2.AutoGenerateColumns = False
                DataGridView2.DataSource = custDV

                'SalesDVAuto = New DataView(dsSpec.Tables(1), "PackagingFormat =1 AND UnitSize = " & ComboBox4.Text & " AND VersionNumber=" & lblVersionNumber.Text, "", DataViewRowState.CurrentRows)
                'gridBulkSales.AutoGenerateColumns = False
                'gridBulkSales.DataSource = SalesDVAuto

            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub DataGridView3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridBulkSales.KeyDown
        Try
            If e.KeyCode = Keys.Delete Then
                gridBulkSales.CurrentRow.DataBoundItem("Quantity") = DBNull.Value
                gridBulkSales.CurrentRow.DataBoundItem("SalesPrice") = 0.00
                gridBulkSales.Refresh()
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        gridBulkSales.Rows.Add(5000, 23.65)
        gridBulkSales.Rows.Add(10000, 20.65)
    End Sub

    Private Function PrintPackagingHeaderRight(y As Integer, ctl As Control) As Integer
        Try
            Dim Rect = New XRect(310, y, 260, 16)
            graph.DrawRectangle(pen, brushRed, Rect)
            graph.DrawString(ctl.Text.ToUpper, font10Bold, brushBlack, Rect, format)
            Return y + 16
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
        Return y
    End Function

    Private Function PrintPackagingHeaderLeft(y As Integer, ctl As Control) As Integer
        Try
            Dim Rect = New XRect(30, y, 260, 16)
            graph.DrawRectangle(pen, brushRed, Rect)
            graph.DrawString(ctl.Text.ToUpper, font10Bold, brushBlack, Rect, format)
            Return y + 16
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
        Return y
    End Function

    Private Function CheckEmptyNull(ctrl As Control) As String
        Return If(ctrl IsNot Nothing AndAlso Not String.IsNullOrEmpty(ctrl.Text), ctrl.Text, "")
    End Function

    Private Function TextUpper(ctrl As Control) As String
        Return CheckEmptyNull(ctrl).ToUpper()
    End Function

    Private Sub DisplaySalesRepData()
        Try
            Dim DbCon As New MySqlConnection
            Dim dbUp As New MySqlCommand
            DbCon.ConnectionString = My.Settings.DBCon
            DbCon.Open()
            dbUp.Connection = DbCon
            dbUp.CommandType = CommandType.Text
            dbUp.CommandText = "SELECT * FROM salesreps where ID ='" + lblSalesRep.Text + "'"
            Dim dataReader As MySqlDataReader = dbUp.ExecuteReader()
            While dataReader.Read()
                email = dataReader.Item("Email")
                Phone = dataReader.Item("Phone")
            End While

        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
            DbCon.Close()
        End Try

    End Sub

    Private Function GetCurrentSalesGrid() As DataGridView
        Try
            If Not chkDisplayBox.Checked And Not chkDisplayBag.Checked Then
                Return gridBulkSales
            ElseIf chkDisplayBag.Checked And (Label11.Text = "Stick Packs" Or Label11.Text = "Sachets") Then
                Return gridbBulkSalesBags
            ElseIf chkDisplayBox.Checked Then
                Return gridBoxSales
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
        Return Nothing
    End Function

    Private Function PrintRightData(dict As Dictionary(Of String, String), y As Integer, Optional IsPackagingDataDisplay As Boolean = False) As Integer
        Dim Rect

        RKey = 390
        RValue = 470


        For Each key In dict.Keys
            If IsPackagingDataDisplay Then
                Rect = New XRect(310, y, 110, 16)
                RKey = 320
            Else
                Rect = New XRect(385, y, 80, 16)
            End If

            graph.DrawRectangle(pen, brushLightGray, Rect)
            graph.DrawString(key.ToString(), font9Bold, brushBlack, RKey, y + 11)


            If IsPackagingDataDisplay Then
                Rect = New XRect(420, y, 150, 16)
                RValue = 430
            Else
                Rect = New XRect(465, y, 105, 16)
            End If

            graph.DrawRectangle(pen, brushTransparent, Rect)
            graph.DrawString(dict(key).ToString(), font9Regular, brushBlack, RValue, y + 11)

            y += 16
        Next
        Return y
    End Function

    Private Function PrintLeftData(dict As Dictionary(Of String, String), y As Integer, Optional IsPackagingDataDisplay As Boolean = False) As Integer
        Dim Rect
        LKey = 45
        LValue = 150
        For Each key In dict.Keys
            If IsPackagingDataDisplay Then
                Rect = New XRect(30, y, 110, 16)
            Else
                Rect = New XRect(30, y, 110, 16)
            End If

            graph.DrawRectangle(pen, brushLightGray, Rect)
            graph.DrawString(key.ToString(), font9Bold, brushBlack, LKey, y + 11)


            If IsPackagingDataDisplay Then
                Rect = New XRect(140, y, 150, 16)
                LValue = 150
            Else
                Rect = New XRect(140, y, 230, 16)
            End If
            graph.DrawRectangle(pen, brushTransparent, Rect)
            graph.DrawString(dict(key).ToString(), font9Regular, brushBlack, LValue, y + 11)

            y += 16

        Next
        Return y
    End Function

    Private Sub GetSalesRepData()
        Dim sqlConnection = New MySqlConnection()
        Try
            sqlConnection.ConnectionString = My.Settings.DBCon
            sqlConnection.Open()

            Dim mySqlDataAdapter = New MySqlDataAdapter("select * from salesreps where ID='" & lblSalesRep.Text & "'", sqlConnection)
            mySqlDataAdapter.Fill(SalesRepData, "SalesRepData")
        Catch ex As Exception
            Helper.WriteLog(ex)
        Finally
            sqlConnection.Close()
        End Try

    End Sub

    Private Sub CalculateSachetsSize()
        Try
            Dim ServingSiz
            Dim ServingPer
            Dim Fill

            If FormulaType.Text <> "Powder" Then
                ServingSiz = If(String.IsNullOrEmpty(ServingSizeTextBox.Text), "0", ServingSizeTextBox.Text)
            Else

            End If

            If chkDisplayBox.Checked Then
                If FormulaType.Text = "Powder" Then

                    'Fill Serving Data
                    ServingPer = If(String.IsNullOrEmpty(txtStickPacksSachets.Text), "0", txtStickPacksSachets.Text)
                    Dim TempServingSize As Double
                    Double.TryParse(ServingSizeTextBox.Text, TempServingSize)
                    ServingSize.Add("Serving Size", TempServingSize.ToString("N2") & " " & ServingSizeUOMLabel.Text)
                    ServingSize.Add("Servings Per Sachets", TempServingSize)
                    ServingSize.Add("Fill Weight", (Convert.ToDecimal(TempServingSize) * Convert.ToDecimal(ServingPer)).ToString("N2") & " gm")

                Else
                    'Fill Serving Data
                    ServingSiz = If(String.IsNullOrEmpty(ServingSizeTextBox.Text), "0", ServingSizeTextBox.Text)
                    ServingPer = If(String.IsNullOrEmpty(TextBox35.Text), "0", TextBox35.Text)
                    Dim SachetsPerIFC = If(String.IsNullOrEmpty(txtStickPacksSachets.Text), "0", txtStickPacksSachets.Text)
                    Fill = Convert.ToInt16(ServingPer) * Convert.ToInt16(SachetsPerIFC)

                    ServingSize.Add("Serving Size", ServingSiz)
                    ServingSize.Add("Servings Per Sachets", ServingPer)
                    If FormulaType.Text = "Capsule" Then
                        ServingSize.Add("Fill Weight", Fill & " Capsule(s)")
                    ElseIf FormulaType.Text = "Tablet" Then
                        ServingSize.Add("Fill Weight", Fill & " Tablet(s)")
                    End If
                End If

            Else
                'Fill Serving Data
                ServingSiz = If(String.IsNullOrEmpty(ServingSizeTextBox.Text), "0", ServingSizeTextBox.Text)

                If FormulaType.Text = "Powder" Then
                    ServingPer = Double.Parse(TextBox35.Text / ServingSiz).ToString("N2")
                Else
                    ServingPer = Convert.ToInt16(TextBox35.Text / ServingSiz)
                End If

                Fill = If(String.IsNullOrEmpty(TextBox35.Text), "0", TextBox35.Text)

                ServingSize.Add("Serving Size", ServingSiz & " " & ServingSizeUOMLabel.Text)
                ServingSize.Add("Servings Per Sachets", ServingPer)

                If FormulaType.Text = "Capsule" Then
                    ServingSize.Add("Fill Weight", TextBox35.Text & " Capsule(s)")
                ElseIf FormulaType.Text = "Tablet" Then
                    ServingSize.Add("Fill Weight", TextBox35.Text & " Tablet(s)")
                Else
                    Dim FillWeight As Double
                    Double.TryParse(TextBox35.Text, FillWeight)
                    ServingSize.Add("Fill Weight", FillWeight.ToString("N2") & " gm")
                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateBlisterSize()
        Try
            Dim ServingSiz
            Dim ServingPer
            Dim Fill

            If FormulaType.Text <> "Powder" Then
                ServingSiz = If(String.IsNullOrEmpty(ServingSizeTextBox.Text), "0", ServingSizeTextBox.Text)
            Else
                Double.TryParse(ServingSizeTextBox.Text, ServingSiz)
            End If

            If chkDisplayBox.Checked Then
                If FormulaType.Text <> "Powder" Then

                    ServingSize.Add("Serving Size", ServingSiz)
                    ServingSize.Add("Servings per Blister", Convert.ToInt16((Convert.ToInt16(If(String.IsNullOrEmpty(txtBlisterCount.Text), "0", txtBlisterCount.Text)) / Convert.ToInt16(ServingSiz))))
                    If FormulaType.Text = "Capsule" Then
                        ServingSize.Add("Fill Weight", (Convert.ToInt16(If(String.IsNullOrEmpty(txtBlisterCount.Text), "0", txtBlisterCount.Text)) * Convert.ToInt16(If(String.IsNullOrEmpty(txtBlisterCountAmount.Text), "0", txtBlisterCountAmount.Text))) & " Capsule(s)")
                    ElseIf FormulaType.Text = "Tablet" Then
                        ServingSize.Add("Fill Weight", (Convert.ToInt16(If(String.IsNullOrEmpty(txtBlisterCount.Text), "0", txtBlisterCount.Text)) * Convert.ToInt16(If(String.IsNullOrEmpty(txtBlisterCountAmount.Text), "0", txtBlisterCountAmount.Text))) & " Tablet(s)")
                    End If

                End If
            Else
                If FormulaType.Text <> "Powder" Then

                    ServingSize.Add("Serving Size", ServingSiz)
                    ServingSize.Add("Servings per Blister", Convert.ToInt16((Convert.ToInt16(If(String.IsNullOrEmpty(txtBlisterCount.Text), "0", txtBlisterCount.Text)) / Convert.ToInt16(ServingSiz))))
                    If FormulaType.Text = "Capsule" Then
                        ServingSize.Add("Fill Weight", If(String.IsNullOrEmpty(txtBlisterCount.Text), "0", txtBlisterCount.Text) & " Caposule(s)")
                    ElseIf FormulaType.Text = "Tablet" Then
                        ServingSize.Add("Fill Weight", If(String.IsNullOrEmpty(txtBlisterCount.Text), "0", txtBlisterCount.Text) & " Tablet(s)")
                    End If

                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub CalculateStickPackSize()
        Try
            Dim ServingSiz As Double
            Dim ServingPer
            Dim Fill

            If FormulaType.Text = "Powder" Then
                'ServingSiz = If(String.IsNullOrEmpty(ServingSizeTextBox.Text), 0.00, ServingSizeTextBox.Text)
                Double.TryParse(ServingSizeTextBox.Text, ServingSiz)
            Else

                ServingSiz = Convert.ToInt32(If(String.IsNullOrEmpty(ServingSizeTextBox.Text), 0, ServingSizeTextBox.Text))
            End If

            If chkDisplayBox.Checked Then
                ServingPer = Convert.ToInt16(If(String.IsNullOrEmpty(TextBox38.Text), "0", TextBox38.Text))
                If FormulaType.Text = "Powder" Then
                    ServingSize.Add("Serving Size", ServingSiz.ToString("N2") & " " & ServingSizeUOMLabel.Text)
                    ServingSize.Add("Servings per Stickpack", ServingPer)
                    ServingSize.Add("Fill Weight", Convert.ToDecimal(ServingSiz * ServingPer).ToString("N2") & " gm")
                Else
                    ServingSize.Add("Serving Size", ServingSiz)
                    ServingSize.Add("Servings per Stickpack", ServingPer)
                    If FormulaType.Text = "Capsule" Then
                        ServingSize.Add("Fill Weight", (ServingSiz * ServingPer) & " Capsule(s)")
                    ElseIf FormulaType.Text = "Tablet" Then
                        ServingSize.Add("Fill Weight", (ServingSiz * ServingPer) & " Tablet(s)")
                    End If
                End If
            ElseIf chkDisplayBag.Checked Then
                ServingPer = Convert.ToInt16(If(String.IsNullOrEmpty(TextBox38.Text), "0", TextBox38.Text))
                If FormulaType.Text = "Powder" Then
                    ServingSize.Add("Serving Size", ServingSiz.ToString("N2") & " " & ServingSizeUOMLabel.Text)
                    ServingSize.Add("Servings per Stickpack", ServingPer)
                    ServingSize.Add("Fill Weight", Convert.ToDecimal(ServingSiz * ServingPer).ToString("N2") & " gm")
                Else
                    ServingSize.Add("Serving Size", ServingSiz)
                    ServingSize.Add("Servings per Container", ServingPer)
                    If FormulaType.Text = "Capsule" Then
                        ServingSize.Add("Fill Weight", (ServingSiz * ServingPer) & " Capsule(s)")
                    ElseIf FormulaType.Text = "Tablet" Then
                        ServingSize.Add("Fill Weight", (ServingSiz * ServingPer) & " Tablet(s)")
                    End If
                End If
            Else

                If FormulaType.Text = "Powder" Then
                    Dim FillWeight As Double
                    Double.TryParse(TextBox48.Text, FillWeight)
                    ServingSize.Add("Serving Size", ServingSiz.ToString("N2") & " " & ServingSizeUOMLabel.Text)
                    If ServingSiz > 0 Then
                        ServingSize.Add("Servings per Stickpack", Convert.ToInt32(FillWeight / ServingSiz))
                    Else
                        ServingSize.Add("Servings per Stickpack", "")
                    End If

                    ServingSize.Add("Fill Weight", TextBox48.Text & " gm")
                Else
                    ServingSize.Add("Serving Size", ServingSiz)
                    ServingSize.Add("Servings per Stickpack", Convert.ToInt16(If(String.IsNullOrEmpty(TextBox48.Text), "0", TextBox48.Text)) / ServingSiz)
                    If FormulaType.Text = "Capsule" Then
                        ServingSize.Add("Fill Weight", If(String.IsNullOrEmpty(TextBox48.Text), "0", TextBox48.Text) & " Capsule(s)")
                    ElseIf FormulaType.Text = "Tablet" Then
                        ServingSize.Add("Fill Weight", If(String.IsNullOrEmpty(TextBox48.Text), "0", TextBox48.Text) & " Tablet(s)")
                    End If

                End If
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub btnSaveterms_Click(sender As Object, e As EventArgs) Handles btnSaveterms.Click
        Try
            Dim DbCon As New MySqlConnection
            Dim dbUp As New MySqlCommand
            Try
                DbCon.ConnectionString = My.Settings.DBCon
                DbCon.Open()
                dbUp.Connection = DbCon
                dbUp.CommandType = CommandType.Text
                dbUp.CommandText = "UPDATE Formulas SET OtherIngredients=@OtherIngredients, LabTestingCostPolicy=@LabTestingCostPolicy ,QuoteExpiryDisclaimer=@QuoteExpiryDisclaimer,Message=@Message WHERE FormulaID = " & FormulaID.Text
                dbUp.Parameters.AddWithValue("@OtherIngredients", rtxtOtherIngredients.Text)
                dbUp.Parameters.AddWithValue("@LabTestingCostPolicy", txtTestingCost.Text)
                dbUp.Parameters.AddWithValue("@QuoteExpiryDisclaimer", txtExpiry.Text)
                dbUp.Parameters.AddWithValue("@Message", RichTextBox1.Text)
                dbUp.ExecuteNonQuery()

                For Each id In UpdatedIndex
                    Dim rows() As DataRow = frm.dsSal.Tables("salestxns").Select("SalesDetailID = " & id.Key & "AND [Type]='BULK'")
                    If rows.Length > 0 Then
                        rows(0)("Overridden") = 1
                        rows(0)("OverriddenSalesPrice") = id.Value
                        'UpdateSalesData(id.Value, id.Key)
                    End If
                Next
                If UpdatedIndex.Count > 0 Then
                    frm.dasal_commands()
                    frm.CalculateBulkSales(frm.gridBulkSales)
                End If
                'dsSal.AcceptChanges()
                UpdatedIndex = New Dictionary(Of Integer, Decimal)

                For Each id In UpdatedBoxIndex
                    Dim rows() As DataRow = frm.dsSalBox.Tables("salesBoxTxns").Select("SalesDetailID = " & id.Key & "AND [Type]='BOX'")
                    If rows.Length > 0 Then
                        rows(0)("Overridden") = 1
                        rows(0)("OverriddenSalesPrice") = id.Value
                        'UpdateSalesData(id.Value, id.Key)
                    End If
                Next
                If UpdatedBoxIndex.Count > 0 Then
                    frm.dasalBox_commands()
                    frm.CalculateBulkSales(frm.gridBoxSales)
                End If
                UpdatedBoxIndex = New Dictionary(Of Integer, Decimal)

                For Each id In UpdatedBagIndex
                    Dim rows() As DataRow = frm.dsSalBags.Tables("salesBagsTxns").Select("SalesDetailID = " & id.Key & "AND [Type]='BULKBAGS'")
                    If rows.Length > 0 Then
                        rows(0)("Overridden") = 1
                        rows(0)("OverriddenSalesPrice") = id.Value
                        'UpdateSalesData(id.Value, id.Key)
                    End If
                Next
                If UpdatedBagIndex.Count > 0 Then
                    frm.dasalBag_commands()
                    frm.CalculateBulkSales(frm.gridbBulkSalesBags)
                End If
                UpdatedBagIndex = New Dictionary(Of Integer, Decimal)

                Threading.Thread.Sleep(1000)
                MessageBox.Show("Data updated successfully...", "Message", MessageBoxButtons.OK)
            Catch ex As Exception
                Dim test As String = "GT"
            Finally
                DbCon.Close()
            End Try

            Formulator2.dsFF.Tables("formulaF").Rows(0)("OtherIngredients") = rtxtOtherIngredients.Text
            Formulator2.dsFF.Tables("formulaF").Rows(0)("LabTestingCostPolicy") = txtTestingCost.Text
            Formulator2.dsFF.Tables("formulaF").Rows(0)("QuoteExpiryDisclaimer") = txtExpiry.Text
            Formulator2.dsFF.Tables("formulaF").Rows(0)("Message") = RichTextBox1.Text
            dsFF.AcceptChanges()

        Catch ex As Exception
            Helper.WriteLog(ex)
            MessageBox.Show("Error occured...", "Message", MessageBoxButtons.OK)
        End Try

    End Sub

    Private Sub PrintPageNumber()
        graph.DrawString("Page: " & PageNumber, font6Bold, brushBlack, 555, Xlength - 10)
    End Sub



    Private Sub txtBox_LostFocus(sender As Object, e As EventArgs)
        FormatTextBox(sender, e)
    End Sub
    Private Sub txtBox_KeyPress(sender As Object, e As KeyPressEventArgs)
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
    Private Sub txtBox_KeyPress_Number(sender As Object, e As KeyPressEventArgs)
        Dim Ctl As Control = CType(sender, Control)
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtBox_TextChanged(sender As Object, e As EventArgs)
        Dim Ctl As Control = CType(sender, Control)

        Dim PrintingCost = Ctl.Text.Split(".")
        If Ctl.Text.Contains(".") Then
            If PrintingCost(1).Length > 1 Then
                Ctl.Text = PrintingCost(0) + "." + PrintingCost(1).Substring(0, 2)
            End If
        End If
    End Sub

    Private Sub gridBulkSales_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles gridBulkSales.EditingControlShowing

        Dim txt As TextBox = CType(e.Control, TextBox)

        RemoveHandler txt.KeyPress, AddressOf txtBox_KeyPress
        RemoveHandler txt.LostFocus, AddressOf txtBox_LostFocus
        RemoveHandler txt.KeyPress, AddressOf txtBox_KeyPress_Number

        If gridBulkSales.CurrentCell.ColumnIndex = 1 Then
            AddHandler txt.KeyPress, AddressOf txtBox_KeyPress
            AddHandler txt.LostFocus, AddressOf txtBox_LostFocus
        ElseIf gridBulkSales.CurrentCell.ColumnIndex = 0 Then
            AddHandler txt.KeyPress, AddressOf txtBox_KeyPress_Number
        End If
    End Sub

    Private Sub gridBoxSales_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles gridBoxSales.EditingControlShowing
        Dim txt As TextBox = CType(e.Control, TextBox)

        RemoveHandler txt.KeyPress, AddressOf txtBox_KeyPress
        RemoveHandler txt.LostFocus, AddressOf txtBox_LostFocus
        RemoveHandler txt.KeyPress, AddressOf txtBox_KeyPress_Number

        If gridBoxSales.CurrentCell.ColumnIndex = 1 Then
            AddHandler txt.KeyPress, AddressOf txtBox_KeyPress
            AddHandler txt.LostFocus, AddressOf txtBox_LostFocus
        ElseIf gridBoxSales.CurrentCell.ColumnIndex = 0 Then
            AddHandler txt.KeyPress, AddressOf txtBox_KeyPress_Number
        End If
    End Sub

    Private Sub gridbBulkSalesBags_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles gridbBulkSalesBags.EditingControlShowing
        Dim txt As TextBox = CType(e.Control, TextBox)

        RemoveHandler txt.KeyPress, AddressOf txtBox_KeyPress
        RemoveHandler txt.LostFocus, AddressOf txtBox_LostFocus
        RemoveHandler txt.KeyPress, AddressOf txtBox_KeyPress_Number

        If gridbBulkSalesBags.CurrentCell.ColumnIndex = 1 Then
            AddHandler txt.KeyPress, AddressOf txtBox_KeyPress
            AddHandler txt.LostFocus, AddressOf txtBox_LostFocus
        ElseIf gridbBulkSalesBags.CurrentCell.ColumnIndex = 0 Then
            AddHandler txt.KeyPress, AddressOf txtBox_KeyPress_Number
        End If
    End Sub


    'Private Sub ChangeUpdatedFlag(ID As String)
    '    Try
    '        Dim DbCon As New MySqlConnection
    '        DbCon.ConnectionString = My.Settings.DBCon
    '        DbCon.Open()

    '        Dim Cmd As New MySqlCommand()
    '        Cmd.CommandText = "Update salesdetails set Updated=1 where SalesDetailID=@SalesDetailID"
    '        Cmd.Parameters.Add("@SalesDetailID", ID)
    '        Cmd.Connection = DbCon
    '        Cmd.ExecuteNonQuery()

    '    Catch ex As Exception
    '        Helper.WriteLog(ex)
    '    Finally
    '        DbCon.Close()
    '    End Try
    'End Sub

    Private Sub gridBulkSales_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles gridBulkSales.CellEndEdit
        Try
            If e.RowIndex <> -1 Then
                Dim SaledID = gridBulkSales.Rows(e.RowIndex).DataBoundItem("SalesDetailID")
                Dim SalesPrice = gridBulkSales.Rows(e.RowIndex).DataBoundItem("OverriddenSalesPrice")
                If UpdatedIndex.ContainsKey(SaledID) Then
                    UpdatedIndex.Remove(SaledID)
                End If
                UpdatedIndex.Add(SaledID, SalesPrice)
                'UpdatedIndex.Add(gridBulkSales.Rows(e.RowIndex).DataBoundItem("SalesDetailID"), gridBulkSales.Rows(e.RowIndex).DataBoundItem("OverriddenSalesPrice"))
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try

    End Sub

    Private Sub gridBoxSales_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles gridBoxSales.CellEndEdit
        Try
            If e.RowIndex <> -1 Then
                Dim SaledID = gridBoxSales.Rows(e.RowIndex).DataBoundItem("SalesDetailID")
                Dim SalesPrice = gridBoxSales.Rows(e.RowIndex).DataBoundItem("OverriddenSalesPrice")
                If UpdatedBoxIndex.ContainsKey(SaledID) Then
                    UpdatedBoxIndex.Remove(SaledID)
                End If
                UpdatedBoxIndex.Add(SaledID, SalesPrice)
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub

    Private Sub gridbBulkSalesBags_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles gridbBulkSalesBags.CellEndEdit
        Try
            If e.RowIndex <> -1 Then
                If e.RowIndex <> -1 Then
                    Dim SaledID = gridbBulkSalesBags.Rows(e.RowIndex).DataBoundItem("SalesDetailID")
                    Dim SalesPrice = gridbBulkSalesBags.Rows(e.RowIndex).DataBoundItem("OverriddenSalesPrice")
                    If UpdatedBagIndex.ContainsKey(SaledID) Then
                        UpdatedBagIndex.Remove(SaledID)
                    End If
                    UpdatedBagIndex.Add(SaledID, SalesPrice)
                End If
                'UpdatedBagIndex.Add(gridbBulkSalesBags.Rows(e.RowIndex).DataBoundItem("SalesDetailID"), gridbBulkSalesBags.Rows(e.RowIndex).DataBoundItem("OverriddenSalesPrice"))
            End If
        Catch ex As Exception
            Helper.WriteLog(ex)
        End Try
    End Sub


    'Private Sub UpdateSalesData(SP As Decimal, ID As Integer)
    '    Dim DbCon As New MySqlConnection
    '    DbCon.ConnectionString = My.Settings.DBCon
    '    DbCon.Open()
    '    Dim cmdCommand As New MySqlCommand()
    '    cmdCommand.Connection = DbCon
    '    cmdCommand.CommandType = CommandType.Text
    '    cmdCommand.CommandText = "UPDATE salesdetails SET OverriddenSalesPrice=@OverriddenSalesPrice, Overridden=@Overridden WHERE SalesDetailID=@SalesDetailID"
    '    cmdCommand.Parameters.AddWithValue("@OverriddenSalesPrice", SP)
    '    cmdCommand.Parameters.AddWithValue("@Overridden", 1)
    '    cmdCommand.Parameters.AddWithValue("@SalesDetailID", ID)
    '    cmdCommand.ExecuteNonQuery()
    '    DbCon.Close()
    'End Sub
End Class
