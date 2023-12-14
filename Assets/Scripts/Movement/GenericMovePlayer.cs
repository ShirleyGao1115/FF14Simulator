using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator.Movement
{
    public class GenericMovePlayer : MonoBehaviour
    {

        public GenericMoveInputs mInputs = App.Instance?.MoveInputs;

        private PlayerUnit mPlayerInfo;

        public float Speed = 0.1f;

        public void  Awake()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void AddDelegate()
        {
            if (mInputs == null)
                mInputs = App.Instance.MoveInputs;
            
            mInputs.onCharacterMove += onCharacterMove;
        }

        public void onCharacterMove(Vector3 moveForward)
        {
            Vector3 moveDir = new Vector3(moveForward.x, 0, moveForward.z);
            Vector3 deltaPos = moveDir * Speed;
            this.transform.position += deltaPos;
        }

        public void Disable()
        {
            mInputs.onCharacterMove -= onCharacterMove;
        }
    }

}
