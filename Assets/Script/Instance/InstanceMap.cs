using System;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMap : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_zoneList;

    public GameObject m_returnMainButton;


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.m_zoneList == null)
        {
            GameManager.Instance.m_zoneList = m_zoneList ;
        }

        for (int i = 0; i < GameManager.Instance.m_zoneList.Count; i++)
        {
            if(GameManager.Instance.m_zoneList[i] == null)
                GameManager.Instance.m_zoneList[i] = m_zoneList[i];

        }
        GameManager.Instance.InstanceMap();


    }


}
