using System;

namespace Minesweeper {
    [Serializable]
    public struct MinesweeperSettings {
        // basic settings
        public int width;
        public int height;
        public int countOfMines;
        
        // timer settings
        public bool hasTimer;
        public float timeValue;

        public MinesweeperSettings(int width, int height, int countOfMines) : this() {
            this.width = width;
            this.height = height;
            this.countOfMines = countOfMines;
            hasTimer = false;
        }

        /// <summary>
        /// Use this constructor if you want to use timer functionality in the playback.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="countOfMines">Count of mines.</param>
        /// <param name="timeValue">Timer's value.</param>
        public MinesweeperSettings(int width, int height, int countOfMines, float timeValue) : this() {
            this.width = width;
            this.height = height;
            this.countOfMines = countOfMines;
            this.timeValue = timeValue;
            hasTimer = true;
        }
    }
}