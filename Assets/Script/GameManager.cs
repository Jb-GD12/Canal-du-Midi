using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Tooltip("Liste regroupant l'état -alignement correct- des tuiles qui font le chemin souhaité")] public List<bool> m_correctAlignList;

    public bool m_gameOver;
    public bool m_win;

    private void Start()
    {
        m_gameOver = false;
        m_win = false;
    }

    void Update()
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

            for (int i = 0; i < m_correctAlignList.Count-1; i++)
            {
                if (m_correctAlignList[i] && m_correctAlignList[i++])
                {
                    StartCoroutine(Vicotory());
                }
            }
        }
    }

    IEnumerator Vicotory()
    {
        yield return new WaitForSeconds(0.3f);
        m_win = true;
        Debug.Log("win !");
    }
}
