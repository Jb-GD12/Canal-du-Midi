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
        GameManager.Instance.StartLevel();

        GameManager.Instance.m_looseScreen = Instantiate(m_looseScreen, GameManager.Instance.m_uiParent.transform);
        GameManager.Instance.m_looseScreen.SetActive(false);

        GameManager.Instance.m_winScreen = Instantiate(m_winScreen, GameManager.Instance.m_uiParent.transform);
        GameManager.Instance.m_winScreen.SetActive(false);

        /*m_pauseScreen = Instantiate(m_pauseScreen, m_uiParent.transform.parent);
        m_pauseScreen.SetActive(false);*/

        GameManager.Instance.m_countDown = Instantiate(m_countDown, GameManager.Instance.m_uiParent.transform);
        GameManager.Instance.m_countDown.SetActive(true);
    }

    
}
