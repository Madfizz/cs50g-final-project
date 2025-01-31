﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMStartState : BaseState
{
    private Player player;
    private Boss boss;
    private EnvironmentManager enviroManager;
    private GameUI gameUI;

    public GMStartState(Player _player, Boss _boss, EnvironmentManager _enviroManager, GameUI _gameUI)
    {
        player = _player;
        boss = _boss;
        enviroManager = _enviroManager;
        gameUI = _gameUI;
    }

    public override void Enter()
    {
        player.stateMachine.ChangeState(player.playerStartState);
        boss.stateMachine.ChangeState(boss.bossStartState);
        enviroManager.stateMachine.ChangeState(enviroManager.enviroEvadeState); 
        gameUI.stateMachine.ChangeState(gameUI.gameUIStartState);
    }

    public override void LogicUpdate()
    {
        if (player.playerMovement.canControl)
        {
            GameManager.instance.gmStateMachine.ChangeState(GameManager.instance.gmEvadeState);
        }

        player.stateMachine.currentState.LogicUpdate();
        boss.stateMachine.currentState.LogicUpdate();
        //enviroManager.stateMachine.currentState.LogicUpdate();
        gameUI.stateMachine.currentState.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        player.stateMachine.currentState.PhysicsUpdate();
        boss.stateMachine.currentState.PhysicsUpdate();
    }
}
