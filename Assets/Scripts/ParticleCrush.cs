using UnityEngine;
using System.Collections;

public class ParticleCrush : MonoBehaviour {
	public ParticleSystem[] particle;
	public AudioClip[] clips;
	public AudioSource audi;

	public void Play(Color color)
	{
		audi.clip = clips[Random.Range(0, clips.Length)];
		audi.Play ();
		for(int i = 0; i < particle.Length; i++)
		{
			particle [i].startColor = color;
		}
		for(int i = 0; i < particle.Length; i++)
		{
			particle [i].Play ();
		}
	}
}
