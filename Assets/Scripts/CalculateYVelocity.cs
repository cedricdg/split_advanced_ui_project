using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
namespace LeapmotionProject
{

    public class CalculateYVelocity : MonoBehaviour
    {

        public float HandRotationOrigin = 90;
        public float HandRotationThreshhold = 20;

        public float MaxSpeed = 5f;
        public float MinSpeed = 1f;

        public float yVel;
        public int LastHandVelocityRange;
        public float BreakForceWrongAngle = 0.08f;
        public float BreakForceValidAngle = 0.02f;

        public void Update()
        {
            if (IsValidAngle())
			{
				var body = this.GetComponent<Rigidbody>();
                if(yVel < body.velocity.y){
                    yVel = Mathf.Abs(body.velocity.y);
				}
            }
		}

        public void FixedUpdate()
		{
			Debug.Log("yvel: " + yVel);

            if (IsValidAngle())
            {
                yVel -= BreakForceValidAngle;
			}
			else
			{
				yVel -= BreakForceWrongAngle;
            }


			if (yVel < 0f)
			{
				yVel = 0f;
            } else if(yVel > MaxSpeed + MinSpeed){
                yVel = MaxSpeed + MinSpeed;
            }
        }

        private bool IsValidAngle()
		{
			return transform.eulerAngles.z < HandRotationOrigin + HandRotationThreshhold
						   && transform.eulerAngles.z > HandRotationOrigin - HandRotationThreshhold;
		}

        public float GetVerticalInput()
        {
            if (yVel > 0.01f){
				float yAbs = yVel;

                if (yAbs > MinSpeed)
                {
                    float result = yAbs / MaxSpeed;
                    return Mathf.Min(result, 1f) * Mathf.Min(result, 1f);
                }
            }
            return -1;
        }
    }

}