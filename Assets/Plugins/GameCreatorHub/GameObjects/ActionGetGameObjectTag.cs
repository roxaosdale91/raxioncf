namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetGameObjectTag : IAction
    {
        [Header("Select GameObject to get GameObject Tag/n as a String.")]
        [Space(10)]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();

        [Space(20)]
        [VariableFilter(Variable.DataType.String)]
        public VariableProperty gameObjectTag = new VariableProperty();

        private GameObject GO;
        private string tag1;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            GO = gameObjectToCheck.GetValue(target);
            
            if (GO != null)
            {
                tag1 = GO.tag;
            }
            
            this.gameObjectTag.Set(tag1, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionGetGameObjectTag";
        #endif
    }


}
