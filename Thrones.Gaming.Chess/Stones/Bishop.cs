using System;
using System.Collections.Generic;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Bishop : Stone
    {
        internal Bishop(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public Bishop(EnumStoneColor color, int x, int y) : base(string.Empty, true, color, SessionFactory.GetTable().GetLocation(x, y), null)
        {
        }

        public override List<Location> GetMovementLocations(Location target, Table table)
        {
            List<Location> result = null;

            if (CheckMove(target, table))
            {
                result = new List<Location>();

                var span = target - Location;
                int currentX = Location.X;
                int currentY = Location.Y;

                for (int i = 1; i <= span.XDiff; i++)
                {
                    currentX += span.XMovement == Direction.Forward ? 1 : -1;
                    currentY += span.YMovement == Direction.Forward ? 1 : -1;

                    var location = table.GetLocation(currentX, currentY);
                    result.Add(location);
                }
            }

            return result;
        }

        public override bool TryMove(Location target, Table table, out IStone willEated)
        {
            willEated = default;
            if (CheckMove(target, table) == false)
            {
                return false;
            }

            //int currentX = Location.X;
            //int currentY = Location.Y;

            //var span = target - Location;

            //for (int i = 1; i <= span.XDiff; i++)
            //{
            //    currentX += span.XMovement == MovementDirection.Forward ? 1 : -1;
            //    currentY += span.YMovement == MovementDirection.Forward ? 1 : -1;                

            //    var checkLocation = table.GetLocation(currentX, currentY);
            //    if (checkLocation == null)
            //    {
            //        return false;
            //    }

            //    var checkLocationStone = table.Stones.GetFromLocation(checkLocation);
            //    if (checkLocationStone != null)
            //    {
            //        if (i != span.XDiff)
            //        {
            //            // son nokta değil arada taş var
            //            return false;
            //        }
            //        else
            //        {
            //            willEated = checkLocationStone;
            //            break;
            //        }
            //    }
            //}

            if (MovementRules.DiagonalCheck(Location, target, table) == false)
            {
                return false;
            }

            willEated = table.Stones.GetFromLocation(target);
            if (willEated == this)
            {
                willEated = null;
            }

            return true;
        }

        protected override bool CheckMove(Location target, Table table)
        {
            // gidilecek location'da kendi taşı varsa
            if (Player.Stones.FirstOrDefault(s => s.Location == target) != null)
            {
                return false;
            }

            var span = target - Location;

            if (span.IsDiagonal == false)
            {
                return false;
            }

            return true;
        }
    }
}
