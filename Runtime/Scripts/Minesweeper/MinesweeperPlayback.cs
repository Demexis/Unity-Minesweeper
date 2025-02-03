using System;
using UnityEngine;

namespace Minesweeper {
    public sealed partial class MinesweeperPlayback {
        public enum State {
            Awaiting,
            Going,
            Win,
            Lost
        }
        
        public event Action OnLose;
        public event Action OnWin;
        public event Action<MinesweeperCell> MineExploded;
        public event Action<MinesweeperCell> CellOpened;
        public event Action<MinesweeperCell> CellFlagged;
        public event Action<MinesweeperCell> CellUnflagged;
        
        public MinesweeperField Field { get; }

        public State CurrentState { get; private set; }

        public MinesweeperSettings Settings { get; }
        
        private bool IsTurnAllowed => CurrentState is State.Awaiting or State.Going;

        public MinesweeperPlayback(MinesweeperSettings settings) {
            if (settings.width <= 0) {
                throw new Exception("Width should be greater than zero.");
            }

            if (settings.height <= 0) {
                throw new Exception("Height should be greater than zero.");
            }

            if (settings.countOfMines <= 0) {
                throw new Exception("Count of mines should be greater than zero.");
            }

            Settings = settings;
            Field = new MinesweeperField(Settings.width, Settings.height);
            CurrentState = State.Awaiting;

            if (settings.hasTimer) {
                TurnTimer = settings.timeValue;
            }
            
            CloseAllCells();

            MineExploded += _ => {
                Lose();
            };

            CellOpened += _ => {
                ResetTimer();
                if (CheckIfWon()) {
                    Win();
                }
            };
        }

        private void CloseAllCells() {
            for (var x = 0; x < Field.Width; x++) {
                for (var y = 0; y < Field.Height; y++) {
                    var cellPosition = new Vector2Int(x, y);
                    Field.Grid[cellPosition].Closed = true;
                }
            }
        }

        public void Update(float deltaTime) {
            UpdateTimer(deltaTime);
        }

        public bool OpenCell(int x, int y) {
            if (!IsTurnAllowed) {
                return false;
            }
            
            var cellPosition = new Vector2Int(x, y);
            var cell = Field.Grid[cellPosition];

            if (cell.Flagged) {
                return false;
            }

            if (CurrentState == State.Awaiting) {
                GenerateMines(new Vector2Int(x, y));
                CurrentState = State.Going;
            }

            if (!cell.Closed) {
                return false;
            }
            
            if (cell.Mined) {
                MineExploded?.Invoke(cell);
                return false;
            }

            OpenAllSafeNeighboursRecursive(x, y);
            return true;
        }

        public void FlagCell(int x, int y) {
            if (!IsTurnAllowed) {
                return;
            }
            
            var cellPosition = new Vector2Int(x, y);
            var cell = Field.Grid[cellPosition];

            if (!cell.Closed) {
                return;
            }
            
            cell.Flagged = !cell.Flagged;

            if (cell.Flagged) {
                CellFlagged?.Invoke(cell);
            } else {
                CellUnflagged?.Invoke(cell);
            }
        }

        private void OpenAllSafeNeighboursRecursive(int x, int y) {
            var cellPosition = new Vector2Int(x, y);
            var cell = Field.Grid[cellPosition];

            if (!cell.Closed) {
                return;
            }
            
            var cellPos = new Vector2Int(x, y);

            cell.Closed = false;
            CellOpened?.Invoke(cell);

            var isNearMines = Field.GetNeighbourMinesCount(x, y) != 0;
            
            foreach (var neighbourOffset in MapUtils.eightDirections) {
                var neighbourPos = cellPos + neighbourOffset;
                
                if (neighbourPos.x < 0
                    || neighbourPos.x >= Field.Width
                    || neighbourPos.y < 0
                    || neighbourPos.y >= Field.Height) {
                    continue;
                }
                
                // don't open mines
                if (Field.Grid[neighbourPos].Mined) {
                    continue;
                }

                if (isNearMines) {
                    if (Field.GetNeighbourMinesCount(neighbourPos.x, neighbourPos.y) != 0) {
                        continue;
                    }
                }
                
                OpenAllSafeNeighboursRecursive(neighbourPos.x, neighbourPos.y);
            }
        }

        private void Lose() {
            if (CurrentState == State.Lost) {
                return;
            }
            
            CurrentState = State.Lost;
            OnLose?.Invoke();
        }

        private void Win() {
            if (CurrentState == State.Win) {
                return;
            }
            
            CurrentState = State.Win;
            OnWin?.Invoke();
        }
    }
}