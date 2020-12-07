using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public class Table
    {
        public static Dictionary<string, int> xAxisConverter = new Dictionary<string, int>
        {
            { "a", 1 },
            { "b", 2 },
            { "c", 3 },
            { "d", 4 },
            { "e", 5 },
            { "f", 6 },
            { "g", 7 },
            { "h", 8 },
        };

        public static Dictionary<int, string> xAxis = new Dictionary<int, string>()
        {
            { 1, "a" },
            { 2, "b" },
            { 3, "c" },
            { 4, "d" },
            { 5, "e" },
            { 6, "f" },
            { 7, "g" },
            { 8, "h" },
        };
        public static Dictionary<int, string> yAxis = new Dictionary<int, string>()
        {
            { 1, "1" },
            { 2, "2" },
            { 3, "3" },
            { 4, "4" },
            { 5, "5" },
            { 6, "6" },
            { 7, "7" },
            { 8, "8" }
        };

        public List<Location> Locations { get; private set; }
        public List<IStone> Stones { get; private set; }

        private Table()
        {
            Locations = new List<Location>();
            Stones = new List<IStone>();
        }

        public static Table CreateOne()
        {
            var table = new Table();

            foreach (var x in xAxis.Keys)
            {
                string xVal = xAxis[x];

                foreach (var y in yAxis.Keys)
                {
                    string yVal = yAxis[y];
                    table.Locations.Add(new Location($"{xVal}{yVal}", x, y));
                }
            }

            return table;
        }

        internal void AddStones(List<IStone> stones)
        {
            Stones.AddRange(stones);
        }

        public Location GetLocation(int x, int y) => Locations.FirstOrDefault(l => l.X == x && l.Y == y);
    }
}
