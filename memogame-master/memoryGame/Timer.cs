using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memoryGame
{
    class Stoper
    {
        public static int seconds = 0;
        public static int minutes = 0;
        public static string zeroSeconds = "0";
        public static string zeroMinutes = "0";
        public static void Timer()
        {
            seconds++;
            if (seconds > 9) zeroSeconds = "";
            else zeroSeconds = "0";
            if (minutes > 9) zeroMinutes = "";
            else zeroMinutes = "0";

            if (seconds == 59 && minutes <= 59)
            {
                minutes++;
                seconds = 0;

            }
            else return;
        }
        public static void TimerReset()
        {
            seconds = 0;
            minutes = 0;
        }
    }
}
