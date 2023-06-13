using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulator.Movement;

public class App : MonoBehaviour
{
    public static App Instance = null;

    private GenericMoveInputs MoveInputs;

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
        Application.targetFrameRate = 60;
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
