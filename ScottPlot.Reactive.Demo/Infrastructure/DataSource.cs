using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace ScottPlot.Reactive.Demo
{
    class DataSource
    {
        public static IObservable<double> ObserveValues()
        {
            var class3 = new DataFactory();
            var obs = Observable.Interval(TimeSpan.FromMilliseconds(300))
           .Select(a =>
           {
               return class3.NextData();
           });

            return obs;
        }

        public static IObservable<OHLC> ObserveOHLCValues()
        {
            var class3 = new DataFactory();

            var obs3 = Observable.Create<OHLC>(observer => Observable.Interval(TimeSpan.FromMilliseconds(300))
        .Select(a => class3.NextOHLC())
            .Subscribe(a =>
            {
                if (a != null)
                    observer.OnNext(a);
                else
                    observer.OnCompleted();
            }));
            return obs3;
        }
    }
}
