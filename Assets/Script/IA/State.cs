namespace Script.IA
{
    public struct State
    {
        public float[] SensorValues;
        public float DistanceToParkingSlot;

        public State(float[] sensors, float distanceToParkingSlot)
        {
            SensorValues = sensors;
            DistanceToParkingSlot = distanceToParkingSlot;
        }
    }
}