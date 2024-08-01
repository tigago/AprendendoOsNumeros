using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Reader : MonoBehaviour
{
    public static Reader Instance;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _numeros0a19, _dezenas, _centenas, _e, _semana, _meses;
    [SerializeField] private AudioClip _cento, _mil;
    [SerializeField] private float _minWaitTime = 0.5f;

    public UnityEvent OnReadingFinished, OnReadingBegin;
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        if (_audioSource != null)
        {
            _audioSource.spatialBlend = 0f;
            _audioSource.playOnAwake = false;
        }
    }
#endif

    private void Awake()
    {
        Instance = this;
    }

    public void ReadMonth(int i)
    {
        if (i < 1 || i > _meses.Length) return;
        OnReadingBegin?.Invoke();
        PlayAudioAndWait(_meses[i-1], true, true);
    }

    public void ReadAllMonths()
    {
        OnReadingBegin?.Invoke();
        StartCoroutine(ReadAllMonthsCo());
    }

    private IEnumerator ReadAllMonthsCo()
    {
        for (int i = 0; i < 12; i++)
        {
            yield return StartCoroutine(PlayAudioAndWait(_meses[i]));
        }
        OnReadingFinished?.Invoke();
    }

    public void ReadAllDaysOfTheWeek()
    {
        OnReadingBegin?.Invoke();
        StartCoroutine(ReadAllDaysOfTheWeekCo());
    }

    private IEnumerator ReadAllDaysOfTheWeekCo()
    {
        for (int i = 0; i < 7; i++)
        {
            yield return StartCoroutine(PlayAudioAndWait(_semana[i]));
        }
        OnReadingFinished?.Invoke();
    }
    public void ReadDayOfWeek(int i)
    {
        if (i < 0 || i >= _semana.Length) return;
        OnReadingBegin?.Invoke();
        PlayAudioAndWait(_semana[i], true, true);
    }
    public void ReadNumber(int n)
    {
        if (n > 9999) return;
        OnReadingBegin?.Invoke();
        StartCoroutine(ReadNumberCo(n));
    }

    private IEnumerator ReadNumberCo(int n)
    {
        if (n > 9999)
        {
            OnReadingFinished?.Invoke();
            yield break;
        }
        int restante = 0;
        if (n > 999)
        {
            restante = n % 1000;
            int milharParaLer = n / 1000;
            if (milharParaLer > 1) yield return StartCoroutine(PlayAudioAndWait(_numeros0a19[milharParaLer]));
            yield return StartCoroutine(PlayAudioAndWait(_mil));
            if ((restante < 100 || restante % 100 == 0) && restante != 0) yield return StartCoroutine(PlayAudioAndWait(RandomE(),true));
        }else if (n > 99)
        {
            restante = n % 100;
            if (n == 100) yield return StartCoroutine(PlayAudioAndWait(_centenas[1]));
            else
            {
                int centenaParaLer = n / 100;
                yield return StartCoroutine(PlayAudioAndWait(centenaParaLer == 1 ? _cento : _centenas[centenaParaLer]));
                if (restante != 0) yield return StartCoroutine(PlayAudioAndWait(RandomE(),true));
            }
        }else if(n > 19)
        {
            restante = n % 10;
            int dezenaParaLer = n / 10;
            yield return StartCoroutine(PlayAudioAndWait(_dezenas[dezenaParaLer]));
            if (restante != 0) yield return StartCoroutine(PlayAudioAndWait(RandomE(),true));
        }
        else
        {
            yield return StartCoroutine(PlayAudioAndWait(_numeros0a19[n]));
        }

        if (restante <= 0)
        {
            OnReadingFinished?.Invoke();
            yield break;
        }
        StartCoroutine(ReadNumberCo(restante));
    }

    private IEnumerator PlayAudioAndWait(AudioClip a, bool ignoreMinTime = false, bool callback = false)
    {
        _audioSource.PlayOneShot(a);
        float wait = a.length;
        if (!ignoreMinTime && wait < _minWaitTime) wait = _minWaitTime;
        yield return new WaitForSeconds(wait);
        if (callback) OnReadingFinished?.Invoke();
    }

    private AudioClip RandomE()
    {
        int r = Random.Range(0, _e.Length);
        return _e[r];
    }
}
