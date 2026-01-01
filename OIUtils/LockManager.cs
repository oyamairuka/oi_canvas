namespace OIUtils;

public class LockManager
{
    public class Flag
    {
        private int i_;
        internal Flag() { i_ = 0; }
        internal ref int I() { return ref i_; }
    }

    public class LockObject : IDisposable
    {
        private bool disposed_;
        private readonly Flag f_;
        private bool haveLocked_;
        public LockObject(Flag f) { f_ = f; }

        // アンマネージドリソースが追加されたらファイナライザを定義する
        // ~LockObject()
        // {
        //     Dispose(false);
        // }

        public bool Try()
        {
            int result = Interlocked.CompareExchange(ref f_.I(), 1, 0);
            // 1が返る場合はすでに取得されていた
            if (result == 1) return false;
            haveLocked_ = true;
            return true;
        }

        public void Release()
        {
            if (haveLocked_)
            {
                Interlocked.CompareExchange(ref f_.I(), 0, 1);
                haveLocked_ = false;
            }
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed_)
            {
                if (disposing)
                {
                    if (haveLocked_)
                    {
                        Release();
                    }
                }
                // アンマネージドリソースがある場合は以下にその処理を追加

                disposed_ = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // アンマネージドリソースが追加された場合は以下が必要,アンマネージドリソースがない場合は不要
            // GC.SuppressFinalize(this);
        }
    }

    private readonly Flag f_;
    public LockManager() { f_ = new(); }

    public LockObject CreateLockObject() { return new LockObject(f_); }
}
