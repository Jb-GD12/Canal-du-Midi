using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Variables de niveaux")]
    [Tooltip("Liste regroupant l'état -alignement correct- des tuiles qui font le chemin souhaité")] public List<bool> m_correctAlignList;
    public UIManager m_uiManager;

    public bool m_gameOver;
    public bool m_win;


    public void Start()
    {
        m_gameOver = false;
        m_win = false;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                PipeRotation pipe = hit.collider.GetComponent<PipeRotation>();

                if (!m_gameOver && !m_win && pipe != null && !pipe.m_isTouch)
                {
                    pipe.m_isTouch = true;
                }
            }

            StartLevel();
            
        }
    }

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

    IEnumerator Victory()
    {
        yield return new WaitForSeconds(0.3f);
        m_win = true;
        
    }

    public void GameOver()
    {
        m_gameOver = true;
        Debug.Log("GameOver");
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

    public void StartLevel()
    {
        
    }

    public void ReturnMenu()
    {
        //Retour à la zone de sélection
    }
}
