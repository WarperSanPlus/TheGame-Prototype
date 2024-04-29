using Assembly_CSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Controller
{
    [Header("Move")]
    [SerializeField] float movementSpeed = 20;
    CharacterController cc;
    Vector3 move = Vector3.zero;
    Animator animator;

    void Start()
    {
        this.cc = this.GetComponent<CharacterController>();
        this.animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        this.OnMove(this.move);

        if (this.target != null)
        {
            this.transform.rotation = Quaternion.Euler(0, this.target.eulerAngles.y, 0);
        }
    }

    protected override void OnMove(Vector2 direction)
    {
        this.move = direction;
        Vector3 moveDirection = this.target.transform.forward * direction.y + this.target.transform.right * direction.x;

        if (moveDirection.magnitude > 0)
        {
            moveDirection.y = 0;
            moveDirection = moveDirection.normalized;
            this.animator.SetBool("Walking", true);
        }
        else
        {
            this.animator.SetBool("Walking", false);
        }
        this.cc.Move((moveDirection * this.movementSpeed + Vector3.down) * Time.deltaTime);
    }
}
