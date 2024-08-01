using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBlock : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text[] _numberTexts;
    [SerializeField] private MeshFilter _meshFilter;
    private const float _fallSpeed = 10f;
    private const float _rotSpeed = 10f;
    private bool _falling = false;
    private TowerButton _button;
    private TowerChallengeObj _Obj;
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

    public void SetMesh(Mesh m)
    {
        _meshFilter.mesh = m;
    }
    public void Fall(TowerButton button, TowerChallengeObj obj)
    {
        _button = button;
        _Obj = obj;
        _falling = true;
    }
    private void Update()
    {
        if (!_falling) return;
        transform.position -= Vector3.up * _fallSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, _Obj.TowerRot * Quaternion.Euler(Vector3.up * 60f), _rotSpeed * Time.deltaTime);
        if (transform.position.y <= _Obj.TowerHeight)
        {
            //caiu
            _falling = false;
            if (_Obj.IsCorrectBlock(this))
            {
                _button.PieceLanded(true);
                transform.position = Vector3.up * _Obj.TowerHeight;
                transform.rotation = _Obj.TowerRot * Quaternion.Euler(Vector3.up * 60f);
                _Obj.UpdateLastBlock(this);
            }
            else
            {
                //To do: crumble
                _button.PieceLanded(false);
                Destroy(gameObject);
            }
        }
    }


}
