Imports System.IO
Imports QRCoder
Imports AForge
Imports AForge.Video
Imports AForge.Video.DirectShow
Imports ZXing
Imports ZXing.Aztec
Imports MessagingToolkit.QRCode.Codec
Imports MessagingToolkit.QRCode.Codec.Data
Imports Webcamp_Captutue






Public Class Form1

    Dim CaptureDevice As FilterInfoCollection
    Dim FinalFrame As VideoCaptureDevice
    Dim bmp As Bitmap

    Dim emptyImage As Image

    Dim openTestcheck As Boolean = False

    'Dim readerQR As QRCodeDecoder

    'WithEvents myWebCam As WebcamCapturer





    Dim formContent As String = ""
    'Dim vbTab As String = vbTab
    'Dim vbNewLine As String = vbNewLine
    Dim sectionNumber As Integer = 0

    Dim inputGender As String = ""
    Dim isFever_Str As String = ""
    Dim isDryCough_Str As String = ""
    Dim isSoreThroat_Str As String = ""
    Dim isTirediness_Str As String = ""


    Dim fileName As String = "App_records.txt"
    Dim currentSectionNumFile As String = "currentSectionNumber.txt"
    Dim cntrFromFile As String = ""
    Dim glblCounter = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dateDTP.MinDate = DateTime.Now
        dateDTP.MaxDate = DateTime.Now

        CaptureDevice = New FilterInfoCollection(FilterCategory.VideoInputDevice)
        For Each Device As FilterInfo In CaptureDevice
            ComboBox_WebCamraDevices.Items.Add(Device.Name)

        Next


        ComboBox_WebCamraDevices.SelectedIndex = 0
        FinalFrame = New VideoCaptureDevice()

        disableAndNotVisible()

    End Sub

    Private Sub disableAndNotVisible()
        TextBoxQrCode.Visible = False
        TextBoxQrCode.Enabled = False

        PictureBoxQRScanner.Enabled = False
        ComboBox_WebCamraDevices.Enabled = False
        Button_StartScan.Enabled = False
        Button_ReadQRCode.Enabled = False

    End Sub



    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click


        If ((File.Exists(currentSectionNumFile)) And (Not File.Exists(fileName))) Then
            File.Delete(currentSectionNumFile)
        End If

        If (isFormCompled()) Then
            If (File.Exists(currentSectionNumFile)) Then
                Dim sectionFilecontent As StreamReader = File.OpenText(currentSectionNumFile)
                cntrFromFile = sectionFilecontent.ReadLine()

                sectionFilecontent.Close()
                sectionNumber = Integer.Parse(cntrFromFile)
                sectionNumber += 1
            Else
                sectionNumber = 1
                Dim sectionFileconten As StreamWriter = File.CreateText(currentSectionNumFile)
                sectionFileconten.Write(sectionNumber.ToString())
                sectionFileconten.Close()
            End If

            inputGender = If(rdBtnMale_Gender.Checked, "Male", If(rdBtnFemale_Gender.Checked, "Female", ""))
            isFever_Str = If(rdBtnYes_Fever.Checked, "Yes", If(rdBtnNo_Fever.Checked, "No", ""))
            isDryCough_Str = If(rdBtnYes_DryCough.Checked, "Yes", If(rdBtnNo_DryCough.Checked, "No", ""))
            isSoreThroat_Str = If(rdBtnYes_SoreThroat.Checked, "Yes", If(rdBtnNo_SoreThroat.Checked, "No", ""))
            isTirediness_Str = If(rdBtnYes_Tirediness.Checked, "Yes", If(rdBtnNo_Tiredines.Checked, "No", ""))

            formContent = "Section " + sectionNumber.ToString() + ": " + vbNewLine _
                            + "Name: " + vbTab + txtBxFirstName.Text + vbTab + txtBxMiddleName.Text + vbTab + txtBxLastName.Text + vbNewLine _
                            + "Age: " + vbTab + txtBxAge.Text + vbNewLine _
                            + "Contact Number: " + vbTab + txtBxContactNum.Text + vbNewLine _
                            + "E-Mail: " + vbTab + txtBxEMail.Text + vbNewLine _
                            + "Gender: " + vbTab + inputGender + vbNewLine _
                            + "Date: " + vbTab + dateDTP.Text + vbNewLine _
                            + "Barangay: " + vbTab + txtBxBarangay.Text + vbNewLine + vbNewLine _
                            + "*Health Condition" + vbTab + vbNewLine _
                            + "If Has Fever: " + vbTab + isFever_Str + vbNewLine _
                            + "If Has Dry Cough " + vbTab + isDryCough_Str + vbNewLine _
                            + "If Has Sore Throat: " + vbTab + isSoreThroat_Str + vbNewLine _
                            + "If Has Tiredines: " + vbTab + isTirediness_Str + vbNewLine + vbNewLine _
                            + "END_OF_SECTION_" + sectionNumber.ToString() + vbNewLine + vbNewLine + vbNewLine

            MessageBox.Show(formContent)

            If (File.Exists(fileName)) Then
                Dim outputFile As StreamWriter = File.AppendText(fileName)
                outputFile.WriteLine(formContent)
                outputFile.Close()

                Dim sectionFileconten As StreamWriter = File.CreateText(currentSectionNumFile)
                sectionFileconten.Write(sectionNumber.ToString())
                sectionFileconten.Close()
            Else
                Dim outputFile As StreamWriter = File.CreateText(fileName)
                outputFile.WriteLine(formContent)
                outputFile.Close()

                Dim sectionFileconten As StreamWriter = File.CreateText(currentSectionNumFile)
                sectionFileconten.Write(sectionNumber.ToString())
                sectionFileconten.Close()
            End If

            resetForm()

        Else
            ifEmptyFieldWarning()
            MessageBox.Show("Please Complete the Form")
            resestBackColor()
        End If

    End Sub



    Private Sub ifEmptyFieldWarning()

        If (txtBxFirstName.Text = "") Then
            txtBxFirstName.BackColor = Color.Red
        End If

        If (txtBxMiddleName.Text = "") Then
            txtBxMiddleName.BackColor = Color.Red
        End If

        If (txtBxLastName.Text = "") Then
            txtBxLastName.BackColor = Color.Red
        End If

        If (txtBxAge.Text = "") Then
            txtBxAge.BackColor = Color.Red
        End If

        If (txtBxContactNum.Text = "") Then
            txtBxContactNum.BackColor = Color.Red
        End If

        If (txtBxEMail.Text = "") Then
            txtBxEMail.BackColor = Color.Red
        End If

        If (txtBxBarangay.Text = "") Then
            txtBxBarangay.BackColor = Color.Red
        End If




        If (rdBtnMale_Gender.Checked = rdBtnFemale_Gender.Checked) Then
            grpBxGender.BackColor = Color.Red
        End If

        If (rdBtnYes_Fever.Checked = rdBtnNo_Fever.Checked) Then
            grpBxFever.BackColor = Color.Red
        End If

        If (rdBtnYes_DryCough.Checked = rdBtnNo_DryCough.Checked) Then
            grpBxDryCough.BackColor = Color.Red
        End If

        If (rdBtnYes_SoreThroat.Checked = rdBtnNo_SoreThroat.Checked) Then
            grpBxSoreThroat.BackColor = Color.Red
        End If

        If (rdBtnYes_Tirediness.Checked = rdBtnNo_Tiredines.Checked) Then
            grpBxTirediness.BackColor = Color.Red
        End If

    End Sub




    Private Sub resestBackColor()
        txtBxFirstName.BackColor = Color.White
        txtBxMiddleName.BackColor = Color.White
        txtBxLastName.BackColor = Color.White
        txtBxAge.BackColor = Color.White
        txtBxContactNum.BackColor = Color.White
        txtBxEMail.BackColor = Color.White
        txtBxBarangay.BackColor = Color.White


        grpBxGender.BackColor = Color.White
        grpBxFever.BackColor = Color.White
        grpBxDryCough.BackColor = Color.White
        grpBxSoreThroat.BackColor = Color.White
        grpBxTirediness.BackColor = Color.White

    End Sub




    Private Function isNotEmptyTextBoxes() As Boolean
        If (Not (txtBxFirstName.Text = "") And Not (txtBxFirstName.Text = "") And Not (txtBxMiddleName.Text = "") And Not (txtBxLastName.Text = "") And Not (txtBxAge.Text = "") And Not (txtBxContactNum.Text = "") And Not (txtBxBarangay.Text = "")) Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function isFormCompled() As Boolean


        If ((Not (txtBxFirstName.Text = "") And Not (txtBxMiddleName.Text = "") And Not (txtBxLastName.Text = "") And Not (txtBxAge.Text = "") And Not (txtBxContactNum.Text = "") And Not (txtBxBarangay.Text = "")) _
                And (Not (rdBtnMale_Gender.Checked = rdBtnFemale_Gender.Checked) And
                Not (rdBtnYes_Fever.Checked = rdBtnNo_Fever.Checked) And Not (rdBtnYes_DryCough.Checked = rdBtnNo_DryCough.Checked) _
                And Not (rdBtnYes_SoreThroat.Checked = rdBtnNo_SoreThroat.Checked) And Not (rdBtnYes_Tirediness.Checked = rdBtnNo_Tiredines.Checked))) Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub resetForm()
        txtBxFirstName.Text = ""
        txtBxMiddleName.Text = ""
        txtBxLastName.Text = ""
        txtBxAge.Text = ""
        txtBxContactNum.Text = ""
        txtBxEMail.Text = ""
        rdBtnMale_Gender.Checked = False
        rdBtnFemale_Gender.Checked = False
        txtBxBarangay.Text = ""

        rdBtnYes_Fever.Checked = False
        rdBtnNo_Fever.Checked = False

        rdBtnYes_DryCough.Checked = False
        rdBtnNo_DryCough.Checked = False

        rdBtnYes_SoreThroat.Checked = False
        rdBtnNo_SoreThroat.Checked = False

        rdBtnYes_Tirediness.Checked = False
        rdBtnNo_Tiredines.Checked = False
    End Sub

    Private Sub NewRecordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewRecordToolStripMenuItem.Click
        resetForm()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MessageBox.Show("By Tan, Frederick B. ", "Contact Tracing App")
    End Sub

    Private Sub btnViewRecords_Click(sender As Object, e As EventArgs) Handles btnViewRecords.Click

        If (File.Exists(fileName)) Then
            rchTxtBxDisplayRecords.AppendText(getAllDataRecords())
            btnViewRecords.Enabled = False
        Else
            MessageBox.Show("No Records has been save yet")
        End If

    End Sub

    Private Function getAllDataRecords() As String
        Dim AllRecords As String = ""


        If (File.Exists(fileName)) Then
            Dim line As String = ""

            Dim inputFile As StreamReader = File.OpenText(fileName)
            While (Not inputFile.EndOfStream)
                line = inputFile.ReadLine() + vbNewLine
                'rchTxtBxDisplayRecords.AppendText(line)
                AllRecords += line
            End While
            inputFile.Close()

        Else
            MessageBox.Show("No Records has been save yet")
        End If


        Return AllRecords



    End Function


    Private Function getDatafromFormToQT() As String

        Dim dataFormStr As New List(Of String)
        Dim dataContentFormStr As String = ""

        If (isFormCompled()) Then

            inputGender = If(rdBtnMale_Gender.Checked, "Male", If(rdBtnFemale_Gender.Checked, "Female", ""))
            isFever_Str = If(rdBtnYes_Fever.Checked, "Yes", If(rdBtnNo_Fever.Checked, "No", ""))
            isDryCough_Str = If(rdBtnYes_DryCough.Checked, "Yes", If(rdBtnNo_DryCough.Checked, "No", ""))
            isSoreThroat_Str = If(rdBtnYes_SoreThroat.Checked, "Yes", If(rdBtnNo_SoreThroat.Checked, "No", ""))
            isTirediness_Str = If(rdBtnYes_Tirediness.Checked, "Yes", If(rdBtnNo_Tiredines.Checked, "No", ""))

            dataFormStr.Add(txtBxFirstName.Text)
            dataFormStr.Add(txtBxMiddleName.Text)
            dataFormStr.Add(txtBxLastName.Text)
            dataFormStr.Add(txtBxAge.Text)
            dataFormStr.Add(txtBxContactNum.Text)
            dataFormStr.Add(txtBxEMail.Text)
            dataFormStr.Add(inputGender)
            dataFormStr.Add(dateDTP.Text)
            dataFormStr.Add(txtBxBarangay.Text)
            dataFormStr.Add(isFever_Str)
            dataFormStr.Add(isDryCough_Str)
            dataFormStr.Add(isSoreThroat_Str)
            dataFormStr.Add(isTirediness_Str)



            dataContentFormStr = txtBxFirstName.Text + vbNewLine _
            + txtBxMiddleName.Text + vbNewLine _
            + txtBxLastName.Text + vbNewLine _
            + txtBxAge.Text + vbNewLine _
            + txtBxContactNum.Text + vbNewLine _
            + txtBxEMail.Text + vbNewLine _
            + inputGender + vbNewLine _
            + dateDTP.Text + vbNewLine _
            + txtBxBarangay.Text + vbNewLine _
            + isFever_Str + vbNewLine _
            + isDryCough_Str + vbNewLine _
            + isSoreThroat_Str + vbNewLine _
            + isTirediness_Str + vbNewLine


            'ElseIf () Then



        Else

            ifEmptyFieldWarning()
            MessageBox.Show("Please Complete the Form")
            resestBackColor()

            'dataContentFormStr = getDatafromFormToQT()


        End If

        Return dataContentFormStr
    End Function

    Private Sub setDatafromFromQRToForm(strParam As String)


        Dim dataContentFormStr As New List(Of String)
        Dim strDataFormList As New List(Of String)
        Dim tempStr As String = ""

        For Each leterCH In strParam
            dataContentFormStr.Add(leterCH)
        Next

        If ((dataContentFormStr.Equals(""))) Then

            For index = 1 To dataContentFormStr.Count
                If (dataContentFormStr(index) <> vbNewLine) Then
                    tempStr += dataContentFormStr(index)
                Else
                    strDataFormList.Add(tempStr)
                End If
            Next



            txtBxFirstName.Text = strDataFormList(0)
            txtBxMiddleName.Text = strDataFormList(1)
            txtBxLastName.Text = strDataFormList(2)
            txtBxAge.Text = strDataFormList(3)
            txtBxContactNum.Text = strDataFormList(4)
            txtBxEMail.Text = strDataFormList(5)
            inputGender = strDataFormList(6)
            dateDTP.Text = strDataFormList(7)
            txtBxBarangay.Text = strDataFormList(8)
            isFever_Str = strDataFormList(9)
            isDryCough_Str = strDataFormList(10)
            isSoreThroat_Str = strDataFormList(11)
            isTirediness_Str = strDataFormList(12)



            If (inputGender = "Male") Then
                rdBtnMale_Gender.Checked = True
            ElseIf (inputGender = "Female") Then
                rdBtnFemale_Gender.Checked = True

            End If

            If (isFever_Str = "Yes") Then
                rdBtnYes_Fever.Checked = True
            ElseIf (isFever_Str = "No") Then
                rdBtnNo_Fever.Checked = True
            End If

            If (isDryCough_Str = "Yes") Then
                rdBtnYes_DryCough.Checked = True
            ElseIf (isDryCough_Str = "No") Then
                rdBtnNo_DryCough.Checked = True
            End If

            If (isSoreThroat_Str = "Yes") Then
                rdBtnYes_SoreThroat.Checked = True
            ElseIf (isSoreThroat_Str = "No") Then
                rdBtnNo_SoreThroat.Checked = True
            End If

            If (isTirediness_Str = "Yes") Then
                rdBtnYes_Tirediness.Checked = True
            ElseIf (isTirediness_Str = "No") Then
                rdBtnNo_Tiredines.Checked = True
            End If


        End If


    End Sub



    Private Sub btnClearDisplay_Click(sender As Object, e As EventArgs) Handles btnClearDisplay.Click
        rchTxtBxDisplayRecords.Clear()
        btnViewRecords.Enabled = True

    End Sub
    Private Sub keyPress_Letters_Spaces_Numbers_Period(sender As Object, e As KeyPressEventArgs) Handles txtBxBarangay.KeyPress, txtBxMiddleName.KeyPress, txtBxLastName.KeyPress, txtBxFirstName.KeyPress
        If ((Not Char.IsLetter(e.KeyChar)) And (Not Char.IsControl(e.KeyChar)) And (Not Char.IsWhiteSpace(e.KeyChar)) And (Not Char.IsNumber(e.KeyChar) And Not (e.KeyChar.ToString() = "."))) Then

            e.Handled = True
        End If
    End Sub
    Private Sub keyPress_Numbers(sender As Object, e As KeyPressEventArgs) Handles txtBxContactNum.KeyPress, txtBxAge.KeyPress
        If ((Not Char.IsNumber(e.KeyChar)) And (Not Char.IsControl(e.KeyChar))) Then

            e.Handled = True
        End If
    End Sub

    Private Sub keyPres_EMail(sender As Object, e As KeyPressEventArgs) Handles txtBxEMail.KeyPress
        If ((Not Char.IsLetter(e.KeyChar)) And (Not Char.IsControl(e.KeyChar)) And (Not Char.IsWhiteSpace(e.KeyChar)) And (Not Char.IsNumber(e.KeyChar)) _
                And Not (e.KeyChar.ToString() = ".") And Not (e.KeyChar.ToString() = "@") And Not (e.KeyChar.ToString() = "_")) Then

            e.Handled = True
        End If
    End Sub

    Private Sub rchTxtBxDisplayRecords_KeyPress(sender As Object, e As KeyPressEventArgs) Handles rchTxtBxDisplayRecords.KeyPress
        e.Handled = True
    End Sub

    Private Sub rchTxtBxDisplayRecords_KeyUp(sender As Object, e As KeyEventArgs) Handles rchTxtBxDisplayRecords.KeyUp
        e.Handled = True
    End Sub

    Private Sub rchTxtBxDisplayRecords_KeyDown(sender As Object, e As KeyEventArgs) Handles rchTxtBxDisplayRecords.KeyDown
        e.Handled = True
    End Sub

    Private Sub btnGeneraateQRCode_Click(sender As Object, e As EventArgs) Handles btnGeneraateQRCode.Click

        generateQRCode()

    End Sub

    Private Sub generateQRCode()
        If getDatafromFormToQT() <> "" Then

            Dim gen As New QRCodeGenerator
            Dim data = gen.CreateQrCode(getDatafromFormToQT(), QRCodeGenerator.ECCLevel.Q)
            Dim code As New QRCode(data)
            PictureBoxQRCodeDisplay.Image = code.GetGraphic(6)
        End If
    End Sub

    Private Sub ButtonSaveQRCode_Click(sender As Object, e As EventArgs) Handles ButtonSaveQRCode.Click

        Dim noImage As Image

        Dim imgQRCode

        If Not (Image.Equals(PictureBoxQRCodeDisplay.Image, noImage)) Then

            imgQRCode = New Bitmap(PictureBoxQRCodeDisplay.Image)
        Else

            MessageBox.Show("ERRROR!! No QR Code Generated")
            Return

        End If
        glblCounter += 1
        Dim imgQRCodeStrFileName = "QRCode_" + glblCounter.ToString() + ".png"

        Dim mySFD As New SaveFileDialog()
        mySFD.Filter = "PNG Image | *.png"
        mySFD.FileName = imgQRCodeStrFileName


        If mySFD.ShowDialog = Windows.Forms.DialogResult.OK Then
            imgQRCode.Save(mySFD.FileName)
        End If

    End Sub

    'Public Delegate Sub FinalFrame_NewFrame(sender As Object, eventArgs As NewFrameEventArgs)

    'Dim mynewFrameEventHandler As NewFrameEventHandler = AddressOf FinalFrame_NewFrame


    Private Sub Button_StartScan_Click(sender As Object, e As EventArgs) Handles Button_StartScan.Click


        StartWebCam()


        'FinalFrame = New VideoCaptureDevice(CaptureDevice(ComboBox_WebCamraDevices.SelectedIndex).MonikerString)
        'FinalFrame += New NewFrameEventHandler(FinalFrame_NewFrameD)
        'FinalFrame.Start()


    End Sub

    Private Sub StartWebCam()

        Dim camss As VideoCaptureDeviceForm = New VideoCaptureDeviceForm()
        If camss.ShowDialog() = Windows.Forms.DialogResult.OK Then
            FinalFrame = camss.VideoDevice
            AddHandler FinalFrame.NewFrame, New NewFrameEventHandler(AddressOf FinalFrame_NewFrame)
            FinalFrame.Start()

        End If

    End Sub


    Public Sub FinalFrame_NewFrame(sender As Object, eventArgs As NewFrameEventArgs)

        bmp = DirectCast(eventArgs.Frame.Clone(), Bitmap)
        PictureBoxQRScanner.Image = DirectCast(eventArgs.Frame.Clone(), Bitmap) 'CType(eventArgs.Frame.Clone(), Bitmap)
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        'FinalFrame.Stop()
        If (FinalFrame.IsRunning = True) Then
            stopWebCam()
        End If

    End Sub

    Private Sub stopWebCam()
        FinalFrame.Stop()
    End Sub



    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Dim readerQRZing As New BarcodeReader(Of Bitmap)
        'Dim resultQR As Result = readerQRZing.Decode(TryCast(PictureBoxQRCodeDisplay.Image, Bitmap))

        'Try
        'Dim decodedStr As String = resultQR.ToString().Trim()
        'If (Not (decodedStr.Equals(""))) Then
        'rchTxtBxDisplayRecords.Text = decodedStr
        'End If
        'Catch ex As Exception

        'End Try


    End Sub

    Private Sub Button_ReadQRCode_Click(sender As Object, e As EventArgs) Handles Button_ReadQRCode.Click

        'Timer1.Start()

        readQR()


    End Sub

    Private Sub readQR()
        PictureBoxQRScanPausse.Image = PictureBoxQRScanner.Image

        MessageBox.Show("Hello")

        Try
            stopWebCam()
            Dim readerQR As QRCodeDecoder = New QRCodeDecoder()
            rchTxtBxDisplayRecords.Text = readerQR.Decode(New QRCodeBitmapImage(PictureBoxQRScanPausse.Image))
            MessageBox.Show("Detected!!!")

        Catch ex As Exception
            StartWebCam()

        End Try

    End Sub
    Private Sub ButtonOpenFile_Click(sender As Object, e As EventArgs) Handles ButtonOpenFile.Click
        Dim ofd As New OpenFileDialog()

        'Dim imgQRCodeStrFileName = "QRCode_" + glblCounter.ToString() + ".png"
        ofd.Filter = "PNG Image | *.png"
        '  mySFD.FileName = imgQRCodeStrFileName

        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBoxQRScanPausse.Image = Image.FromFile(ofd.FileName)
            openTestcheck = True

        End If

    End Sub

    Private Sub ButtonDecodeQR_Click(sender As Object, e As EventArgs) Handles ButtonDecodeQR.Click




        If (openTestcheck) Then
            Dim deco As QRCodeDecoder = New QRCodeDecoder()
            rchTxtBxDisplayRecords.Text = deco.Decode(New QRCodeBitmapImage(PictureBoxQRScanPausse.Image))
            'setDatafromFromQRToForm(deco.Decode(New QRCodeBitmapImage(PictureBoxQRScanPausse.Image)))
            openTestcheck = False
        Else
            MessageBox.Show("ERRROR!! No QR Code Input")
            Return
        End If

    End Sub
End Class
