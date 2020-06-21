using ScottPlot.Demo.WPF.WpfDemos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScottPlot.Reactive.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double[] data = new double[100_000];

        public MainWindow()
        {
            InitializeComponent();

            var one = new DoubleModel(
                wpfPlot1,
             DataSource.ObserveValues(),
                Observable.Interval(TimeSpan.FromMilliseconds(150)).ObserveOnDispatcher().Select(a => Unit.Default));

            var two = new OHLCModel(
                wpfPlot2,
                DataSource.ObserveOHLCValues(),
                Observable.Interval(TimeSpan.FromMilliseconds(1000)).ObserveOnDispatcher().Select(a => Unit.Default));

            var three = new DoubleModel(wpfPlot3, DataSource.ObserveValues());

            PlotView1.Data = DataGen.RandomStockPrices(new Random(), 200, sequential: true);
        }
    }
}
