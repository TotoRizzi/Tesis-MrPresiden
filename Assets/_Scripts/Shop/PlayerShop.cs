using UnityEngine;
public class PlayerShop : MonoBehaviour
{
    [SerializeField] float _speedMovement;

    Animator _animator;

    System.Action _currentState;
    InputManager _inputManager;
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _inputManager = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _currentState = DefaultState;
    }
    void Update()
    {
        _currentState();
    }

    void DefaultState()
    {
        Vector3 inputs = new Vector3(_inputManager.GetAxisRaw("Horizontal"), 0, 0);

        transform.position += inputs * _speedMovement * Time.deltaTime;

        PlayAnimation(inputs.magnitude != 0 ? "Run" : "Idle");

        if (inputs.magnitude != 0)
            transform.eulerAngles = inputs.x < 0 ? new Vector3(0, 180, 0) : Vector3.zero;
    }
    void PlayAnimation(string stateName) => _animator.Play(stateName);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<NextSceneOnTrigger>()) _currentState = delegate { PlayAnimation("Idle"); };
    }
}
