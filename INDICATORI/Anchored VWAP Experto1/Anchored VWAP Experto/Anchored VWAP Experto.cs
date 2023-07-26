using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class AnchoredVWAPExperto : Indicator
    {

        [Output("VWAP", LineColor = "Yellow")]
        public IndicatorDataSeries VWAP { get; set; }

        public enum PriceType
        {
            Typical = 1,
            High = 2,
            Low = 3,
            Median = 4
        }

        [Parameter("VWAP Price", Group = "VWAP Calcuation Type", DefaultValue = PriceType.Typical)]
        public PriceType VWAPSelectedType { get; set; }

        [Parameter("Arrow Color", Group = "Arrows Settings", DefaultValue = Colors.Yellow)]
        public Colors ArrowColor { get; set; }

        [Parameter("VWAP Number", Group = "Use for Multiple VWAPs", DefaultValue = 0)]
        public int VWAPNumber { get; set; }

        [Parameter("Icon Style", Group = "Arrows Settings", DefaultValue = ChartIconType.UpArrow)]
        public ChartIconType IconStyle { get; set; }

        public ChartIcon Arrow;
        public DateTime IconTime;
        public bool IconExists = false;
        public int IndexIconStart;
        public int ArrowCreated;
        public ChartIcon VWAPIcon;
        public DataSeries VWAPTPrice;

        protected override void Initialize()
        {

            foreach (ChartObject o in Chart.Objects)
            {
                var VWAP_Name = "VWAP-Arrow" + VWAPNumber;
                if (o.Name == VWAP_Name && o.ObjectType == ChartObjectType.Icon)
                {
                    o.IsInteractive = true;
                    VWAPIcon = (ChartIcon)o;
                    IconTime = VWAPIcon.Time;
                    VWAPIcon.Color = ArrowColor.ToString();
                    VWAPIcon.IconType = IconStyle;
                    IconExists = true;
                    ArrowCreated = 1;
                }
            }

            if (VWAPSelectedType == PriceType.Typical)
                VWAPTPrice = Bars.TypicalPrices;
            else if (VWAPSelectedType == PriceType.High)
                VWAPTPrice = Bars.HighPrices;
            else if (VWAPSelectedType == PriceType.Low)
                VWAPTPrice = Bars.LowPrices;
            else if (VWAPSelectedType == PriceType.Median)
                VWAPTPrice = Bars.MedianPrices;

            if (!IconExists)
            {
                IndexIconStart = Chart.FirstVisibleBarIndex + ((Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex) / 2);
                Print (IndexIconStart, " ", Chart.BarsTotal);
            }
            Chart.ObjectUpdated += OnChartObjectUpdated;
        }

        public override void Calculate(int index)
        {
            //******************** Create VWAP Arrow One *********************************//
            if (index == IndexIconStart && IconExists == false)
            {
                var VWAP_Name = "VWAP-Arrow" + VWAPNumber;
                double pr = Bars.LowPrices[index] - (Bars.HighPrices[index] - Bars.LowPrices[index]);
                Arrow = Chart.DrawIcon(VWAP_Name, ChartIconType.UpArrow, index, pr, Color.Gold);
                Arrow.IsInteractive = true;
                VWAPIcon.Color = ArrowColor.ToString();
                VWAPIcon.IconType = IconStyle;
                ArrowCreated = 2;
            }

            //************************ Draw VWAP One Line **********************************//
            if (IsLastBar && ArrowCreated == 2)
            {
                double sumP = 0.0;
                double sumVl = 0.0;
                for (int j = 0; j < Chart.BarsTotal; j++)
                {
                    if (j >= IndexIconStart)
                    {
                        sumP += VWAPTPrice[j] * Bars.TickVolumes[j];
                        sumVl += Bars.TickVolumes[j];
                        VWAP[j] = sumP / sumVl;
                    }
                }
            }
            else if (IsLastBar && ArrowCreated == 1)
            {
                double sumP = 0.0;
                double sumVl = 0.0;
                for (int j = 0; j < Chart.BarsTotal; j++)
                {
                    if (Bars.OpenTimes[j] >= IconTime)
                    {
                        sumP += VWAPTPrice[j] * Bars.TickVolumes[j];
                        sumVl += Bars.TickVolumes[j];
                        VWAP[j] = sumP / sumVl;
                    }
                }
            }

        }

        void OnChartObjectUpdated(ChartObjectUpdatedEventArgs obj)
        {
            if (obj.ChartObject.ObjectType == ChartObjectType.Icon)
            {
                var VWAP_Name = "VWAP-Arrow" + VWAPNumber;
                if (obj.ChartObject.Name == VWAP_Name)
                {
                    UpdateIconOne();
                }
            }
        }
        private void UpdateIconOne()
        {
            double sumP = 0.0;
            double sumVl = 0.0;
            for (int i = 0; i < Chart.BarsTotal; i++)
            {
                VWAP[i] = double.NaN;
            }
            ArrowCreated = 1;
            if (IconExists)
            {
                IconTime = VWAPIcon.Time;
                VWAPIcon.Color = ArrowColor.ToString();
                VWAPIcon.IconType = IconStyle;
            }
            else
            {
                IconTime = Arrow.Time;
                Arrow.Color = ArrowColor.ToString();
                Arrow.IconType = IconStyle;
            }
            for (int j = 0; j < Chart.BarsTotal; j++)
            {
                if (Bars.OpenTimes[j] >= IconTime)
                {
                    sumP += VWAPTPrice[j] * Bars.TickVolumes[j];
                    sumVl += Bars.TickVolumes[j];
                    VWAP[j] = sumP / sumVl;
                }
            }
        }
    }
}


