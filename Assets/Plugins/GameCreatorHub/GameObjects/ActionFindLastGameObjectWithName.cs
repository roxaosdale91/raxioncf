namespace GameCreator.Core
{
    using UnityEngine;
    using System.Linq;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionFindLastGameObjectWithName : IAction
    {
        public enum ContainsOrEquals
        {
            Equals = 0,
            Contains = 1
        }

        public enum CaseSensitive
        {
            SearchIsCaseSensitive = 0,
            SearchIsCaseInsensitive = 1
        }

        [Header("Find Last GameObject of those with specified Name.")]
        [Space(10)]
        public StringProperty gameObjectsNameToCheck = new StringProperty();

        [Header("Do you want to search for GO Names which/nContain or Equal search criteria?")]
        [Space(10)]
        public ContainsOrEquals GONamesMustEqualOrContain;

        [Header("Do you want to search for GO Names in/nCase Sensitive or Case Insensitive manner?")]
        [Space(10)]
        public CaseSensitive searchCase;

        [Space(10)]
        [Header("This field is for info only.  /nCount of GOs with given/containing Name.")]
        public int counting;

        [Space(10)]
        [Header("This field is for info only.  /nPotential Name of GO found.")]
        public string found;

        [Space(20)]
        [Header("Last GameObject found with that Name...  returns a NULL if none found.")]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty LastGameObjectFound = new VariableProperty();

        private string nameToCheck;
        private string nameToCheck1;
        private int counter;
        private GameObject[] foundGOs;
        private GameObject foundGO;

        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            nameToCheck = gameObjectsNameToCheck.GetValue(target);
            nameToCheck1 = nameToCheck.ToUpper();

            counter = 0;

            if (GONamesMustEqualOrContain == ContainsOrEquals.Equals)
            {
                if (searchCase == CaseSensitive.SearchIsCaseSensitive)
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.name.Equals(nameToCheck)).ToArray();
                }
                else
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.name.ToUpper().Equals(nameToCheck1)).ToArray();
                }
            }
            else
            {
                if (searchCase == CaseSensitive.SearchIsCaseSensitive)
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.name.Contains(nameToCheck)).ToArray();
                }
                else
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.name.ToUpper().Contains(nameToCheck1)).ToArray();
                }
            }

            if (foundGOs != null)
            {
                counter = foundGOs.Length;
            }
            if (counter > 0)
            {
                counter = counter - 1;
                foundGO = foundGOs[counter];
            }
            this.LastGameObjectFound.Set(foundGO, target);

            return true;
        }

        private void OnValidate ()
        {
            nameToCheck1 = nameToCheck.ToUpper();

            counter = 0;

            if (GONamesMustEqualOrContain == ContainsOrEquals.Equals)
            {
                if (searchCase == CaseSensitive.SearchIsCaseSensitive)
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.name.Equals(nameToCheck)).ToArray();
                }
                else
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.name.ToUpper().Equals(nameToCheck1)).ToArray();
                }
            }
            else
            {
                if (searchCase == CaseSensitive.SearchIsCaseSensitive)
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.name.Contains(nameToCheck)).ToArray();
                }
                else
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.name.ToUpper().Contains(nameToCheck1)).ToArray();
                }
            }

            if (foundGOs != null)
            {
                counter = foundGOs.Length;
                counting = counter;
                found = foundGOs[counter - 1].name;
            }
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionFindLastGameObjectWithName";
        #endif
    }
}
