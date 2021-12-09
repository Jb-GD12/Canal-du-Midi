using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tuile : MonoBehaviour
{
    [HideInInspector] public bool m_isTouch;
    [SerializeField] private bool m_isTheWay;
    [SerializeField] private bool m_isRotating;

    [Header("Listes")]
    [SerializeField] private List<GameObject> m_tuileList;
    [SerializeField] [Tooltip("Liste des 4 direction dans lesquels peut être tourné la tuile")] private List<Vector3> m_rotationList; 
    [SerializeField] [Tooltip("Liste de/des ID de rotation qui permettent de faire le bon chemin")] private List<int> m_correctRotationList;

    [Header("")]
    [SerializeField] private int m_idFbx;
    [SerializeField] private int m_idTuile;
    [SerializeField] private int m_currentRotation;

    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.m_correctAlignList.Add(false);
        Instantiate(m_tuileList[m_idFbx], this.transform);
        m_isTouch = false;
        m_isRotating = false;
        transform.rotation = Quaternion.Euler(m_rotationList[m_currentRotation]);
        if(m_isTheWay)
            Verification();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isTouch && !m_isRotating)
        {
            Debug.Log("tourne !");
            m_isRotating = true;
            m_currentRotation++;
            if (m_currentRotation == 4)
                m_currentRotation = 0;
            m_isTouch = false;
        }

        if (m_isRotating)
        {
            TuileRotation();
        }
    }

    private void TuileRotation()
    {
        transform.rotation = Quaternion.Euler(m_rotationList[m_currentRotation]);
        
        if(m_isTheWay)
            Verification();
    }

    private void Verification()
    {
        for (int i = 0; i < m_correctRotationList.Count; i++)
        {
            if (m_currentRotation == m_correctRotationList[i])
            {
                GameManager.Instance.m_correctAlignList[m_idTuile] = true;
                break;
            }
            else
            {
                GameManager.Instance.m_correctAlignList[m_idTuile] = false;
            }
        }
        GameManager.Instance.Verification();
        m_isRotating = false;
    }
}
