using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    private Rigidbody m_rb;

    public Vector3 m_distance;
    public Vector3 m_movementFrequency;
    Vector3 m_moveposition;
    Vector3 m_startPosition;

    [HideInInspector] public bool m_isTouch;
    private bool m_isAir;

    // Start is called before the first frame update
    void Start()
    {
        //m_rb = GetComponent<Rigidbody>();
        m_isTouch = false;
        m_startPosition = transform.position;
    }

    private void Update()
    {
        if (m_isTouch)
        {
            m_moveposition.y = m_startPosition.y + 0.9f + Mathf.Sin(Time.time * m_movementFrequency.y) * m_distance.y;

            transform.position = m_moveposition;
        }

        if (!m_isAir && transform.position.y > 0)
            m_isAir = true;


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_isAir)
        {
            m_isTouch = false;
            m_isAir = false;
        }
    }

}
