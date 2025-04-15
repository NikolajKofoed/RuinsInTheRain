using UnityEngine;

public class LockParticleZ : MonoBehaviour
{
    public float lockZ = 0f;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void LateUpdate()
    {
        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = particles[i].position;
            pos.z = lockZ;
            particles[i].position = pos;
        }

        ps.SetParticles(particles, count);
    }
}
