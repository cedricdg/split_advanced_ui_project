using System;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace LeapmotionProject
{
    public class FirstPersonHandsController : FirstPersonControllerExtensible
    {
        public string TagWithCalculateYVelocity = "Handpalm";
        public float RunningThreshold = 0.5f;
        public float RunningSpeed = 20;
        
        protected override InputData GetInputData()
        {
            InputData inputData = base.GetInputData();
            var handsInput = GetHandVelocity();
            if (handsInput > 0.01f)
            {
                inputData.Vertical = handsInput;
                inputData.IsWalking = handsInput < RunningThreshold;
                inputData.Speed = handsInput * RunningSpeed;
            }
            return inputData;
        }

        protected float GetHandVelocity()
        {
            var hands = GameObject.FindGameObjectsWithTag(TagWithCalculateYVelocity);
            const int numberOfHands = 2;
            var result = hands.Select(hand => hand.GetComponent<CalculateYVelocity>().GetVerticalInput())
                .Where(input => input > 0f)
                .Sum() / numberOfHands;
            return Math.Min(1f, result);
        }
    }
}