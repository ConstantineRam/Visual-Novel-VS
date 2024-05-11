using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class StorySet
{
    public string Id { get; private set; }

    private readonly Dictionary<string, StoryBit> _bits = new Dictionary<string, StoryBit>(); 
    public static StorySet Create(in Guid id)
    {
        return new StorySet(id);
    }

    public const int AllowedInitialStoryBits = 1;
    private StorySet(in Guid id)
    {
        Id = id.ToString();
    }

    public StorySet PushBit(in StoryBit bit)
    {
        if (_bits.ContainsKey(bit.Id))
        {
            Debug.LogWarning($"Attempt to push {typeof(StoryBit)} with id {bit.Id} into {GetType()} {Id}, but bit with such Id already exists.");
            return this;
        }

        if (bit.IsInitial && InitialStoryBitsCount() >= AllowedInitialStoryBits)
        {
            Debug.LogWarning($"Attempt to push Initial StoryBit {typeof(StoryBit)} with id {bit.Id} into {GetType()} {Id}, but Initial bits are already at {AllowedInitialStoryBits}.");
            return this;
        }

        _bits.Add(bit.Id, bit);
        return this;
    }

    public int InitialStoryBitsCount()
        => _bits.Count(b => b.Value.IsInitial);
        
    

    public bool HasInitialBit()
    {
        var initialBitsCount = InitialStoryBitsCount();

        if (initialBitsCount == 0) return false;
        if (initialBitsCount != AllowedInitialStoryBits)
        {
            throw new Exception($"Unexpected Error; {GetType()} {Id} has {initialBitsCount} Initial {typeof(StoryBit)} allowed {AllowedInitialStoryBits}");
        }

        return true;
    }

    public int Count()
        => _bits.Count;

    public StoryBit GetBit(string id)
        => !_bits.TryGetValue(id, out var result) ? StoryBitNullObject.Create() : result;
}
