using JetBrains.Annotations;
using UnityEngine;

namespace Minesweeper {
    public sealed class MinesweeperUiInstance : MonoBehaviour {
        [SerializeField] private MinesweeperInstance minesweeperInstance;
        [SerializeField] private MinesweeperFrame minesweeperFrame;
        private IMinesweeperUI minesweeperUI;

        private void Start() {
            minesweeperUI = new MinesweeperUI(minesweeperFrame, minesweeperInstance.MinesweeperSystem);
        }

        [UsedImplicitly]
        public void Show() {
            minesweeperFrame.Show();
        }

        [UsedImplicitly]
        public void Hide() {
            minesweeperFrame.Hide();
        }

        [UsedImplicitly]
        public void StopAndHideGame() {
            minesweeperUI.StopAndHideGame();
        }

        [UsedImplicitly]
        public void StepToLeft() {
            minesweeperUI.StepToLeft();
        }
        
        [UsedImplicitly]
        public void StepToRight() {
            minesweeperUI.StepToRight();
        }
        
        [UsedImplicitly]
        public void StepToDown() {
            minesweeperUI.StepToDown();
        }
        
        [UsedImplicitly]
        public void StepToUp() {
            minesweeperUI.StepToUp();
        }

        [UsedImplicitly]
        public void OpenSelectedCell() {
            minesweeperUI.OpenSelectedCell();
        }

        [UsedImplicitly]
        public void FlagSelectedCell() {
            minesweeperUI.FlagSelectedCell();
        }
    }
}