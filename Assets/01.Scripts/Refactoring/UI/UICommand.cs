using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommand : MonoBehaviour
{
    public void OnAttackButtonClicked()
    {
        CommandManager.Instance.SetCommand(CommandManager.CommandType.Attack);
    }

    public void OnMoveButtonClicked()
    {
        CommandManager.Instance.SetCommand(CommandManager.CommandType.Move);
    }
}
