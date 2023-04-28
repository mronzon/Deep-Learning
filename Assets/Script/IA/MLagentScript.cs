using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class MLagentScript : Agent
{
    public int speed = 2;
    public float rotationValue = 30;
    public float timeToRest = 10;
    [SerializeField] private Transform targetTransform;

    private Rigidbody rb;
    private Vector3 originalPositon;
    private Quaternion originalRotation;
    private List<int> rewardTaken = new List<int>();
    private float timePassed;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        originalPositon = transform.position;
        originalRotation = transform.rotation;
        timePassed = 0f;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = originalPositon;
        transform.rotation = originalRotation;
        rewardTaken.Clear();
        timePassed = 0f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(targetTransform.position);
        sensor.AddObservation(transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        timePassed += Time.deltaTime;
        if (timePassed > timeToRest)
        {
            SetReward(-1f);
            EndEpisode();
            return;
        }
        float horizontalMovement = actions.ContinuousActions[0];
        float verticalMovement = actions.ContinuousActions[1];
        Vector3 input = new Vector3(verticalMovement, 0, 0);
        transform.Translate(input * Time.deltaTime * speed);
        if (verticalMovement != 0)
        {
            Quaternion deltaRotation = 
                Quaternion.Euler(new Vector3(0, rotationValue * horizontalMovement, 0) * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-5f);
            EndEpisode();
        }

        if (other.TryGetComponent<ParkingSlot>(out ParkingSlot parkingSlot))
        {
            Vector3 position = transform.position;
            Vector3 parkingSlotPosition = parkingSlot.transform.position;
            position.y = parkingSlotPosition.y;
            Vector3 distance = position - parkingSlotPosition;
            if (distance.magnitude < 3)
            {
                SetReward(5f);
                Debug.Log("Arriver !");
                EndEpisode();
            }
        }

        if (other.TryGetComponent<Reward>(out Reward reward))
        {
            if (!rewardTaken.Contains(reward.id))
            {
                SetReward(1f);
                rewardTaken.Add(reward.id);
            }
        }
    }
}
