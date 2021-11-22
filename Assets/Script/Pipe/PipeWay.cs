using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeWay : MonoBehaviour
{
    [SerializeField] private int m_idTuile; //De zéro à x
    
    private PipeRotation m_idRotation;
    private int m_idTampon;

    public bool[,] m_testArray = new bool[4,4];
    
    [SerializeField] [Tooltip("Liste de/des ID de rotation qui permettent de faire le bon chemin")] private List<int> m_correctRotationList;

    [SerializeField] private bool m_isAlign;
    
    // Start is called before the first frame update
    void Start()
    {
        m_idRotation = GetComponent<PipeRotation>();
        m_idTampon = -1;
        GameManager.Instance.m_correctAlignList.Add(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (m_idTampon != m_idRotation.m_currentRotation)
        {
            AlignVerif();
            m_idTampon = m_idRotation.m_currentRotation;
        }
    }

    //Vérifie si la tuile est correctement agancé.
    private void AlignVerif()
    {
        Debug.Log("vérification");
        for (int i = 0; i < m_correctRotationList.Count; i++)
        {
            if (m_idRotation.m_currentRotation == m_correctRotationList[i])
            {
                GameManager.Instance.m_correctAlignList[m_idTuile] = true;
                m_isAlign = true;
            }
            else
            {
                GameManager.Instance.m_correctAlignList[m_idTuile] = false;
                m_isAlign = false;
            }
        }
    }
}
