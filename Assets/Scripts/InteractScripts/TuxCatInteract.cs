using Interfaces;
using UnityEngine;

public class TuxCatInteract : MonoBehaviour, Interactable
{
    [SerializeField]
    private AudioSource source;
    

    /// <inheritdoc/>
    public void OnClick()
    {
        if (this.source == null)
            return;

        this.source.Play();
    }
}
