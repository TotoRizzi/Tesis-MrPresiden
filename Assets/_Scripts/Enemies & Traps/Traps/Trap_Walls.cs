using UnityEngine;
public class Trap_Walls : MonoBehaviour
{
    enum WallsStates { GO, BACK, WAIT_TOP, WAIT_BASE }
    Vector3 _initialPos, _targetPos;
    EventFSM<WallsStates> _myFsm;
    Collider2D _coll;

    [SerializeField] Trap_Walls _otherWall;

    [Header("VARIABLES")]
    [SerializeField] float _waitBaseTime;
    [SerializeField] float _waitTopTime;
    [SerializeField] float _goTopTime;
    [SerializeField] float _goBaseTime;
    private void Start()
    {
        _coll = GetComponent<Collider2D>();
        _coll.enabled = false;
        _initialPos = transform.position;
        Vector3 posDif = _otherWall.transform.position - transform.position - (_otherWall.transform.position - transform.position) * .5f;
        _targetPos = transform.position + posDif;

        var go = new State<WallsStates>("GO");
        var back = new State<WallsStates>("BACK");
        var wait_top = new State<WallsStates>("WAIT_TOP");
        var wait_base = new State<WallsStates>("WAIT_BASE");

        StateConfigurer.Create(wait_base).SetTransition(WallsStates.GO, go).Done();
        StateConfigurer.Create(go).SetTransition(WallsStates.WAIT_TOP, wait_top).Done();
        StateConfigurer.Create(wait_top).SetTransition(WallsStates.BACK, back).Done();
        StateConfigurer.Create(back).SetTransition(WallsStates.WAIT_BASE, wait_base).Done();

        float timer = 0;

        #region WAIT_BASE

        wait_base.OnEnter += x => timer = 0;

        wait_base.OnUpdate += () =>
        {
            timer += Time.deltaTime;
            if (timer >= _waitBaseTime) _myFsm.SendInput(WallsStates.GO);
        };

        #endregion

        #region GO

        go.OnEnter += x => timer = 0;

        go.OnUpdate += () =>
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(_initialPos, _targetPos, timer / _goTopTime);

            if (timer / _goTopTime >= 1) _myFsm.SendInput(WallsStates.WAIT_TOP);
        };

        #endregion

        #region WAIT_TOP

        wait_top.OnEnter += x =>
        {
            timer = 0;
            _coll.enabled = true;
        };

        wait_top.OnUpdate += () =>
        {
            timer += Time.deltaTime;
            if (timer >= _waitTopTime) _myFsm.SendInput(WallsStates.BACK);
        };

        wait_top.OnExit += x => _coll.enabled = false;

        #endregion

        #region BACK

        back.OnEnter += x => timer = 0;

        back.OnUpdate += () =>
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(_targetPos, _initialPos, timer / _goBaseTime);
            if (timer / _goBaseTime >= 1) _myFsm.SendInput(WallsStates.WAIT_BASE);
        };

        #endregion

        _myFsm = new EventFSM<WallsStates>(wait_base);
    }

    private void Update()
    {
        _myFsm.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Helpers.GameManager.PlayerDead();
            Debug.Log("Player");
        }
    }
}
