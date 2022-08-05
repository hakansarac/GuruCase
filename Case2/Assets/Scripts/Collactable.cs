using UnityEngine;

abstract public class Collactable : MonoBehaviour
{
    public abstract void OnTriggerEnter(Collider other);
}
