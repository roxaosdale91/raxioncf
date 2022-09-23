namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionFindGameObjectByName : IAction
    {
        [Header("Select Name of [first] GameObject to Find.")]
        [Space(10)]
        public StringProperty gameObjectNameToCheck = new StringProperty();

        [Space(20)]
        [Header("If nothing is found, this returns a NULL.")]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty firstGameObjectFound = new VariableProperty();

        private string nameToCheck;

        private GameObject foundGO;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            nameToCheck = gameObjectNameToCheck.GetValue(target);

            foundGO = GameObject.Find(nameToCheck);
            
            this.firstGameObjectFound.Set(foundGO, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionFindGameObjectByName";
        #endif
    }
}
