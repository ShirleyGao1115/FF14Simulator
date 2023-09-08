using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Simulator {
    public class UnitManager
    {
        private static string playerPrefabPath = "Assets/TestAsset/GameResources/prefab/character/songoku_1.prefab";
        public void Initialize()
        {
            
        }

        public static GameObject CreatePlayer(string aPlayerName)
        {
            GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(playerPrefabPath);

            GameObject instance = GameObject.Instantiate<GameObject>(playerPrefab);
            instance.name = aPlayerName;

            return instance;
        }
    }

}
