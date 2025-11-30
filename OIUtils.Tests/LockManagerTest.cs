namespace OIUtils.Tests;

[TestClass]
public sealed class LockManagerTest
{
    [TestMethod]
    public void TestMethod1()
    {
        Console.WriteLine("Hello, World!");

        LockManager lockManager = new();

        {
            using LockManager.LockObject l = lockManager.CreateLockObject();

            Console.WriteLine(l.Try());
            Console.WriteLine(l.Try());
            Console.WriteLine(l.Try());
        }

        Console.WriteLine("FIN");
    }
}
