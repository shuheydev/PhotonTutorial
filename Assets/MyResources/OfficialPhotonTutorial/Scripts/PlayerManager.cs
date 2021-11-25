using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Harusoft.PhotonTutorial
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        [Tooltip("Beam Object")]
        [SerializeField]
        private GameObject beams;

        bool isFiring;

        //PhotonView photonView;

        #endregion

        #region Public Fields

        [Tooltip("現在のHP")]
        public float Health = 1f;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                beams.SetActive(false);
            }
        }

        private void Update()
        {
            ProcessInputs();

            if (photonView.IsMine)
            {
                ProcessInputs();
                if (Health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }

            // 条件:
            // 1.Beamオブジェクトがちゃんとセットされていること
            // 2.Beamの表示状態とisFiringが一致しないこと。
            if (beams != null && isFiring != beams.activeInHierarchy)
            {
                beams.SetActive(isFiring);
            }
        }

        /// <summary>
        /// プレイヤーとビームがぶつかったときの処理
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            //
            if (photonView != null)
            {
                //自分じゃない場合は何もしない。
                //自分の体力だけを減らす
                //IsMineで自分とその他のプレイヤーを判別できるということか
                if (!photonView.IsMine)
                {
                    return;
                }
            }

            //シューティングの方ではタグを使って判別していたな。
            //ビーム以外のものと衝突しても何もしない。
            if (!other.name.Contains("Beam"))
            {
                return;
            }

            Health -= 0.1f;//
        }

        private void OnTriggerStay(Collider other)
        {
            //
            if (photonView != null)
            {
                //自分じゃない場合は何もしない。
                //自分の体力だけを減らす
                //IsMineで自分とその他のプレイヤーを判別できるということか
                if (!photonView.IsMine)
                {
                    return;
                }
            }

            //シューティングの方ではタグを使って判別していたな。
            //ビーム以外のものと衝突しても何もしない。
            if (!other.name.Contains("Beam"))
            {
                return;
            }

            Health -= 0.1f * Time.deltaTime;//
        }


        /// <summary>
        /// 押されている間はビームを表示し、
        /// 離すとビームを非表示にする
        /// </summary>
        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!isFiring)
                {
                    isFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (isFiring)
                {
                    isFiring = false;
                }
            }
        }
        #endregion
    }
}
