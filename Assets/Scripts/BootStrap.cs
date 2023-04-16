using UnityEngine;

public class BootStrap
{
    [RuntimeInitializeOnLoadMethod]
    private static void Run()
    {
#if !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Confined;
#endif
    }
}