using Leopotam.Ecs;
using UnityEngine;

sealed class CardsInit : IEcsInitSystem
{
    private EcsWorld world;
    private StaticData staticData;

    public void Init()
    {
        for (int i = 0; i < staticData.cards.Length; i++)
        {
            EcsEntity cardEntity = world.NewEntity();
            ref var card = ref cardEntity.Get<Card>();
            card.CardId = i;
            ref var cardRT = ref cardEntity.Get<Body>();
            cardRT.ObjRt = staticData.cards[i];
            if (i == 0)
            {
                cardEntity.Get<CorrectCardMarker>();
            }
            cardEntity.Get<OpenedMarker>();
        }
    }
}
