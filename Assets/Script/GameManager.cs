using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Tooltip("Liste regroupant l'état -alignement correct- des tuiles qui font le chemin souhaité")] public List<bool> m_correctAlignList;

    // Start is called before the first frame update
    void Start()
    {
        m_correctAlignList.Capacity = 25;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Pipe pipe = hit.collider.GetComponent<Pipe>();

                if (pipe != null && !pipe.m_isTouch)
                {
                    pipe.m_isTouch = true;
                }
            }
        }
    }
}
