using UnityEngine;
using HeroShooterMaker.StatusEffects;

namespace HeroShooterMakerDemo
{
    public class Knockback : StatusEffect
    {
        [Tooltip("speed applied to displacement")]
        public float Velocity;
        [Tooltip("types of direction for displacement")]
        public DirectionType DirectionSetting;
        public enum DirectionType
        {
            OppositeFromMiddle,
            ApplyExtraVector,
        }
        [Tooltip("Applies manual vector. only relevant for direction setting ApplyExtraVector. x value determines horizontal movement and y value determines vertical movement")]
        public Vector2 ExtraVector;
        public float DecayVelocityRate = 0;
        [HideInInspector] public Vector3 sourcePosition;
        Vector3 direction;

        public override void ApplyEffect()
        {
            Active = true;
            RemainingDuration = Stats.EffectDuration;

            direction = AffectedPlayer.PlayerArmature.transform.position - sourcePosition;

            switch (DirectionSetting)
            {
                case DirectionType.OppositeFromMiddle:
                    break;
                case DirectionType.ApplyExtraVector:
                    direction = direction.normalized * ExtraVector.x;
                    direction.y = ExtraVector.y;
                    break;
                default:
                    break;
            }
            AffectedPlayer.PlayerMovement.ResetCharacterVelocity();
            AffectedPlayer.PlayerMovement.ApplyExternalForce(direction.normalized, Velocity);

        }

        protected override void RemoveEffect()
        {
            Active = false;
            AffectedPlayer.PlayerMovement.ApplyExternalForce(Vector3.zero, 0);
        }

        void Update()
        {
            float updatedVelocity = Velocity - (DecayVelocityRate * (Stats.EffectDuration - RemainingDuration));
            AffectedPlayer.PlayerMovement.ApplyExternalForce(direction.normalized, updatedVelocity);
        }
    }

}
