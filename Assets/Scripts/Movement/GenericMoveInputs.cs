using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulator.Movement {

    public class GenericMoveInputs : MonoBehaviour {
        public bool isPlayerMove;  // player move
        public bool isPlayerAutoMove;
        public Vector3 playeMoveDirection;

        public bool isCameraMove;
        public bool isCameraRotate;
        public bool isCameraUpDown;

        public virtual void Initialize() {
            // RotateActionStart = new Vector2();
        }

        public virtual void QueryInputSystem() {

            // isSlowModifier = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
            // isFastModifier = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
            // isRotateAction = Input.GetButton("Fire2");

            // // Get mouse starting point when the button was clicked.
            // if ( Input.GetButtonDown("Fire2") ) {
            //     RotateActionStart.x = Input.mousePosition.x;
            //     RotateActionStart.y = Input.mousePosition.y;
            // }
            // isLockForwardMovement = Input.GetButton("Fire3");
            // ResetMovement = Input.GetKey(KeyCode.Space);

            // isPanLeft = Input.GetKey(KeyCode.A);
            // isPanRight = Input.GetKey(KeyCode.D);
            // isPanUp = Input.GetKey(KeyCode.Q);
            // isPanDown = Input.GetKey(KeyCode.Z);

            // isMoveForward = Input.GetKey(KeyCode.W);
            // isMoveBackward = Input.GetKey(KeyCode.S);

            // isMoveForwardAlt = Input.GetAxis("Mouse ScrollWheel") > 0;
            // isMoveBackwardAlt = Input.GetAxis("Mouse ScrollWheel") < 0;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                isPlayerMove = true;
            }

            

        }

    }
}