using UnityEngine;

public class FindChild : MonoBehaviour
{
    
    /// <summary>
    ///     finds chilren of parent according to tag of children
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_tag"></param>
    /// <returns> gameobject child according to tag </returns>
    public static GameObject FindGameObjectInChildWithTag(GameObject _parent, string _tag)
    {
        Transform t = _parent.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.tag == _tag)
            {
                return t.GetChild(i).gameObject;
            }

        }

        return null;
    }
}
