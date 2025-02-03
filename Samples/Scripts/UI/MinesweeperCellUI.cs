using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper {
    public sealed class MinesweeperCellUI {
        public readonly MinesweeperCell cell;
        public readonly GameObject cellObject;
        public readonly Image foregroundImage; 
        public readonly Image backgroundImage;

        public readonly Action leftClickCallback;
        public readonly Action rightClickCallback;

        public MinesweeperCellUI(MinesweeperCell cell, GameObject cellObject, Image foregroundImage, Image backgroundImage,
            Action leftClickCallback, Action rightClickCallback) {
            this.cell = cell;
            this.cellObject = cellObject;
            this.foregroundImage = foregroundImage;
            this.backgroundImage = backgroundImage;
            this.leftClickCallback = leftClickCallback;
            this.rightClickCallback = rightClickCallback;
        }
    }
}