using System;

namespace Script.IA
{
    public struct Action
    {
        public float Speed;
        public float TurningDegree;
        public Action(float speed, float turningDegree)
        {
            Speed = speed;
            TurningDegree = turningDegree;
        }

        public static Action RandomAction(int numActions, Random randomSeed)
        {
            int range = numActions / 2;
            float speed = -1f +  0.1f * randomSeed.Next(range);
            float turningDegree = -1f +  0.1f * randomSeed.Next(range);
            return new Action(speed:speed, turningDegree:turningDegree);
        }

    }
    
}