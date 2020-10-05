using UnityEngine;

public class GunController : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 3f;

    public float gunBlowback = .5f;
    public float gunReposition = .05f;

    //public GameObject _levelController;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject gunBody;

    public AudioClip fire;
    public AudioSource _gunShots;

    private float nextTimeToFire = 0f;
    //private Vector3 gunPos = (0, 0, -6f);

    public bool menuIsOpen = false;

    void Awake()
    {
        _gunShots = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && !menuIsOpen)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void FixedUpdate()
    {
        if((fpsCam.gameObject.transform.localPosition.z - gunBody.transform.localPosition.z) > -3f)//gun starts at -5f
        {
            //Debug.Log("gunBody - camera z == " + (fpsCam.gameObject.transform.localPosition.z - gunBody.transform.localPosition.z));

            gunBody.transform.localPosition += Vector3.forward * gunReposition;
        }
    }

    void Shoot()
    {
        //play effects
        _gunShots.PlayOneShot(fire, 1f);

        muzzleFlash.Play();
        //make gun move backwards
        gunBody.transform.localPosition -= Vector3.forward * gunBlowback;

        RaycastHit hit;

        //if the shot hit something...
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name + " was shot!");

            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            //make impact particle on object that is hit
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }

    }
}
