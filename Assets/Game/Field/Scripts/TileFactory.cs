using UnityEngine;
using Zenject;

namespace Game.Field.Scripts
{
    public partial class Tile
    {
        public class Factory : PlaceholderFactory<Tile>
        {
            public Tile Create(int x, int y, int z, int fieldSize, string name, Sprite icon, Transform parent = null)
            {
                var tile = base.Create();
                
                tile.X = x;
                tile.Y = y;
                tile.Z = z;
                tile.Name = name;
                tile.transform.position = new Vector3(x - fieldSize / 2, y - fieldSize / 2, -z) / 2f;
                tile.SetIcon(icon);
                tile.transform.SetParent(parent);
                
                return tile;
            }
        }
    }
}