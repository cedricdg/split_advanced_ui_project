using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
namespace LeapmotionProject
{

    public class PrintVelocity : MonoBehaviour, IInputAxisProvider
    {

        public float HandRotationOrigin = 90;
        public float HandRotationThreshhold = 20;

        public float MaxSpeed = 5f;
        public float MinSpeed = 1f;

        public readonly List<Vector3> LastHandVelocities = new List<Vector3>();
        public int LastHandVelocityRange;

        public void Update()
        {
            if (IsValidAngle())
			{
				var body = this.GetComponent<Rigidbody>();
                LastHandVelocities.Add(body.velocity);
            }
        }

        public float GetVerticalInput()
        {
            if(LastHandVelocities.Count > 0){
                var averageVel = new Vector3(
                    LastHandVelocities.Average(v => Mathf.Abs(v.x)),
                    LastHandVelocities.Average(v => Mathf.Abs(v.y)),
                    LastHandVelocities.Average(v => Mathf.Abs(v.z)));
                Debug.Log("Size: " + LastHandVelocities.Count);
                LastHandVelocities.Clear();
                Debug.Log("velz " + averageVel.y);
                float yAbs = Mathf.Abs(averageVel.y);
                if (yAbs > MinSpeed)
                {
                    float result = yAbs / MaxSpeed;
                    Debug.Log(result + " = " + yAbs + " / " + MaxSpeed);
                    return Mathf.Min(result, 1f) * Mathf.Min(result, 1f);
				}
			}
            return -1;
        }

        private bool IsValidAngle()
        {
            return transform.eulerAngles.z < HandRotationOrigin + HandRotationThreshhold
                           && transform.eulerAngles.z > HandRotationOrigin - HandRotationThreshhold;
        }
    }

}