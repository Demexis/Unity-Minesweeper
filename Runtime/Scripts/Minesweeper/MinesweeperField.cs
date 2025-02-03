using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public sealed class MinesweeperField {
        public Dictionary<Vector2Int, MinesweeperCell> Grid { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public MinesweeperField(int width, int height) {
            Grid = new Dictionary<Vector2Int, MinesweeperCell>();
            Width = width;
            Height = height;
            
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    var cellPosition = new Vector2Int(x, y);
                    Grid[cellPosition] = new MinesweeperCell(x, y);
                }
            }
        }
        
        public int GetNeighbourMinesCount(int x, int y) {
            var count = 0;
            var cellPos = new Vector2Int(x, y);
            
            foreach (var neighbourOffset in MapUtils.eightDirections) {
                var neighbourIndex = cellPos + neighbourOffset;
                    
                if (neighbourIndex.x < 0
                    || neighbourIndex.x >= Width
                    || neighbourIndex.y < 0
                    || neighbourIndex.y >= Height) {
                    continue;
                }
                
                var neighbourCell = Grid[neighbourIndex];
                if (neighbourCell.Mined) {
                    count++;
                }
            }
            return count;
        }
    }
}