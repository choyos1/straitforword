using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    public float Speed = 20f;
    public Vector3 direction = Vector3.up;
    public System.Action<Projectile> destroyed;
    public new BoxCollider2D collider{ get; private set; }
    

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }
     private void IfDestroy()
    {
        if (destroyed != null) {
            destroyed.Invoke(this);
        }
    }
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    private void CheckCollision(Collider2D other)
    {
        Sheild sheilds = other.gameObject.GetComponent<Sheild>();

            Destroy(gameObject);
        }
 
}
