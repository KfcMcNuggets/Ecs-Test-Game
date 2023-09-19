using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Leopotam.Ecs;
using UnityEngine.UI;
using TMPro;

public class StaticData : MonoBehaviour
{
    [SerializeField]
    public Vector3[] positions;

    [SerializeField]
    public Transform[] cards;

    [SerializeField]
    public EventSystem EventSystem;

    [SerializeField]
    public TextMeshProUGUI bestScore,
        currentScore,
        header;

    public int RemixCounts;
    public int MaxCounts;
}
