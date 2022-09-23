namespace GameCreator.Core
{
    using UnityEngine;
    using System.Linq;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionFindIndexedGameObjectWithName : IAction
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
        [Header("Find Indexed GameObject of those with specified Name/n1 being first.")]
        [Header("Index cannot exceed counting below.")]
        public StringProperty gameObjectsNameToCheck = new StringProperty();

        [Space(10)]
        public NumberProperty indexOfGameObjectWithinSearch = new NumberProperty();

        [Header("Do you want to search for GO Names which/nContain or Equal search criteria?")]
        [Space(10)]
        public ContainsOrEquals GONamesMustEqualOrContain;

        [Header("Do you want to search for GO Names in/nCase Sensitive or Case Insensitive manner?")]
        [Space(10)]
        public CaseSensitive searchCase;

        [Space(10)]
        [Header("This field is for info only.  Count of GOs with given Name.")]
        public int counting;

        [Space(10)]
        [Header("This field is for info only.  /nPotential Name of GO found.")]
        public string found;

        [Space(20)]
        [Header("Indexed (within total count) GameObject found with that Name...  returns a NULL if none found.")]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty indexedGameObjectFound = new VariableProperty();

        private string nameToCheck;
        private string nameToCheck1;
        private int counter;
        private GameObject[] foundGOs;
        private GameObject foundGO;
        private int indexa;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            indexa = indexOfGameObjectWithinSearch.GetInt(target);
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
        public static new string NAME = "Custom/GameObjects/ActionFindIndexedGameObjectWithName";
        #endif
    }
}
