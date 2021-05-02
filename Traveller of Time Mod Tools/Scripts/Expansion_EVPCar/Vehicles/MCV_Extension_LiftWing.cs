using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace EVP.DestinyEngine
{
    public class MCV_Extension_LiftWing : MonoBehaviour
    {

        private MCV_BodyParts bodyPart;
        private Rigidbody m_Rigidbody;

        [SerializeField] private float m_AerodynamicEffect = 0.02f;   // How much aerodynamics affect the speed of the aeroplane.
        [SerializeField] private float m_DragIncreaseFactor = 0.001f; // how much drag should increase with speed.
        [SerializeField] private float m_AirBrakesEffect = 3f;        // How much the air brakes effect the drag.
        [SerializeField] private float m_RollEffect = 1f;             // The strength of effect for roll input.
        [SerializeField] private float m_PitchEffect = 1f;            // The strength of effect for pitch input.
        [SerializeField] private float m_YawEffect = 0.2f;            // The strength of effect for yaw input.
        [SerializeField] private float m_BankedTurnEffect = 0.5f;     // The amount of turn from doing a banked turn.

        public float RollInput { get; private set; }
        public float PitchInput { get; private set; }
        public float YawInput { get; private set; }
        public float ThrottleInput { get; private set; }
        public bool AirBrakes { get; private set; }                     // Whether or not the air brakes are being applied.
        public float ForwardSpeed { get; private set; }

        private float m_OriginalDrag;         // The drag when the scene starts.
        private float m_OriginalAngularDrag;  // The angular drag when the scene starts.
        private float m_BankedTurnAmount;
        private float m_AeroFactor;
        private Player player;

        private void Awake()
        {
            bodyPart = GetComponent<MCV_BodyParts>();
        }

        private void Start()
        {
            player = ReInput.players.GetPlayer(0);
            m_Rigidbody = bodyPart.ModularCustomVehicle.vehicleRigidbody;
        }

        private void FixedUpdate()
        {
            if (bodyPart.ModularCustomVehicle == null)
            {
                return;
            }

            CalculateForwardSpeed();
            CalculateDrag();
            CaluclateAerodynamicEffect();
            CalculateTorque();
        }

        private void HandleInput()
        {
            float roll = player.GetAxis("HorizontalAlt");
            float pitch = player.GetAxis("VerticalAlt");
            bool airBrakes = player.GetButton("Fire1");

            RollInput = roll;
            PitchInput = pitch;
            //YawInput = yawInput;
        }

        private void CalculateTorque()
        {
            // We accumulate torque forces into this variable:
            var torque = Vector3.zero;
            // Add torque for the pitch based on the pitch input.
            torque += PitchInput * m_PitchEffect * bodyPart.ModularCustomVehicle.transform.right;
            // Add torque for the yaw based on the yaw input.
            torque += YawInput * m_YawEffect * bodyPart.ModularCustomVehicle.transform.up;
            // Add torque for the roll based on the roll input.
            torque += -RollInput * m_RollEffect * bodyPart.ModularCustomVehicle.transform.forward;
            // Add torque for banked turning.
            torque += m_BankedTurnAmount * m_BankedTurnEffect * bodyPart.ModularCustomVehicle.transform.up;
            // The total torque is multiplied by the forward speed, so the controls have more effect at high speed,
            // and little effect at low speed, or when not moving in the direction of the nose of the plane
            // (i.e. falling while stalled)
            m_Rigidbody.AddTorque(torque * ForwardSpeed * m_AeroFactor);
        }


        private void CalculateDrag()
        {
            // increase the drag based on speed, since a constant drag doesn't seem "Real" (tm) enough
            float extraDrag = m_Rigidbody.velocity.magnitude * m_DragIncreaseFactor;
            // Air brakes work by directly modifying drag. This part is actually pretty realistic!
            m_Rigidbody.drag = (AirBrakes ? (m_OriginalDrag + extraDrag) * m_AirBrakesEffect : m_OriginalDrag + extraDrag);
            // Forward speed affects angular drag - at high forward speed, it's much harder for the plane to spin
            m_Rigidbody.angularDrag = m_OriginalAngularDrag * ForwardSpeed;
        }


        private void CalculateForwardSpeed()
        {
            // Forward speed is the speed in the planes's forward direction (not the same as its velocity, eg if falling in a stall)
            var localVelocity = bodyPart.ModularCustomVehicle.transform.InverseTransformDirection(m_Rigidbody.velocity);
            ForwardSpeed = Mathf.Max(0, localVelocity.z);
        }



        private void CaluclateAerodynamicEffect()
        {
            // "Aerodynamic" calculations. This is a very simple approximation of the effect that a plane
            // will naturally try to align itself in the direction that it's facing when moving at speed.
            // Without this, the plane would behave a bit like the asteroids spaceship!
            if (m_Rigidbody.velocity.magnitude > 0)
            {
                // compare the direction we're pointing with the direction we're moving:
                m_AeroFactor = Vector3.Dot(bodyPart.ModularCustomVehicle.transform.forward, m_Rigidbody.velocity.normalized);
                // multipled by itself results in a desirable rolloff curve of the effect
                m_AeroFactor *= m_AeroFactor;
                // Finally we calculate a new velocity by bending the current velocity direction towards
                // the the direction the plane is facing, by an amount based on this aeroFactor
                var newVelocity = Vector3.Lerp(m_Rigidbody.velocity, bodyPart.ModularCustomVehicle.transform.forward * ForwardSpeed,
                                               m_AeroFactor * ForwardSpeed * m_AerodynamicEffect * Time.deltaTime);
                m_Rigidbody.velocity = newVelocity;

                // also rotate the plane towards the direction of movement - this should be a very small effect, but means the plane ends up
                // pointing downwards in a stall
                m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation,
                                                      Quaternion.LookRotation(m_Rigidbody.velocity, bodyPart.ModularCustomVehicle.transform.up),
                                                      m_AerodynamicEffect * Time.deltaTime);
            }
        }

    }
}
