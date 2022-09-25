namespace GameCreator.Melee
{
    using System;
    using System.Collections;
	using System.Collections.Generic;
    using UnityEngine;
    using GameCreator.Characters;
    using GameCreator.Core;
    using GameCreator.Localization;
    using GameCreator.Core.Hooks;

    [CreateAssetMenu(fileName = "Melee Weapon", menuName = "Game Creator/Melee/Melee Weapon")]
    public class MeleeWeapon : ScriptableObject
	{
        public enum WeaponBone
        {
            Root = -1,
            RightHand = HumanBodyBones.RightHand,
            LeftHand = HumanBodyBones.LeftHand,
            RightArm = HumanBodyBones.RightLowerArm,
            LeftArm = HumanBodyBones.LeftLowerArm,
            RightFoot = HumanBodyBones.RightFoot,
            LeftFoot = HumanBodyBones.LeftFoot,
            Camera = 100,
        }

        public enum HitLocation
        {
            FrontUpper, FrontMiddle, FrontLower,
            BackUpper, BackMiddle, BackLower, JuggleFront, JuggleBack, KnockDownFront, KnockDownBack

        }

        public const CharacterAnimation.Layer LAYER_STANCE = CharacterAnimation.Layer.Layer1;

        // PROPERTIES: ----------------------------------------------------------------------------

        // general:
        [LocStringNoPostProcess] public LocString weaponName = new LocString("Weapon Name");
        [LocStringNoPostProcess] public LocString weaponDescription = new LocString("Weapon Description");

        public MeleeShield defaultShield;
        public CharacterState characterState;
        public AvatarMask characterMask;

        // 3d model:
        public GameObject prefab;
        public WeaponBone attachment = WeaponBone.RightHand;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;// 3d model:
        public List<GameObject> prefabs = new List<GameObject>();

        // audio:
        public AudioClip audioSheathe;
        public AudioClip audioDraw;
        public AudioClip audioImpactNormal;
        public AudioClip audioImpactKnockback;

        // reactions:
        public List<MeleeClip> groundHitReactionsFrontUpper = new List<MeleeClip>();
        public List<MeleeClip> groundHitReactionsFrontMiddle = new List<MeleeClip>();
        public List<MeleeClip> groundHitReactionsFrontLower = new List<MeleeClip>();

        public List<MeleeClip> groundHitReactionsBackUpper = new List<MeleeClip>();
        public List<MeleeClip> groundHitReactionsBackMiddle = new List<MeleeClip>();
        public List<MeleeClip> groundHitReactionsBackLower = new List<MeleeClip>();

        public List<MeleeClip> airborneHitReactionsFrontUpper  = new List<MeleeClip>();
        public List<MeleeClip> airborneHitReactionsFrontMiddle = new List<MeleeClip>();
        public List<MeleeClip> airborneHitReactionsFrontLower = new List<MeleeClip>();

        public List<MeleeClip> airborneHitReactionsBackUpper  = new List<MeleeClip>();
        public List<MeleeClip> airborneHitReactionsBackMiddle = new List<MeleeClip>();
        public List<MeleeClip> airborneHitReactionsBackLower = new List<MeleeClip>();

        public List<MeleeClip> knockbackReaction = new List<MeleeClip>();
        public List<MeleeClip> knockupReaction = new List<MeleeClip>();
        public List<MeleeClip> stunReaction = new List<MeleeClip>();

        // combo system:
        public List<Combo> combos = new List<Combo>();

        // impacts:
        public GameObject prefabImpactNormal;
        public GameObject prefabImpactKnockback;

        // PRIVATE PROPERTIES: --------------------------------------------------------------------

        private int prevRandomHit = -1;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public List<GameObject> EquipWeapon(CharacterAnimator character)
        {
            if (this.prefabs == null || this.prefabs.Count == 0) return null;
            if (character == null) return null;

            var instances = new List<GameObject>();
            
            foreach (var prefab in prefabs)
            {
                GameObject instance = Instantiate(prefab);
                instances.Add(instance);
                instance.transform.localScale = prefab.transform.localScale;

                var blade = instance.GetComponentInChildren<BladeComponent>();
                Transform bone = null;
                switch (blade.bone)
                {
                    case BladeComponent.WeaponBone.Root:
                        bone = character.transform;
                        break;

                    case BladeComponent.WeaponBone.Camera:
                        bone = HookCamera.Instance.transform;
                        break;

                    default:
                        bone = character.animator.GetBoneTransform((HumanBodyBones)blade.bone);
                        break;
                }
                if (!bone) continue;
                blade.transform.SetParent(bone);

                blade.transform.localPosition = prefab.transform.position;
                blade.transform.localRotation = prefab.transform.rotation;

            }
            return instances;
        }

        public MeleeClip GetHitReaction(bool isGrounded, HitLocation location, bool isKnockback, bool isKnockedUp, bool isAttackKnockup, 
                MeleeClip.KnockUpType knockuptype)
        {
            int index;
            MeleeClip meleeClip = null;
            List<MeleeClip> hitReactionList;

            if(isAttackKnockup)  // Use index 1 always for knockup
            {
                index = UnityEngine.Random.Range(0, this.knockupReaction.Count - 1);
                if (this.knockupReaction.Count != 1 && index == this.prevRandomHit) index++;
                this.prevRandomHit = index;

                bool isFront = this.GetHitLocation(location);

                switch(knockuptype) {
                    case MeleeClip.KnockUpType.Regular:
                        if(isFront) {
                            meleeClip = this.knockupReaction[0];           
                        } else {
                            meleeClip = this.knockupReaction[1];
                        }
                        break;
                    case MeleeClip.KnockUpType.Smash:
                        if(isFront) {
                                meleeClip = this.knockupReaction[2];           
                            } else {
                                meleeClip = this.knockupReaction[3];
                            }
                        break;
                }

                return meleeClip;
            }
            
            if (isKnockback) // Use index 0 always for knockback
            {
                index = UnityEngine.Random.Range(0, this.knockbackReaction.Count - 1);
                if (this.knockbackReaction.Count != 1 && index == this.prevRandomHit) index++;
                this.prevRandomHit = index;

                 switch (location)
                {
                    case HitLocation.FrontUpper:
                    case HitLocation.FrontMiddle:
                    case HitLocation.FrontLower:
                        meleeClip = this.knockbackReaction[0];
                        break;

                    case HitLocation.BackLower:
                    case HitLocation.BackMiddle:
                    case HitLocation.BackUpper:
                        meleeClip = this.knockbackReaction[1];
                        break;
                }

                return meleeClip;
            }

            if (isKnockedUp) {
                 var seed = 0;
                 var indexAnim = 0;
                 var lowerBound = 0;
                 var upperBound = 1;

                if(this.airborneHitReactionsBackMiddle.Count != 1) upperBound = this.airborneHitReactionsBackMiddle.Count;

                System.Random randomizer = new System.Random();

                indexAnim = randomizer.Next(lowerBound, upperBound);

                switch (location)
                {
                    case HitLocation.FrontUpper:
                    case HitLocation.FrontMiddle:
                    case HitLocation.FrontLower:
                        index = UnityEngine.Random.Range(0, this.airborneHitReactionsFrontMiddle.Count);
                        if (this.airborneHitReactionsFrontMiddle.Count != 1 && index == this.prevRandomHit) index++;
                        this.prevRandomHit = index;
                        upperBound = this.airborneHitReactionsFrontMiddle.Count;

                        indexAnim = randomizer.Next(lowerBound, upperBound);

                        meleeClip = this.airborneHitReactionsFrontMiddle[indexAnim];
                        break;

                    case HitLocation.BackLower:
                    case HitLocation.BackMiddle:
                    case HitLocation.BackUpper:
                        index = UnityEngine.Random.Range(0, this.airborneHitReactionsBackMiddle.Count);
                        if (this.airborneHitReactionsBackMiddle.Count != 1 && index == this.prevRandomHit) index++;
                        this.prevRandomHit = index;
                        upperBound = this.airborneHitReactionsBackMiddle.Count;

                        indexAnim = randomizer.Next(lowerBound, upperBound);

                        meleeClip = this.airborneHitReactionsBackMiddle[indexAnim];
                        break;
                }

                return meleeClip;
            }
            
            switch (location)
            {
                case HitLocation.FrontUpper:
                    hitReactionList = isGrounded ? this.groundHitReactionsFrontUpper : this.airborneHitReactionsFrontUpper;
                    break;
                case HitLocation.FrontMiddle:
                    hitReactionList = isGrounded ? this.groundHitReactionsFrontMiddle : this.airborneHitReactionsFrontMiddle;
                    break;
                case HitLocation.FrontLower:
                    hitReactionList = isGrounded ? this.groundHitReactionsFrontLower : this.airborneHitReactionsFrontLower;
                    break;
                case HitLocation.BackUpper:
                    hitReactionList = isGrounded ? this.groundHitReactionsBackUpper : this.airborneHitReactionsBackUpper;
                    break;
                case HitLocation.BackMiddle:
                    hitReactionList = isGrounded ? this.groundHitReactionsBackMiddle : this.airborneHitReactionsBackMiddle;
                    break;
                case HitLocation.BackLower:
                    hitReactionList = isGrounded ? this.groundHitReactionsBackLower : this.airborneHitReactionsBackLower;
                    break;
                default: hitReactionList = knockbackReaction; break;
            }

            index = UnityEngine.Random.Range(0, hitReactionList.Count - 1);
            if (hitReactionList.Count != 1 && index == this.prevRandomHit) index++;
            this.prevRandomHit = index;

            meleeClip = hitReactionList[index];

            return meleeClip;
        }

        public Boolean GetHitLocation(HitLocation location) {
            bool isFront = false;

            switch (location) {
                case HitLocation.FrontUpper:
                case HitLocation.FrontMiddle:
                case HitLocation.FrontLower:
                    isFront = true;
                    break;

                
                case HitLocation.BackUpper:
                case HitLocation.BackMiddle:
                case HitLocation.BackLower:
                    isFront = false;
                    break;
            }

            return isFront;
        }
    }
}
