using System;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.CarMovement
{
    
    public class UserMovement : MonoBehaviour
    {
        public int speed = 2;

        public float rotationValue = 30;

        private Rigidbody rb;

        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        public Tuple<float, float> Move(Vector3 distance)
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");
            
            Vector3 input = new Vector3(verticalMovement, 0, 0);
            transform.Translate(input * Time.deltaTime * speed);
            if (verticalMovement != 0)
            {
                Quaternion deltaRotation = 
                    Quaternion.Euler(new Vector3(0, rotationValue * horizontalMovement, 0) * Time.deltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
            else
            {
                horizontalMovement = 0;
            }

            return Tuple.Create(verticalMovement, horizontalMovement);
        }

    }
}