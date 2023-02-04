using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Cell
{
    public Vector3 Position;
    public bool HasRoot;

    public Cell(Vector3 position, bool hasRoot = false)
    {
        Position = position;
        HasRoot = hasRoot;
    }
}

