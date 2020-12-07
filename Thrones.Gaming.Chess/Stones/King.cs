using System;
using System.Collections.Generic;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class King : Stone
    {
        internal King(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public King(EnumStoneColor color, int x, int y) : base(string.Empty, true, color, SessionFactory.GetTable().GetLocation(x, y), null)
        {
        }

        public override List<Location> GetMovementLocations(Location target, Table table)
        {
            return null;
        }

        public override bool TryMove(Location target, Table table, out IStone willEated)
        {
            willEated = null;

            if (CheckMove(target, table) == false)
            {
                return false;
            }

            var span = target - Location;

            // yatay - dikey hareket
            if (span.XDiff == 0 || span.YDiff == 0)
            {
                if (MovementRules.HorizontalOrVerticalCheck(Location, target, table) == false)
                {
                    return false;
                }
            }

            // çapraz
            else
            {
                if (MovementRules.DiagonalCheck(Location, target, table) == false)
                {
                    return false;
                }
            }

            willEated = table.Stones.GetFromLocation(target);
            return true;
        }

        protected override bool CheckMove(Location target, Table table)
        {
            // hedef lokasyonda kendi taşı var
            if (Player.Stones.FirstOrDefault(s => s.Location == target) != null)
            {
                return false;
            }

            var span = target - Location;

            if (span.XDiff > 1 || span.YDiff > 1)
            {
                return false;
            }

            return true;
        }

        public bool CouldRun(Table table)
        {
            IStone _s = null;
            bool leftOpen = false;
            bool leftTopOpen = false;
            bool topOpen = false;
            bool rightTopOpen = false;
            bool rightOpen = false;
            bool rightBottomOpen = false;
            bool bottomOpen = false;
            bool leftBottomOpen = false;
            var enemyStones = table.Stones.Where(s => s.Player != this.Player);

            #region check left-open

            // left
            var left = table.GetLocation(Location.X - 1, Location.Y);
            if (left != null && TryMove(left, table, out _s))
            {
                leftOpen = true;
                GhostMove(left);
                foreach (var enemyStone in enemyStones)
                {
                    if (enemyStone.TryMove(left, table, out IStone _e))
                    {
                        leftOpen = false;
                        break;
                    }
                }
                UndoGhost();
            }

            if (leftOpen) { return true; }

            #endregion

            #region check leftTop-open

            // left-top
            var leftTop = table.GetLocation(Location.X - 1, Location.Y + 1);
            if (leftTop != null && TryMove(leftTop, table, out _s))
            {
                leftTopOpen = true;
                GhostMove(leftTop);
                foreach (var enemyStone in enemyStones)
                {
                    if (enemyStone.TryMove(leftTop, table, out IStone _e))
                    {
                        leftTopOpen = false;
                        break;
                    }
                }
                UndoGhost();
            }

            if (leftTopOpen) { return true; }

            #endregion

            #region check top open

            // top
            var top = table.GetLocation(Location.X, Location.Y + 1);
            if (top != null && TryMove(top, table, out _s))
            {
                topOpen = true;
                GhostMove(top);
                foreach (var enemtStone in enemyStones)
                {
                    if (enemtStone.TryMove(top, table, out IStone _e))
                    {
                        topOpen = false;
                    }
                }
                UndoGhost();
            }

            if (topOpen) { return true; }

            #endregion

            #region check rightTop-open

            // right-top
            var rightTop = table.GetLocation(Location.X + 1, Location.Y + 1);
            if (rightTop != null && TryMove(rightTop, table, out _s))
            {
                rightTopOpen = true;
                GhostMove(rightTop);
                foreach (var enemtStone in enemyStones)
                {
                    if (enemtStone.TryMove(rightTop, table, out IStone _e))
                    {
                        rightTopOpen = false;
                    }
                }
                UndoGhost();
            }

            if (rightTopOpen) { return true; }

            #endregion

            #region check right open

            // right
            var right = table.GetLocation(Location.X + 1, Location.Y);
            if (right != null && TryMove(right, table, out _s))
            {
                rightOpen = false;
                GhostMove(right);
                foreach (var enemtStone in enemyStones)
                {
                    if (enemtStone.TryMove(right, table, out IStone _e))
                    {
                        rightOpen = false;
                    }
                }
                UndoGhost();
            }

            if (rightOpen) { return true; }

            #endregion

            #region check rightBottom-open

            // right-bottom
            var rightBottom = table.GetLocation(Location.X + 1, Location.Y - 1);
            if (rightBottom != null && TryMove(rightBottom, table, out _s))
            {
                rightBottomOpen = true;
                GhostMove(rightBottom);
                foreach (var enemtStone in enemyStones)
                {
                    if (enemtStone.TryMove(rightBottom, table, out IStone _e))
                    {
                        rightBottomOpen = false;
                    }
                }
                UndoGhost();
            }

            if (rightBottomOpen) { return true; }

            #endregion

            #region check bottom-open

            // bottom
            var bottom = table.GetLocation(Location.X, Location.Y - 1);
            if (bottom != null && TryMove(bottom, table, out _s))
            {
                bottomOpen = true;
                GhostMove(bottom);
                foreach (var enemtStone in enemyStones)
                {
                    if (enemtStone.TryMove(bottom, table, out IStone _e))
                    {
                        bottomOpen = false;
                    }
                }
                UndoGhost();
            }

            if (bottomOpen) { return true; }

            #endregion

            #region check leftBottom-open

            // left-bottom
            var leftBottom = table.GetLocation(Location.X - 1, Location.Y - 1);
            if (leftBottom != null && TryMove(leftBottom, table, out _s))
            {
                leftBottomOpen = true;
                GhostMove(leftBottom);
                foreach (var enemtStone in enemyStones)
                {
                    if (enemtStone.TryMove(leftBottom, table, out IStone _e))
                    {
                        leftBottomOpen = false;
                    }
                }
                UndoGhost();
            }

            return leftBottomOpen;

            #endregion
        }
    }
}
