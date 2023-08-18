using UnityEngine;
public class Grid : MonoBehaviour
{
    Node[,] _grid;
    [SerializeField, Tooltip("Always being pair value")] int _width;
    [SerializeField, Tooltip("Always being pair value")] int _height;
    [SerializeField] float _offset;
    [SerializeField] Node _nodePrefab;

    static Grid Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        _grid = new Node[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Node node = Instantiate(_nodePrefab);
                node.transform.SetParent(transform);
                node.name += $"{x}{y}";
                _grid[x, y] = node;
                node.transform.position = new Vector3(x * node.transform.localScale.x * _offset, y * node.transform.localScale.z * _offset, 0);
            }
        }
        transform.position = new Vector3(-(_width / 2) * _offset, -(_height / 2) * _offset, 0);
    }
}
