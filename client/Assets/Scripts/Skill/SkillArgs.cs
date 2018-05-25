using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArgs
{
    public ChessEntity Owner;
    public ChessEntity Sender;

    public int FlowIndex;
	public int Index;

	public void OnReset()
    {
        Owner = null;
        Sender = null;
        FlowIndex = -1;
		Index = -1;
	}
}