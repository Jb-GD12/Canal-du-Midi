using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tuile : MonoBehaviour
{
    [HideInInspector] public bool m_isTouch;
    [SerializeField] [Tooltip("Bool qui vérifie si c'est une tuile 'chemin'")] private bool m_isTheWay;
    [SerializeField] private bool m_isRotating;

    [Header("Listes")]
    [SerializeField] [Tooltip("List de FBX 'Tuile'")] private List<GameObject> m_tuileList;
    [SerializeField] [Tooltip("Liste des 4 direction dans lesquels peut être tourné la tuile")] private List<Vector3> m_rotationList; 
    [SerializeField] [Tooltip("Liste de/des ID de rotation qui permettent de faire le bon chemin si il s'agit d'un chemin correct")] private List<int> m_correctRotationList;

    [Header("")]
    [SerializeField] [Tooltip("ID de la liste 'Tuile List'")] private int m_idFbx;
    [SerializeField] [Tooltip("ID de la tuile pour le tableau de vérification")] private int m_idTuile;
    [SerializeField] [Tooltip("Rotation courante")] [Range(0,3)] private int m_currentRotation;

    
    // Start is called before the first frame update
    void Start()
    {
        
        Instantiate(m_tuileList[m_idFbx], this.transform);
        m_isTouch = false;
        m_isRotating = false;
        transform.rotation = Quaternion.Euler(m_rotationList[m_currentRotation]);

        //Vérification de l'alignement avant de rentrer en jeu
        if (m_isTheWay)
        {
            GameManager.Instance.m_correctAlignList.Add(false);
            StartCoroutine(LateStart());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //En prévision de l'ajout d'animations de rotation --> m_isRotating = false à la fin de l'animation
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

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        Verification();
    }

    /// <summary>
    /// Fonction qui gère la Rotation de la tuile et lance la vérification
    /// </summary>
    private void TuileRotation()
    {
        transform.rotation = Quaternion.Euler(m_rotationList[m_currentRotation]);
        
        if(m_isTheWay)
            Verification();
    }

    /// <summary>
    /// Fonction qui vérifie si l'alignement de la tuile est correct
    /// </summary>
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