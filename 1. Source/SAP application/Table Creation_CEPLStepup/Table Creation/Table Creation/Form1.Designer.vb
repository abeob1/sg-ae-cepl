<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.txtSBOPass = New System.Windows.Forms.TextBox()
        Me.txtSBOUser = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cmbDBType = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.txtServer = New System.Windows.Forms.TextBox()
        Me.txtPass = New System.Windows.Forms.TextBox()
        Me.txtUser = New System.Windows.Forms.TextBox()
        Me.lblPass = New System.Windows.Forms.Label()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.txtSBOPass)
        Me.GroupBox1.Controls.Add(Me.txtSBOUser)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.cmbDBType)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.lblCompany)
        Me.GroupBox1.Controls.Add(Me.txtServer)
        Me.GroupBox1.Controls.Add(Me.txtPass)
        Me.GroupBox1.Controls.Add(Me.txtUser)
        Me.GroupBox1.Controls.Add(Me.lblPass)
        Me.GroupBox1.Controls.Add(Me.lblUser)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.btnConnect)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 14)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(505, 236)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Connection"
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.Color.DarkRed
        Me.Label3.Location = New System.Drawing.Point(240, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(257, 23)
        Me.Label3.TabIndex = 55
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(332, 204)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(119, 23)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "SQ to SO"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(332, 170)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(119, 23)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Fleet Management"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(134, 50)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 20)
        Me.TextBox1.TabIndex = 2
        '
        'txtSBOPass
        '
        Me.txtSBOPass.Location = New System.Drawing.Point(134, 109)
        Me.txtSBOPass.Name = "txtSBOPass"
        Me.txtSBOPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSBOPass.Size = New System.Drawing.Size(100, 20)
        Me.txtSBOPass.TabIndex = 4
        '
        'txtSBOUser
        '
        Me.txtSBOUser.Location = New System.Drawing.Point(134, 78)
        Me.txtSBOUser.Name = "txtSBOUser"
        Me.txtSBOUser.Size = New System.Drawing.Size(100, 20)
        Me.txtSBOUser.TabIndex = 3
        Me.txtSBOUser.Text = "manager"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(14, 112)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(88, 16)
        Me.Label13.TabIndex = 51
        Me.Label13.Text = "SBO Password"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(14, 78)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(88, 16)
        Me.Label14.TabIndex = 49
        Me.Label14.Text = "SBO Username"
        '
        'cmbDBType
        '
        Me.cmbDBType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbDBType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbDBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDBType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDBType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbDBType.Items.AddRange(New Object() {"MSSQL2005", "MSSQL2008"})
        Me.cmbDBType.Location = New System.Drawing.Point(134, 140)
        Me.cmbDBType.Name = "cmbDBType"
        Me.cmbDBType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbDBType.Size = New System.Drawing.Size(137, 22)
        Me.cmbDBType.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(14, 148)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 16)
        Me.Label2.TabIndex = 46
        Me.Label2.Text = "Database Type"
        '
        'lblCompany
        '
        Me.lblCompany.Location = New System.Drawing.Point(14, 50)
        Me.lblCompany.Name = "lblCompany"
        Me.lblCompany.Size = New System.Drawing.Size(56, 16)
        Me.lblCompany.TabIndex = 45
        Me.lblCompany.Text = "Company"
        '
        'txtServer
        '
        Me.txtServer.Location = New System.Drawing.Point(134, 16)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(100, 20)
        Me.txtServer.TabIndex = 0
        Me.txtServer.Text = "(local)"
        '
        'txtPass
        '
        Me.txtPass.Location = New System.Drawing.Point(134, 209)
        Me.txtPass.Name = "txtPass"
        Me.txtPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPass.Size = New System.Drawing.Size(100, 20)
        Me.txtPass.TabIndex = 7
        '
        'txtUser
        '
        Me.txtUser.Location = New System.Drawing.Point(134, 177)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(100, 20)
        Me.txtUser.TabIndex = 6
        Me.txtUser.Text = "sa"
        '
        'lblPass
        '
        Me.lblPass.Location = New System.Drawing.Point(14, 209)
        Me.lblPass.Name = "lblPass"
        Me.lblPass.Size = New System.Drawing.Size(112, 16)
        Me.lblPass.TabIndex = 44
        Me.lblPass.Text = "Database Password"
        '
        'lblUser
        '
        Me.lblUser.Location = New System.Drawing.Point(14, 177)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(112, 16)
        Me.lblUser.TabIndex = 41
        Me.lblUser.Text = "Database Username"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(14, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 16)
        Me.Label1.TabIndex = 39
        Me.Label1.Text = "Database Server"
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(332, 138)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(119, 23)
        Me.btnConnect.TabIndex = 8
        Me.btnConnect.Text = "Connect"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(513, 262)
        Me.Controls.Add(Me.GroupBox1)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Table Creations"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents txtSBOPass As System.Windows.Forms.TextBox
    Friend WithEvents txtSBOUser As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents cmbDBType As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblCompany As System.Windows.Forms.Label
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents txtPass As System.Windows.Forms.TextBox
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents lblPass As System.Windows.Forms.Label
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button

End Class
