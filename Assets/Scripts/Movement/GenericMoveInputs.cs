using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Simulator.Movement {

    public delegate void OnCameraUpDown(CameraUpDownType udStatus);
    public delegate void OnCameraRotate(Vector2 rotatePercentage);
    public delegate void OnCameraZooming(float zoomDelta);
    public delegate void OnCharacterMove(Vector3 moveForward);

    public enum CameraUpDownType
    {
        None, Up, Down
    }

    public enum ZoomingStatus
    {
        None, In, Out
    }

    /// <summary>
    /// MouseSingleDown:only rotate camera
    /// MouseDoubleDown:camera rotate and char move(direction:camera forward)
    /// CharMoveCameraRotate:camera rotate and char move(direction:wasd)
    /// </summary>
    public enum InputStatus {
        Default, MouseSingleDown, MouseDoubleDown, OnlyCharMove, CharMoveCameraRotate, CameraUpDown, CameraUpDownTemp
    }

    public class GenericMoveInputs {
        static public InputStatus inputStatus = InputStatus.Default;
        static public ZoomingStatus zoomStatus = ZoomingStatus.None;
        static public CameraUpDownType cameraUDStatus = CameraUpDownType.None;

        public event OnCameraUpDown onCameraUpDown;
        public event OnCameraRotate onCameraRotate;
        public event OnCameraZooming onCameraZooming;
        public event OnCharacterMove onCharacterMove;

        public GameObject mainPlayer;
        public GenericMoveCamera cameraCtrl;
        public virtual void Initialize(GameObject aMainPlayerObj) {
            mainPlayer = aMainPlayerObj;
            cameraCtrl = App.Instance.CameraCtrl;
        }

        public void Start() {

        }

        public void Update() {
            updateInputStatus();
            Vector2 mousePer = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 moveDir;
            switch(inputStatus){
                case InputStatus.Default:
                    break;
                case InputStatus.CameraUpDown:
                    onCameraUpDown?.Invoke(cameraUDStatus);
                    break;
                case InputStatus.CameraUpDownTemp:
                    break;
                case InputStatus.MouseSingleDown:
                    onCameraRotate?.Invoke(mousePer);
                    break;
                case InputStatus.MouseDoubleDown:
                    Vector3 cameraDir = cameraCtrl.getCameraForwardDir();
                    onCameraRotate?.Invoke(mousePer);
                    onCharacterMove?.Invoke(new Vector3(cameraDir.x, cameraDir.y, cameraDir.z));
                    break;
                case InputStatus.OnlyCharMove:
                    moveDir = getMoveDir(cameraCtrl.getCameraForwardDir());
                    onCharacterMove?.Invoke(moveDir);
                    break;
                case InputStatus.CharMoveCameraRotate:
                    onCameraRotate?.Invoke(mousePer);
                    moveDir = getMoveDir(cameraCtrl.getCameraForwardDir());
                    onCharacterMove?.Invoke(moveDir);
                    break;
                default:
                    break;
            }

            switch(zoomStatus){
                case ZoomingStatus.In:
                case ZoomingStatus.Out:
                    onCameraZooming?.Invoke(Input.mouseScrollDelta.y);
                    break;
                case ZoomingStatus.None:
                default:
                    break;
            }
        }

        private void updateInputStatus() {
            // zoom
            if (Input.mouseScrollDelta.y > 0){
                zoomStatus = ZoomingStatus.In;
            }
            else if(Input.mouseScrollDelta.y < 0){
                zoomStatus = ZoomingStatus.Out;
            }
            else{
                zoomStatus = ZoomingStatus.None;
            }

            // input
            // camera updown
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
                if (Input.GetKey(KeyCode.UpArrow)){
                    inputStatus = InputStatus.CameraUpDown;
                    cameraUDStatus = CameraUpDownType.Up;
                }
                else if(Input.GetKey(KeyCode.DownArrow)){
                    inputStatus = InputStatus.CameraUpDown;
                    cameraUDStatus = CameraUpDownType.Down;
                }
                else{
                    inputStatus = InputStatus.CameraUpDownTemp;
                    cameraUDStatus = CameraUpDownType.None;
                }
            }
            else
            {
                bool isMouseLeft = Input.GetMouseButton(0);
                bool isMouseRight = Input.GetMouseButton(1);
                bool isW = Input.GetKey(KeyCode.W);
                bool isA = Input.GetKey(KeyCode.A);
                bool isS = Input.GetKey(KeyCode.S);
                bool isD = Input.GetKey(KeyCode.D);
                bool isWASDLegal = false;

                if (isW || isA || isS || isD)
                {
                    int pushNum = 0;
                    if (isW)
                        pushNum++;
                    if (isA)
                        pushNum++;
                    if (isS)
                        pushNum++;
                    if (isD)
                        pushNum++;
                    
                    if (pushNum <= 2)
                        isWASDLegal = true;
                    else
                        isWASDLegal = false;

                    if (isWASDLegal)
                        if ((isW && isA) || (isS && isD))
                            isWASDLegal = false;
                }

                if (isWASDLegal)
                {
                    if (!(isMouseLeft || isMouseRight))
                        inputStatus = InputStatus.OnlyCharMove;// 只按了wasd
                    else
                        inputStatus = InputStatus.CharMoveCameraRotate;
                }
                else
                {
                    if (isMouseLeft && isMouseRight)
                        inputStatus = InputStatus.MouseDoubleDown;
                    else if (!isMouseLeft && !isMouseRight)
                        inputStatus = InputStatus.Default;
                    else 
                        inputStatus = InputStatus.MouseSingleDown;
                }
            }
        }

        private Vector3 getMoveDir(Vector3 cameraDir)
        {
            Vector3 kbDir = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
                kbDir.z = 1;
            if (Input.GetKey(KeyCode.S))
                kbDir.z = -1;
            if (Input.GetKey(KeyCode.A))
                kbDir.x = -1;
            if (Input.GetKey(KeyCode.D))
                kbDir.x = 1;

            kbDir = kbDir.normalized;
            kbDir = kbDir + cameraDir;
            kbDir = kbDir.normalized;

            return kbDir;
        }

        public void Dispose()
        {
            onCameraUpDown = null;
            onCameraRotate = null;
            onCharacterMove = null;
        }
    }
}