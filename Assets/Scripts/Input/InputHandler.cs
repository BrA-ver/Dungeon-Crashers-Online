using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    [field: SerializeField] public Vector2 MoveInput { get; private set; }
    [field: SerializeField] public Vector2 LookInput { get; private set; }

    public event Action onAttackPress;
    public event Action onJumpPress;

    int sceneIndex;
    bool ignoreInput = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.activeSceneChanged += OmSceneChange;
    }

    private void OnEnable()
    {
        Scene cuurentScene = SceneManager.GetActiveScene();
        //if (cuurentScene.buildIndex == 0)
        //{
        //    Cursor.visible = true;
        //    Cursor.lockState = CursorLockMode.None;
        //    ignoreInput = true;
        //}
        //else
        //{
        //    Cursor.visible = false;
        //    Cursor.lockState = CursorLockMode.Locked;
        //    ignoreInput = false;
        //}
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OmSceneChange;
    }

    private void OmSceneChange(Scene scene0, Scene scene1)
    {
        //if (scene1.buildIndex == 0)
        //{
        //    Cursor.visible = true;
        //    Cursor.lockState = CursorLockMode.None;
        //    ignoreInput = true;
        //}
        //else
        //{
        //    Cursor.visible = false;
        //    Cursor.lockState = CursorLockMode.Locked;
        //    ignoreInput = false;
        //}

        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (ignoreInput) return;

        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (ignoreInput) return;

        LookInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (ignoreInput) return;

        if (context.performed)
            onAttackPress?.Invoke();
    }

    public void OnJumpPress(InputAction.CallbackContext context)
    {
        if (ignoreInput) return;

        if (context.performed)
            onJumpPress?.Invoke();
    }
}
