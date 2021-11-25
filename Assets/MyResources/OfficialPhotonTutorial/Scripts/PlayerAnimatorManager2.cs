using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Harusoft.PhotonTutorial
{
    //[RequireComponent(typeof(Animator))]
    public class PlayerAnimatorManager2 : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        private float directionDampTime = 0.25f;

        #endregion

        private Animator animator;

        #region MonoBehaviour Callbacks
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();

            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager はAnimator Componentを見つけられません", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!animator)
            {
                return;
            }

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //走っているときだけジャンプできるように
            if (stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0)
            {
                v = 0;
            }

            animator.SetFloat("Speed", h * h + v * v);

            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }
        #endregion
    }
}