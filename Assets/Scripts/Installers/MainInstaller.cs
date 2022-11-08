using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<StageUi>().FromComponentInHierarchy().AsSingle();
    }
}