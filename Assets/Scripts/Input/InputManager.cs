using Fantasy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 输入管理器，负责检测和处理玩家输入
/// </summary>
public class InputManager : MonoBehaviour
{
    private PlayerInputAction inputActions;

    public static InputManager Instance{ get; set; }
    private void Awake()
    {
        inputActions = new();
        Instance = this;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += Move;
        inputActions.Player.Move.canceled += CancelMove;
        inputActions.Player.Attack.performed += Attack;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Move.performed -= Move;
        inputActions.Player.Move.canceled -= CancelMove;
        inputActions.Player.Attack.performed -= Attack;
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        NetManager.Instance.session.Send(new Fantasy.Command()
        {
            commandType=2,
            clientId=NetManager.Instance.Id,
            frameId=FSManager.Instance.frameId,
            x=(int)(direction.x*1000),
            y=0,
            z=(int)(direction.y*1000),
        });
    }

    private void CancelMove(InputAction.CallbackContext context)
    {
        NetManager.Instance.session.Send(new Fantasy.Command()
        {
            commandType = 2,
            clientId = NetManager.Instance.Id,
            frameId = FSManager.Instance.frameId,
            x = 0,
            y = 0,
            z = 0,
        });
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(!Physics.Raycast(ray, out var hitInfo,100 ,1 << LayerMask.NameToLayer("Env")))
        {
            return;
        }
        //Debug.Log(1+" " + hitInfo.point.x + " " + hitInfo.point.y);
        NetManager.Instance.session.Send(new Fantasy.Command()
        {
            commandType = 3,
            clientId = NetManager.Instance.Id,
            frameId = FSManager.Instance.frameId,
            x = (int)(hitInfo.point.x*1000),
            y = (int)(hitInfo.point.z*1000),
        });
    }
}
