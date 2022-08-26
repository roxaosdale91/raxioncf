namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionFindGameObjectByTag : IAction
    {
        [Header("Select Tag of [first] GameObject to Find.")]
        [Space(10)]
        public StringProperty gameObjectTagToCheck = new StringProperty();

        [Space(20)]
        [Header("If nothing is found, this returns a NULL.")]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty firstGameObjectFound = new VariableProperty();

        private string tagToCheck;

        private GameObject foundGO;

        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            tagToCheck = gameObjectTagToCheck.GetValue(target);

            foundGO = GameObject.FindGameObjectWithTag(tagToCheck);

            this.firstGameObjectFound.Set(foundGO, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionFindGameObjectByTag";
        #endif
    }
}

