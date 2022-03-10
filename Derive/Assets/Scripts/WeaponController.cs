using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

	[SerializeField] private PlayerController playerController;

	public float power = 0;
	public float cooldown = 0;
	private GameObject prefab;
	private GameObject weapon;

	//Put AudioSource on prefabs
	public AudioClip attackSound;
	public ParticleSystem attackParticles;

	[SerializeField] private WeaponScriptableObject weaponSO;

	private void Update()
	{
		
	}

	private void OnEnable()
	{
		prefab = weaponSO.prefab;
		power = weaponSO.power;
		cooldown = weaponSO.cooldown;
	}


}
