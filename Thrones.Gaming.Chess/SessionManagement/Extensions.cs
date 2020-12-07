using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public static class Extensions
    {
        public static string ToMinute(this long time)
        {
            var hours = time / 360000;
            var minutes = (time - (hours * 360000)) / 60000;
            var seconds = (time - (hours * 360000) - (minutes * 60000)) / 1000;
            var milliseconds = time - (hours * 360000) - (minutes * 60000) - (seconds * 1000);

            return $"{hours}h: {minutes}m: {seconds}.{milliseconds}s";
        }

        public static TValue GetLast<TKey, TValue>(this Dictionary<TKey, TValue> keyValuePairs)
        {
            TKey lastKey = keyValuePairs.Keys.LastOrDefault();

            if (lastKey is null)
            {
                return (TValue)default;
            }

            return keyValuePairs[lastKey];
        }
    }
}
