using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<StageUi>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<GauntletStartUi>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<EasterEggActivator>().FromComponentInHierarchy()
            .AsSingle();
    }
}