Imports System.Runtime.InteropServices

Public Class Form1
    'Declare the variables
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer
    Dim locked As Boolean = False
    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As IntPtr
    Private Declare Auto Function GetWindowText Lib "user32" (ByVal hWnd As System.IntPtr, ByVal lpString As System.Text.StringBuilder, ByVal cch As Integer) As Integer
    Dim youtubeurl As String = "http://www.youtube.com/embed/[VIDEOID]?autohide=1&autoplay=1&fs=0&iv_load_policy=3&showinfo=0&rel=0&cc_load_policy=1"

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel2.MouseDown, Panel1.MouseDown
        If e.Button = MouseButtons.Left Then
            drag = True
            mousex = Windows.Forms.Cursor.Position.X - Me.Left
            mousey = Windows.Forms.Cursor.Position.Y - Me.Top
        End If
    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel2.MouseMove, Panel1.MouseMove
        If drag Then
            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub
    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel2.MouseUp, Panel1.MouseUp
        drag = False
    End Sub

    Private Sub Form1_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave
        Me.BringToFront()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
        Me.Capture = True
        WebBrowser1.Navigate("http://cyanlabs.co.uk/external/floatingbrowser.html")

    End Sub

    Private Sub NsTrackBar1_Scroll(sender As Object) Handles NsTrackBar1.Scroll
        Me.Opacity = NsTrackBar1.Value / 100
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size
        NsTrackBar1.Value = Me.Opacity * 100
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim pt As POINTAPI
        pt.x = -1
        pt.y = -1
        GetCursorPos(pt)
        If pt.x < Location.X - 10 Or pt.y < Location.Y Or pt.x > Location.X + Width - 1 Or pt.y > Location.Y + Height - 1 Then
            Me.Height = 248
            Panel2.Visible = False
            Panel1.Visible = False
        Else
            Me.Height = 300
            Panel2.Visible = True
            Panel1.Visible = True
        End If
    End Sub
    <DllImport("User32.dll")> _
    Public Shared Function GetCursorPos(ByRef pt As POINTAPI) As Integer
    End Function

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure POINTAPI
        Public x As Integer
        Public y As Integer
    End Structure

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Const WS_EX_TOPMOST As Integer = &H8
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or WS_EX_TOPMOST
            Return cp
        End Get
    End Property
    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            e.Handled = True
            ParseURI(TextBox1.Text)

        End If
    End Sub

    Sub ParseURI(ByVal uri As String)

        If uri.Contains("youtube.com/watch?v=") Then
            uri = uri.Substring(uri.IndexOf("=") + 1)
            If uri.Length > 11 Then uri = uri.Substring(0, uri.IndexOf("&"))
            uri = "http://www.youtube.com/embed/" & uri & "?autohide=1&autoplay=1&fs=0&iv_load_policy=3&showinfo=0&rel=0&cc_load_policy=1"

            'Dim ele = (From X In WebBrowser1.Document.GetElementsByTagName("div").Cast(Of HtmlElement)() Where X.GetAttribute("id") = "player-api" Select X).FirstOrDefault
            'If ele IsNot Nothing Then ele.Style = "Display:none"
            'MsgBox(uri)
            'WebBrowser1.DocumentText = WebBrowser1.Document.GetElementById("player-api").OuterHtml.Replace("<div id=""player-api""", "<div id=""player-api"" style=""width:410px;height:249px""")
            'MsgBox(WebBrowser1.Document.GetElementById("player-api").InnerHtml)
        ElseIf uri = "about:floatingbrowser" Then
            uri = "http://cyanlabs.co.uk/external/floatingbrowser.html"
        End If
        WebBrowser1.Navigate(uri)
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        e.SuppressKeyPress = (e.KeyCode = Keys.Enter)
    End Sub
End Class
