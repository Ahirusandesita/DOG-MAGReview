using System;

static class GCNotifier
{
    public static event Action Collected;
    public static void Start()
    {
        new DummyObject();
    }
    class DummyObject
    {
        ~DummyObject()
        {
            if (!AppDomain.CurrentDomain.IsFinalizingForUnload()
            && !Environment.HasShutdownStarted)
            {
                Collected?.Invoke();
                new DummyObject();
            }
        }
    }
}
