using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public class SessionInformation
    {
        public string Name { get; set; }
        public string FullTypeName { get; set; }
        public int CurrentIndexer { get; set; }
        public List<PlayerInformation> Players { get; set; }
        public TableInformation Table { get; set; }
        public MovementResultInformation MovementResult { get; set; }
    }
}
