using System.Collections.Generic;
using Thrones.Gaming.Chess.Players;

namespace ChessPlaying.API.Model.Request
{
    public class CreateSessionRequest
    {
        public List<PlayerInformation> Players { get; set; }
    }
}
