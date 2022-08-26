namespace GameCreator.Core
{
    using UnityEngine;
    using System.Linq;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionFindFirstGameObjectWithTag : IAction
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

        [Header("Find First GameObject of those with specified Tag.")]
        [Space(10)]
        public StringProperty gameObjectsTagToCheck = new StringProperty();

        [Header("Do you want to search for GO Tags which/nContain or Equal search criteria?")]
        [Space(10)]
        public ContainsOrEquals GOTagsMustEqualOrContain;

        [Header("Do you want to search for GO Tags in/nCase Sensitive or Case Insensitive manner?")]
        [Space(10)]
        public CaseSensitive searchCase;

        [Space(10)]
        [Header("This field is for info only.  /nCount of GOs with given/containing Tag.")]
        public int counting;

        [Space(10)]
        [Header("This field is for info only.  /nPotential Name of GO found by Tag.")]
        public string found;

        [Space(20)]
        [Header("First GameObject found with that Tag...  returns a NULL if none found.")]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty firstGameObjectFound = new VariableProperty();

        private string tagToCheck;
        private string tagToCheck1;
        private int counter;
        private GameObject[] foundGOs;
        private GameObject foundGO;

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
            if (counter > 0)
            {
                foundGO = foundGOs[0];
            }
            this.firstGameObjectFound.Set(foundGO, target);

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
                counting = counter;
                found = foundGOs[0].name;
            }
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionFindFirstGameObjectWithTag";
        #endif
    }
}
