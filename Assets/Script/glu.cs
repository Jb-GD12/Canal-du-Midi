using UnityEngine;

public class glu : MonoBehaviour
{
    [SerializeField][Range(0,10)] [Tooltip("Force d'impulsion de la tuile")] private float m_impulseForce;
    [SerializeField] [Tooltip("Vitesse de rotation de la tuile")] private float m_rotationSpeed;

    private Rigidbody m_rb;

    private bool m_isInAir;

    // Use this for initialization
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
                m_rb = hitInfo.collider.GetComponent<Rigidbody>();
                if (m_rb != null)
                {
                    m_rb.AddForce(Vector3.up * m_impulseForce, ForceMode.Impulse);
                    m_isInAir = true;

                    

                    Quaternion targetAngle = Quaternion.LookRotation(new Vector3(0,0,90));
                    //change la rotation du personnage (exemple je tourne à droite, donc je regarde vers la droite)
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * m_rotationSpeed);
                }
            }
        }

        

    }

    void Rotation()
    {
        Vector3 targetDir = Vector3.zero;

        targetDir = Vector3.forward * 0.5f;
        targetDir = Vector3.right * 0f;

        targetDir.Normalize();
        targetDir.y = 0;


    }
}
