using Leopotam.Ecs;
using UnityEngine;

sealed class SceneControl : IEcsRunSystem, IEcsInitSystem
{
    // auto-injected fields.
    private EcsWorld world = null;
    private EcsFilter<Card> cardFilter;
    private StaticData staticData;

    public void Init() { }

    void IEcsRunSystem.Run()
    {
        foreach (int i in cardFilter)
        {
            Debug.Log(i + "fixing i for test return");
            ref EcsEntity card = ref cardFilter.GetEntity(i);
            switch (staticData.CurrentState)
            {
                case StaticData.CardsState.Opening:
                    break;
                case StaticData.CardsState.Open:
                    break;
                case StaticData.CardsState.Closing:
                    if (
                        card.Has<OpenedMarker>()
                        && !card.Has<MovementSpeed>()
                        && !card.Has<FinishedRotatingMarker>()
                    )
                    {
                        card.Get<RotationSpeed>();
                    }

                    break;
                case StaticData.CardsState.ClosedToRemix:
                    if (!card.Has<MovementSpeed>() && !card.Has<FinishingMovingMarker>())
                    {
                        card.Get<MovementSpeed>();
                    }
                    else
                    {
                        staticData.ShufflePositions();
                        staticData.CurrentState++;
                        Debug.Log("return here");
                        return;
                    }
                    break;
            }
        }
    }
}
