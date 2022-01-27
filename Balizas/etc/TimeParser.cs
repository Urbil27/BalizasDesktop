using System;
using System.Collections.Generic;
using System.Text;

namespace Balizas.etc
{
    internal class TimeParser
    {
        public int hours;
        public int minutes;
        public TimeParser(String time)
        {
            string[] timeSeparated = time.Split(":");
            hours = int.Parse(timeSeparated[0]);
            minutes = int.Parse(timeSeparated[1]);
        }
    }
}
