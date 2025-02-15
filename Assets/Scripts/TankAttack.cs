using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : MonoBehaviour
{
    private PlayerMovement2 player;
    public ParticleSystem meleeCharge;
    public GameObject ChargingCircleUI;
    public GameObject AoEIndicatorCanvas;
    public float moveSpeed;

    //melee 
    public float maxMeleeRadius;
    public float startMeleeDistance;
    public int meleeDamage;
    public float meleeChargeTime;
    public float attackCooldown;

    private float attackTimer;
    private bool isAttacking;
    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }
    }
    private bool growChargeCircle;
    private float chargeCircleTimer;

    //projectile
    public GameObject projectile;
    public float projSpawnDist = 1;
    public float projSpeed = 5;
    public int projPerBurst;
    public float projInterval;
    public float burstFireCooldown;

    private FieldOfView fov;
    private float burstFireTimer;

    //Sound
    public AudioSource chargeUpsfx;
    public AudioSource impactsfx;
    public AudioSource cooldownsfx;
    

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement2>();
        AoEIndicatorCanvas.SetActive(false);
        fov = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);   


            if(dist <= startMeleeDistance && attackTimer <= 0 && !isAttacking)
            {
                StartCoroutine(AttackSequence());
                isAttacking = true;

            }else if((dist > startMeleeDistance || attackTimer > 0) && fov.visibleTargets.Count > 0 && burstFireTimer <=0 && !isAttacking)
            {
                StartCoroutine(BurstFire(player.gameObject));
                burstFireTimer = burstFireCooldown;
            }

            if(dist > startMeleeDistance && fov.visibleTargets.Count > 0 && !isAttacking)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
            }


            if(growChargeCircle && Mathf.Abs(chargeCircleTimer/meleeChargeTime)<1)
            {
                chargeCircleTimer += Time.deltaTime;
                ChargingCircleUI.transform.localScale = 
                    new Vector3(chargeCircleTimer/meleeChargeTime,chargeCircleTimer/meleeChargeTime,chargeCircleTimer/meleeChargeTime);
            }

            attackTimer -= Time.deltaTime;
            burstFireTimer -= Time.deltaTime;
        }
            
    }

    private IEnumerator AttackSequence()
    {
        //do charge up particles and sfx
        ChargeMelee();
        chargeUpsfx.volume = PlayerPrefs.GetFloat("sfxSound",1f) * PlayerPrefs.GetFloat("masterSound",1f);
        chargeUpsfx.Play();
        yield return new WaitForSeconds(meleeChargeTime);
        meleeCharge.Stop();
        AttackPlayer();
    }

    private IEnumerator BurstFire(GameObject target)
    {
        for(int i = 0; i<projPerBurst; i++)
        {
            yield return new WaitForSeconds(projInterval*(i+1));
            Shoot(target);
        }
        
    }

    private void Shoot(GameObject target)
    {
        GameObject g = Instantiate(projectile);
        g.transform.SetParent(transform);
        Vector3 dir = (target.transform.position - transform.position).normalized * projSpawnDist;

        g.transform.position = transform.position + dir;
        g.transform.SetParent(null);
        Rigidbody prb = g.GetComponent<Rigidbody>();
        prb.AddForce(dir.normalized*projSpeed, ForceMode.VelocityChange);
    }
    
    private void AttackPlayer()
    {
        impactsfx.volume = PlayerPrefs.GetFloat("sfxSound",1f) * PlayerPrefs.GetFloat("masterSound",1f);
        impactsfx.Play();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxMeleeRadius);
        foreach(Collider hc in hitColliders){

            Rigidbody rb = hc.GetComponent<Collider>().attachedRigidbody;
            if(rb != null){
                Health h = rb.gameObject.GetComponent<Health>();
                if (h != null && rb.gameObject.layer == 9){
                  h.takeDamage(meleeDamage);
                } 
            }                        
        } 
        
        ResetMelee();
    }

    void ChargeMelee()
    {
        AoEIndicatorCanvas.SetActive(true);
        meleeCharge.Play();
        growChargeCircle = true;
    }

    private void ResetMelee()
    {
        cooldownsfx.volume = PlayerPrefs.GetFloat("sfxSound",1f) * PlayerPrefs.GetFloat("masterSound",1f);
        cooldownsfx.Play();
        growChargeCircle = false;
        chargeCircleTimer = 0;
        attackTimer = attackCooldown;
        isAttacking = false;
        ChargingCircleUI.transform.localScale = new Vector3(0,0,0);
        AoEIndicatorCanvas.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxMeleeRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, startMeleeDistance);
    }


}
