using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    #region Variable d'état

    /*[HideInInspector]*/ public bool m_levelStart;
    [HideInInspector] public bool m_isDontDestroy = false;

    #endregion

    #region Selection

    public List<GameObject> m_zoneList;
    
    [HideInInspector] public List<ContainerLevelSO> m_LevelList;
    [HideInInspector] public int m_levelAccess = 1;
    public LayerMask m_levelLayer;
    public LayerMask m_zoneLayer;
    [HideInInspector] public int m_currentZone;
    [HideInInspector] public ContainerZoneSO m_selectedZone;

    public string m_nomLieux;

    public InstanceMap m_instanceMap;
    public InstanceLvl m_instanceLvl;

    #endregion

    #region levelVariables

    [Header("Variables de temps")] 
    [Tooltip("Liste regroupant l'état -alignement correct- des tuiles qui font le chemin souhaité")] public List<bool> m_correctAlignList;
    public int m_seconde;
    public int m_minute;
    
    [HideInInspector] public bool m_gameOver;
    [HideInInspector] public bool m_win;

    [HideInInspector] public LvlInfo_SO m_lvlSO;

    #endregion

    #region Interface

    [Header("GameObject d'interface du main manu")]
    public GameObject m_returnMainButtonUi;
    public GameObject m_nomLieuxUi;

    [Header("GameObject d'interface des niveaux")]
    public GameObject m_looseScreenUi;
    public GameObject m_winScreenUi;
    public GameObject m_pauseButtonUi;
    public GameObject m_pauseScreenUi;
    public GameObject m_countDownUi;

    [HideInInspector] public GameObject m_uiParent;

    [HideInInspector] public bool m_isPause;

    #endregion

    public void Awake()
    {
        if (!m_isDontDestroy)
        {
            DontDestroyOnLoad(this);
            m_isDontDestroy = true;
        }
        if(PlayerPrefs.GetInt("UnlockLvl") == 0)
            PlayerPrefs.SetInt("UnlockLvl", 1);

        //Penser à supprimer cette ligne pour la version finale
        m_levelAccess = PlayerPrefs.GetInt("UnlockLvl");
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
                        m_lvlSO = hit.collider.GetComponent<ContainerLevelSO>().m_SO;
                        
                        if(m_lvlSO.indexLevel <= PlayerPrefs.GetInt("UnlockLvl"))
                        {
                            m_seconde = m_lvlSO.seconde;
                            m_minute = m_lvlSO.minute;
                            
                            SceneManager.LoadScene(m_lvlSO.indexLevel + 1, LoadSceneMode.Single);
                        }
                    }
                    else
                    {
                        m_selectedZone = hit.collider.GetComponent<ContainerZoneSO>();
                        if (m_selectedZone.m_SO.zoneID <= PlayerPrefs.GetInt("UnlockLvl"))
                        {
                            ZoomSelection(m_selectedZone);
                        }
                    }
                }

                //fonctinonalité du touche inLevel >> La tuile tourne de 90° quand le joueur la touche
                else
                {
                    
                    Tuile pipe = hit.collider.GetComponent<Tuile>();
                    
                        if (!m_gameOver && !m_win && pipe != null && !pipe.m_isTouch)
                        {
                            pipe.m_isTouch = true;
                        }
                        
                }
                
            }      
            
        }
        
    }

    public void LaunchMain() 
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void InstanceMap()
    {
        for (int i = 0; i < Instance.m_zoneList.Count; i++)
        {
            m_zoneList[i] = Instantiate(Instance.m_zoneList[i]);
            m_zoneList[i].SetActive(false);
        }
        
        m_zoneList[m_currentZone].SetActive(true);

        //Launch interface et setactive false les non utile
        m_uiParent = Instantiate(m_instanceMap.m_canvas);
        
        m_returnMainButtonUi = Instantiate(m_instanceMap.m_returnMainButton, Instance.m_uiParent.transform);
        if (m_currentZone == 0)
        {
            m_returnMainButtonUi.SetActive(false);
        }

        m_nomLieuxUi =  Instantiate(m_instanceMap.m_nomLieux, Instance.m_uiParent.transform);
        if (m_currentZone == 0)
        {
            m_nomLieuxUi.SetActive(false);
        }
        else
        {
            m_nomLieuxUi.GetComponentInChildren<TMP_Text>().text = m_nomLieux;
        }
        
        m_correctAlignList.Clear();
        m_LevelList.Clear();

        m_levelStart = false;
        m_win = false;
        m_gameOver = false;
    }

    /// <summary>
    /// Fonction d'initialisation des interfaces des niveaux
    /// </summary>
    public void StartLevel()
    {
        Time.timeScale = 1f;
        
        m_gameOver = false;
        m_win = false;
        m_levelStart = true;

        
        Instance.m_uiParent = Instantiate(m_instanceLvl.m_canvas);
        
        m_looseScreenUi = Instantiate(m_looseScreenUi, m_uiParent.transform);
        m_looseScreenUi.SetActive(false);

        m_winScreenUi = Instantiate(m_winScreenUi, m_uiParent.transform);
        m_winScreenUi.SetActive(false);

        //Instancier le bouton pause
        m_pauseButtonUi = Instantiate(m_pauseButtonUi, m_uiParent.transform);
        
        m_pauseScreenUi = Instantiate(m_pauseScreenUi, m_uiParent.transform);
        m_pauseScreenUi.SetActive(false);

        m_countDownUi = Instantiate(m_countDownUi, m_uiParent.transform);
        m_countDownUi.SetActive(true);

        
        Verification();
    }

    /// <summary>
    /// Fonction de vérification de l'alignement des tuiles
    /// </summary>
    public void Verification()
    {
        int size = m_correctAlignList.Count;
        int valide = 0;

        for (int i = 0; i < size; i++)
        {
            if (m_correctAlignList[i])
            {
                valide += 1;
            }
            else
            {
                break;
            }
        }

        if (valide == size && !m_win)
        {
            StartCoroutine(Victory());
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
        m_winScreenUi.SetActive(true);
        m_countDownUi.SetActive(false);
        m_pauseButtonUi.SetActive(false);
        
        if (m_lvlSO.indexLevel == m_levelAccess)
            m_levelAccess += 1;

        m_countDownUi.SetActive(false);
        PlayerPrefs.SetInt("UnlockLvl", m_levelAccess);
    }

    /// <summary>
    /// Fonction de GameOver
    /// </summary>
    public void GameOver()
    {
        m_pauseButtonUi.SetActive(false);
        m_gameOver = true;
        Debug.Log("GameOver");
        m_looseScreenUi.SetActive(true);
        m_countDownUi.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        Instance.m_pauseButtonUi.SetActive(false);
        Instance.m_pauseScreenUi.SetActive(true);
    }

    public void QuitPause()
    {
        Time.timeScale = 1f;
        Instance.m_pauseButtonUi.SetActive(true);
        Instance.m_pauseScreenUi.SetActive(false);
    }
    
    public void Restart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Affichage de la zone sélectionné
    /// </summary>
    /// <param name="p_selectedZone"></param>
    public void ZoomSelection(ContainerZoneSO p_selectedZone)
    {
        Debug.Log("ZoomSelection");

        //Zoom sur la zone sélectionné
        Debug.Log(p_selectedZone.m_SO.zoneID);

        Instance.m_currentZone = p_selectedZone.m_SO.zoneID;

        Instance.m_nomLieux = p_selectedZone.m_SO.nomDeZone;

        Instance.m_nomLieuxUi.GetComponentInChildren<TMP_Text>().text = m_nomLieux;

        Instance.m_zoneList[0].SetActive(false);
        Instance.m_zoneList[Instance.m_currentZone].SetActive(true);
        Instance.m_returnMainButtonUi.SetActive(true);
        Instance.m_nomLieuxUi.SetActive(true);
    }

    /// <summary>
    /// Retour à la main map à partir d'une zone
    /// </summary>
    public void DezoomSelection()
    {
        //Me demandez pas comment ça marche, ça marche.
        Instance.m_zoneList[Instance.m_currentZone].SetActive(false);
        Instance.m_currentZone = 0;
        Instance.m_zoneList[Instance.m_currentZone].SetActive(true);
        Instance.m_returnMainButtonUi.SetActive(false);
        Instance.m_nomLieuxUi.SetActive(false);
    }

    public void ReInit()
    {
        m_levelAccess = 1;
        PlayerPrefs.SetInt("UnlockLvl", m_levelAccess);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Application.Quit();
    }
}