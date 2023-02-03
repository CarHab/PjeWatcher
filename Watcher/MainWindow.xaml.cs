using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Watcher;

public partial class MainWindow : Window
{
    public Retriever? DataAccess { get; set; }
    public string CurrentUrl { get; set; } = "";
    public DispatcherTimer? DispatcherTimer { get; set; } = null;
    public MainWindow()
    {
        InitializeComponent();
        Left = SystemParameters.WorkArea.Width - Width;
        Top = SystemParameters.WorkArea.Height - Height;
        
    }

    private void TextChangedEventHandler(object sender, TextChangedEventArgs args)
    {
        CurrentUrl = InputBox.Text;
        DispatcherTimer?.Stop();

        if (CurrentUrl is not "")
        {
            DataAccess = new();
            DispatcherTimer = new();
            DispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            DispatcherTimer.Start();
            DispatcherTimer.Interval = new TimeSpan(0, 1, 0);
        }
    }

    private void DispatcherTimer_Tick(object sender, EventArgs e)
    {
        GetText();

        // Forcing the CommandManager to raise the RequerySuggested event
        CommandManager.InvalidateRequerySuggested();
    }

    public async void GetText()
    {
        string responseText = await DataAccess.GetText(CurrentUrl);
        string currentTime = DateTime.Now.ToShortTimeString();

        string fullText = $"{currentTime} - {responseText}";

        Output.Text = fullText;
    }
}
