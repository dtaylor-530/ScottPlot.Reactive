using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for PlotView.xaml
    /// </summary>
    public partial class PlotView : UserControl
    {
        public PlotView()
        {
            InitializeComponent();
        }


        public OHLC[] Data
        {
            get { return (OHLC[])GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(OHLC[]), typeof(PlotView), new PropertyMetadata(null,Changed ));

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlotView plotView &&  e.NewValue is OHLC[] data)
            {
               _ = plotView.wpfPlot1.plt.PlotCandlestick(data);

                plotView.wpfPlot1.Render();
            }
        }
    }
}
