using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObj : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] [Tooltip("Force d'impulsion de la tuile")] private float m_impulseForce;
    [SerializeField] [Tooltip("Vitesse de rotation de la tuile")] private float m_rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                RotateObj rotateScript = hitInfo.collider.GetComponent<RotateObj>();
                if (rotateScript != null)
                {
                    rotateScript.m_isTouch = true;
                }
            }
        }
    }
}
