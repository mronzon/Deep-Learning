using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class Canvas : MonoBehaviour
    {
        public Text _textDisplay;

        private void Awake()
        {
            Debug.Log(_textDisplay);
        }

        public void TextChange(float acceleration, float turningDegree, float distance)
        {
            Debug.Log("Acceleration : " + acceleration + "\nDistance : " + distance + "\nTurning Degree : " +
                turningDegree);
        }
    }
}