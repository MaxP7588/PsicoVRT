using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    [Test]
    public void NewTestScriptSimplePasses()
    {
        int expectedValue = 5;
        int actualValue = 5;
        Assert.AreEqual(expectedValue, actualValue, "Los valores deberían ser iguales");
    }

    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        
        GameObject testObject = new GameObject("TestObject");
        testObject.AddComponent<Rigidbody>();

        yield return null;

        Assert.IsNotNull(testObject.GetComponent<Rigidbody>(), "El objeto debería tener un Rigidbody");

        if (Application.isPlaying)
        {
            Object.Destroy(testObject);
            yield return null;
        }
        else
        {
            Object.DestroyImmediate(testObject);
        }
    }
}
