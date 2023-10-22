using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Apps.Platforms.Android.Renderers
{
    public class CustomShellRenderer : ShellRenderer
    {
        IShellItemRenderer _currentView;

        public CustomShellRenderer() { }

        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
            => new CustomShellItemRenderer(this);

        protected override void SwitchFragment(FragmentManager manager, global::Android.Views.View targetView, ShellItem newItem, bool animate = true)
        {
            _currentView = CreateShellItemRenderer(newItem);
            _currentView.ShellItem = newItem;
            var fragment = _currentView.Fragment;

            FragmentTransaction transaction = manager.BeginTransaction();
            transaction.Add(targetView.Id, fragment);

            transaction.Replace(fragment.Id, fragment);
            transaction.CommitAllowingStateLoss();
        }
    }

    public class CustomShellItemRenderer : ShellItemRenderer
    {
        public CustomShellItemRenderer(IShellContext context) :
            base(context)
        { }

        protected override void SetupAnimation(ShellNavigationSource navSource, FragmentTransaction t, Page page)
            => t.SetCustomAnimations(0, 0);
    }
}

