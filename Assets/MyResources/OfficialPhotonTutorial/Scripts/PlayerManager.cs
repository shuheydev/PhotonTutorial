using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Harusoft.PhotonTutorial
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Fields

        [Tooltip("Beam Object")]
        [SerializeField]
        private GameObject beams;

        bool isFiring;

        [Tooltip("プレイヤーUIのプレファブ")]
        [SerializeField]
        private GameObject playerUiPrefab;


        #endregion

        #region Public Fields

        [Tooltip("現在のHP")]
        public float Health = 1f;

        [Tooltip("ローカルプレイヤーのインスタンス。ローカルプレイヤーがシーンに描画されていることを意味する")]
        public static GameObject LocalPlayerInstance;

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

            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }

            //重要!
            //ロード時にDestoryされないようにする。
            //これによってレベル同期時に破壊されない。
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            //カメラコンポーネントを取得
            CameraWork2 _cameraWork = this.gameObject.GetComponent<CameraWork2>();

            //取得できていることを確認する
            if (_cameraWork != null)
            {
                //自分の場合、追跡する
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayerPrefabのCameraWork2コンポーネント。", this);
            }

#if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            {
                this.CalledOnLevelWasLoaded(scene.buildIndex);
            };
#endif

            //PlayerUIを生成して、
            //自分(Player)オブジェクトをセットする
            //これでPlayerUIに名前とHPが表示される
            if (playerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(playerUiPrefab);
                //_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
                //または、PlayerUIコンポーネントを取得してSetTargetを呼び出す。
                _uiGo.GetComponent<PlayerUI2>().SetTarget(this);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }


        private void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
            }

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

        private void CalledOnLevelWasLoaded(int level)
        {
            //プレイヤーが減って、アリーナのサイズが小さくなったときに、
            //残ったプレイヤーがアリーナの外にでないように、アリーナの中心に戻す
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            //UIをセットする
            GameObject _uiGo = Instantiate(this.playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

#if !UNITY_5_4_OR_NEWER
        private void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif

        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //このプレイヤーを所有しています。データを他のプレイヤーに送ります。
                stream.SendNext(isFiring);
                stream.SendNext(Health);
            }
            else
            {
                //ネットワークプレイヤー。データ受信
                this.isFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }
        #endregion
    }
}
