namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetParentGameObject : IAction
    {
        [Header("Select GameObject to get Parent GameObject from.")]
        [Header("Will output this GameObject if no Parent GameObject...")]
        [Space(10)]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();

        [Space(20)]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty parentGameObject = new VariableProperty();

        private GameObject GO;
        private GameObject outputGameObject;
        private Transform parenttrans;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            GO = gameObjectToCheck.GetValue(target);
            outputGameObject = GO;

            if(GO.transform.parent != null)
            {
                parenttrans = GO.transform.parent;
                outputGameObject = parenttrans.gameObject;
            }
            
            this.parentGameObject.Set(outputGameObject, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionGetParentGameObject";
        #endif
    }


}
