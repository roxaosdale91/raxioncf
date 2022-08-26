namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ConditionIsComponentInGameObject : ICondition
    {
        [Header("This condition checks if selected GameObject has a [string] named Component.")]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();
        [Space(10)]

        [Header("Enter the string of the name of the Component or Script to check for.")]
        public StringProperty stringOfComponentToCheck = new StringProperty();
        [Space(10)]

        private GameObject GO;
        private string compStr;
        private Component compo;

        public bool satisfied = false;

        public override bool Check (GameObject target)
        {
            GO = gameObjectToCheck.GetValue(target);
            compStr = stringOfComponentToCheck.GetValue(target);
            //compo = GO.GetComponent(compStr);

            if (GO.GetComponent(compStr))
            {
                satisfied = true;
            }
            if (!GO.GetComponent(compStr))
            {
                satisfied = false;
            }

            return this.satisfied;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ConditionIsComponentInGameObject";
        #endif
    }
}
