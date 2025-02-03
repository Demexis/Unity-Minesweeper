using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minesweeper {
    public partial class MinesweeperPlayback {
        private const int MAX_GENERATIONS = 99;
        
        private void GenerateMines(Vector2Int firstCell) {
            var generationAttempt = 0;
            
            do {
                ClearGeneration();
                PlaceMinesWhiteHole(Settings.countOfMines, firstCell);
                generationAttempt++;
                if (generationAttempt >= MAX_GENERATIONS) {
                    Debug.LogError($"Count of generation attempts exceeded {MAX_GENERATIONS}.");
                    break;
                }
            } while (!CheckThatCantBeWonByFirstTurn());
        }
        
        /// <summary>
        /// Places mines randomly, but more likely far away from the starting point.
        /// </summary>
        private void PlaceMinesWhiteHole(int countOfMines, Vector2Int firstCell) {
            var totalCellsCount = Field.Width * Field.Height;
            if (countOfMines >= totalCellsCount) {
                throw new Exception($"Given count of mines ({countOfMines}) should be less"
                    + $" than total count of all cells on the grid ({totalCellsCount}).");
            }

            var rand = new System.Random();

            var maxDistance = new Vector2Int(Field.Width, Field.Height).magnitude;
            
            for (var i = 0; i < countOfMines; i++) {
                var randomPos = new Vector2Int(Random.Range(0, Field.Width), Random.Range(0, Field.Height));

                var cell = Field.Grid[randomPos];

                if (cell.Mined || randomPos == firstCell) {
                    i--;
                    continue;
                }

                var randomPosDistance = (firstCell - randomPos).magnitude;

                var distanceMultiplier = randomPosDistance / maxDistance;

                var placeMine = ((float) rand.NextDouble() < distanceMultiplier);

                if (!placeMine) {
                    i--;
                    continue;
                }
                
                cell.Mined = true;
            }
        }
        
        private void PlaceMinesRandomly(int countOfMines, Vector2Int firstCell) {
            var totalCellsCount = Field.Width * Field.Height;
            if (countOfMines >= totalCellsCount) {
                throw new Exception($"Given count of mines ({countOfMines}) should be less"
                    + $" than total count of all cells on the grid ({totalCellsCount}).");
            }
            
            for (var i = 0; i < countOfMines; i++) {
                var randomPos = new Vector2Int(Random.Range(0, Field.Width), Random.Range(0, Field.Height));

                var cell = Field.Grid[randomPos];

                if (cell.Mined || randomPos == firstCell) {
                    i--;
                    continue;
                }

                cell.Mined = true;
            }
        }

        private void ClearGeneration() {
            ClearAllMines();
            
            void ClearAllMines() {
                for (var x = 0; x < Field.Width; x++) {
                    for (var y = 0; y < Field.Height; y++) {
                        var cellPosition = new Vector2Int(x, y);
                        Field.Grid[cellPosition].Mined = false;
                    }
                }
            }
        }
    }
}