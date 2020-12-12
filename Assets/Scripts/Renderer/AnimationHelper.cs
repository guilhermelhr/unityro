using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/**
 * Taken from RoSpriteData.cs at RoRebuild
 * 
 * 
 */
public static class AnimationHelper {


    public static bool IsFourDirectionAnimation(EntityType type, SpriteMotion motion) {

        if (type != EntityType.PC) {
            return false;
        }

        switch (motion) {
            case SpriteMotion.Idle:
            case SpriteMotion.Sit:
            case SpriteMotion.Walk:
                return false;
            default:
                return true;
        }
    }

    public static int GetFourDirectionSpriteIndexForAngle(Direction facing, float cameraRotation) {
        cameraRotation += 45f * (int)facing;
        if(cameraRotation > 360)
            cameraRotation -= 360;
        if(cameraRotation < 0)
            cameraRotation += 360;

        var index = Mathf.FloorToInt(cameraRotation / 45f);

        if(index > 7)
            index = 0;

        return Mathf.Clamp(index, 0, 7);
    }

    public static int GetSpriteIndexForAngle(Direction facing, float cameraRotation) {
        cameraRotation += 45f * (int)facing + (45f / 2f);
        if(cameraRotation > 360)
            cameraRotation -= 360;
        if(cameraRotation < 0)
            cameraRotation += 360;

        var index = Mathf.FloorToInt(cameraRotation / 45f);

        return Mathf.Clamp(index, 0, 7); ;
    }
}
