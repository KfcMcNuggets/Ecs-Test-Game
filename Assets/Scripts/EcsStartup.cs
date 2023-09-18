using Leopotam.Ecs;
using UnityEngine;

sealed class EcsStartup : MonoBehaviour
{
    private StaticData staticData;

    EcsWorld world;
    EcsSystems InitSystems,
        UpdateSystems,
        FixedUpdateSystems;

    void Start()
    {
        staticData = GetComponent<StaticData>();
        world = new EcsWorld();
        InitSystems = new EcsSystems(world);
        UpdateSystems = new EcsSystems(world);
        FixedUpdateSystems = new EcsSystems(world);
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(InitSystems);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(UpdateSystems);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(FixedUpdateSystems);
#endif
        InitSystems.Add(new CardsInit()).Inject(staticData).Init();

        UpdateSystems.Add(new MouseClick()).Add(new SceneControl()).Inject(staticData).Init();

        FixedUpdateSystems.Add(new CardRotator()).Add(new CardMoving()).Inject(staticData).Init();

        //UpdateSystems
        // .Add (new TestSystem1 ())
        // .Add (new TestSystem2 ())

        // register one-frame components (order is important), for example:
        // .OneFrame<TestComponent1> ()
        // .OneFrame<TestComponent2> ()

        // inject service instances here (order doesn't important), for example:
        // .Inject (new CameraService ())
        // .Inject (new NavMeshSupport ())
        //.Init();
    }

    void Update()
    {
        UpdateSystems?.Run();
    }

    private void FixedUpdate()
    {
        FixedUpdateSystems?.Run();
    }

    void OnDestroy()
    {
        if (UpdateSystems != null)
        {
            UpdateSystems.Destroy();
            UpdateSystems = null;
        }
        if (FixedUpdateSystems != null)
        {
            FixedUpdateSystems.Destroy();
            FixedUpdateSystems = null;
        }

        if (InitSystems != null)
        {
            InitSystems.Destroy();
            InitSystems = null;
        }
        if (world != null)
        {
            world.Destroy();
            world = null;
        }
    }
}
