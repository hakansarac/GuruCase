using UnityEngine;

public class RemoveDiamond : Collactable
{

    public delegate void DiamondState();
    public static event DiamondState DiamondUpdate;

    /// <summary>
    ///     it is called when player collect diamond  
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter(Collider other)
    {
        DiamondUpdate();
        ParticleController.Instance.PlayDiamond();
        this.gameObject.SetActive(false);
    }
}
