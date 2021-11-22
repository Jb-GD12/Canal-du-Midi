using UnityEngine;

public class ContainerLevelSO : MonoBehaviour
{
    public LvlInfo_SO m_SO;

    private void Awake()
    {
        GameManager.Instance.m_LevelList.Add(this);
    }
}
