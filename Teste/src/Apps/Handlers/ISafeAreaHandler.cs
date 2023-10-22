namespace Apps.Handlers
{
    public interface ISafeAreaHandler
    {
#if IOS
        public double GetTopArea();
        public double GetBottomArea();

#elif ANDROID
        public void SetFullArea();
#endif
    }
}

