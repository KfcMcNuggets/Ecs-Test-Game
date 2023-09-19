using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Leopotam.Ecs;
using UnityEngine.UI;

public class StaticData : MonoBehaviour
{
    [SerializeField]
    public Vector2[] positions;

    [SerializeField]
    public RectTransform[] cards;

    [SerializeField]
    public EventSystem EventSystem;

    [SerializeField]
    public GraphicRaycaster graphicRaycaster;

    [SerializeField]
    public Sprite shirt;

    [SerializeField]
    public Sprite[] face;

    public int RemixCounts;
    public int MaxCounts;

    public void ShufflePositions()
    {
        positions.Shuffle();
    }
}
