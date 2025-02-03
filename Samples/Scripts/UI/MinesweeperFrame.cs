using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper {
    public sealed class MinesweeperFrame : MonoBehaviour {
        [SerializeField] private MinesweeperSpritesStorage spritesStorage;

        [SerializeField] private Text messageText;
        [SerializeField] private Image progressBar;
        
        public event Action<MinesweeperPlayback> ConnectedPlayback = delegate { };
        public event Action<MinesweeperPlayback> DisconnectedPlayback = delegate { };
        
        public event Action<bool> VisibleChanged = delegate { };

        public bool IsVisible {
            get => isVisible;
            set {
                isVisible = value;
                VisibleChanged.Invoke(value);
            }
        }
        private bool isVisible;
        
        public MinesweeperFrameCellSelection CellSelection { get; private set; }
        public MinesweeperFrameTransformer Transformer { get; private set; }
        public RectTransform RectTransform { get; set; }
        [CanBeNull] public MinesweeperPlayback Playback { get; private set; }
        public Dictionary<Vector2Int, MinesweeperCellUI> CellsGrid { get; private set; }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        
        private CanvasGroup canvasGroup;

        private void Start() {
            CellSelection = GetComponent<MinesweeperFrameCellSelection>();
            Transformer = GetComponent<MinesweeperFrameTransformer>();
            RectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        private void Update() {
            if (Playback == null) {
                return;
            }
            
            if (Playback.CurrentState == MinesweeperPlayback.State.Going) {
                if (!string.IsNullOrWhiteSpace(messageText.text)) {
                    messageText.text = string.Empty;
                }
                
                var timerSecond = Mathf.FloorToInt(Playback.TurnTimer).ToString();

                if (messageText.text != timerSecond) {
                    messageText.text = timerSecond;
                }
            }
        }

        public void Show() {
            if (Playback == null) {
                return;
            }
            
            IsVisible = true;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            CellSelection.SelectionCellPosition = Vector2Int.zero;
            var gridSize = new Vector2Int(Playback.Field.Width, Playback.Field.Height);
            CellSelection.SetSelectionSize(Transformer.CalculateCellSize(gridSize));
            CellSelection.SetSelectionScreenPosition(Transformer.CalculateCellPosition(CellSelection.SelectionCellPosition, gridSize));
            CellSelection.SetVisible(true);
        }

        public void Hide() {
            IsVisible = false;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            CellSelection.SelectionCellPosition = Vector2Int.zero;
            CellSelection.SetVisible(false);
        }

        public void ConnectPlayback([NotNull] MinesweeperPlayback playback) {
            if (Playback != null) {
                DisconnectPlayback();
            }
            Playback = playback;
            ClearField();
            SetupField(playback.Field);
            Playback.CellOpened += UpdateCellSprite;
            Playback.CellOpened += _ => RecalculateProgress();
            Playback.CellFlagged += UpdateCellSprite;
            Playback.CellUnflagged += UpdateCellSprite;
            Playback.OnLose += ShowLose; 
            Playback.OnWin += ShowWin; 
            ConnectedPlayback.Invoke(playback);

            progressBar.fillAmount = 0f;
            messageText.text = "Awaiting";
        }

        public void DisconnectPlayback() {
            if (Playback == null) {
                return;
            }
            
            var previousPlayback = Playback;
            Playback = null;
            ClearField();
            DisconnectedPlayback.Invoke(previousPlayback);
        }
        
        private void ClearField() {
            if (CellsGrid == null) {
                return;
            }
            
            for (var x = 0; x < GridWidth; x++) {
                for (var y = 0; y < GridHeight; y++) {
                    var cellPos = new Vector2Int(x, y);
                    Destroy(CellsGrid[cellPos].cellObject);
                }
            }
            
            CellsGrid = null;
        }

        private void SetupField(MinesweeperField field) {
            CellsGrid = new Dictionary<Vector2Int, MinesweeperCellUI>();
            GridWidth = field.Width;
            GridHeight = field.Height;
            CreateCells();
            
            void CreateCells() {
                var cellsGridSize = new Vector2Int(GridWidth, GridHeight);
                for (var x = 0; x < GridWidth; x++) {
                    for (var y = 0; y < GridHeight; y++) {
                        var cellPos = new Vector2Int(x, y);
                        var cell = field.Grid[cellPos];
                        CellsGrid[cellPos] = Transformer.CreateCellUI(x, y, cell, cellsGridSize, () => {
                            if (Playback == null) {
                                return;
                            }

                            Playback.OpenCell(cellPos.x, cellPos.y);
                        }, () => {
                            Playback?.FlagCell(cellPos.x, cellPos.y);
                        }, () => {
                            CellSelection.SelectionCellPosition = cellPos;
                            CellSelection.SetSelectionScreenPosition(Transformer.CalculateCellPosition(cellPos, cellsGridSize));
                            CellSelection.SetVisible(true);
                        }, () => {
                            CellSelection.SetVisible(false);
                        });
                        UpdateCellSprite(cell);
                    }
                }
            }
        }

        private void UpdateCellSprite(MinesweeperCell cell) {
            if (Playback == null) {
                return;
            }
            
            var cellPos = new Vector2Int(cell.x, cell.y);
            var uiCell = CellsGrid[cellPos];

            uiCell.backgroundImage.sprite =
                cell.Closed ? spritesStorage.closedCellSprite : spritesStorage.openCellSprite;
            
            if (cell.Closed) {
                uiCell.foregroundImage.sprite = cell.Flagged ? spritesStorage.flaggedSprite : spritesStorage.invisibleSprite;
                return;
            }

            uiCell.foregroundImage.sprite = Playback.Field.GetNeighbourMinesCount(cell.x, cell.y) switch {
                0 => spritesStorage.zeroMinesSprite,
                1 => spritesStorage.oneMineSprite,
                2 => spritesStorage.twoMinesSprite,
                3 => spritesStorage.threeMinesSprite,
                4 => spritesStorage.fourMinesSprite,
                5 => spritesStorage.fiveMinesSprite,
                6 => spritesStorage.sixMinesSprite,
                7 => spritesStorage.sevenMinesSprite,
                8 => spritesStorage.eightMinesSprite,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void ShowLose() {
            if (CellsGrid == null) {
                return;
            }
            
            for (var x = 0; x < GridWidth; x++) {
                for (var y = 0; y < GridHeight; y++) {
                    var cellPosition = new Vector2Int(x, y);
                    var uiCell = CellsGrid[cellPosition];
                    if (uiCell.cell.Mined) {
                        uiCell.foregroundImage.sprite = spritesStorage.mineSprite;
                        uiCell.backgroundImage.sprite = spritesStorage.explodedSprite;
                    }
                }
            }

            messageText.text = "Failed";
        }
        
        private void ShowWin() {
            if (CellsGrid == null) {
                return;
            }
            
            for (var x = 0; x < GridWidth; x++) {
                for (var y = 0; y < GridHeight; y++) {
                    var cellPosition = new Vector2Int(x, y);
                    var uiCell = CellsGrid[cellPosition];
                    uiCell.backgroundImage.color = Color.green;
                    
                    if (uiCell.cell.Mined) {
                        uiCell.foregroundImage.sprite = spritesStorage.mineSprite;
                        uiCell.backgroundImage.sprite = spritesStorage.defusedSprite;
                    }
                }
            }
            
            progressBar.fillAmount = 1f;
            messageText.text = "Defused";
        }

        private void RecalculateProgress() {
            if (Playback == null) {
                return;
            }
            
            var openedCells = 0;
            var neededToOpenCells = GridWidth * GridHeight - Playback.Settings.countOfMines;
            
            for (var x = 0; x < GridWidth; x++) {
                for (var y = 0; y < GridHeight; y++) {
                    var cellPosition = new Vector2Int(x, y);
                    var cell = CellsGrid[cellPosition].cell;

                    if (!cell.Closed) {
                        openedCells++;
                    }
                }
            }

            var progress = openedCells / (float) neededToOpenCells;

            progressBar.fillAmount = progress;
        }
    }
}