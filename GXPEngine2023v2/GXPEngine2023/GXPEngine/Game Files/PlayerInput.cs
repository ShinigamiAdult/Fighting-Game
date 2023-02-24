using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PlayerInput
{
    int key;
    int timeout = 10;

    public PlayerInput(int i)
    {
        key = i;
    }
    public void Update()
    {
        if(timeout > 0)
            timeout--;
    }

    public int GetTime()
    {
        return timeout;
    }

    public void TurnOff()
    {
        timeout = 0;
    }
    public int GetKey()
    {
        return key;
    }
}
