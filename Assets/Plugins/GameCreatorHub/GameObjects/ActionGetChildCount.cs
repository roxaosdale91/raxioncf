namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetChildCount : IAction
    {
        [Header("Select GameObject to count Children of.")]
        [Space(10)]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();

        [Space(20)]
        [VariableFilter(Variable.DataType.Number)]
        public VariableProperty countOfChildGameObjects = new VariableProperty();

        private GameObject GO;
        private int count;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            count = 0;
            GO = gameObjectToCheck.GetValue(target);

            
            count = GO.transform.childCount;
            
            
            this.countOfChildGameObjects.Set(count, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionGetChildCount";
        #endif
    }


}
