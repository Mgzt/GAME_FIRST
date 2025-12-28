using UnityEngine;

public abstract class ToolAction : MonoBehaviour
{
    public abstract ToolType ToolType { get; }
    public abstract void Use();
}
