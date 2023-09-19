using UnityEngine;
using Leopotam.Ecs;
using static StaticData;

sealed class CardMoving : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
    private EcsFilter<MovementSpeed> movementFilter;
    private EcsFilter<RemixingCard> remixingFilter;
    private EcsFilter<FinishedMoving> finishedFilter;
    private StaticData staticData;

    public void Init() { }

    void IEcsRunSystem.Run()
    {
        if (!movementFilter.IsEmpty())
        {
            Debug.Log($"we have {movementFilter.GetEntitiesCount()} MovementSpeed, moving card ");
            MoveCard();
        }
        else if (finishedFilter.GetEntitiesCount() == remixingFilter.GetEntitiesCount())
        {
            Debug.Log("CheckingRemix");
            CheckRemix();
        }
    }

    private void CheckRemix()
    {
        foreach (int i in finishedFilter)
        {
            ref EcsEntity cardEntity = ref finishedFilter.GetEntity(i);
            ref Card cardComp = ref cardEntity.Get<Card>();
            if (cardComp.CurrentMixes == cardComp.MaxRemixes)
            {
                cardComp.MaxRemixes++;
                cardComp.CurrentMixes = 0;
                cardEntity.Del<RemixingCard>();
                cardEntity.Del<FinishedMoving>();
                cardEntity.Get<ClosedToChooseCard>();
                Debug.Log($"EndRemix {cardComp.CurrentMixes}/{cardComp.MaxRemixes}");
            }
            else
            {
                Debug.Log($"EndRemix {cardComp.CurrentMixes}/{cardComp.MaxRemixes}");
                staticData.ShufflePositions();
                cardEntity.Del<FinishedMoving>();
                cardEntity.Get<MovementSpeed>();
            }
        }
    }

    private void MoveCard()
    {
        foreach (int i in movementFilter)
        {
            ref EcsEntity card = ref movementFilter.GetEntity(i);
            float speed = movementFilter.Get1(i).Speed;
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
                card.Get<FinishedMoving>();
                card.Get<Card>().CurrentMixes++;
            }
        }
    }
}
