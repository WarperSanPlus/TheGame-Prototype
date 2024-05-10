using Controllers;
using UnityEngine;

/// <summary>
/// Respawns the player when they collide with it
/// </summary>
[RequireComponent(typeof(Collider))]
public class DeathPlane : MonoBehaviour
{
    /// <inheritdoc/>
    private void OnTriggerEnter(Collider other) 
    {
        if (!other.gameObject.TryGetComponent(out PlayerController player))
            return;

        player.Respawn();
    }
}
