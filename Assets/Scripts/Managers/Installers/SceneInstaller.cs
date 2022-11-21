using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PersistentDataManager>().AsSingle();

        Container.BindInterfacesAndSelfTo<DialogueManager>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<FadeOverlay>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<GlobalLight>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<GlobalSounds>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneLoader>().FromComponentInHierarchy().AsSingle();
    }
}