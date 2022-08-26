namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ConditionIsGameObjectActive : ICondition
    {
        public enum TypeOfActive
        {
            ActiveAsSelfOnly = 0,
            ActiveWithinHierarchy = 1
        }

        [Header("This condition checks if selected GameObject is Active either/nin and of itself or within a hierarchy.")]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();
        [Space(10)]
        [Header("Is check that GameObject is Active as self or within/na GameObject hierarchy?")]
        public TypeOfActive basisOfCheck;


        private GameObject GO;

        public bool satisfied = false;

        public override bool Check (GameObject target)
        {
            GO = gameObjectToCheck.GetValue(target);

            satisfied = GO.activeSelf;

            if (basisOfCheck == TypeOfActive.ActiveWithinHierarchy)
            {
                satisfied = GO.activeInHierarchy;
            }

            return this.satisfied;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ConditionIsGameObjectActive";
        #endif
    }
}
