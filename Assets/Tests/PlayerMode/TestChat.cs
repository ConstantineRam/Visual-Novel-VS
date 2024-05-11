using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestChat
{
    private TestHarnessEditor _testHarnessEditor;
    private CancellationToken GetTestToken => _cts?.Token ?? CancellationToken.None;
    private CancellationTokenSource _cts;
    private bool _sceneLoaded = false;
    private bool _isInitialized = false;
    private Chat _chat;
    private ChatData _testChatData = ChatData.TestChatData;
    /// <summary>
    /// Method to set up the Unity environment before running tests.
    /// </summary>
    /// <returns>Returns an IEnumerator object.</returns>
    [OneTimeSetUp]
    public virtual void UnitySetUp()
    {
        Debug.Log($"<color=#198e88>==== {GetType()} started ====</color>"); 
        _cts = new CancellationTokenSource();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Assets/Scenes/TestSceneChat.unity", LoadSceneMode.Single);
    }
    
    [OneTimeTearDown]
    public void UnityTearDown()
    {
        _cts?.Dispose();
        Debug.Log($"<color=#198e88>==== {GetType()} ended ====</color>");
    }    
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("<color=#20B2AA>Scene Loading done</color>");
        _sceneLoaded = true;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }    
    
    private void SetUpDependencies()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        Debug.Log($"<color=#20B2AA>Setting Dependencies for {GetType()} started</color>");
        var testHarness = GameObject.Find("TestHarness");
        if (testHarness == null)
        {
            Assert.Fail("<color=#e30022>TestHarness GameObject was not found in the specified scene.</color>");
        }
        else
        {
            _testHarnessEditor = testHarness.GetComponent<TestHarnessEditor>();
            if (_testHarnessEditor == null)
            {
                Assert.Fail("<color=#e30022>TestHarnessEditor component was not found on the TestHarness GameObject.</color>");
            }
        }

        var testChat = GameObject.Find("TestChat");
        if (testChat == null)
        {
            Assert.Fail("<color=#e30022>TestChat GameObject was not found in the specified scene.</color>");
        }
        else
        {
            (_chat) = testChat.GetComponent<Chat>();
            if (_chat == null)
            {
                Assert.Fail("<color=#e30022>Chat component was not found on the testChat GameObject.</color>");
            }
        
            _chat.Initialize(_testChatData);        
        }
        
        Debug.Log($"<color=#20B2AA>Setting Dependencies for {GetType()} ended</color>");
    }

    private StorySet CreateTestStorySet0()
    {
        var result = StorySet.Create(new Guid("f2e13643-9b52-47e2-a790-46230436d9a3"));

        var storyBit0 =  StoryBit.Create("TestBit0").MarkInitial();
        var bit0CharTalk0 = CharacterTalk.Create("TestBit0CT0");
        bit0CharTalk0.Add(CharacterTalkBit.Create("TestBit0CT0B0"))
            .Add(CharacterTalkBit.Create("TestBit0CT0B1").MarkAsNarrator() as CharacterTalkBit)
            .Add(CharacterTalkBit.Create("TestBit0CT0B2"));
        
        var bit0CharTalk1 = CharacterTalk.Create("TestBit0CT1");
        bit0CharTalk1.Add(CharacterTalkBit.Create("TestBit1CT0B0"))
            .Add(CharacterTalkBit.Create("TestBit1CT0B1").MarkAsNarrator() as CharacterTalkBit)
            .Add(CharacterTalkBit.Create("TestBit1CT0B2"));
        storyBit0.Add(bit0CharTalk0).Add(bit0CharTalk1);
        Assert.IsTrue(storyBit0.Count<CharacterTalk>() == 2);
        result.PushBit(storyBit0);

        
        var storyBit1 =  StoryBit.Create("TestBit1");
        var bit1CharTalk0 = CharacterTalk.Create("TestBit1CT0");
        bit1CharTalk0.Add(CharacterTalkBit.Create("TestBit1CT0B0"))
            .Add(CharacterTalkBit.Create("TestBit1CT0B1").MarkAsNarrator() as CharacterTalkBit)
            .Add(CharacterTalkBit.Create("TestBit1CT0B2"));
        
        var bit1CharTalk1 = CharacterTalk.Create("TestBit1CT1");
        bit1CharTalk1.Add(CharacterTalkBit.Create("TestBit1CT1B0"))
            .Add(CharacterTalkBit.Create("TestBit1CT1B1").MarkAsNarrator() as CharacterTalkBit)
            .Add(CharacterTalkBit.Create("TestBit1CT1B2"));
        storyBit1.Add(bit1CharTalk0).Add(bit1CharTalk1);
        Assert.IsTrue(storyBit1.Count<CharacterTalk>() == 2);
        
        result.PushBit(storyBit1);
        
        Assert.IsTrue(result.Count() == 2);
        return result;
    }

    [UnityTest]
    public IEnumerator TestChatCharacterTalkBitCanBePushed()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        const int expected = 3;
        
        var testStory = CreateTestStorySet0();
        var storyBit = testStory.GetBit("TestBit0");
        Assert.IsFalse(storyBit.IsNullObject, "StoryBit is Null Object");
        
        var characterTalk = storyBit.GetCharacterTalk("TestBit0CT0");
        Assert.IsFalse(characterTalk.IsNullObject, "CharacterTalk is NullObject");
        
        _chat.Push(characterTalk);
        Assert.IsTrue(_chat.Count() == expected, $"Expected bits {expected}, found {_chat.Count()}"); 
    }

    [UnityTest]
    public IEnumerator TestChatIsInitialized()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        
        Assert.IsTrue(_chat.IsInitialized);
        yield return null;
    }
}
