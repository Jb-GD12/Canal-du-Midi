using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tuile : MonoBehaviour
{
    [HideInInspector] public bool m_isTouch;
    [SerializeField] private bool m_isTheWay;
    
    [SerializeField] [Tooltip("Liste des 4 direction dans lesquels peut être tourné la tuile")] 
    private List<Vector3> m_targetRotationList; // 0 = Gauche; 1 = Haut; 2 = Droit; 4 = Bas
    
    public int m_currentRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        m_isTouch = false;
        transform.rotation = Quaternion.Euler(m_targetRotationList[m_currentRotation]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TuileRotation()
    {
        transform.rotation = Quaternion.Euler(m_targetRotationList[m_currentRotation]);
        GameManager.Instance.Verification();
    }
}
