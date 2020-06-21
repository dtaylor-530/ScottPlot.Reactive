using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScottPlot.Reactive
{
    public class OHLCModel
    {
        private List<OHLC> data = new List<OHLC>();
        int index = 0;
        private readonly SynchronizationContext mainThread;
        private readonly WpfPlot wpfPlot;

        public OHLCModel(WpfPlot wpfPlot, IObservable<OHLC> dataObservable, IObservable<Unit> renderObservable = null)
        {
            mainThread = SynchronizationContext.Current;
            //var plt = new ScottPlot.Plot(600, 400);
            //wpfPlot.plt = plt;
            this.wpfPlot = wpfPlot;

            // signal.maxRenderIndex = 1;
            // plot the data array only once
            dataObservable.Subscribe(d =>
            {
                //signal.maxRenderIndex = index;
                data.Add(d);


            });

            (renderObservable ?? dataObservable.Select(a => Unit.Default))
                //.SubscribeOn(mainThread)
                .Subscribe(a =>
            {
                mainThread.Send(a => Render(), null);
            });

        }



        void Render()
        {
            //if (AutoAxisCheckbox.IsChecked == true)
            // wpfPlot.plt.AxisAuto();
            var arr = data.ToArray();
            var signal = wpfPlot.plt.PlotCandlestick(arr);
            if (arr.Length >= 100)
            {
                Task.Run(() => Statistics.Finance.Bollinger(arr))
                    .ContinueWith(async a =>
                {
                    var (sma, bolL, bolU) = (await a);
                    double[] xs = DataGen.Consecutive(arr.Length);
                    wpfPlot.plt.PlotScatter(xs, bolL, color: Color.Blue, markerSize: 0);
                    wpfPlot.plt.PlotScatter(xs, bolU, color: Color.Blue, markerSize: 0);
                    wpfPlot.plt.PlotScatter(xs, sma, color: Color.Blue, markerSize: 0, lineStyle: LineStyle.Dash);
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }

            double[] autoAxisLimits = wpfPlot.plt.AxisAuto(verticalMargin: .5);
            double oldX2 = autoAxisLimits[1];
            wpfPlot.plt.Axis(x2: oldX2 + 10);

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
