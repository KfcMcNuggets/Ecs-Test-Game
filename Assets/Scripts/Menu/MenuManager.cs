using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private const float AnimLength = 0.3f;

    [SerializeField]
    private GameObject okBtn;

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
        soundOffSprite,
        vibroOnSprite,
        vibroOffSprite;

    [SerializeField]
    private Image soundIc,
        vibroIc;

    [SerializeField]
    private Vector2 startBtnShow,
        startBtnHide,
        settingsButtonShow,
        soundBtnShow,
        soundBtnHide,
        vibroBtnShow,
        vibroBtnHide,
        topChipShow,
        leftChipShow,
        rightChipHide,
        rightChipShow;

    private Sequence openSceneAnim,
        closeSceneAnim,
        showSettingsAnim,
        changeVibroAnim,
        changeSoundAnim;
    private bool startGame;

    private void Start()
    {
        okBtn.SetActive(false);
        InitAnims();
        UpdateSoundIcon();
        UpdateVibroIcon();
        openSceneAnim.Restart();
    }

    public void StartGame()
    {
        startGame = true;
        closeSceneAnim.Restart();
    }

    public void ExitGame()
    {
        startGame = false;
        closeSceneAnim.Restart();
    }

    public void GoSettings()
    {
        showSettingsAnim.Restart();
        okBtn.SetActive(true);
    }

    public void BackMain()
    {
        openSceneAnim.Restart();
        okBtn.SetActive(false);
    }

    public void ChangeSound()
    {
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound", 1) == 1 ? 0 : 1);
        PlayerPrefs.Save();
        changeSoundAnim.Restart();
    }

    public void ChangeVibro()
    {
        PlayerPrefs.SetInt("Vibro", PlayerPrefs.GetInt("Vibro", 1) == 1 ? 0 : 1);
        PlayerPrefs.Save();
        changeVibroAnim.Restart();
    }

    private void InitAnims()
    {
        openSceneAnim = DOTween.Sequence();
        openSceneAnim
            .Append(startBtn.DOAnchorPos(startBtnShow, AnimLength))
            .Join(settingsBtn.DOAnchorPos(settingsButtonShow, AnimLength))
            .Join(soundBtn.DOAnchorPos(soundBtnHide, AnimLength))
            .Join(vibroBtn.DOAnchorPos(vibroBtnHide, AnimLength))
            .Join(topChip.DOAnchorPos(topChipShow, AnimLength))
            .Join(leftChip.DOAnchorPos(leftChipShow, AnimLength))
            .Join(rightChip.DOAnchorPos(rightChipShow, AnimLength));

        closeSceneAnim = DOTween.Sequence();
        closeSceneAnim
            .AppendCallback(() =>
            {
                if (startGame)
                {
                    SceneManager.LoadSceneAsync("Game");
                }
                else
                {
                    Application.Quit();
                }
            })
            .Append(startBtn.DOAnchorPos(startBtnHide, AnimLength))
            .Join(settingsBtn.DOAnchorPos(startBtnHide, AnimLength))
            .Join(soundBtn.DOAnchorPos(soundBtnHide, AnimLength))
            .Join(vibroBtn.DOAnchorPos(vibroBtnHide, AnimLength))
            .Join(topChip.DOAnchorPos(startBtnHide, AnimLength))
            .Join(leftChip.DOAnchorPos(startBtnHide, AnimLength))
            .Join(rightChip.DOAnchorPos(rightChipHide, AnimLength));

        showSettingsAnim = DOTween.Sequence();
        showSettingsAnim
            .Append(startBtn.DOAnchorPos(startBtnHide, AnimLength))
            .Join(settingsBtn.DOAnchorPos(startBtnHide, AnimLength))
            .Join(soundBtn.DOAnchorPos(soundBtnShow, AnimLength))
            .Join(vibroBtn.DOAnchorPos(vibroBtnShow, AnimLength))
            .Join(topChip.DOAnchorPos(topChipShow, AnimLength))
            .Join(leftChip.DOAnchorPos(startBtnHide, AnimLength))
            .Join(rightChip.DOAnchorPos(rightChipShow, AnimLength));

        changeVibroAnim = DOTween.Sequence();
        changeVibroAnim
            .Append(vibroBtn.DOAnchorPos(vibroBtnHide, AnimLength))
            .AppendCallback(() =>
            {
                UpdateVibroIcon();
            })
            .Append(vibroBtn.DOAnchorPos(vibroBtnShow, AnimLength));

        changeSoundAnim = DOTween.Sequence();
        changeSoundAnim
            .Append(soundBtn.DOAnchorPos(soundBtnHide, AnimLength))
            .AppendCallback(() =>
            {
                UpdateSoundIcon();
            })
            .Append(soundBtn.DOAnchorPos(soundBtnShow, AnimLength));
    }

    private void UpdateVibroIcon()
    {
        vibroIc.sprite = PlayerPrefs.GetInt("Vibro", 1) == 1 ? vibroOnSprite : vibroOffSprite;
    }

    private void UpdateSoundIcon()
    {
        soundIc.sprite = PlayerPrefs.GetInt("Sound", 1) == 1 ? soundOnSprite : soundOffSprite;
    }
}
