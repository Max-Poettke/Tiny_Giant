using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SocialPlatforms;
using Fusion.Addons.Physics;

[RequireComponent(typeof(NetworkRigidbody3D))]
[DefaultExecutionOrder(NetworkCharacterMovement.EXECUTION_ORDER)]
public class NetworkCharacterMovement : NetworkBehaviour
{
    public const int EXECUTION_ORDER = 100;
    public LocalCharacterMovement localCharacterMovement;
    [Networked]
    private Vector3 lookDirection{get;set;}

    ChangeDetector changeDetector;
    [HideInInspector]
    public NetworkRigidbody3D networkRigidbody3D;
    protected virtual void Awake()
    {
        networkRigidbody3D = GetComponent<NetworkRigidbody3D>();
    }

    // As we are in shared topology, having the StateAuthority means we are the local user
    public virtual bool IsLocalNetworkCharacterMovement => Object && Object.HasStateAuthority;

    public override void Spawned()
    {
        base.Spawned();
        if (IsLocalNetworkCharacterMovement)
        {
            localCharacterMovement = FindObjectOfType<LocalCharacterMovement>();
            if (localCharacterMovement == null) Debug.LogError("Missing LocalCharacterMovement in the scene");
        }
        changeDetector = GetChangeDetector(ChangeDetector.Source.SnapshotFrom);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        // Update the rig at each network tick for local player. The NetworkTransform will forward this to other players
        if (IsLocalNetworkCharacterMovement && localCharacterMovement)
        {
            PlayerState playerState = localCharacterMovement.PlayerState;
            ApplyLocalStateToRigParts(playerState);
        }
    }

    protected virtual void ApplyLocalStateToRigParts(PlayerState rigState)
    {
        transform.position = rigState.bodyPosition;
        transform.rotation = rigState.bodyRotation;
    }

    void UpdateCharacterMovementWithNetworkState()
    {
        PlayerState rigState = localCharacterMovement.PlayerState;
        rigState.bodyPosition = transform.position;
        rigState.bodyRotation = transform.rotation;
        rigState.lookDirection = lookDirection;
    }

    public override void Render()
    {
        base.Render();
        if (IsLocalNetworkCharacterMovement)
        {
            // Extrapolate for local user :
            // we want to have the visual at the good position as soon as possible, so we force the visuals to follow the most fresh hardware positions

            PlayerState rigState = localCharacterMovement.PlayerState;

            transform.position = rigState.bodyPosition;
            transform.rotation = rigState.bodyRotation;
        } else {
            foreach(var changedNetworkedVarName in changeDetector.DetectChanges(this))
            {
                if (changedNetworkedVarName == nameof(PlayerState))
                {
                    // Will be called when the local user change the hand pose structure
                    // We trigger here the actual animation update
                    UpdateCharacterMovementWithNetworkState();
                }
            }
        }
    }
}
