using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assembly_CSharp
{
    public abstract class Controller : MonoBehaviour
    {
        #region Move

        public void Move(InputAction.CallbackContext context) => this.OnMove(context.ReadValue<Vector2>());
        protected virtual void OnMove(Vector2 direction) { }

        #endregion

        #region Look
        [Header("Look")]

        [SerializeField] public Transform target;
        [SerializeField] float sensitivity = 2.0f;
        [SerializeField] float maxAngleX = 60f;
        [SerializeField] float maxAngleY = 180f;
        [SerializeField] bool isClampedHorizontaly = false;
        Vector3 camRotation = Vector3.zero;
        
        public void Look(InputAction.CallbackContext context) => this.OnLook(context.ReadValue<Vector2>());
        protected virtual void OnLook(Vector2 direction) => this.RotateCamera(this.target, direction, this.sensitivity, Time.deltaTime);

        void RotateCamera(Transform target, Vector2 direction, float sensitivity, float elapsed)
        {
            this.camRotation += new Vector3(-direction.y, direction.x, 0) * sensitivity * elapsed;
            this.camRotation.x = Mathf.Clamp(this.camRotation.x, -this.maxAngleX, this.maxAngleX);

            if(this.isClampedHorizontaly)
                this.camRotation.y = Mathf.Clamp(this.camRotation.y, -this.maxAngleY, this.maxAngleY);

            target.rotation = Quaternion.Euler(this.camRotation);
        }

        #endregion
    }
}
