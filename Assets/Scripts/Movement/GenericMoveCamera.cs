using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulator.Movement {
    public class GenericMoveCamera {
        public GenericMoveInputs mInputs = App.Instance?.MoveInputs;
        private Camera mCamera;
        public Camera MainCamera{
            get {
                if (mCamera == null)
                    mCamera = Camera.main;
                return mCamera;
            }
        }

        public float minimumZoom = 10f;
        public float maximumZoom = 70f;

        [Header("Look At")]
        private GameObject lookAtTarget = null;
        private GameObject characterTarget = null;
        private PlayerUnit characterUnit = null;

        [SerializeField]
        private float deltaY = 0.0f;
        private float upDeltaY = 1.0f;
        private float downDeltaY = -1.0f;
        public float singleDeltaY = 0.1f;

        private float distanceToTarget;
        private Vector3 eulerAngles;
        private Quaternion rotate;

        private float sensitivity = 10f;
        private float yMinMax = 5f;

        public void Initialize(GameObject aMainPlayer)
        {
            characterTarget = aMainPlayer;
            characterUnit = characterTarget.GetComponent<PlayerUnit>();
            lookAtTarget = new GameObject("LookAtTarget");
            SynchronizeLookAtPosition();

            distanceToTarget = (MainCamera.transform.position - lookAtTarget.transform.position).magnitude;
            eulerAngles = MainCamera.transform.eulerAngles;
            rotate = MainCamera.transform.rotation;
        }

        public void AddDelegate()
        {
            if (mInputs == null)
                mInputs = App.Instance.MoveInputs;
            mInputs.onCameraZooming += onZooming;
            mInputs.onCameraUpDown += onCameraUpDown;
            mInputs.onCameraRotate += onCameraRotate;
        }

        public Vector3 getCameraForwardDir()
        {
            return MainCamera.transform.forward;
        }

        public void SynchronizeLookAtPosition()
        {
            Vector3 position = characterTarget.transform.position;
            position.y += (characterUnit.PlayerHeight + deltaY);
            lookAtTarget.transform.position = position;
        }

        public void onZooming(float deltaY)
        {
            float deltaZoom = deltaY * -5.0f;
            float targetFov = mCamera.fieldOfView + deltaZoom;
            targetFov = Math.Min(maximumZoom, Math.Max(minimumZoom, targetFov));

            mCamera.fieldOfView = targetFov;
        }

        public void onCameraUpDown(CameraUpDownType aType)
        {
            float targetDelta = deltaY + (aType == CameraUpDownType.Up ? 1 : -1) * singleDeltaY;
            targetDelta = targetDelta > upDeltaY ? upDeltaY : targetDelta;
            targetDelta = targetDelta < downDeltaY ? downDeltaY : targetDelta;
            deltaY = targetDelta;
            SynchronizeLookAtPosition();
        }

        public void onCameraRotate(Vector2 rotatePercentage)
        {
            float rotateX = rotatePercentage.x * sensitivity;
            float rotateY = - rotatePercentage.y * 0.2f * sensitivity;
            Vector3 pos = rotateAroundPlayer(MainCamera.transform.position, Vector3.up, rotateX);
            
            MainCamera.transform.position = pos;
            MainCamera.transform.LookAt(lookAtTarget.transform);
            MainCamera.transform.Rotate(new Vector3(rotateY, 0, 0));
            rotate = MainCamera.transform.rotation;
        }

        private Vector3 rotateAroundPlayer(Vector3 pos, Vector3 axis, float angle)
        {
            Vector3 position = pos;
            Vector3 center = lookAtTarget.transform.position;

            return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
        } 

        public void Update()
        {
            // 保证角色跟随
            SynchronizeLookAtPosition();
            Quaternion q = rotate;
            MainCamera.transform.position = lookAtTarget.transform.position - q * Vector3.forward * distanceToTarget;
            MainCamera.transform.rotation = rotate;
        }

        public void Disable()
        {
            mInputs.onCameraZooming -= onZooming;
            mInputs.onCameraUpDown -= onCameraUpDown;
            mInputs.onCameraRotate -= onCameraRotate;
        }
    }

}