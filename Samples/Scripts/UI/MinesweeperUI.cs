using System;
using UnityEngine;

namespace Minesweeper {
    public interface IMinesweeperUI {
        MinesweeperFrame Frame { get; }

        void StopAndHideGame();
        void StepToLeft();
        void StepToRight();
        void StepToDown();
        void StepToUp();
        void OpenSelectedCell();
        void FlagSelectedCell();
    }

    public sealed class MinesweeperUI : IMinesweeperUI, IDisposable {
        public MinesweeperFrame Frame { get; }

        private readonly IMinesweeperSystem minesweeperSystem;

        public MinesweeperUI(MinesweeperFrame frame, IMinesweeperSystem minesweeperSystem) {
            Frame = frame;
            this.minesweeperSystem = minesweeperSystem;

            minesweeperSystem.StartedNewGame += StartNewGame;
            minesweeperSystem.StoppedGame += StopGame;
        }

        public void Dispose() {
            minesweeperSystem.StartedNewGame -= StartNewGame;
            minesweeperSystem.StoppedGame -= StopGame;
        }

        private void StartNewGame(MinesweeperPlayback playback) {
            Frame.ConnectPlayback(playback);
        }

        private void StopGame(MinesweeperPlayback playback) {
            if (Frame.Playback == null) {
                return;
            }

            if (Frame.Playback != playback) {
                return;
            }

            Frame.DisconnectPlayback();
        }
        
        public void StopAndHideGame() {
            if (Frame.Playback == null) {
                return;
            }
            
            minesweeperSystem.StopGame(Frame.Playback);
        }

        public void StepToLeft() {
            if (Frame.Playback == null) {
                return;
            }
            
            var position = Frame.CellSelection.SelectionCellPosition + Vector2Int.left;
            
            if (position.x < 0) {
                return;
            }
            
            Frame.CellSelection.SelectionCellPosition = position;
            var fieldSize = new Vector2Int(Frame.Playback.Field.Width, Frame.Playback.Field.Height);
            Frame.CellSelection.SetSelectionScreenPosition(Frame.Transformer.CalculateCellPosition(position, fieldSize));
            Frame.CellSelection.SetVisible(true);
        }
        
        public void StepToRight() {
            if (Frame.Playback == null) {
                return;
            }
            
            var position = Frame.CellSelection.SelectionCellPosition + Vector2Int.right;
            
            if (position.x >= Frame.Playback.Field.Width) {
                return;
            }
            
            Frame.CellSelection.SelectionCellPosition = position;
            var fieldSize = new Vector2Int(Frame.Playback.Field.Width, Frame.Playback.Field.Height);
            Frame.CellSelection.SetSelectionScreenPosition(Frame.Transformer.CalculateCellPosition(position, fieldSize));
            Frame.CellSelection.SetVisible(true);
        }
        
        public void StepToDown() {
            if (Frame.Playback == null) {
                return;
            }
            
            var position = Frame.CellSelection.SelectionCellPosition + Vector2Int.down;
            
            if (position.y < 0) {
                return;
            }
            
            Frame.CellSelection.SelectionCellPosition = position;
            var fieldSize = new Vector2Int(Frame.Playback.Field.Width, Frame.Playback.Field.Height);
            Frame.CellSelection.SetSelectionScreenPosition(Frame.Transformer.CalculateCellPosition(position, fieldSize));
            Frame.CellSelection.SetVisible(true);
        }
        
        public void StepToUp() {
            if (Frame.Playback == null) {
                return;
            }
            
            var position = Frame.CellSelection.SelectionCellPosition + Vector2Int.up;
            
            if (position.y >= Frame.Playback.Field.Height) {
                return;
            }
            
            Frame.CellSelection.SelectionCellPosition = position;
            var fieldSize = new Vector2Int(Frame.Playback.Field.Width, Frame.Playback.Field.Height);
            Frame.CellSelection.SetSelectionScreenPosition(Frame.Transformer.CalculateCellPosition(position, fieldSize));
            Frame.CellSelection.SetVisible(true);
        }

        public void OpenSelectedCell() {
            if (Frame.Playback == null) {
                return;
            }
            
            Frame.CellsGrid[Frame.CellSelection.SelectionCellPosition].leftClickCallback.Invoke();
        }

        public void FlagSelectedCell() {
            if (Frame.Playback == null) {
                return;
            }
            
            Frame.CellsGrid[Frame.CellSelection.SelectionCellPosition].rightClickCallback.Invoke();
        }
    }
}
