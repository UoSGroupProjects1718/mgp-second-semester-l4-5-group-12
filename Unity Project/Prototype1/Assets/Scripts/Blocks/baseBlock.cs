using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class baseBlock : MonoBehaviour {

    public int playerOwner;
    public Sprite normalSprite, damagedSprite;
    [SerializeField] private int blockHealth;

    [HideInInspector] public int currentHealth;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentHealth = blockHealth;
    }

    public void UpdateHealth()
    {
        if (currentHealth == blockHealth)
        {
            sr.sprite = normalSprite;
        }
        else if (currentHealth < blockHealth)
        {
            sr.sprite = damagedSprite;
        }

        if (blockHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
