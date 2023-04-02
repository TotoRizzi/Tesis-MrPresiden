using System.Collections.Generic;
using UnityEngine;
public class PauseManager : MonoBehaviour
{
    bool _paused = false;
    Stack<IScreen> _stack;

    [SerializeField] Transform _mainGame;

    public bool Paused { get { return _paused; } }
    private void Awake()
    {
        _stack = new Stack<IScreen>();
    }
    private void Start()
    {
        Push(new PauseGO(_mainGame));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_paused) Pop();
            else Push(Instantiate(Resources.Load<ScreenPause>("PauseCanvas")));

            TurnPause();
        }
    }
    public void Pop()
    {
        if (_stack.Count <= 1) return;
        _stack.Pop().Free();
        if (_stack.Count > 0) _stack.Peek().Activate();
    }
    public void Push(IScreen screen)
    {
        if (_stack.Count > 0) _stack.Peek().Deactivate();
        _stack.Push(screen);
        screen.Activate();
    }

    public void TurnPause() => _paused = !_paused;

    public void PauseObjectsInCinematic()
    {
        _stack.Peek().PauseObjectsInCinematic();
    }
}
