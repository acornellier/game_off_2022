using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] AudioManager.Settings _audioSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(_audioSettings);

        Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PersistentDataManager>().AsSingle();

        Container.BindInterfacesAndSelfTo<DialogueManager>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<FadeOverlay>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<GlobalSounds>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneLoader>().FromComponentInHierarchy().AsSingle();
    }
}