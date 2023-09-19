using Leopotam.Ecs;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

sealed class MouseClick : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
    private PointerEventData pointerEventData;
    private EcsFilter<CorrectCard> correctCardFilter;
    private EcsFilter<ClosedToChooseCard> closedCardFilter;
    private EcsFilter<Card> cardFilter;
    private EcsFilter<OpenedCard> openedCardFilter;
    private EcsFilter<Body> bodyFilter;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private StaticData staticData;
    private List<RaycastResult> results;

    private GameObject correctCardGO;

    public void Init()
    {
        correctCardGO = cardFilter.GetEntity(0).Get<Body>().ObjRt.gameObject;
        raycaster = staticData.graphicRaycaster;
        eventSystem = staticData.EventSystem;
        results = new List<RaycastResult>();
    }

    void IEcsRunSystem.Run()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointerEventData = new PointerEventData(eventSystem);

            pointerEventData.position = Input.mousePosition;

            results.Clear();

            raycaster.Raycast(pointerEventData, results);

            if (results.Count > 0 && results[0].gameObject.CompareTag("Card"))
            {
                CheckClick(results[0].gameObject);
            }
            else
            {
                Debug.Log(results.Count);
            }
        }
    }

    private void CheckClick(GameObject cardObj)
    {
        if (!openedCardFilter.IsEmpty())
        {
            CloseCards();
        }
        if (!closedCardFilter.IsEmpty())
        {
            CheckCorrectOpen();
        }
    }

    private void CloseCards()
    {
        foreach (int i in cardFilter)
        {
            ref EcsEntity cardEntity = ref cardFilter.GetEntity(i);
            if (cardEntity.Has<OpenedCard>())
            {
                cardEntity.Del<OpenedCard>();
                cardEntity.Get<ClosingCard>();
                cardEntity.Get<RotationSpeed>();
            }
            else
            {
                cardEntity.Del<ClosedToChooseCard>();
                cardEntity.Get<ClosedToRemixCard>();
            }
        }
    }

    private void CheckCorrectOpen()
    {
        if (results[0].gameObject == correctCardGO)
        {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log($"Incorrect {results[0].gameObject.name} != {correctCardGO.name}");
        }

        foreach (int i in closedCardFilter)
        {
            ref EcsEntity card = ref bodyFilter.GetEntity(i);
            ref Body body = ref bodyFilter.Get1(i);
            if (body.ObjRt.gameObject == results[0].gameObject)
            {
                card.Del<ClosedToChooseCard>();
                card.Get<OpeningCard>();
                card.Get<RotationSpeed>();
            }
        }
    }
}
