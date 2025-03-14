using UnityEngine;

public class Node
{
    public string name;
    public enum State
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public State state;

    public virtual State Evaluate()
    {
        return State.SUCCESS;
    }
}
