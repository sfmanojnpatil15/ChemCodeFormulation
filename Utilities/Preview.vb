Imports System.Drawing.Printing

Public Class Preview

    Shared i As Int16 = 0
    Shared SizePIndex As Integer = 0

    ' Y Axis values for Bulk  and Box Headers
    Shared yBulkHeader
    Shared yBoxHeader

    ' X Axis values for Bulk And Box
    Shared xKeys As Int16 = 300
    Shared xValue As Int16 = xKeys + 130
    Shared xUnit As Int16 = xKeys + 160

    ' X Axis values for AdditionalServices
    Shared xAdditionalChargesKeys As Int16 = 800
    Shared xAdditionalChargesValue As Int16 = xAdditionalChargesKeys + 200
    Shared xAdditionalChargesUnit As Int16 = xAdditionalChargesKeys + 150

    Shared Wordbreak = New List(Of String)

    Public Shared Sub PrintPowder(y As Int32, x As Int32, e As PrintPageEventArgs, right As StringFormat, frm As Formulator2)
        e.Graphics.DrawString("Blending", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        e.Graphics.DrawString("₹" & frm.TextBox12.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)

        y += 20
        e.Graphics.DrawString("Wastage %", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        e.Graphics.DrawString(frm.TextBox14.Text & "%    " & "₹" & frm.TextBox13.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)

        y += 20
        e.Graphics.DrawString("Lab Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        e.Graphics.DrawString("₹" & frm.TextBox15.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)

        y += 20
        If frm.ComboBox3.Text = "Bulk" Then
            e.Graphics.DrawString("Flavor Profile(kg)", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
            e.Graphics.DrawString("₹" & frm.txtBlendingFlavourProfileKg.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)
        Else
            e.Graphics.DrawString("Flavor Profile(Serving)", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
            e.Graphics.DrawString("₹" & frm.txtBlendingFlavourProfileServing.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)
        End If

    End Sub

    Public Shared Sub PrintTablet(y As Int32, x As Int32, e As PrintPageEventArgs, right As StringFormat, frm As Formulator2)
        If String.IsNullOrEmpty(frm.ComboBox2.Text) Then
            e.Graphics.DrawString("Empty", New Font("Arial", 9, FontStyle.Italic), Brushes.Black, x, y)
        Else
            e.Graphics.DrawString(frm.ComboBox2.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        End If

        e.Graphics.DrawString("₹" & frm.TextBox2.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)

        y += 20
        e.Graphics.DrawString("Compression", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        e.Graphics.DrawString("₹" & frm.TextBox3.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)

        y += 20
        e.Graphics.DrawString("Wastage %", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        e.Graphics.DrawString(frm.TextBox5.Text & "%    " & "₹" & frm.TextBox4.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)

        y += 20
        e.Graphics.DrawString("Lab Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        e.Graphics.DrawString("₹" & frm.TextBox6.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)
    End Sub

    Public Shared Sub PrintCapsule(y As Int32, x As Int32, e As PrintPageEventArgs, right As StringFormat, frm As Formulator2)
        If String.IsNullOrEmpty(frm.ComboBox5.Text) Then
            e.Graphics.DrawString("Empty", New Font("Arial", 9, FontStyle.Italic), Brushes.Black, x, y)
        Else
            e.Graphics.DrawString(frm.ComboBox5.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        End If

        e.Graphics.DrawString("₹" & frm.TextBox1.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)

        y += 20
        e.Graphics.DrawString("Encapsulation", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        e.Graphics.DrawString("₹" & frm.TextBox7.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)

        y += 20
        e.Graphics.DrawString("Wastage %", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        e.Graphics.DrawString(frm.TextBox10.Text & "%    " & "₹" & frm.TextBox11.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)

        y += 20
        e.Graphics.DrawString("Lab Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y)
        e.Graphics.DrawString("₹" & frm.txtlabcost.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, x + 200, y, right)
    End Sub

    Public Shared Sub PrintStickPacks(y As Int32, e As PrintPageEventArgs, frm As Formulator2)

        Dim NewLine = y
        Dim XLable = 300
        Dim XValues = 300 + 150
        Dim TotalLine = 0

        e.Graphics.DrawString("Format", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString(frm.ComboBox3.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
        NewLine += 20

        e.Graphics.DrawString("Size", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString(frm.ComboBox6.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
        NewLine += 20

        e.Graphics.DrawString("Colors", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString(frm.cmbstickpanelColors.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
        NewLine += 20

        e.Graphics.DrawString("Material", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        Wordbreak = Helper.Wordwrap(frm.ComboBox7, 55)

        For Each word In Wordbreak
            If String.IsNullOrEmpty(word) Then
                Continue For
            End If
            i += 1
            e.Graphics.DrawString(word, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            If i = Wordbreak.Count Then
                Continue For
            End If
            NewLine += 20
        Next
        i = 0
        'e.Graphics.DrawString(ComboBox7.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
        NewLine += 20
        e.Graphics.DrawString("Quantity", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString(frm.ComboBox8.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
        NewLine += 20
        e.Graphics.DrawString("Packet Contents", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString(frm.TextBox16.Text & " " & frm.Label51.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Filling Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString("₹ " & frm.TextBox18.Text & "  per/1000", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
        NewLine += 20
        e.Graphics.DrawString("Printing Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString("₹ " & frm.TextBox19.Text & "  per/1000", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

        TotalLine = NewLine

        XLable = 800
        XValues = 800 + 165
        NewLine = y
        e.Graphics.DrawString(frm.lblASStickPacks.Text.ToUpper(), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine - 30)


        e.Graphics.DrawString("Printing Plates", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString("₹ " & frm.txtStickPrintingPlateBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
        NewLine += 20
        e.Graphics.DrawString("Art Preparation", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString("₹ " & frm.txtStickArtPrepBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

        If Not String.IsNullOrEmpty(frm.txtStickOtherDescBulk.Text) Then
            NewLine += 20
            e.Graphics.DrawString(frm.txtStickOtherDescBulk.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtStickOtherCostBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
        End If

        NewLine += 20
        e.Graphics.DrawString("Research & Development", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString("₹ " & frm.txtStickpackBulkRD.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Shipper/Case Count ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
        e.Graphics.DrawString(frm.txtStickShipperCountBulk.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XValues, NewLine)
        e.Graphics.DrawString("₹ " & frm.txtStickShipperCostBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues + 50, NewLine)

        Dim BoxHeader = TotalLine + 60

        XLable = 300
        XValues = 300 + 150

        If frm.chkDisplayBox.Checked Then

            e.Graphics.DrawLine(New Pen(Brushes.DarkGray, 1), 300, TotalLine + 40, 1050, TotalLine + 40)

            e.Graphics.DrawString("Display Box / IFC Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, BoxHeader)
            NewLine = BoxHeader + 40
            e.Graphics.DrawString("Qty", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.TextBox39DisplayQunatity.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            e.Graphics.DrawString("₹ " & frm.TextBox20.Text & "  per/100", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues + 100, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Description", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.TextBox38DisplayDesc.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Spec#", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.TextBox43DisplaySpec.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Board Grade", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.TextBox42DisplayBoardGrade.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Dimension", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.TextBox41DisplayDimension.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Product Style", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.TextBox40DisplayProductStyle.Text & " " & frm.Label51.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Stick Packs", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtStickPacks.Text & "  per IFC", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            TotalLine = NewLine

            XLable = 800
            XValues = 800 + 165

            e.Graphics.DrawString(frm.lblASDSStickPack.Text.ToUpper(), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, BoxHeader)
            NewLine = BoxHeader + 40

            e.Graphics.DrawString("Printing Plates", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.TextBox21.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Art Preparation", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.TextBox22.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Secondary/Packout", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.TextBox23.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Shrink Wrap", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtShrinkWrapStickPack.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Wafer Seal", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtWaferSeal.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            If Not String.IsNullOrEmpty(frm.TextBox25.Text) Then
                NewLine += 20
                e.Graphics.DrawString(frm.TextBox25.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
                e.Graphics.DrawString("₹ " & frm.TextBox24.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            End If

            NewLine += 20
            e.Graphics.DrawString("Research & Development", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtStickpackBoxRD.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Shipper/Case Count ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.txtStickPacksShipperCaseCount.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XValues, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtStickPacksShipperCaseCountAmt.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues + 50, NewLine)

        End If

        If frm.chkDisplayBag.Checked Then


            e.Graphics.DrawLine(New Pen(Brushes.DarkGray, 1), 300, TotalLine + 40, 1050, TotalLine + 40)

            e.Graphics.DrawString("Standup Bag", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, BoxHeader)
            NewLine = BoxHeader + 40

            e.Graphics.DrawString("Description", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.txtStickPackBagDescription.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20

            e.Graphics.DrawString("Qty", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.txtStickPackBagQty.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Material", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            Wordbreak = Helper.Wordwrap(frm.txtStickPackBagMaterial, 55)

            For Each word In Wordbreak
                i += 1
                e.Graphics.DrawString(word, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

                If i = Wordbreak.Count Then
                    Continue For
                End If
                NewLine += 20
            Next
            i = 0
            'e.Graphics.DrawString(txtStickPackBagMaterial.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Size", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.txtStickPackBagSize.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Print Colors", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.txtStickPackBagPrintColors.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Tear Notch", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.cmbStickPackBagTearNotch.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Zipper", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.cmbStickPackBagZipper.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            e.Graphics.DrawString("Serving Weight", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XValues + 150, NewLine)
            e.Graphics.DrawString(frm.txtStickPackBagServingWeight.Text & "  gram", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues + 250, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Hanger Hole", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.cmbStickPackBagHangerHole.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            e.Graphics.DrawString("Servings", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XValues + 150, NewLine)
            e.Graphics.DrawString(frm.txtStickPackBagServings.Text & "", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues + 250, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Printing Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtStickPackBagPrintingCost.Text & "  per/1000", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            e.Graphics.DrawString("Fill Weight", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XValues + 150, NewLine)
            e.Graphics.DrawString(frm.txtStickPackBagFillWeight.Text & "  grams", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues + 250, NewLine)


            NewLine += 20
            e.Graphics.DrawString("Stick Packs", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtStickPacks.Text & "  per IFC", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            TotalLine = NewLine

            XLable = 800
            XValues = 800 + 165

            e.Graphics.DrawString(frm.lblASDSStandUpBags.Text.ToUpper(), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, BoxHeader)
            NewLine = BoxHeader + 40
            'e.Graphics.DrawLine(New Pen(Brushes.DarkGray, 1), 800, y - 125, 800, y + 200)
            e.Graphics.DrawString("Printing Plates", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtPrintingPlatesStickBag.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Art Preparation", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtArtPreStickBag.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Secondary/Packout", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtSecondaryPackStickBag.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            If Not String.IsNullOrEmpty(frm.txtOtherStickBag.Text) Then
                NewLine += 20
                e.Graphics.DrawString(frm.txtOtherStickBag.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
                e.Graphics.DrawString("₹ " & frm.txtOtherCostStickBag.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            End If

            NewLine += 20
            e.Graphics.DrawString("Research & Development", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtStickpackBagRD.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Shipper/Case Count ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, NewLine)
            e.Graphics.DrawString(frm.txtShipperCountStickBag.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XValues, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtShipperCostStickBag.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues + 50, NewLine)

        End If
        TotalLine = TotalLine + 50
        XLable = 300
        XValues = 300 + 150
        'NewLine += 35
        e.Graphics.DrawString("Pre-Pakaging Material Cost ₹ " + frm.lblPreAmtStickPack.Text + " Pre-Pakaging Material Cost ₹ " + frm.lblAmtStickPack.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XLable, TotalLine - 20)

        e.Graphics.DrawString("Total Cost/Unit ₹", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XLable, TotalLine)
        e.Graphics.DrawString(frm.TotalUnitCost.Text & " ea.", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, XValues, TotalLine)
    End Sub

    Public Shared Sub PrintSachets(y As Int16, e As PrintPageEventArgs, frm As Formulator2)

        yBulkHeader = y
        Dim NewLine
        xKeys = 300
        xValue = xKeys + 130
        e.Graphics.DrawString("Format:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, yBulkHeader)
        e.Graphics.DrawString(frm.ComboBox3.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, yBulkHeader)

        NewLine = yBulkHeader + 20

        e.Graphics.DrawString("Size:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString(frm.ComboBox11.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Colors:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString(frm.ComboBox12.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Material:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString(frm.ComboBox10.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Quantity:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString(frm.ComboBox9.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        'NewLine += 20
        'e.Graphics.DrawString("Cost Rs: " & textbox34.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Filling Cost:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString("₹ " + frm.TextBox33.Text + " per/1000", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Printing Cost:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString("₹ " + frm.TextBox28.Text + " per/1000", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        Dim TotalLine = NewLine

        'NewLine += 20
        'e.Graphics.DrawString("Display:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        'e.Graphics.DrawString(TextBox27.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        yBoxHeader = NewLine + 80

        ' Additional Charges BULK
        e.Graphics.DrawString(frm.lblASSachets.Text.ToUpper(), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, yBulkHeader - 30)


        e.Graphics.DrawString("Printing Plates:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, yBulkHeader)
        e.Graphics.DrawString("₹ " + frm.txtSachetPrintingPlatesBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, yBulkHeader)
        NewLine = yBulkHeader + 20

        e.Graphics.DrawString("Art Preparation:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
        e.Graphics.DrawString("₹ " + frm.txtSachetArtPrepBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)

        If Not String.IsNullOrEmpty(frm.txtSachetOtherCostDesc.Text) Then
            NewLine += 20
            e.Graphics.DrawString(frm.txtSachetOtherCostDesc.Text + ":", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹ " + frm.txtSachetOtherCost.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
        End If

        NewLine += 20
        e.Graphics.DrawString("Research & Development", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
        e.Graphics.DrawString("₹ " + frm.txtSachetsBulkRD.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)


        NewLine += 20
        e.Graphics.DrawString("Shipper/Case Count:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
        e.Graphics.DrawString(frm.txtSachetShipperCountBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesUnit, NewLine)
        e.Graphics.DrawString("₹ " + frm.txtSachetShipperCost.Text & " ea.", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)

        If frm.chkDisplayBox.Checked Then

            e.Graphics.DrawLine(New Pen(Brushes.DarkGray, 1), 300, TotalLine + 40, 1050, TotalLine + 40)


            e.Graphics.DrawString("Display Box / IFC Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, yBoxHeader)

            NewLine = yBoxHeader + 40
            e.Graphics.DrawString("Qty:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.TextBox38DisplayQty.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            e.Graphics.DrawString("₹ " + frm.TextBox27.Text + " per/1000", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue + 30, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Description:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.TextBox39Desc.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Spec#:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.TextBox44Spec.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Board Grade:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.TextBox43BoardGrade.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Dimension:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.TextBox42Dimension.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Product Style:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.TextBox41ProductStyle.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Sachet Contents:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.TextBox35.Text & " " & frm.Label98.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Sachets:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString("₹" & frm.txtStickPacksSachets.Text & " " & frm.Label73.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            e.Graphics.DrawString(frm.lblASDSSachets.Text.ToUpper(), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, yBoxHeader)

            NewLine = yBoxHeader + 40
            e.Graphics.DrawString("Printing Plates:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹ " + frm.TextBox32.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Art Preparation:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹ " + frm.TextBox31.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("ShrinkWrap:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹ " + frm.TextBox30.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Secondary/Packout:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹ " + frm.txtShrinkWrapSachets.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Wafer Seal:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹ " + frm.txtWaferSealSachets.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)

            If Not String.IsNullOrEmpty(frm.TextBox26.Text) Then
                NewLine += 20
                e.Graphics.DrawString(frm.TextBox26.Text + ":", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
                e.Graphics.DrawString("₹ " + frm.TextBox29.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            End If

            NewLine += 20

            e.Graphics.DrawString("Research & Development" + ":", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹ " + frm.txtSachetsBoxRD.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)


            NewLine += 20
            e.Graphics.DrawString("Shipper/Case Count:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString(frm.txtSachetsShipperCaseCount.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesUnit, NewLine)
            e.Graphics.DrawString("₹ " + frm.txtSachetsShipperCaseCountAmt.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            TotalLine = NewLine
        End If

        If frm.chkDisplayBag.Checked Then

            e.Graphics.DrawLine(New Pen(Brushes.DarkGray, 1), 300, TotalLine + 40, 1050, TotalLine + 40)

            e.Graphics.DrawString("Standup Bag", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, yBoxHeader)
            NewLine = yBoxHeader + 40

            e.Graphics.DrawString("Description", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtSachetsBagDescription.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            NewLine += 20

            e.Graphics.DrawString("Qty", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtSachetsBagQty.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Material", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            Wordbreak = Helper.Wordwrap(frm.txtSachetsBagMaterial, 55)

            For Each word In Wordbreak
                i += 1
                e.Graphics.DrawString(word, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

                If i = Wordbreak.Count Then
                    Continue For
                End If
                NewLine += 20
            Next
            i = 0
            'e.Graphics.DrawString(txtStickPackBagMaterial.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, XValues, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Size", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtSachetsBagSize.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Print Colors", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtSachetsBagPrintColors.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Tear Notch", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.cmbSachetsBagTearNotch.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Zipper", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.cmbSachetsBagZipper.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            e.Graphics.DrawString("Serving Weight", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xValue + 150, NewLine)
            e.Graphics.DrawString(frm.txtSachetsBagServingWeight.Text & "  gram", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue + 250, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Hanger Hole", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.cmbSachetsBagHangerHole.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            e.Graphics.DrawString("Servings", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xValue + 150, NewLine)
            e.Graphics.DrawString(frm.txtSachetsBagServings.Text & "", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue + 250, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Printing Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtSachetsBagPrintingCost.Text & "  per/1000", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            e.Graphics.DrawString("Fill Weight", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xValue + 150, NewLine)
            e.Graphics.DrawString(frm.txtSachetsBagFillWeight.Text & "  grams", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue + 250, NewLine)


            NewLine += 20
            e.Graphics.DrawString("Sachets", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtStickPacksSachets.Text & "  per IFC", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            TotalLine = NewLine

            xKeys = 800
            xValue = 800 + 165

            e.Graphics.DrawString(frm.lblASDSStandUpBags.Text.ToUpper(), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, yBoxHeader)
            NewLine = yBoxHeader + 40
            'e.Graphics.DrawLine(New Pen(Brushes.DarkGray, 1), 800, y - 125, 800, y + 200)
            e.Graphics.DrawString("Printing Plates", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtSachetsBagPrintingPlates.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Art Preparation", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtSachetsBagArtPreparation.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Secondary/Packout", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtSachetsBagSecondaryPackout.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            If Not String.IsNullOrEmpty(frm.txtSachetsBagOtherDescription.Text) Then
                NewLine += 20
                e.Graphics.DrawString(frm.txtSachetsBagOtherDescription.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
                e.Graphics.DrawString("₹ " & frm.txtSachetsBagOther.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            End If

            NewLine += 20
            e.Graphics.DrawString("Research & Development", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtSachetsBagRD.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Shipper/Case Count ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtSachetsBagShipperCaseCount.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xValue, NewLine)
            e.Graphics.DrawString("₹ " & frm.txtSachetsBagShipperCaseCost.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue + 50, NewLine)

        End If

        TotalLine += 65
        xKeys = 300
        e.Graphics.DrawString("Pre-Pakaging Material Cost ₹ " + frm.lblPreAmtSachets.Text + " Pre-Pakaging Material Cost ₹ " + frm.lblAmtSachets.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xKeys, TotalLine - 20)
        e.Graphics.DrawString("Total Cost/Unit ₹:", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, TotalLine)
        e.Graphics.DrawString(frm.TotalUnitCost.Text & " ea.", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xValue, TotalLine)

    End Sub

    Public Shared Sub PrintBottle(y As Int16, e As PrintPageEventArgs, frm As Formulator2, dsPB As DataSet, Optional hasMultipleCT As Boolean = False)
        With frm.ComboBox4
            ' Do While Me.SizePIndex < .Items.Count

            Dim xSize As Integer = 300

            Dim ySize As Integer = y

            Dim stc As Double = frm.TextBox8.Text
            Dim PreCo As Double = (stc / 1000) * .Items(SizePIndex)
            PreCo = Math.Round(PreCo, 2)

            e.Graphics.DrawString(.Items(SizePIndex).ToString & " CT", New Font("Arial", 8, FontStyle.Bold), Brushes.Black, xSize, ySize)
            ySize += 21
            e.Graphics.DrawString("Pre-bottling material cost:  ₹" & frm.Label26.Text, New Font("Arial", 8, FontStyle.Italic), Brushes.Black, xSize, ySize)
            ySize += 15
            e.Graphics.DrawString("Servings per Container: " & .Items(SizePIndex) / frm.ServingSizeTextBox.Text, New Font("Arial", 8, FontStyle.Italic), Brushes.Black, xSize, ySize)
            ySize += 30
            e.Graphics.DrawString("Category ", New Font("Arial", 8, FontStyle.Bold), Brushes.Black, xSize, ySize)
            e.Graphics.DrawString("Material Name", New Font("Arial", 8, FontStyle.Bold), Brushes.Black, xSize + 150, ySize)
            e.Graphics.DrawString("Unit Cost", New Font("Arial", 8, FontStyle.Bold), Brushes.Black, xSize + 360, ySize)
            ySize += 21

            Dim custDV As DataView = New DataView(dsPB.Tables("bottles"), "SizeCount = " & .Items(SizePIndex) & " AND VersionNumber = " & If(String.IsNullOrEmpty(frm.VersionCmbBox.Text), "0", frm.VersionCmbBox.Text) & "", "BindingIndex ASC", DataViewRowState.CurrentRows)

            For Each r As DataRowView In custDV
                If Not IsDBNull(r.Item(4)) Then
                    e.Graphics.DrawString(r.Item(4).ToString, New Font("Arial", 8, FontStyle.Regular), Brushes.Black, xSize, ySize)
                    e.Graphics.DrawString(r.Item(5).ToString, New Font("Arial", 8, FontStyle.Regular), Brushes.Black, xSize + 70, ySize)
                    e.Graphics.DrawString(r.Item(7).ToString, New Font("Arial", 8, FontStyle.Regular), Brushes.Black, xSize + 360, ySize)
                    If Not IsDBNull(r.Item(7)) Then
                        PreCo += r.Item(7)
                    End If
                End If
                ySize += 21
            Next
            ySize += 5
            PreCo = Math.Round(PreCo, 2)
            e.Graphics.DrawString("Total Cost/Unit:  ₹" & frm.TotalUnitCost.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xSize, ySize)
            e.Graphics.DrawString("Total unit cost:  ₹" & frm.lblBottleUnitCostSumAMT.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xSize + 150, ySize)


            'If SizeNo = SizesPerPage Then
            '    SizeNo = 1
            '    e.HasMorePages = True
            'End If
            '   Loop
        End With
    End Sub

    Public Shared Sub PrintBag(y As Int16, e As PrintPageEventArgs, frm As Formulator2)

        Dim Temp = y
        e.Graphics.DrawString("Description", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, y)
        e.Graphics.DrawString(frm.txtDexcription.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)

        y += 20
        e.Graphics.DrawString("Qty", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, y)
        e.Graphics.DrawString(frm.txtQty.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)

        y += 20
        e.Graphics.DrawString("Material", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, y)
        Wordbreak = Helper.Wordwrap(frm.txtMaterial, 55)

        For Each word In Wordbreak
            i += 1
            e.Graphics.DrawString(word, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)

            If i = Wordbreak.Count Then
                Continue For
            End If
            y += 20
        Next
        i = 0

        'e.Graphics.DrawString(txtMaterial.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)

        y += 20
        e.Graphics.DrawString("Size", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, y)
        e.Graphics.DrawString(frm.txtSize.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)


        y += 20
        e.Graphics.DrawString("Print Colors", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, y)
        e.Graphics.DrawString(frm.txtPrintColors.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)

        y += 20
        e.Graphics.DrawString("Tear Notch", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, y)
        e.Graphics.DrawString(frm.cmbTearNotch.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)
        e.Graphics.DrawString("Serving Weight", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 550, y)
        e.Graphics.DrawString(frm.txtServingWeight.Text + "  " + frm.lblServingWeightunits.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 550 + 106, y)
        y += 20
        e.Graphics.DrawString("Zipper", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, y)
        e.Graphics.DrawString(frm.cmbZipper.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)
        e.Graphics.DrawString("Servings", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 550, y)
        e.Graphics.DrawString(frm.txtServing.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 550 + 106, y)
        y += 20
        e.Graphics.DrawString("Hanger Hole", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, y)
        e.Graphics.DrawString(frm.cmbHangerHole.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)
        e.Graphics.DrawString("Fill Weight", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 550, y)
        e.Graphics.DrawString(frm.txtFillWeight.Text + "  " + frm.lblFillWeightUnits.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 550 + 106, y)
        y += 20
        e.Graphics.DrawString("Printing Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, y)
        e.Graphics.DrawString("₹ " + frm.txtPrintingCost.Text + " per/1000", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, 300 + 100, y)

        Dim Totalline = y

        y = Temp
        e.Graphics.DrawString(frm.lblASBAG.Text.ToUpper(), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, y - 30)


        e.Graphics.DrawString("Printing Plates", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, y)
        e.Graphics.DrawString("₹" + frm.txtPrintingPlates.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, y)
        y += 20
        e.Graphics.DrawString("Art Preperation", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, y)
        e.Graphics.DrawString("₹" + frm.txtArtPreperation.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, y)
        y += 20
        e.Graphics.DrawString("Filling Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, y)
        e.Graphics.DrawString("₹" + frm.txtSecondaryPackoutBag.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, y)

        If Not (String.IsNullOrEmpty(frm.txtOtherDesc.Text)) Then
            y += 20
            e.Graphics.DrawString(frm.txtOtherDesc.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, y)
            e.Graphics.DrawString("₹" + frm.txtOther.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, y)
        End If

        y += 20
        e.Graphics.DrawString("Research & Development", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, y)
        e.Graphics.DrawString("₹" + frm.txtStandupBagRD.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, y)

        y += 20
        e.Graphics.DrawString("Shipper/Case Count ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, y)
        e.Graphics.DrawString(frm.txtShipperCaseCount.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesUnit, y)
        e.Graphics.DrawString("₹" + frm.txtShipperCaseAmt.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, y)

        Totalline += 50

        e.Graphics.DrawString("Pre-Pakaging Material Cost ₹ " + frm.lblPreAmtStandupBag.Text + " Pre-Pakaging Material Cost ₹ " + frm.lblAmtStandupBag.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xKeys, Totalline - 20)
        e.Graphics.DrawString("Total Cost/Unit ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300, Totalline)
        e.Graphics.DrawString("₹ " & frm.TotalUnitCost.Text & " ea.", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, 300 + 100, Totalline)
    End Sub

    Public Shared Sub PrintBlister(y As Int16, e As PrintPageEventArgs, frm As Formulator2)

        Dim NewLine
        'NewLine += 20
        yBulkHeader = y
        NewLine = yBulkHeader

        e.Graphics.DrawString("Format", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, yBulkHeader)
        e.Graphics.DrawString(frm.ComboBox3.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
        NewLine += 20
        e.Graphics.DrawString("Blister Format", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString(frm.txtBlisterFormat.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
        NewLine += 20

        e.Graphics.DrawString("Blister Size", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString(frm.txtBlisterSize.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Material", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)

        Wordbreak = Helper.Wordwrap(frm.txtboxBlisterMaterial, 55)

        For Each word In Wordbreak
            i += 1
            e.Graphics.DrawString(word, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            If i = Wordbreak.Count Then
                Continue For
            End If
            NewLine += 20
        Next
        i = 0

        'e.Graphics.DrawString(txtboxBlisterMaterial.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Blister Count", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString(frm.txtBlisterCount.Text & " " & frm.cmbBlisterCountType.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Blister Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString("₹" + frm.txtBlisterCost.Text + " " + frm.Label112Blister.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Printing Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
        e.Graphics.DrawString("₹" + frm.txtBlisterPrintingCostAmt.Text + " " + frm.Label112Blister.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

        Dim TotalLine = NewLine
        yBoxHeader = NewLine + 80

        'Additional Services Bulk

        e.Graphics.DrawString(frm.lblASBlister.Text.ToUpper(), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, yBulkHeader - 30)

        e.Graphics.DrawString("Printing Plates", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, yBulkHeader)
        e.Graphics.DrawString("₹" + frm.txtPrintingBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, yBulkHeader)

        NewLine = yBulkHeader + 20

        e.Graphics.DrawString("Art Preperation", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
        e.Graphics.DrawString("₹" + frm.txtArtPrepBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
        NewLine += 20
        e.Graphics.DrawString("Tooling Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
        e.Graphics.DrawString("₹" + frm.txtToolingCostBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
        If Not String.IsNullOrEmpty(frm.txtOtherCostBulk.Text) Then
            NewLine += 20
            e.Graphics.DrawString(frm.txtOtherBulk.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹" + frm.txtOtherCostBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
        End If

        NewLine += 20
        e.Graphics.DrawString("Research & Development", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
        e.Graphics.DrawString("₹" + frm.txtBlisterBulkRD.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)

        NewLine += 20
        e.Graphics.DrawString("Shipper/Case Count ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
        e.Graphics.DrawString(frm.txtShipperCountBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesUnit, NewLine)
        e.Graphics.DrawString("₹" + frm.txtShipperCostBulk.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)

        If frm.chkDisplayBox.Checked Then

            e.Graphics.DrawLine(New Pen(Brushes.DarkGray, 1), 300, TotalLine + 40, 1050, TotalLine + 40)

            e.Graphics.DrawString("BLISTER BOX", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, yBoxHeader)
            NewLine = yBoxHeader + 40

            e.Graphics.DrawString("Qty", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtQtyDiplaybox.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            e.Graphics.DrawString("₹ " + frm.txtAmontDisplaybox.Text + " per/1000", New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue + 70, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Folding Carton/Box Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtBlisterBoxCostAmt.Text + " " + frm.Label112Blister.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue + 40, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Description", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtDescDiplaybox.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Spec#", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtSepcDisplaybox.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Board Grade", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtBoradGradeDiplaybox.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Dimension", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtDimensionDisplaybox.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Product Style", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtProductStyle.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Blister per Box", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, NewLine)
            e.Graphics.DrawString(frm.txtBlisterCountAmount.Text + " " + frm.Label112Blister.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xValue, NewLine)
            NewLine += 20

            NewLine = y - 80

            'Additional Services Display Box

            e.Graphics.DrawString(frm.lblASDSBlister.Text.ToUpper(), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, yBoxHeader)
            NewLine = yBoxHeader + 40
            e.Graphics.DrawString("Printing Plates", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹" + frm.txtBlisterPrintingPlatesAmt.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Art Preperation", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹" + frm.txtBlisterArtPreperationAmt.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            NewLine += 20

            e.Graphics.DrawString("Secondary/ Packout", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹" + frm.txtBlisterPackoutAmt.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            NewLine += 20

            e.Graphics.DrawString("Shrink Wrap ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString(frm.cmbBlisterShrinkWrap.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesUnit, NewLine)
            e.Graphics.DrawString("₹" + frm.txtBlisterShrinkWrap.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            NewLine += 20

            e.Graphics.DrawString("Wafer Seal(s) ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString(frm.cmbBlisterWaferSeal.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesUnit, NewLine)
            e.Graphics.DrawString("₹" + frm.txtBlisterWaferSeal.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)

            If Not String.IsNullOrEmpty(frm.txtBlisterOtherAmt.Text) Then
                NewLine += 20
                e.Graphics.DrawString(frm.txtOtherDescBlister.Text, New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
                e.Graphics.DrawString("₹" + frm.txtBlisterOtherAmt.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            End If
            NewLine += 20
            e.Graphics.DrawString("Research & Development", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹" + frm.txtBlisterBoxRD.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)

            NewLine += 20
            e.Graphics.DrawString("Tooling Cost", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString("₹" + frm.txttoolcostamtblister.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)
            NewLine += 20
            e.Graphics.DrawString("Shipper/Case Count ", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xAdditionalChargesKeys, NewLine)
            e.Graphics.DrawString(frm.txtBlisterCaseCount.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesUnit, NewLine)
            e.Graphics.DrawString("₹" + frm.txtBlisterCaseCountAmt.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xAdditionalChargesValue, NewLine)

            TotalLine = NewLine
        End If


        TotalLine += 50

        e.Graphics.DrawString("Pre-Pakaging Material Cost ₹ " + frm.lblPreAmtBlister.Text + " Pre-Pakaging Material Cost ₹ " + frm.lblAmtBlister.Text, New Font("Arial", 9, FontStyle.Regular), Brushes.Black, xKeys, TotalLine - 20)
        e.Graphics.DrawString("Total Cost/Unit₹", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xKeys, TotalLine)
        e.Graphics.DrawString(frm.TotalUnitCost.Text & " ea.", New Font("Arial", 9, FontStyle.Bold), Brushes.Black, xValue, TotalLine)
    End Sub


End Class
