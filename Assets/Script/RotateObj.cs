using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    private Rigidbody m_rb;


    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    public void Rotation(float p_impForce)
    {
        m_rb.velocity = new Vector3(0f, 0f);
        m_rb.AddForce(Vector3.up * p_impForce, ForceMode.Impulse);
    }

}
