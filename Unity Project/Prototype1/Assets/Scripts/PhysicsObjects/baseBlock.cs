using UnityEngine;

public class baseBlock : MonoBehaviour {

    public float blockHealth;
    public int playerOwner;

    private void Update()
    {
        if (blockHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
