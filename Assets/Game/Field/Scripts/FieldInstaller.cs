using UnityEngine;
using Zenject;

namespace Game.Field.Scripts
{
    public class FieldInstaller : MonoInstaller
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Transform tilesContainer;
    
        public override void InstallBindings()
        {
            Container.BindFactory<Tile, Tile.Factory>().FromComponentInNewPrefab(tilePrefab).AsSingle();
            
            Container.BindInterfacesAndSelfTo<Transform>().FromComponentInHierarchy(tilesContainer).AsSingle();
        }
    }
}