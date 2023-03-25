using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V109.CacheStorage;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

    public MainWindow(string caseNumber, bool backgroundProcess)
    {
        InitializeComponent();
        Left = SystemParameters.WorkArea.Width - Width;
        Top = SystemParameters.WorkArea.Height - Height;
        CaseNumber = caseNumber;
        Start();
    }

    private void Start()
    {
        DispatcherTimer?.Stop();

        DataAccess = new(CaseNumber);
        DispatcherTimer = new();
        DispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
        DispatcherTimer.Start();
        DispatcherTimer.Interval = new TimeSpan(0, 0, 2);
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
}
