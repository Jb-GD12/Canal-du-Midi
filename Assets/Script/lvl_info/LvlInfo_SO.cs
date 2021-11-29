
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelInformationsSO")]
public class LvlInfo_SO : ScriptableObject
{

    public string nomDuNiveau;
    public int indexLevel;
    public int seconde;
    public int minute;

}
