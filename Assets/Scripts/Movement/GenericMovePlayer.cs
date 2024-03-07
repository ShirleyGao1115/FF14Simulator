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

        public void Awake()
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
            mInputs.onCharacterLookAt += onCharacterLookAt;
        }

        public void onCharacterMove(float moveForwardAngle)
        {
            Vector3 moveDir = Quaternion.Euler(0, moveForwardAngle * Mathf.Rad2Deg, 0) * Vector3.forward;
            Vector3 deltaPos = moveDir * Speed;
            this.transform.position += deltaPos;
            this.onCharacterLookAt(moveForwardAngle);
        }

        public void onCharacterLookAt(float moveForwardAngle)
        {
            this.transform.eulerAngles = new Vector3(0, moveForwardAngle * Mathf.Rad2Deg, 0);
        }

        public void Disable()
        {
            mInputs.onCharacterMove -= onCharacterMove;
            mInputs.onCharacterLookAt -= onCharacterLookAt;
        }
    }

}
