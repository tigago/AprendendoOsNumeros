using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    [SerializeField] private TMPro.TMP_InputField _inputField;
    [SerializeField] private Button _ouvirButton, _mesesButton, _semanaButton, _playButton;
    [SerializeField] private Button[] _allButtons;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _inputField.onSubmit.AddListener(OuvirClicked);
        _ouvirButton.onClick.AddListener(()=>OuvirClicked(_inputField.text));
        _mesesButton.onClick.AddListener(()=>Reader.Instance.ReadAllMonths());
        _semanaButton.onClick.AddListener(() => Reader.Instance.ReadAllDaysOfTheWeek());
        _playButton.onClick.AddListener(() => GameManager.Instance.BeginGame());
        Reader.Instance.OnReadingFinished.AddListener(OnReadFinished);
        Reader.Instance.OnReadingBegin.AddListener(OnReadBegin);
    }

    private void OuvirClicked(string txt)
    {
        if (txt == string.Empty) return;
        if (!_ouvirButton.interactable) return;
        int toRead = int.Parse(txt);
        Reader.Instance.ReadNumber(toRead);
    }

    private void OnReadBegin()
    {
        SetButtonsInteractable(false);
    }
    private void OnReadFinished()
    {
        SetButtonsInteractable(true);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        foreach(Button b in _allButtons)
        {
            b.interactable = interactable;
        }
    }
}
