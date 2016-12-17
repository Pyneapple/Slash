using UnityEngine;
using System.Collections;

public class Slash : MonoBehaviour {
	ParticleSystem particleSlash;
	ParticleSystem particle
	{
		get{
			if(particleSlash == null)
			{
				particleSlash = GetComponent <ParticleSystem> ();
			}
			return particleSlash;
		}
	}
	
	public void StartSlash(Transform parent)
	{
		transform.SetParent (parent);
	}

	public void SetColor(Color colo)
	{
		particle.startColor = colo;
	}

	public void StopSlash()
	{
		transform.SetParent (null);
		Invoke("Despawn", particle.startLifetime);
	}

	void Despawn()
	{
		SmartPool.Despawn (gameObject);
	}
		
}
