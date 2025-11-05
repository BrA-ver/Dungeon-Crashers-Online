using UnityEngine;

public class Wizard : Player
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Debug.Log("Wizard Called Awake");
    }
}
