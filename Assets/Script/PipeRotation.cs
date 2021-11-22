using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PipeRotation : MonoBehaviour
{
    [HideInInspector] public bool m_isTouch;
    private bool m_isRotate;
    private bool m_indexIsChanged;
    
    [SerializeField] [Tooltip("Liste des 4 direction dans lesquels peut être tourné la tuile")] private List<Vector3> m_targetRotationList; // 0 = Gauche; 1 = Haut; 2 = Droit; 4 = Bas
    [SerializeField] [Tooltip("Multiplicateur de temps de rotation")] private float m_lerpTime;

    
    [FormerlySerializedAs("m_currentIndex")] [HideInInspector] public int m_currentRotation;

    private Vector3 m_minAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        m_isTouch = false;
        m_indexIsChanged = false;
        m_currentRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isTouch)
        {
            m_isRotate = true;
            if (!m_indexIsChanged)
            {
                m_currentRotation += 1;
                if (m_currentRotation == 4)
                    m_currentRotation = 0;
                m_indexIsChanged = true;
                
                StartCoroutine(CoolDown());
            }
            m_isTouch = false;
        }

        //Rotation de la pièce à 90°
        if (m_isRotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(m_targetRotationList[m_currentRotation]),Time.deltaTime * m_lerpTime);
        }

        //vérification de l'alignement de la tuile
        if (m_isRotate &&
            (transform.rotation.eulerAngles.y > m_targetRotationList[m_currentRotation].y - 2f))
        {
            m_isRotate = false;
            transform.rotation = Quaternion.Euler(m_targetRotationList[m_currentRotation]);
            GameManager.Instance.Verification();
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
