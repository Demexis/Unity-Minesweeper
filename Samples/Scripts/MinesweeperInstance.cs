using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Minesweeper {
    public sealed class MinesweeperInstance : MonoBehaviour {
        [SerializeField] private MinesweeperSettings minesweeperSettings; 
        public IMinesweeperSystem MinesweeperSystem { get; private set; }

        [CanBeNull] private MinesweeperPlayback minesweeperPlayback;

        private void Awake() {
            MinesweeperSystem = new MinesweeperSystem();
        }

        private void Update() {
            MinesweeperSystem.Update(Time.deltaTime);
        }

        [UsedImplicitly]
        public void StartNewGame() {
            StopGame();
            
            minesweeperPlayback = MinesweeperSystem.StartNewGame(minesweeperSettings,
                () => Debug.Log("You win! Custom win-logic has been executed."),
                () => Debug.Log("You lose. Custom lose-logic has been executed."));
        }

        [UsedImplicitly]
        public void StopGame() {
            if (minesweeperPlayback != null) {
                MinesweeperSystem.StopGame(minesweeperPlayback);
            }
        }
    }
}