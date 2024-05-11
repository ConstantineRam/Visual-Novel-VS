using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalk : Talk, NullObject
{
    public readonly string Id;
    public IReadOnlyCollection<TalkBit> Enumerate()
    {
        throw new System.NotImplementedException();
    }

    private PlayerTalk(string id)
    {
        Id = id;
    }


    public virtual bool IsNullObject => false;

    public static PlayerTalk Create(string id)
    {
        return new PlayerTalk(id);
    }
}
