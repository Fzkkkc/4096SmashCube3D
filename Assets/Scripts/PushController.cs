using UnityEngine;

public class PushController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _pushForce;
    [SerializeField] private float _cubeMaxPosX;
    [Space] [SerializeField] private TouchSlider _touchSlider;

    private Cube _mainCube;

    private bool _canMove;
    private bool _isPointerDown;
    private Vector3 _cubePos;

    private void Start()
    {
        SpawnCube();
        _canMove = true;
        AddListeners();
    }

    private void Update()
    {
        if (_isPointerDown)
            SmoothMoveCubeX();
    }

    private void OnPointerDown() => 
        _isPointerDown = true;

    private void OnPointerDrag(float xMovement){
        if(_isPointerDown)
            MoveCubeX(xMovement);
    }

    private void OnPointerUp()
    {
        if (_isPointerDown && _canMove)
        {
            _isPointerDown = false;
            _canMove = false;

            PushCube();

            Invoke("SpawnNewCube", 0.3f);
        }
    }

    private void OnDestroy() =>
        RemoveListeners();

    private void SmoothMoveCubeX()
    {
        _mainCube.transform.position = Vector3.Lerp(
            _mainCube.transform.position,
            _cubePos,
            _moveSpeed * Time.deltaTime);
    }

    private void MoveCubeX(float xMovement)
    {
        _cubePos = _mainCube.transform.position;
        _cubePos.x = xMovement * _cubeMaxPosX;
    }
    
    private void PushCube() => 
        _mainCube.CubeRigidbody.AddForce(
            Vector3.forward*_pushForce,
            ForceMode.Impulse);

    private void SpawnNewCube()
    {
        _mainCube.IsMainCube = false;
        _canMove = true;
        SpawnCube();
    }
    
    private void SpawnCube()
    {
        _mainCube = CubeSpawner.Instance.SpawnRandom();
        _mainCube.IsMainCube = true;
        _cubePos = _mainCube.transform.position;
    }
    
    private void AddListeners()
    {
        _touchSlider.OnPointerDownEvent += OnPointerDown;
        _touchSlider.OnPointerDragEvent += OnPointerDrag;
        _touchSlider.OnPointerUpEvent += OnPointerUp;
    }
    
    private void RemoveListeners()
    {
        _touchSlider.OnPointerDownEvent -= OnPointerDown;
        _touchSlider.OnPointerDragEvent -= OnPointerDrag;
        _touchSlider.OnPointerUpEvent -= OnPointerUp;
    }
}
