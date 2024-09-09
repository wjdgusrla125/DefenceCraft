using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public IntVariable Gold;

    public GameObject Nexus;
    public GameObject NexusPrefab;

    public bool isGameOver = false;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        NexusPrefab = Resources.Load<GameObject>("Building/BlackCastle");
        Nexus = Instantiate(NexusPrefab, Vector3.zero, Quaternion.identity);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Nexus == null)
        {
            isGameOver = true;
        }
    }
}
