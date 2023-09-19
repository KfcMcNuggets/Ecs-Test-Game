using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform startBtn,
        settingsBtn,
        soundBtn,
        vibroBtn,
        topChip,
        leftChip,
        rightChip;

    [SerializeField]
    private Sprite soundOnSprite,
        soundOfSprite;

    private void Start() { }

    private Sequence openSceneAnim,
        closeSceneAnim,
        showSettingsAnim,
        changeVibroAnim,
        changeSoundAnim;
}
