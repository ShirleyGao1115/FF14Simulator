using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator.Movement
{
    public class GenericMovePlayer : MonoBehaviour
    {

        public GenericMoveInputs GetInputs;

        public Camera MainCamera;

        public GenericMoveCamera CameraMover;

        private Transform mTrans;

        public float Speed = 1.0f;

        public void  Awake()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {
            if (GetInputs == null)
            {
                GetInputs = App.Instance.MoveInputs;
            }
            mTrans = this.transform;
        }

        // Update is called once per frame
        void Update()
        {
            GetInputs.QueryInputSystem();
            if (GetInputs != null && GetInputs.isPlayerMove)
            {
                Debug.Log("is player move");
                Vector3 position = mTrans.position + GetInputs.playerMoveDirection * Speed;
                mTrans.position = position;
            }
        }
    }

}
