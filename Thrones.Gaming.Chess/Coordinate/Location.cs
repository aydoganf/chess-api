using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Coordinate
{
    public class Location
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public string Name => $"{Table.xAxis[X]}{Y}";


        public Location(string name, int x, int y)
        {
            //Name = name;
            X = x;
            Y = y;
        }

        public static LocationSpan operator -(Location target, Location current)
        {
            int xDiff = target.X - current.X;
            int yDiff = target.Y - current.Y;
            var xMovement = Direction.None;
            var yMovement = Direction.None;

            if (xDiff > 0)
            {
                xMovement = Direction.Forward;
            }
            else if (xDiff < 0) 
            {
                xDiff *= -1;
                xMovement = Direction.Backward;
            }

            if (yDiff > 0)
            {
                yMovement = Direction.Forward;
            }
            else if (yDiff < 0) 
            { 
                yDiff *= -1;
                yMovement = Direction.Backward;
            }

            return new LocationSpan(xDiff, yDiff, xMovement, yMovement);
        }
    }
}
