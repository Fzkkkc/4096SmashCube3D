using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    private Cube _cube;
    private Rigidbody _cubeRigidbody;

    private static readonly int MAX_CUBE_NUMBER = 4096 ;
    private static readonly float EXPLOSION_FORCE = 400f;
    private static readonly float EXPLOSION_RADIUS = 1.5f;
    private static readonly float PUSH_FORCE = 2.5f;

    private void Awake()
    {
        _cube = GetComponent<Cube>();
        _cubeRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Cube otherCube = other.gameObject.GetComponent<Cube>();

        if (otherCube == null || _cube.CubeID <= otherCube.CubeID || _cube.CubeNumber != otherCube.CubeNumber)
        {
            return;
        }

        Vector3 contactPoint = other.contacts[0].point;

        if (otherCube.CubeNumber < MAX_CUBE_NUMBER)
        {
            Cube newCube = CubeSpawner.Instance.Spawn(_cube.CubeNumber * 2, contactPoint + Vector3.up * 1.6f);

            newCube.CubeRigidbody.AddForce(new Vector3(0, 0.3f, 1f) * PUSH_FORCE, ForceMode.Impulse);
            newCube.CubeRigidbody.AddTorque(Vector3.one * Random.Range(-20f, 20f));
        }

        Collider[] surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);

        foreach (Collider collider in surroundedCubes)
        {
            Rigidbody rigidbody = collider.attachedRigidbody;
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(EXPLOSION_FORCE, contactPoint, EXPLOSION_RADIUS);
            }
        }

        FX.Instance.PlayCubeExplosionFX(contactPoint, _cube.CubeColor);

        CubeSpawner.Instance.DestroyCube(_cube);
        CubeSpawner.Instance.DestroyCube(otherCube);
    }
}