using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerChallengeObj : MonoBehaviour
{
    [SerializeField] private GameObject _towerButtonPrefab, _towerPiecePrefab;
    [SerializeField] private int _numLen = 6;
    [SerializeField] private int _minStartRange = 0;
    [SerializeField] private int _maxStartRange = 5;
    [SerializeField] private Mesh _baseMesh, _midMesh, _topMesh;
    [SerializeField] private Transform _buttonsParent;
    private int _lastNumber;
    private int _firstNumber;
    private float _towerHeight = 1f;
    public float TowerHeight { get { return _towerHeight; } }
    public Quaternion _towerRot = Quaternion.identity;
    public Quaternion TowerRot { get { return _towerRot; } }

    private Tween _camTweenPosition, _camTweenRotation;
    private void Start()
    {
        _camTweenPosition = GameManager.Instance.CameraHPivot.DOMove(Vector3.up * 3f, 0.3f);
        _camTweenRotation = GameManager.Instance.CameraHPivot.DORotate(Vector3.zero, 0.3f);
        int firstNumber = Random.Range(_minStartRange, _maxStartRange + 1);
        TowerBlock firstBlock = Instantiate(_towerPiecePrefab, transform).GetComponent<TowerBlock>();
        firstBlock.transform.position = Vector3.zero;
        firstBlock.transform.rotation = Quaternion.identity;
        firstBlock.SetText(firstNumber.ToString());
        firstBlock.SetMesh(_baseMesh);
        Reader.Instance.ReadNumber(firstNumber);
        StartCoroutine(SpawnButtonsCo(firstNumber));
        _lastNumber = firstNumber;
        _firstNumber = firstNumber;
    }

    private IEnumerator SpawnButtonsCo(int first)
    {
        List<int> spawnList = new List<int>();
        for (int i = first+1; i <=first + _numLen + 1; i++)
        {
            spawnList.Add(i);
        }
        spawnList.Shuffle();
        foreach(int n in spawnList)
        {
            TowerButton newButton = Instantiate(_towerButtonPrefab, _buttonsParent).GetComponent<TowerButton>();
            newButton.Setup(n.ToString(), this);
            yield return new WaitForSeconds(0.1f);
        }
        GameCanvas.Instance.SetTimer(10f);
    }
    public void TowerButtonClicked(string s, TowerButton button)
    {
        TowerBlock newBlock = Instantiate(_towerPiecePrefab, transform).GetComponent<TowerBlock>();
        newBlock.transform.position = GameManager.Instance.CameraHPivot.position + Vector3.up * 5f;
        newBlock.transform.rotation = Quaternion.identity;
        if (_firstNumber + _numLen + 1 == int.Parse(s)) newBlock.SetMesh(_topMesh);
        newBlock.SetText(s);
        newBlock.Fall(button, this);
    }

    public bool IsCorrectBlock(TowerBlock block)
    {

        int n = int.Parse(block.GetText());
        if (n == _lastNumber + 1)
        {
            return true;
        }
        return false;
    }

    public void UpdateLastBlock(TowerBlock block)
    {
        int n = int.Parse(block.GetText());
        Reader.Instance.ReadNumber(n);
        _lastNumber = n;
        _towerHeight = block.transform.position.y + 1f;
        _towerRot = block.transform.rotation;
        if (_camTweenPosition != null) _camTweenPosition.Kill();
        if (_camTweenRotation != null) _camTweenRotation.Kill();
        _camTweenPosition = GameManager.Instance.CameraHPivot.DOMoveY(block.transform.position.y + 3f, 0.2f);
        _camTweenRotation = GameManager.Instance.CameraHPivot.DORotate(block.transform.eulerAngles, 0.2f);
        if (_firstNumber + _numLen + 1 == n) StartCoroutine(WinGame());
    }

    private IEnumerator WinGame()
    {
        yield return new WaitForSeconds(0.5f);
        _camTweenPosition = GameManager.Instance.CameraHPivot.DOMoveY(TowerHeight + 15f, 2f);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.AddPoint();
    }
}
