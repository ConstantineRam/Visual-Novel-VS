using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBitImpl<T> : TalkBit where T : TalkBit
{
    public bool IsNarrator { get; private set; }
    private readonly string _localizationId;


    protected TalkBitImpl(in string localizationId)
    {
        _localizationId = localizationId;
    }

    public TalkBitImpl<T> MarkAsNarrator()
    {
        IsNarrator = true;
        return this;
    }

    public bool HasLocalizationId => GetLocalizationId != null;
    public string GetLocalizationId => _localizationId ?? string.Empty;
    public string GetShownString => GetLocalizationId;
}
