using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Util
{
    public static class ChartHelper
    {
        public static List<ChartTimeData> GetDailyReceived(DateTime date)
        {
            List<ChartTimeData> timeData = new List<ChartTimeData>();

            List<Event> events = EventHelper.GetEventsByDate(date);

            timeData.AddRange(events.Select(e => new ChartTimeData() { x = e.EventDate, y=1 }));

            return timeData;
        }
    }
}
