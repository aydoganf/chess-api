using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.SessionManagement;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.Players
{
    public class Player
    {
        public string Nickname { get; private set; }
        public long Duration { get; internal set; }
        public List<IStone> Stones { get; private set; }
        public List<IStone> Eats { get; private set; }
        public EnumStoneColor Color { get; private set; }        

        private Player()
        {
        }

        internal static Player CreateOne(string nickname, EnumStoneColor color)
        {
            var player = new Player();
            player.Nickname = nickname;
            player.Stones = new List<IStone>();
            player.Eats = new List<IStone>();
            player.Color = color;
            return player;
        }

        internal void AddStone(IStone stone)
        {
            this.Stones.Add(stone);
            stone.SetPlayer(this);
        }

        internal void SetStones(List<IStone> stones)
        {
            if (Stones.Count != 0)
            {
                throw new InvalidOperationException($"Player {Nickname} has already stones.");
            }

            Stones = stones;
            foreach (var stone in Stones)
            {
                stone.SetPlayer(this);
            }
        }

        internal void Eat(IStone stone)
        {
            stone.Player.Stones.Remove(stone);
            Eats.Add(stone);
        }

        internal King GetKing()
        {
            if (Color == EnumStoneColor.Black)
            {
                return (King)Stones.FirstOrDefault(s => s is King);
            }

            return (King)Stones.FirstOrDefault(s => s is King);
        }
    }
}
