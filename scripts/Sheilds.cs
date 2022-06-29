using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Sheild : MonoBehaviour
{
    public BoxCollider2D HitBox { get; private set; }
    private void Awake()
    {
        HitBox = GetComponent<BoxCollider2D>();
    }
    private void OnHITEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            gameObject.SetActive(false);
        }
    }
}