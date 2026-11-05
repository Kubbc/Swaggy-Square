using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool playerImmortallity = false;
    public float playerImmortallitySeconds;
    public Color standardColor = new Color(0, 234, 255, 255);

    public float playerHealth;
    private float maxPlayerHealth;
    public bool isPlayerDead = false;

    [SerializeField]
    private float deathDuration;

    [Header("Assignments")]
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject playerDeathEffect;
    [SerializeField]
    private Vector2 recoilVelocity;
    [SerializeField]
    private RawImage vignette;
    public SpriteRenderer playerSr;
    [SerializeField]
    private MenuManager menuManager;

    [Header("Controller")]
    [SerializeField]
    private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(UpStart());

        maxPlayerHealth = playerHealth;

        Debug.Log("PlayerController Loaded Succesfully in: " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerDead) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movementVelocity = new Vector2(moveX, moveY).normalized * speed;
        recoilVelocity = Vector2.Lerp(recoilVelocity, Vector2.zero, 5f * Time.deltaTime);

        rb.linearVelocity = movementVelocity + recoilVelocity;

        float healthLost = 1f - (playerHealth / maxPlayerHealth);

        if (!isPlayerDead)
        {
            float alpha = Mathf.Lerp(0f, 150f / 255f, 1f - (playerHealth / maxPlayerHealth));

            vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
        }

        if (playerHealth <= 0 && !isPlayerDead)
        {
            isPlayerDead = true;
            playerHealth = 0;

            rb.linearVelocity = Vector2.zero;
            Time.timeScale = 0f;

            StartCoroutine(Death());
        }
    }

    public void ApplyRecoil(Vector2 force)
    {
        recoilVelocity += force;
    }

    protected virtual IEnumerator Death()
    {
        float startAlpha = 150f / 255f;
        float elapsed = 0f;

        Debug.Log("Death fade started");

        yield return new WaitForSecondsRealtime(1.5f);
        Instantiate(playerDeathEffect, transform.position, Quaternion.Euler(0f, 0f, 90f));

        while (elapsed < deathDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsed / deathDuration);
            float alpha = Mathf.Lerp(startAlpha, 1f, progress);

            vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);

            yield return null;
        }

        vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, 1f);

        menuManager.menuIsOpen = true;
        menuManager.pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    protected virtual IEnumerator UpStart()
    {
        float startAlpha = 1f;
        float elapsed = 0f;

        Debug.Log("Spawn fade started");

        while (elapsed < deathDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / deathDuration);
            float alpha = Mathf.Lerp(startAlpha, 0f, progress);

            vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);

            yield return null;
        }

        vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, 0f);

        Debug.Log("Spawn fade finished");
    }
}
