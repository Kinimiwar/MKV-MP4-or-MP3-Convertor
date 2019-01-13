Imports System.ComponentModel
Imports System.IO
Imports System.Text
Imports System.Threading
Imports Convertor.My.Resources
Imports MediaInfoDotNet
Imports Microsoft.WindowsAPICodePack.Taskbar

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Icon = My.Resources.app_icon
        Width = 500
        Height = 462
        ToolStripStatusLabel2.Text = Format(Now.Date, "dd MMM yyyy")
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As CancelEventArgs) Handles OpenFileDialog1.FileOk
        For Each file In OpenFileDialog1.FileNames
            Notrepeat(file)
            If Notrepeat(file) = True Then
                ListBox1.Items.Add(file)
            Else
                MsgBox(Form1_ListBox1_DragDrop_Duplicated_files_are_not_allowed_in_list, , Form1_ListBox1__Error)
            End If
        Next
        ListBox1.ClearSelected()
        ListBox1.SelectedIndex = CInt(ListBox1.Items.Count - OpenFileDialog1.FileNames.Length)
    End Sub

    Private Function Notrepeat(file As String) As Boolean
        For Each Item In ListBox1.Items
            If Item = file Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "MKV (*.mkv)|*.mkv"
        OpenFileDialog1.FilterIndex = 1
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Deletelistboxselected()
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim myFiles() As String
            myFiles = e.Data.GetData(DataFormats.FileDrop)
            For i = 0 To myFiles.Length - 1
                Notrepeat(myFiles(i))
                If Notrepeat(myFiles(i)) = True Then
                    ListBox1.Items.Add(myFiles(i))
                Else
                    MsgBox(Form1_ListBox1_DragDrop_Duplicated_files_are_not_allowed_in_list, , Form1_ListBox1__Error)
                End If
            Next
            ListBox1.ClearSelected()
            ListBox1.SelectedIndex = CInt(ListBox1.Items.Count - myFiles.Length)
        End If
    End Sub

    Private Sub ListBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox1.KeyDown
        If e.KeyCode = Keys.Delete Then
            Deletelistboxselected()
        End If
        If (e.Control And e.KeyCode = Keys.A) Then
            ListBox1.BeginUpdate()
            ListBox1.Select()
            SendKeys.Send("{Home}")
            SendKeys.Send("+{End}")
            ListBox1.EndUpdate()
        End If
    End Sub

    Private Sub CheckedListBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles CheckedListBox1.KeyDown
        If e.KeyCode = Keys.Delete Then
            Deletecheckboxchecked()
        End If
        If (e.Control And e.KeyCode = Keys.A) Then
            For i = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(i, True)
            Next
        End If
    End Sub

    Private Sub Deletelistboxselected()
        Dim j As Integer
        For i = 0 To ListBox1.SelectedIndices.Count - 1
            ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
            j = ListBox1.SelectedIndex
        Next
        DataGridView1.Rows.Clear()
        If ListBox1.Items.Count >= 1 Then
            If j - 1 >= 0 Then
                ListBox1.SelectedIndex = j - 1
            Else
                ListBox1.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub Deletecheckboxchecked()
        With CheckedListBox1
            If .CheckedItems.Count > 0 Then
                For checked As Integer = .CheckedItems.Count - 1 To 0 Step -1
                    .Items.Remove(.CheckedItems(checked))
                Next
            End If
        End With
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If DialogResult.OK = FolderBrowserDialog1.ShowDialog() Then
            Dim result As MsgBoxResult =
                    MsgBox(
                        Form1_Button4_Click_Would_you_like_search_in_path_ & FolderBrowserDialog1.SelectedPath &
                        Form1_Button4_Click__for_MKV_files__,
                        MsgBoxStyle.YesNo, Form1_Button4_Click_Search_folder)
            If result = MsgBoxResult.Yes Then
                Cursor = Cursors.WaitCursor
                Autosearch(FolderBrowserDialog1.SelectedPath)
                Cursor = Cursors.Default
                Label1.Text = Form1_Button4_Click_Search_Results_in___ & FolderBrowserDialog1.SelectedPath
                Width = 980
            End If
        End If
    End Sub

    Private Sub Autosearch(src As String)
        For i = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, True)
        Next
        Deletecheckboxchecked()
        Dim folder As New DirectoryInfo(src)
        For Each file As FileInfo In folder.GetFiles("*.mkv", SearchOption.AllDirectories)
            CheckedListBox1.Items.Add(file.FullName)
            Application.DoEvents()
        Next
    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        For Each i In CheckedListBox1.CheckedIndices
            Notrepeat(CheckedListBox1.Items(i))
            If Notrepeat(CheckedListBox1.Items(i)) = True Then
                ListBox1.Items.Add(CheckedListBox1.Items(i))
            Else
                MsgBox(Form1_ListBox1_DragDrop_Duplicated_files_are_not_allowed_in_list, , Form1_ListBox1__Error)
            End If
        Next
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Deletecheckboxchecked()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If Button6.Text = Form1_Button6_Click_Select_All Then
            For i = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(i, True)
            Next
            Button6.Text = Form1_Button6_Click_Deselect_All
        Else
            For i = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(i, False)
            Next
            Button6.Text = Form1_Button6_Click_Select_All
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Width = 500
        For i = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, True)
        Next
        Deletecheckboxchecked()
    End Sub

    Private Sub ListBox1_MouseClick(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseClick
        If ListBox1.SelectedIndices.Count > 1 Then
            DataGridView1.Rows.Clear()
        ElseIf ListBox1.SelectedIndices.Count = 1
            DataGridView1.Rows.Clear()
            Getinfo(ListBox1.SelectedItem)
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        ListBox1.BeginUpdate()
        ListBox1.Select()
        SendKeys.Send("{Home}")
        SendKeys.Send("+{End}")
        ListBox1.EndUpdate()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If Button10.Text = Form1_Button10_Click_Convert_All Then
            Button10.Text = Form1_Button10_Click__Stop
            Dim args As New ArgumentType
            Dim filessrc2 As New List(Of String)
            For i = 0 To ListBox1.Items.Count - 1
                DataGridView1.Rows.Clear()
                filessrc2.Add(ListBox1.Items(i))
            Next
            args.Filessrc = filessrc2
            BackgroundWorker1.RunWorkerAsync(args)
        ElseIf Button10.Text = Form1_Button10_Click__Stop Then
            Button10.Text = Form1_Button10_Click_Convert_All
            BackgroundWorker1.CancelAsync()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = Form1_Button3_Click_Convert_Selected Then
            Button3.Text = Form1_Button10_Click__Stop
            Dim args As New ArgumentType
            Dim filessrc2 As New List(Of String)
            For Each Selected In ListBox1.SelectedItems
                DataGridView1.Rows.Clear()
                filessrc2.Add(Selected)
            Next
            args.Filessrc = filessrc2
            BackgroundWorker1.RunWorkerAsync(args)
        ElseIf Button3.Text = Form1_Button10_Click__Stop Then
            Button3.Text = Form1_Button3_Click_Convert_Selected
            BackgroundWorker1.CancelAsync()
        End If
    End Sub

    Public Class ArgumentType
        Public Filessrc As List(Of String)
    End Class

    Private Delegate Sub Thread1Delegate(currentsrc As String)

    Private Sub Thread1(currentsrc As String)
        ToolStripStatusLabel1.Text = currentsrc
        CheckBox1.Enabled = False
        CheckBox3.Enabled = False
        CheckBox4.Enabled = False
        CheckBox5.Enabled = False
        CheckBox6.Enabled = False
        Button13.Enabled = False
        CheckBox2.Enabled = False
        CheckBox7.Enabled = False
    End Sub

    Private Sub Thread2(currentsrc As String)
        ListBox1.ClearSelected()
        Dim index As Integer = ListBox1.FindString(currentsrc)
        If index <> -1 Then
            ListBox1.SetSelected(index, True)
        End If
    End Sub

    Private Delegate Sub Thread1Delegate2(stat As Boolean, interval As Integer)

    Private Sub TimerCond(stat As Boolean, interval As Integer)
        If stat = True
            Timer1.Interval = interval
            Timer1.Start()
        ElseIf stat = False
            Timer1.Stop()
        End If
    End Sub

    Private Sub Getinfo(filesrc As String)
        Dim aviFile = New MediaFile(filesrc)
        For i = 0 To aviFile.Video.Count - 1
            DataGridView1.Rows.Add(aviFile.Video(i).ID - 1, aviFile.Video(i).Format,
                                   aviFile.Video(i).Width & " x " & aviFile.Video(i).Height & " p",
                                   aviFile.Video(i).CodecId.ToString)
        Next
        For i = 0 To aviFile.Audio.Count - 1
            DataGridView1.Rows.Add(aviFile.Audio(i).ID - 1, aviFile.Audio(i).Format,
                                   aviFile.Audio(i).SampleRate & " bit/s", aviFile.Audio(i).CodecId)
        Next
        For i = 0 To aviFile.Text.Count - 1
            DataGridView1.Rows.Add(aviFile.Text(i).ID - 1, aviFile.Text(i).Format, "",
                                   aviFile.Text(i).CodecId)
        Next
    End Sub
    Private mp3quality As String = ""
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim updateLabe1 = New Thread1Delegate(AddressOf Thread1)
        Dim updateTimer1 = New Thread1Delegate2(AddressOf TimerCond)
        Dim updateLabe2 = New Thread1Delegate(AddressOf Thread2)
        Dim args As ArgumentType = e.Argument
        For j = 0 To args.Filessrc.Count - 1
            Invoke(updateTimer1, True, 50)
            Dim filesrc As String = args.Filessrc(j)
            Invoke(updateLabe2, filesrc)
            Dim filename As String = Path.GetFileNameWithoutExtension(filesrc)
            Dim src As String = Path.GetDirectoryName(filesrc)
            Dim convsrc As String = Application.StartupPath & "\convtemp\"
            Dim aviFile = New MediaFile(filesrc)
            Dim vextension(CInt(aviFile.Video.Count) - 1) As String
            Dim aextension(CInt(aviFile.Audio.Count) - 1) As String
            Dim textension(CInt(aviFile.Text.Count) - 1) As String
            Dim convtype = ""
            Dim ms As String = ""
            Invoke(updateLabe1, filename)
            BackgroundWorker1.ReportProgress(100)
            If CheckBox7.Checked Then
                convtype = "ffmpeg-mp3"
            ElseIf CheckBox2.Checked AndAlso CheckBox7.Checked = False Then
                convtype = "ffmpeg"
            Else
                Dim builder2 As New StringBuilder
                builder2.Append("tracks " & Chr(34) & filesrc & Chr(34))
                For i = 0 To aviFile.Video.Count - 1
                    If aviFile.Video(i).CodecId = "V_MPEG1" Then
                        vextension(i) = ".m1v"
                        convtype = "MP4BOX"
                    ElseIf aviFile.Video(i).CodecId = "V_MPEG2" Then
                        vextension(i) = ".m2v"
                        convtype = "MP4BOX"
                    ElseIf aviFile.Video(i).CodecId = "V_MPEG4/ISO/SP" Or aviFile.Video(i).CodecId = "V_MPEG4/ISO/ASP" Or
                           aviFile.Video(i).CodecId = "V_MPEG4/ISO/AP" Or aviFile.Video(i).CodecId = "V_MPEG4/MS/V2" Or
                           aviFile.Video(i).CodecId = "V_MPEG4/MS/V3" Then
                        vextension(i) = ".m4v"
                        convtype = "MP4BOX"
                    ElseIf aviFile.Video(i).CodecId = "V_MPEG4/ISO/AVC" Then
                        vextension(i) = ".h264"
                        convtype = "MP4BOX"
                    ElseIf aviFile.Video(i).CodecId = "V_MPEGH/ISO/HEVC" Then
                        vextension(i) = ".h265"
                        convtype = "MP4BOX"
                    Else
                        convtype = "ffmpeg"
                    End If
                    builder2.Append(Chr(32) & aviFile.Video(i).ID - 1 & Chr(58) & Chr(34) & convsrc & filename &
                                    aviFile.Video(i).ID - 1 & vextension(i) & Chr(34))
                Next
                If convtype = "MP4BOX" Then
                    For i = 0 To aviFile.Audio.Count - 1
                        If aviFile.Audio(i).CodecId = "A_MPEG/L1" Or aviFile.Audio(i).CodecId = "A_MPEG/L2" Or
                           aviFile.Audio(i).CodecId = "A_MPEG/L3" Then
                            aextension(i) = ".mp3"
                            convtype = "MP4BOX"
                        ElseIf aviFile.Audio(i).CodecId = "A_AAC" Or aviFile.Audio(i).CodecId = "A_AAC/MPEG2/MAIN" Or
                               aviFile.Audio(i).CodecId = "A_AAC/MPEG2/LC" Or aviFile.Audio(i).CodecId = "A_AAC/MPEG2/LC/SBR" Or
                               aviFile.Audio(i).CodecId = "A_AAC/MPEG2/SSR" Or aviFile.Audio(i).CodecId = "A_AAC/MPEG4/MAIN" Or
                               aviFile.Audio(i).CodecId = "A_AAC/MPEG4/LC" Or aviFile.Audio(i).CodecId = "A_AAC/MPEG4/LC/SBR" Or
                               aviFile.Audio(i).CodecId = "A_AAC/MPEG4/LC/SBR/PS" Or
                               aviFile.Audio(i).CodecId = "A_AAC/MPEG4/SSR" Or
                               aviFile.Audio(i).CodecId = "A_AAC/MPEG4/LTP" Then
                            aextension(i) = ".aac"
                            convtype = "MP4BOX"
                        ElseIf aviFile.Audio(i).CodecId = "A_AC3" Or aviFile.Audio(i).CodecId = "A_AC3/BSID9" Or
                               aviFile.Audio(i).CodecId = "A_AC3/BSID10" Or aviFile.Audio(i).CodecId = "A_DTS" Or
                               aviFile.Audio(i).CodecId = "A_EAC3;" Then
                            aextension(i) = ".ac3"
                            convtype = "MP4BOX"
                        Else
                            convtype = "ffmpeg"
                        End If
                        builder2.Append(Chr(32) & aviFile.Audio(i).ID - 1 & Chr(58) & Chr(34) & convsrc & filename &
                                        aviFile.Audio(i).ID - 1 & aextension(i) & Chr(34))
                    Next
                End If
                If convtype = "MP4BOX" Then
                    For i = 0 To aviFile.Text.Count - 1
                        If aviFile.Text(i).CodecId = "S_SSA" Or aviFile.Text(i).CodecId = "S_ASS" Or
                           aviFile.Text(i).CodecId = "S_TEXT/ASS" Or aviFile.Text(i).CodecId = "S_TEXT/SSA" Then
                            textension(i) = ".sub"
                        ElseIf aviFile.Text(i).CodecId = "S_VOBSUB" Then
                            textension(i) = ".idx"
                        ElseIf aviFile.Text(i).Format = "SubRip" Then
                            textension(i) = ".srt"
                        ElseIf aviFile.Text(i).Format = "TTML" Then
                            textension(i) = ".ttml"
                        ElseIf aviFile.Text(i).Format = "WebVTT" Then
                            textension(i) = ".vtt"
                        Else
                            textension(i) = ".srt"
                        End If
                        builder2.Append(Chr(32) & aviFile.Text(i).ID - 1 & Chr(58) & Chr(34) & convsrc & filename &
                                        aviFile.Text(i).ID - 1 & textension(i) & Chr(34))
                    Next
                End If
                If CheckBox6.Checked = True Then
                    builder2.Append(Chr(32) & "-r" & Chr(32) & Chr(34) & "log\mkvextract-" & filename & ".log" & Chr(34))
                End If
                ms = builder2.ToString

                If (BackgroundWorker1.CancellationPending) Then
                    e.Cancel = True
                    Exit Sub
                End If
            End If
            Select Case convtype
                Case "MP4BOX"
                    Try
                        Dim p As New Process
                        p.StartInfo.FileName = "extract.exe"
                        p.StartInfo.UseShellExecute = True
                        p.StartInfo.Arguments = ms
                        If CheckBox5.Checked = False Then
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                        Else
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal
                        End If
                        p.Start()
                        While Not p.HasExited
                            If (BackgroundWorker1.CancellationPending) Then
                                p.Kill()
                                e.Cancel = True
                                Exit Sub
                            End If
                        End While
                        p.WaitForExit()
                        If CheckBox3.Checked = True Then
                            For i = 0 To aviFile.Text.Count - 1
                                If File.Exists(convsrc & filename & aviFile.Text(i).ID - 1 & textension(i)) Then
                                    File.Copy(convsrc & filename & aviFile.Text(i).ID - 1 & textension(i),
                                              src & "\" & filename & "-in subtitle-" & aviFile.Text(i).ID - 1 &
                                              textension(i))
                                End If
                            Next
                        End If
                    Catch ex As Exception
                        MsgBox(Form1_BackgroundWorker1_DoWork_Error___ + ex.Message, , "MKV Extract")
                        BackgroundWorker1.ReportProgress(0)
                        Continue For
                    End Try
                    Try
                        Dim builder As New StringBuilder
                        For i = 0 To aviFile.Video.Count - 1
                            builder.Append(Chr(32) & "-add " & Chr(34) & convsrc & filename &
                                           aviFile.Video(i).ID - 1 & vextension(i) & Chr(34))
                        Next
                        For i = 0 To aviFile.Audio.Count - 1
                            builder.Append(Chr(32) & "-add " & Chr(34) & convsrc & filename & aviFile.Audio(i).ID - 1 &
                                           aextension(i) & Chr(34))
                        Next
                        If CheckBox3.Checked = False Then
                            For i = 0 To aviFile.Text.Count - 1
                                If File.Exists(convsrc & filename & aviFile.Text(i).ID - 1 & textension(i)) Then
                                    'convert to utf8
                                    MyExtentions.Common.Files.ToUTF8(convsrc & filename & aviFile.Text(i).ID - 1 & textension(i))
                                    builder.Append(
                                        Chr(32) & "-add " & Chr(34) & convsrc & filename & aviFile.Text(i).ID - 1 &
                                        textension(i) & Chr(34))
                                End If
                            Next
                        End If
                        builder.Append(Chr(32) & "-new " & Chr(34) & Path.ChangeExtension(filesrc, "mp4") & Chr(34))
                        If CheckBox6.Checked = True Then
                            builder.Append(
                                Chr(32) & "-lf" & Chr(32) & Chr(34) & "log\MP4Box-" & filename & ".log" & Chr(34) &
                                Chr(32) & "-logs all@debug")
                        End If
                        Dim s As String = builder.ToString
                        Dim p As New Process
                        p.StartInfo.FileName = "MP4Box.exe"
                        p.StartInfo.UseShellExecute = True
                        p.StartInfo.Arguments = s
                        If CheckBox5.Checked = False Then
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                        Else
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal
                        End If
                        BackgroundWorker1.ReportProgress(650)
                        p.Start()
                        While Not p.HasExited
                            If (BackgroundWorker1.CancellationPending) Then
                                p.Kill()
                                e.Cancel = True
                                Exit Sub
                            End If
                        End While
                        p.WaitForExit()
                    Catch ex2 As Exception
                        MsgBox(Form1_BackgroundWorker1_DoWork_Error___ + ex2.Message, , "MP4Box")
                        BackgroundWorker1.ReportProgress(0)
                        Continue For
                    End Try
                Case "ffmpeg"
                    Try
                        Dim builder As New StringBuilder
                        builder.Append(
                            "-i" & Chr(32) & Chr(34) & filesrc & Chr(34) & Chr(32) & "-c:v libx264 -c:a aac" &
                            Chr(32) & Chr(34) & Path.ChangeExtension(filesrc, "mp4") & Chr(34) & Chr(32) & "-y")
                        If CheckBox3.Checked = False Then
                            builder.Append(Chr(32) & "-c:s copy")
                        End If
                        Dim s As String = builder.ToString
                        Dim p As New Process
                        p.StartInfo.FileName = "ffmpeg.exe"
                        p.StartInfo.Arguments = s
                        If CheckBox5.Checked = False Then
                            Invoke(updateTimer1, False, 0)
                            p.StartInfo.UseShellExecute = False
                            p.StartInfo.RedirectStandardError = True
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                            p.StartInfo.CreateNoWindow = True
                        Else
                            Invoke(updateTimer1, False, 300)
                            p.StartInfo.UseShellExecute = True
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal
                        End If
                        BackgroundWorker1.ReportProgress(150)
                        p.Start()
                        If CheckBox5.Checked = False Then
                            Dim output As String
                            Dim logsrc As String = "log\ffmpeg-" & filename & ".log"
                            If File.Exists(logsrc) Then
                                File.Delete(logsrc)
                            End If
                            While Not p.HasExited
                                Dim currentFrame As Integer
                                While Not p.StandardError.EndOfStream
                                    If (BackgroundWorker1.CancellationPending) Then
                                        e.Cancel = True
                                        Exit While
                                    End If
                                    output = p.StandardError.ReadLine() & Environment.NewLine
                                    If output.Contains("frame=") Then
                                        currentFrame = CInt(Mid(output, 7, 6))
                                        BackgroundWorker1.ReportProgress(
                                        150 + ((currentFrame / aviFile.Video(0).FrameCount) * 850))
                                    End If
                                    If CheckBox6.Checked = True Then
                                        File.AppendAllText(logsrc, output)
                                    End If
                                End While
                                If e.Cancel = True Then
                                    p.Kill()
                                    Exit Sub
                                End If
                            End While
                        Else
                            While Not p.HasExited
                                If (BackgroundWorker1.CancellationPending) Then
                                    p.Kill()
                                    e.Cancel = True
                                    Exit Sub
                                End If
                            End While
                        End If
                        p.WaitForExit()
                    Catch ex2 As Exception
                        MsgBox(Form1_BackgroundWorker1_DoWork_Error___ + ex2.Message, , "ffmpeg")
                        BackgroundWorker1.ReportProgress(0)
                        Continue For
                    End Try
                Case "ffmpeg-mp3"
                    Try
                        Dim builder As New StringBuilder
                        builder.Append(
                            "-i" & Chr(32) & Chr(34) & filesrc & Chr(34) & Chr(32) & mp3quality & "-vn" &
                            Chr(32) & Chr(34) & Path.ChangeExtension(filesrc, "mp3") & Chr(34) & Chr(32) & "-y")

                        Dim s As String = builder.ToString
                        Dim p As New Process
                        p.StartInfo.FileName = "ffmpeg.exe"
                        p.StartInfo.Arguments = s
                        If CheckBox5.Checked = False Then
                            Invoke(updateTimer1, False, 0)
                            p.StartInfo.UseShellExecute = False
                            p.StartInfo.RedirectStandardError = True
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                            p.StartInfo.CreateNoWindow = True
                        Else
                            Invoke(updateTimer1, False, 300)
                            p.StartInfo.UseShellExecute = True
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal
                        End If
                        BackgroundWorker1.ReportProgress(150)
                        p.Start()
                        If CheckBox5.Checked = False Then
                            Dim output As String
                            Dim logsrc As String = "log\ffmpeg-" & filename & ".log"
                            If File.Exists(logsrc) Then
                                File.Delete(logsrc)
                            End If
                            While Not p.HasExited
                                Dim currentFrame As Integer
                                While Not p.StandardError.EndOfStream
                                    If (BackgroundWorker1.CancellationPending) Then
                                        e.Cancel = True
                                        Exit While
                                    End If
                                    output = p.StandardError.ReadLine() & Environment.NewLine
                                    If output.Contains("time=") Then
                                        currentFrame = ((Mid(output, 22, 2)) * 3600000 + (Mid(output, 25, 2)) * 60000 + (Mid(output, 28, 2)) * 1000)
                                        BackgroundWorker1.ReportProgress(
                                        150 + ((currentFrame / aviFile.Audio(0).Duration) * 850))
                                    End If
                                    If CheckBox6.Checked = True Then
                                        File.AppendAllText(logsrc, output)
                                    End If
                                End While
                                If e.Cancel = True Then
                                    p.Kill()
                                    Exit Sub
                                End If
                            End While
                        Else
                            While Not p.HasExited
                                If (BackgroundWorker1.CancellationPending) Then
                                    p.Kill()
                                    e.Cancel = True
                                    Exit Sub
                                End If
                            End While
                        End If
                        p.WaitForExit()
                    Catch ex2 As Exception
                        MsgBox(Form1_BackgroundWorker1_DoWork_Error___ + ex2.Message, , "ffmpeg")
                        BackgroundWorker1.ReportProgress(0)
                        Continue For
                    End Try
            End Select

            If CheckBox4.Checked = True And File.Exists(filesrc) Then
                File.Delete(filesrc)
            End If
            If CheckBox1.Checked = True And Directory.Exists("convtemp") Then
                For Each filei As String In Directory.GetFiles("convtemp")
                    File.Delete(filei)
                Next
            End If
            Invoke(updateTimer1, False, 0)
            BackgroundWorker1.ReportProgress(1000)
            Thread.Sleep(1000)
        Next
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) _
        Handles BackgroundWorker1.ProgressChanged
        ToolStripProgressBar1.Value = e.ProgressPercentage
        TaskbarManager.Instance.SetProgressValue(ToolStripProgressBar1.Value, 1000)
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles BackgroundWorker1.RunWorkerCompleted
        If e.Cancelled = True Then
            Timer1.Stop()
            ToolStripStatusLabel1.Text = Form1_BackgroundWorker1_RunWorkerCompleted_Ready__
            CheckBox1.Enabled = True
            If CheckBox7.Checked = False Then
                CheckBox3.Enabled = True
            End If
            CheckBox4.Enabled = True
            Button13.Enabled = True
            If CheckBox5.Checked = True Then
                CheckBox6.Enabled = False
                CheckBox6.Checked = False
            Else
                CheckBox6.Enabled = True
            End If
            If CheckBox6.Checked = True Then
                CheckBox5.Enabled = False
                CheckBox5.Checked = False
            Else
                CheckBox5.Enabled = True
            End If
            CheckBox2.Enabled = True
            CheckBox7.Enabled = True
            ToolStripProgressBar1.Value = 0
            TaskbarManager.Instance.SetProgressValue(0, 1000)
            If CheckBox1.Checked = True And Directory.Exists("convtemp") Then
                For Each filei As String In Directory.GetFiles("convtemp")
                    File.Delete(filei)
                Next
            End If
            MsgBox(Form1_BackgroundWorker1_RunWorkerCompleted_Stopped)
        Else
            Button3.Text = Form1_Button3_Click_Convert_Selected
            Button10.Text = Form1_Button10_Click_Convert_All
            ToolStripStatusLabel1.Text = Form1_BackgroundWorker1_RunWorkerCompleted_Ready__
            CheckBox1.Enabled = True
            If CheckBox7.Checked = False Then
                CheckBox3.Enabled = True
            End If
            CheckBox4.Enabled = True
            Button13.Enabled = True
            If CheckBox5.Checked = True Then
                CheckBox6.Enabled = False
                CheckBox6.Checked = False
            Else
                CheckBox6.Enabled = True
            End If
            If CheckBox6.Checked = True Then
                CheckBox5.Enabled = False
                CheckBox5.Checked = False
            Else
                CheckBox5.Enabled = True
            End If
            CheckBox2.Enabled = True
            CheckBox7.Enabled = True
            ToolStripProgressBar1.Value = 0
            TaskbarManager.Instance.SetProgressValue(0, 1000)
            MsgBox(Form1_BackgroundWorker1_RunWorkerCompleted_Completed_Successfully)
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedIndices.Count > 1 Then
            DataGridView1.Rows.Clear()
            Button12.Enabled = False
        ElseIf ListBox1.SelectedIndices.Count = 1
            DataGridView1.Rows.Clear()
            Getinfo(ListBox1.SelectedItem)
            Button12.Enabled = True
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ToolStripProgressBar1.Increment(1)
        TaskbarManager.Instance.SetProgressValue(ToolStripProgressBar1.Value, 1000)
        If ToolStripProgressBar1.Value = 950 Then
            Timer1.Stop()
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked = True Then
            CheckBox5.Enabled = False
            CheckBox5.Checked = False
        Else
            CheckBox5.Enabled = True
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then
            CheckBox6.Enabled = False
            CheckBox6.Checked = False
        Else
            CheckBox6.Enabled = True
        End If
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        If Directory.Exists("log")
            For Each filei As String In Directory.GetFiles("log")
                File.Delete(filei)
            Next
        End If
        If CheckBox1.Checked = True And Directory.Exists("convtemp") Then
            For Each filei As String In Directory.GetFiles("convtemp")
                File.Delete(filei)
            Next
        End If
    End Sub

    Private Sub MP4Box_GUI()
        If File.Exists(Application.StartupPath & "\MP4Box-GUI.exe") Then
            Process.Start(Application.StartupPath & "\MP4Box-GUI.exe")
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If Directory.Exists(Path.GetDirectoryName(ListBox1.SelectedItem)) Then
            Process.Start("explorer.exe", Path.GetDirectoryName(ListBox1.SelectedItem))
        End If
    End Sub
    Private prev_checked3 As Boolean

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged

        If CheckBox7.Checked Then
            prev_checked3 = CheckBox3.Checked
            CheckBox3.Checked = False
            CheckBox3.Enabled = False
            CheckBox2.Enabled = False
            ComboBox_mp3quality.Visible = True
        Else
            CheckBox3.Checked = prev_checked3
            CheckBox3.Enabled = True
            CheckBox2.Enabled = True
            ComboBox_mp3quality.Visible = False
        End If
    End Sub

    Private Sub ComboBox_mp3quality_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_mp3quality.SelectedIndexChanged
        Select Case ComboBox_mp3quality.SelectedIndex
            Case 0
                mp3quality = ""
            Case 1
                mp3quality = "-b:a 64K "
            Case 2
                mp3quality = "-b:a 128K "
            Case 3
                mp3quality = "-b:a 192K "
            Case 4
                mp3quality = "-b:a 256K "
            Case 5
                mp3quality = "-b:a 384K "
            Case 5
                mp3quality = "-b:a 512K "
        End Select

    End Sub
End Class
