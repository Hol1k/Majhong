using UnityEngine;
using Zenject;

namespace Game.Field.Scripts
{
    public class FieldInstaller : MonoInstaller
    {
        [SerializeField] private GameObject tilePrefab;
    
        public override void InstallBindings()
        {
            Container.BindFactory<Tile, Tile.Factory>().FromComponentInNewPrefab(tilePrefab).AsSingle();
        }
    }
}