using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulator.Movement {

    public enum CameraUpDownType
    {
        None, Up, Down
    }

    public class GenericMoveInputs {
        public bool isPlayerMove;// player move
        public bool isPlayerAutoMove;
        public Vector3 playerMoveDirection;

        public bool isCameraRotate = false; 
        public bool isCameraMove;
        public CameraUpDownType cameraUpDownType = CameraUpDownType.None;
        public bool isZoomIn;
        public bool isZoomOut;

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

            isZoomIn = Input.GetAxis("Mouse ScrollWheel") > 0;
            isZoomOut = Input.GetAxis("Mouse ScrollWheel") < 0;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                isPlayerMove = true;

                playerMoveDirection = new Vector3(0, 0, 0);
                if (Input.GetKey(KeyCode.W))
                    playerMoveDirection.z += 1;

                if (Input.GetKey(KeyCode.S))
                    playerMoveDirection.z -= 1;

                if (Input.GetKey(KeyCode.A))
                    playerMoveDirection.x += 1;

                if (Input.GetKey(KeyCode.D))
                    playerMoveDirection.x -= 1;
            }
            else
                isPlayerMove = false;

            if (Input.GetKey(KeyCode.LeftControl) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    cameraUpDownType = CameraUpDownType.Up;
                else
                    cameraUpDownType = CameraUpDownType.Down;
            }
            else
                cameraUpDownType = CameraUpDownType.None;

        }

    }
}