Public Class MultiSelListBox
    Inherits ListBox

    Private MouseDownOnIndex As Integer
    Private bMouseDownOnSelection As Boolean
    Private bMouseDownOutsideSelection As Boolean
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_LBUTTONUP As Integer = &H202
    Private Const WM_MOUSEMOVE As Integer = &H200
    Private Const MK_LBUTTON As Integer = &H1&

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_LBUTTONDOWN
                Dim pt As New Point(m.LParam.ToInt32)
                MouseDownOnIndex = Me.IndexFromPoint(pt)
                If Me.SelectedItems.Count >= 1 _
                   And Me.SelectedIndices.Contains(MouseDownOnIndex) _
                   And m.WParam = MK_LBUTTON Then
                    bMouseDownOnSelection = True
                    Return
                Else
                    bMouseDownOutsideSelection = True
                    MyBase.WndProc(m)
                End If
            Case WM_MOUSEMOVE
                If bMouseDownOnSelection Then
                    Me.DoDragDrop(Me.SelectedItems, DragDropEffects.Move)
                End If
                bMouseDownOnSelection = False
                MyBase.WndProc(m)
            Case WM_LBUTTONUP
                Dim pt As New Point(m.LParam.ToInt32)
                If MouseDownOnIndex = Me.IndexFromPoint(pt) _
                   And m.WParam = 0 And Not bMouseDownOutsideSelection Then
                    Dim down As New Message()
                    down.HWnd = m.HWnd
                    down.Msg = WM_LBUTTONDOWN
                    down.WParam = m.WParam
                    down.LParam = m.LParam
                    down.Result = IntPtr.Zero
                    MyBase.WndProc(down)
                End If
                bMouseDownOutsideSelection = False
                MyBase.WndProc(m)
            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub
End Class