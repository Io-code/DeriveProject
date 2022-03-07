using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SpawnerManager : MonoBehaviour
{
    public Animator animations;
    public GameObject[] weapons;

    public int selectedWeapon = 0;
    
    private GameObject hand;
    private GameObject prefab;
    private bool isDetecting;
    private GameObject player;
    
    private GameObject weaponHolder;


    private void Start()
    {
        SpawnRandomWeapon();
        StartCoroutine(RerollWeapons());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerInput>())
        {
            weaponHolder = other.GetComponentInChildren<Animator>().gameObject;
            isDetecting = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInput>())
        {
            weaponHolder = null;
            isDetecting = false;
        }
    }

    private void Update()
    {
        prefab.transform.Rotate(new Vector3(0f, 1, 0f) * Time.deltaTime * 100);
        if (weaponHolder != null)
        {
            PickupWeapon();
        }
    }

    private void SpawnRandomWeapon()
    {
        int test = Random.Range(0, weapons.Length);
        prefab = weapons[test];
        prefab = Instantiate(prefab, transform.position + Vector3.up*0.8f, Quaternion.identity);
    }
    
    private IEnumerator RerollWeapons()
    {
        yield return new WaitForSeconds(5);
        Destroy(prefab);
        SpawnRandomWeapon();
        StartCoroutine(RerollWeapons());
    }

    void PickupWeapon()
    {
        if (isDetecting && player.GetComponent<PlayerController>().interacting)
        {
            foreach (Transform weapon in weaponHolder.transform)
            {
                if (weapon.gameObject.name == prefab.name.Replace("(Clone)", ""))
                {
                    weapon.gameObject.SetActive(true);
                    weaponHolder.GetComponent<Animator>().SetBool(weapon.name, true);
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                    weaponHolder.GetComponent<Animator>().SetBool(weapon.name, false);
                }
            }
        }

        player.GetComponent<PlayerController>().interacting = false;
    }
}
