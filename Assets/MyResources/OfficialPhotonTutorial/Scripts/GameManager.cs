using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Com.Harusoft.PhotonTutorial
{
    /// <summary>
    /// プレイヤーの接続や切断をリッスンする役割がある。
    /// </summary>
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Photone Callbacks

        /// <summary>
        /// ローカルのプレイヤーがRoomを退出するときに呼ばれる。
        /// LauncherScene(ユーザー名を入力する画面)を表示する
        /// </summary>
        public override void OnLeftRoom()
        {
            //base.OnLeftRoom();
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// プレイヤーが入室するたびに呼ばれる。
        /// Master Client(部屋主かな)の場合は、LoadArenaを呼ぶ。
        /// つまり、人数に合わせて適したアリーナのSceneをロードする。
        /// </summary>
        /// <param name="other"></param>
        public override void OnPlayerEnteredRoom(Player other)
        {
            //base.OnPlayerEnteredRoom(newPlayer);
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //base.OnPlayerLeftRoom(otherPlayer);
            Debug.LogFormat("OnPlayerLefRoom() {0}", otherPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ここでいうLevelというのは、環境(Scene)のことのようだ。
        /// 部屋の大きさであったり、多分アイテムの配置であったり。
        /// </summary>
        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork: Levelをロードしています。でも、私達はMaster Clientではないみたい");
            }

            Debug.LogFormat("PhotonNetwork: レベル : {0} をロード中。", PhotonNetwork.CurrentRoom.PlayerCount);

            //Launcher.AwakeメソッドのPhotonNetwork.AutomaticallySyncScene = true;は、ここで効果を持つ。
            //ルーム内の接続中のすべてのクライアントが同じSceneを読み込むようになっている
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
        #endregion
    }
}
