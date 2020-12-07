using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.Movement
{
    public class MovementResultInformation
    {
        public bool IsOK { get; set; }
        public Location Location { get; set; }
        public string Message { get; set; }
        public bool CheckRemoved { get; set; }
        public bool Check { get; set; }
        public bool Checkmate { get; set; }
        public StoneInformation Stone { get; set; }

        public MovementResultInformation(MovementResult result, bool check = false, bool checkmate = false)
        {
            if (result == null)
            {
                return;
            }

            IsOK = result.IsOK;
            Location = result.Location;
            Message = result.Message;
            CheckRemoved = result.CheckRemoved;
            Stone = result.Stone == null ? null : new StoneInformation(result.Stone);
            Check = check;
            Checkmate = checkmate;
        }
    }
}
