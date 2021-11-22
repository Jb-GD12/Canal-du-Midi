using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    #region Variable d'état

    [HideInInspector] public bool m_levelStart;

    #endregion

    #region LevelSelection

    [HideInInspector] public ContainerLevelSO m_selectedLevel;
     public List<ContainerLevelSO> m_LevelList;
    [HideInInspector] public int m_levelAccess = 0;
    public LayerMask m_checkLayer;


    #endregion

    #region levelVariables

    [Header("Variables de niveaux")]
    [Tooltip("Liste regroupant l'état -alignement correct- des tuiles qui font le chemin souhaité")] public List<bool> m_correctAlignList;
    public int m_seconde;
    public int m_minute;

    public bool m_gameOver;
    public bool m_win;



    #endregion

    #region Interface

    [Header("GameObject d'interface")]
    public GameObject m_looseScreen;
    public GameObject m_winScreen;
    public GameObject m_pauseScreen;
    public GameObject m_countDown;

    [HideInInspector] public Canvas m_uiParent;

    [HideInInspector] public bool m_isPause;

    #endregion

    public void Start()
    {
        m_levelStart = false;
        PlayerPrefs.SetInt("UnlockLvl", m_levelAccess);

        for (int i = 0; i < m_LevelList.Count; i++) {
            for (int j = 0; j < m_LevelList.Count - 1; j++)
            {
                if(m_LevelList[i].m_SO.indexLevel > m_LevelList[j].m_SO.indexLevel)
                {
                    ContainerLevelSO tampon = m_LevelList[i];
                    m_LevelList[i] = m_LevelList[j];
                    m_LevelList[j] = tampon;
                }
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Fonctionnalité des menu
                if (!m_levelStart)
                {
                    Debug.Log("trolololo");
                    LayerMask layerMask = hit.collider.gameObject.layer;
                    m_selectedLevel = hit.collider.gameObject.GetComponent<ContainerLevelSO>();

                    if (layerMask == (layerMask | (1 << m_checkLayer.value)) && m_selectedLevel.m_SO.indexLevel <= PlayerPrefs.GetInt("UnlockLevel"))
                    {
                        m_seconde = m_selectedLevel.m_SO.seconde;
                        m_minute = m_selectedLevel.m_SO.minute;

                        SceneManager.LoadScene(m_selectedLevel.m_SO.levelScene.name, LoadSceneMode.Single);
                        StartCoroutine(WaitForStart());
                    }
                }
                //fonctinonalité in Game
                else
                {
                    PipeRotation pipe = hit.collider.GetComponent<PipeRotation>();

                    if (!m_gameOver && !m_win && pipe != null && !pipe.m_isTouch)
                    {
                        pipe.m_isTouch = true;
                    }
                }

            }

            
        }
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(0.3f);
        StartLevel();
    }

    /// <summary>
    /// Fonction d'initialisation des interfaces des niveaux
    /// </summary>
    public void StartLevel()
    {
        m_uiParent = FindObjectOfType<Canvas>();
        if(m_uiParent == null)
        {
            m_uiParent = Instantiate(new Canvas());
        }
        
        m_looseScreen = Instantiate(m_looseScreen, m_uiParent.transform);
        m_looseScreen.SetActive(false);

        /*m_winScreen = Instantiate(m_winScreen, m_uiParent.transform.parent);
        m_winScreen.SetActive(false);

        m_pauseScreen = Instantiate(m_pauseScreen, m_uiParent.transform.parent);
        m_pauseScreen.SetActive(false);*/

        m_countDown = Instantiate(m_countDown, m_uiParent.transform);
        m_countDown.SetActive(true);

        m_gameOver = false;
        m_win = false;
        m_levelStart = true;
    }

    /// <summary>
    /// Fonction de vérification de l'alignement des tuiles
    /// </summary>
    public void Verification()
    {
        int size = m_correctAlignList.Count;
        int valide = 0;

        for (int i = 0; i < m_correctAlignList.Count - 1; i++)
        {
            if (m_correctAlignList[i] == true && m_correctAlignList[i + 1] == true)
            {
                valide += 1;
                if (valide == size)
                    StartCoroutine(Victory());
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// Fonction de victoire
    /// </summary>
    /// <returns></returns>
    IEnumerator Victory()
    {
        yield return new WaitForSeconds(0.3f);
        m_win = true;
        
    }

    /// <summary>
    /// Fonction de GameOver
    /// </summary>
    public void GameOver()
    {
        m_gameOver = true;
        Debug.Log("GameOver");
        m_looseScreen.SetActive(true);
        m_countDown.SetActive(false);
    }

    public void Restart()
    {
        Debug.Log("Restart");
    }

    public void ZoomSelection()
    {
        //Zoom sur le niveau sélectionné
    }

    public void DezoomSelection()
    {
        //Retour au menu principal via la selection de niveau
    }

    public void ReturnMenu()
    {
        m_levelStart = false;
    }
}
