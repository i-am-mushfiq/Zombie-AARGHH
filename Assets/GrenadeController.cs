using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    public GameObject grenadePrefab;    // Reference to the grenade prefab
    public Transform throwPoint;        // Point from which the grenade will be thrown
    public float throwForce = 10f;      // Force applied to throw the grenade
    public float throwAngle = 45f;      // Angle at which the grenade will be thrown

    public void Throw(bool throwRight)
    {
        if (grenadePrefab == null || throwPoint == null)
        {
            Debug.Log("GrenadePrefab or ThrowPoint is not assigned.");
            return;
        }

        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);
        //Debug.Log("Throwing!");

        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float radians = throwAngle * Mathf.Deg2Rad;
            Vector2 throwDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            if (!throwRight)
            {
                throwDirection.x *= -1;
            }

            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on the grenade prefab.");
        }
    }
}
