namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetRootGameObject : IAction
    {
        [Header("Select GameObject to get Root GameObject from.")]
        [Header("This is the GameObject at the top of a hierarchy of GameObjects.")]
        [Header("Will output this GameObject if no Root GameObject...")]
        [Space(10)]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();

        [Space(20)]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty rootGameObject = new VariableProperty();

        private GameObject GO;
        private GameObject outputGameObject;
        private Transform roottrans;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            GO = gameObjectToCheck.GetValue(target);
            outputGameObject = GO;

            if(GO.transform.root != null)
            {
                roottrans = GO.transform.root;
                outputGameObject = roottrans.gameObject;
            }
            
            this.rootGameObject.Set(outputGameObject, target);

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionGetRootGameObject";
        #endif
    }


}
