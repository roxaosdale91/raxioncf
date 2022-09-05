namespace GameCreator.Characters
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Core;
    using GameCreator.Characters;
    using GameCreator.Variables;
    using GameCreator.Melee;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    [AddComponentMenu("")]
	public class ActionCharacterDash : IAction
	{
        private static readonly Vector3 PLANE = new Vector3(1, 0, 1);

        public enum Direction
        {
            CharacterMovement3D,
            TowardsTarget,
            TowardsPosition,
            MovementSidescrollXY,
            MovementSidescrollZY
        }

        public TargetCharacter character = new TargetCharacter(TargetCharacter.Target.Player);

        public Direction direction = Direction.CharacterMovement3D;
        public TargetGameObject target = new TargetGameObject();
        public TargetPosition position = new TargetPosition();

        public NumberProperty impulse = new NumberProperty(5f);
        public NumberProperty duration = new NumberProperty(0f);
        public float drag = 10f;

        [Space]
        public AnimationClip dashClipForward;
        public AnimationClip dashClipBackward;
        public AnimationClip dashClipRight;
        public AnimationClip dashClipLeft;

        public float waitTime = 0.50f;
        private bool forceStop = false;

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            Character characterTarget = this.character.GetCharacter(target);
            if (characterTarget == null) return true;

            

            CharacterLocomotion locomotion = characterTarget.characterLocomotion;
            CharacterMelee melee = characterTarget.GetComponent<CharacterMelee>();
            CharacterAnimator animator = characterTarget.GetCharacterAnimator();

            Vector3 moveDirection = Vector3.zero;
            locomotion.Stop();

            if (melee != null)
			{
                melee.IsDodging = true;
				if(melee.currentMeleeClip != null && melee.currentMeleeClip.isAttack == true) {
                    melee.StopAttack();
					animator.StopGesture(0f);
                    melee.currentMeleeClip = null;
				}
			}

            switch (this.direction)
            {
                case Direction.CharacterMovement3D:
                    moveDirection = locomotion.GetMovementDirection();
                    break;

                case Direction.TowardsTarget:
                    Transform targetTransform = this.target.GetTransform(target);
                    if (targetTransform != null)
                    {
                        moveDirection = targetTransform.position - characterTarget.transform.position;
                        moveDirection.Scale(PLANE);
                    }
                    break;

                case Direction.TowardsPosition:
                    Vector3 targetPosition = this.position.GetPosition(target);
                    moveDirection = targetPosition - characterTarget.transform.position;
                    moveDirection.Scale(PLANE);
                    break;

                case Direction.MovementSidescrollXY:
                    moveDirection = locomotion.GetMovementDirection();
                    moveDirection.Scale(new Vector3(1, 1, 0));
                    break;

                case Direction.MovementSidescrollZY:
                    moveDirection = locomotion.GetMovementDirection();
                    moveDirection.Scale(new Vector3(0, 1, 1));
                    break;
            }

            Vector3 charDirection = Vector3.Scale(
                characterTarget.transform.TransformDirection(Vector3.forward), 
                PLANE
            );

            Vector3 newDirection = charDirection;

            float angle = Vector3.SignedAngle(moveDirection, charDirection, Vector3.up);
            AnimationClip clip = null;

            if (angle <= 45f && angle >= -45f) {
                clip = this.dashClipForward;
                newDirection = Vector3.forward;
            }
            else if (angle < 135f && angle > 45f) {
                clip = this.dashClipLeft;
                newDirection = Vector3.left;
            }
            else if (angle > -135f && angle < -45f) {
                clip = dashClipRight;
                newDirection = Vector3.right;
            }
            else {
                clip = this.dashClipBackward;
                newDirection = Vector3.back;
            }
            
            var directionMovement = CharacterLocomotion.OVERRIDE_FACE_DIRECTION.MovementDirection;
            locomotion.overrideFaceDirection = directionMovement;

            bool isDashing = characterTarget.Dash(
                moveDirection.normalized,
                this.impulse.GetValue(target),
                this.duration.GetValue(target),
                this.drag
            );
            
            if (isDashing && clip != null && animator != null)
            {
                animator.CrossFadeGesture(clip, 1f, null, 0.05f, 0.5f);
            }


            melee.IsDodging = false;
            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Character/Character Dash";
        private const string TITLE_NAME = "Character {0} dash {1}";

        public override string GetNodeTitle()
        {
            return string.Format(
                TITLE_NAME,
                this.character,
                this.direction
            );
        }

        private SerializedProperty spCharacter;
        private SerializedProperty spDirection;
        private SerializedProperty spTarget;
        private SerializedProperty spPosition;

        private SerializedProperty spImpulse;
        private SerializedProperty spDuration;
        private SerializedProperty spDrag;

        private SerializedProperty spDashForward;
        private SerializedProperty spDashBackward;
        private SerializedProperty spDashRight;
        private SerializedProperty spDashLeft;

        protected override void OnEnableEditorChild()
        {
            this.spCharacter = this.serializedObject.FindProperty("character");
            this.spDirection = this.serializedObject.FindProperty("direction");
            this.spTarget = this.serializedObject.FindProperty("target");
            this.spPosition = this.serializedObject.FindProperty("position");

            this.spImpulse = this.serializedObject.FindProperty("impulse");
            this.spDuration = this.serializedObject.FindProperty("duration");
            this.spDrag = this.serializedObject.FindProperty("drag");

            this.spDashForward = this.serializedObject.FindProperty("dashClipForward");
            this.spDashBackward = this.serializedObject.FindProperty("dashClipBackward");
            this.spDashRight = this.serializedObject.FindProperty("dashClipRight");
            this.spDashLeft = this.serializedObject.FindProperty("dashClipLeft");
        }

        protected override void OnDisableEditorChild()
        {
            this.spCharacter = null;
            this.spDirection = null;
            this.spTarget = null;
            this.spPosition = null;

            this.spImpulse = null;
            this.spDuration = null;
            this.spDrag = null;

            this.spDashForward = null;
            this.spDashBackward = null;
            this.spDashRight = null;
            this.spDashLeft = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(this.spCharacter);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spDirection);
            switch (this.spDirection.enumValueIndex)
            {
                case (int)Direction.TowardsTarget:
                    EditorGUILayout.PropertyField(this.spTarget);
                    break;

                case (int)Direction.TowardsPosition:
                    EditorGUILayout.PropertyField(this.spPosition);
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spImpulse);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spDuration);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spDrag);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spDashForward);
            EditorGUILayout.PropertyField(this.spDashBackward);
            EditorGUILayout.PropertyField(this.spDashRight);
            EditorGUILayout.PropertyField(this.spDashLeft);

            serializedObject.ApplyModifiedProperties();
        }
        #endif
    }
}
