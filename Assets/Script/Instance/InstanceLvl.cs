using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceLvl : MonoBehaviour
{
    [SerializeField] private GameObject m_looseScreen;
    [SerializeField] private GameObject m_winScreen;
    [SerializeField] private GameObject m_pauseButton; 
    [SerializeField] private GameObject m_pauseScreen;
    [SerializeField] private GameObject m_countDown;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.m_looseScreenUi == null)
            GameManager.Instance.m_looseScreenUi = m_looseScreen;

        if (GameManager.Instance.m_pauseScreenUi == null)
            GameManager.Instance.m_pauseScreenUi = m_pauseScreen;
        
        if (GameManager.Instance.m_pauseButtonUi == null)
            GameManager.Instance.m_pauseButtonUi = m_pauseButton;
        
        if (GameManager.Instance.m_winScreenUi == null)
            GameManager.Instance.m_winScreenUi = m_winScreen;
        
        if (GameManager.Instance.m_countDownUi == null)
            GameManager.Instance.m_countDownUi = m_countDown;

        GameManager.Instance.StartLevel();
    }

    
}
