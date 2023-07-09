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
        [Parameter("Number of bars back", DefaultValue = 40)]
        public int startIndex { get; set; }
        
        [Output("Line", LineColor = "Yellow")]
        public IndicatorDataSeries Result { get; set; }

        public int IndexIconStart;
        public DataSeries VWAPTPrice;

        protected override void Initialize()
        {
             VWAPTPrice = Bars.TypicalPrices;
        }
        
        public override void Calculate(int index)
        {                                       
            if (IsLastBar)
            {           
                IndexIconStart = Bars.Count - IndexOfMin(startIndex) - 1;
                Print ("Anchor - indice ultima barra: ", Bars.Count, " ", "indice inizio: ", IndexIconStart);
                
                double sumP = 0.0;
                double sumVl = 0.0;
                for (int j = 0; j < Chart.BarsTotal; j++)
                {
                    if (j >= IndexIconStart)
                    {
                        sumP += VWAPTPrice[j] * Bars.TickVolumes[j];
                        sumVl += Bars.TickVolumes[j];
                        Result[j] = Math.Round(sumP / sumVl, 5) ;                                       
                    }                      
                }
            }           
        }
                
       //calculate the IndexMin   
       private int IndexOfMin(int index)
        {   
            if ( Bars.Count > startIndex)
            {
            double minimum = Bars.Last(1).Low;
            int minIndex = 0;
            for (int i = 1; i <= index; i++)
            {
                if ( Bars.Last(i).Low < minimum)
                {
                    minIndex = i;
                    minimum =  Bars.Last(i).Low;
                }
            }
            return minIndex;
            }
            else
            {
                return 0;
             }
        }                
    }
}


