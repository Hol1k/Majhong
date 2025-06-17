using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Field.Scripts
{
    public partial class Tile : MonoBehaviour
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        
        public string Name { get; private set; }
        
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        private void SetIcon(Sprite sprite)
        {
            iconSpriteRenderer.sprite = sprite;
        }
    }
}