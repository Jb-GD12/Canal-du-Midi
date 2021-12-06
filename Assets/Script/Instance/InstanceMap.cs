using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMap : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_zoneList;

    public GameObject m_returnMainButton;


    // Start is called before the first frame update
    /*void Awake()
    {
        Debug.Log("letgo");
        for (int i = 0; i < GameManager.Instance.m_zoneList.Count; i++)
        {
            if (GameManager.Instance.m_zoneList[i] == null)
            {
                GameManager.Instance.m_zoneList.Add(Instantiate(m_zoneList[i]));
            }
            GameManager.Instance.m_zoneList[i].SetActive(false);
        }

        GameManager.Instance.m_zoneList[GameManager.Instance.m_currentZone].SetActive(true);

        //Launch interface et setactive false les non utile
        GameManager.Instance.m_uiParent = FindObjectOfType<Canvas>();
        if (GameManager.Instance.m_uiParent == null)
        {
            GameManager.Instance.m_uiParent = Instantiate(new Canvas());
        }

        GameManager.Instance.m_returnMainButton = Instantiate(m_returnMainButton, GameManager.Instance.m_uiParent.transform);
        if (GameManager.Instance.m_currentZone == 0)
        {
            GameManager.Instance.m_returnMainButton.SetActive(false);
       */ }
    }

    
}
