using DG.Tweening;
using UnityEngine;

public class LevelEnding : MonoBehaviour
{
    [SerializeField] CanvasGroup winScreen;
    [SerializeField] CanvasGroup loseScreen;

    protected LevelController levelController;

    protected virtual void Start()
    {
        levelController = FindObjectOfType<LevelController>();
    }

    protected virtual void Update()
    {

    }

    public virtual void Win()
    {
        Time.timeScale = 0f;
        winScreen.gameObject.SetActive(true);
        winScreen.DOFade(1f, 0.3f).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public virtual void Lose()
    {
        Time.timeScale = 0f;
        loseScreen.gameObject.SetActive(true);
        loseScreen.DOFade(1f, 0.3f).SetEase(Ease.OutQuint).SetUpdate(true);
    }
}
