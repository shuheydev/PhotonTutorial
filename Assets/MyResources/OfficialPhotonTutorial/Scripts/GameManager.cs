using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Com.Harusoft.PhotonTutorial
{
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

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}
