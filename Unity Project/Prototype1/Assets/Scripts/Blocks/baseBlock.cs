using UnityEngine;

public class baseBlock : MonoBehaviour {

    public int playerOwner;
    public Sprite[] blockSprites;

    [HideInInspector] public int blockHealth = 3;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (blockHealth == 3)
        {
            sr.sprite = blockSprites[0];
        }
        else if (blockHealth == 2)
        {
            sr.sprite = blockSprites[1];
        }
        else if (blockHealth == 1)
        {
            sr.sprite = blockSprites[2];
        }

        if (blockHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
