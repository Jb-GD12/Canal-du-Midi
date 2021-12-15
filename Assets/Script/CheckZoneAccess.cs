using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckZoneAccess : MonoBehaviour
{
    [SerializeField] private GameObject m_punaise;
    
    void Start()
    {
        if(GetComponent<ContainerZoneSO>().m_SO.zoneIndex <= GameManager.Instance.m_levelAccess)
        {
            m_punaise.GetComponent<MeshRenderer>().materials[0].color = Color.green;
        }
        else if (GetComponent<ContainerZoneSO>().m_SO.zoneIndex > GameManager.Instance.m_levelAccess)
        {
            m_punaise.GetComponent<MeshRenderer>().materials[0].color = Color.red;
        }
    }
}
