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
            ref var cardBody = ref cardEntity.Get<Body>();
            cardBody.objTransform = staticData.cards[i];

            cardEntity.Get<OpenedCard>();
            if (i == 1)
            {
                cardEntity.Get<CorrectCard>();
            }
            card.entity = cardEntity;
            cardBody.entity = cardEntity;
        }
    }
}
