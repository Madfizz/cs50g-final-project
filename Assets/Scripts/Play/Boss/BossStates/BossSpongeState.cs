﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpongeState : BaseState
{
    private Boss boss;

    public BossSpongeState(Boss _boss)
    {
        boss = _boss;
    }

    public override void Enter()
    {
       
    }

    public override void LogicUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            boss.stateMachine.ChangeState(boss.bossWeakState);
        }
    }

    public override void PhysicsUpdate()
    {
        boss.bossMovement.LookAtPlayer();
    }
}
