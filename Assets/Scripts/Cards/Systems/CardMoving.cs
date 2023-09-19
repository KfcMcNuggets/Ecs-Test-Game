using UnityEngine;
using Leopotam.Ecs;
using static StaticData;

sealed class CardMoving : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
    private EcsFilter<Movement> movementFilter;
    private EcsFilter<RemixingCard> remixingFilter;
    private EcsFilter<FinishedMoving> finishedFilter;
    private StaticData staticData;

    public void Init() { }

    void IEcsRunSystem.Run()
    {
        if (!movementFilter.IsEmpty())
        {
            MoveCard();
        }
        else if (
            !finishedFilter.IsEmpty()
            && finishedFilter.GetEntitiesCount() == remixingFilter.GetEntitiesCount()
        )
        {
            CheckRemix();
        }
    }

    private void CheckRemix()
    {
        staticData.positions.Shuffle();
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
            }
            else
            {
                cardEntity.Del<FinishedMoving>();
                cardEntity.Get<Movement>();
            }
        }
    }

    private void MoveCard()
    {
        foreach (int i in movementFilter)
        {
            ref EcsEntity card = ref movementFilter.GetEntity(i);
            Movement movementComponent = movementFilter.Get1(i);
            int cardId = card.Get<Card>().CardId;
            Transform objTransform = card.Get<Body>().objTransform;
            objTransform.position = Vector3.MoveTowards(
                objTransform.position,
                GetPositionWithHeght(staticData.positions[cardId], objTransform.position),
                Time.fixedDeltaTime * movementComponent.Speed
            );

            if (objTransform.position == staticData.positions[cardId])
            {
                card.Del<Movement>();
                card.Get<FinishedMoving>();
                card.Get<Card>().CurrentMixes++;
            }
        }
    }

    private Vector3 GetPositionWithHeght(Vector3 destPosition, Vector3 currentPos)
    {
        destPosition.z = Mathf.Abs(currentPos.x - destPosition.x) / -5;
        destPosition.y = (currentPos.x - destPosition.x) / -5;
        return destPosition;
    }
}
