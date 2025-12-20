using UnityEngine;

public class FogObject : MonoBehaviour
{
    ParticleSystem particles;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = particles.main.startColor.color;
        color = Color.white;
    }
}
