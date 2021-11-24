using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericNetworkManager : MonoBehaviour
{
    public static GenericNetworkManager Instance;

    //[HideInInspector]public string azureAnchorId="";
    //[HideInInspector] public PhotonView localUser;
    //private bool isConnected;

    private void Awake()
    {
        //TODO:‚±‚±‚Í‰½‚ð‚â‚Á‚Ä‚¢‚é‚ñ‚¾?
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(Instance.gameObject);
                Instance = this;
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ConnectToNetwork();
    }

    private void ConnectToNetwork()
    {
        OnReadyToStartNetwork?.Invoke();
    }

    public static event Action OnReadyToStartNetwork;

    // Update is called once per frame
    void Update()
    {

    }
}
