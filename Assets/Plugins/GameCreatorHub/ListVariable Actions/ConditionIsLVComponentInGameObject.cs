namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ConditionIsLVComponentInGameObject : ICondition
    {
        [Header("This condition checks if selected GameObject has a ListVariables component in it.")]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();
        [Space(10)]

        public bool satisfied = false;

        private GameObject GO;

        public override bool Check (GameObject target)
        {
            satisfied = false;
            GO = gameObjectToCheck.GetValue();

            if (GO.GetComponent<ListVariables>() != null)
            {
                satisfied = true;
            }

            return this.satisfied;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/ListVariables/ConditionIsLVComponentInGameObject";
        #endif
    }
}
