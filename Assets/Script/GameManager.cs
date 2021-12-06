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
    [HideInInspector] public int m_currentZone;
    [HideInInspector] public ContainerZoneSO m_selectedZone;

    #endregion

    #region levelVariables

   [Header("Variables de niveaux")] [Tooltip("Liste regroupant l'état -alignement correct- des tuiles qui font le chemin souhaité")] public List<bool> m_correctAlignList;
    public int m_seconde;
    public int m_minute;

    public bool m_gameOver;
    public bool m_win;

    [HideInInspector] public LvlInfo_SO m_lvlSO;

    #endregion

    #region Interface

    [Header("GameObject d'interface du main manu")]
    public GameObject m_returnMainButton;

    [Header("GameObject d'interface des niveaux")]
    public GameObject m_looseScreen;
    public GameObject m_winScreen;
    public GameObject m_pauseScreen;
    public GameObject m_countDown;

    [HideInInspector] public Canvas m_uiParent;

    [HideInInspector] public bool m_isPause;

    #endregion

    public void Awake()
    {
        
        //Penser à supprimer cette ligne pour la version finale
        PlayerPrefs.SetInt("UnlockLvl", m_levelAccess);
        //DontDestroyOnLoad(this);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
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
                    }else
                    {
                        m_selectedZone = hit.collider.GetComponent<ContainerZoneSO>();

                        if (m_selectedZone.m_SO.zoneID <= PlayerPrefs.GetInt("UnlockLvl"))
                        {
                            Debug.Log("m_currentZone");
                            ZoomSelection(m_selectedZone);
                            
                        }
                        
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

    public void LaunchMain() 
    {
        Debug.Log("start Launch " + m_zoneList.Count);
        m_levelStart = false;
        SceneManager.LoadScene(1, LoadSceneMode.Single);

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

        if (valide == size)
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
        Debug.Log("victory");
        m_win = true;
        m_winScreen.SetActive(true);
        if (m_lvlSO.indexLevel == PlayerPrefs.GetInt("UnlockLvl"))
            PlayerPrefs.SetInt("UnlockLvl", m_lvlSO.indexLevel + 1);
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

    /// <summary>
    /// Affichage de la zone sélectionné
    /// </summary>
    /// <param name="p_selectedZone"></param>
    public void ZoomSelection(ContainerZoneSO p_selectedZone)
    {
        Debug.Log("ZoomSelection");

        //Zoom sur la zone sélectionné
        Debug.Log(p_selectedZone);

        m_currentZone = p_selectedZone.m_SO.zoneID;
        m_zoneList[0].SetActive(false);
        m_zoneList[m_currentZone].SetActive(true);
        m_returnMainButton.SetActive(true);
        Debug.Log(m_currentZone);
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


    }
}