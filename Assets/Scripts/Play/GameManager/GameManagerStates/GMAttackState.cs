﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMAttackState : BaseState
{
    private Player player;
    private Boss boss;
    private EnvironmentManager enviroManager;
    private GameUI gameUI;

    private float time;

    public GMAttackState(Player _player, Boss _boss, EnvironmentManager _enviroManager, GameUI _gameUI)
    {
        player = _player;
        boss = _boss;
        enviroManager = _enviroManager;
        gameUI = _gameUI;
    }

    public override void Enter()
    {
        player.stateMachine.ChangeState(player.playerAttackState);
        boss.stateMachine.ChangeState(boss.bossWeakState);
        enviroManager.stateMachine.ChangeState(enviroManager.enviroLaunchState);
        gameUI.stateMachine.ChangeState(gameUI.gameUIPlayState);
        // Keep track of time passed in this state
        time = 0;
    }

    public override void LogicUpdate()
    {
        // Player launching to attack Boss
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player.playerAttack.readyToLaunch)
            {
                enviroManager.launchpadManager.Launch();
                player.playerAttack.Launch();
            }
        }

        // If Boss dies, change to game end state
        if (boss.bossData.health <= 0)
        {
            GameManager.instance.gmStateMachine.ChangeState(GameManager.instance.gmEndState);
        }

        if (player.playerAttack.hasDoneBigAttack)
        {
            GameManager.instance.gmStateMachine.ChangeState(GameManager.instance.gmEvadeState);
        }

        // If failed to attack Boss in time, change back to Evade State
        if (time > GameManager.instance.gameData.attackStateFailedTime)
        {
            GameManager.instance.gmStateMachine.ChangeState(GameManager.instance.gmEvadeState);
        }

        time += Time.deltaTime;

        player.stateMachine.currentState.LogicUpdate();
        boss.stateMachine.currentState.LogicUpdate();
        enviroManager.stateMachine.currentState.LogicUpdate();
        gameUI.stateMachine.currentState.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        player.stateMachine.currentState.PhysicsUpdate();
        boss.stateMachine.currentState.PhysicsUpdate();
        enviroManager.stateMachine.currentState.PhysicsUpdate();
    }

    public override void Exit()
    {
        
    }
}
