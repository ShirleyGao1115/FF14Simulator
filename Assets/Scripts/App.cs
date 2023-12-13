using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulator;
using Simulator.Movement;


public class App : MonoBehaviour
{
    public static App Instance = null;

    public GenericMoveInputs MoveInputs;

    public GameObject MainPlayer;

    void Awake()
    {
        if (Instance == null)
        {
            Init();
        }
        else
        {

        }
    }

    void Init()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        MainPlayer = UnitManager.CreatePlayer("MainPlayer");

        MoveInputs = new GenericMoveInputs();
        MoveInputs.Initialize(MainPlayer);

        Debug.Log($"mainplayer = {MainPlayer != null}");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveInputs.Update();
    }

    private void OnDestroy() {
        MoveInputs.Dispose();
    }
}

