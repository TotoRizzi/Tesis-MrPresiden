using UnityEngine;
public class DontDestroyOnLoad : MonoBehaviour
{
    DontDestroyOnLoad Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
