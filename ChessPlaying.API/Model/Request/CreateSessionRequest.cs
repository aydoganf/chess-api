using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thrones.Gaming.Chess.Players;

namespace ChessPlaying.API.Model.Request
{
    public class CreateSessionRequest
    {
        public List<PlayerInformation> Players { get; set; }
    }
}
