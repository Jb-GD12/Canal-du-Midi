using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private string objectName = "GameManager";
    [SerializeField] private bool dontDestroyOnLoad = true;

    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<T>();
                if (m_instance == null)
                {
                    CreateSingleton();
                }
            }
            Debug.Log("lts go");
            (m_instance as Singleton<T>)?.Initialize();
            return m_instance;

        }
    }

    static void CreateSingleton()
    {
        //création d'un objet
        GameObject singletonObject = new GameObject();

        //Ajout du composant Singleton
        m_instance = singletonObject.AddComponent<T>();

    }

    protected void Initialize()
    {
        Debug.Log("initialisation");

        //Optionnel nommage, config... etc
        gameObject.name = objectName;

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
