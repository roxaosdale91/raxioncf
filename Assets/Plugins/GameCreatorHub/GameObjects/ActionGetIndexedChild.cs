namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetIndexedChild : IAction
    {
        [Header("Select GameObject to get Indexed Child from.")]
        [Header("Will output this GameObject if no children...")]
        [Space(10)]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();

        [Space(20)]
        [Header("This field is for information purposes only...")]
        public int childCount;

        [Space(20)]
        [Header("Please enter the index of the Child you want to get/nstarting with 1 and up to the above child count.")]
        public NumberProperty indexOfChild = new NumberProperty();

        [Space(20)]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty childGameObject = new VariableProperty();

        private GameObject GO;
        private GameObject outputGameObject;
        private Transform childtrans;
        private int index;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            GO = gameObjectToCheck.GetValue(target);
            outputGameObject = GO;
            index = indexOfChild.GetInt(target);

            if(GO.transform.childCount > 0)
            {
                if (index < 1)
                {
                    index = 1;
                }
                if (index > GO.transform.childCount)
                {
                    index = GO.transform.childCount;
                }
                index = index - 1;

                childtrans = GO.transform.GetChild(index);
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
        public static new string NAME = "Custom/GameObjects/ActionGetIndexedChild";
        #endif
    }


}
