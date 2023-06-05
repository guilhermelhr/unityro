using System.Collections;
using ROIO.Models.FileTypes;
using UnityEngine;
using UnityRO.Core.Camera;
using UnityRO.Core.Database;
using UnityRO.Core.GameEntity;

namespace UnityRO.Core.Sprite {
    public class FramePaceCalculator {
        private const int AVERAGE_ATTACK_SPEED = 435;
        private const int AVERAGE_ATTACKED_SPEED = 288;
        private const int MAX_ATTACK_SPEED = AVERAGE_ATTACKED_SPEED * 2;

        private CoreSpriteGameEntity Entity;
        private ViewerType ViewerType;
        private CharacterCamera CharacterCamera;

        private int CurrentFrame = 0;
        private long AnimationStart = GameManager.Tick;
        private float CurrentDelay = 0f;
        private MotionRequest CurrentMotion;
        private MotionRequest? NextMotion;
        private ACT CurrentACT;
        private ACT.Action CurrentAction;
        private int ActionId;

        private float AttackMotion = 6f;
        private float MotionSpeed = 1f;

        private Coroutine MotionQueueCoroutine;

        public FramePaceCalculator(
            CoreSpriteGameEntity entity,
            ViewerType viewerType,
            ACT currentACT,
            CharacterCamera characterCamera
        ) {
            Entity = entity;
            ViewerType = viewerType;
            CurrentACT = currentACT;
            CharacterCamera = characterCamera;
        }

        public int GetActionIndex() {
            var cameraDirection = (int)CharacterCamera.Direction;
            var entityDirection = (int)Entity.Direction + 8;

            return (ActionId + (cameraDirection + entityDirection) % 8) % CurrentACT.actions.Length;
        }

        public int GetCurrentFrame() {
            CurrentAction = CurrentACT.actions[GetActionIndex()];

            var isIdle = (Entity.Status.EntityType == EntityType.PC && CurrentMotion.Motion is SpriteMotion.Idle or SpriteMotion.Sit);
            var frameCount = CurrentAction.frames.Length;
            var deltaSinceMotionStart = (GameManager.Tick - AnimationStart);

            var maxFrame = frameCount - 1;

            if (isIdle) {
                CurrentFrame = Entity.HeadDirection;
            }

            CurrentDelay = GetDelay();
            if (deltaSinceMotionStart >= CurrentDelay) {
                PCLog($"{CurrentMotion.Motion} Frame delay passed {deltaSinceMotionStart} {CurrentDelay}, advancing frame");
                AnimationStart = GameManager.Tick;

                if (CurrentFrame < maxFrame && !isIdle) {
                    CurrentFrame++;
                }
            }

            if (CurrentFrame >= maxFrame) {
                if (AnimationHelper.IsLoopingMotion(CurrentMotion.Motion)) {
                    PCLog($"{CurrentMotion.Motion} Animation ended, looping");
                    CurrentFrame = 0;
                } else if (NextMotion.HasValue && ViewerType == ViewerType.Body) {
                    PCLog($"{CurrentMotion.Motion} Animation ended, next is available, advancing");
                    // Since body is the main component, it's the only one "allowed" to ask for the next motion
                    Entity.ChangeMotion(NextMotion.Value);
                } else {
                    PCLog($"{CurrentMotion.Motion} Animation ended, stopping");
                    CurrentFrame = maxFrame;
                }
            }

            return CurrentFrame;
        }

        public float GetDelay() {
            if (ViewerType == ViewerType.Body && CurrentMotion.Motion == SpriteMotion.Walk) {
                return CurrentAction.delay / 150 * Entity.Status.MoveSpeed;
            }

            if (CurrentMotion.Motion is SpriteMotion.Attack1 or SpriteMotion.Attack2 or SpriteMotion.Attack3) {
                var delayTime = AttackMotion * MotionSpeed;
                if (delayTime < 0) {
                    delayTime = 0;
                }

                return delayTime / CurrentAction.frames.Length;
            }

            return CurrentAction.delay;
        }

        public void OnMotionChanged(MotionRequest currentMotion, MotionRequest? nextMotion, int actionId) {
            if (MotionQueueCoroutine != null) {
                Entity.StopCoroutine(MotionQueueCoroutine);
                MotionQueueCoroutine = null;
            }

            if (currentMotion.delay > GameManager.Tick) {
                MotionQueueCoroutine = Entity.StartCoroutine(DelayCurrentMotion(currentMotion, nextMotion, actionId));
                return;
            }

            if (CurrentMotion.Motion is SpriteMotion.Attack1 or SpriteMotion.Attack2 or SpriteMotion.Attack3) {
                if ((EntityType)Entity.GetEntityType() == EntityType.PC) {
                    MotionSpeed = GetMotionSpeed();
                    AttackMotion = 6f;

                    var isSecondAttack = WeaponTypeDatabase.IsSecondAttack(
                        Entity.Status.Job,
                        Entity.Status.IsMale ? 1 : 0,
                        Entity.Status.Weapon,
                        Entity.Status.Shield
                    );

                    if (isSecondAttack) {
                        if ((JobType)Entity.Status.Job is JobType.JT_NOVICE or JobType.JT_SUPERNOVICE or JobType.JT_SUPERNOVICE_B) {
                            if (Entity.Status.IsMale) {
                                AttackMotion = 5.85f;
                            }
                        } else if ((JobType)Entity.Status.Job is JobType.JT_ASSASSIN or JobType.JT_ASSASSIN_H or JobType.JT_ASSASSIN_B) {
                            switch ((WeaponType)Entity.Status.Weapon) {
                                case WeaponType.CATARRH:
                                case WeaponType.SHORTSWORD_SHORTSWORD:
                                case WeaponType.SWORD_SWORD:
                                case WeaponType.AXE_AXE:
                                case WeaponType.SHORTSWORD_SWORD:
                                case WeaponType.SHORTSWORD_AXE:
                                case WeaponType.SWORD_AXE:
                                    AttackMotion = 3.0f;
                                    break;
                            }
                        }
                    } else {
                        AttackMotion = (JobType)Entity.Status.Job switch {
                            JobType.JT_THIEF => 5.75f,
                            JobType.JT_MERCHANT => 5.85f,
                            _ => AttackMotion
                        };
                    }

                    var usingArrow = WeaponTypeDatabase.IsWeaponUsingArrow(Entity.Status.Weapon);
                    if (usingArrow) {
                        //TODO some additional checks see Pc.cpp line 847
                        // Dividing by 25f to get rid of the mult we do when reading the act delays...
                        AttackMotion += 8 / (MotionSpeed / 25f);
                    }
                } else {
                    for (var index = 0; index < CurrentACT.sounds.Length; index++) {
                        var sound = CurrentACT.sounds[index];
                        if (sound == "atk") {
                            AttackMotion = index;
                        }
                    }
                }
            }

            AnimationStart = GameManager.Tick;
            CurrentFrame = 0;
            CurrentMotion = currentMotion;
            NextMotion = nextMotion;
            ActionId = actionId;

            CurrentAction = CurrentACT.actions[GetActionIndex()];
            CurrentDelay = GetDelay();
            PCLog($"{ViewerType} Current delay for {CurrentMotion.Motion} is {CurrentDelay}");
        }

        private IEnumerator DelayCurrentMotion(MotionRequest currentMotion, MotionRequest? nextMotion, int actionId) {
            yield return new WaitUntil(() => GameManager.Tick > currentMotion.delay);
            OnMotionChanged(currentMotion, nextMotion, actionId);
        }

        private float GetMotionSpeed() {
            CurrentAction = CurrentACT.actions[GetActionIndex()];
            var attackSpeed = Entity.Status.AttackSpeed > MAX_ATTACK_SPEED ? MAX_ATTACK_SPEED : Entity.Status.AttackSpeed;
            var multiplier = attackSpeed / (float)AVERAGE_ATTACK_SPEED;
            var motionSpeed = CurrentAction.delay * multiplier;

            return motionSpeed;
        }

        private void PCLog(string message) {
            if (Entity.GetEntityType() == (int)EntityType.PC && ViewerType == ViewerType.Body && !Entity.HasAuthority()) {
                //Debug.Log(message);
            }
        }
    }
}