using System;
using System.Collections;
using System.Threading.Tasks;
//https://forum.unity.com/threads/async-await-in-unittests.513857/#post-6046400
// UNITY doesn't support direct async tests. (version 2019.4.0f1)
// Async is to be supported with version 2.0. Consider rework tests.
// 2.0 was abandoned by Unity, partial async support moved to prev version. This util still worth using.

public static class UnityTestUtils
{
    /// <summary>
    /// Executes an asynchronous method synchronously.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam> <param name="asyncFunc">The asynchronous method as a <see cref="Func{Task{T}}"/>.</param> <returns>The result of the asynchronous method.</returns>
    /// <example>
    /// <pre>
    /// <code>
    /// [Test]
    /// public void Test()
    /// {
    ///   var result = RunAsyncMethodSync(() => GetTestTaskAsync(4));
    ///   Assert.That(result, Is.EqualTo(4));
    /// }
    /// </code>
    /// </pre>
    /// </example>
    public static T RunAsyncMethodSync<T> ( Func<Task<T>> asyncFunc )
    {
        return Task.Run(async ( ) => await asyncFunc()).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Executes the given asynchronous method synchronously.
    /// </summary>
    /// <param name="asyncFunc">The asynchronous method to be executed.</param>
    /// <example>
    /// <pre>
    /// <code>
    /// [Test]
    /// public void Test()
    /// {
    ///   var result = RunAsyncMethodSync(() => GetTestTaskAsync(4));
    ///   Assert.That(result, Is.EqualTo(4));
    /// }
    /// 
    /// public async Task &#60;int&#62; GetTestTaskAsync(int a) {
    ///         await Task.Delay(TimeSpan.FromMilliseconds(200));
    ///         return a;
    /// }
    /// </code>
    /// </pre>
    /// </example> 
    public static void RunAsyncMethodSync ( Func<Task> asyncFunc )
    {
        Task.Run(async ( ) => await asyncFunc()).GetAwaiter().GetResult();
    }


    /// <summary>
    /// Waits for the given Task to complete using a Coroutine.
    /// </summary>
    /// <param name="task">The Task to wait for.</param>
    /// <returns>An IEnumerator to be used with Coroutine.</returns>
    public static IEnumerator Await(Task task)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task is {IsFaulted: true, Exception: not null})
        {
            throw task.Exception;
        }
    }


    /// <summary>
    /// Waits for a <see cref="Task"/> to complete.
    /// </summary>
    /// <param name="taskDelegate">The delegate that represents the asynchronous operation to wait for.</param>
    /// <returns>An enumerator that can be used to iterate over the sequence of awaitable objects.</returns>


    public static IEnumerator Await ( Func<Task> taskDelegate )
    {
        return Await(taskDelegate.Invoke());
    }


}


