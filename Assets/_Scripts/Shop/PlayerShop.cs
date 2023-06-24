using UnityEngine;
public class PlayerShop : MonoBehaviour
{
    [SerializeField] float _speedMovement;
    [SerializeField] GameObject _shopCanvas;

    Animator _animator;

    System.Action _currentState;
    InputManager _inputManager;
    bool _onShopTrigger;
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

        if (_inputManager.GetButtonDown("Interact") && _onShopTrigger)
        {
            _shopCanvas.SetActive(true);
            _currentState = delegate
            {
                PlayAnimation("Idle");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            };
        }
    }
    void PlayAnimation(string stateName) => _animator.Play(stateName);

    public void SetDefaultState()
    {
        _currentState = DefaultState;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<NextSceneOnTrigger>()) _currentState = delegate { PlayAnimation("Idle"); };
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>()) _onShopTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>()) _onShopTrigger = false;
    }
}
