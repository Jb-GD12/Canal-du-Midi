using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLvlAccess : MonoBehaviour
{
    [SerializeField] private GameObject m_punaise;
    
    void Start()
    {
        if (GetComponent<ContainerLevelSO>().m_SO.indexLevel <= GameManager.Instance.m_levelAccess)
        {
            m_punaise.GetComponent<MeshRenderer>().materials[0].color = Color.green;
            Debug.Log("yes");
        }
        else
        {
            m_punaise.GetComponent<MeshRenderer>().materials[0].color = Color.red;
        }
    }
}
