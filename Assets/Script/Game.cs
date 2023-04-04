using System;
using System.Collections;
using UnityEngine;

using Script.CarMovement;

namespace Script
{
    public class Game : MonoBehaviour
    {
        private Canvas canvas;
        private UserMovement car;
        private ParkingSlot parkingSlot;
        private void Awake()
        {
            car = GameObject.FindGameObjectWithTag("Car").GetComponent<UserMovement>();
            parkingSlot = GameObject.FindGameObjectWithTag("ParkingSlot").GetComponent<ParkingSlot>();
            canvas = GameObject.FindGameObjectWithTag("UI Display").GetComponent<Canvas>();
        }

        private void Update()
        {
            ArrayList listDistance = CheckObstacle();
            Vector3 carPosition = car.transform.position;
            Vector3 parkingPosition = parkingSlot.transform.position;
            carPosition.y = parkingPosition.y;
            Vector3 distanceToParkingSlot = carPosition - parkingPosition;
            
            Tuple<float, float> values = car.Move(distanceToParkingSlot);
            
            // canvas.TextChange(values.Item1, values.Item2, distanceToParkingSlot.magnitude);
        }

        private ArrayList CheckObstacle()
        {
            ArrayList distanceRayCast = new ArrayList();
            Vector3 carPosition = car.transform.position - new Vector3(0, 0.3f, 0);
            Vector3 directionRay = Quaternion.Euler(0, 90, 0) * car.transform.forward;
            Ray ray;
            RaycastHit hitData;
            float stepAngle = 360f / 8f;
            for (int i = 0; i < 8; i++)
            {
                directionRay = Quaternion.Euler(0, stepAngle * i, 0) * directionRay;
                ray = new Ray(carPosition, directionRay);
                Debug.DrawRay(ray.origin, ray.direction);
                if (Physics.Raycast(ray, out hitData))
                {
                    distanceRayCast.Add(hitData.distance < 8 ? distanceRayCast : -1);
                }
                else
                {
                    distanceRayCast.Add(-1);
                }
            }
            return distanceRayCast;
        }
        
    }
}