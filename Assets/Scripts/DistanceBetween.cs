namespace GameCreator.Core
{
    using GameCreator.Variables;
    using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	public class DistanceBetween : IAction
	{
		public TargetGameObject objectOne;
		public TargetGameObject objectTwo;
		public VariableProperty storeInstance = new VariableProperty();

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			float distance = Vector3.Distance(objectOne.gameObject.transform.position, objectTwo.gameObject.transform.position);
			this.storeInstance.Set(distance, target);
			return true;
        }

		#if UNITY_EDITOR
        public static new string NAME = "Custom/DistanceBetween";
		#endif
	}
}
