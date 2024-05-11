using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class TestCharacterTalk
{
 
    [Test]
    public void CharacterTalkCreated()
    {
        var talk = CharacterTalk.Create("StoryTest_CharTalkTest");
        Assert.NotNull(talk);
    }

    [Test]
    public void CharacterTalkBitCreated()
    {
        var talkBit = CharacterTalkBit.Create("testLine0");
        Assert.NotNull(talkBit);
    }

    [Test]
    public void CharacterTalk_BitsCanBeAdded()
    {
        var talk = CharacterTalk.Create("StoryTest_CharTalkTest");
        talk.Add(CharacterTalkBit.Create("testLine0"));
        
        Assert.IsTrue(talk.Any());
    }       
    
    [Test]
    public void CharacterTalk_HasAddedAmountOfBits()
    {
        const int neededAmount = 3;
        var talk = CharacterTalk.Create("StoryTest_CharTalkTest");
        talk.Add(CharacterTalkBit.Create("testLine0"));
        talk.Add(CharacterTalkBit.Create("testLine1"));
        talk.Add(CharacterTalkBit.Create("testLine2"));
        Assert.IsTrue(talk.Count() == 3, $"Needed {neededAmount} got {talk.Count()}");
        var bits = talk.Enumerate();
        Assert.IsTrue(bits.Any(b => b.GetLocalizationId == "testLine0"), "Missing 0");
        Assert.IsTrue(bits.Any(b => b.GetLocalizationId == "testLine1"), "Missing 1");
        Assert.IsTrue(bits.Any(b => b.GetLocalizationId == "testLine2"), "Missing 2");
    } 
    
    [Test]
    public void CharacterTalk_ThrowsWarningIfNotAllowedDuplicateIdFound()
    {
        var talk = CharacterTalk.Create("StoryTest_CharTalkTest");
        
        talk.Add(CharacterTalkBit.Create("testLine0"));
        
        LogAssert.Expect(LogType.Warning, $"Attempt to add not allowed duplicate CharacterTalkBit with id testLine0.");
        talk.Add(CharacterTalkBit.Create("testLine0"));
        Assert.IsTrue(talk.Count() == 1);
    }     
    
    [Test]
    public void CharacterTalkBit_ShouldHasLocalizationId()
    {
        var talkBit = CharacterTalkBit.Create("testLine0");
        Assert.NotNull(talkBit);  
        
        Assert.IsTrue(talkBit.HasLocalizationId);
    }       
}
