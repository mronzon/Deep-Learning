using System;
using System.Collections;
using UnityEngine;

using Script.CarMovement;
using Script.IA;

namespace Script
{
    public class Game : MonoBehaviour
    {
        private Canvas canvas;
        private UserMovement car;
        private IAMovement _IaCar;
        private ParkingSlot parkingSlot;
        private QLearningAgent _agent;
        private void Awake()
        {
            car = GameObject.FindGameObjectWithTag("Car").GetComponent<UserMovement>();
            _IaCar = GameObject.FindGameObjectWithTag("Car").GetComponent<IAMovement>();
            parkingSlot = GameObject.FindGameObjectWithTag("ParkingSlot").GetComponent<ParkingSlot>();
            canvas = GameObject.FindGameObjectWithTag("UI Display").GetComponent<Canvas>();
            _agent = new QLearningAgent(81 * 8 + 500, 40, 0.1, 0.1, 0);
        }

        private void Update()
        {
            float[] sensorsValue = CheckObstacle();
            Vector3 carPosition = car.transform.position;
            Vector3 parkingPosition = parkingSlot.transform.position;
            carPosition.y = parkingPosition.y;
            Vector3 distanceToParkingSlot = carPosition - parkingPosition;
            
            
            
        }

        private float[] CheckObstacle()
        {
            float[] distanceRayCast = new float[8];
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
                    distanceRayCast[i] = hitData.distance < 8f ? hitData.distance : -1f;
                }
                else
                {
                    distanceRayCast[i] = -1f;
                }
            }
            return distanceRayCast;
        }
        
    }
}