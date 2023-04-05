using System;
using UnityEngine;
using Action = Script.IA.Action;

namespace Script.CarMovement
{
    public class IAMovement : MonoBehaviour
    {
        public int speed = 2;

        public float rotationValue = 30;

        private Rigidbody rb;

        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        public void Move(Action action)
        {
            float horizontalMovement = (float) action.Speed;
            float verticalMovement = (float) action.TurningDegree;
            
            Vector3 input = new Vector3(verticalMovement, 0, 0);
            transform.Translate(input * Time.deltaTime * speed);
            if (verticalMovement != 0)
            {
                Quaternion deltaRotation = 
                    Quaternion.Euler(new Vector3(0, rotationValue * horizontalMovement, 0) * Time.deltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
        }

        public void Move(Action action, float time)
        {
            float horizontalMovement = action.Speed;
            float verticalMovement = action.TurningDegree;
            
            Vector3 input = new Vector3(verticalMovement, 0, 0);
            transform.Translate(input * time * speed);
            if (verticalMovement != 0)
            {
                Quaternion deltaRotation = 
                    Quaternion.Euler(new Vector3(0, rotationValue * horizontalMovement, 0) * time);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
        }
    }
}