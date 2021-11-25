using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Harusoft.PhotonTutorial
{
    /// <summary>
    /// Player名入力欄。プレイヤーに名前を入力してもらう。
    /// プレイヤー名はキャラクターの上に表示される
    /// </summary>
    [RequireComponent(typeof(InputField))]// InputFieldを強制することができる。
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        //プレイヤー名のためのキー
        const string playerNamePrefKey = "PlayerName";

        #endregion

        #region MonoBehaviour Callbacks

        /// <summary>
        /// オブジェクトの初期化時に呼ばれる
        /// </summary>
        private void Start()
        {
            string defaultName = string.Empty;
            InputField inputField = this.GetComponent<InputField>();
            if (inputField != null)
            {
                //PlayerPrefsクラスからキーを使って値(プレイヤー名)を取得する
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    inputField.text = defaultName;
                }
            }

            //
            PhotonNetwork.NickName = defaultName;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// プレイヤー名をセットして、PlayerPrefsに保存する。
        /// 現時点ではまだ呼ばれない。
        /// 将来的にはユーザーがInputFieldを編集するたびに記録するために呼び出されるようにする。
        /// InputFieldのOnValueChangedとSetPlayerName()を結びつける。
        /// </summary>
        /// <param name="value"></param>
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("プレイヤー名が空です。入力してください");
                return;
            }

            PhotonNetwork.NickName = value;

            //入力されたプレイヤー名をキーとセットで保存する。
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
        #endregion
    }
}