using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator.Movement
{
    public class GenericMovePlayer : MonoBehaviour
    {

        public GenericMoveInputs GetInpus;

        public Camera MainCamera;

        public GenericMoveCamera CameraMover;

        public void  Awake()
        {
            if (GetInpus == null)
            {
                GetInpus = App.Instance.MoveInputs;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
