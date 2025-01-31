﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LaunchpadDir { Floor, Ceiling, Left, Right };

public class EnvironmentManager : MonoBehaviour
{
    // State machine and states
    public StateMachine stateMachine;
    public EnviroSpongeState enviroSpongeState;
    public EnviroLaunchState enviroLaunchState;
    public EnviroEvadeState enviroEvadeState;

    // EnvironmentManager components
    public LaunchpadManager launchpadManager;
    public EnvironmentData enviroData;

    private void Awake()
    {
        // Environment Components
        launchpadManager = GetComponent<LaunchpadManager>();
        enviroData = GetComponent<EnvironmentData>();

        // State machine setup
        enviroSpongeState = new EnviroSpongeState(this);
        enviroLaunchState = new EnviroLaunchState(this);
        enviroEvadeState = new EnviroEvadeState(this);
        stateMachine = new StateMachine(enviroSpongeState);
    }
}
