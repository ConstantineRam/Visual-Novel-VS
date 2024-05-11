using NUnit.Framework;

public class CoreExtenshions
{

    private static (object obj, string expected)[] extenshionTestValues =
        new (object obj, string expected)[] 
    {
    (null, "Null"), ("" , "System.String")
    
    };

    [Test]
    [Category("Unit")]
    public void CoreExtensions_TypeAsStringOrNull_ResultRespectedResults([ValueSource(nameof(extenshionTestValues))] (object obj, string expected) testCase  )
    {
        var result = testCase.obj.TypeAsStringOrNull() ;
        Assert.IsTrue(result == testCase.expected, $" Expected {testCase.expected} for {testCase.obj} got {result}.");
    }



    
}
