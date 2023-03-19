using System;
using TMPro;
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
            Vector3 distanceToParkingSlot = car.transform.position - parkingSlot.transform.position;
            
            Tuple<float, float> values = car.Move(distanceToParkingSlot);
            
            canvas.TextChange(values.Item1, values.Item2, distanceToParkingSlot.magnitude);
        }
        
    }
}