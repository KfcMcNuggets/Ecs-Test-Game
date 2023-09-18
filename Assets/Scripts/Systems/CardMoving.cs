using UnityEngine;
using Leopotam.Ecs;
using static StaticData;

sealed class CardMoving : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
    private EcsFilter<MovementSpeed> filter;
    private EcsFilter<FinishingMovingMarker> marker;
    private StaticData staticData;

    public void Init() { }

    void IEcsRunSystem.Run()
    {
        if (staticData.CurrentState == CardsState.Remixing && filter.IsEmpty())
        {
            foreach (int i in marker)
            {
                ref EcsEntity card = ref marker.GetEntity(i);
                card.Del<FinishingMovingMarker>();
            }
            if (staticData.RemixCounts == staticData.MaxCounts)
            {
                staticData.MaxCounts++;
                staticData.RemixCounts = 0;
                staticData.CurrentState = StaticData.CardsState.ClosedToChoose;
            }
            else
            {
                staticData.RemixCounts++;
                staticData.CurrentState = StaticData.CardsState.ClosedToRemix;
            }
        }

        foreach (int i in filter)
        {
            ref EcsEntity card = ref filter.GetEntity(i);
            float speed = filter.Get1(i).Speed;
            int cardId = card.Get<Card>().CardId;
            RectTransform rt = card.Get<Body>().ObjRt;

            rt.anchoredPosition = Vector2.MoveTowards(
                rt.anchoredPosition,
                staticData.positions[cardId],
                Time.fixedDeltaTime * speed
            );

            if (rt.anchoredPosition == staticData.positions[cardId])
            {
                card.Del<MovementSpeed>();
                card.Get<FinishingMovingMarker>();
            }
        }
    }
}
