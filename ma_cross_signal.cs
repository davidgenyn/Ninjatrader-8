#region Using declarations
using System;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.NinjaScript;
using NinjaTrader.NinjaScript.Strategies;
#endregion

namespace NinjaTrader.NinjaScript.Strategies
{
    public class MACrossSignal : Strategy
    {
        private SMA fastMA;
        private SMA slowMA;

        [Range(1, int.MaxValue), NinjaScriptProperty]
        [Display(Name = "Fast MA Period", Order = 1, GroupName = "Parameters")]
        public int FastPeriod { get; set; } = 9;

        [Range(1, int.MaxValue), NinjaScriptProperty]
        [Display(Name = "Slow MA Period", Order = 2, GroupName = "Parameters")]
        public int SlowPeriod { get; set; } = 20;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Name = "MA Cross Signal (Alerts Only)";
                Calculate = Calculate.OnBarClose;
                IsOverlay = true;
                IsInstantiatedOnEachOptimizationIteration = false;
            }
            else if (State == State.DataLoaded)
            {
                fastMA = SMA(FastPeriod);
                slowMA = SMA(SlowPeriod);
            }
        }

        protected override void OnBarUpdate()
        {
            if (CurrentBar < Math.Max(FastPeriod, SlowPeriod))
                return;

            // Cross boven
            if (CrossAbove(fastMA, slowMA, 1))
            {
                Alert("MAcrossLong", Priority.Medium, "MA9 crossed ABOVE MA20", NinjaTrader.Core.Globals.InstallDir + @"\sounds\Alert1.wav", 10, Brushes.Green, Brushes.White);
            }

            // Cross onder
            if (CrossBelow(fastMA, slowMA, 1))
            {
                Alert("MAcrossShort", Priority.Medium, "MA9 crossed BELOW MA20", NinjaTrader.Core.Globals.InstallDir + @"\sounds\Alert2.wav", 10, Brushes.Red, Brushes.White);
            }
        }
    }
}
