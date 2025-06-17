using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Field.Scripts
{
    public partial class Tile : MonoBehaviour
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        
        public string Name { get; private set; }

        public bool IsAvailable { get; private set; }
        
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        public void CheckIsAvailable(List<Tile> allTilesList)
        {
            IsAvailable =
                // If this tile hasn't neighbored tiles on the left and the right sides
                !(allTilesList.Any(t => t.Y == Y && t.Z == Z && t.X == X - 1) && allTilesList.Any(t => t.Y == Y && t.Z == Z && t.X == X + 1)) &&
                // If this tile hasn't any tile on the top
                !allTilesList.Any(t => t.Z == Z + 1 && t.X == X - 1 && t.Y == Y - 1) &&
                !allTilesList.Any(t => t.Z == Z + 1 && t.X == X && t.Y == Y - 1) &&
                !allTilesList.Any(t => t.Z == Z + 1 && t.X == X + 1 && t.Y == Y - 1) &&
                !allTilesList.Any(t => t.Z == Z + 1 && t.X == X - 1 && t.Y == Y) &&
                !allTilesList.Any(t => t.Z == Z + 1 && t.X == X && t.Y == Y) &&
                !allTilesList.Any(t => t.Z == Z + 1 && t.X == X + 1 && t.Y == Y) &&
                !allTilesList.Any(t => t.Z == Z + 1 && t.X == X - 1 && t.Y == Y + 1) &&
                !allTilesList.Any(t => t.Z == Z + 1 && t.X == X && t.Y == Y + 1) &&
                !allTilesList.Any(t => t.Z == Z + 1 && t.X == X + 1 && t.Y == Y + 1);
            
            backgroundSpriteRenderer.color =
                iconSpriteRenderer.color =
                    IsAvailable ? Color.white : Color.gray;
        }

        private void SetIcon(Sprite sprite)
        {
            iconSpriteRenderer.sprite = sprite;
        }
    }
}