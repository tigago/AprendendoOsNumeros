using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBlock : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text[] _numberTexts;

    public void SetText(string s)
    {
        foreach (TMPro.TMP_Text t in _numberTexts)
        {
            t.text = s;
        }
    }

    public string GetText()
    {
        return _numberTexts[0].text;
    }
}
