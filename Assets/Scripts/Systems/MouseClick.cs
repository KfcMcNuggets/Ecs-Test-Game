using Leopotam.Ecs;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

sealed class MouseClick : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
    private PointerEventData pointerEventData;
    private EcsFilter<CorrectCardMarker> cardFilter;
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
            Debug.Log("CLICK");
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

    private void CheckClick(GameObject card)
    {
        switch (staticData.CurrentState)
        {
            case StaticData.CardsState.Open:
                staticData.CurrentState = StaticData.CardsState.Closing;
                break;

            case StaticData.CardsState.ClosedToChoose:
                CheckCorrectOpen();
                break;
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

        foreach (int i in bodyFilter)
        {
            ref EcsEntity card = ref bodyFilter.GetEntity(i);
            ref Body body = ref bodyFilter.Get1(i);
            if (body.ObjRt.gameObject == results[0].gameObject)
            {
                card.Get<RotationSpeed>();
            }
        }
        staticData.CurrentState = StaticData.CardsState.Opening;
    }
}
