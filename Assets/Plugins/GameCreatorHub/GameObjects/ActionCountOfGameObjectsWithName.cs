namespace GameCreator.Core
{
    using UnityEngine;
    using System.Linq;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionCountOfGameObjectsWithName : IAction
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

        [Header("Search and count GameObjects with what name?")]
        [Space(10)]
        public StringProperty gameObjectsNameToCheck = new StringProperty();

        [Header("Do you want to search for GO Names which/nContain or Equal search criteria?")]
        [Space(10)]
        public ContainsOrEquals GONamesMustEqualOrContain;

        [Header("Do you want to search for GO Names in/nCase Sensitive or Case Insensitive manner?")]
        [Space(10)]
        public CaseSensitive searchCase;

        [Space(10)]
        [Header("This field is for info only.  Count of GOs with given/containing Name/naccording to search criteria.")]
        public int counting;

        [Space(20)]
        [Header("Count of GameObjects found with that Name.")]
        [VariableFilter(Variable.DataType.Number)]
        public VariableProperty countOfGameObjectsFound = new VariableProperty();

        private string nameToCheck;
        private string nameToCheck1;
        private int counter;
        private GameObject[] foundGOs;
        
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

            this.countOfGameObjectsFound.Set(counter, target);

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
            }
            counting = counter;
        }

#if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionCountOfGameObjectsWithName";
        #endif
    }
}
