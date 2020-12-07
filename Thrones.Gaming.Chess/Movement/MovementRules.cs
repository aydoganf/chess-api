using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.SessionManagement;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.Movement
{
    public static class MovementRules
    {
        public static bool DiagonalCheck(Location from, Location to, Table table)
        {
            int currentX = from.X;
            int currentY = from.Y;

            var span = to - from;

            for (int i = 1; i <= span.XDiff; i++)
            {
                if (span.XMovement == Direction.Forward)
                {
                    currentX += 1;
                }
                else
                {
                    currentX -= 1;
                }


                if (span.YMovement == Direction.Forward)
                {
                    currentY += 1;
                }
                else
                {
                    currentY -= 1;
                }
                

                var checkLocation = table.Locations.FirstOrDefault(l => l.X == currentX && l.Y == currentY);
                if (checkLocation == null)
                {
                    return false;
                }

                var checkLocationStone = table.Stones.GetFromLocation(checkLocation);
                if (checkLocationStone != null)
                {
                    if (i != span.XDiff)
                    {
                        // son nokta değil arada taş var
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool HorizontalOrVerticalCheck(Location from, Location to, Table table)
        {
            var span = to - from;

            int currentX = from.X;
            int currentY = from.Y;
            int targetLocationBorder = span.YDiff == 0 ? span.XDiff : span.YDiff;

            for (int i = 1; i <= targetLocationBorder; i++)
            {
                if (span.YDiff == 0)
                {
                    currentX += span.XMovement == Direction.Forward ? 1 : -1;
                }

                if (span.XDiff == 0)
                {
                    currentY += span.YMovement == Direction.Forward ? 1 : -1;
                }

                var checkLocation = table.GetLocation(currentX, currentY);
                if (checkLocation == null)
                {
                    return false;
                }

                var nextLocationStone = table.Stones.GetFromLocation(checkLocation);
                if (nextLocationStone != null)
                {
                    if (i != targetLocationBorder)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
