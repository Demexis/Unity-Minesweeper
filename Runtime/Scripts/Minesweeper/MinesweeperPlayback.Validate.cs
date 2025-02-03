using UnityEngine;

namespace Minesweeper {
    public partial class MinesweeperPlayback {
        private bool CheckThatCantBeWonByFirstTurn() {
            for (var x = 0; x < Field.Width; x++) {
                for (var y = 0; y < Field.Height; y++) {
                    if (CheckCell(x, y)) {
                        return true;
                    }
                }
            }

            return false;
            
            bool CheckCell(int x, int y) {
                var cellPosition = new Vector2Int(x, y);
                var cell = Field.Grid[cellPosition];
                if (cell.Mined) {
                    return false;
                }
                
                foreach (var direction in MapUtils.nineDirections) {
                    var neighbourPos = direction + new Vector2Int(x, y);
                    if (neighbourPos.x < 0
                        || neighbourPos.x >= Field.Width
                        || neighbourPos.y < 0
                        || neighbourPos.y >= Field.Height) {
                        continue;
                    }
                    
                    var neighbourCell = Field.Grid[neighbourPos];

                    if (neighbourCell.Mined) {
                        continue;
                    }

                    var neighbourMines = Field.GetNeighbourMinesCount(neighbourPos.x, neighbourPos.y);

                    if (neighbourMines == 0) {
                        return false;
                    }
                }

                return true;
            }
        }
        
        private bool CheckIfWon() {
            for (var x = 0; x < Field.Width; x++) {
                for (var y = 0; y < Field.Height; y++) {
                    var cellPosition = new Vector2Int(x, y);
                    var cell = Field.Grid[cellPosition];
                    if (cell.Closed && !cell.Mined) {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}