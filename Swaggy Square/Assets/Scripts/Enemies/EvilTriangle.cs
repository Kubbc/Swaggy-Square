using UnityEngine;

public class EvilTriangle : Enemy
{
    protected override void Update()
    {
        base.Update();

        Vector2 direction = transform.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();

        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Damage();
            Debug.Log(collision.gameObject.tag);
        }
    }
}
