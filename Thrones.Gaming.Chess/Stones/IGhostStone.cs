using Thrones.Gaming.Chess.Coordinate;

namespace Thrones.Gaming.Chess.Stones
{
    public interface IGhostStone : IStone
    {
        bool Move(Location target);
    }
}
