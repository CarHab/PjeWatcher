using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using Watcher.Services;

namespace Watcher;

public partial class MainWindow : Window
{
    public Retriever? DataAccess { get; set; }
    public string CaseNumber { get; set; } = "";
    public DispatcherTimer? DispatcherTimer { get; set; } = null;
    private bool _first = true;
    private string _currentText = "";
    private NotifyIcon MyNotifyIcon;
    private readonly bool _backgroundProcess;

    public MainWindow(string caseNumber, bool backgroundProcess)
    {
        InitializeComponent();

        _backgroundProcess = backgroundProcess;

        MyNotifyIcon = new System.Windows.Forms.NotifyIcon();

        MyNotifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(
         System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name);
        MyNotifyIcon.Visible = true;

        MyNotifyIcon.MouseDoubleClick +=
            new System.Windows.Forms.MouseEventHandler
                (MyNotifyIcon_MouseDoubleClick);

        Left = SystemParameters.WorkArea.Width - Width;
        Top = SystemParameters.WorkArea.Height - Height;

        CaseNumber = caseNumber;
        Start();
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    private const int GWL_EX_STYLE = -20;
    private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;

    private void Start()
    {
        DispatcherTimer?.Stop();

        DataAccess = new(CaseNumber);
        DispatcherTimer = new();
        DispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
        DispatcherTimer.Start();
        DispatcherTimer.Interval = new TimeSpan(0, 1, 0);
    }

    private void DispatcherTimer_Tick(object sender, EventArgs e)
    {
        GetText();

        // Forcing the CommandManager to raise the RequerySuggested event
        CommandManager.InvalidateRequerySuggested();
    }

    public async Task GetText()
    {
        string currentTime = DateTime.Now.ToShortTimeString();
        try
        {
            string responseText = await DataAccess.GetText();

            if (Settings.NotifyEmail && !_first && responseText != _currentText)
            {
                MailService mailService = new(responseText);
                await mailService.SendEmail();
            }

            if (Settings.NotifyDesktop && !_first && responseText != _currentText)
            {
                new ToastContentBuilder()
                .AddButton(new ToastButton()
                    .SetContent("Abrir")
                    .SetProtocolActivation(new Uri(Settings.GetLastLink())))
                .AddText("PJE Watcher")
                .AddText($"Nova atualização: {responseText}")
                .Show();
            }

            _currentText = responseText;
            _first = false;
            string fullText = $"{currentTime} - {responseText}";

            Output.Text = fullText;
        }
        catch (Exception e)
        {
            string fullText = $"{currentTime} - {e.Message}";

            Output.Text = fullText;
        }
    }


    private void MyNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        WindowState = WindowState.Normal;
    }

    private void Root_StateChanged(object sender, EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            ShowInTaskbar = false;
            MyNotifyIcon.Visible = true;
        }
        else if (WindowState == WindowState.Normal)
        {
            MyNotifyIcon.Visible = false;
            ShowInTaskbar = true;
        }
    }

    private void Root_Loaded(object sender, RoutedEventArgs e)
    {
        if (_backgroundProcess)
            WindowState = WindowState.Minimized;

        var helper = new WindowInteropHelper(this).Handle;
        SetWindowLong(helper, GWL_EX_STYLE, (GetWindowLong(helper, GWL_EX_STYLE) | WS_EX_TOOLWINDOW));
    }

    private void Root_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }
}


