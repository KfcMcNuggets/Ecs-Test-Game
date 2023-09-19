using Leopotam.Ecs;
using UnityEngine;

sealed class CardRotator : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
    private EcsFilter<RotationSpeed> rotationFilter;
    private EcsFilter<ClosedToRemixCard> closedFilter;
    private EcsFilter<Card> cardFilter;
    private EcsFilter<EndGameMarker> endGameFilter;
    private StaticData staticData;
    private Vector3 rotateAngle;

    public void Init()
    {
        rotateAngle = new Vector3(0, 2, 0);
    }

    void IEcsRunSystem.Run()
    {
        if (!rotationFilter.IsEmpty())
        {
            RotateCards();
        }
        if (closedFilter.GetEntitiesCount() == cardFilter.GetEntitiesCount())
        {
            MarkCardsToRemix();
        }
    }

    private void MarkCardsToRemix()
    {
        staticData.positions.Shuffle();
        foreach (int i in closedFilter)
        {
            ref EcsEntity cardEntity = ref closedFilter.GetEntity(i);
            ref Body body = ref cardEntity.Get<Body>();
            cardEntity.Del<ClosedToRemixCard>();
            cardEntity.Get<Movement>();
            cardEntity.Get<RemixingCard>();
        }
    }

    private void RotateCards()
    {
        foreach (int i in rotationFilter)
        {
            ref EcsEntity card = ref rotationFilter.GetEntity(i);
            float speed = rotationFilter.Get1(i).Speed;
            int cardId = card.Get<Card>().CardId;
            ref Body body = ref card.Get<Body>();
            Transform objTransform = body.objTransform;

            objTransform.eulerAngles = Vector3.MoveTowards(
                objTransform.eulerAngles,
                objTransform.eulerAngles + rotateAngle,
                speed
            );

            if (
                objTransform.eulerAngles.y >= body.FinalYAngle - 5
                && objTransform.eulerAngles.y <= body.FinalYAngle + 5
            )
            {
                Debug.Log("finished");
                objTransform.eulerAngles = new Vector3(0, body.FinalYAngle, 0);
                card.Del<RotationSpeed>();
                if (card.Has<ClosingCard>())
                {
                    card.Del<ClosingCard>();
                    card.Get<ClosedToRemixCard>();
                    body.FinalYAngle = 0;
                }
                else if (card.Has<OpeningCard>())
                {
                    body.FinalYAngle = 180;
                    card.Get<OpenedCard>();
                    card.Del<OpeningCard>();
                }
            }
        }
    }
}
