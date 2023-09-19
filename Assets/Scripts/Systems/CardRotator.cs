using Leopotam.Ecs;
using UnityEngine;

sealed class CardRotator : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
    private EcsFilter<RotationSpeed> rotationFilter;
    private EcsFilter<ClosedToRemixCard> closedFilter;
    private EcsFilter<Card> cardFilter;
    private StaticData staticData;
    private Vector3 rotateAngle;

    public void Init()
    {
        rotateAngle = new Vector3(0, 1, 0);
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
        foreach (int i in closedFilter)
        {
            staticData.ShufflePositions();
            ref EcsEntity cardEntity = ref closedFilter.GetEntity(i);
            cardEntity.Del<ClosedToRemixCard>();
            cardEntity.Get<MovementSpeed>();
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
            RectTransform rt = body.ObjRt;

            rt.eulerAngles = Vector3.MoveTowards(
                rt.eulerAngles,
                rt.eulerAngles + rotateAngle,
                Time.fixedDeltaTime * speed
            );

            if (
                rt.eulerAngles.y >= body.FinalYAngle - 5 && rt.eulerAngles.y <= body.FinalYAngle + 5
            )
            {
                Debug.Log("finished");
                rt.eulerAngles = new Vector3(0, body.FinalYAngle, 0);
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
