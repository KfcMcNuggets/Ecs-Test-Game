using Leopotam.Ecs;
using UnityEngine;

sealed class CardRotator : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
    private EcsFilter<RotationSpeed> filter;
    private EcsFilter<FinishedRotatingMarker> marker;
    private StaticData staticData;
    private Vector3 rotateAngle;

    public void Init()
    {
        rotateAngle = new Vector3(0, 1, 0);
    }

    void IEcsRunSystem.Run()
    {
        if (
            (
                staticData.CurrentState == StaticData.CardsState.Opening
                || staticData.CurrentState == StaticData.CardsState.Closing
            ) && filter.IsEmpty()
        )
        {
            staticData.CurrentState = staticData.CurrentState + 1;
            foreach (int i in marker)
            {
                ref EcsEntity card = ref marker.GetEntity(i);
                card.Del<FinishedRotatingMarker>();
            }
        }

        foreach (int i in filter)
        {
            ref EcsEntity card = ref filter.GetEntity(i);
            float speed = filter.Get1(i).Speed;
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
                card.Get<FinishedRotatingMarker>();
                if (card.Has<OpenedMarker>())
                {
                    card.Del<OpenedMarker>();
                    body.FinalYAngle = 0;
                }
                else
                {
                    body.FinalYAngle = 180;
                    card.Get<OpenedMarker>();
                }
            }
        }
    }
}
