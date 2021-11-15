using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private int m_indexTuile;
    
    [HideInInspector] public bool m_isTouch;
    private bool m_isRotate;
    private bool m_indexIsChanged;
    
    [SerializeField] [Tooltip("Liste des 4 direction dans lesquels peut être tourné la tuile")] private List<Vector3> m_targetRotationList; // 0 = Gauche; 1 = Haut; 2 = Droit; 4 = Bas
    [SerializeField] [Tooltip("Liste de/des index de la liste de rotation qui est/sont correct.s")] private List<int> m_alignIndexList;
    [SerializeField] [Tooltip("Multiplicateur de temps de rotation")] private float m_lerpTime;

    [SerializeField] private int m_currentIndex;

    private Vector3 m_minAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        m_isTouch = false;
        m_indexIsChanged = false;
        m_currentIndex = 0;

        //initialisation de la liste ui vérifiera à chaque fois si les pièces sont aligné
        for (int i = 0; i < m_alignIndexList.Count; i++)
        {
            if (m_currentIndex == m_alignIndexList[i])
            {
                GameManager.Instance.m_correctAlignList.Insert(m_indexTuile, true); 
            }
            else
            {
                GameManager.Instance.m_correctAlignList.Insert(m_indexTuile, false); 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (m_isTouch)
        {
            m_isRotate = true;
            if (!m_indexIsChanged)
            {
                m_currentIndex += 1;
                if (m_currentIndex == 4)
                    m_currentIndex = 0;
                m_indexIsChanged = true;
                
                StartCoroutine(CoolDown());
            }
            
            m_isTouch = false;
            m_minAngle = m_targetRotationList[m_currentIndex] - new Vector3(0, 2, 0);
        }

        //Rotation de la pièce à 90°
        if (m_isRotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(m_targetRotationList[m_currentIndex]),Time.deltaTime * m_lerpTime);
        }

        //vérification de l'alignement de la tuile
        if (m_isRotate && (transform.rotation.eulerAngles.y > m_minAngle.y - 2f))
        {
            AlignVerif();
        }
        
        //Empêche les tuiles de tourner. Cela évite une verification constante peu utile.
        if (m_isRotate && (transform.rotation.eulerAngles == m_minAngle))
        {
            m_isRotate = false;
        } 
        
    }

    /// <summary>
    /// Fonction de vérification de l'alignement de la tuile + information envoyé dans le singleton
    /// </summary>
    private void AlignVerif()
    {
        for (int i = 0; i < m_alignIndexList.Count; i++)
        {
            if (m_currentIndex == m_alignIndexList[i])
            {
                GameManager.Instance.m_correctAlignList.Insert(m_indexTuile, true); 
            }
            else
            {
                GameManager.Instance.m_correctAlignList.Insert(m_indexTuile, false); 
            }
        }
    }

    /// <summary>
    /// Fonction de temps d'attente avant de pouvoir rappuyer sur une tuile afin d'éviter des bugs.
    /// </summary>
    /// <returns></returns>
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        
        m_indexIsChanged = false;
    }
}
