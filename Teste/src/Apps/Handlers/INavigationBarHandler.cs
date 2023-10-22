namespace Apps.Handlers
{
    public enum eNavigationBarStyle { Default, LightContent, DarkContent }

    public interface INavigationBarHandler
	{
        public void SetColor(Color color);
        public void SetStyle(eNavigationBarStyle NavigationBarStyle);
    }
}
