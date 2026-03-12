using UnityEngine;

public static class MovementStyles
{
    public enum MovementStyle
    {
        AlwaysFaceForward,      // character armature faces the direction the player looking at
        FaceMovement,           // character armature turns to face the direction player is trying to move
        RotateInsteadOfStrafe,  //character armature rotates when trying to move left or right
    }
}
