namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Variables;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionVariableWait : IAction 
	{
		public NumberProperty waitTime = new NumberProperty(0.0f);
        private bool forceStop = false;

		// EXECUTABLE: ----------------------------------------------------------------------------
		
        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{
            this.forceStop = false;
            float waitTimeValue = this.waitTime.GetValue(target);
            float stopTime = Time.time + waitTimeValue;
            WaitUntil waitUntil = new WaitUntil(() => Time.time > stopTime || this.forceStop);

            yield return waitUntil;
			yield return 0;
		}

        public override void Stop()
        {
            this.forceStop = true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public static new string NAME = "General/Variable Wait";
		private const string NODE_TITLE = "Wait Variable Dependent Time";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spWaitTime;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return NODE_TITLE;
		}

		protected override void OnEnableEditorChild()
		{
			this.spWaitTime = this.serializedObject.FindProperty("waitTime");
		}

		protected override void OnDisableEditorChild()
		{
			this.spWaitTime = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spWaitTime);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}