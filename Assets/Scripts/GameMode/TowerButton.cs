using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _text;
    [SerializeField] private Button _button;
    private TowerChallengeObj _obj;
    public void Setup(string text, TowerChallengeObj obj)
    {
        _text.text = text;
        _obj = obj;
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void Start()
    {
        transform.localScale = Vector3.one * 0.1f;
        transform.DOScale(Vector3.one, 0.2f);
    }
    private void OnButtonClicked()
    {
        _obj.TowerButtonClicked(_text.text, this);
        _button.interactable = false;
    }

    public void PieceLanded(bool correct)
    {
        if (correct) {
            Destroy(gameObject);
            return;
        }
        _button.interactable = true;
    }
}
