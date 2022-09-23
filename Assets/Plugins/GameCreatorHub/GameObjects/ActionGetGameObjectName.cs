namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetGameObjectName : IAction
    {
        [Header("Select GameObject to get GameObject Name/nas a String.")]
        [Space(10)]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();

        [Space(20)]
        [VariableFilter(Variable.DataType.String)]
        public VariableProperty gameObjectName = new VariableProperty();

        private GameObject GO;
        private string name1;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            GO = gameObjectToCheck.GetValue(target);
            
            if (GO != null)
            {
                name1 = GO.name;
            }
            
            this.gameObjectName.Set(name1, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionGetGameObjectName";
        #endif
    }


}
