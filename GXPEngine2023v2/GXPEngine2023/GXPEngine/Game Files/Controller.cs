using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;

public class Controller : GameObject
{
    public virtual void CotrollerInputs()
    { }
    public virtual void Update()
    {
        CotrollerInputs();
    }


}

