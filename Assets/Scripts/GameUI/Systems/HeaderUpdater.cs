using System.Collections;
using UnityEngine;
using Leopotam.Ecs;

sealed class HeaderUpdater : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world;
    private StaticData staticData;
    private EcsEntity header;
    private EcsFilter<OpenedCard> openedFilter;
    private EcsFilter<ClosedToChooseCard> closedFilter;
    private EcsFilter<EndGameMarker> endGameFilter;
    private EcsFilter<Header> headerFilter;

    public void Init()
    {
        header = headerFilter.GetEntity(0);
        header.Get<Header>().headerText = staticData.header;
    }

    public void Run()
    {
        if (header.Has<UpdateHeaderMarker>())
        {
            header.Get<Header>().headerText.text = GetHeaderText();
        }
    }

    private string GetHeaderText()
    {
        string headerText = "";
        if (header.Has<EndGameMarker>())
        {
            return "Неверно!";
        }
        if (!openedFilter.IsEmpty())
        {
            return "Продолжить";
        }
        if (!closedFilter.IsEmpty())
        {
            return "Найди туза червей";
        }
        return "Верно";
    }
}
