using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    public Invader[] PreFab = new Invader[5];
    public AnimationCurve Speed = new AnimationCurve();
    public Vector3 Direction { get; private set; } = Vector3.right;
    public Vector3 StartingSpot { get; private set; }
    public System.Action<Invader> killed;
    public int InvaderDestroyed { get; private set; }
    public int InvadersLeft => TotalInvaders - InvaderDestroyed;
    public int TotalInvaders => Row * Col;
    public float PercentDestroyed => (float)InvaderDestroyed / (float)TotalInvaders;
    public int Row = 5;
    public int Col = 11;
    public Projectile MisslePreFab;
    public float MissleSpawnRate = 1f;

    private void Awake()
    {
        StartingSpot = transform.position;
        for (int i = 0; i < Row; i++)
        {
            float width = 2f * (Col - 1);
            float height = 2f * (Row - 1);
            Vector2 centerOffset = new Vector2(-width * 1f, -height * 1f);
            Vector3 rowLocation = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);
            for (int j = 0; j < Col; j++)
            {
                Invader invader = Instantiate(PreFab[i], transform);
                invader.killed += OnInvaderDestroyed;
                Vector3 position = rowLocation;
                position.x += 2f * j;
                invader.transform.localPosition = position;
            }
        }
    }
    private void Start()
    {
        InvokeRepeating(nameof(MissleDrop), MissleSpawnRate, MissleSpawnRate);
    }
    private void MissleDrop()
    {
        int Straglers = InvadersLeft;

        if (Straglers == 0)
        {
            return;
        }
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (Random.value < (1f / (float)Straglers))
            {
                Instantiate(MisslePreFab, invader.position, Quaternion.identity);
                break;
            }
        }
    }
    private void Update()
    {
        float Speed = this.Speed.Evaluate(PercentDestroyed);
        transform.position += Direction * Speed * Time.deltaTime;
        Vector3 leftSide = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightSide = Camera.main.ViewportToWorldPoint(Vector3.right);
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (Direction == Vector3.right && invader.position.x >= (rightSide.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (Direction == Vector3.left && invader.position.x <= (leftSide.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }
    private void AdvanceRow()
    {
        Direction = new Vector3(-Direction.x, 0f, 0f);
        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }
    private void OnInvaderDestroyed(Invader invader)
    {
        invader.gameObject.SetActive(false);
        InvaderDestroyed++;
        killed(invader);
        this.InvaderDestroyed++;
        if (this.InvaderDestroyed >= this.TotalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ResetInvaders()
    {
        InvaderDestroyed = 0;
        Direction = Vector3.right;
        transform.position = StartingSpot;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }
}
