namespace UnityRO.Core.Sprite {
    public class AnimationHelper {
        public static bool IsLoopingMotion(SpriteMotion motion) {
            switch (motion) {
                case SpriteMotion.Walk:
                case SpriteMotion.Freeze1:
                case SpriteMotion.Freeze2:
                case SpriteMotion.Standby:
                case SpriteMotion.Idle:
                    return true;
                case SpriteMotion.Casting:
                case SpriteMotion.Dead:
                case SpriteMotion.Sit:
                case SpriteMotion.Attack1:
                case SpriteMotion.Attack2:
                case SpriteMotion.Attack3:
                case SpriteMotion.Hit:
                case SpriteMotion.PickUp:
                case SpriteMotion.Special:
                case SpriteMotion.Performance1:
                case SpriteMotion.Performance2:
                case SpriteMotion.Performance3:
                    return false;
            }

            return false;
        }

        public static int GetMotionIdForSprite(EntityType type, SpriteMotion motion) {
            if (motion == SpriteMotion.Idle) {
                return 0;
            }

            if (type == EntityType.NPC) {
                switch (motion) {
                    case SpriteMotion.Walk:
                        return 1 * 8;
                    case SpriteMotion.Hit:
                        return 2 * 8;
                    case SpriteMotion.Attack1:
                        return 3 * 8;
                }
            }

            //if(type == EntityType.Monster2) {
            //    if(motion == SpriteMotion.Attack2)
            //        return 5 * 8;
            //}

            if (type == EntityType.MOB || type == EntityType.PET) {
                switch (motion) {
                    case SpriteMotion.Walk:
                        return 1 * 8;
                    case SpriteMotion.Attack1:
                        return 2 * 8;
                    case SpriteMotion.Attack2:
                        return 2 * 8;
                    case SpriteMotion.Attack3:
                        return 2 * 8;
                    case SpriteMotion.Hit:
                        return 3 * 8;
                    case SpriteMotion.Dead:
                        return 4 * 8;
                }
            }

            if (type == EntityType.PET) {
                switch (motion) {
                    case SpriteMotion.Special:
                        return 5 * 8;
                    case SpriteMotion.Performance1:
                        return 6 * 8;
                    case SpriteMotion.Performance2:
                        return 7 * 8;
                    case SpriteMotion.Performance3:
                        return 8 * 8;
                }
            }

            if (type == EntityType.PC) {
                switch (motion) {
                    case SpriteMotion.Walk:
                        return 1 * 8;
                    case SpriteMotion.Sit:
                        return 2 * 8;
                    case SpriteMotion.PickUp:
                        return 3 * 8;
                    case SpriteMotion.Standby:
                        return 4 * 8;
                    case SpriteMotion.Attack1:
                        return 5 * 8;
                    case SpriteMotion.Hit:
                        return 6 * 8;
                    case SpriteMotion.Freeze1:
                        return 7 * 8;
                    case SpriteMotion.Dead:
                        return 8 * 8;
                    case SpriteMotion.Freeze2:
                        return 9 * 8;
                    case SpriteMotion.Attack2:
                        return 10 * 8;
                    case SpriteMotion.Attack3:
                        return 11 * 8;
                    case SpriteMotion.Casting:
                        return 12 * 8;
                }
            }

            return 0;
        }
    }
}