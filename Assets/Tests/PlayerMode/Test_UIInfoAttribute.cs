using System.Collections;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class Test_UIInfoAttribute
{
    private TestHarnessEditor _testHarnessEditor;
    private CancellationToken GetTestToken => _cts?.Token ?? CancellationToken.None;
    private RectTransform GetTestUIParent => _testHarnessEditor.GetTestParent;
    private CancellationTokenSource _cts;
    private bool _sceneLoaded = false;
    private bool _isInitialized = false;
    
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
        SceneManager.LoadScene("Assets/Scenes/TestScene.unity", LoadSceneMode.Single);
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
            Assert.Fail("TestHarness GameObject was not found in the specified scene.");
        }
        else
        {
            _testHarnessEditor = testHarness.GetComponent<TestHarnessEditor>();
            if (_testHarnessEditor == null)
            {
                Assert.Fail("TestHarnessEditor component was not found on the TestHarness GameObject.");
            }
        }
        Debug.Log($"<color=#20B2AA>Setting Dependencies for {GetType()} ended</color>");
    }
    
    [Test]
    public void Test_UiElement_CreatesNullObject()
    {
        var nullObject = UIElement.CreateNullObject(GetTestToken);
        Assert.IsTrue(nullObject != null);
    }     
    
    [Test]
    public void Test_UIInfo_ClassReturnsAttributeIfItExists()
    {
        var att = TestStory.GetUIInfo();
        Assert.IsNotNull(att);
    }
    
    [Test]
    public void Test_UIInfo_ClassWithNoAttributeReturnsNullObject()
    { 
        var attNullObject = TestStoryNoAttribute.GetUIInfo();
        Assert.IsTrue(attNullObject is {IsNullObject: true});
    }
    
    [UnityTest]
    public IEnumerator Test_UiInfo_LoadingPrefabFromClassWithNoAttributeReturnsNullObject()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        
        LogAssert.Expect(LogType.Error, "TestStoryNoAttribute has no UIInfoAttribute attached.");
        var task = TestStoryNoAttribute.LoadPrefabAsResource(GetTestToken);
        while (!task.IsCompleted) yield return null;
        var result = task.GetAwaiter().GetResult();
        
        var s = result is UIElementNullObject;
        Assert.IsTrue(s);
    }   

    [UnityTest]
    public IEnumerator Test_UiInfo_LoadingPrefabYieldsNonNullResult()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        
        var task = TestStory.LoadPrefabAsResource(GetTestToken);
        while (!task.IsCompleted) yield return null;
        var prefab = task.GetAwaiter().GetResult();
      
        Assert.IsTrue(prefab != null);
        yield return null;
    }        
    
    [UnityTest]
    public IEnumerator Test_UiInfo_LoadingPrefabYieldPrefabWithTestStoryComponent()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        
        var task = TestStory.LoadPrefabAsResource(GetTestToken);
        while (!task.IsCompleted) yield return null;
        var prefab = task.GetAwaiter().GetResult();
      
        Assert.IsTrue(prefab != null && !prefab.IsNullObject && prefab.GetComponent<TestStory>() != null);
        yield return null;
    }    

    [UnityTest]
    public IEnumerator Test_UiInfo_UniquePrefabIsCreatedAndLoaded()
    {
        {
            yield return new WaitWhile(() => _sceneLoaded == false);
            SetUpDependencies();
        }
        
        var task = TestStory.Load(GetTestToken, GetTestUIParent, TestStory.TestStoryData);
        while (!task.IsCompleted) yield return null;
        var go = task.GetAwaiter().GetResult();

        Assert.IsTrue(go != null && !go.IsNullObject && go is TestStory);
        yield return null;        
    }    
}
