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

    #region Selection

    public List<GameObject> m_zoneList;
    
    [HideInInspector] public List<ContainerLevelSO> m_LevelList;
    public int m_levelAccess = 1;
    public LayerMask m_levelLayer;
    public LayerMask m_zoneLayer;
    /*[HideInInspector]*/ public int m_currentZone;

    #endregion

    #region levelVariables

    [Header("Variables de niveaux")] [Tooltip("Liste regroupant l'état -alignement correct- des tuiles qui font le chemin souhaité")] public List<bool> m_correctAlignList;
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
        m_currentZone = 0;
        m_levelStart = false;
        PlayerPrefs.SetInt("UnlockLvl", m_levelAccess);
        var test = PlayerPrefs.GetInt("UnlockLvl");
        Debug.Log(test);

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

        for (int i = 1; i < m_zoneList.Count; i++)
        {
            m_zoneList[i].SetActive(false);
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
                //Fonctionnalité du touch outLevel 
                if (!m_levelStart)
                {
                    LayerMask layerMask = hit.collider.gameObject.layer;

                    //Sélection de niveau et chargement du niveau dans une nouvelle scène
                    if (layerMask == (layerMask | (1 << m_levelLayer.value)))
                    {
                        ContainerLevelSO selectedLevel = hit.collider.GetComponent<ContainerLevelSO>();
                        
                        if(selectedLevel.m_SO.indexLevel <= PlayerPrefs.GetInt("UnlockLvl"))
                        {
                            m_seconde = selectedLevel.m_SO.seconde;
                            m_minute = selectedLevel.m_SO.minute;

                            SceneManager.LoadScene(selectedLevel.m_SO.indexLevel, LoadSceneMode.Single);
                            StartCoroutine(WaitForStart());
                        }
                    }else
                    {
                        ContainerZoneSO selectedZone = hit.collider.GetComponent<ContainerZoneSO>();
                        int test = PlayerPrefs.GetInt("UnlockLvl");
                        Debug.Log(test);

                        if (selectedZone.m_SO.zoneID <= PlayerPrefs.GetInt("UnlockLvl"))
                        {
                            
                            ZoomSelection(selectedZone);
                            
                        }else
                            Debug.Log("no");
                        
                    }
                }
                //fonctinonalité du touche inLevel >> La tuile tourne de 90° quand le joueur la touche
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
        Debug.Log("vérification");
        int size = m_correctAlignList.Count;
        int valide = 0;

        for (int i = 0; i < size; i++)
        {
            if (m_correctAlignList[i])
            {
                valide += 1;
                Debug.Log(valide);
            }
            else
            {
                break;
            }
        }
        
        if (valide == size)
            StartCoroutine(Victory());
    }
    
    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(0.3f);
        StartLevel();
    }

    /// <summary>
    /// Fonction de victoire
    /// </summary>
    /// <returns></returns>
    IEnumerator Victory()
    {
        yield return new WaitForSeconds(0.3f);
        Debug.Log("victory");
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

    public void ZoomSelection(ContainerZoneSO p_selectedZone)
    {
        //Zoom sur le niveau sélectionné
        Debug.Log("yes");
        Debug.Log(p_selectedZone);

        m_zoneList[m_currentZone].SetActive(false);
        m_currentZone = p_selectedZone.m_SO.zoneID;
        m_zoneList[m_currentZone].SetActive(true);


    }

    public void DezoomSelection()
    {
        //Retour au menu principal via la selection de niveau
        m_zoneList[m_currentZone].SetActive(false);
        m_currentZone = 0;
        m_zoneList[m_currentZone].SetActive(true);
    }

    public void LevelToMenu()
    {
        m_levelStart = false;
    }

    public void ReturnMainMap()
    {
        m_currentZone = 0;
    }
}