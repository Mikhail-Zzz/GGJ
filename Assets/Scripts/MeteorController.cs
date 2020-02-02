using S = System;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    private S.Random Rand { get; set; } = new S.Random();
    private float TimeForNext { get; set; }

    private const int MIN_SECS_TO_NEXT = 3;
    private const int MAX_SECS_TO_NEXT = 30;

    private const int SHIP_X = 10;
    private const int SHIP_Y = -5;

    // Start is called before the first frame update
    void Start()
    {
        TimeForNext = Rand.Next(MIN_SECS_TO_NEXT, MAX_SECS_TO_NEXT);
    }

    // Update is called once per frame
    void Update()
    {
        TimeForNext -= Time.deltaTime;
        if(TimeForNext <= 0)
        {
            TimeForNext = Rand.Next(MIN_SECS_TO_NEXT, MAX_SECS_TO_NEXT);

            int xl = Rand.Next(-100, -20);
            int xr = Rand.Next(20, 100);

            int yt = Rand.Next(-100, -20);
            int yb = Rand.Next(20, 100);

            int x = Rand.NextDouble() > 0.5 ? xl : xr;
            int y = Rand.NextDouble() > 0.5 ? yt : yb;

            GameObject meteor = Instantiate(
                gameObject,
                new Vector3(x, y),
                new Quaternion()
            );

            meteor.GetComponent<Rigidbody2D>().AddForce(new Vector2(SHIP_X - x, SHIP_Y - y), ForceMode2D.Force);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
