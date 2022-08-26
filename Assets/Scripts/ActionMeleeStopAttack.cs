namespace GameCreator.Melee
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Characters;
	using GameCreator.Core.Hooks;

    [AddComponentMenu("")]
	public class ActionMeleeStopAttack : IAction
	{
		public TargetCharacter character = new TargetCharacter(TargetCharacter.Target.Player);

		private static readonly Vector3 PLANE = new Vector3(1, 0, 1);

        public enum Direction
        {
            CharacterMovement3D,
            TowardsTarget,
            TowardsPosition,
            MovementSidescrollXY,
            MovementSidescrollZY
        }

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			Character _character = this.character.GetCharacter(target);
			CharacterAnimator characterAnimator = _character.GetCharacterAnimator();
			PlayerCharacter player = HookPlayer.Instance.Get<PlayerCharacter>();


			Vector3 moveDirection = Vector3.zero;

			Vector3 charDirection = Vector3.zero;

            Vector3 newDirection = charDirection;


			if (character == null) {
				return true;
			} else {
				charDirection = Vector3.Scale(
                _character.transform.TransformDirection(Vector3.forward), 
                PLANE
            );
			}

			CharacterMelee melee = _character.GetComponent<CharacterMelee>();
			if (melee != null && melee.currentMeleeClip != null)
			{
				if(melee.currentMeleeClip.isAttack == true) {
					melee.StopAttack();
					melee.Blade.EventAttackEnd.Invoke();

					// float angle = Vector3.SignedAngle(moveDirection, charDirection, Vector3.up);

					// if (angle <= 45f && angle >= -45f) {
					// 	newDirection = Vector3.forward;
					// }
					// else if (angle < 135f && angle > 45f) {
					// 	newDirection = Vector3.left;
					// }
					// else if (angle > -135f && angle < -45f) {
					// 	newDirection = Vector3.right;
					// }
					// else {
					// 	newDirection = Vector3.back;
					// }

					// _character.characterLocomotion.SetDirectionalDirection(newDirection);
					// player.ComputeMovement(newDirection);
				}
			}

            return true;
        }

		#if UNITY_EDITOR

        public static new string NAME = "Melee/Input Melee Stop Attack";
		private const string NODE_TITLE = "Stop melee attack on {0}";

        public override string GetNodeTitle()
        {
			return string.Format(NODE_TITLE, this.character);
        }

        #endif
    }
}
