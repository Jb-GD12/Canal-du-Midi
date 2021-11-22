using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelInformationsSO")]
public class LvlInfo_SO : ScriptableObject
{

    public string nomDuNiveau;
    public SceneAsset levelScene;
    public int indexLevel;
    public int seconde;
    public int minute;

}
