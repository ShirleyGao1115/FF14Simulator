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

            if (MainCamera == null)
            {
                MainCamera = Camera.main;
            }
            mTrans = this.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (GetInputs != null && GetInputs.isPlayerMove)
            {
                Debug.Log("is player move");
            }
        }
    }

}
