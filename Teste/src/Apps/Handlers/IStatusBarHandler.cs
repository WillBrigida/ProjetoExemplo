namespace Apps.Handlers
{
    public enum eStatusBarStyle { Default, LightContent, DarkContent }

    public interface IStatusBarHandler
    {
        public void SetColor(Color color);
        public void SetStyle(eStatusBarStyle statusBarStyle);
    }
}

