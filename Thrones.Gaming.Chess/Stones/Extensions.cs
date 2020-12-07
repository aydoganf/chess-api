using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrones.Gaming.Chess.Coordinate;

namespace Thrones.Gaming.Chess.Stones
{
    public static class Extensions
    {
        public static IStone GetFromLocation(this List<IStone> stones, Location location)
        {
            return stones.FirstOrDefault(s => s.Location == location || s.GhostLocation == location);
        }
    }
}
