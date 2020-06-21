using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows;


namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for LiveDataGrowing.xaml
    /// </summary>
    public partial class DoubleModel
    {
        private double[] data = new double[100_000];
        int index = 0;
        private readonly SynchronizationContext context;
        private readonly WpfPlot wpfPlot;

        public DoubleModel(WpfPlot wpfPlot, IObservable<double> dataObservable, IObservable<Unit> renderObservable = null)
        {
            context = SynchronizationContext.Current;
            this.wpfPlot = wpfPlot;
            var signal = wpfPlot.plt.PlotSignal(data);
            signal.maxRenderIndex = 1;

            dataObservable.Subscribe(d =>
            {
                signal.maxRenderIndex = index;
                data[index++] = d;


            });

            (renderObservable ?? dataObservable.Select(a => Unit.Default))
                          .Subscribe(a =>
            {
                context.Send(a => Render(), null);
            });
        }

        void Render()
        {
            //if (AutoAxisCheckbox.IsChecked == true)
            // wpfPlot.plt.AxisAuto();

            double[] autoAxisLimits = wpfPlot.plt.AxisAuto(verticalMargin: .5);
            double oldX2 = autoAxisLimits[1];
            wpfPlot.plt.Axis(x2: oldX2 + 100);

            wpfPlot.Render(skipIfCurrentlyRendering: true);
        }

        //private void DisableAutoAxis(object sender, RoutedEventArgs e)
        //{
        //    double[] autoAxisLimits = wpfPlot1.plt.AxisAuto(verticalMargin: .5);
        //    double oldX2 = autoAxisLimits[1];
        //    wpfPlot1.plt.Axis(x2: oldX2 + 1000);
        //}
    }
}
