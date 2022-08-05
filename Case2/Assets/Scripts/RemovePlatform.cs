using UnityEngine;

public class RemovePlatform : MonoBehaviour
{
	/// <summary>
	/// it is called when platforms fall
	/// </summary>
	/// <param name="col"></param>
	private void OnCollisionEnter(Collision col)
	{
		Destroy(col.gameObject);
	}
}
