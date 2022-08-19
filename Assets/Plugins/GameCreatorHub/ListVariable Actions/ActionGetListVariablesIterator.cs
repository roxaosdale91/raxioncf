namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetListVariablesIterator : IAction
    {
        [Header("Enter GameObject containing the ListVariables component to get the Interator (posn in index).")]
        public GameObjectProperty gameObjectContainingListVariables = new GameObjectProperty();
        
        [Space(10)]
        [Header("Output Iterator integer within any ListVariables component in/n the GameObject - if iterator is -1 then now LV component found!")]
        [VariableFilter(Variable.DataType.Number)]
        public VariableProperty listVariablesIterator = new VariableProperty();


        private int iterator = -1;
        private GameObject GO;

        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            GO = gameObjectContainingListVariables.GetValue();
            iterator = -1;

            if (GO.GetComponent<ListVariables>() != null)
            {
                iterator = GO.GetComponent<ListVariables>().iterator;
            }


            this.listVariablesIterator.Set(iterator, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/ListVariables/ActionGetListVariablesIterator";
        #endif
    }
}

