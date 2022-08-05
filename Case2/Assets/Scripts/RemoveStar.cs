using UnityEngine;

public class RemoveStar : Collactable
{
    public delegate void BoostAction();
    public static event BoostAction StarBoost;

    /// <summary>
    ///     it is called when player collect star  
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter(Collider other)
    {
        StarBoost();
        ParticleController.Instance.PlayStar();
        this.gameObject.SetActive(false);
    }
}
