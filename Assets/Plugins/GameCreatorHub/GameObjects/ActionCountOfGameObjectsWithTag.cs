namespace GameCreator.Core
{
    using UnityEngine;
    using System.Linq;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionCountOfGameObjectsWithTag : IAction
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

        [Header("Search and count GameObjects with what tag?")]
        [Space(10)]
        public StringProperty gameObjectsTagToCheck = new StringProperty();

        [Header("Do you want to search for GO Tags which/nContain or Equal search criteria?")]
        [Space(10)]
        public ContainsOrEquals GOTagsMustEqualOrContain;

        [Header("Do you want to search for GO Tags in/nCase Sensitive or Case Insensitive manner?")]
        [Space(10)]
        public CaseSensitive searchCase;

        [Space(10)]
        [Header("This field is for info only.  Count of GOs with given/containing Tag/naccording to search criteria.")]
        public int counting;

        [Space(20)]
        [Header("Count of GameObjects found with that tag.")]
        [VariableFilter(Variable.DataType.Number)]
        public VariableProperty countOfGameObjectsFound = new VariableProperty();

        private string tagToCheck;
        private string tagToCheck1;
        private int counter;
        private GameObject[] foundGOs;

        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            tagToCheck = gameObjectsTagToCheck.GetValue(target);
            tagToCheck1 = tagToCheck.ToUpper();

            counter = 0;

            if (GOTagsMustEqualOrContain == ContainsOrEquals.Equals)
            {
                if (searchCase == CaseSensitive.SearchIsCaseSensitive)
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.tag.Equals(tagToCheck)).ToArray();
                }
                else
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.tag.ToUpper().Equals(tagToCheck1)).ToArray();
                }
            }
            else
            {
                if (searchCase == CaseSensitive.SearchIsCaseSensitive)
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.tag.Contains(tagToCheck)).ToArray();
                }
                else
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.tag.ToUpper().Contains(tagToCheck1)).ToArray();
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

            tagToCheck1 = tagToCheck.ToUpper();

            counter = 0;

            if (GOTagsMustEqualOrContain == ContainsOrEquals.Equals)
            {
                if (searchCase == CaseSensitive.SearchIsCaseSensitive)
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.tag.Equals(tagToCheck)).ToArray();
                }
                else
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.tag.ToUpper().Equals(tagToCheck1)).ToArray();
                }
            }
            else
            {
                if (searchCase == CaseSensitive.SearchIsCaseSensitive)
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.tag.Contains(tagToCheck)).ToArray();
                }
                else
                {
                    foundGOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.tag.ToUpper().Contains(tagToCheck1)).ToArray();
                }
            }

            if (foundGOs != null)
            {
                counter = foundGOs.Length;
            }
            counting = counter;
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionCountOfGameObjectsWithTag";
        #endif
    }
}

