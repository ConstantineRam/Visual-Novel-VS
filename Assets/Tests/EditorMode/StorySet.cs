using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class TestStorySet
{
    [Test]
    public void StorySet_Created()
    {
        Assert.NotNull(StorySet.Create(new Guid("467d085f-0a21-45c3-b5f8-246fa2b2ed77") ) );
    }

    [Test]
    public void StorySet_CanStoryBitsPushed()
    {
        var s = StorySet.Create(new Guid("666d33f3-df70-479a-8a3d-4d5a4db58e19") );
        var bit1 = StoryBit.Create("da76bbb2-f0a8-4662-a70f-4236d7a2ab2e");
        var bit2 = StoryBit.Create("b169ad88-4018-466c-a741-7ae485caa30b");
        s.PushBit(bit1).PushBit(bit2);
        Assert.IsTrue(s.Count() == 2);
    }

    [Test]
    public void StorySet_ReturnsNullObjectStoryBitIfRequestIdIsMissing()
    {
        var s = StorySet.Create(new Guid("b7a6cfff-5d91-446b-9b3c-b47c947f0fda") );

        var bit = s.GetBit("SomeStoryBitThatDoesntExist");
        Assert.IsTrue(bit.IsNullObject);
    }

    [Test]
    public void StorySet_ReturnsStoryBitWithSpecificId()
    {
        var s = StorySet.Create(new Guid("b7d3f0bd-f13c-45c0-a374-af9fa0f5a5b7") );
        var bit1 = StoryBit.Create("8f4f51b2-440c-4181-b38d-afca4cd883ed");
        var bit2 = StoryBit.Create("26f2fa39-7eee-48fd-9f59-d84baa0ad0ef");
        var bit3 = StoryBit.Create("79ebe687-d5e0-4dfa-beb9-43289cce1d5a");
        s.PushBit(bit1).PushBit(bit2).PushBit(bit3);
        
        
        Assert.NotNull(s.GetBit("26f2fa39-7eee-48fd-9f59-d84baa0ad0ef"));
    }

    
    [Test]
    public void StorySet_ThrowWarningIfCharacterTalksWithDuplicateIdAdded()
    {
        var s = StorySet.Create(new Guid("6ec65078-2fb7-4605-ad68-0325cb494bf7") );
        
        var bit1 = StoryBit.Create("96d9a43e-c78b-4223-9574-519af78180ca");
        var bit2 = StoryBit.Create("96d9a43e-c78b-4223-9574-519af78180ca");

        s.PushBit(bit1).PushBit(bit2);
        
        Assert.IsTrue(s.Count() == 1);
    }      
    
    [Test]
    public void StorySet_ThrowsWarningIfBitsWithIdenticalPushed()
    {
        var s = StorySet.Create(new Guid("666d33f3-df70-479a-8a3d-4d5a4db58e19") );
        var bit1 = StoryBit.Create("6ee0cb24-585a-4805-9461-14b0ba7fdc0f");
        var bit2 = StoryBit.Create("6ee0cb24-585a-4805-9461-14b0ba7fdc0f");
        s.PushBit(bit1);
        
        LogAssert.Expect(LogType.Warning, $"Attempt to push StoryBit with id 6ee0cb24-585a-4805-9461-14b0ba7fdc0f into StorySet 666d33f3-df70-479a-8a3d-4d5a4db58e19, but bit with such Id already exists.");
        s.PushBit(bit2);
        Assert.IsTrue(s.Count() == 1);
    }
    
    [Test]
    public void StorySet_CanHaveOnlyOneBitMarkedInitial()
    {
        var s = StorySet.Create(new Guid("5a61b622-29e9-41b3-940e-c38585e9120a") );
        var bit1 = StoryBit.Create("96bbf337-d2fc-4312-998f-db1f96862746").MarkInitial();
        var bit2 = StoryBit.Create("946b07e6-35c2-409a-a6ea-f1e88371a667").MarkInitial();
        
        s.PushBit(bit1).PushBit(bit2);
        Assert.IsTrue(s.Count() == StorySet.AllowedInitialStoryBits
            ,$"{typeof(StorySet)} has {s.Count()} amount of {typeof(StoryBit)}, should be {StorySet.AllowedInitialStoryBits}." );
    }    
}
