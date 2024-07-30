using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPool : MonoBehaviour
{
    public GameObject particlePrefab; // Reference to the particle system prefab
    public int poolSize = 10; // Number of particle systems in the pool

    private Queue<GameObject> pool;

    void Start()
    {
        // Initialize the pool
        pool = new Queue<GameObject>();

        // Populate the pool with particle systems
        for (int i = 0; i < poolSize; i++)
        {
            GameObject particleObject = Instantiate(particlePrefab);
            particleObject.SetActive(false);
            pool.Enqueue(particleObject);
        }
    }

    // Method to activate a particle system at a specified position
    public void ActivateParticleSystem(Vector3 position)
    {
        if (pool.Count == 0)
        {
            Debug.LogWarning("No particle systems available in the pool.");
            return;
        }

        // Get a particle system from the pool
        GameObject particleObject = pool.Dequeue();
        particleObject.transform.position = position;
        particleObject.SetActive(true);

        // Optional: Destroy the particle system after it's finished playing
        // Assuming the particle system will be inactive when it finishes
        ParticleSystem ps = particleObject.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            StartCoroutine(DeactivateAfterCompletion(ps, particleObject));
        }
        else
        {
            Debug.LogError("No ParticleSystem component found on the prefab.");
        }
    }

    // Coroutine to deactivate the particle system after it's finished playing
    private IEnumerator<WaitForSeconds> DeactivateAfterCompletion(ParticleSystem ps, GameObject particleObject)
    {
        yield return new WaitForSeconds(ps.main.duration);

        particleObject.SetActive(false);
        pool.Enqueue(particleObject);
    }
}
