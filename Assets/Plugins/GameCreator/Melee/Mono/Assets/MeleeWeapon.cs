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
        public Vector3 rotationOffset;

        // audio:
        public AudioClip audioSheathe;
        public AudioClip audioDraw;
        public AudioClip audioImpactNormal;
        public AudioClip audioImpactKnockback;

        // reactions:
        public List<MeleeClip> groundHitReactionsFront = new List<MeleeClip>();
        public List<MeleeClip> groundHitReactionsBehind = new List<MeleeClip>();

        public List<MeleeClip> airborneHitReactionsFront = new List<MeleeClip>();
        public List<MeleeClip> airborneHitReactionsBehind = new List<MeleeClip>();

        public List<MeleeClip> knockbackReaction = new List<MeleeClip>();

        // combo system:
        public List<Combo> combos = new List<Combo>();

        // impacts:
        public GameObject prefabImpactNormal;
        public GameObject prefabImpactKnockback;

        // PRIVATE PROPERTIES: --------------------------------------------------------------------

        private int prevRandomHit = -1;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public GameObject EquipWeapon(CharacterAnimator character)
        {
            if (this.prefab == null) return null;
            if (character == null) return null;

            Transform bone = null;
            switch (this.attachment)
            {
                case WeaponBone.Root:
                    bone = character.transform;
                    break;

                case WeaponBone.Camera:
                    bone = HookCamera.Instance.transform;
                    break;

                default:
                    bone = character.animator.GetBoneTransform((HumanBodyBones)this.attachment);
                    break;

            }

            if (!bone) return null;

            GameObject instance = Instantiate(this.prefab);
            instance.transform.localScale = this.prefab.transform.localScale;

            instance.transform.SetParent(bone);

            instance.transform.localPosition = this.positionOffset;
            instance.transform.localRotation = Quaternion.Euler(this.rotationOffset);

            return instance;
        }

        public enum SpecialHitReaction {
            isKnockback = 0,
            isInitialKnockUp = 1
        }

        public MeleeClip GetHitReaction(bool isGrounded, bool frontalAttack, bool isKnockback, bool isKnockedUp, bool isAttackKnockup)
        {
            int index;
            MeleeClip meleeClip = null;
            if(isAttackKnockup)  // Use index 1 always for knockup
            {
                index = UnityEngine.Random.Range(0, this.knockbackReaction.Count - 1);
                if (this.knockbackReaction.Count != 1 && index == this.prevRandomHit) index++;
                this.prevRandomHit = index;

                switch (frontalAttack)
                {
                    case true:
                        meleeClip = this.knockbackReaction[1];
                        break;

                    case false:
                        meleeClip = this.knockbackReaction[2];
                        break;
                }

                return meleeClip;
            }
            
            if (isKnockback) // Use index 0 always for knockback
            {
                index = UnityEngine.Random.Range(0, this.knockbackReaction.Count - 1);
                if (this.knockbackReaction.Count != 1 && index == this.prevRandomHit) index++;
                this.prevRandomHit = index;
                

                return this.knockbackReaction[0];
            }

            if (isKnockedUp) {
                 var seed = 0;
                 var indexAnim = 0;
                 var lowerBound = 0;
                 var upperBound = 1;

                if(this.airborneHitReactionsBehind.Count != 1) upperBound = this.airborneHitReactionsBehind.Count;

                System.Random randomizer = new System.Random();

                indexAnim = randomizer.Next(lowerBound, upperBound);

                switch (frontalAttack)
                {
                    case true:
                        index = UnityEngine.Random.Range(0, this.airborneHitReactionsFront.Count);
                        if (this.airborneHitReactionsFront.Count != 1 && index == this.prevRandomHit) index++;
                        this.prevRandomHit = index;
                        upperBound = this.airborneHitReactionsFront.Count;

                        indexAnim = randomizer.Next(lowerBound, upperBound);

                        meleeClip = this.airborneHitReactionsFront[indexAnim];
                        break;

                    case false:
                        index = UnityEngine.Random.Range(0, this.airborneHitReactionsBehind.Count);
                        if (this.airborneHitReactionsBehind.Count != 1 && index == this.prevRandomHit) index++;
                        this.prevRandomHit = index;
                        upperBound = this.airborneHitReactionsBehind.Count;

                        indexAnim = randomizer.Next(lowerBound, upperBound);

                        meleeClip = this.airborneHitReactionsBehind[indexAnim];
                        break;
                }

                return meleeClip;
            }

            switch (isGrounded)
            {
                case true:
                    switch (frontalAttack)
                    {
                        case true:
                            index = UnityEngine.Random.Range(0, this.groundHitReactionsFront.Count - 1);
                            if (this.groundHitReactionsFront.Count != 1 && index == this.prevRandomHit) index++;
                            this.prevRandomHit = index;

                            meleeClip = this.groundHitReactionsFront[index];
                            break;

                        case false:
                            index = UnityEngine.Random.Range(0, this.groundHitReactionsBehind.Count - 1);
                            if (this.groundHitReactionsBehind.Count != 1 && index == this.prevRandomHit) index++;
                            this.prevRandomHit = index;

                            meleeClip = this.groundHitReactionsBehind[index];
                            break;
                    }
                    break;

                case false:
                    switch (frontalAttack)
                    {
                        case true:
                            index = UnityEngine.Random.Range(0, this.airborneHitReactionsFront.Count - 1);
                            if (this.airborneHitReactionsFront.Count != 1 && index == this.prevRandomHit) index++;
                            this.prevRandomHit = index;

                            meleeClip = this.airborneHitReactionsFront[index];
                            break;

                        case false:
                            index = UnityEngine.Random.Range(0, this.airborneHitReactionsBehind.Count - 1);
                            if (this.airborneHitReactionsBehind.Count != 1 && index == this.prevRandomHit) index++;
                            this.prevRandomHit = index;

                            meleeClip = this.airborneHitReactionsBehind[index];
                            break;
                    }
                    break;
            }

            return meleeClip;
        }
    }
}
