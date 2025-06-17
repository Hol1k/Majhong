using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Field.Scripts
{
    public class FieldController : MonoBehaviour
    {
        private Tile.Factory _tileFactory;
        private Transform _tilesContainer;
        
        private List<Tile> _tiles =  new List<Tile>();
        [SerializeField] private int fieldSize = 21;

        [SerializeField] private int tilesPairsCount = 144;

        [Space]
        [SerializeField] private AssetReferenceSprite[] tileSpritesAssets;
        private float _loadedSpritesCount;
        
        private readonly Dictionary<string, Sprite> _tileIcons = new Dictionary<string, Sprite>();

        [Inject]
        public void Construct(Tile.Factory tileFactory, Transform tilesContainer)
        {
            _tileFactory = tileFactory;
            _tilesContainer = tilesContainer;
        }

        private void Awake()
        {
            Addressables.InitializeAsync().Completed += obj => LoadTiles();
        }

        private void Start()
        {
            StartCoroutine(GenerateTilesAfterLoadSprites());
        }

        private void RegenerateTiles()
        {
            foreach (var tile in _tiles)
            {
                Destroy(tile.gameObject);
            }
            _tiles.Clear();

            for (int pair = 0; pair < tilesPairsCount; pair++)
            {
                string tileName = _tileIcons.Keys.ElementAt(Random.Range(0, _tileIcons.Count));
                for (int i = 0; i < 2; i++)
                {
                    int xCord;
                    int yCord;
                    int zCord;
                    
                    do
                    {
                        xCord = Random.Range(1, fieldSize - 1);
                        yCord = Random.Range(1, fieldSize - 1);
                    } while (!ValidateTileCords(xCord, yCord, out zCord));

                    var tile = _tileFactory.Create(xCord, yCord, zCord,fieldSize,
                        tileName, _tileIcons[tileName],
                        _tilesContainer);
                    
                    _tiles.Add(tile);
                }
            }

            foreach (var tile in _tiles)
            {
                tile.CheckIsAvailable(_tiles);
            }
        }

        private bool ValidateTileCords(int x, int y, out int zCord)
        {
            int z = 0;

            bool isStageValid = true;
            while (isStageValid)
            {
                var currStageTiles = _tiles.Where(t => t.Z == z).ToList();
                
                bool isPositionValid = !currStageTiles.Any(t =>
                    (t.X >= x - 1 && t.X <= x + 1) && (t.Y >= y - 1 && t.Y <= y + 1));

                if (isPositionValid)
                {
                    zCord = z;
                    return true;
                }

                isStageValid = false;

                // 1 tile on a center
                if (currStageTiles.Any(t => t.X == x && t.Y == y))
                    isStageValid = true;

                // 2 tiles on edges
                if (currStageTiles.Any(t => t.X == x - 1 && t.Y == y) && currStageTiles.Any(t => t.X == x + 1 && t.Y == y))
                    isStageValid = true;
                if (currStageTiles.Any(t => t.X == x && t.Y == y - 1) && currStageTiles.Any(t => t.X == x && t.Y == y + 1))
                    isStageValid = true;

                // 3 tiles (1 on the edge, 2 on corners)
                if (currStageTiles.Any(t => t.X == x - 1 && t.Y == y - 1) &&
                    currStageTiles.Any(t => t.X == x - 1 && t.Y == y + 1) &&
                    currStageTiles.Any(t => t.X == x + 1 && t.Y == y))
                    isStageValid = true;
                if (currStageTiles.Any(t => t.X == x - 1 && t.Y == y - 1) &&
                    currStageTiles.Any(t => t.X == x + 1 && t.Y == y - 1) &&
                    currStageTiles.Any(t => t.X == x && t.Y == y + 1))
                    isStageValid = true;
                if (currStageTiles.Any(t => t.X == x + 1 && t.Y == y - 1) &&
                    currStageTiles.Any(t => t.X == x + 1 && t.Y == y + 1) &&
                    currStageTiles.Any(t => t.X == x - 1 && t.Y == y))
                    isStageValid = true;
                if (currStageTiles.Any(t => t.X == x - 1 && t.Y == y + 1) &&
                    currStageTiles.Any(t => t.X == x + 1 && t.Y == y + 1) &&
                    currStageTiles.Any(t => t.X == x && t.Y == y - 1))
                    isStageValid = true;
                
                // 4 tiles on corners
                if (currStageTiles.Any(t => t.X == x - 1 && t.Y == y - 1) &&
                    currStageTiles.Any(t => t.X == x + 1 && t.Y == y - 1) &&
                    currStageTiles.Any(t => t.X == x - 1 && t.Y == y + 1) &&
                    currStageTiles.Any(t => t.X == x + 1 && t.Y == y + 1))
                    isStageValid = true;

                z++;
            }

            zCord = -1;
            return false;
        }

        private void LoadTiles(Action onLoaded = null)
        {
            foreach (var tileSpriteAsset in tileSpritesAssets)
            {
                tileSpriteAsset.LoadAssetAsync().Completed += handle =>
                {
                    var sprite = handle.Result;
                    _tileIcons.Add(sprite.name, sprite);
                    _loadedSpritesCount++;
                };
            }
        }

        private IEnumerator GenerateTilesAfterLoadSprites()
        {
            while (_loadedSpritesCount < 100)
                yield return null;
            
            RegenerateTiles();
        }
    }
}
