using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Challenge", menuName = "Challenge/CreateNewChallenge", order = 1)]
public class Challenge : ScriptableObject
{
    public string ChallengeName;
    public float[] TimePerLevel = new float[] { -1f };
    public int StartFromGameLevel = 0;
    public GameObject ChallengePrefab;
}
