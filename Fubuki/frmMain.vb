Imports System.IO
Imports System.Net
Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports ComponentOwl.BetterListView

Public Class frmMain
    Dim chk As Integer = 0, clipb As String, IDMan As String
    Dim dl As Boolean = False
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Clipboard.Clear()
        If Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\iTNz-NETWORK\Fubuki", "IDMan", Nothing) <> vbNullString Then
            IDMan = Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\iTNz-NETWORK\Fubuki", "IDMan", Nothing)
            DownloadSelectedWithIDMToolStripMenuItem.Enabled = True
            dl = True
        Else
            SettingsToolStripMenuItem_Click(sender, e)
        End If
    End Sub
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Try
            If ToolStripTextBox1.Text.StartsWith("http://forum.tirkx.com/main/showthread.php?") Or ToolStripTextBox1.Text.StartsWith("http://elise.tirkx.com/main/showthread.php?") Then

                Dim myData(3) As String
                Dim listType, listFS As Boolean
                Dim web As New WebClient

                web.Encoding = System.Text.Encoding.UTF8

                web.Headers("Cookie") = "bb_userid=204348; bb_password=2ceebcc570b39f2304d1cf973b427f22;"
                Dim Data() As String = web.DownloadString("http://forum.tirkx.com/main/tirkx_load_anime_per_topic.php?aid=" + Regex.Match(ToolStripTextBox1.Text, "\d+").Groups(0).Value).Split("{")

                ToolStripLabel1.Visible = False
                ToolStripButton3.Visible = False
                ToolStripTextBox1.Visible = False
                ToolStripButton1.Visible = False
                ToolStripSeparator1.Visible = False

                ToolStripComboBox1.Items.Clear()
                ToolStripComboBox2.Items.Clear()
                BetterListView1.Items.Clear()
                BetterListView1.BeginUpdate()
                For i = 1 To Data.Length - 1
                    listType = False
                    listFS = False

                    'Data(i).Split("}")(0)
                    myData(0) = Regex.Split(Regex.Split(Data(i), "},")(0), """phpbb_link"":""")(1).Split("""")(0) 'phpbb_link
                    myData(1) = Regex.Split(Regex.Split(Data(i), "},")(0), """fname"":""")(1).Replace("}", "]").Split("""")(0) 'fname
                    myData(2) = Regex.Split(Regex.Split(Data(i), "},")(0), """fansubName"":""")(1).Split("""")(0) 'fansubName
                    myData(3) = Regex.Split(Regex.Split(Data(i), "},")(0), """type"":""")(1).Split("""")(0) 'type


                    For idxType = 0 To ToolStripComboBox1.Items.Count - 1
                        If myData(3) = ToolStripComboBox1.Items(idxType) Then listType = True
                    Next
                    If listType = False Then
                        ToolStripComboBox1.Items.Add(myData(3))
                    End If

                    For idxFS = 0 To ToolStripComboBox2.Items.Count - 1
                        If myData(2) = ToolStripComboBox2.Items(idxFS) Then listFS = True
                    Next
                    If listFS = False Then
                        ToolStripComboBox2.Items.Add(myData(2))
                    End If

                    BetterListView1.Items.Add(New BetterListViewItem({myData(3), myData(1), myData(0).Replace("\", "")}))
                    If myData(3) = "EN" Then
                        BetterListView1.Items(i - 1).ForeColor = Color.Blue
                        BetterListView1.Items(i - 1).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_blue
                    ElseIf myData(3) = "TH" Then
                        BetterListView1.Items(i - 1).ForeColor = Color.Green
                        BetterListView1.Items(i - 1).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_green
                    ElseIf myData(3) = "AU" Then
                        BetterListView1.Items(i - 1).ForeColor = Color.Red
                        BetterListView1.Items(i - 1).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_red
                    Else
                        BetterListView1.Items(i - 1).ForeColor = Color.Gray
                        BetterListView1.Items(i - 1).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_gray
                    End If

                Next
                BetterListView1.EndUpdate()
                If BetterListView1.VScrollBarVisible = True Then
                    BetterListView1.Columns(1).Width = BetterListView1.Width - BetterListView1.Columns(0).Width - 3 - SystemInformation.VerticalScrollBarWidth
                Else
                    BetterListView1.Columns(1).Width = BetterListView1.Width - BetterListView1.Columns(0).Width - 3
                End If

                ToolStripLabel2.Visible = True
                ToolStripComboBox1.Visible = True
                ToolStripComboBox2.Visible = True
                ToolStripButton2.Visible = True
                ToolStripSeparator2.Visible = True
                ToolStripButton3.Visible = True
                Timer1.Enabled = False
            Else
                MessageBox.Show("URL Format http://forum.tirkx.com/main/showthread.php?" + vbCrLf + "URL Format http://elise.tirkx.com/main/showthread.php?", "Msgbox", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Msgbox", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged
        Try
            If ToolStripTextBox1.Text.StartsWith("http://forum.tirkx.com/main/showthread.php?") Or ToolStripTextBox1.Text.StartsWith("http://elise.tirkx.com/main/showthread.php?") Then
                Dim myData(3) As String, flag As Integer = 0
                Dim listType, listFS As Boolean
                Dim web As New WebClient
                web.Encoding = System.Text.Encoding.UTF8

                web.Headers("Cookie") = "bb_userid=204348; bb_password=2ceebcc570b39f2304d1cf973b427f22;"
                Dim Data() As String = web.DownloadString("http://forum.tirkx.com/main/tirkx_load_anime_per_topic.php?aid=" + Regex.Match(ToolStripTextBox1.Text, "\d+").Groups(0).Value).Split("{")


                BetterListView1.Items.Clear()
                BetterListView1.BeginUpdate()
                For i = 1 To Data.Length - 1
                    listType = False
                    listFS = False

                    myData(0) = Regex.Split(Regex.Split(Data(i), "},")(0), """phpbb_link"":""")(1).Split("""")(0) 'phpbb_link
                    myData(1) = Regex.Split(Regex.Split(Data(i), "},")(0), """fname"":""")(1).Replace("}", "]").Split("""")(0) 'fname
                    myData(2) = Regex.Split(Regex.Split(Data(i), "},")(0), """fansubName"":""")(1).Split("""")(0) 'fansubName
                    myData(3) = Regex.Split(Regex.Split(Data(i), "},")(0), """type"":""")(1).Split("""")(0) 'type

                    If ToolStripComboBox1.Items(ToolStripComboBox1.SelectedIndex).ToString() = myData(3) Then
                        BetterListView1.Items.Add(New BetterListViewItem({myData(3), myData(1), myData(0).Replace("\", "")}))
                        If myData(3) = "EN" Then
                            BetterListView1.Items(flag).ForeColor = Color.Blue
                            BetterListView1.Items(flag).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_blue
                        ElseIf myData(3) = "TH" Then
                            BetterListView1.Items(flag).ForeColor = Color.Green
                            BetterListView1.Items(flag).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_green
                        ElseIf myData(3) = "AU" Then
                            BetterListView1.Items(flag).ForeColor = Color.Red
                            BetterListView1.Items(flag).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_red
                        Else
                            BetterListView1.Items(flag).ForeColor = Color.Gray
                            BetterListView1.Items(flag).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_gray
                        End If
                        flag += 1
                    End If

                Next
                BetterListView1.EndUpdate()
                If BetterListView1.VScrollBarVisible = True Then
                    BetterListView1.Columns(1).Width = BetterListView1.Width - BetterListView1.Columns(0).Width - 3 - SystemInformation.VerticalScrollBarWidth
                Else
                    BetterListView1.Columns(1).Width = BetterListView1.Width - BetterListView1.Columns(0).Width - 3
                End If
            Else
                MessageBox.Show("URL Format http://forum.tirkx.com/main/showthread.php?" + vbCrLf + "URL Format http://elise.tirkx.com/main/showthread.php?", "Msgbox", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Msgbox", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ToolStripComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox2.SelectedIndexChanged
        Try
            If ToolStripTextBox1.Text.StartsWith("http://forum.tirkx.com/main/showthread.php?") Or ToolStripTextBox1.Text.StartsWith("http://elise.tirkx.com/main/showthread.php?") Then
                Dim myData(3) As String, flag As Integer = 0
                Dim listType, listFS As Boolean
                Dim web As New WebClient
                web.Encoding = System.Text.Encoding.UTF8

                web.Headers("Cookie") = "bb_userid=204348; bb_password=2ceebcc570b39f2304d1cf973b427f22;"
                Dim Data() As String = web.DownloadString("http://forum.tirkx.com/main/tirkx_load_anime_per_topic.php?aid=" + Regex.Match(ToolStripTextBox1.Text, "\d+").Groups(0).Value).Split("{")
                BetterListView1.Items.Clear()
                BetterListView1.BeginUpdate()
                For i = 1 To Data.Length - 1
                    listType = False
                    listFS = False

                    myData(0) = Regex.Split(Regex.Split(Data(i), "},")(0), """phpbb_link"":""")(1).Split("""")(0) 'phpbb_link
                    myData(1) = Regex.Split(Regex.Split(Data(i), "},")(0), """fname"":""")(1).Replace("}", "]").Split("""")(0) 'fname
                    myData(2) = Regex.Split(Regex.Split(Data(i), "},")(0), """fansubName"":""")(1).Split("""")(0) 'fansubName
                    myData(3) = Regex.Split(Regex.Split(Data(i), "},")(0), """type"":""")(1).Split("""")(0) 'type

                    If ToolStripComboBox2.Items(ToolStripComboBox2.SelectedIndex).ToString() = myData(2) Then
                        BetterListView1.Items.Add(New BetterListViewItem({myData(3), myData(1), myData(0).Replace("\", "")}))

                        If myData(3) = "EN" Then
                            BetterListView1.Items(flag).ForeColor = Color.Blue
                            BetterListView1.Items(flag).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_blue
                        ElseIf myData(3) = "TH" Then

                            BetterListView1.Items(flag).ForeColor = Color.Green
                            BetterListView1.Items(flag).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_green
                        ElseIf myData(3) = "AU" Then
                            BetterListView1.Items(flag).ForeColor = Color.Red
                            BetterListView1.Items(flag).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_red
                        Else
                            BetterListView1.Items(flag).ForeColor = Color.Gray
                            BetterListView1.Items(flag).SubItems(1).Image = Fubuki.My.Resources.Resources.square_small_gray
                        End If
                        flag += 1
                    End If
                Next
                BetterListView1.EndUpdate()
                If BetterListView1.VScrollBarVisible = True Then
                    BetterListView1.Columns(1).Width = BetterListView1.Width - BetterListView1.Columns(0).Width - 3 - SystemInformation.VerticalScrollBarWidth
                Else
                    BetterListView1.Columns(1).Width = BetterListView1.Width - BetterListView1.Columns(0).Width - 3
                End If
            Else
                MessageBox.Show("URL Format http://forum.tirkx.com/main/showthread.php?" + vbCrLf + "URL Format http://elise.tirkx.com/main/showthread.php?", "Msgbox", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Msgbox", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub ToolStripTextBox1_TextChanged(sender As Object, e As EventArgs)
    'BetterListView1.FindItemWithText(ToolStripTextBox1.Text)
    'End Sub

    'Private Sub Button1_Click(sender As Object, e As EventArgs)
    '    Dim myData(5) As String
    '    Dim web As New WebClient
    '    web.Headers("Cookie") = "bb_userid=204348; bb_password=2ceebcc570b39f2304d1cf973b427f22;"
    '    Dim Data() As String = web.DownloadString("http://forum.tirkx.com/main/tirkx_anime_list_home.php").Split("`")
    '    Dim x = web.DownloadString("http://forum.tirkx.com/main/tirkx_load_anime_per_topic.php?aid=182220&tirkx_locate=58089BD18B&tirkx_watch_anime=")
    '    'Dim Data() As String = web.DownloadString("http://forum.tirkx.com/main/tirkx_load_anime_per_topic.php?aid=182220&tirkx_locate=58089BD18B&tirkx_watch_anime=").Split("`")
    '    Me.Text = Data.Length
    '    'ListData(0) = Data(i).Split("`")(0) listall
    '    BetterListView1.Items.Clear()
    '    BetterListView1.BeginUpdate()
    '    For i = 0 To Data.Length - 2
    '        myData(0) = Data(i).Split("`")(0).Split("$")(0) 'locate
    '        myData(1) = Data(i).Split("`")(0).Split("$")(1) 'AnimeID
    '        myData(2) = Data(i).Split("`")(0).Split("$")(2) 'AnimeName
    '        myData(3) = Data(i).Split("`")(0).Split("$")(3) 'Type
    '        myData(4) = Data(i).Split("`")(0).Split("$")(4) 'watch_anime
    '        BetterListView1.Items.Add(New BetterListViewItem({myData(3), myData(2)}))
    '    Next
    '    BetterListView1.EndUpdate()
    'End Sub

    Private Sub BetterListView1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles BetterListView1.KeyPress
        Dim data As String = ""
        BetterListView1.BeginUpdate()
        'enter
        'If AscW(e.KeyChar) = 13 Then
        '    For i = 0 To BetterListView1.Count - 1
        '        If BetterListView1.Items(i).Selected Then
        '            data += BetterListView1.Items(i).SubItems(2).Text + vbCrLf
        '            Process.Start(IDMan, " /d """ & BetterListView1.Items(i).SubItems(2).Text).WaitForExit() 'Process.Start(BetterListView1.Items(i).SubItems(2).Text) 
        '        End If
        '    Next
        '    'MsgBox(data)
        '    'For Each item As BetterListViewItem In BetterListView1.SelectedItems
        '    'MsgBox(item.SubItems(1).Text)
        '    'Next
        'End If
        'alt+a
        If AscW(e.KeyChar) = 1 Then
            For i = 0 To BetterListView1.Count - 1
                BetterListView1.Items(i).Selected = True
            Next
        End If
        BetterListView1.EndUpdate()
    End Sub

    Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        On Error Resume Next
        If BetterListView1.VScrollBarVisible Then
            BetterListView1.Columns(1).Width = BetterListView1.Width - BetterListView1.Columns(0).Width - 3 - SystemInformation.VerticalScrollBarWidth
        Else
            BetterListView1.Columns(1).Width = BetterListView1.Width - BetterListView1.Columns(0).Width - 3
        End If
    End Sub

    Private Sub VisitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VisitToolStripMenuItem.Click
        Process.Start("https://www.facebook.com/groups/wixoss/")
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        BetterListView1.Items.Clear()
        ToolStripLabel2.Visible = False
        ToolStripComboBox1.Visible = False
        ToolStripComboBox2.Visible = False
        ToolStripButton2.Visible = False
        ToolStripButton3.Visible = False
        ToolStripSeparator2.Visible = False

        ToolStripLabel1.Visible = True
        ToolStripTextBox1.Visible = True
        ToolStripButton1.Visible = True
        ToolStripSeparator1.Visible = True
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If Clipboard.GetText.StartsWith("http://forum.tirkx.com/main/showthread.php?") Or Clipboard.GetText.StartsWith("http://elise.tirkx.com/main/showthread.php?") And chk = 0 Then
            chk = 1
            clipb = Clipboard.GetText
            'If MessageBox.Show("Automatic Paste", "Msgbox", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = vbYes Then
            ToolStripTextBox1.Text = clipb
            ToolStripButton1_Click(sender, e)
            'End If
            Clipboard.Clear()
            chk = 0
        End If
    End Sub

    Private Sub DownloadSelectedWithIDMToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownloadSelectedWithIDMToolStripMenuItem.Click
        If BetterListView1.Count <> 0 Then
            FolderBrowserDialog1.Description = "Select folder to savefile"
            If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                BetterListView1.BeginUpdate()
                For i = 0 To BetterListView1.Count - 1
                    If BetterListView1.Items(i).Selected Then
                        'Clipboard.SetText(IDMan & " /a /n /d """ & BetterListView1.Items(i).SubItems(2).Text & """")
                        'MsgBox(IDMan & " /a /n /d " & BetterListView1.Items(i).SubItems(2).Text)
                        Process.Start(IDMan, " /a /n /d """ & BetterListView1.Items(i).SubItems(2).Text & """ /p " + FolderBrowserDialog1.SelectedPath + "").WaitForExit()
                    End If
                Next
                BetterListView1.EndUpdate()
            End If
        End If
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        If BetterListView1.Count <> 0 Then
            SaveFileDialog1.Title = "Export to textfile"
            SaveFileDialog1.Filter = "Text Documents (*.txt)|*.txt"
            If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim data As String = "#" + ToolStripTextBox1.Text & vbCrLf
                For i = 0 To BetterListView1.Count - 1
                    data += BetterListView1.Items(i).SubItems(2).Text & vbCrLf
                Next
                File.WriteAllText(SaveFileDialog1.FileName, data)
            End If
        Else
            MsgBox("No data", vbExclamation, "MsgBox")
        End If
        'Shell("C:\Program Files\Internet Explorer\iexplore.exe http://amagumori.3dfxwave.com/Yuru%20Yuri%20San%20Hai!/[Amagumori%20&%20DAI-TAKU%20&%20LLP]%20Yuru%20Yuri%20San%20Hai!%20-%2002%20(TX%201280x720%20x264%20AAC).mp4")
    End Sub

    Private Sub BetterListView1_DoubleClick(sender As Object, e As EventArgs) Handles BetterListView1.DoubleClick
        If BetterListView1.Count <> 0 And dl = True Then
            Process.Start(IDMan, " /d """ & BetterListView1.Items(BetterListView1.SelectedIndices(0)).SubItems(2).Text).WaitForExit() 'Process.Start(BetterListView1.Items(i).SubItems(2).Text) 
        End If
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        Dim count As Integer = 0
        For i = 0 To BetterListView1.Count - 1
            If BetterListView1.Items(i).Selected Then
                count += 1
            End If
        Next
        DownloadSelectedWithIDMToolStripMenuItem.Text = "Download Queue (" + Str(count) + " selected) to IDM"
    End Sub

    Private Sub BetterListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles BetterListView1.SelectedIndexChanged

    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        OpenFileDialog1.Title = "Target to IDMan.exe"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "IDMan.exe|IDMan.exe"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            'MsgBox(OpenFileDialog1.FileName.Split("\")(OpenFileDialog1.FileName.Split("\").Length - 1))
            If File.Exists(OpenFileDialog1.FileName) And OpenFileDialog1.FileName.Split("\")(OpenFileDialog1.FileName.Split("\").Length - 1) = "IDMan.exe" Then
                Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\iTNz-NETWORK\Fubuki", "IDMan", OpenFileDialog1.FileName)
                DownloadSelectedWithIDMToolStripMenuItem.Enabled = True
                dl = True
                IDMan = Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\iTNz-NETWORK\Fubuki", "IDMan", Nothing)
            End If
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim aboutfrm As New frmAbout
        aboutfrm.ShowDialog()
    End Sub
End Class
