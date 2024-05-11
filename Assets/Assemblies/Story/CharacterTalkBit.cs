
public class CharacterTalkBit : TalkBitImpl<CharacterTalkBit>
{
    public static CharacterTalkBit Create(in string localizationId)
    {
        return new CharacterTalkBit(localizationId);
    }
    private CharacterTalkBit (in string localizationId) : base( localizationId) { }


}