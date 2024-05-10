using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioObject : MonoBehaviour 
{
    private AudioSource source;

    public void SetClip(AudioClip clip)
    {
        if (!this.CheckSource())
            return;

        this.source.clip = clip;
        this.source.Play();
    }

    private bool CheckSource()
    {
        if (this.source != null)
            return true;

        this.source = this.GetComponent<AudioSource>();
        return this.source != null;
    }
}