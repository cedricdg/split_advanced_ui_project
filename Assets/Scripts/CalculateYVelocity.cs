using UnityEngine;

public class CalculateYVelocity : MonoBehaviour
{
    public float HandRotationOrigin = 90;
    public float HandRotationThreshhold = 20;

    public float MaxSpeed = 5f;
    public float MinSpeed = 1f;

    public float BreakForceWrongAngle = 0.08f;
    public float BreakForceValidAngle = 0.02f;
    
    public float YVelocity;

    public bool IsHandValidAngle;

    public void Update()
    {
        if (IsValidAngle())
        {
            var body = GetComponent<Rigidbody>();
            if (YVelocity < body.velocity.y)
            {
                YVelocity = Mathf.Abs(body.velocity.y);
            }
            IsHandValidAngle = true;
        } else {
            IsHandValidAngle = false;
        }
    }

    public void FixedUpdate()
    {
        if (IsValidAngle())
        {
            YVelocity -= BreakForceValidAngle;
        }
        else
        {
            YVelocity -= BreakForceWrongAngle;
        }


        if (YVelocity < 0f)
        {
            YVelocity = 0f;
        }
        else if (YVelocity > MaxSpeed + MinSpeed)
        {
            YVelocity = MaxSpeed + MinSpeed;
        }
    }

    private bool IsValidAngle()
    {
        return transform.eulerAngles.z < HandRotationOrigin + HandRotationThreshhold
               && transform.eulerAngles.z > HandRotationOrigin - HandRotationThreshhold;
    }

    public float GetVerticalInput()
    {
        if (YVelocity > 0.01f)
        {
            float yAbs = YVelocity;

            if (yAbs > MinSpeed)
            {
                float result = yAbs / MaxSpeed;
                return Mathf.Min(result, 1f) * Mathf.Min(result, 1f);
            }
        }
        return -1;
    }
}