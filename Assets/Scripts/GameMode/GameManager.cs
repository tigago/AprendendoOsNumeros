using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Transform _cameraHPivot, _cameraVPivot, _cameraBeginPoint;
    [SerializeField] private Challenge[] _challenges;
    [SerializeField] private int _maxChallengesPerLevel = 5;
    public Transform CameraHPivot { get { return _cameraHPivot; } }
    public Transform CameraVPivot { get { return _cameraVPivot; } }

    private int _currentLevel = 0;
    public int CurrentLevel { get { return _currentLevel; } }

    private int _points;
    public int Points { get { return _points; } }

    private Challenge _currentChallenge = null;
    private GameObject _currentChallengeObj;
    private int _currentChallengeIndex = -1;
    public Challenge CurrentChallenge { get { return _currentChallenge; } }


    private List<Challenge> _currentLevelChallenges = new List<Challenge>();
    private void Awake()
    {
        Instance = this;
    }

    public void BeginGame()
    {
        MainMenu.Instance.gameObject.SetActive(false);
        GameCanvas.Instance.gameObject.SetActive(true);
        _cameraHPivot.position = _cameraBeginPoint.position;
        BeginLevel(0);
    }

    private void BeginLevel(int levelToBegin)
    {
        if (_currentChallengeObj != null) Destroy(_currentChallengeObj);
        _currentLevelChallenges.Clear();
        foreach(Challenge c in _challenges)
        {
            if (c.StartFromGameLevel <= levelToBegin) _currentLevelChallenges.Add(c);
        }
        _currentChallengeIndex = -1;
        _currentLevelChallenges.Shuffle();
        _currentLevel = levelToBegin;
        StartCoroutine(BeginLevelCo());
    }

    private IEnumerator BeginLevelCo()
    {
        GameCanvas.Instance.NewLevel(_currentLevel + 1);
        yield return new WaitForSeconds(1.4f);
        BeginNextChallenge();
    }

    private void BeginNextChallenge()
    {
        _currentChallengeIndex++;
        if (_currentChallengeIndex >= _maxChallengesPerLevel || _currentChallengeIndex >= _currentLevelChallenges.Count)
        {
            _currentLevel++;
            BeginLevel(_currentLevel);
            return;
        }
        StartCoroutine(BeginChallengeCo(_currentLevelChallenges[_currentChallengeIndex]));
    }

    private IEnumerator BeginChallengeCo(Challenge c)
    {
        if (_currentChallengeObj != null) Destroy(_currentChallengeObj);
        _cameraHPivot.position = _cameraBeginPoint.position;
        _currentChallenge = c;
        GameCanvas.Instance.NewChallenge();
        yield return new WaitForSeconds(2f);
        _currentChallengeObj = Instantiate(_currentChallenge.ChallengePrefab, Vector3.zero, Quaternion.identity);

    }

    public void AddPoint()
    {
        _points++;
        GameCanvas.Instance.SetPointsText(_points);
        BeginNextChallenge();
    }

}
