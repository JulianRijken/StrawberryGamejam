using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private WheelMenu wheelMenu;

    private Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.UI.Enable();
    }

    private void Update()
    {
        int wheelInput = Mathf.RoundToInt(controls.UI.SideWaysNavigation.ReadValue<float>());
        Debug.Log(wheelInput);
        wheelMenu.TryRotate(wheelInput);
    }




    private enum MenuOptions
    {
        Play,
        Settings,
        LevelEditor,
        Credits

    }
}
