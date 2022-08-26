namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionGetGameObjectDepthInHierarchy : IAction
    {
        [Header("Select GameObject to query depth of in any hierarchy.")]
        [Header("0 = no hierarchy, 1 = Root GO, 2 = Child GO,/n3 = Grandchild GO, 4 = GreatGrandChild GO etc.")]
        [Space(10)]
        public GameObjectProperty gameObjectToCheck = new GameObjectProperty();

        [VariableFilter(Variable.DataType.Number)]
        public VariableProperty foundGameObjectDepth = new VariableProperty();

        private GameObject GO;
        private int depth;
        private bool hasHierarchy;

        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {

            GO = gameObjectToCheck.GetValue(target);
            hasHierarchy = false;
            depth = 0;

            if (GO.transform.childCount > 0)
            {
                hasHierarchy = true;
            }

            if (hasHierarchy == true)
            {
                if(GO.transform.parent == null)
                {
                    depth = 1;
                }

                if(GO.transform.parent != null)
                {
                    if (GO.transform.parent.parent == null)
                    {
                        depth = 2;
                    }
                    if (GO.transform.parent.parent != null && GO.transform.parent.parent.parent == null)
                    {
                        depth = 3;
                    }
                    if (GO.transform.parent.parent.parent != null && GO.transform.parent.parent.parent.parent == null)
                    {
                        depth = 4;
                    }
                    if (GO.transform.parent.parent.parent.parent != null && GO.transform.parent.parent.parent.parent.parent == null)
                    {
                        depth = 5;
                    }
                    if (GO.transform.parent.parent.parent.parent.parent != null && GO.transform.parent.parent.parent.parent.parent.parent == null)
                    {
                        depth = 6;
                    }
                }
            }
            

            this.foundGameObjectDepth.Set(depth, target);

            return true;
        }


        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionGetGameObjectDepthInHierarchy";
        #endif
    }
}
