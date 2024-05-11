using System.Collections;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[TestFixture]
public class TestFixture
{
    private TestHarnessEditor _testHarnessEditor;
    protected CancellationToken GetTestToken => _cts?.Token ?? CancellationToken.None;
    protected RectTransform GetTestUIParent => _testHarnessEditor.GetTestParent;
    private CancellationTokenSource _cts;
    protected bool _sceneLoaded = false;
    private bool _isInitialized = false;
    /// <summary>
    /// Method to set up the Unity environment before running tests.
    /// </summary>
    /// <returns>Returns an IEnumerator object.</returns>
    [OneTimeSetUp]
    protected void UnitySetUp()
    {
        
        _cts = new CancellationTokenSource();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Assets/Scenes/TestScene.unity", LoadSceneMode.Single);
    }

    [OneTimeTearDown]
    public void UnityTearDown()
    {
        _cts?.Dispose();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loading done");
        _sceneLoaded = true;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    protected void SetUpDependencies()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        Debug.Log("Setting Dependencies started");
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
        Debug.Log("Setting Dependencies ended");
    }
    

}
