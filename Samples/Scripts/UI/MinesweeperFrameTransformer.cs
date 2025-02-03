using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minesweeper {
    public sealed class MinesweeperFrameTransformer : MonoBehaviour {
        [field: SerializeField] public Vector2Int FrameSize { get; set; }
        [field: SerializeField] public RectTransform FrameStartTransform { get; set; }
        
        public MinesweeperCellUI CreateCellUI(int x, int y, MinesweeperCell cell, Vector2Int gridSize, 
            Action onLeftClick, Action onRightClick, Action onPointerEnter, Action onPointerExit) {
            var cellObj = new GameObject {
                name = $"({x}, {y})"
            };

            cellObj.transform.SetParent(FrameStartTransform);

            var rect = cellObj.AddComponent<RectTransform>();
            
            var button = cellObj.AddComponent<Button>(); // just for visuals
            button.navigation = new Navigation() {
                mode = Navigation.Mode.None
            };
            
            var rectEventTrigger = cellObj.AddComponent<EventTrigger>();

            var leftClickTrigger = new EventTrigger.Entry() {
                eventID = EventTriggerType.PointerClick
            };
            leftClickTrigger.callback.AddListener(eventData => {
                var mouseEventData = (PointerEventData)eventData;

                if (mouseEventData.button != PointerEventData.InputButton.Left) {
                    return;
                }
                
                onLeftClick.Invoke();
            });
            
            var rightClickTrigger = new EventTrigger.Entry() {
                eventID = EventTriggerType.PointerClick
            };
            rightClickTrigger.callback.AddListener(eventData => {
                var mouseEventData = (PointerEventData)eventData;

                if (mouseEventData.button != PointerEventData.InputButton.Right) {
                    return;
                }
                
                onRightClick.Invoke();
            });
            
            var pointerEnterTrigger = new EventTrigger.Entry() {
                eventID = EventTriggerType.PointerEnter
            };
            pointerEnterTrigger.callback.AddListener(_ => {
                onPointerEnter.Invoke();
            });
            
            var pointerExitTrigger = new EventTrigger.Entry() {
                eventID = EventTriggerType.PointerExit
            };
            pointerExitTrigger.callback.AddListener(_ => {
                onPointerExit.Invoke();
            });
            
            rectEventTrigger.triggers.Add(leftClickTrigger);
            rectEventTrigger.triggers.Add(rightClickTrigger);
            rectEventTrigger.triggers.Add(pointerEnterTrigger);
            rectEventTrigger.triggers.Add(pointerExitTrigger);

            var backgroundImage = cellObj.AddComponent<Image>();

            var foregroundObj = new GameObject();
            var foregroundRect = foregroundObj.AddComponent<RectTransform>();
            foregroundObj.transform.SetParent(cellObj.transform);

            var foregroundImage = foregroundObj.AddComponent<Image>();

            var rectSize = CalculateCellSize(gridSize);
            
            rect.sizeDelta = rectSize;
            rect.localScale = new Vector3(1, 1, 1);
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(0, 0);

            foregroundRect.sizeDelta = rectSize;

            var slotPos = CalculateCellPosition(new Vector2Int(x, y), gridSize);
            rect.position = slotPos;
            foregroundRect.anchoredPosition = Vector2.zero;

            return new MinesweeperCellUI(cell, cellObj, foregroundImage, backgroundImage, onLeftClick, onRightClick);
        }

        public Vector2 CalculateCellPosition(Vector2Int cellPosition, Vector2Int gridSize) {
            var cellSize = CalculateCellSize(gridSize);
            
            var slotPos = cellSize * cellPosition;
            var sizeOffset = cellSize / 2f;

            var slotAnchoredPos = (Vector2)FrameStartTransform.position + (slotPos + sizeOffset) * FrameStartTransform.lossyScale;

            return slotAnchoredPos;
        }

        public Vector2 CalculateCellSize(Vector2Int gridSize) {
            return new Vector2(FrameSize.x / (float)gridSize.x, FrameSize.y / (float)gridSize.y);
        }
    }
}