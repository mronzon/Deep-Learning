using System;
using UnityEngine;

using Script.CarMovement;
using Script.IA;

namespace Script
{
    public class Game : MonoBehaviour
    {
        public bool IAControl = false;
        public String pathToSaveFile;
        private Canvas canvas;
        private UserMovement car;
        private IAMovement _IaCar;
        private ParkingSlot parkingSlot;
        private QLearningAgent _agent;
        private Vector3 _carPositionStart;
        private Quaternion _carRotation;
        public float timeToReset = 10f;
        private float _timePassed = 0f;
        private bool _educateTheIA = true;
        private void Awake()
        {
            car = GameObject.FindGameObjectWithTag("Car").GetComponent<UserMovement>();
            _IaCar = GameObject.FindGameObjectWithTag("Car").GetComponent<IAMovement>();
            parkingSlot = GameObject.FindGameObjectWithTag("ParkingSlot").GetComponent<ParkingSlot>();
            canvas = GameObject.FindGameObjectWithTag("UI Display").GetComponent<Canvas>();
            _agent = new QLearningAgent(42, 0.8, 0.5, 0);
            Transform transformCar = car.transform;
            _carPositionStart = transformCar.position;
            _carRotation = transformCar.rotation;
        }

        private void Update()
        {
            float[] sensorsValue = CheckObstacle();
            Vector3 distanceToParkingSlot = GetDistanceToParking();
            if (IAControl)
            {
                if (_educateTheIA)
                {
                    EducateIA();
                }
                _timePassed += Time.deltaTime;
                if (_timePassed > timeToReset)
                {
                    _timePassed = 0;
                    ResetCar();
                    _educateTheIA = true;
                }
                State state = new State(sensors: sensorsValue, distanceToParkingSlot: MathF.Round(distanceToParkingSlot.magnitude, 2));
                Script.IA.Action action = _agent.ChooseAction(state);
                _IaCar.Move(action);
                distanceToParkingSlot = GetDistanceToParking();
                sensorsValue = CheckObstacle();
                State newState = new State(sensors: sensorsValue, distanceToParkingSlot: MathF.Round(distanceToParkingSlot.magnitude, 2));
                if (distanceToParkingSlot.magnitude > 50)
                {
                    ResetCar();
                    
                }
                _agent.UpdateQ(state, action, GetReward(distanceToParkingSlot.magnitude), newState);
            }
            else
            {
                car.Move();
            }
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
                    distanceRayCast[i] = hitData.distance < 8f ? MathF.Round(hitData.distance, 2) : -1f;
                }
                else
                {
                    distanceRayCast[i] = -1f;
                }
            }
            return distanceRayCast;
        }

        private Vector3 GetDistanceToParking()
        {
            Vector3 carPosition = car.transform.position;
            Vector3 parkingPosition = parkingSlot.transform.position;
            carPosition.y = parkingPosition.y;
            return carPosition - parkingPosition;
        }

        private void EducateIA()
        {
            float[] sensorsValue ;
            Vector3 distanceToParkingSlot;
            float time = 1f / 60f;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    sensorsValue = CheckObstacle();
                    distanceToParkingSlot = GetDistanceToParking();
                    State state = new State(sensors: sensorsValue, distanceToParkingSlot: MathF.Round(distanceToParkingSlot.magnitude, 2));
                    Script.IA.Action action = _agent.ChooseAction(state);
                    _IaCar.Move(action, time);
                    distanceToParkingSlot = GetDistanceToParking();
                    sensorsValue = CheckObstacle();
                    State newState = new State(sensors: sensorsValue, distanceToParkingSlot: MathF.Round(distanceToParkingSlot.magnitude, 2));
                    if (distanceToParkingSlot.magnitude > 50)
                    {
                        ResetCar();
                    }
                    _agent.UpdateQ(state, action, GetReward(distanceToParkingSlot.magnitude), newState);
                }
                ResetCar();
            }
            _agent.SaveModel(pathToSaveFile);
            _educateTheIA = false;

        }
        
        private float GetReward(float distance)
        {
            if (distance > 50)
            {
                return 0;
            }

            if (distance > 5)
            {
                return -1.1f * distance + 55.5f;
            }

            return -10 * distance + 50;
        }

        private void ResetCar()
        {
            car.transform.position = _carPositionStart;
            car.transform.rotation = _carRotation;
        }
    }
}