﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulator.Movement {

    public class GenericMoveCamera : MonoBehaviour {
        public GenericMoveInputs GetInputs = App.Instance?.MoveInputs;
        private Camera mCamera;
        public float MinimumZoom = 10f;
        public float MaximumZoom = 70f;

        [Header("Look At")]
        private GameObject LookAtTarget = null;
        public PlayerUnit CharacterTarget = null;
        private Vector3 CameraCharacterOffset;

        [SerializeField]
        private float DeltaY = 0.0f;
        private float UpDeltaY = 1.0f;
        private float DownDeltaY = -1.0f;
        public float SingleDeltaY = 0.2f;

        private Movement _Forward;
        private Movement _PanX;
        private Movement _RotateX;
        private Movement _PanY;
        private Movement _RotateY;
        private float _Resolution = 1f;

        [Header("Operational")]
        public bool Operational = true;


        [Header("Camera")]
        public bool LevelCamera = true;
        [Range(0f,15f)]
        public float LevelCameraAngleThreshold = 7.5f;
        public bool ForwardMovementLockEnabled = true;

        [Header("Movement Speed")]
        public float MovementSpeedMagnification = 1f;
        public float WheelMouseMagnification = 5f;
        public float ShiftKeyMagnification = 2f;
        public float ControlKeyMagnification = 0.25f;

        public float RotationMagnification = 1f;

        [Header("Pan Speed Modifications")]
        public float PanLeftRightSensitivity = 1f;
        public float PanUpDownSensitivity = 1f;

        [Header("Mouse Rotation Sensitivity")]
        public float MouseRotationSensitivity = 0.5f;

        [Header("Dampening")]
        public float ForwardDampenRate = 0.99f;
        public float PanningDampenRate = 0.95f;
        public float RotateDampenRate = 0.99f;


        [Header("Movement Limits - X")]
        public bool LockX = false;
        public bool UseXRange;
        public float XRangeMin;
        public float XRangeMax;

        [Header("Movement Limits - Y")]
        public bool LockY = false;
        public bool UseYRange;
        public float YRangeMin;
        public float YRangeMax;

        [Header("Movement Limits - Z")]
        public bool LockZ = false;
        public bool UseZRange;
        public float ZRangeMin;
        public float ZRangeMax;

        // Rotation when in Awake(), to prevent weird rotations later
        private Vector3 _DefaultRotation;

        private class Movement {
            private readonly Action<float> _Action;
            private readonly Func<float> _DampenRate;
            private float _Velocity;
            private float _Dampen;

            public Movement(Action<float> aAction, Func<float> aDampenRate) {
                _Action = aAction;
                _DampenRate = aDampenRate;
                _Velocity = 0f;
                _Dampen = 0;
            }

            public void ChangeVelocity(float aAmount) {
                _Velocity += aAmount;
                _Dampen = _DampenRate();
            }

            public void SetVelocity(float aAmount) {
                _Velocity = aAmount;
                _Dampen = _DampenRate();
            }

            public void Update(bool aDampen = true) {
                if (_Dampen > 0)
                    if (_Velocity >= -0.001f && _Velocity <= 0.001f) {
                        _Dampen = 0;
                        _Velocity = 0;
                    } else {
                        if (aDampen)
                            _Velocity *= _Dampen;

                        _Action(_Velocity);
                    }
            }
        }

        public void SetResolution(float aResolution) {
            _Resolution = aResolution;
        }

        public void Awake() {
            if ( GetInputs == null && App.Instance != null )
                GetInputs = App.Instance.MoveInputs;

            _DefaultRotation = gameObject.transform.localRotation.eulerAngles;
            mCamera = this.transform.GetComponent<Camera>();
        }

        public void Start() {
            if ( GetInputs == null && App.Instance != null )
            {
                GetInputs = App.Instance.MoveInputs;
                Debug.Log("Camera Get Inputs Set");
            }

            if (CharacterTarget == null)
            {
                GameObject playerObj = App.Instance.MainPlayer;
                CharacterTarget = playerObj.GetComponentInChildren<PlayerUnit>();
                CameraCharacterOffset = this.transform.position - CharacterTarget.transform.position;
            }

            if (LookAtTarget == null)
            {
                LookAtTarget = new GameObject("LookAtTarget");
                LookAtTarget.transform.parent = CharacterTarget.transform.parent;
                SynchronizeLookAtPosition();
            }


            // if (LookAtTarget == null) {
            //     _Forward = new Movement(aAmount => gameObject.transform.Translate(Vector3.forward*aAmount), () => ForwardDampenRate);
            // } else {
            //     _Forward = new Movement(aAmount => gameObject.GetComponent<UnityEngine.Camera>().fieldOfView += aAmount, () => ForwardDampenRate);
            // }

            // _PanX = new Movement(aAmount => gameObject.transform.Translate(Vector3.left*aAmount), () => PanningDampenRate);
            // _PanY = new Movement(aAmount => gameObject.transform.Translate(Vector3.up*aAmount), () => PanningDampenRate);

            // _RotateX = new Movement(aAmount => gameObject.transform.Rotate(Vector3.up*aAmount), () => RotateDampenRate);
            // _RotateY = new Movement(aAmount => gameObject.transform.Rotate(Vector3.left*aAmount), () => RotateDampenRate);

        }

        public void SynchronizeLookAtPosition()
        {
            Vector3 position = CharacterTarget.transform.position;
            position.y += CharacterTarget.PlayerHeight + DeltaY;
            LookAtTarget.transform.position = position;
        }

        public void Update() {

            if (!Operational)
                return;

            GetInputs.QueryInputSystem();

            // zoom
            if (GetInputs.isZoomIn || GetInputs.isZoomOut)
            {
                float deltaZoom = GetInputs.isZoomIn ? -5.0f: 5.0f;
                float targetFov = mCamera.fieldOfView + deltaZoom;
                targetFov = Math.Min(MaximumZoom, Math.Max(MinimumZoom, targetFov));

                mCamera.fieldOfView = targetFov;
            }

            // camera up down
            if (GetInputs.cameraUpDownType != CameraUpDownType.None)
            {
                float deltaY = DeltaY + (GetInputs.cameraUpDownType == CameraUpDownType.Up ? SingleDeltaY : -1 * SingleDeltaY);
                DeltaY = Math.Min(UpDeltaY, Math.Max(DownDeltaY, deltaY));
            }

            // player position sync
            Vector3 newCameraPos = CharacterTarget.transform.position + CameraCharacterOffset;
            this.transform.position = newCameraPos;

            SynchronizeLookAtPosition();
            this.transform.LookAt(LookAtTarget.transform);

            Vector3 START_POSITION = gameObject.transform.position;

            // if (GetInputs.ResetMovement) {
            //     ResetMovement();
            // } else {

            //     float MAG = (GetInputs.isSlowModifier ? ControlKeyMagnification : 1f)*(GetInputs.isFastModifier ? ShiftKeyMagnification : 1f);

            //     if (GetInputs.isPanLeft) {
            //         _PanX.ChangeVelocity(0.01f*MAG*_Resolution*PanLeftRightSensitivity);
            //     } else if (GetInputs.isPanRight) {
            //         _PanX.ChangeVelocity(-0.01f*MAG*_Resolution*PanLeftRightSensitivity);
            //     }

            //     if ( _PanX != null )
            //         _PanX.Update();

            //     if (GetInputs.isMoveForward ) {
            //         _Forward.ChangeVelocity(0.005f*MAG*_Resolution*MovementSpeedMagnification);
            //     } else if (GetInputs.isMoveBackward ) {
            //         _Forward.ChangeVelocity(-0.005f*MAG*_Resolution*MovementSpeedMagnification);
            //     }

            //     if (GetInputs.isMoveForwardAlt) {
            //         _Forward.ChangeVelocity(0.005f*MAG*_Resolution*MovementSpeedMagnification*WheelMouseMagnification);
            //     } else if (GetInputs.isMoveBackwardAlt) {
            //         _Forward.ChangeVelocity(-0.005f*MAG*_Resolution*MovementSpeedMagnification*WheelMouseMagnification);
            //     }

            //     if (GetInputs.isPanUp) {
            //         _PanY.ChangeVelocity(0.005f*MAG*_Resolution*PanUpDownSensitivity);
            //     } else if (GetInputs.isPanDown) {
            //         _PanY.ChangeVelocity(-0.005f*MAG*_Resolution*PanUpDownSensitivity);
            //     }

            //     bool FORWARD_LOCK = GetInputs.isLockForwardMovement && ForwardMovementLockEnabled;
            //     _Forward.Update(!FORWARD_LOCK);

            //     _PanY.Update();

            //     // Pan
            //     if (GetInputs.isRotateAction) {

            //         float X = (Input.mousePosition.x - GetInputs.RotateActionStart.x)/Screen.width*MouseRotationSensitivity;
            //         float Y = (Input.mousePosition.y - GetInputs.RotateActionStart.y)/Screen.height*MouseRotationSensitivity;

            //         _RotateX.SetVelocity(X*MAG*RotationMagnification*_Resolution);
            //         // _RotateY.SetVelocity(Y*MAG*RotationMagnification*_Resolution);

            //     }

            //     _RotateX.Update();
            //     // _RotateY.Update();
            // }


            // Lock at object
            // if (LookAtTarget != null ) {
            //     transform.LookAt(LookAtTarget.transform);
            //     if (gameObject.GetComponent<UnityEngine.Camera>().fieldOfView < MinimumZoom) {
            //         ResetMovement();
            //         gameObject.GetComponent<UnityEngine.Camera>().fieldOfView = MinimumZoom;
            //     } else if (gameObject.GetComponent<UnityEngine.Camera>().fieldOfView > MaximumZoom) {
            //         ResetMovement();
            //         gameObject.GetComponent<UnityEngine.Camera>().fieldOfView = MaximumZoom;
            //     }
            // }

            // Set ranges
            Vector3 END_POSITION = transform.position;

            // if (LockX)
            //     END_POSITION.x = START_POSITION.x;
            // if (LockY)
            //     END_POSITION.y = START_POSITION.y;
            // if (LockZ)
            //     END_POSITION.z = START_POSITION.z;

            // if (UseXRange && gameObject.transform.position.x < XRangeMin) END_POSITION.x = XRangeMin;
            // if (UseXRange && gameObject.transform.position.x > XRangeMax) END_POSITION.x = XRangeMax;

            // if (UseYRange && gameObject.transform.position.y < YRangeMin) END_POSITION.y = YRangeMin;
            // if (UseYRange && gameObject.transform.position.y > YRangeMax) END_POSITION.y = YRangeMax;

            // if (UseZRange && gameObject.transform.position.z < ZRangeMin) END_POSITION.z = ZRangeMin;
            // if (UseZRange && gameObject.transform.position.z > ZRangeMax) END_POSITION.z = ZRangeMax;

            // transform.position = END_POSITION;

            // Level Camera
            // if (LevelCamera) {
            //     // Fix 1.2
            //     // When leveling the camera you want to make sure you don't look straight up or straight down, otherwise the camera rolls wildly.
            //     // This code prevents this rolling from occurring.
            //     Vector3 ROT = gameObject.transform.rotation.eulerAngles;
            //     if (ROT.x > 180)
            //         ROT.x -= 360;

            //     if (ROT.x > (90-LevelCameraAngleThreshold)) {
            //         ROT.x = (90-LevelCameraAngleThreshold);
            //         gameObject.transform.rotation = Quaternion.Euler(ROT);
            //     } else if (ROT.x < (-90+LevelCameraAngleThreshold)) {
            //         ROT.x = -90+LevelCameraAngleThreshold;
            //         gameObject.transform.rotation = Quaternion.Euler(ROT);

            //     }

            //     LevelTheCamera();
            // }

        }

        public void ResetMovement() {
            _PanX.SetVelocity(0);
            _PanY.SetVelocity(0);
            _Forward.SetVelocity(0);
            _RotateX.SetVelocity(0);
            _RotateY.SetVelocity(0);

            _PanX.Update();
            _PanY.Update();
            _Forward.Update();
            _RotateX.Update();
            _RotateY.Update();
        }

        public void OnCollisionEnter(Collision collision) {
            ResetMovement();
        }

        public void PanY( float aMagnitude ) {
            _PanY.ChangeVelocity(0.005f*aMagnitude*_Resolution*PanUpDownSensitivity);
        }

        public void PanX(float aMagnitude) {
            _PanX.ChangeVelocity(-0.01f*aMagnitude*_Resolution*PanLeftRightSensitivity);
        }

        public void ForwardBack( float aMagnitude ) {
            _Forward.ChangeVelocity(-0.005f*aMagnitude*_Resolution*MovementSpeedMagnification);
        }

        public void LevelTheCamera() {
            transform.rotation = Quaternion.LookRotation(transform.forward.normalized, Vector3.up);
        }

    }

}