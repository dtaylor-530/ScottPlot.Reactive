#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Reactive
{
    public class DataFactory
    {
        public List<double> data = new List<double>();
        Random rand = new Random(0);
        OHLC[] ohlcs;
        int i = 0;

        public DataFactory()
        {
            ohlcs = DataGen.RandomStockPrices(rand, 20000, sequential: true);
        }

        public double NextData()
        {
            return Math.Round(rand.NextDouble() - .5, 3);
        }

        public OHLC? NextOHLC()
        {
            if (i < 20000)
            {
                return ohlcs[i++];
            }
            return null;
            //{
            //    var latest = ohlcs[i];
            //    var old = ohlcs[i % 20000];
            //    return new OHLC
            //    {
            //        close = old.close,
            //        open = old.open,
            //        high = old.high,
            //        low = old.low,
            //        closedHigher = old.closedHigher,
            //        highestOpenClose = old.highestOpenClose,
            //        lowestOpenClose = old.lowestOpenClose,
            //        time = old.time+,
            //    };
            //}
        }
    }
}
