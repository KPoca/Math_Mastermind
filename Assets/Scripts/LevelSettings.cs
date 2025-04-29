using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Scriptable Objects/LevelSettings")]
[System.Serializable]
public class LevelSettings : ScriptableObject
{
    public int difficulty;
    public float spawnRate;
    //public Problem problem;
    public float timePerProblem;
    public int numberOfProblems;

    [Header("Spawn-Odds Multipliers")]
    public float pickupMultiplier = 1f;     // < 1 = rarer, > 1 = more frequent
    public float windMultiplier   = 1f;     //  "
}
