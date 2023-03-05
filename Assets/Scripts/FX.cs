using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _cubeExplosionFX;
    private ParticleSystem.MainModule _cubeExplosionFXMainModule;
    public static FX Instance;

    private void Awake() =>
        Instance = this;

    private void Start()
    {
        _cubeExplosionFXMainModule = _cubeExplosionFX.main;
    }

    public void PlayCubeExplosionFX(Vector3 position, Color color)
    {
        _cubeExplosionFXMainModule.startColor = new ParticleSystem.MinMaxGradient(color);
        _cubeExplosionFX.transform.position = position;
        _cubeExplosionFX.Play();
    }
}
