using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Reactive.Demo.Infrastructure
{
    class PlotFactory
    {
        public static Plot CreateHistogramPlot()
        {
            var plt = new Plot(600, 400);

            Random rand = new Random(0);
            double[] values = DataGen.RandomNormal(rand, pointCount: 1000, mean: 50, stdDev: 20);
            var hist = new Statistics.Histogram(values, min: 0, max: 100);

            double barWidth = hist.binSize * 1.2; // slightly over-side to reduce anti-alias rendering artifacts

            plt.PlotBar(hist.bins, hist.countsFrac, barWidth: barWidth, outlineWidth: 0);
            plt.PlotScatter(hist.bins, hist.countsFracCurve, markerSize: 0, lineWidth: 2, color: Color.Black);
            plt.Title("Normal Random Data");
            plt.YLabel("Frequency (fraction)");
            plt.XLabel("Value (units)");
            plt.Axis(null, null, 0, null);
            plt.Grid(lineStyle: LineStyle.Dot);

            return plt;
            //plt.SaveFig("Advanced_Statistics_Histogram.png");
        }
        
        public static Plot CreateAdvancedStatisticsPlot()
        {
            var plt = new ScottPlot.Plot(600, 400);

            // create some sample data to represent test scores
            Random rand = new Random(0);
            double[] scores = DataGen.RandomNormal(rand, 250, 85, 5);

            // create a Population object from the data
            var pop = new ScottPlot.Statistics.Population(scores);

            // display the original values scattered vertically
            double[] ys = DataGen.RandomNormal(rand, pop.values.Length, stdDev: .15);
            plt.PlotScatter(pop.values, ys, markerSize: 10,
                markerShape: MarkerShape.openCircle, lineWidth: 0);

            // display the bell curve for this distribution
            double[] curveXs = DataGen.Range(pop.minus3stDev, pop.plus3stDev, .1);
            double[] curveYs = pop.GetDistribution(curveXs);
            plt.PlotScatter(curveXs, curveYs, markerSize: 0, lineWidth: 2);

            // improve the style of the plot
            plt.Title($"Test Scores (mean: {pop.mean:0.00} +/- {pop.stDev:0.00}, n={pop.n})");
            plt.XLabel("Score");
            plt.Grid(lineStyle: LineStyle.Dot);

            return plt;
            //plt.SaveFig("Advanced_Statistics_Population.png");
        }
    }
}
