using System.Collections;
using UnityEngine;

public class HarmlessCircle : Enemy
{
    [SerializeField]
    private float pauseTimeForTarget;
    private Vector2 targetLocation;
    private bool isWaitingForTarget;

    protected override void Start()
    {
        base.Start();
        ChooseNewTarget();
    }

    private void ChooseNewTarget()
    {
        targetLocation = new Vector2(Random.Range(-19f, 19f), Random.Range(-19f, 19f));
    }

    protected override void Update()
    {
        Vector2 direction = targetLocation - rb.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    private void FixedUpdate()
    {
        if (isWaitingForTarget) return;

        Vector2 nextPosition = Vector2.MoveTowards(rb.position, targetLocation, speed * Time.fixedDeltaTime);

        rb.MovePosition(nextPosition);

        if (Vector2.Distance(nextPosition, targetLocation) < 0.01f)
        {
            StartCoroutine(WaitForNewTarget());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Damage();
            Debug.Log(collision.gameObject.tag);
        }
    }

    private IEnumerator WaitForNewTarget()
    {
        isWaitingForTarget = true;

        yield return new WaitForSeconds(pauseTimeForTarget);

        ChooseNewTarget();
        isWaitingForTarget = false;
    }
}