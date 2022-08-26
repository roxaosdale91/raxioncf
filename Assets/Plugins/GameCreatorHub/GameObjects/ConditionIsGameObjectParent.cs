namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ConditionIsGameObjectParent : ICondition
    {
        [Header("This condition checks if selected GameObject is a Parent/ni.e. has Child GameObjects.")]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();
        [Space(10)]

        private GameObject GO;

        public bool satisfied = false;

        public override bool Check (GameObject target)
        {
            GO = gameObjectToCheck.GetValue(target);

            if (GO.transform.childCount > 0)
            {
                satisfied = true;
            }

            return this.satisfied;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ConditionIsGameObjectParent";
        #endif
    }
}
