﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectileGrouping;

    public bool readyToLaunch;
    public bool hasDoneBigAttack;

    private int projectilesToPool;

    private Player player;
    private List<GameObject> projectiles;

    private Vector3 launchDir;

    private void Start()
    {
        player = GetComponent<Player>();

        // Projectile data setup
        BasicProjectile projData = projectile.GetComponent<BasicProjectile>();
        projData.speed = player.playerData.gravFireSpeed;
        projData.destroyAfter = player.playerData.gravFireDestroyAfter;

        // Object pooling setup
        projectiles = new List<GameObject>();
        projectilesToPool = player.playerData.gravProjectiles;
        projectiles = ObjectPooler.CreateObjectPool(projectilesToPool, projectile);
        projectiles = ObjectPooler.AssignParentGrouping(projectiles, projectileGrouping);

        readyToLaunch = false;
        hasDoneBigAttack = false;
    }

    #region SpongeState Attack
    // Fire single projectile in opposite direction to current surface
    public void FireProjectile()
    {
        Vector3 fireDir = player.playerMovement.GetPlayerFireDirection();

        GameObject projectile = ObjectPooler.GetPooledObject(projectiles);

        // If player has projectiles available to fire
        if (projectile != null)
        {
            projectile.transform.position = transform.position;

            // Determine fire direction
            if (fireDir == Vector3.right || fireDir == Vector3.left)
            {
                int rotAngle = fireDir == Vector3.right ? 90 : -90;

                projectile.transform.rotation = Quaternion.Euler(transform.rotation.x, rotAngle, transform.rotation.z);
                player.playerSFX.playerAudioSource.PlayOneShot(player.playerSFX.playerAudio[3]);
                projectile.GetComponent<BasicProjectile>().dir = Vector3.forward;
            }
            else
            {
                projectile.transform.rotation = transform.rotation;
                player.playerSFX.playerAudioSource.PlayOneShot(player.playerSFX.playerAudio[3]);
                projectile.GetComponent<BasicProjectile>().dir = fireDir;
            }
        
            projectile.SetActive(true);
        }
    }
    #endregion

    #region AttackState Attack
    // When on a launchpad in AttackState, launch
    public void Launch()
    {
        launchDir = player.playerMovement.GetPlayerFireDirection();
        player.playerSFX.playerAudioSource.PlayOneShot(player.playerSFX.playerAudio[2]);
        player.playerRb.AddForce(launchDir * player.playerData.launchSpeed, ForceMode.Impulse);
    }

    public void BigAttack()
    {
        StartCoroutine(BigAttackCoroutine());
    }

    IEnumerator BigAttackCoroutine()
    {
        yield return new WaitForSeconds(2);
        // Launch player in opposite direction
        player.playerRb.AddForce(-launchDir * player.playerData.launchSpeed, ForceMode.Impulse);
        player.playerSFX.playerAudioSource.PlayOneShot(player.playerSFX.playerAudio[2]);
        yield return new WaitForSeconds(0.60f);
        player.playerSFX.playerAudioSource.PlayOneShot(player.playerSFX.playerAudio[1]);
        yield return new WaitForSeconds(1.4f);
        // Signal to GameManager to initiate state change out of AttackState
        hasDoneBigAttack = true;
    }

    // When Player enters a launch trigger, flag it
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LaunchpadTrigger"))
        {
            // Check if in AttackState and not GravState,
            // Flag that player is ready to launch over to Boss to do Big Attack
            if (player.stateMachine.currentState == player.playerAttackState)
            {
                readyToLaunch = true;
            }
        }

        if (other.CompareTag("BossWeakHitbox"))
        {
            BigAttack();
        }
    }

    // When Player exits a launch trigger, unflag it
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LaunchpadTrigger"))
        {
            if (player.stateMachine.currentState == player.playerAttackState)
            {
                readyToLaunch = false;
            }
        }
    }
    #endregion
}
