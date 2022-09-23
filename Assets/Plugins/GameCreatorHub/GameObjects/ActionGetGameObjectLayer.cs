namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetGameObjectLayer : IAction
    {
        [Header("Select GameObject to get GameObject Layer/nas a String.")]
        [Space(10)]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();

        [Space(20)]
        [VariableFilter(Variable.DataType.String)]
        public VariableProperty gameObjectLayer = new VariableProperty();

        private GameObject GO;
        private string layer1;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            GO = gameObjectToCheck.GetValue(target);
            
            if (GO != null)
            {
                layer1 = GO.layer.ToString();
            }
            
            this.gameObjectLayer.Set(layer1, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionGetGameObjectLayer";
        #endif
    }


}
