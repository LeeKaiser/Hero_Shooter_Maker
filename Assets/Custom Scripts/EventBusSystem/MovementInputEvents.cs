using UnityEngine;
using StarterAssets;

namespace MovementInputEvents
{
    //add all event types for possible playable character movement related events here

    //jumping related
    public struct PlayerJump{
        public CharCore playerIdentity;
    }

    public struct PlayerLandOnGround{
        public CharCore playerIdentity;
        public bool grounded;
    }

    public struct PlayerGrounded{
        public CharCore playerIdentity;
    }


    //moving related
    public struct PlayerMove{
        public CharCore playerIdentity;
    }

    public struct PlayerMoveForward{
        public CharCore playerIdentity;
    }

    public struct PlayerMoveStrafe{
        public CharCore playerIdentity;
    }

    public struct PlayerMoveBackward{
        public CharCore playerIdentity;
    }

    public struct PlayerStopMove{
        public CharCore playerIdentity;
    }


    //crouching related
    public struct PlayerStartCrouch{
        public CharCore playerIdentity;
    }

    public struct PlayerHoldCrouch{
        public CharCore playerIdentity;
    }

    public struct PlayerReleaseCrouch{
        public CharCore playerIdentity;
    }

    public struct PlayerNotCrouch{
        public CharCore playerIdentity;
    }
}
