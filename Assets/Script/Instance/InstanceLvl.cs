using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceLvl : MonoBehaviour
{
    [SerializeField] private GameObject m_looseScreen;
    [SerializeField] private GameObject m_winScreen;
    [SerializeField] private GameObject m_pauseScreen;
    [SerializeField] private GameObject m_countDown;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.m_looseScreen == null)
            GameManager.Instance.m_looseScreen = m_looseScreen;

        if (GameManager.Instance.m_pauseScreen == null)
            GameManager.Instance.m_pauseScreen = m_pauseScreen;
        
        if (GameManager.Instance.m_winScreen == null)
            GameManager.Instance.m_winScreen = m_winScreen;
        
        if (GameManager.Instance.m_countDown == null)
            GameManager.Instance.m_countDown = m_countDown;

        GameManager.Instance.StartLevel();
    }

    
}
