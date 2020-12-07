using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.Movement;

namespace Thrones.Gaming.Chess.Coordinate
{
    public struct LocationSpan
    {
        public int XDiff { get; private set; }
        public int YDiff { get; private set; }
        public Direction XMovement { get; private set; }
        public Direction YMovement { get; private set; }
        public bool IsDiagonal { get; private set; }


        public LocationSpan(int xDiff, int yDiff, Direction xMovement, Direction yMovement) : this()
        {
            XDiff = xDiff;
            YDiff = yDiff;
            XMovement = xMovement;
            YMovement = yMovement;
            IsDiagonal = xDiff == yDiff;
        }
    }
}
