namespace OIUtils.Tests;

[TestClass]
public sealed class LockManagerTest
{
    [TestMethod]
    public void CreateLockHandle_WithNoConditions_GivesNewLockHandle()
    {   // 毎回新しいロックハンドルが返ることを確認する
        LockManager lockManager = new();

        using LockManager.LockHandle a = lockManager.CreateLockHandle();
        using LockManager.LockHandle b = lockManager.CreateLockHandle();
        Assert.IsFalse(ReferenceEquals(a, b));
    }

    // LockHandleのテストここから
    [TestMethod]
    public void Try_WithSameLockManager_RelatedToSameFlag()
    {   // 同一のLockManagerから作られた場合は同一のコンディションのフラグに関連することを確認する
        LockManager lockManager = new();

        using LockManager.LockHandle a = lockManager.CreateLockHandle();
        using LockManager.LockHandle b = lockManager.CreateLockHandle();

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

        using LockManager.LockHandle a = lockManagerA.CreateLockHandle();
        using LockManager.LockHandle b = lockManagerB.CreateLockHandle();

        bool resultA = a.Try();
        bool resultB = b.Try();

        Assert.IsTrue(resultA);
        Assert.IsTrue(resultB);
    }

    [TestMethod]
    public void Dispose_WithOutOfScope_ReleaseLock()
    {   // スコープ外になったときに自動的にロックが解放されることを確認する
        LockManager lockManager = new();

        using var l = lockManager.CreateLockHandle();
        {
            using var a = lockManager.CreateLockHandle();
            bool resultA = a.Try();
            Assert.IsTrue(resultA);

            bool resultInScope = l.Try();
            Assert.IsFalse(resultInScope);
        }

        // スコープ外になったのでロック獲得に成功することを確認する
        bool resultOutOfScope = l.Try();
        Assert.IsTrue(resultOutOfScope);
    }
}
