namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionFindClosestGameObjectWithTag : IAction
    {
        [Header("Select your GameObject.")]
        [Space(10)]
        public GameObjectProperty yourGameObject = new GameObjectProperty();

        [Header("Provide string of Tag to look for closest/nGameObject with that Tag.")]
        [Space(10)]
        public StringProperty tagToLookFor = new StringProperty();

        [Space(10)]
        [Header("This field is for info only.  Gives Name of closest/GameObject with given tag.")]
        public string nameOfClosestGameObjectWithTag;

        [Space(20)]
        [Header("Closest GameObject to yours found with that Tag.../nreturns a NULL if none found.")]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty closestGameObjectWithTagFound = new VariableProperty();

        private GameObject yourGO;
        private string nameOfClosest;
        private GameObject foundGO;
        private GameObject[] foundGOs;
        private string tagToCheck;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
            tagToCheck = tagToLookFor.GetValue(target);
            yourGO = yourGameObject.GetValue(target);

            if (yourGO != null && tagToCheck != null)
            {
                foundGOs = GameObject.FindGameObjectsWithTag(tagToCheck);
                foundGO = null;
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;
                foreach (GameObject go in foundGOs)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        foundGO = go;
                        distance = curDistance;
                    }
                }
            }
            
            this.closestGameObjectWithTagFound.Set(foundGO, target);

            return true;
        }

        private void OnValidate ()
        {


            if (yourGO != null && tagToCheck != null)
            {
                foundGOs = GameObject.FindGameObjectsWithTag(tagToCheck);
                foundGO = null;
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;
                foreach (GameObject go in foundGOs)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        foundGO = go;
                        distance = curDistance;
                        nameOfClosest = foundGO.name;
                    }
                }
            }
        }

        #if UNITY_EDITOR
        public static new string NAME = "Custom/GameObjects/ActionFindClosestGameObjectWithTag";
        #endif
    }
}
