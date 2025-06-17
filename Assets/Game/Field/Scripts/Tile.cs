using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.Field.Scripts
{
    public partial class Tile : MonoBehaviour
    {
        private FieldController _fieldController;
        
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        
        public string Name { get; private set; }

        public bool IsAvailable { get; private set; }
        
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        [Inject]
        private void Construct(FieldController fieldController)
        {
            _fieldController = fieldController;
        }

        private void OnDestroy()
        {
            _fieldController.Tiles.Remove(this);
            
            var tilesToCheckAvailability = _fieldController.Tiles.Where(t =>
                (t.Z == Z && t.X == X - 2 && t.Y == Y) ||
                (t.Z == Z && t.X == X + 2 && t.Y == Y) ||
                (t.Z == Z - 1 && t.X == X - 1 && t.Y == Y - 1) ||
                (t.Z == Z - 1 && t.X == X && t.Y == Y - 1) ||
                (t.Z == Z - 1 && t.X == X + 1 && t.Y == Y - 1) ||
                (t.Z == Z - 1 && t.X == X - 1 && t.Y == Y) ||
                (t.Z == Z - 1 && t.X == X && t.Y == Y) ||
                (t.Z == Z - 1 && t.X == X + 1 && t.Y == Y) ||
                (t.Z == Z - 1 && t.X == X - 1 && t.Y == Y + 1) ||
                (t.Z == Z - 1 && t.X == X && t.Y == Y + 1) ||
                (t.Z == Z - 1 && t.X == X + 1 && t.Y == Y + 1)
                ).ToArray();

            foreach (var tile in tilesToCheckAvailability)
            {
                tile.CheckIsAvailable();
            }
            
            Destroy(gameObject);
        }

        public void SetSelected(bool isSelected)
        {
            backgroundSpriteRenderer.color = isSelected ? Color.green : Color.white;
        }
        
        public void CheckIsAvailable()
        {
            IsAvailable =
                // If this tile hasn't neighbored tiles on the left and the right sides
                !(_fieldController.Tiles.Any(t => t.Y == Y && t.Z == Z && t.X == X - 1) && _fieldController.Tiles.Any(t => t.Y == Y && t.Z == Z && t.X == X + 1)) &&
                // If this tile hasn't any tile on the top
                !_fieldController.Tiles.Any(t => t.Z == Z + 1 && t.X == X - 1 && t.Y == Y - 1) &&
                !_fieldController.Tiles.Any(t => t.Z == Z + 1 && t.X == X && t.Y == Y - 1) &&
                !_fieldController.Tiles.Any(t => t.Z == Z + 1 && t.X == X + 1 && t.Y == Y - 1) &&
                !_fieldController.Tiles.Any(t => t.Z == Z + 1 && t.X == X - 1 && t.Y == Y) &&
                !_fieldController.Tiles.Any(t => t.Z == Z + 1 && t.X == X && t.Y == Y) &&
                !_fieldController.Tiles.Any(t => t.Z == Z + 1 && t.X == X + 1 && t.Y == Y) &&
                !_fieldController.Tiles.Any(t => t.Z == Z + 1 && t.X == X - 1 && t.Y == Y + 1) &&
                !_fieldController.Tiles.Any(t => t.Z == Z + 1 && t.X == X && t.Y == Y + 1) &&
                !_fieldController.Tiles.Any(t => t.Z == Z + 1 && t.X == X + 1 && t.Y == Y + 1);
            
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