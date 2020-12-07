namespace Thrones.Gaming.Chess.Stones
{
    public sealed class StoneInformation
    {
        public string Type { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }
        public int MoveCount { get; set; }

        public StoneInformation(IStone stone)
        {
            Type = stone.GetType().Name;
            Color = stone.Color.ToString();
            Location = $"{stone.Location.X}|{stone.Location.Y}";
            MoveCount = stone.MoveCount;
        }

        public StoneInformation()
        {
        }

        public EnumStoneColor GetStoneColor()
        {
            if (Color == "Black")
            {
                return EnumStoneColor.Black;
            }

            return EnumStoneColor.White;
        }

        public int GetXLocation() => int.Parse(Location.Split("|")[0]);
        public int GetYLocation() => int.Parse(Location.Split("|")[1]);
        public string GetFullTypeName() => $"Thrones.Gaming.Chess.Stones.{Type}";
    }
}
