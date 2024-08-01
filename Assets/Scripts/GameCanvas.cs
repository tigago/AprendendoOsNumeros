using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas Instance;

    [SerializeField] private TMPro.TMP_Text _pointsText, _levelText, _challengeText, _newLevelText;
    [SerializeField] private Image _timerImg;
    [SerializeField] private RectTransform _titleParent;


    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    public void NewChallenge()
    {
        StartCoroutine(ChallengeAnimationCo());
        _levelText.text = "Level " + (GameManager.Instance.CurrentLevel + 1).ToString();
        _challengeText.text = GameManager.Instance.CurrentChallenge.ChallengeName;
        //int timeCount = GameManager.Instance.CurrentChallenge.TimePerLevel.Length;
        //float timeToChoose = GameManager.Instance.CurrentLevel >= timeCount ? GameManager.Instance.CurrentChallenge.TimePerLevel[timeCount - 1] : GameManager.Instance.CurrentChallenge.TimePerLevel[GameManager.Instance.CurrentLevel];
        //if (timeToChoose > 0)
        //{
            _timerImg.DOFillAmount(1f, 0.3f);
            _timerImg.gameObject.SetActive(true);
        //}
        //else
        //{
        //    _timerImg.gameObject.SetActive(false);
        //}
    }

    public void SetPointsText(int p, bool skipAnim = false)
    {
        _pointsText.text = p.ToString("00");
        if(!skipAnim) _pointsText.transform.DOPunchScale(Vector3.one * 1.2f, 0.3f);
    }

    private IEnumerator ChallengeAnimationCo()
    {
        _titleParent.gameObject.SetActive(true);
        _titleParent.anchoredPosition = new Vector2(-2000f, 0f);
        _titleParent.DOAnchorPosX(0, 0.5f);
        yield return new WaitForSeconds(1f);
        _titleParent.DOAnchorPosX(2000, 0.5f);
        _titleParent.gameObject.SetActive(true);

    }

    public void NewLevel(int level)
    {
        _newLevelText.text = "Level " + level.ToString() + "!";
        StartCoroutine(NewLevelAnimCo());
    }

    private IEnumerator NewLevelAnimCo()
    {
        _newLevelText.transform.localScale = Vector3.one * 0.1f;
        _newLevelText.gameObject.SetActive(true);
        _newLevelText.transform.DOScale(Vector3.one * 1f, 0.4f);
        yield return new WaitForSeconds(1f);
        _newLevelText.transform.DOScale(Vector3.one * 0.04f, 0.4f);
        yield return new WaitForSeconds(0.4f);
        _newLevelText.gameObject.SetActive(false);

    }

    public void SetTimer(float time)
    {
        _timerImg.DOFillAmount(0f, time).SetEase(Ease.Linear);
    }
}
