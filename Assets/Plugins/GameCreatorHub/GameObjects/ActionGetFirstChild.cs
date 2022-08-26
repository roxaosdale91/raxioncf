namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetFirstChild : IAction
    {
        [Header("Select GameObject to get First Child from.")]
        [Header("Will output this GameObject if no children...")]
        [Space(10)]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();

        [Space(20)]
        [Header("This field is for information purposes only...")]
        public int childCount;

        [Space(20)]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty childGameObject = new VariableProperty();

        private GameObject GO;
        private GameObject outputGameObject;
        private Transform childtrans;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            GO = gameObjectToCheck.GetValue(target);
            outputGameObject = GO;

            if(GO.transform.childCount > 0)
            {
                childtrans = GO.transform.GetChild(0);
                outputGameObject = childtrans.gameObject;
            }
            
            this.childGameObject.Set(outputGameObject, target);

            return true;
        }

        private void OnValidate ()
        {
            
            if (GO != null)
            {
                childCount = GO.transform.childCount;
            }
        }

#if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionGetFirstChild";
        #endif
    }


}
