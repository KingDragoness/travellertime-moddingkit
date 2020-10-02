using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using KinematicCharacterController;
using DestinyEngine;

namespace TestingTesting
{
    public class PlatformTest : Interactables, IMoverController
    {
        public PhysicsMover Mover;
        public PlayableDirector Director;
        public float Speed = 1f;

        private Transform _transform;

        [Header("Storage Variables")]
        public double m_DirectorTime;

        private void Start()
        {
            _transform = this.transform;

            Mover.MoverController = this;
        }

        public override void CommandExecute(ActionCommand command)
        {
            throw new System.NotImplementedException();
        }

        void Update_State()
        {
            Director.time = m_DirectorTime;
        }

        public override void LoadState()
        {
            string DirectorTime = Get_Variable("m_DirectorTime");

            if (double.TryParse(DirectorTime, out m_DirectorTime))
            {
                double value = double.Parse(DirectorTime);
                m_DirectorTime = value;
            }

            Update_State();
        }

        public override void SaveState()
        {
            Save_Variable("m_DirectorTime", Director.time.ToString());
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            Vector3 _positionBeforeAnim = _transform.position;
            Quaternion _rotationBeforeAnim = _transform.rotation;

            EvaluateAtTime(Time.time * Speed);

            goalPosition = _transform.position;
            goalRotation = _transform.rotation;

            _transform.position = _positionBeforeAnim;
            _transform.rotation = _rotationBeforeAnim;
        }

        public void EvaluateAtTime(double time)
        {
            Director.time = time % Director.duration;
            Director.Evaluate();
        }

        public override void ExecuteFunction(string functionName)
        {
            throw new System.NotImplementedException();
        }
    }
}
