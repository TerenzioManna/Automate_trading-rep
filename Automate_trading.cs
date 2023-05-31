using cAlgo.API;
using cAlgo.API.Internals;
using System;
using System.Linq;
using System.Net;
using Telegram.Bot;


namespace cAlgo.Robots
{

    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class STATISTICO : Robot
    {

        [Parameter("Name", Group = "General", DefaultValue = "bot1_nas100")]
        public string InstanceName { get; set; }

        [Parameter("Data avvio", Group = "General", DefaultValue = "2023-05-31 00:00:00")]
        public string string_dateBot { get; set; }

        [Parameter("Lot Size", Group = "Volume", DefaultValue = 0.5, MinValue = 0.01, Step = 0.01)]
        public double LotSize { get; set; }

        [Parameter("Take Profit", Group = "Protection (pips)", DefaultValue = 20, MinValue = 1, Step = 0.5)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Stop Loss", Group = "Protection (pips)", DefaultValue = 40, MinValue = 0, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Max Spread", Group = "Protection (pips)", DefaultValue = 1, MinValue = 0, Step = 0.1)]
        public double maxSpread { get; set; }

        [Parameter("Enable", Group = "Protection Break-Even (pips)", DefaultValue = false)]
        public bool Enable_breakEven { get; set; }

        [Parameter("Break-Even Trigger", Group = "Protection Break-Even (pips)", DefaultValue = 1, MinValue = 0, Step = 0.5)]
        public double BreakEvenPips { get; set; }

        [Parameter("Break-Even Extra", Group = "Protection Break-Even (pips)", DefaultValue = 1, MinValue = 0, Step = 0.5)]
        public double BreakEvenExtraPips { get; set; }

        [Parameter("Run on opening bar", Group = "Strategy", DefaultValue = false)]
        public bool On_bar { get; set; }

        [Parameter("Make new order immediately after lost", Group = "Strategy", DefaultValue = false)]
        public bool orderLost { get; set; }

        [Parameter("Wait next candle to make order", Group = "Strategy", DefaultValue = false)]
        public bool nextCandle { get; set; }

        [Parameter("Stop new trade when total daily profit > 0", Group = "Strategy", DefaultValue = false)]
        public bool StopWhenWin { get; set; }

        [Parameter("Force to close position daily", Group = "Strategy", DefaultValue = false)]
        public bool closePosition { get; set; }

        [Parameter("Max trades in a day", Group = "Strategy", DefaultValue = 1, MinValue = 1, Step = 1)]
        public int maxTrades { get; set; }

        [Parameter("Max volume tick", Group = "Strategy", DefaultValue = 1, MinValue = 1, Step = 10)]
        public int maxVolumeTick { get; set; }

        [Parameter("Enable", Group = "Strategy (Martingala)", DefaultValue = false)]
        public bool Martingala { get; set; }

        [Parameter("Lot adding", Group = "Strategy (Martingala)", DefaultValue = 0.1, MinValue = 0, Step = 0.1)]
        public double lotAdding { get; set; }

        [Parameter("SB1", Group = "Strategy Buy (Compra = 1, Vendi = 2)", DefaultValue = 0, MinValue = 1, MaxValue = 2, Step = 1)]
        public int sb1 { get; set; }

        [Parameter("SB2", Group = "Strategy Buy (Compra = 1, Vendi = 2)", DefaultValue = 0, MinValue = 0, MaxValue = 2, Step = 1)]
        public int sb2 { get; set; }

        [Parameter("SB3", Group = "Strategy Buy (Compra = 1, Vendi = 2)", DefaultValue = 0, MinValue = 1, MaxValue = 2, Step = 1)]
        public int sb3 { get; set; }

        [Parameter("SB4", Group = "Strategy Buy (Compra = 1, Vendi = 2)", DefaultValue = 0, MinValue = 1, MaxValue = 2, Step = 1)]
        public int sb4 { get; set; }

        [Parameter("SS1", Group = "Strategy Sell (Compra = 1, Vendi = 2)", DefaultValue = 0, MinValue = 1, MaxValue = 2, Step = 1)]
        public int ss1 { get; set; }

        [Parameter("SS2", Group = "Strategy Sell (Compra = 1, Vendi = 2)", DefaultValue = 0, MinValue = 1, MaxValue = 2, Step = 1)]
        public int ss2 { get; set; }

        [Parameter("SS3", Group = "Strategy Sell (Compra = 1, Vendi = 2)", DefaultValue = 0, MinValue = 1, MaxValue = 2, Step = 1)]
        public int ss3 { get; set; }

        [Parameter("SS4", Group = "Strategy Sell (Compra = 1, Vendi = 2)", DefaultValue = 0, MinValue = 1, MaxValue = 2, Step = 1)]
        public int ss4 { get; set; }

        [Parameter("B1", Group = "Strategy (Positions back candle Buy)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int b1 { get; set; }

        [Parameter("B2", Group = "Strategy (Positions back candle Buy)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int b2 { get; set; }

        [Parameter("B3", Group = "Strategy (Positions back candle Buy)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int b3 { get; set; }

        [Parameter("B4", Group = "Strategy (Positions back candle Buy)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int b4 { get; set; }
        
        [Parameter("S1", Group = "Strategy (Positions back candle Sell)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int s1 { get; set; }
        
        [Parameter("S2", Group = "Strategy (Positions back candle Sell)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int s2 { get; set; }
        
        [Parameter("S3", Group = "Strategy (Positions back candle Sell)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int s3 { get; set; }
        
        [Parameter("S4", Group = "Strategy (Positions back candle Sell)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int s4 { get; set; }
        
        [Parameter("B1_minB", Group = "Strategy (min body candle Buy)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int minB_b1 { get; set; }
        
        [Parameter("B2_minB", Group = "Strategy (min body candle Buy)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int minB_b2 { get; set; }
        
        [Parameter("S1_minB", Group = "Strategy (min body candle Sell)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int minB_s1 { get; set; }
        
        [Parameter("S2_minB", Group = "Strategy (min body candle Sell)", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int minB_s2 { get; set; }
        
        [Parameter("B1_maxB", Group = "Strategy (max body candle Buy)", DefaultValue = 1000, MinValue = 0, Step = 1)]
        public int maxB_b1 { get; set; }
        
        [Parameter("B2_maxB", Group = "Strategy (max body candle Buy)", DefaultValue = 1000, MinValue = 0, Step = 1)]
        public int maxB_b2 { get; set; }
        
        [Parameter("S1_maxB", Group = "Strategy (max body candle Sell)", DefaultValue = 1000, MinValue = 0, Step = 1)]
        public int maxB_s1 { get; set; }
        
        [Parameter("S2_maxB", Group = "Strategy (max body candle Sell)", DefaultValue = 1000, MinValue = 0, Step = 1)]
        public int maxB_s2 { get; set; }                            

        [Parameter("X1", Group = "Strategy Buy", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int x1 { get; set; }

        [Parameter("X2", Group = "Strategy Buy", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int x2 { get; set; }
        
        [Parameter("X3", Group = "Strategy Buy", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int x3 { get; set; }
        
        [Parameter("X4", Group = "Strategy Buy", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int x4 { get; set; }

        [Parameter("Y1", Group = "Strategy Sell", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int y1 { get; set; }

        [Parameter("Y2", Group = "Strategy Sell", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int y2 { get; set; }
        
        [Parameter("Y3", Group = "Strategy Sell", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int y3 { get; set; }
        
        [Parameter("Y4", Group = "Strategy Sell", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int y4 { get; set; }

        [Parameter("Enable", Group = "Strategy Time trading (Utc)", DefaultValue = false)]
        public bool enableTime { get; set; }

        [Parameter("Opening hour", Group = "Strategy Time trading (Utc)", DefaultValue = 8, MinValue = 0, MaxValue = 23, Step = 1)]
        public int openHour { get; set; }

        [Parameter("Close hour", Group = "Strategy Time trading (Utc)", DefaultValue = 20, MinValue = 0, MaxValue = 23, Step = 1)]
        public int closeHour { get; set; }

        [Parameter("Opening minute", Group = "Strategy Time trading (Utc)", DefaultValue = 0, MaxValue = 59, MinValue = 0, Step = 1)]
        public int openMinute { get; set; }

        [Parameter("Closing minute", Group = "Strategy Time trading (Utc)", DefaultValue = 0, MinValue = 0, MaxValue = 59, Step = 1)]
        public int closeMinute { get; set; }

        [Parameter("Exclude monday", Group = "Strategy Time trading (Utc)", DefaultValue = false)]
        public bool excludeMonday { get; set; }

        [Parameter("Exclude tuesday", Group = "Strategy Time trading (Utc)", DefaultValue = false)]
        public bool excludeTuesday { get; set; }

        [Parameter("Exclude wednesday", Group = "Strategy Time trading (Utc)", DefaultValue = false)]
        public bool excludeWednesday { get; set; }

        [Parameter("Exclude thursday", Group = "Strategy Time trading (Utc)", DefaultValue = false)]
        public bool excludeThursday { get; set; }

        [Parameter("Exclude friday", Group = "Strategy Time trading (Utc)", DefaultValue = false)]
        public bool excludeFriday { get; set; }

        [Parameter("Send a Telegram", Group = "Telegram Notifications", DefaultValue = false)]
        public bool IncludeTelegram { get; set; }

        [Parameter("Bot Token", Group = "Telegram Notifications", DefaultValue = "5261129940:AAFKZoXMGYjUBmtmsdIGbuKOCA0jEvwWQSk")]
        public string BotToken { get; set; }

        [Parameter("Chat ID", Group = "Telegram Notifications", DefaultValue = "-1001559045628")]
        public string ChatID { get; set; }

        //msg allert telegram        
        string text0, text1, text2, text3, text4, text5, text_close;
        string text00, text10, text20, text30, text40, text50, text_open;

        internal bool triggerOpenBuy = false, triggerOpenSell = false, triggerCloseBuy = false, triggerCloseSell = false;

        internal bool general = false;

        internal bool statisticaBuy = false, statisticaSell = false;

        internal bool filterBuy = false, filterSell = false;

        internal bool candleBuy = false, candleSell = false;

        internal int trades = 0;

        internal DateTime startDateTime, startDateTime_bot;

        internal double resetVolume = 1;

        internal double positionLot = 0;

        internal double takeProfit = 0;

        internal double stopLoss = 0;

        internal bool positionClosed = true;

        internal bool trigRsiDiv = false;

        internal double deltaRsiDiv = 0;

        internal int lastCandle = 0;

        internal int lotMoltip = 2;

        protected override void OnStart()
        {
            //public event Action Closed
            Positions.Closed += PositionsOnClosed;

            //Init DateTime
            startDateTime = DateTime.Now;
            startDateTime_bot = DateTime.Parse(string_dateBot);
            //telegram notification
            //telegram = new Telegram();

            //number of trades done today
            reset_max_Trades();
            trades = tradesToday();

            //Telegram security protocol
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        protected override void OnTick()
        {
            if (On_bar == false)
            {
                PositionManagement();
                BreakEvenAdjustment();
            }
        }

        protected override void OnBar()
        {
            if (On_bar == true)
            {
                PositionManagement();
                BreakEvenAdjustment();
            }
        }

        private void Timer_TimerTick()
        {
        }

        private void PositionsOnClosed(PositionClosedEventArgs args)
        {
            var position = args.Position;

            double giornaliero = History.Where(x => x.ClosingTime.Date == Time.Date).Sum(x => x.NetProfit);
            double totale = History.Where(x => x.ClosingTime.Date <= startDateTime_bot).Sum(x => x.NetProfit);

            Print("Trade numero: {0} Commissione : {1}  Profitto : {2} Profitto giornaliero : {3} Profitto totale : {4}", trades, Math.Round(position.Commissions, 2), position.NetProfit, giornaliero, totale);

            text00 = "Chiusura posizione: " + trades + "\r\n";
            text10 = "Commissione: " + Math.Round(position.Commissions, 1) + "\r\n";
            text20 = "Profitto: " + position.NetProfit + "\r\n";
            text30 = "Profitto giornaliero: " + Math.Round(giornaliero,1) + "\r\n";
            text40 = "Profitto totale: " + Math.Round(totale,1);
            text50 = "";
            text_close = text0 + text00 + text10 + text20 + text30 + text40 + text50;

            if (IncludeTelegram == true)
            {
                SendTelegram(text_close);
            }

            positionClosed = true;
            lastCandle = Bars.Count;

            //MARTINGALA
            if (Martingala == true)
            {
                positionLot = ((position.Quantity * lotMoltip) + (lotAdding)) * resetVolume;
                //takeProfit = position.TakeProfit.Value;
                //stopLoss = position.StopLoss.Value;
            }
            else
            {
                positionLot = LotSize;
                //takeProfit = TakeProfitInPips;
                //stopLoss = StopLossInPips;
            }

            //SE VINCO SMETTO DI GIOCARE
            if (StopWhenWin == true && giornaliero > 0)
            {
                trades = maxTrades;
                lastCandle = 0;
                return;
            }

            //APRI IMMEDIATAMENTE APPENA PERDI
            if (orderLost == true)
            {
                if (position.GrossProfit < 0 && max_Trades_Day())
                {

                    if (position.TradeType == TradeType.Buy)
                    {
                        OpenPosition(TradeType.Sell, positionLot);
                        return;
                    }
                    if (position.TradeType == TradeType.Sell)
                    {
                        OpenPosition(TradeType.Buy, positionLot);
                        return;
                    }
                }
            }

        }

        private void PositionManagement()
        {

            general = max_Trades_Day() && timeTrading() && !isOpenAPosition() && spread()
            && dayTrading() && sameCandle() && volumeTick(maxVolumeTick);

            statisticaBuy = GR(sb1, b1, minB_b1, maxB_b1) && GR(sb2, b2, minB_b2, maxB_b2) && GR(sb3, b3) && GR(sb4, b4);
            statisticaSell = GR(ss1, s1, minB_s1, maxB_s1) && GR(ss2, s2, minB_s2, maxB_s2) && GR(ss3, s3) && GR(ss4, s4);

            triggerOpenBuy = general && statisticaBuy && minMaxBuy(x1,x2) && minMaxBuy(x3,x4);
            triggerOpenSell = general && statisticaSell && minMaxSell(y1,y2) && minMaxBuy(y3,y4);

            executeOrder();
      
            
            //closePositionMinute(lastOpenPositionMinute + 10);
            //closePositionHour(lastOpenPositionHour);                         
        }

        public async void SendTelegram(string telegramMessage)
        {
            try
            {
                var bot = new TelegramBotClient(BotToken);

                await bot.SendTextMessageAsync(ChatID, telegramMessage, Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            catch (Exception ex)
            {
                Print("ERROR: " + ex.Message);
            }

        }

        internal bool minMaxBuy(int i, int j)
        {
            if (Bars.Last(i).Close - Bars.Last(j).Close <= 0)
            {
                return true;
            }
            else
                return false;
        }

        internal bool minMaxSell(int i, int j)
        {
            if (Bars.Last(i).Close - Bars.Last(j).Close >= 0)
            {
                return true;
            }
            else
                return false;
        }
        internal bool sameCandle()
        {
            if (nextCandle == true && Bars.Count == lastCandle)
            {
                return false;
            }
            else
                return true;
        }

        internal void closePositionHour(int hour)
        {
            if (Server.TimeInUtc.Hour == hour)
            {
                CloseAllPositions();
            }
        }

        internal void closePositionMinute(int minute)
        {
            if (Server.TimeInUtc.Minute == minute)
            {
                CloseAllPositions();
            }
        }

        internal bool volumeTick(int volume)
        {
            if (Bars.TickVolumes.Last(1) > volume)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool spread()
        {
            if (Symbol.Spread <= maxSpread)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal double GetBody(int index)
        {
            double body = (Bars.Last(index).Open - Bars.Last(index).Close) / Symbol.PipSize;
            return Math.Abs(body);
        }

        internal bool timeTrading()
        {
            if (enableTime == true)
            {
                bool a = Server.TimeInUtc.Hour >= openHour && Server.TimeInUtc.Hour <= closeHour;
                bool b = Server.TimeInUtc.Minute >= openMinute && Server.TimeInUtc.Minute <= closeMinute;
                return a && b;
            }
            else
                return true;
        }

        bool dayTrading()
        {
            if (excludeMonday == true && Server.TimeInUtc.DayOfWeek == DayOfWeek.Monday)
            {
                return false;
            }

            if (excludeTuesday == true && Server.TimeInUtc.DayOfWeek == DayOfWeek.Tuesday)
            {
                return false;
            }

            if (excludeWednesday == true && Server.TimeInUtc.DayOfWeek == DayOfWeek.Wednesday)
            {
                return false;
            }
            if (excludeThursday == true && Server.TimeInUtc.DayOfWeek == DayOfWeek.Thursday)
            {
                return false;
            }
            if (excludeFriday == true && Server.TimeInUtc.DayOfWeek == DayOfWeek.Friday)
            {
                return false;
            }
            else
                return true;
        }

        internal bool max_Trades_Day()
        {
            reset_max_Trades();
            if (trades < maxTrades)
                return true;
            else
                return false;
        }

        private void reset_max_Trades()
        {
            if (Server.Time.Day != startDateTime.Day)
            {
                trades = 0;
                resetVolume = 0;
                startDateTime = Server.Time;
                lastCandle = 0;

                if (closePosition == true)
                {
                    CloseAllPositions();
                }

            }
        }

        private int tradesToday()
        {
            int Trades = 0;
            var LastTrades = History.OrderByDescending(iTrade => iTrade.EntryTime).Take(10).ToArray();

            for (int i = 0; i < LastTrades.Length; i++)
            {
                if (LastTrades[i].EntryTime.Day == Bars.OpenTimes.LastValue.Day && LastTrades[i].ClosingTime.Month == Bars.OpenTimes.LastValue.Month && LastTrades[i].ClosingTime.Year == Bars.OpenTimes.LastValue.Year)
                {
                    //Print("Symbol : {0}  ClosingTime : {1}", LastTrades[i].SymbolName, LastTrades[i].ClosingTime);
                    Trades++;
                }
            }

            //Print("Numero di trade fatti oggi: {0}", Trades);
            return Trades;
        }

        //L'indice relativo a 24ore prima
        //int g = Bars.OpenTimes.GetIndexByTime(Server.TimeInUtc.AddDays(-1));

        private bool Gi(int index, double MinBody = 0, double maxBody = 1000, double upperMinShadow = 0, double upperMaxShadow = 1000, double lowerMinShadow = 0, double lowerMaxShadow = 1000)
        {
            bool aa = Bars.OpenPrices[index] < Bars.ClosePrices[index];
            bool a = ((Bars.ClosePrices[index] - Bars.OpenPrices[index]) / Symbol.PipSize) >= MinBody;
            bool b = ((Bars.ClosePrices[index] - Bars.OpenPrices[index]) / Symbol.PipSize) <= maxBody;
            bool c = ((Bars.HighPrices[index] - Bars.ClosePrices[index]) / Symbol.PipSize) >= upperMinShadow;
            bool d = ((Bars.HighPrices[index] - Bars.ClosePrices[index]) / Symbol.PipSize) <= upperMaxShadow;
            bool e = ((Bars.OpenPrices[index] - Bars.LowPrices[index]) / Symbol.PipSize) >= lowerMinShadow;
            bool f = ((Bars.OpenPrices[index] - Bars.LowPrices[index]) / Symbol.PipSize) <= lowerMaxShadow;

            if (aa && a && b && c && d && e && f)
                return true;
            else
                return false;
        }

        private bool Ri(int index, double MinBody = 0, double maxBody = 1000, double upperMinShadow = 0, double upperMaxShadow = 1000, double lowerMinShadow = 0, double lowerMaxShadow = 1000)
        {
            bool aa = Bars.OpenPrices[index] > Bars.ClosePrices[index];
            bool a = ((Bars.OpenPrices[index] - Bars.ClosePrices[index]) / Symbol.PipSize) >= MinBody;
            bool b = ((Bars.OpenPrices[index] - Bars.ClosePrices[index]) / Symbol.PipSize) <= maxBody;
            bool c = ((Bars.HighPrices[index] - Bars.OpenPrices[index]) / Symbol.PipSize) >= upperMinShadow;
            bool d = ((Bars.HighPrices[index] - Bars.OpenPrices[index]) / Symbol.PipSize) <= upperMaxShadow;
            bool e = ((Bars.ClosePrices[index] - Bars.LowPrices[index]) / Symbol.PipSize) >= lowerMinShadow;
            bool f = ((Bars.ClosePrices[index] - Bars.LowPrices[index]) / Symbol.PipSize) <= lowerMaxShadow;

            if (aa && a && b && c && d && e && f)
                return true;
            else
                return false;
        }


        private bool GR(int type, int index, double MinBody = 0, double maxBody = 1000, double upperMinShadow = 0, double upperMaxShadow = 1000, double lowerMinShadow = 0, double lowerMaxShadow = 1000)
        {
            if (type == 1
                && Bars.Last(index).Open < Bars.Last(index).Close
                && ((Bars.Last(index).Close - Bars.Last(index).Open) / Symbol.PipSize) >= MinBody
                && ((Bars.Last(index).Close - Bars.Last(index).Open) / Symbol.PipSize) <= maxBody
                && ((Bars.Last(index).High - Bars.Last(index).Close) / Symbol.PipSize) >= upperMinShadow
                && ((Bars.Last(index).High - Bars.Last(index).Close) / Symbol.PipSize) <= upperMaxShadow
                && ((Bars.Last(index).Open - Bars.Last(index).Low) / Symbol.PipSize) >= lowerMinShadow
                && ((Bars.Last(index).Open - Bars.Last(index).Low) / Symbol.PipSize) <= lowerMaxShadow)
            {
                return true;
            }

            if (type == 2
                && Bars.Last(index).Open > Bars.Last(index).Close
                && ((Bars.Last(index).Open - Bars.Last(index).Close) / Symbol.PipSize) >= MinBody
                && ((Bars.Last(index).Open - Bars.Last(index).Close) / Symbol.PipSize) <= maxBody
                && ((Bars.Last(index).High - Bars.Last(index).Open) / Symbol.PipSize) >= upperMinShadow
                && ((Bars.Last(index).High - Bars.Last(index).Open) / Symbol.PipSize) <= upperMaxShadow
                && ((Bars.Last(index).Close - Bars.Last(index).Low) / Symbol.PipSize) >= lowerMinShadow
                && ((Bars.Last(index).Close - Bars.Last(index).Low) / Symbol.PipSize) <= lowerMaxShadow)

            {
                return true;
            }

            else
                return false;

        }

        private bool G(int index, double MinBody = 0, double maxBody = 1000, double upperMinShadow = 0, double upperMaxShadow = 1000, double lowerMinShadow = 0, double lowerMaxShadow = 1000)
        {
            bool aa = Bars.Last(index).Open < Bars.Last(index).Close;
            bool a = ((Bars.Last(index).Close - Bars.Last(index).Open) / Symbol.PipSize) >= MinBody;
            bool b = ((Bars.Last(index).Close - Bars.Last(index).Open) / Symbol.PipSize) <= maxBody;
            bool c = ((Bars.Last(index).High - Bars.Last(index).Close) / Symbol.PipSize) >= upperMinShadow;
            bool d = ((Bars.Last(index).High - Bars.Last(index).Close) / Symbol.PipSize) <= upperMaxShadow;
            bool e = ((Bars.Last(index).Open - Bars.Last(index).Low) / Symbol.PipSize) >= lowerMinShadow;
            bool f = ((Bars.Last(index).Open - Bars.Last(index).Low) / Symbol.PipSize) <= lowerMaxShadow;

            if (aa && a && b && c && d && e && f)
                return true;
            else
                return false;
        }

        private bool R(int index, double MinBody = 0, double maxBody = 1000, double upperMinShadow = 0, double upperMaxShadow = 1000, double lowerMinShadow = 0, double lowerMaxShadow = 1000)
        {
            bool aa = Bars.Last(index).Open > Bars.Last(index).Close;
            bool a = ((Bars.Last(index).Open - Bars.Last(index).Close) / Symbol.PipSize) >= MinBody;
            bool b = ((Bars.Last(index).Open - Bars.Last(index).Close) / Symbol.PipSize) <= maxBody;
            bool c = ((Bars.Last(index).High - Bars.Last(index).Open) / Symbol.PipSize) >= upperMinShadow;
            bool d = ((Bars.Last(index).High - Bars.Last(index).Open) / Symbol.PipSize) <= upperMaxShadow;
            bool e = ((Bars.Last(index).Close - Bars.Last(index).Low) / Symbol.PipSize) >= lowerMinShadow;
            bool f = ((Bars.Last(index).Close - Bars.Last(index).Low) / Symbol.PipSize) <= lowerMaxShadow;

            if (aa && a && b && c && d && e && f)
                return true;
            else
                return false;
        }

        private void executeOrder()
        {
            if (PositionOpenByType(TradeType.Buy) && triggerCloseBuy)
            {
                CloseAllPositions();
                return;
            }

            if (PositionOpenByType(TradeType.Sell) && triggerCloseSell)
            {
                CloseAllPositions();
                return;
            }

            if (triggerOpenBuy && triggerOpenSell)
            {
                Print("impossible to open a position");
                return;
            }

            if (triggerOpenBuy)
            {
                ClosePosition(TradeType.Sell);
                openPositionBuy();
            }

            if (triggerOpenSell)
            {
                ClosePosition(TradeType.Buy);
                openPositionSell();
            }

            if (triggerOpenBuy || triggerOpenSell)
            {
                //
            }
        }

        private void openPositionBuy()
        {
            if (!PositionOpenByType(TradeType.Buy))
            {
                OpenPosition(TradeType.Buy);
            }
        }

        private void openPositionSell()
        {
            if (!PositionOpenByType(TradeType.Sell))
            {
                OpenPosition(TradeType.Sell);
            }
        }

        private bool isOpenAPosition()
        {
            if (PositionOpenByType(TradeType.Buy) | PositionOpenByType(TradeType.Sell))
                return true;
            else
                return false;
        }

        private double deltaPips(Symbol s, double price1, double price2, bool enableAbs)
        {
            if (enableAbs)

                return Math.Round(Math.Abs((price1 - price2) / s.PipSize), 1);
            else

                return Math.Round(((price1 - price2) / s.PipSize), 1);
        }

        private void OpenPosition(TradeType typePosition, double volume = 0)
        {
            if (trades == 0)
            {
                volume = Symbol.QuantityToVolumeInUnits(LotSize);
                stopLoss = StopLossInPips;
                takeProfit = TakeProfitInPips;
            }
            else
            {
                volume = Symbol.QuantityToVolumeInUnits(positionLot);
            }

            if (positionClosed == true)
            {
                ExecuteMarketOrder(typePosition, this.SymbolName, volume, InstanceName, stopLoss, takeProfit, DateTime.Now.ToString());
                trades++;
                resetVolume = 1;
                positionClosed = false;

                text0 = "<b>" + InstanceName + "</b>" + "\r\n";
                text1 = typePosition + " posizione " + trades + ": " + "\r\n";
                text2 = "Volume: " + volume + "\r\n";
                text3 = "Take profit: " + takeProfit + "\r\n";
                text4 = "Stop loss: " + stopLoss + "\r\n";
                text5 = "";
                text_open = text0 + text1 + text2 + text3 + text4 + text5;

                if (IncludeTelegram == true)
                {
                    SendTelegram(text_open);
                }
            }
        }

        private void ClosePosition(TradeType typePosition)
        {
            var Pos = Positions.Find(InstanceName, this.SymbolName, typePosition);
            if (Pos != null)
            {
                ClosePosition(Pos);
            }
        }

        private void CloseAllPositions()
        {
            foreach (var Pos in Positions)
            {
                ClosePosition(Pos);
            }
        }

        private bool PositionOpenByType(TradeType typePosition)
        {
            var pos = Positions.FindAll(InstanceName, this.SymbolName, typePosition);
            if (pos.Count() >= 1)
            {
                return true;
            }
            return false;
        }

        private void BreakEvenAdjustment()
        {
            if (Enable_breakEven == false)
            {
                return;
            }

            var allPositions = Positions.FindAll(InstanceName, this.SymbolName);

            foreach (Position position in allPositions)
            {
                var entryPrice = position.EntryPrice;
                var distance = position.TradeType == TradeType.Buy ? Symbol.Bid - entryPrice : entryPrice - Symbol.Ask;

                // move stop loss to break even plus and additional (x) pips
                if (distance >= BreakEvenPips * Symbol.PipSize)
                {
                    if (position.TradeType == TradeType.Buy)
                    {
                        if (position.StopLoss <= position.EntryPrice + (Symbol.PipSize * BreakEvenExtraPips) || position.StopLoss == null)
                        {
                            double newStopLoss = position.EntryPrice + (Symbol.PipSize * BreakEvenExtraPips);
                            if (position.StopLoss == newStopLoss)
                            {
                                return;
                            }
                            else
                                ModifyPosition(position, newStopLoss, position.TakeProfit);
                            //Print("Stop Loss to Break Even set for BUY position {0}", position.Id);
                        }
                    }
                    else
                    {
                        if (position.StopLoss >= position.EntryPrice - (Symbol.PipSize * BreakEvenExtraPips) || position.StopLoss == null)
                        {
                            double newStopLoss = entryPrice - (Symbol.PipSize * BreakEvenExtraPips);
                            if (position.StopLoss == newStopLoss)
                            {
                                return;
                            }
                            else
                                ModifyPosition(position, newStopLoss, position.TakeProfit);
                            //Print("Stop Loss to Break Even set for SELL position {0}", position.Id);
                        }
                    }
                }
            }
        }
    }
}
