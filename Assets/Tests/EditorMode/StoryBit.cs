using System;
using NUnit.Framework;


public class TestStoryBit
{
    
    [Test]
    public void StoryBit_Created()
    {
       Assert.NotNull(StoryBit.Create("c702bdb5-5277-4ebd-ac1c-b43690133410") );
    }

  
    
    [Test]
    public void StoryBit_CanBeMarkedInitial()
    {
        var s = StoryBit.Create("64b6c34e-f40f-49f6-8e32-2f38c8c1f910" ).MarkInitial();
        Assert.IsTrue(s.IsInitial);
    }
    
    [Test]
    public void StoryBit_CanHaveCharacterTalksAdded()
    {
        var s = StoryBit.Create("74d988b1-97eb-4e6b-950f-e2b4fa28d725" ).MarkInitial();
        var characterTalk0 = CharacterTalk.Create( "36e22b36-3a2c-45da-a618-3bb0376156ef").Add(CharacterTalkBit.Create("testLine0"))
            .Add(CharacterTalkBit.Create("testLine1"));
        
        var characterTalk1 = CharacterTalk.Create("995d3b28-bce2-4b47-b31b-e34245575489").Add(CharacterTalkBit.Create("testLineLong0"));

        s.Add(characterTalk0).Add(characterTalk1);

        Assert.IsTrue(s.Count<CharacterTalk>() == 2);
    }

    [Test]
    public void StoryBit_CanHavePlayerTalksAdded()
    {
        var s = StoryBit.Create("74d988b1-97eb-4e6b-950f-e2b4fa28d725" ).MarkInitial();
        var characterTalk0 = CharacterTalk.Create( "36e22b36-3a2c-45da-a618-3bb0376156ef").Add(CharacterTalkBit.Create("testLine0"))
            .Add(CharacterTalkBit.Create("testLine1"));
        
        var characterTalk1 = CharacterTalk.Create("995d3b28-bce2-4b47-b31b-e34245575489").Add(CharacterTalkBit.Create("testLineLong0"));

        s.Add(characterTalk0).Add(characterTalk1);

        Assert.IsTrue(s.Count<CharacterTalk>() == 2);
    }    
    
    
    [Test]
    public void StoryBit_ReturnsCharacterTalkWithId()
    {
        var s = StoryBit.Create("74d988b1-97eb-4e6b-950f-e2b4fa28d725" ).MarkInitial();
        var characterTalk0 = CharacterTalk.Create( "36e22b36-3a2c-45da-a618-3bb0376156ef").Add(CharacterTalkBit.Create("testLine0"))
            .Add(CharacterTalkBit.Create("testLine1"));

       var talk = s.GetCharacterTalk("36e22b36-3a2c-45da-a618-3bb0376156ef");
       Assert.NotNull(talk);
    }
    
    [Test]
    public void StoryBit_ReturnsNullObjectCharacterTalkIfWrongIdProvided()
    {
        var s = StoryBit.Create("74d988b1-97eb-4e6b-950f-e2b4fa28d725" ).MarkInitial();
        var characterTalk0 = CharacterTalk.Create( "36e22b36-3a2c-45da-a618-3bb0376156ef").Add(CharacterTalkBit.Create("testLine0"));

        var talk = s.GetCharacterTalk("ObviouslyWrongId");
        Assert.IsTrue(talk.IsNullObject);
    }    
}
