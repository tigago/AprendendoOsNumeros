using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Transform _cameraHPivot, _cameraVPivot;

    private void Awake()
    {
        Instance = this;
    }
}
