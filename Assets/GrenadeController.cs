using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    public GameObject grenadePrefab;    // Reference to the grenade prefab
    public Transform throwPoint;        // Point from which the grenade will be thrown
    public float throwForce = 10f;      // Force applied to throw the grenade

    public void Throw(bool throwRight)
    {
        

        if (grenadePrefab == null || throwPoint == null)
        {
            Debug.Log("GrenadePrefab or ThrowPoint is not assigned.");
            return;
        }

        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);
        Debug.Log("Throwing !");

        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 throwDirection = throwRight ? Vector2.right : Vector2.left;

            rb.AddForce(throwDirection.normalized * throwForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on the grenade prefab.");
        }
    }
}
