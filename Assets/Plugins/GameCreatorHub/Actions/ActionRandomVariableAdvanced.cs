namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Variables;

    [AddComponentMenu("")]
	public class ActionRandomVariableAdvanced : IAction
	{
        public enum Step
        {
            Decimal,
            Integer
        }

        public Step step = Step.Decimal;

        public NumberProperty min = new NumberProperty(0f);
        public NumberProperty max = new NumberProperty(10f);

        [VariableFilter(Variable.DataType.Number)]
        public VariableProperty result = new VariableProperty(Variable.VarType.GlobalVariable);

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            float minValue = this.min.GetValue(target);
            float maxValue = this.max.GetValue(target);

            if (this.step == Step.Integer)
            {
                minValue = Mathf.Floor(minValue);
                maxValue = Mathf.Floor(maxValue);
            }

            float value = Random.Range(minValue, maxValue);
            switch (this.step)
            {
                case Step.Integer: this.result.Set(Mathf.Round(value), target); break;
                case Step.Decimal: this.result.Set(value, target); break;
            }

            return true;
        }

		#if UNITY_EDITOR
        public static new string NAME = "Variables/Variable Random Advanced";
        private const string NODE_TITLE = "Random between {0} and {1} into {2} as {3}";

        public override string GetNodeTitle()
        {
            return string.Format(
                NODE_TITLE,
                this.min,
                this.max,
                this.result,
                this.step
            );
        }

        #endif
    }
}
