using UnityEngine;
using NUnit.Framework;
using System.Threading;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class Test_Story
{
    private TestHarnessEditor _testHarnessEditor;
    private CancellationToken GetTestToken => _cts?.Token ?? CancellationToken.None;
    private RectTransform GetTestUIParent => _testHarnessEditor.GetTestParent;
    private CancellationTokenSource _cts;
    private bool _sceneLoaded = false;
    private bool _isInitialized = false;
    private PlayableTestStory _story;
    private StoryData _storyData = TestStory.TestStoryData;
    
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
        SceneManager.LoadScene("Assets/Scenes/TestScene_WithStory.unity", LoadSceneMode.Single);
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

        var testStory = GameObject.Find("PlayableTestStory");
        if (testStory == null)
        {
            Assert.Fail("<color=#e30022>TestStory GameObject was not found in the specified scene.</color>");
        }
        else
        {
            _story = testStory.GetComponent<PlayableTestStory>();
            if (_story == null)
            {
                Assert.Fail("<color=#e30022>PlayableTestStory component was not found on the PlayableTestStory GameObject.</color>");
            }
        
            UnityTestUtils.RunAsyncMethodSync(() => _story.Initialize(_storyData, GetTestToken));        
        }
        
        Debug.Log($"<color=#20B2AA>Setting Dependencies for {GetType()} ended</color>");
    }
    
    [UnityTest]
    public IEnumerator Test_StoryInitializedAfterLaunch()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }

        Assert.NotNull(_story, "Story component wasn't populated.");
        Assert.IsTrue(!_story.IsNullObject, "Null object was created.");
        Assert.IsTrue(_story.IsInitialized,"Story was not initialized.");    
        yield return null;        
    }

    private static System.Type[] _goValues = new System.Type[] {typeof(CharacterSlot), typeof(BackgroundSlot)}; 
    [UnityTest]
    public IEnumerator Test_StoryHasSpecificGameObject<T>([ValueSource(nameof(_goValues))] T requestedType) where T : System.Type
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        var go = _story.GetComponentInChildren(requestedType);

        Assert.NotNull(go, $"Can't find {typeof(T)} component.");
        yield return null;        
    }      

    [UnityTest]
    public IEnumerator Test_Story_AllInitializableObjectsAreInitialized()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }

        var initializables = _story.GetComponentsInChildren<Initializable>();
        foreach (var i in initializables)
        {
            Assert.IsTrue(i.IsInitialized, $" {_story.name} has {i} not initialized.");
        }
        yield return null;
    }


    [UnityTest]
    public IEnumerator Test_Story_CheckIfAllBackgroundsAssignedAndReturnTrueIfTheyAre()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        
        Assert.IsTrue(_story.IsAllBackgroundsAssigned());
        yield return null;        
    }     

    [UnityTest]
    [TestCase(3, ExpectedResult = (IEnumerator) null)]
    public IEnumerator Test_Story_HasCharacterLibraryWithProperAmountOfEntities(int requiredAmount)
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        
        Assert.IsTrue(_story.GetCharacterAmount == requiredAmount);
        yield return null;        
    }      
    
    [UnityTest]
    [TestCase(2, ExpectedResult = (IEnumerator) null)]
    public IEnumerator Test_Story_HasBackgroundLibraryWithProperAmountOfBackgrounds(int requiredAmount)
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        
        Assert.IsTrue(_story.GetBackgroundsAmount == requiredAmount);
        yield return null;        
    }  

    [UnityTest]
    [TestCase("TestBackground_2(Clone)", ExpectedResult = (IEnumerator) null)]
    public IEnumerator Test_Story_AssignsSpecificBackgroundAsCurrent(string expectedPrefab)
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        
        _story.SetBackground(TestStoryBackgrounds.Background1);
        var currentBackground = _story.GetCurrentBackground;
        
        Assert.IsNotNull(currentBackground);
        Assert.IsTrue(currentBackground.name == expectedPrefab, $"Expected {expectedPrefab} got {currentBackground.name}");
        yield return null;        
    }    
    
    [UnityTest]
    public IEnumerator Test_Story_ReturnsCurrentlyAssignedBackground()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        var currentBackground = _story.GetCurrentBackground;
        
        Assert.IsNotNull(currentBackground);
        yield return null;        
    }

    [UnityTest]
    public IEnumerator Test_Story_ReturnsCurrentlyAssignedCharacter()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        _story.SetCharacter(TestStoryCharacter.CharacterOtherGuy);
        var current = _story.GetCurrentCharacter;
        
        Assert.IsNotNull(current);
        yield return null;        
    }    
}
