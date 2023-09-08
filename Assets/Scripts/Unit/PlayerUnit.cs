using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator {
    public class PlayerUnit : MonoBehaviour
    {
        public enum PlayerRace{
            Human, Food
        };

        [SerializeField]
        public PlayerRace Race;

        [SerializeField]
        public float PlayerHeight = 1.0f;
    }

}
