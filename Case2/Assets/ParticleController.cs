using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] ParticleSystem coin;
    [SerializeField] ParticleSystem diamond;
    [SerializeField] ParticleSystem starExplosion;
   
    #region Singleton
    public static ParticleController Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    //end singleton

    /// <summary>
    ///     Play gold particle
    /// </summary>
    public void PlayGold()
    {
        coin.Play();
    }

    /// <summary>
    ///     Play diamond particle
    /// </summary>
    public void PlayDiamond()
    {
        diamond.Play();
    }

    /// <summary>
    ///     Play star particle
    /// </summary>
    public void PlayStar()
    {
        starExplosion.Play();
    }
}
