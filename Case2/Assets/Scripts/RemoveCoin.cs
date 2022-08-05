using UnityEngine;

public class RemoveCoin : Collactable
{

    public delegate void CoinState();
    public static event CoinState GoldUpdate;

    /// <summary>
    ///     it is called when player collect coin  
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter(Collider other)
    {
        GoldUpdate();
        ParticleController.Instance.PlayGold();
        this.gameObject.SetActive(false);
    }
}
