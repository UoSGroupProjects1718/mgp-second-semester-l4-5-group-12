using System.Collections;
using UnityEngine;

/*
 * Shooting Script
 * 
 * This is where we handle the shooting of projectiles, at the moment we only shoot one projectile but in future we can have multiple to chose from.
 * 
 * The way this script works is quite simple, and can be improved later if needed.
 * The script is waiting for the 'GetMouseButtonUp' call, whenever we release the left mouse button the script translates the viewport's coordinates to 3D worldspace;
 * eventhough we are making a 2D we need to translate it to 3D worldspace because UNITY still uses the 'Z' axis when we are working in 2D (unless we use Vector2).
 * We then create a projectile on top of the player, and we shoot it in the direction of where the player has clicked. The shooting happens right after releasing the mouse button.
 * 
 * This is a little cheaty way of doing this, because we always assume that the camera will center on the object we are shooting from;
 * in the even of the camera being off-center we can receive some weird effects on the shooting mechanic.
 * Later in the development I will implement a function that will always focus on the object that we use to shoot with, this will help the player in aiming and make things easier.
 * 
 * At the moment the further away from the object the player aims the bigger will be the force applied to the rigidbody,
 * this probably needs to be fixed but at the moment I don't know how to fix it so I will probably need to ask Chris about it.
 * 
 * TODO:
 *  - Choose how much force should be applied to the projectile.
 *  - Have some aiming assitance (show where the player is aiming).
 *  - Make the player lock in the aim before shooting.
 * 
 * IDEAS:
 *  - Show where the object will go by shooting a see-through projectile (helps aiming).
 *  
 */

[RequireComponent(typeof(PlayerController))]
public class ShootingScript : MonoBehaviour
{
    [Header("Shooting settings.")]
    [Tooltip("This is the prefab of the projectile that player will shoot.")]
    [SerializeField] private GameObject projectilePrefab;
    [Tooltip("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer tincidunt.")]
    [SerializeField] private GameObject aimPrefab;
    [Tooltip("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer tincidunt.")]
    [SerializeField] private GameObject dummyProjectilePrefab;
    [Tooltip("This is the value that is used to multiply the projectile's force by (Higher number = Higher Force).")]
    [SerializeField] private float shootForceMultiplier;

    private GameObject crosshairInstance;

    private Vector3 mousePosition;
    private Vector3 screenPos;
    private Vector3 worldPos;
    private int playerNumber;
    private int currentRound;
    private bool canShoot;
    private bool aimLocked;


    private void Start()
    {
        playerNumber = GetComponent<PlayerController>().playerNumber;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && aimLocked
           && playerNumber == currentRound && canShoot)
        {
            SpawnProjectile(screenPos);
        }

        if (Input.GetMouseButtonDown(0))
        {
            // We do this here, so we don't have to do it each frame.
            currentRound = GameManager.GMInstance.playerTurn;
            canShoot = GameManager.GMInstance.canShoot;

            if (playerNumber == currentRound && canShoot)
            {
                mousePosition   = Input.mousePosition;
                screenPos       = Camera.main.ScreenToWorldPoint(mousePosition);
                screenPos.z     = 0.0f;

                LockAim(screenPos);
            }
        }
    }

    //GetMouseButtonDown(0)
    //GetKeyDown(KeyCode.Space)

    private void LockAim(Vector3 _aimPosition)
    {
        worldPos = _aimPosition - transform.position;

        if (!crosshairInstance)
        {
            crosshairInstance = Instantiate(aimPrefab, _aimPosition, Quaternion.identity);
        }
        else
        {
            Destroy(crosshairInstance);

            crosshairInstance = Instantiate(aimPrefab, _aimPosition, Quaternion.identity);
        }

        GameManager.GMInstance.shootingAim = crosshairInstance;
        aimLocked = true;
    }

    private void SpawnProjectile(Vector3 _shootPosition) 
    {
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D newProjectileRB = newProjectile.GetComponent<Rigidbody2D>();
        baseProjectile newProjectileStats = newProjectile.GetComponent<baseProjectile>();

        //worldPos = _shootPosition - transform.position;

        newProjectileStats.playerOwner = playerNumber;
        newProjectileRB.AddForce(worldPos.normalized * shootForceMultiplier * 1000);

        Destroy(crosshairInstance);
        aimLocked = false;

        GameManager.GMInstance.ChangeTurn();
    }
}
