using System;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace LeapmotionProject
{
    public class FirstPersonHandsController : FirstPersonControllerExtensible
    {
        public string TagWithCalculateYVelocity = "Handpalm";
        public float RunningThreshold = 0.5f;
        
        protected override InputData GetInputData()
        {
            InputData inputData = base.GetInputData();
            var handsInput = GetInputFromLeapmotion();
            if (handsInput > 0.01f)
            {
                inputData.Vertical = handsInput;
                inputData.IsWalking = handsInput < RunningThreshold;
                inputData.Speed = (1 - handsInput) * m_WalkSpeed + handsInput * m_RunSpeed;
            }
            return inputData;
        }

        protected float GetInputFromLeapmotion()
        {
            var hands = GameObject.FindGameObjectsWithTag(TagWithCalculateYVelocity);
            var result = hands.Select(hand => hand.GetComponent<CalculateYVelocity>().GetVerticalInput())
                .Where(input => input > 0f)
                .Sum();
            return Math.Min(1f, result);
        }
    }
}