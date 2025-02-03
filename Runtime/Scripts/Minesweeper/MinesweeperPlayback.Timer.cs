namespace Minesweeper {
    public partial class MinesweeperPlayback {
        public float TurnTimer { get; private set; }
        
        private void UpdateTimer(float deltaTime) {
            if (CurrentState != State.Going) {
                return;
            }
            
            if (!Settings.hasTimer) {
                return;
            }

            if (TurnTimer > 0) {
                TurnTimer -= deltaTime;
                return;
            }

            Lose();
        }

        private void ResetTimer() {
            TurnTimer = Settings.timeValue;
        }
    }
}