using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ZoneInformationsSO")]

public class ZoneInfo_SO : ScriptableObject
{
    public string nomDeZone;
    [Tooltip("ID de la zone dans le tableau zone")] public int zoneID;
    [Tooltip("Index par rapport à 'UnlockLvl")] public int zoneIndex;

}
