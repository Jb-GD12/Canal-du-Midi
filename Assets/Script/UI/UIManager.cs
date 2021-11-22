using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("GameObject d'interface")]
    [SerializeField] private GameObject m_looseScreen;
    [SerializeField] private GameObject m_winScreen;
    [SerializeField] private GameObject m_pauseScreen;
    [SerializeField] private GameObject m_countDown;
    private Canvas m_uiParent;

    private bool m_isPause;

    private void Start()
    {
        
    }

    public void StartLevel()
    {
        m_uiParent = Instantiate(m_uiParent);
        m_looseScreen = Instantiate(m_looseScreen, m_uiParent.transform);
        m_looseScreen.SetActive(false);
        m_winScreen = Instantiate(m_winScreen, m_uiParent.transform);
        m_winScreen.SetActive(false);
        m_pauseScreen = Instantiate(m_pauseScreen, m_uiParent.transform);
        m_pauseScreen.SetActive(false);
        m_countDown = Instantiate(m_countDown, m_uiParent.transform);
        m_countDown.SetActive(true);
    }

    public void EnablePause()
    {
        m_pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void DisablePause()
    {
        m_pauseScreen.SetActive(true);
        Time.timeScale = 1;
    }

    public void EnableWin()
    {
        m_winScreen.SetActive(true);
        m_countDown.SetActive(false);
    }

    public void EnableLoose()
    {
        m_looseScreen.SetActive(true);
        m_countDown.SetActive(false);
    }
}
