using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    [SerializeField] private TMPro.TMP_InputField _inputField;
    [SerializeField] private Button _ouvirButton;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _inputField.onSubmit.AddListener(OuvirClicked);
        _ouvirButton.onClick.AddListener(()=>OuvirClicked(_inputField.text));
    }

    private void OuvirClicked(string txt)
    {
        if (txt == string.Empty) return;
        if (!_ouvirButton.interactable) return;
        _ouvirButton.interactable = false;
        int toRead = int.Parse(txt);
        NumberReader.Instance.ReadNumber(toRead);
    }

    public void OnReadFinished()
    {
        _ouvirButton.interactable = true;
    }
}
