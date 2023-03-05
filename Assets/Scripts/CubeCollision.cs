using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    private Cube _cube;

    private void Awake() => 
        _cube = GetComponent<Cube>();

    private void OnCollisionEnter(Collision other)
    {
        Cube otherCube = other.gameObject.GetComponent<Cube>();

        if (otherCube != null && _cube.CubeID > otherCube.CubeID)
        {
            if (_cube.CubeNumber == otherCube.CubeNumber)
            {
                Vector3 contactPoint = other.contacts[0].point;

                if (otherCube.CubeNumber < CubeSpawner.Instance.MaxCubeNumber)
                {
                    Cube newCube = CubeSpawner.Instance.Spawn(
                        _cube.CubeNumber * 2,
                        contactPoint + Vector3.up * 1.6f);

                    float pushForce = 2.5f;
                    newCube.CubeRigidbody.AddForce(
                        new Vector3(0, 0.3f, 1f) * 
                        pushForce, ForceMode.Impulse);

                    float randomValue = Random.Range(-20f, 20f);
                    Vector3 randomDirection = Vector3.one * randomValue;
                    newCube.CubeRigidbody.AddTorque(randomDirection);
                }

                Collider[] surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);
                float explosionForce = 400f;
                float explosionRadius = 1.5f;

                foreach (Collider collider in surroundedCubes)
                {
                    if (collider.attachedRigidbody != null)
                        collider.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
                }
                
                FX.Instance.PlayCubeExplosionFX(contactPoint, _cube.CubeColor);
                
                CubeSpawner.Instance.DestroyCube(_cube);
                CubeSpawner.Instance.DestroyCube(otherCube);
            }
        }
    }
}
