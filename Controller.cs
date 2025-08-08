using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{

    private Vector2 dirInput;

    private bool attack;

    public void UpdateInputs()
    {

        dirInput.x = Input.GetAxisRaw("Horizontal");

        dirInput.y = Input.GetAxisRaw("Vertical");

        attack = Input.GetKeyUp(KeyCode.Space);

    }

    public Vector2 getDirInput()
    {
        return dirInput;
    }

    public bool AttackPressed()
    {
        return attack;
    }

    public void setDirInputs(Vector2 dirInput)
    {
        this.dirInput = dirInput;
    }


}
