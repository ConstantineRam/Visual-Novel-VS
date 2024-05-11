using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterTalk : Talk, NullObject
{
    public string Id { get; private set; }

    private readonly List<CharacterTalkBit> _talkBits = new List<CharacterTalkBit>(); 
    public static CharacterTalk Create(string id)
    {
        var result = new CharacterTalk(id);

        return result;
    }

    private protected CharacterTalk(string id)
    {
        Id = id;
    }

    public bool Any()
        => _talkBits.Any();
    public int Count()
        => _talkBits.Count;

    /// <inheritdoc/>
    public IReadOnlyCollection<TalkBit> Enumerate()
        => _talkBits.ToList();
    public CharacterTalk Add(CharacterTalkBit talkBit)
    {
        if (_talkBits.Any(b => b.GetLocalizationId == talkBit.GetLocalizationId)
            && !DuplicateLocalizationIds.IsAllowed(talkBit.GetLocalizationId))
        {
            Debug.LogWarning($"Attempt to add not allowed duplicate {typeof(CharacterTalkBit)} with id {talkBit.GetLocalizationId}.");
            return this;
        }
        
        _talkBits.Add(talkBit);
        return this;
    }

    /// <inheritdoc/>
    public virtual bool IsNullObject => false;
    


}

public class CharacterTalkNullObject : CharacterTalk
{
    public sealed override bool IsNullObject => true;
    
    // ReSharper disable once MemberCanBePrivate.Global
    public const string CharacterTalkNullObjectId = "CharacterTalkNullObject";
    private CharacterTalkNullObject() : base(CharacterTalkNullObjectId) { }

    public static CharacterTalkNullObject Create()
        => new CharacterTalkNullObject();
}