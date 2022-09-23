namespace GameCreator.Core
{
    using UnityEngine;
    using System.Linq;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionFindIndexedGameObjectWithTag : IAction
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

        [Space(10)]
        [Header("Find Indexed GameObject of those with specified Tag/n1 being first.")]
        [Header("Index cannot exceed counting below.")]
        public StringProperty gameObjectstagToCheck = new StringProperty();

        [Space(10)]
        public NumberProperty indexOfGameObjectWithinSearch = new NumberProperty();

        [Header("Do you want to search for GO Tags which/nContain or Equal search criteria?")]
        [Space(10)]
        public ContainsOrEquals GOTagsMustEqualOrContain;

        [Header("Do you want to search for GO Tags in/nCase Sensitive or Case Insensitive manner?")]
        [Space(10)]
        public CaseSensitive searchCase;

        [Space(10)]
        [Header("This field is for info only.  Count of GOs with given Tag.")]
        public int counting;

        [Space(10)]
        [Header("This field is for info only.  /nPotential Name of GO found by Tag.")]
        public string found;

        [Space(20)]
        [Header("Indexed (within total count) GameObject found with that Tag...  returns a NULL if none found.")]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty indexedGameObjectFound = new VariableProperty();

        private string tagToCheck;
        private string tagToCheck1;
        private int counter;
        private GameObject[] foundGOs;
        private GameObject foundGO;
        private int indexa;

        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            indexa = indexOfGameObjectWithinSearch.GetInt(target);
            tagToCheck = gameObjectstagToCheck.GetValue(target);
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
                if (indexa < 1)
                {
                    indexa = 1;
                }
                if (indexa > counter)
                {
                    indexa = counter;
                }
                indexa = indexa - 1;
                foundGO = foundGOs[indexa];
            }

            this.indexedGameObjectFound.Set(foundGO, target);

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
            if (counter > 0)
            {
                if (indexa < 1)
                {
                    indexa = 1;
                }
                if (indexa > counter)
                {
                    indexa = counter;
                }
                indexa = indexa - 1;
                foundGO = foundGOs[indexa];
                found = foundGO.name;
            }
        }

#if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionFindIndexedGameObjectWithTag";
#endif
    }
}
