using Zenject;

namespace Game.Field.Scripts
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Tile>().FromComponentsInHierarchy().AsTransient();
            
            Container.BindInterfacesAndSelfTo<InputController>().FromComponentsInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<FieldController>().FromComponentsInHierarchy().AsSingle();
        }
    }
}