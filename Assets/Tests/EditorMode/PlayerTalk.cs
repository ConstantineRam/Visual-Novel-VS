using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestPlayerTalk
{
    [Test]
    public void PlayerTalkCreated()
    {
        var talk = PlayerTalk.Create("StoryTest_CharTalkTest");
        Assert.NotNull(talk);
    }

}
