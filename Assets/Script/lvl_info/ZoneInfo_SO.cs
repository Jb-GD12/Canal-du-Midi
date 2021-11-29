using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ZoneInformationsSO")]

public class ZoneInfo_SO : ScriptableObject
{
    public string nomDeZone;
    public int zoneID;

}
