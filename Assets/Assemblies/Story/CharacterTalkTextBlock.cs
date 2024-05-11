using System;

public class CharacterTalkTextBlock : StoryTextBlock
{
    public override Type SupportedTalkBit()
        => typeof(CharacterTalkBit);
}
