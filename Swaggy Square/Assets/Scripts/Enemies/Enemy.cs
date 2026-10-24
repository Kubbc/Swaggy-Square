using System.Collections;
using Unity.AI.Navigation.LowLevel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected Transform player;
    [SerializeField]
    protected PlayerController playerController;
    [SerializeField]
    protected GunController gunController;
    [SerializeField]
    protected Rigidbody2D rb;
    [SerializeField]
    protected SpriteRenderer sr;
    [SerializeField]
    protected Light2D enemyLight;
    [SerializeField]
    protected GameObject deathEffect;
    [SerializeField]
    private GameObject explosionSFX;

    [SerializeField]
    protected float spawnDuration;
    [SerializeField]
    protected float DamageDuration;
    [SerializeField]
    protected float damageKnockback;
    [SerializeField]
    protected float knockBackRecoveryDuration;
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float enemyDamageDuration;

    [SerializeField]
    protected float detectionRadius;
    protected bool playerDetected = false;
    protected bool hasSpawned = false;
    protected bool isBeingKnockedBack = false;
    private Coroutine damageCoroutine;

    protected virtual void Start()
    {
        if (!hasSpawned)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            StartCoroutine(Spawn());
        }
    }

    protected virtual void Update()
    {
        if (hasSpawned && !isBeingKnockedBack)
        {
            if (playerDetected)
            {
                var dir = player.position - transform.position;
                rb.linearVelocity = new Vector2(dir.x, dir.y).normalized * speed;
            }
            else
            {
                Collider2D[] colliders = PlayerDetection();

                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Player"))
                    {
                        Debug.Log("Player in range");
                        playerDetected = true;
                    }
                }
            }
        }
    }

    protected Collider2D[] PlayerDetection()
    {
        return Physics2D.OverlapCircleAll(transform.position, detectionRadius);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.1f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }

    public virtual void EnemyDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0f)
        {
            if (deathEffect != null)
            {
                Instantiate(explosionSFX, transform.position, Quaternion.Euler(0f, 0f, 90f));

                Instantiate(deathEffect, transform.position, Quaternion.Euler(0f, 0f, 90f));
            }
            else
            {
                Debug.LogError($"{name} has no deathEffect assigned!", this);
            }

            Destroy(gameObject);
            return;
        }

        StartCoroutine(EnemyUnderDamage());
    }

    protected virtual void Damage()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);

            Time.timeScale = 1f;
            isBeingKnockedBack = false;
        }
        playerController.playerHealth -= damage;
        playerController.playerImmortallity = true;
        damageCoroutine = StartCoroutine(UnderDamage());
        StartCoroutine(PlayerImmortallity());
        Debug.Log("Damage called! Damage amount: " + damage);
    }

    protected virtual IEnumerator Spawn()
    {
        float elapsed = 0f;

        while (elapsed < spawnDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Clamp01(elapsed / spawnDuration);
            float intensity = Mathf.Clamp01(elapsed * 2 / spawnDuration);

            enemyLight.intensity = intensity * 2;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

            yield return null;
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);

        hasSpawned = true;
    }

    protected virtual IEnumerator EnemyUnderDamage()
    {
        sr.color = Color.white;

        yield return new WaitForSeconds(enemyDamageDuration);

        sr.color = Color.red;
    }

    protected virtual IEnumerator UnderDamage()
    {
        Debug.Log("Under damage");

        isBeingKnockedBack = true;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(DamageDuration);

        Time.timeScale = 1f;

        Vector2 dir = (transform.position - player.position).normalized;
        rb.linearVelocity = dir * damageKnockback;

        yield return new WaitForSecondsRealtime(0.2f);

        float elapsed = 0f;

        while (elapsed < knockBackRecoveryDuration)
        {
            elapsed += Time.deltaTime;

            Vector2 dirToPlayer = (player.position - transform.position).normalized;

            Vector2 chaseVelocity = dirToPlayer * speed;

            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, chaseVelocity, elapsed / knockBackRecoveryDuration);

            yield return null;
        }

        isBeingKnockedBack = false;
    }

    protected virtual IEnumerator PlayerImmortallity()
    {
        int i = 0;
        while (i != 4)
        {
            playerController.playerSr.color = Color.white;
            yield return new WaitForSecondsRealtime(playerController.playerImmortallitySeconds / 8);
            playerController.playerSr.color = playerController.standardColor;
            yield return new WaitForSecondsRealtime(playerController.playerImmortallitySeconds / 8);
            i ++;
        }
    }
}