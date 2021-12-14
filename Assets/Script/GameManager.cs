using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : Singleton<GameManager>
{

    #region Variable d'état

    /*[HideInInspector]*/ public bool m_levelStart;
    [HideInInspector] public bool m_isDontDestroy = false;

    #endregion

    #region Selection

    public List<GameObject> m_zoneList;
    
    [HideInInspector] public List<ContainerLevelSO> m_LevelList;
    public int m_levelAccess = 1;
    public LayerMask m_levelLayer;
    public LayerMask m_zoneLayer;
    /*[HideInInspector]*/ public int m_currentZone;
    [HideInInspector] public ContainerZoneSO m_selectedZone;

    public InstanceMap m_instanceMap;

    #endregion

    #region levelVariables

   [Header("Variables de niveaux")] 
   [Tooltip("Liste regroupant l'état -alignement correct- des tuiles qui font le chemin souhaité")] public List<bool> m_correctAlignList;
    public int m_seconde;
    public int m_minute;

    public bool m_gameOver;
    public bool m_win;

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

    [HideInInspector] public Canvas m_uiParent;

    [HideInInspector] public bool m_isPause;

    #endregion

    public void Awake()
    {
        if (!m_isDontDestroy)
        {
            DontDestroyOnLoad(this);
            m_isDontDestroy = true;
        }

        //Penser à supprimer cette ligne pour la version finale
        PlayerPrefs.SetInt("UnlockLvl", m_levelAccess);
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
                            StartCoroutine(WaitForStart());
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
        for (int i = 0; i < GameManager.Instance.m_zoneList.Count; i++)
        {
            m_zoneList[i] = Instantiate(GameManager.Instance.m_zoneList[i]);
            m_zoneList[i].SetActive(false);
        }
        
        m_zoneList[m_currentZone].SetActive(true);

        //Launch interface et setactive false les non utile
        GameManager.Instance.m_uiParent = FindObjectOfType<Canvas>();
        if (GameManager.Instance.m_uiParent == null)
        {
            GameManager.Instance.m_uiParent = Instantiate(new Canvas());
        }

        GameManager.Instance.m_returnMainButtonUi = Instantiate(m_instanceMap.m_returnMainButton, GameManager.Instance.m_uiParent.transform);
        if (m_currentZone == 0)
        {
            GameManager.Instance.m_returnMainButtonUi.SetActive(false);
        }

        GameManager.Instance.m_nomLieuxUi =  Instantiate(m_instanceMap.m_nomLieux, GameManager.Instance.m_uiParent.transform);
        if (m_currentZone == 0)
        {
            GameManager.Instance.m_nomLieuxUi.SetActive(false);
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
        m_gameOver = false;
        m_win = false;
        m_levelStart = true;

        m_uiParent = FindObjectOfType<Canvas>();
        if (m_uiParent == null)
        {
            m_uiParent = Instantiate(new Canvas());
        }

        GameManager.Instance.m_looseScreenUi = Instantiate(m_looseScreenUi, GameManager.Instance.m_uiParent.transform);
        GameManager.Instance.m_looseScreenUi.SetActive(false);

        GameManager.Instance.m_winScreenUi = Instantiate(m_winScreenUi, GameManager.Instance.m_uiParent.transform);
        GameManager.Instance.m_winScreenUi.SetActive(false);

        //Instancier le bouton pause
        
        GameManager.Instance.m_pauseScreenUi = Instantiate(GameManager.Instance.m_pauseScreenUi, m_uiParent.transform.parent);
        GameManager.Instance.m_pauseScreenUi.SetActive(false);

        GameManager.Instance.m_countDownUi = Instantiate(m_countDownUi, GameManager.Instance.m_uiParent.transform);
        GameManager.Instance.m_countDownUi.SetActive(true);

        
        Verification();
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
            }
            else
            {
                break;
            }
        }

        if (valide == size && !m_win)
        {
            Debug.Log("victory");
            StartCoroutine(Victory());
        }
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
        m_win = true;
        m_winScreenUi.SetActive(true);
        m_countDownUi.SetActive(false);
        
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
        m_gameOver = true;
        Debug.Log("GameOver");
        m_looseScreenUi.SetActive(true);
        m_countDownUi.SetActive(false);
    }

    public void Pause()
    {
        //GameManager.Instance.m_pauseButtonUi.SetActive(false);
        GameManager.Instance.m_pauseScreenUi.SetActive(true);
    }

    public void QuitPause()
    {
        //GameManager.Instance.m_pauseButtonUi.SetActive(false);
        GameManager.Instance.m_pauseScreenUi.SetActive(false);
    }
    
    public void Restart()
    {
        
        Debug.Log("Restart");
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

        m_currentZone = p_selectedZone.m_SO.zoneID;

        GameManager.Instance.m_nomLieuxUi.GetComponentInChildren<TMP_Text>().text = p_selectedZone.m_SO.nomDeZone;

        GameManager.Instance.m_zoneList[0].SetActive(false);
        m_zoneList[m_currentZone].SetActive(true);
        GameManager.Instance.m_returnMainButtonUi.SetActive(true);
        GameManager.Instance.m_nomLieuxUi.SetActive(true);
    }

    /// <summary>
    /// Retour à la main map à partir d'une zone
    /// </summary>
    public void DezoomSelection()
    {
        //Me demandez pas comment ça marche, ça marche.
        GameManager.Instance.m_zoneList[GameManager.Instance.m_currentZone].SetActive(false);
        GameManager.Instance.m_currentZone = 0;
        GameManager.Instance.m_zoneList[GameManager.Instance.m_currentZone].SetActive(true);
        GameManager.Instance.m_returnMainButtonUi.SetActive(false);
        GameManager.Instance.m_nomLieuxUi.SetActive(false);
    }
}