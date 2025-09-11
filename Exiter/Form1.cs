namespace Exiter;


using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public partial class Form1 : Form
{
    private NotifyIcon trayIcon;
    private ContextMenuStrip trayMenu;
    private Timer moveTimer;
    private bool toggle;

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public uint type;
        public MOUSEINPUT mi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public nint dwExtraInfo;
    }

    private const uint INPUT_MOUSE = 0;
    private const uint MOUSEEVENTF_MOVE = 0x0001;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    public Form1()
    {
        InitializeComponent();
        this.WindowState = FormWindowState.Minimized;
        this.ShowInTaskbar = false;
        this.Visible = false;

        trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("Exit", null, OnExit);

        trayIcon = new NotifyIcon();
        trayIcon.Text = "Exiter";
        try
        {
            trayIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? SystemIcons.Application;
        }
        catch
        {
            trayIcon.Icon = SystemIcons.Application;
        }
        trayIcon.ContextMenuStrip = trayMenu;
        trayIcon.Visible = true;

        moveTimer = new Timer();
        moveTimer.Interval = 30000; // 30 seconds
        moveTimer.Tick += MoveCursorJiggle;
        moveTimer.Start();


        // Hide the form immediately
        this.Hide();
    }

    protected override void SetVisibleCore(bool value)
    {
        base.SetVisibleCore(false);
    }

    private void OnExit(object? sender, EventArgs e)
    {
        moveTimer?.Stop();
        trayIcon.Visible = false;
        trayIcon.Dispose();
        Application.Exit();
    }

    private void MoveCursorJiggle(object? sender, EventArgs e)
    {
        int delta = toggle ? 1 : -1;
        toggle = !toggle;

        var inputs = new[]
        {
            new INPUT { type = INPUT_MOUSE, mi = new MOUSEINPUT { dx = delta, dy = 0, dwFlags = MOUSEEVENTF_MOVE } },
            new INPUT { type = INPUT_MOUSE, mi = new MOUSEINPUT { dx = -delta, dy = 0, dwFlags = MOUSEEVENTF_MOVE } }
        };

        SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        this.Hide();
    }
}
