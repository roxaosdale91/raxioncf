namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ConditionIsGameObjectGrandChild : ICondition
    {

        [Header("This condition checks if selected GameObject is a GrandChild/ni.e. is within a hierarchy and has a Parent GameObject/nand Parent also has its own Parent GameObject.")]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();
        [Space(10)]

        private GameObject GO;

        public bool satisfied = false;

        public override bool Check (GameObject target)
        {
            GO = gameObjectToCheck.GetValue(target);

            if (GO.transform.parent.parent != null)
            {
                satisfied = true;
            }

            return this.satisfied;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ConditionIsGameObjectGrandChild";
        #endif
    }
}
