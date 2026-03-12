using UnityEngine;
using StarterAssets;

namespace MovementInputEvents
{
    //add all event types for possible playable character movement related events here

    //jumping related
    public struct PlayerJump{
        public GameObject PlayerIdentity;
    }

    public struct PlayerAirborne{
        public GameObject PlayerIdentity;
    }

    public struct PlayerLandOnGround{
        public GameObject PlayerIdentity;
    }

    public struct PlayerGrounded{
        public GameObject PlayerIdentity;
    }


    //moving related
    public struct PlayerMove{
        public GameObject PlayerIdentity;
    }

    public struct PlayerMoveForward{
        public GameObject PlayerIdentity;
    }

    public struct PlayerMoveStrafe{
        public GameObject PlayerIdentity;
    }

    public struct PlayerMoveBackward{
        public GameObject PlayerIdentity;
    }

    public struct PlayerStopMove{
        public GameObject PlayerIdentity;
    }


    //crouching related
    public struct PlayerStartCrouch{
        public GameObject PlayerIdentity;
    }

    public struct PlayerHoldCrouch{
        public GameObject PlayerIdentity;
    }

    public struct PlayerReleaseCrouch{
        public GameObject PlayerIdentity;
    }

    public struct PlayerNotCrouch{
        public GameObject PlayerIdentity;
    }
}
