using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalkBit : TalkBitImpl<PlayerTalkBit>
{
    public static PlayerTalkBit Create(in string localizationId)
    {
        return new PlayerTalkBit(localizationId);
    }
    private PlayerTalkBit (in string localizationId) : base( localizationId) { }

}
