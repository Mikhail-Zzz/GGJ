using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProbeController : MonoBehaviour
{
    private Collider2D ProbeCollider;
    public static GameObject Collided { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        ProbeCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            return;

        Collided = col.gameObject;
        Destroy(Collided);

        gameObject.transform.position = new Vector3(-999, -999);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Collided = null;
    }
}
