using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class StoryBit : NullObject
{
    public string Id { get; private set; }

    private readonly List<CharacterTalk> _characterTalks = new List<CharacterTalk>();
    public virtual bool IsNullObject => false;
    /// <summary>
    /// If this story bit is marked initial. It means this is the starting bit for a story.
    /// It should have only one
    /// and
    /// will rise an Exception of no other story bits in a story set pointing into it.
    /// Only one Initial story is allowed per Story Set and one is required.
    /// </summary>
    /// <value>
    /// <c>true</c> if the property IsInitial is set; otherwise, <c>false</c>.
    /// </value>
    public bool IsInitial { get; private set; }

    public static StoryBit Create(in string id)
    {
        return new StoryBit(id);
    }

    private protected StoryBit(in string id)
    {
        Id = id.ToString();
    }

    /// <summary>
    /// Marks the current StoryBit as the initial StoryBit.
    /// Check IsInitial for more information.
    /// </summary>
    /// <returns>
    /// The current StoryBit instance.
    /// </returns>
    public StoryBit MarkInitial()
    {
        IsInitial = true;
        return this;
    }

    public StoryBit Add(CharacterTalk characterTalk)
    {

        _characterTalks.Add(characterTalk);
        return this;
    }

    public int Count<T>() where T : Talk
    {
        if (typeof(T) == typeof(CharacterTalk))
        {
            return _characterTalks.Count;
        }

        Debug.LogError($"Wrong {typeof(Talk)} was requested {typeof(T)}.");
        return 0;
    }

    public CharacterTalk GetCharacterTalk(string id)
    {
        if (id == null)
        {
            return CharacterTalkNullObject.Create();
        }

        return _characterTalks.FirstOrDefault(t => t.Id == id) ?? CharacterTalkNullObject.Create();
    }
}

public class StoryBitNullObject : StoryBit
{
    public const string NullObjectId = "StoryBitNullObjectId";
    
    public static StoryBit Create()
        => new StoryBitNullObject();

    public sealed override bool IsNullObject => true;

    private StoryBitNullObject() : base(NullObjectId)
    {
    }
} 