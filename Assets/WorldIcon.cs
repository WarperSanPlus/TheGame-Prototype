using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class WorldIcon : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float minDistance = 1;

    [SerializeField]
    private float maxDistance = 10;

    private CanvasGroup group;

    // Start is called before the first frame update
    void Start()
    {
        this.target = Camera.main.transform;
        this.group = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.target == null)
            return;

        // Rotate towards camera
        this.transform.LookAt(this.target);

        var alpha = 1f;
        var distance = Vector3.Distance(this.transform.position, this.target.transform.position);
        
        if (distance < this.minDistance || distance > this.maxDistance)
            alpha = distance / this.minDistance;

        this.group.alpha = alpha;
    }
}
