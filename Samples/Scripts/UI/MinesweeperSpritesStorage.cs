using UnityEngine;

namespace Minesweeper {
    [CreateAssetMenu(fileName = nameof(MinesweeperSpritesStorage), menuName = "ScriptableObjects/Storages/" + nameof(MinesweeperSpritesStorage),
        order = 1)]
    public sealed class MinesweeperSpritesStorage : ScriptableObject {
        public Sprite closedCellSprite;
        public Sprite openCellSprite;

        public Sprite invisibleSprite;
        public Sprite zeroMinesSprite;
        public Sprite oneMineSprite;
        public Sprite twoMinesSprite;
        public Sprite threeMinesSprite;
        public Sprite fourMinesSprite;
        public Sprite fiveMinesSprite;
        public Sprite sixMinesSprite;
        public Sprite sevenMinesSprite;
        public Sprite eightMinesSprite;
        
        public Sprite mineSprite;
        public Sprite explodedSprite;
        public Sprite defusedSprite;
        public Sprite flaggedSprite;
    }
}