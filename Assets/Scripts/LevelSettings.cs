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
}
