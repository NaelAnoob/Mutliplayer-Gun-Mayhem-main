using System;
using System.Collections;
using UnityEngine;
[System.Serializable]
public class WeaponUsage : ItemUsage
{
    public enum FireMode { Auto, Burst, SemiAuto}
    public FireMode fireMode = FireMode.Auto;
    [Header("Ammo")]
    public int mags;
    public int ammoPerMag;
    public int ammo;
    public float reloadTime = 1f;
    public float spreadAngle = 5f;
    [Header("Burst")]
    public int burstCount = 3; // Number of shots in burst mode
    public float burstDelay = 0.1f; // Time between burst shots
    public float timeBetweenEachBurst = 0.1f;
    [Header("Auto Fire")]
    public float autoFireDelay;
    public GameObject gunObject;
    public Transform gunBarrel;
    [HideInInspector]
    public PlayerStateMachine player;
    public SpawnManager.ObjectType bulletType;
    private bool canShoot = true;
    public override void UseItem()
    {
        if (ammo > 0 && canShoot)
        {
            if (fireMode == FireMode.Auto)
            {
                // Handle autofire
                player.StartCoroutine(AutoFire());
            }
            else if (fireMode == FireMode.Burst)
            {
                // Handle burst fire
                player.StartCoroutine(BurstFire());
            }
            else if(fireMode == FireMode.SemiAuto)
            {
                player.StartCoroutine(BurstFire(true));
            }
        }
        if (ammo <= 0 && canShoot)
        {
            player.StartCoroutine(Reload());
            Debug.Log(canShoot);
        }
    }

    private IEnumerator AutoFire()
    {
        // Fires continuously while holding the fire button and there is ammo
        while (Input.GetKey(KeyCode.Mouse0) && ammo > 0 && canShoot)
        {
            ammo--;
            Fire(); // Call your firing logic
            canShoot = false;
            yield return new WaitForSeconds(autoFireDelay); // Delay between shots
            canShoot = true;
        }
        if (ammo <= 0)
        {
            player.StartCoroutine(Reload());
        }
    }
    private IEnumerator BurstFire(bool isContinous = false)
    {
        int shotsFired = 0;

        while (Input.GetKey(KeyCode.Mouse0) && shotsFired < burstCount && ammo > 0)
        {
            ammo--;
            Fire();
            shotsFired++;
            canShoot = false;
            yield return new WaitForSeconds(burstDelay);
        }

        if (ammo <= 0)
        {
            yield return player.StartCoroutine(Reload());
            canShoot = false;
        }
        if(!isContinous)
        {
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
        }
        yield return new WaitForSeconds(timeBetweenEachBurst);
        canShoot = true;
    }


    //private IEnumerator BurstFire()
    //{
    //    int shotsFired = 0;
    //    while (Input.GetKey(KeyCode.Mouse0) && shotsFired < burstCount && ammo > 0)
    //    {
    //        ammo--;
    //        Fire(); // Call your firing logic
    //        shotsFired++;
    //        canShoot = false;
    //        yield return new WaitForSeconds(burstDelay); // Delay between burst shots
    //        canShoot = true;
    //    }
    //    if(ammo <= 0)
    //    {
    //        canShoot = false;
    //        player.StartCoroutine(Reload());
    //    }
    //    if (Input.GetKeyUp(KeyCode.Mouse0)) 
    //    {
    //        canShoot = true;
    //    }
    //    canShoot = false;
    //}
    public override void InitializeItem(IItem item)
    {

    }

    public override bool SetItem(PlayerStateMachine entity, out ItemUsage usage)
    {
        ItemUsage newUsage = null;
        GameObject weaponObject = GameObject.Instantiate(gunObject, entity.WeaponHolder);
        var item = weaponObject.GetComponent<IItem>();
        WeaponUsage playerUsage = (WeaponUsage)item.usage;
        playerUsage.player = entity;
        newUsage = item.usage = item.usage.CreateInstance(item);
        usage = newUsage;
        return true;
    }
    private void Fire()
    {
        SpawnBullet();
    }
    public void SpawnBullet()
    {
        float randomSpread = UnityEngine.Random.Range(-spreadAngle, spreadAngle);
        
        if (gunBarrel.lossyScale.x < 0)
        {
            Vector2 direction = Vector2.left;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion rotation = Quaternion.Euler(0,0,angle + randomSpread);
            SpawnManager.Instance.RequestSpawn(bulletType, gunBarrel.transform.position, rotation);
        }
        else
        {
            Vector2 direction = Vector2.right;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion rotation = Quaternion.Euler(0,0,angle + randomSpread);
            SpawnManager.Instance.RequestSpawn(bulletType, gunBarrel.transform.position, rotation);
        }
    }
    public IEnumerator Reload()
    {
        if (mags > 0)
        {
            canShoot = false;
            yield return new WaitForSeconds(reloadTime);
            mags--;
            ammo = ammoPerMag;
            canShoot = true;
        }
    }
    public void Initialise(PlayerStateMachine entity)
    {
        player = entity;
        MonoBehaviour.Instantiate(gunObject, entity.WeaponHolder);
    }
    public void UpdateUI()
    {
        //UI update
    }
    IEnumerator WaitGunReset(float nextFireTime)
    {
        canShoot = false;
        yield return new WaitForSeconds(nextFireTime);
        canShoot = true;
    }
}
