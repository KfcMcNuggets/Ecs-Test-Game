using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIAnimations : MonoBehaviour
{
    private const float AnimLength = 0.3f;
    private bool gameEnd;

    [SerializeField]
    private RectTransform header,
        endGameData;

    [SerializeField]
    private Transform cards;

    private string SceneName;

    [SerializeField]
    private float headerShow,
        headerHide,
        endGameDataShow,
        endGameDataHide,
        cardsShow,
        cardsHide;

    private EcsStartup ecsStartup;

    private Sequence startGameAnim,
        endGameAnim,
        endSceneAnim;

    private void Start()
    {
        ecsStartup = GetComponent<EcsStartup>();
        InitAnims();
        startGameAnim.Restart();
    }

    private void InitAnims()
    {
        startGameAnim = DOTween.Sequence();
        endGameAnim = DOTween.Sequence();
        endSceneAnim = DOTween.Sequence();

        startGameAnim.Append(header.DOAnchorPosY(headerShow, AnimLength));
        startGameAnim.Join(cards.DOMoveY(cardsShow, AnimLength));
        startGameAnim.Join(endGameData.DOAnchorPosY(endGameDataHide, AnimLength));
        startGameAnim.AppendCallback(() =>
        {
            ecsStartup.StartGame();
        });

        endGameAnim.Append(header.DOAnchorPosY(headerHide, AnimLength));
        endGameAnim.Join(endGameData.DOAnchorPosY(endGameDataShow, AnimLength));
        endGameAnim.Join(cards.DOMoveY(cardsHide, AnimLength));

        endSceneAnim.Append(header.DOAnchorPosY(headerHide, AnimLength));
        endSceneAnim.Join(endGameData.DOAnchorPosY(endGameDataHide, AnimLength));
        endSceneAnim.Join(cards.DOMoveY(cardsHide, AnimLength));
        endSceneAnim.AppendCallback(() =>
        {
            SceneManager.LoadScene(SceneName);
        });
    }

    private void OnGameEnd()
    {
        if (!gameEnd)
        {
            gameEnd = true;
            endGameAnim.Restart();
        }
    }

    public void GoToMenu()
    {
        SceneName = "Menu";
        endSceneAnim.Restart();
    }

    public void Restart()
    {
        SceneName = "Game";
        endSceneAnim.Restart();
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        MouseClick.GameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        MouseClick.GameEnd -= OnGameEnd;
    }
}
