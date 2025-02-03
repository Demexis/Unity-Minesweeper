namespace Minesweeper {
    public sealed class MinesweeperCell {
        public bool Closed { get; set; }
        public bool Mined { get; set; }
        public bool Flagged { get; set; }

        public readonly int x;
        public readonly int y;

        public MinesweeperCell(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
}