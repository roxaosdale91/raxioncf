namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetListVariablesCount : IAction
    {
        [Header("Enter GameObject containing the ListVariables component to get the Count.")]
        public GameObjectProperty gameObjectContainingListVariables = new GameObjectProperty();
        
        [Space(10)]
        [Header("Output count of items in any ListVariables component in/n the GameObject - if count is -1 then now LV component found!")]
        [VariableFilter(Variable.DataType.Number)]
        public VariableProperty countOfItemsInListVariables = new VariableProperty();


        private int count = -1;
        private GameObject GO;

        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            GO = gameObjectContainingListVariables.GetValue();
            count = -1;

            if (GO.GetComponent<ListVariables>() != null)
            {
                count = GO.GetComponent<ListVariables>().variables.Count;
            }


            this.countOfItemsInListVariables.Set(count, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/ListVariables/ActionGetListVariablesCount";
        #endif
    }
}

