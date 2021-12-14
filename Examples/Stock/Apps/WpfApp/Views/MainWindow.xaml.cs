using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Stock.Domain;
using Stock.Infrastructure.Logger;
using Stock.Infrastructure.Mailer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Providers;
using WpfApp.ViewModels;

namespace WpfApp.Views
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StockManager _stockManager = new();
        private Dictionary<string, TickData> _dictionary = new();

        private Timer? _timer;
        private bool _updated = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // IoC: injecting services
            Services.Logger = new ConsoleLogger(this.GetType()); // new SeriLogger(); //
            Services.Mailer = new SmtpMailer
            {
                Host = "smtp.telenet.be",
                Port = 587,
                // User = ""
                // Password = ""
            };

            Services.Mailer.Send("lcvervoort@telenet.be", new string[] { "luc.vervoort@gmail.com" }, "Mijn titel", "<html><body><H1>Test</H1><p>Since the days of Louis Daguerre the principle of photography has remained the same.</p></body></html>");



            lblCursorPosition.Text = "[" + DateTime.Now.ToString() + "] Loading shares...";

            // De volgende aandelen moeten uit de databank komen via EF:
            _stockManager.Shares.Add("ABI.BR", new ShareData { Count = 160, Value = 150 });
            _stockManager.Shares.Add("ALC", new ShareData { Count = 40, Value = 0 });
            _stockManager.Shares.Add("AAPL", new ShareData { Count = 280, Value = 0 });            
            _stockManager.Shares.Add("MTS.MC", new ShareData { Count = 333, Value = 0 });
            _stockManager.Shares.Add("ASML.AS", new ShareData { Count = 345, Value = 0 });
            _stockManager.Shares.Add("BNB.BR", new ShareData { Count = 14, Value = 0 });
            _stockManager.Shares.Add("CFEB.BR", new ShareData { Count = 60, Value = 0 });
            _stockManager.Shares.Add("COLR.BR", new ShareData { Count = 500, Value = 0 });
            _stockManager.Shares.Add("FLUX.BR", new ShareData { Count = 500, Value = 0 });
            _stockManager.Shares.Add("GBLB.BR", new ShareData { Count = 110, Value = 0 });
            _stockManager.Shares.Add("INTC", new ShareData { Count = 800, Value = 0 });
            _stockManager.Shares.Add("KBC.BR", new ShareData { Count = 100, Value = 0 });
            _stockManager.Shares.Add("MSFT", new ShareData { Count = 150, Value = 0 });
            _stockManager.Shares.Add("NOVN.SW", new ShareData { Count = 200, Value = 0 });
            _stockManager.Shares.Add("OSPN", new ShareData { Count = 500, Value = 0 });
            _stockManager.Shares.Add("PROX.BR", new ShareData { Count = 250, Value = 0 });
            _stockManager.Shares.Add("REPYY", new ShareData { Count = 333, Value = 0 });
            _stockManager.Shares.Add("SIP.BR", new ShareData { Count = 240, Value = 0 });
            _stockManager.Shares.Add("SOLB.BR", new ShareData { Count = 137, Value = 0 });
            _stockManager.Shares.Add("TNET.BR", new ShareData { Count = 500, Value = 0 });
            _stockManager.Shares.Add("UCB.BR", new ShareData { Count = 500, Value = 0 });

            // Poor man's IoC:
            _stockManager.StockQueryProvider = new StockQueryProvider();

            // In plaats van een vaste datum mee te geven, moet de datum die de gebruiker opgeeft, genomen wordt
            var result = _stockManager.GetHistoricalData(new System.DateTime(2021, 7, 1), System.DateTime.Now.Date);
            if (DataContext is ViewModel viewModel)
            {
                // WPF .NET 5 en hoger: om een control te gebruiken, moeten we deze opzoeken met FindName():
                if (this.FindName("StockTabControl") is TabControl stockTabControl)
                {
                    // Show portfolio evolution
                    var portfolioTab = new TabItem() { Header = "Portfolio" };

                    Grid portfolioGrid = new();
                    portfolioGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    portfolioGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    portfolioGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                    // Tick details
                    {

                        TickData tickData = new();
                        tickData.Currency.Text = "EUR".PadRight(64); // eindopdracht: dit moet configureerbaar zijn (vb. EUR, USD, ...)
                        tickData.Name.Text = "My Portfolio".PadRight(155); // moet configureerbaar zijn                  
                        tickData.Container.Children.Add(tickData.Name);
                        tickData.Container.Children.Add(tickData.Currency);
                        tickData.Container.Children.Add(tickData.ExchangeName);
                        tickData.Container.Children.Add(tickData.MarketState);
                        tickData.Container.Orientation = Orientation.Horizontal; // beter: wijzigen naar Grid

                        _dictionary.Add("Portfolio", tickData);
                        portfolioGrid.Children.Add(tickData.Container);
                    }

                    // History: candle stick chart
                    {
                        LiveChartsCore.SkiaSharpView.WPF.CartesianChart candleStickChart = new();
                        candleStickChart.XAxes = viewModel.XAxesDate;
                        candleStickChart.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
                        var o = new ObservableCollection<CandlesticksSeries<FinancialPoint>>();
                        var v = new ObservableCollection<FinancialPoint>();
                        var css = new CandlesticksSeries<FinancialPoint>
                        {
                            Values = v
                        };
                        o.Add(css);
                        candleStickChart.Series = o;
                        candleStickChart.SetValue(Grid.RowProperty, 1); // we plaatsen de chart op rij 1; vergelijk xaml: Grid.Row="1"
                        portfolioGrid.Children.Add(candleStickChart);
                        _dictionary["Portfolio"].CandleStickChart = candleStickChart;
                    }

                    // Current tick value
                    {
                        LiveChartsCore.SkiaSharpView.WPF.CartesianChart lineChart = new();
                        lineChart.XAxes = viewModel.XAxesTime;
                        lineChart.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
                        var o = new ObservableCollection<LineSeries<DateTimePoint>>();
                        var v = new ObservableCollection<DateTimePoint>();
                        var css = new LineSeries<DateTimePoint>
                        {
                            Values = v,
                            Fill = null
                        };
                        o.Add(css);
                        lineChart.Series = o;
                        lineChart.SetValue(Grid.RowProperty, 2);
                        portfolioGrid.Children.Add(lineChart);
                        _dictionary["Portfolio"].LineChart = lineChart;
                    }
                    portfolioTab.Content = portfolioGrid;
                    stockTabControl.Items.Add(portfolioTab);

                    // -----------------------------------------------------------------------------------
                    // We starten met de visualisatie van alle afzonderlijke aandelen van de portefeuille:
                    // -----------------------------------------------------------------------------------

                    // Show tick evolutions
                    foreach (var s in result)
                    {
                        var tab = new TabItem() { Header = s.Key };

                        Grid grid = new();
                        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                        grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                        grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                        // Tick details
                        {

                            TickData tickData = new();
                            tickData.Container.Orientation = Orientation.Horizontal;
                            tickData.Container.Children.Add(tickData.Name);
                            tickData.Container.Children.Add(tickData.Currency);
                            tickData.Container.Children.Add(tickData.ExchangeName);
                            tickData.Container.Children.Add(tickData.MarketState);

                            _dictionary.Add(s.Key, tickData);
                            grid.Children.Add(tickData.Container);
                        }

                        // History: candle stick chart
                        {
                            LiveChartsCore.SkiaSharpView.WPF.CartesianChart candleStickChart = new();
                            candleStickChart.XAxes = viewModel.XAxesDate;
                            candleStickChart.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
                            var o = new ObservableCollection<CandlesticksSeries<FinancialPoint>>();
                            var v = new ObservableCollection<FinancialPoint>();
                            foreach (var fv in s.Value)
                            {
                                v.Add(new FinancialPoint { Date = fv.DateTime.Date, Close = (double)fv.Close, Low = (double)fv.Low, High = (double)fv.High, Open = (double)fv.Open });

                            }
                            var css = new CandlesticksSeries<FinancialPoint>
                            {
                                Values = v
                            };
                            o.Add(css);
                            candleStickChart.Series = o;
                            candleStickChart.SetValue(Grid.RowProperty, 1);
                            grid.Children.Add(candleStickChart);
                            _dictionary[s.Key].CandleStickChart = candleStickChart;
                        }

                        // Current tick value
                        {
                            LiveChartsCore.SkiaSharpView.WPF.CartesianChart lineChart = new();
                            lineChart.XAxes = viewModel.XAxesTime;
                            lineChart.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
                            var o = new ObservableCollection<LineSeries<DateTimePoint>>();
                            var v = new ObservableCollection<DateTimePoint>();
                            var css = new LineSeries<DateTimePoint>
                            {
                                Values = v,
                                Fill = null
                            };
                            o.Add(css);
                            lineChart.Series = o;
                            lineChart.SetValue(Grid.RowProperty, 2);
                            grid.Children.Add(lineChart);
                            _dictionary[s.Key].LineChart = lineChart;
                        }
                        tab.Content = grid;
                        stockTabControl.Items.Add(tab);
                    }
                }
            }

            _timer = new Timer(30000); // Timer loopt af elke 30 seconden - 30000 milliseconden
            // To add the elapsed event handler:
            // ... Type "_timer.Elapsed += " and press tab twice.            
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true; // Activeert de timer bij het starten
            _timer.Start();

            // Execute update immediately, at startup time:
            _timer_Elapsed(null, null);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Execute(sender, e);
        }

        private void Execute(object sender, EventArgs e)
        {
            var result = _stockManager.GetSecurities();
            foreach (var s in result)
            {
                if (_updated && s.Value.MarketState.ToUpper() == "CLOSED")
                {
                    System.Diagnostics.Debug.WriteLine("Market is closed: no need to update");
                    continue;
                }
                // -------------------------------------------------------
                // runs in a thread, so need to switch to main WPF thread:
                // -------------------------------------------------------
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    // We delegeren deze code naar de WPF thread!
                    // TODO: if market is closed, do not update
                    if (_dictionary[s.Key].LineChart.Series is ObservableCollection<LineSeries<DateTimePoint>> chart)
                    {
                        if (chart[0].Values is ObservableCollection<DateTimePoint> v)
                        {
                            v.Add(new DateTimePoint(DateTime.Now, s.Value.RegularMarketPrice));
                        }

                        // Do not update gui if not needed!
                        if(_dictionary[s.Key].Name.Text != s.Value.LongName.PadRight(155))
                            _dictionary[s.Key].Name.Text = s.Value.LongName.PadRight(155);
                        if (_dictionary[s.Key].Currency.Text != s.Value.Currency.PadRight(64)) 
                            _dictionary[s.Key].Currency.Text = s.Value.Currency.PadRight(64);
                        if (_dictionary[s.Key].ExchangeName.Text != s.Value.FullExchangeName.PadRight(100))
                            _dictionary[s.Key].ExchangeName.Text = s.Value.FullExchangeName.PadRight(100);
                        if (_dictionary[s.Key].MarketState.Text != s.Value.MarketState.PadRight(25))
                            _dictionary[s.Key].MarketState.Text = s.Value.MarketState.PadRight(25);
                    }
                }));
            }
            _updated = true;
        }
    }
}

