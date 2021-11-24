using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby Lobby;

    private int roomNumber = 1;
    private int userIdCount;

    private void Awake()
    {
        if (Lobby == null)
        {
            Lobby = this;
        }
        else
        {
            if (Lobby != this)
            {
                Destroy(Lobby.gameObject);
                Lobby = this;
            }
        }

        DontDestroyOnLoad(gameObject);

        //�I�u�W�F�N�g���C���X�^���X�����ꂽ�Ƃ��Ƀl�b�g���[�N�ɐڑ�����?
        GenericNetworkManager.OnReadyToStartNetwork += StartNetwork;
    }

    /// <summary>
    /// PUN�ɐڑ�����?
    /// </summary>
    private void StartNetwork()
    {
        PhotonNetwork.ConnectUsingSettings();
        Lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
