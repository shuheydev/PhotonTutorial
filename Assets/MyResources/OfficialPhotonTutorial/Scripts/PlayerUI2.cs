using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Harusoft.PhotonTutorial
{
    public class PlayerUI2 : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("表示用のプレイヤー名")]
        [SerializeField]
        private Text playerNameText;

        [Tooltip("HP表示用スライダー")]
        [SerializeField]
        private Slider playerHealthSlider;

        private PlayerManager target;

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 targetPosition;

        #endregion

        #region Public Fields

        [Tooltip("プレイヤーからのオフセット(ピクセル)")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            //Canvasを親にする
            //シーンがロードされると、Canvasも当然新しいものになるので、
            //毎回探す。
            //ただし、Findメソッドは遅いのでおすすめしない。
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);

            _canvasGroup = this.GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            //現在のHPをスライダーに反映させる
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health;
            }

            //Playerオブジェクトが破棄されたら、
            //PlayerUIオブジェクトも破棄する
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
        }

        private void LateUpdate()
        {
            //カメラに写っていない場合はUIを表示しない。????
            if (targetRenderer != null)
            {
                this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            //重要!
            //スクリーン上で対象のオブジェクトを追跡する
            //ここではプレイヤー
            if (targetTransform != null)
            {
                //オブジェクトのワールド座標
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                //3Dなワールドからスクリーン上での座標に変換している?

                //オブジェクトのワールド座標をスクリーン座標に変換する
                var targetScreenPos = Camera.main.WorldToScreenPoint(targetPosition);

                
                this.transform.position = Camera.main.WorldToScreenPoint(targetPosition);// + screenOffset;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 渡されたPlayerに
        /// </summary>
        /// <param name="_target"></param>
        public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager はPlayerUI.SetTarget", this);
                return;
            }

            //PlayerManagerのインスタンスをキャッシュしておく。
            //頻繁に体力を確認するので。
            target = _target;
            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }

            //プレイヤーのTransformとRendererを取得する。
            //これらはLateUpdateで使用する
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
            //
            CharacterController characterController = _target.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }
        }
        #endregion
    }
}
