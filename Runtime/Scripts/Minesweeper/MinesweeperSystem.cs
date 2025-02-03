using System;
using System.Collections.Generic;

namespace Minesweeper {
    public interface IMinesweeperSystem {
        event Action<MinesweeperPlayback> StartedNewGame;
        event Action<MinesweeperPlayback> StoppedGame;
        
        MinesweeperPlayback StartNewGame(MinesweeperSettings settings, Action onWin, Action onLose);

        void StopGame(MinesweeperPlayback playback);
        void Update(float deltaTime);
    }
    
    public sealed class MinesweeperSystem : IMinesweeperSystem {
        public event Action<MinesweeperPlayback> StartedNewGame = delegate { };
        public event Action<MinesweeperPlayback> StoppedGame = delegate {  };

        private readonly List<MinesweeperPlayback> playbacks = new();
        
        public MinesweeperPlayback StartNewGame(MinesweeperSettings settings, Action onWin, Action onLose) {
            var playback = new MinesweeperPlayback(settings);
            playback.OnWin += onWin;
            playback.OnLose += onLose;
            playbacks.Add(playback);
            
            StartedNewGame.Invoke(playback);
            return playback;
        }

        public void StopGame(MinesweeperPlayback playback) {
            if (!playbacks.Remove(playback)) {
                return;
            }

            StoppedGame.Invoke(playback);
        }

        public void Update(float deltaTime) {
            // reversed for-loop prevents enumeration exception
            // when playback is being removed on OnLose event invoke
            for (var i = playbacks.Count - 1; i >= 0; i--) {
                var playback = playbacks[i];
                playback.Update(deltaTime);
            }
        }
    }
}