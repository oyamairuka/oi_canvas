namespace OIUtils.Tests;

[TestClass]
public sealed class LockManagerTest
{
    [TestMethod]
    public void CreateLockObject_WithNoConditions_GivesNewLockObject()
    {   // 毎回新しいロックオブジェクトが返ることを確認する
        LockManager lockManager = new();

        using LockManager.LockObject a = lockManager.CreateLockObject();
        using LockManager.LockObject b = lockManager.CreateLockObject();
        Assert.IsFalse(ReferenceEquals(a, b));
    }

    // LockObjectのテストここから
    [TestMethod]
    public void Try_WithSameLockManager_RelatedToSameFlag()
    {   // 同一のLockManagerから作られた場合は同一のコンディションのフラグに関連することを確認する
        LockManager lockManager = new();

        using LockManager.LockObject a = lockManager.CreateLockObject();
        using LockManager.LockObject b = lockManager.CreateLockObject();

        bool resultA = a.Try();
        bool resultB = b.Try();

        Assert.IsTrue(resultA);
        Assert.IsFalse(resultB);

        // リリースして今度はbから先に獲得する
        a.Release();

        resultB = b.Try();
        resultA = a.Try();

        Assert.IsFalse(resultA);
        Assert.IsTrue(resultB);
    }

    [TestMethod]
    public void Try_WithAnotherLockManager_RelatedToAnotherFlag()
    {   // 別のLockManagerから作られた場合は別のコンディションのフラグに関連することを確認する
        LockManager lockManagerA = new();
        LockManager lockManagerB = new();

        using LockManager.LockObject a = lockManagerA.CreateLockObject();
        using LockManager.LockObject b = lockManagerB.CreateLockObject();

        bool resultA = a.Try();
        bool resultB = b.Try();

        Assert.IsTrue(resultA);
        Assert.IsTrue(resultB);
    }
}
