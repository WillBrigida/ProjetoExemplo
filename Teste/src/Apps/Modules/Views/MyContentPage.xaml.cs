using Apps.Handlers;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace Apps.Modules.Views;

public partial class MyContentPage : ContentPage
{
    internal Core.Modules.Base.BaseViewModel VM { get; set; }
    private IStatusBarHandler _stausBarHandler;
    private ISafeAreaHandler _safeAreaHandler;
    private INavigationBarHandler _navigationBarHandler;

    internal static double BottomAreaValue { get; set; }
    internal static bool UseBottomAreaValue { get; set; }
    public List<Microsoft.Maui.Controls.Page> Stack { get; set; }
    public MyContentPage()
    {
        InitializeComponent();
        RegisterEvents();

        _stausBarHandler = Core.CoreHelpers.ServiceProvider.GetRequiredService<IStatusBarHandler>();
        _safeAreaHandler = Core.CoreHelpers.ServiceProvider.GetRequiredService<ISafeAreaHandler>();
#if ANDROID
        _navigationBarHandler = Core.CoreHelpers.ServiceProvider.GetRequiredService<INavigationBarHandler>();
#endif
    }

    private void RegisterEvents()
    {
        App.Current.RequestedThemeChanged += ThemeChanged;
        Shell.Current.Navigating += Navigating;
        Shell.Current.Navigated += Navigated;
    }

    private void Navigated(object sender, ShellNavigatedEventArgs e)
    {
    }

    private void Navigating(object sender, ShellNavigatingEventArgs e)
    {

    }

    private void ThemeChanged(object sender, AppThemeChangedEventArgs e)
    {
        CallHandleStatusBar();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        System.Diagnostics.Debug.WriteLine($">>> Entrou na {this.GetType().Name}");

        if (VM is not null)
            await VM.OnAppearingAsync();

        CallHandleStatusBar();
    }

    protected async override void OnDisappearing()
    {
        base.OnDisappearing();
        System.Diagnostics.Debug.WriteLine($"<<< Saiu da {this.GetType().Name}");

        if (VM is not null)
            await VM.OnDisappearingAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        System.Diagnostics.Debug.WriteLine($"==> Botão 'voltar' pressionado");
        return base.OnBackButtonPressed();
    }

    public void CloseButton_Clicked(System.Object sender, System.EventArgs e) => Shell.Current.GoToAsync("..");

    private void CallHandleStatusBar()
    {
        if (!UseFullArea)
            _stausBarHandler.SetColor(UseTopArea ? Colors.Transparent : StatusBarBackgroundColor);
#if ANDROID
        _navigationBarHandler.SetColor(NavigationBarBackgroundColor);
        _navigationBarHandler.SetStyle(NavigationBarStyle);
#endif
        _stausBarHandler.SetStyle(StatusBarStyle);
    }

    #region _ UseTopArea . . .
    public static readonly BindableProperty UseTopAreaProperty =
        BindableProperty.Create(nameof(UseTopArea),
            typeof(bool),
            typeof(MyContentPage),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: UseTopAreaCoerceValue,
            propertyChanged: UseTopAreaPropertyChanged);

    public static object UseTopAreaCoerceValue(BindableObject bindable, object value) => (bool)value;

    static void UseTopAreaPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as MyContentPage;
        var value = (bool)newValue;

        control.CallHandleSafeArea(value, control.UseBottomArea);

        bindable.CoerceValue(UseTopAreaProperty);
    }

    public bool UseTopArea
    {
        get => (bool)GetValue(UseTopAreaProperty);
        set => SetValue(UseTopAreaProperty, value);
    }
    #endregion

    #region _ UseBottomArea . . .
    public static readonly BindableProperty UseBottomAreaProperty =
        BindableProperty.Create(nameof(UseBottomArea),
            typeof(bool),
            typeof(MyContentPage),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: UseBottomAreaCoerceValue,
            propertyChanged: UseBottomAreaPropertyChanged);

    public static object UseBottomAreaCoerceValue(BindableObject bindable, object value) => (bool)value;

    static void UseBottomAreaPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as MyContentPage;
        var value = (bool)newValue;

        control.CallHandleSafeArea(control.UseTopArea, value);

        bindable.CoerceValue(UseBottomAreaProperty);
    }

    public bool UseBottomArea
    {
        get => (bool)GetValue(UseBottomAreaProperty);
        set => SetValue(UseBottomAreaProperty, value);
    }
    #endregion

    #region _ UseFullArea . . .
    public static readonly BindableProperty UseFullAreaProperty =
        BindableProperty.Create(nameof(UseFullArea),
            typeof(bool),
            typeof(MyContentPage),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: UseFullAreaCoerceValue,
            propertyChanged: UseFullAreaPropertyChanged);

    public static object UseFullAreaCoerceValue(BindableObject bindable, object value) => (bool)value;

    static void UseFullAreaPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as MyContentPage;
        var value = (bool)newValue;

#if ANDROID
        control._safeAreaHandler.SetFullArea();
#endif
        control.CallHandleSafeArea(control.UseTopArea, control.UseBottomArea, value);

        bindable.CoerceValue(UseFullAreaProperty);
    }

    public bool UseFullArea
    {
        get => (bool)GetValue(UseFullAreaProperty);
        set => SetValue(UseFullAreaProperty, value);
    }
    #endregion

    private void CallHandleSafeArea(bool useTopArea, bool useBottomArea, bool useFullArea = false)
    {
        this.On<iOS>().SetUseSafeArea(true);
#if IOS
        double topArea = _safeAreaHandler.GetTopArea();
        double bottomArea = _safeAreaHandler.GetBottomArea();
        var safeInsets = On<iOS>().SafeAreaInsets();

        _stausBarHandler.SetColor(UseTopArea ? Colors.Transparent : StatusBarBackgroundColor);

        BottomAreaValue = 0;

        if (useFullArea)
        {
            _stausBarHandler.SetColor(Colors.Transparent);
            safeInsets.Top = -topArea; //habilita o top bar

            UseBottomAreaValue = useBottomArea;
            safeInsets.Bottom = BottomAreaValue = -bottomArea; //habilita o bottom bar
            Padding = safeInsets;
            return;
        }

        if (useTopArea)
            safeInsets.Top = -topArea; //habilita o top bar


        UseBottomAreaValue = useBottomArea;
        if (useBottomArea)
            safeInsets.Bottom = BottomAreaValue = -bottomArea; //habilita o bottom bar

        Padding = safeInsets;
#endif
    }

    #region _ StatusBarBackgroundColor . . .
    public static readonly BindableProperty StatusBarBackgroundColorProperty =
        BindableProperty.Create(nameof(StatusBarBackgroundColor),
            typeof(Color),
            typeof(MyContentPage),
            defaultValue: Colors.Transparent,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: StatusBarBackgroundColorCoerceValue,
            propertyChanged: StatusBarBackgroundColorPropertyChanged);

    public static object StatusBarBackgroundColorCoerceValue(BindableObject bindable, object value) => (Color)value;

    static void StatusBarBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as MyContentPage;
        var value = (Color)newValue;

        if (!control.UseFullArea)
        {

            control._stausBarHandler.SetColor(control.UseTopArea ? Colors.Transparent : value);

#if ANDROID
            if (control.NavigationBarBackgroundColor is null)
                control._navigationBarHandler.SetColor(value);
#endif

        }
        bindable.CoerceValue(StatusBarBackgroundColorProperty);
    }

    public Color StatusBarBackgroundColor
    {
        get => (Color)GetValue(StatusBarBackgroundColorProperty);
        set => SetValue(StatusBarBackgroundColorProperty, value);
    }
    #endregion

    #region _ NavigationBarBackgroundColor . . .
    public static readonly BindableProperty NavigationBarBackgroundColorProperty =
        BindableProperty.Create(nameof(NavigationBarBackgroundColor),
            typeof(Color),
            typeof(MyContentPage),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: NavigationBarBackgroundColorCoerceValue,
            propertyChanged: NavigationBarBackgroundColorPropertyChanged);

    public static object NavigationBarBackgroundColorCoerceValue(BindableObject bindable, object value) => (Color)value;

    static void NavigationBarBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as MyContentPage;
        var value = (Color)newValue;

#if ANDROID
        control._navigationBarHandler.SetColor(value is null ? control.StatusBarBackgroundColor : value);
#endif

        bindable.CoerceValue(NavigationBarBackgroundColorProperty);
    }

    public Color NavigationBarBackgroundColor
    {
        get => (Color)GetValue(NavigationBarBackgroundColorProperty);
        set => SetValue(NavigationBarBackgroundColorProperty, value);
    }
    #endregion

    #region _ StatusBarStyle . . .
    public static readonly BindableProperty StatusBarStyleProperty =
        BindableProperty.Create(nameof(StatusBarStyle),
            typeof(eStatusBarStyle),
            typeof(MyContentPage),
            defaultValue: eStatusBarStyle.Default,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: StatusBarStyleCoerceValue,
            propertyChanged: StatusBarStylePropertyChanged);

    public static object StatusBarStyleCoerceValue(BindableObject bindable, object value) => (eStatusBarStyle)value;

    static void StatusBarStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as MyContentPage;
        var value = (eStatusBarStyle)newValue;

        control.CallHandleStatusBarStyle(value);

        bindable.CoerceValue(StatusBarStyleProperty);
    }

    private void CallHandleStatusBarStyle(eStatusBarStyle value) => _stausBarHandler.SetStyle(value);

    public eStatusBarStyle StatusBarStyle
    {
        get => (eStatusBarStyle)GetValue(StatusBarStyleProperty);
        set => SetValue(StatusBarStyleProperty, value);
    }
    #endregion

    #region _ NavigationBarStyle . . .
    public static readonly BindableProperty NavigationBarStyleProperty =
        BindableProperty.Create(nameof(NavigationBarStyle),
            typeof(eNavigationBarStyle),
            typeof(MyContentPage),
            defaultValue: eNavigationBarStyle.Default,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: NavigationBarStyleCoerceValue,
            propertyChanged: NavigationBarStylePropertyChanged);

    public static object NavigationBarStyleCoerceValue(BindableObject bindable, object value) => (eNavigationBarStyle)value;

    static void NavigationBarStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as MyContentPage;
        var value = (eNavigationBarStyle)newValue;

        control.CallHandleNavigationBarStyle(value);

        bindable.CoerceValue(NavigationBarStyleProperty);
    }
    private void CallHandleNavigationBarStyle(eNavigationBarStyle value) => _navigationBarHandler.SetStyle(value);

    public eNavigationBarStyle NavigationBarStyle
    {
        get => (eNavigationBarStyle)GetValue(NavigationBarStyleProperty);
        set => SetValue(NavigationBarStyleProperty, value);
    }
    #endregion
}
