using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Harusoft.PhotonTutorial
{
    public class CameraWork2 : MonoBehaviour
    {

        #region Private Fields

        [Tooltip("x-z面から対象への距離")]
        [SerializeField]
        private float distance = 7.0f;

        [Tooltip("カメラの高さ")]
        [SerializeField]
        private float height = 3.0f;

        [Tooltip("対象に対して垂直方向へのオフセット。例えば床よりも遠景を多く移したい場合")]
        [SerializeField]
        private Vector3 centerOffset = Vector3.zero;

        [Tooltip("PhotonNetworkによってインスタンス化されるオブジェクトの場合は、falseを設定してください。そして、OnStartFollowing()メソッドを必要に応じて呼び出してください。")]
        [SerializeField]
        private bool followOnStart = false;

        [Tooltip("カメラが対象を追う際の滑らかさ")]
        [SerializeField]
        private float smoothSpeed = 0.125f;

        //対象のTransformをキャッシュする
        Transform cameraTransform;

        //対象をロストしたか、カメラが切り替わったときに、再接続するためのもの
        bool isFollowing;

        //カメラのオフセットをキャッシュする
        Vector3 cameraOffset = Vector3.zero;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (followOnStart)
            {
                OnStartFollowing();
            }
        }

        //LateUpdateってなんだろう?
        //Update関数が呼び出されたあとに実行される
        /// <summary>
        /// 
        /// </summary>
        private void LateUpdate()
        {
            //Transformの対象はLevel(Scene)のロード時にdestroyされない

            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }

            //追跡が明示的に指定されている場合
            if (isFollowing)
            {
                Follow();
            }
        }

        #region Public Methods

        /// <summary>
        /// 追跡開始イベントを
        /// </summary>
        private void OnStartFollowing()
        {
            //Main CameraのTransformをキャッシュする
            cameraTransform = Camera.main.transform;
            isFollowing = true;

            Cut();
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// 対象をなめらかに追跡する。
        /// 後ろから追いかけるのかな?
        /// </summary>
        private void Follow()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            //カメラの位置を更新する
            cameraTransform.position =
                Vector3.Lerp(
                    cameraTransform.position,
                    this.transform.position + this.transform.TransformVector(cameraOffset),
                    smoothSpeed * Time.deltaTime);

            //カメラが対象を向くようにする
            cameraTransform.LookAt(this.transform.position + centerOffset);
        }

        private void Cut()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

            cameraTransform.LookAt(this.transform.position + centerOffset);
        }

        #endregion




        #endregion
    }
}
