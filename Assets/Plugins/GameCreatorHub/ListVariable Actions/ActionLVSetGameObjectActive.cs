namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionLVSetGameObjectActive : IAction
    {
        [Tooltip("Please identify GameObject that contains the ListVariables component.")]
        public GameObjectProperty gameObjectContainingListVariables = new GameObjectProperty();
        [Tooltip("This field is for info only.")]
        public string listVariablesFound = "NO";
        [Tooltip("This field is for info only.")]
        public int listVariablesCount = 0;

        [Space(10)]
        [Tooltip("Set Index within LV of GameObject you want to set Active.")]
        public NumberProperty indexOfGameObjectToSetActive = new NumberProperty();

        [Space(10)]
        [Tooltip("When you set the indexed GO as active, set the others in the ListVariable inactive?")]
        public BoolProperty setOtherLVGameObjectsInactive = new BoolProperty();


        private int index = -1;
        private bool setOthersInactive = true;
        private bool goodToGo = false;
        private GameObject lvGO;
        private GameObject tempGO;
        private int count;
        private Variable variable;

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            goodToGo = false;
            lvGO = gameObjectContainingListVariables.GetValue();
            index = (int)indexOfGameObjectToSetActive.GetValue();
            setOthersInactive = setOtherLVGameObjectsInactive.GetValue();

            if (index < 0)
            {
                index = 0;
            }

            if (lvGO != null)
            {
                if (lvGO.GetComponent<ListVariables>() != null)
                {
                    goodToGo = true;
                    listVariablesFound = "YES";
                }
            }
            if (goodToGo == true)
            {
                count = lvGO.GetComponent<ListVariables>().variables.Count;
                if (count > 0)
                {
                    if (index > count - 1)
                    {
                        index = count - 1;
                    }
                    for (int i = 0; i < count; i++)
                    {
                        variable = VariablesManager.GetListItem(lvGO.GetComponent<ListVariables>(), ListVariables.Position.Index, i);
                        tempGO = variable.Get() as GameObject;
                        if (i == index)
                        {
                            tempGO.SetActive(true);
                        }
                        else
                        {
                            if (setOthersInactive == true)
                            {
                                tempGO.SetActive(false);
                            }
                        }
                    }
                }
            }

            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/ListVariables/ActionLVSetGameObjectActive";
        #endif

        private void OnValidate()
        {
            goodToGo = false;
            lvGO = gameObjectContainingListVariables.GetValue();
            index = (int)indexOfGameObjectToSetActive.GetValue();
            setOthersInactive = setOtherLVGameObjectsInactive.GetValue();

            if (index < 0)
            {
                index = 0;
            }
            if (lvGO != null)
            {
                if (lvGO.GetComponent<ListVariables>() != null)
                {
                    goodToGo = true;
                    listVariablesFound = "YES";
                }
            }
            if (goodToGo == true)
            {
                count = lvGO.GetComponent<ListVariables>().variables.Count;
                if (count > 0)
                {
                    if (index > count -1)
                    {
                        index = count - 1;
                    }
                    for (int i = 0; i < count; i++)
                    {
                        variable = VariablesManager.GetListItem(lvGO.GetComponent<ListVariables>(), ListVariables.Position.Index, i);
                        tempGO = variable.Get() as GameObject;
                        if (i == index)
                        {
                            tempGO.SetActive(true);
                        }
                        else
                        {
                            if (setOthersInactive == true)
                            {
                                tempGO.SetActive(false);
                            }
                        }
                    }
                }
            }
            listVariablesCount = count;
        }
    }
}

