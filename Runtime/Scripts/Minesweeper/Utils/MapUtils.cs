using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Minesweeper {
    [PublicAPI]
    public static class MapUtils {
        public static readonly Vector2Int[] fourDirections = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        public static readonly Vector2Int[] cornerDirections = {
            Vector2Int.up + Vector2Int.left,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.down + Vector2Int.left,
            Vector2Int.down + Vector2Int.right
        };

        public static readonly Vector2Int[] fiveDirections =
            new[] { Vector2Int.zero }.Concat(fourDirections).ToArray();

        public static readonly Vector2Int[] nineDirections =
            fiveDirections.Concat(cornerDirections).ToArray();

        public static readonly Vector2Int[] eightDirections =
            fourDirections.Concat(cornerDirections).ToArray();
    }
}
