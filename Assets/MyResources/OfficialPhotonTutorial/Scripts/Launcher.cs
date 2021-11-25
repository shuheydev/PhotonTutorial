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

        [Tooltip("1Roomあたりの最大プレイヤー数。Roomが満席の場合、新しいプレイヤーは参加できず、新しいRoomが作成されます")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion

        #region Private Fields
        /// <summary>
        /// クライアントのバージョン番号です。
        /// ユーザーはgameVersionによって分けられます。
        /// 破壊的変更があった場合などに使える。
        /// </summary>
        string gameVersion = "1";

        [Tooltip("UI Panel。名前の入力や接続、プレイを担当する")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("UI Label。接続中であることをユーザーに提示する")]
        [SerializeField]
        private GameObject progressLabel;

        /// <summary>
        /// 
        /// </summary>
        bool isConnecting;

        #endregion


        #region MonoBehaviour CallBacks
        /// <summary>
        /// Startより前に実行されるんじゃなかったかな。たしか。
        /// </summary>
        private void Awake()
        {
            //重要
            //PhotonNetwork.LoadLevel()をMaster Client?で使えるようにする。
            //同じ部屋にいるユーザーがLevel?を自動的に同期する
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            //Button押下時に呼ばれるようにした。
            //Connect();

            progressLabel.SetActive(false);//非表示
            controlPanel.SetActive(true);
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// 接続を開始する。
        /// これはユーザーによってPlayボタンが押されたときに実行されるようにしている。
        /// - すでに接続済みの場合は、ランダムにRoomに参加する
        /// - 未接続の場合は、アプリケーションのインスタンスをPhoton Cloudに接続する
        /// </summary>
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // 接続ずみかを確認する。
            //　未接続の場合はサーバーへの接続の初期化を行う
            if (PhotonNetwork.IsConnected)
            {
                // 重要!
                //ここでランダムにRoomに参加する
                //Roomへの参加に失敗した場合、OnJoinRandomFailed()を受け取る。
                //そしてRoomを新規作成する
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                //重要!
                //まずはPhotonオンラインサーバーに接続しなければならない
                PhotonNetwork.GameVersion = gameVersion;
                isConnecting = PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        /// <summary>
        /// Photonによっていろんなタイミングで呼ばれる。
        /// ユーザーによって接続されたときだけRoomに接続するようにしたい。
        /// そのためのisConnectingフラグ
        /// </summary>
        public override void OnConnectedToMaster()
        {
            //overrideのときにベースクラスメソッドを呼び出さないこと!
            //base.OnConnectedToMaster();
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() はPUNによって呼び出されました");

            if (isConnecting)
            {
                //重要
                //空きのRoomに参加することを試みる。
                //参加できなかった場合はOnJoinRandomFailed()が呼ばれる。
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;

            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            //base.OnDisconnected(cause);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() はPUNによって理由 {0} で呼び出されました", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            //base.OnJoinRandomFailed(returnCode, message);
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() PUNによって呼び出されました。参加可能なRoomが見つかりませんでした。新しく作成します。\n呼び出し中:PhotonNetwork.CreateRoom");

            //Roomへの参加に失敗しました。部屋がないか、すべて満室の可能性があります。
            //なので、部屋を新規作成します
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            //base.OnJoinedRoom();

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Room for 1をロードしました");

                PhotonNetwork.LoadLevel("Room for 1");
            }
        }

        #endregion
    }
}