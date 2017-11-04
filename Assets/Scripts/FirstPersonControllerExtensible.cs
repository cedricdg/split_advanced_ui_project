// Copy of `FirstPersonController`, but fields' and methods' visibility changed from `private` to `protected`.
// Additionally extracted method for easier control.

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonControllerExtensible : MonoBehaviour
    {
        [SerializeField] protected bool m_IsWalking;
        [SerializeField] protected float m_WalkSpeed = 5;
        [SerializeField] protected float m_RunSpeed = 20;
        [SerializeField] [Range(0f, 1f)] protected float m_RunstepLenghten = 0.7f;
        [SerializeField] protected float m_JumpSpeed = 20;
        [SerializeField] protected float m_StickToGroundForce = 10;
        [SerializeField] protected float m_GravityMultiplier = 2;
        [SerializeField] protected MouseLook m_MouseLook;
        [SerializeField] protected bool m_UseFovKick;
        [SerializeField] protected FOVKick m_FovKick = new FOVKick();
        [SerializeField] protected bool m_UseHeadBob;
        [SerializeField] protected CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] protected LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] protected float m_StepInterval = 4;

        [SerializeField]
        protected AudioClip[] m_FootstepSounds; // an array of footstep sounds that will be randomly selected from.

        [SerializeField] protected AudioClip m_JumpSound; // the sound played when character leaves the ground.
        [SerializeField] protected AudioClip m_LandSound; // the sound played when character touches back on ground.

        protected Camera m_Camera;
        protected bool m_Jump;
        protected float m_YRotation;
        protected Vector2 m_Input;
        protected Vector3 m_MoveDir = Vector3.zero;
        protected CharacterController m_CharacterController;
        protected CollisionFlags m_CollisionFlags;
        protected bool m_PreviouslyGrounded;
        protected Vector3 m_OriginalCameraPosition;
        protected float m_StepCycle;
        protected float m_NextStep;
        protected bool m_Jumping;
        protected AudioSource m_AudioSource;

        // Use this for initialization
        protected void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
        }


        // Update is called once per frame
        protected void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        protected void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        protected void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        protected void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        protected void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude +
                                (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                               Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        protected void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        protected void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                        (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        // Copied from FirstPersonController but horizontal and vertical inputs are outsourced to `GetInputData`.
        protected void GetInput(out float speed)
        {
            // Read input
            InputData inputData = GetInputData();
            float horizontal = inputData.Horizontal;
            float vertical = inputData.Vertical;

            bool waswalking = m_IsWalking;
            m_IsWalking = inputData.IsWalking;

            speed = inputData.Speed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }

        // -----Code we added----------------------------------------------------------
        protected struct InputData
        {
            public float Horizontal;
            public float Vertical;
            public bool IsWalking;
            public float Speed;
        }

        protected virtual InputData GetInputData()
        {
            bool isWalking = false;

#if !MOBILE_INPUT // This was copied from `FirstPersonController`
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            isWalking = !Input.GetKey(KeyCode.LeftShift);
#endif

            return new InputData
            {
                Horizontal = CrossPlatformInputManager.GetAxis("Horizontal"),
                Vertical = CrossPlatformInputManager.GetAxis("Vertical"),
                IsWalking = isWalking,
                Speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed // This was copied from `FirstPersonController`
            };
        }
        // ----------------------------------------------------------------------------

        protected void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        protected void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}