using UnityEngine;

namespace Minesweeper {
    public sealed class MinesweeperFrameCellSelection : MonoBehaviour {
        [field: SerializeField] public RectTransform SelectionTransform { get; set; }
        public Vector2Int SelectionCellPosition { get; set; }

        public void SetSelectionScreenPosition(Vector2 selectionPosition) {
            SelectionTransform.position = selectionPosition;
        }

        public void SetSelectionSize(Vector2 size) {
            SelectionTransform.sizeDelta = size;
        }

        public void SetVisible(bool isVisible) {
            SelectionTransform.gameObject.SetActive(isVisible);
        }
    }
}