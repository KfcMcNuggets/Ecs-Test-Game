using Leopotam.Ecs;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

sealed class MouseClick : IEcsRunSystem, IEcsInitSystem
{
    public static event UnityAction GameEnd;
    private EcsWorld world;
    private EcsFilter<CorrectCard> correctCardFilter;
    private EcsFilter<ClosedToChooseCard> closedCardFilter;
    private EcsFilter<Card> cardFilter;
    private EcsFilter<OpenedCard> openedCardFilter;
    private EcsFilter<Body> bodyFilter;
    private EcsFilter<CurrentScores> scoreUpdaterFilter;
    private EcsFilter<Header> headerFilter;
    private Ray ray;
    private RaycastHit hit;
    private StaticData staticData;

    private EcsEntity gameManager;

    public void Init()
    {
        gameManager = world.NewEntity();
        gameManager.Get<Header>();
    }

    void IEcsRunSystem.Run()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameManager.Has<EndGameMarker>())
            {
                GameEnd?.Invoke();
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
                {
                    CheckClick(hit.collider.gameObject);
                }
            }
        }
    }

    private void CheckClick(GameObject cardObj)
    {
        headerFilter.GetEntity(0).Get<UpdateHeaderMarker>();
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
            if (PlayerPrefs.GetInt("Sound", 1) == 1)
            {
                AudioSource.PlayClipAtPoint(staticData.rotateSound, Vector3.zero, 1);
            }
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
        foreach (int i in closedCardFilter)
        {
            ref EcsEntity card = ref bodyFilter.GetEntity(i);
            ref Body body = ref bodyFilter.Get1(i);
            if (hit.collider.gameObject == body.objTransform.gameObject)
            {
                if (card.Has<ClosedToChooseCard>())
                {
                    if (PlayerPrefs.GetInt("Sound", 1) == 1)
                    {
                        AudioSource.PlayClipAtPoint(staticData.rotateSound, Vector3.zero, 1);
                    }
                    if (card.Has<CorrectCard>())
                    {
                        scoreUpdaterFilter.GetEntity(0).Get<UpdateScoresMarker>();
                        if (PlayerPrefs.GetInt("Sound", 1) == 1)
                        {
                            AudioSource.PlayClipAtPoint(staticData.winSound, Vector3.zero, 1);
                        }
                    }
                    else
                    {
                        gameManager.Get<EndGameMarker>();
                    }
                }

                card.Del<ClosedToChooseCard>();
                card.Get<OpeningCard>();
                card.Get<RotationSpeed>();
            }
        }
    }
}
