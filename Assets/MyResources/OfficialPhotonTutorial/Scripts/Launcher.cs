using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Com.Harusoft.PhotonTutorial
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("1Room������̍ő�v���C���[���BRoom�����Ȃ̏ꍇ�A�V�����v���C���[�͎Q���ł����A�V����Room���쐬����܂�")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion

        #region Private Fields
        /// <summary>
        /// �N���C�A���g�̃o�[�W�����ԍ��ł��B
        /// ���[�U�[��gameVersion�ɂ���ĕ������܂��B
        /// �j��I�ύX���������ꍇ�ȂǂɎg����B
        /// </summary>
        string gameVersion = "1";

        #endregion


        #region MonoBehaviour CallBacks
        /// <summary>
        /// Start���O�Ɏ��s�����񂶂�Ȃ��������ȁB�������B
        /// </summary>
        private void Awake()
        {
            //�d�v
            //PhotonNetwork.LoadLevel()��Master Client?�Ŏg����悤�ɂ���B
            //���������ɂ��郆�[�U�[��Level?�������I�ɓ�������
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            Connect();
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// �ڑ����J�n����B
        /// - ���łɐڑ��ς݂̏ꍇ�́A�����_����Room�ɎQ������
        /// - ���ڑ��̏ꍇ�́A�A�v���P�[�V�����̃C���X�^���X��Photon Cloud�ɐڑ�����
        /// </summary>
        private void Connect()
        {
            // �ڑ����݂����m�F����B
            //�@���ڑ��̏ꍇ�̓T�[�o�[�ւ̐ڑ��̏��������s��
            if (PhotonNetwork.IsConnected)
            {
                // �d�v!
                //�����Ń����_����Room�ɎQ������
                //Room�ւ̎Q���Ɏ��s�����ꍇ�AOnJoinRandomFailed()���󂯎��B
                //������Room��V�K�쐬����
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                //�d�v!
                //�܂���Photon�I�����C���T�[�o�[�ɐڑ����Ȃ���΂Ȃ�Ȃ�
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            //override�̂Ƃ��Ƀx�[�X�N���X���\�b�h���Ăяo���Ȃ�����!
            //base.OnConnectedToMaster();
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() ��PUN�ɂ���ČĂяo����܂���");

            //�d�v
            //�󂫂�Room�ɎQ�����邱�Ƃ����݂�B
            //�Q���ł��Ȃ������ꍇ��OnJoinRandomFailed()���Ă΂��B
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            //base.OnDisconnected(cause);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() ��PUN�ɂ���ė��R {0} �ŌĂяo����܂���", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            //base.OnJoinRandomFailed(returnCode, message);
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() PUN�ɂ���ČĂяo����܂����B�Q���\��Room��������܂���ł����B�V�����쐬���܂��B\n�Ăяo����:PhotonNetwork.CreateRoom");

            //Room�ւ̎Q���Ɏ��s���܂����B�������Ȃ����A���ׂĖ����̉\��������܂��B
            //�Ȃ̂ŁA������V�K�쐬���܂�
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            //base.OnJoinedRoom();

        }

        #endregion
    }
}