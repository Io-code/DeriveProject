using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects_Weapon1")]
public class WeaponScriptableObject : ScriptableObject
{
	public float power = 1;
	public float cooldown = 1;
	
	public GameObject prefab; 
	
	//Put AudioSource on prefabs
	public AudioClip attackSound;
	public ParticleSystem attackParticles;

	// Range depends on weapon prefab
	// Hitzone depends on weapon prefab
	
}
