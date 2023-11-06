using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Platform;


namespace Apps.Modules.Components;

public partial class EntryComponent : Grid
{
    private string _ErrorMessage { get; set; }
    private string _HelpMessage { get; set; }

    public event EventHandler ShowPasswordClicked;
    public event EventHandler EntryComponentPickerClicked;
    public event EventHandler EntryComponentFocused;
    public event EventHandler EntryComponentUnfocused;

    private bool controlEntryPicker;

    public EntryComponent()
    {
        //entry.InputTransparent = false;

        InitializeComponent();
        SetBindingProperties();
    }

    private void SetBindingProperties()
    {
        entry.SetBinding(Entry.TextProperty, new Binding
        {
            Path = "EntryComponentText",
            Source = this,
            Mode = BindingMode.TwoWay
        });

        entry.SetBinding(Entry.IsEnabledProperty, new Binding
        {
            Path = "EntryComponentIsEnabled",
            Source = this,
            Mode = BindingMode.TwoWay
        });
    }

    void entry_Focused(System.Object sender, System.EventArgs e)
    {
        if (EntryComponentFocused is not null)
            EntryComponentFocused(sender, e);

#if IOS
        if (grid.Y > 250)
        {
            var res = -grid.Y + 77 > 77 ? grid.Y : -grid.Y + 77;
            (grid.Parent.Parent as VisualElement).TranslateTo(0, -res, 50);
        }
#endif

        border.Stroke = EntryComponentHelpers.HandleBorderColor(EntryComponentShowBorrderWhenFocused,
                                                           EntryComponentIsValid,
                                                           EntryComponentColors.INPUT_FOCUS_COLOR, EntryComponentColors.ERROR_COLOR, EntryComponentColors.INPUT_COLOR);
    }

    void entry_Unfocused(System.Object sender, System.EventArgs e)
    {
        if (EntryComponentUnfocused is not null)
            EntryComponentUnfocused(sender, e);
#if IOS

        (grid.Parent.Parent as VisualElement).TranslateTo(0, 0, 50);
#endif

        border.Stroke = EntryComponentHelpers.HandleBorderColor(EntryComponentIsValid, EntryComponentColors.INPUT_COLOR, EntryComponentColors.ERROR_COLOR);
    }

    void entry_Completed(System.Object sender, System.EventArgs e)
    {
#if ANDROID
        if (Platform.CurrentActivity.CurrentFocus != null)
            Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif
    }

    #region _ EntryComponentTitle . . .
    public static readonly BindableProperty EntryComponentTitleProperty =
        BindableProperty.Create(nameof(EntryComponentTitle),
            typeof(string),
            typeof(EntryComponent),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: EntryComponentTitleCoerceValue,
            propertyChanged: EntryComponentTitlePropertyChanged);

    public static object EntryComponentTitleCoerceValue(BindableObject bindable, object value) => (string)value;

    static void EntryComponentTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        control.label_Title.Text = (string)newValue;
        bindable.CoerceValue(EntryComponentTitleProperty);
    }

    public string EntryComponentTitle
    {
        get => (string)GetValue(EntryComponentTitleProperty);
        set => SetValue(EntryComponentTitleProperty, value);
    }
    #endregion

    #region _ EntryComponentText . . .
    public static readonly BindableProperty EntryComponentTextProperty =
        BindableProperty.Create(nameof(EntryComponentText),
            typeof(string),
            typeof(EntryComponent),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: EntryComponentTextCoerceValue,
            propertyChanged: EntryComponentTextPropertyChanged);

    public static object EntryComponentTextCoerceValue(BindableObject bindable, object value) => (string)value;

    public static void EntryComponentTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        var value = (string)newValue;

        if ((value?.Length ?? 0) <= control.EntryComponentMaxLength)
            control.entry.Text = (string)newValue;

        bindable.CoerceValue(EntryComponentTextProperty);
    }

    public string EntryComponentText
    {
        get => (string)GetValue(EntryComponentTextProperty);
        set => SetValue(EntryComponentTextProperty, value);
    }
    #endregion

    #region _ EntryComponentPlaceHolder . . .
    public static readonly BindableProperty EntryComponentPlaceHolderProperty =
        BindableProperty.Create(nameof(EntryComponentPlaceHolder),
            typeof(string),
            typeof(EntryComponent),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: EntryComponentPlaceHolderPropertyCoerceValue,
            propertyChanged: EntryComponentPlaceHolderPropertyPropertyChanged);

    public static object EntryComponentPlaceHolderPropertyCoerceValue(BindableObject bindable, object value) => (string)value;

    static void EntryComponentPlaceHolderPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        control.entry.Placeholder = (string)newValue;
        bindable.CoerceValue(EntryComponentPlaceHolderProperty);
    }

    public string EntryComponentPlaceHolder
    {
        get => (string)GetValue(EntryComponentPlaceHolderProperty);
        set => SetValue(EntryComponentPlaceHolderProperty, value);
    }
    #endregion

    #region _ EntryComponentHelpText . . .
    public static readonly BindableProperty EntryComponentHelpTextProperty =
        BindableProperty.Create(nameof(EntryComponentHelpText),
            typeof(string),
            typeof(EntryComponent),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: EntryComponentHelpTextCoerceValue,
            propertyChanged: EntryComponentHelpTextPropertyChanged);

    public static object EntryComponentHelpTextCoerceValue(BindableObject bindable, object value) => (string)value;

    static void EntryComponentHelpTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        control._HelpMessage = (string)newValue;
        if (control.EntryComponentIsValid)
            control.HelpErrorText.Text = (string)newValue;
        control.HelpErrorText.IsVisible = !string.IsNullOrEmpty((string)newValue);
        bindable.CoerceValue(EntryComponentTitleProperty);
    }

    public string EntryComponentHelpText
    {
        get => (string)GetValue(EntryComponentHelpTextProperty);
        set => SetValue(EntryComponentHelpTextProperty, value);
    }
    #endregion

    #region _ EntryComponentErrorText . . .
    public static readonly BindableProperty EntryComponentErrorTextProperty =
        BindableProperty.Create(nameof(EntryComponentErrorText),
            typeof(string),
            typeof(EntryComponent),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: EntryComponentErrorTextCoerceValue,
            propertyChanged: EntryComponentErrorTextPropertyChanged);

    public static object EntryComponentErrorTextCoerceValue(BindableObject bindable, object value) => (string)value;

    static void EntryComponentErrorTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        control._ErrorMessage = (string)newValue;
        if (!control.EntryComponentIsValid)
            control.HelpErrorText.Text = (string)newValue;
        control.HelpErrorText.IsVisible = !string.IsNullOrEmpty((string)newValue);
        bindable.CoerceValue(EntryComponentTitleProperty);
    }

    public string EntryComponentErrorText
    {
        get => (string)GetValue(EntryComponentErrorTextProperty);
        set => SetValue(EntryComponentErrorTextProperty, value);
    }
    #endregion

    #region _ EntryComponentIsValid . . .
    public static readonly BindableProperty EntryComponentIsValidProperty =
        BindableProperty.Create(nameof(EntryComponentIsValid),
            typeof(bool),
            typeof(EntryComponent),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: EntryComponentIsValidCoerceValue,
            propertyChanged: EntryComponentIsValidPropertyChanged);

    public static object EntryComponentIsValidCoerceValue(BindableObject bindable, object value) => (bool)value;

    public static void EntryComponentIsValidPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        HandleEntryTitleAndMessage(control, (bool)newValue);
        bindable.CoerceValue(EntryComponentIsValidProperty);
    }

    public bool EntryComponentIsValid
    {
        get => (bool)GetValue(EntryComponentIsValidProperty);
        set => SetValue(EntryComponentIsValidProperty, value);
    }

    private static void HandleEntryTitleAndMessage(EntryComponent control, bool isValid)
    {
        var hasMessage = !string.IsNullOrEmpty(control._HelpMessage) || !string.IsNullOrEmpty(control._ErrorMessage);

        control.HelpErrorText.TextColor = isValid ? EntryComponentColors.INPUT_TITLE_COLOR : EntryComponentColors.ERROR_COLOR;
        control.HelpErrorText.Text = isValid ? control._HelpMessage : control._ErrorMessage;

        //TITLE
        control.label_Title.TextColor = isValid ? EntryComponentColors.INPUT_TITLE_COLOR : EntryComponentColors.ERROR_COLOR;

        //BORDER
        control.border.Stroke = isValid ? EntryComponentColors.INPUT_FOCUS_COLOR : EntryComponentColors.ERROR_COLOR;
    }
    #endregion

    #region _ EntryComponentIsEnabled . . .
    public static readonly BindableProperty EntryComponentIsEnabledProperty =
        BindableProperty.Create(nameof(EntryComponentIsEnabled),
            typeof(bool),
            typeof(EntryComponent),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: EntryComponentIsEnabledCoerceValue,
            propertyChanged: EntryComponentIsEnabledPropertyChanged);

    public static object EntryComponentIsEnabledCoerceValue(BindableObject bindable, object value) => (bool)value;

    public static void EntryComponentIsEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;

        control.entry.IsEnabled = (bool)newValue;

        control.entry.TextColor = !(bool)newValue
            ? Color.FromArgb(AppInfo.RequestedTheme == AppTheme.Light ? "#ACACAC" : "#404040")
            : AppsColors.TEXT_COLOR;

        bindable.CoerceValue(EntryComponentIsEnabledProperty);
    }

    public bool EntryComponentIsEnabled
    {
        get => (bool)GetValue(EntryComponentIsEnabledProperty);
        set => SetValue(EntryComponentIsEnabledProperty, value);
    }
    #endregion

    #region _ EntryComponentIsPassword. . .
    public static readonly BindableProperty EntryComponentIsPasswordProperty =
        BindableProperty.Create(nameof(EntryComponentIsPassword),
            typeof(bool),
            typeof(EntryComponent),
            defaultValue: false,
            defaultBindingMode: BindingMode.OneTime,
            coerceValue: EntryComponentIsPasswordCoerceValue,
            propertyChanged: EntryComponentIsPasswordPropertyChanged);

    public static object EntryComponentIsPasswordCoerceValue(BindableObject bindable, object value) => (bool)value;

    public static void EntryComponentIsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        control.entry.IsPassword = (bool)newValue;
        control.EntryIcon.IsVisible = (bool)newValue;
        bindable.CoerceValue(EntryComponentIsValidProperty);
    }

    public bool EntryComponentIsPassword
    {
        get => (bool)GetValue(EntryComponentIsPasswordProperty);
        set => SetValue(EntryComponentIsPasswordProperty, value);
    }

    #endregion

    #region _ EntryComponentShowBorrderWhenFocused . . .
    public static readonly BindableProperty EntryComponentShowBorrderWhenFocusedProperty =
        BindableProperty.Create(nameof(EntryComponentShowBorrderWhenFocused),
            typeof(bool),
            typeof(EntryComponent),
            defaultValue: true,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: EntryComponentShowBorrderWhenFocusedCoerceValue,
            propertyChanged: EntryComponentShowBorrderWhenFocusedPropertyChanged);

    public static object EntryComponentShowBorrderWhenFocusedCoerceValue(BindableObject bindable, object value) => (bool)value;

    static void EntryComponentShowBorrderWhenFocusedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        control.border.Stroke = (bool)newValue ? EntryComponentColors.INPUT_FOCUS_COLOR : EntryComponentColors.INPUT_COLOR;
        bindable.CoerceValue(EntryComponentIsValidProperty);
    }

    public bool EntryComponentShowBorrderWhenFocused
    {
        get => (bool)GetValue(EntryComponentShowBorrderWhenFocusedProperty);
        set => SetValue(EntryComponentShowBorrderWhenFocusedProperty, value);
    }
    #endregion

    #region _ EntryComponentKeyboard . . .
    public static readonly BindableProperty EntryComponentKeyboardProperty =
        BindableProperty.Create(nameof(EntryComponentKeyboard),
            typeof(Microsoft.Maui.Keyboard),
            typeof(EntryComponent),
            defaultValue: Keyboard.Default,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: EntryComponentKeyboardCoerceValue,
            propertyChanged: EntryComponentKeyboardPropertyChanged);

    public static object EntryComponentKeyboardCoerceValue(BindableObject bindable, object value) => (Keyboard)value;


    static void EntryComponentKeyboardPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        control.entry.Keyboard = (Keyboard)newValue;
        bindable.CoerceValue(EntryComponentKeyboardProperty);
    }

    public Keyboard EntryComponentKeyboard
    {
        get => (Keyboard)GetValue(EntryComponentKeyboardProperty);
        set => SetValue(EntryComponentKeyboardProperty, value);
    }
    #endregion

    #region _ EntryComponentMaxLength . . .
    public static readonly BindableProperty EntryComponentMaxLengthProperty =
        BindableProperty.Create(nameof(EntryComponentMaxLength),
            typeof(int),
            typeof(EntryComponent),
            defaultValue: 50,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: EntryComponentMaxLengthCoerceValue,
            propertyChanged: EntryComponentMaxLengthPropertyChanged);

    public static object EntryComponentMaxLengthCoerceValue(BindableObject bindable, object value) => (int)value;

    static void EntryComponentMaxLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        control.entry.MaxLength = (int)newValue;
        bindable.CoerceValue(EntryComponentMaxLengthProperty);
    }

    public int EntryComponentMaxLength
    {
        get => (int)GetValue(EntryComponentMaxLengthProperty);
        set => SetValue(EntryComponentMaxLengthProperty, value);
    }
    #endregion

    #region _ EntryComponentHasPicker . . .
    public static readonly BindableProperty EntryComponentHasPickerProperty =
        BindableProperty.Create(nameof(EntryComponentHasPicker),
            typeof(bool),
            typeof(EntryComponent),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: EntryComponentHasPickerCoerceValue,
            propertyChanged: EntryComponentHasPickerPropertyChanged);

    public static object EntryComponentHasPickerCoerceValue(BindableObject bindable, object value) => (bool)value;

    public static void EntryComponentHasPickerPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as EntryComponent;
        //control.entry.InputTransparent = (bool)newValue;
        bindable.CoerceValue(EntryComponentHasPickerProperty);
    }

    public bool EntryComponentHasPicker
    {
        get => (bool)GetValue(EntryComponentHasPickerProperty);
        set => SetValue(EntryComponentHasPickerProperty, value);
    }
    #endregion

    void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        entry.IsPassword = !entry.IsPassword;
        EntryIcon.Data = entry.IsPassword ? EntryComponentPath.SHOW_PASSWORD_PATH : EntryComponentPath.HIDE_PASSWORD_PATH;
    }
}

public class EntryComponentColors
{
    internal static Color INPUT_FOCUS_COLOR => Application.Current.Resources.TryGetValue("AccentColor", out var result) ? (Color)result : Colors.Olive;
    internal static Color INPUT_TITLE_COLOR => Application.Current.Resources.TryGetValue("InputTitleColor", out var result) ? (Color)result : Colors.Olive;
    internal static Color ERROR_COLOR => Application.Current.Resources.TryGetValue("ErrorColor", out var result) ? (Color)result : Colors.Red;
    internal static Color INPUT_COLOR => Application.Current.Resources.TryGetValue(AppInfo.RequestedTheme == AppTheme.Light ? "BackgroundColorThemeLight2" : "BackgroundColorThemeDark2", out var result) ? (Color)result : Colors.Olive;
}

public class EntryComponentHelpers
{
    internal static Color HandleBorderColor(bool showBorderWhenFocused, bool isValid,
                                            Color focusEntryColor, Color errorColor, Color entryColor) =>
       showBorderWhenFocused ?
             (isValid ? focusEntryColor : errorColor) :
             (isValid ? entryColor : errorColor);

    internal static Color HandleBorderColor(bool isValid, Color entryColor, Color errorColor)
        => isValid ? entryColor : errorColor;
}

public class EntryComponentPath
{
    internal static PathGeometry HIDE_PASSWORD_PATH => (PathGeometry)new PathGeometryConverter().ConvertFromInvariantString("m644-428-58-58q9-47-27-88t-93-32l-58-58q17-8 34.5-12t37.5-4q75 0 127.5 52.5T660-500q0 20-4 37.5T644-428Zm128 126-58-56q38-29 67.5-63.5T832-500q-50-101-143.5-160.5T480-720q-29 0-57 4t-55 12l-62-62q41-17 84-25.5t90-8.5q151 0 269 83.5T920-500q-23 59-60.5 109.5T772-302Zm20 246L624-222q-35 11-70.5 16.5T480-200q-151 0-269-83.5T40-500q21-53 53-98.5t73-81.5L56-792l56-56 736 736-56 56ZM222-624q-29 26-53 57t-41 67q50 101 143.5 160.5T480-280q20 0 39-2.5t39-5.5l-36-38q-11 3-21 4.5t-21 1.5q-75 0-127.5-52.5T300-500q0-11 1.5-21t4.5-21l-84-82Zm319 93Zm-151 75Z");
    internal static PathGeometry SHOW_PASSWORD_PATH => (PathGeometry)new PathGeometryConverter().ConvertFromInvariantString("M480-320q75 0 127.5-52.5T660-500q0-75-52.5-127.5T480-680q-75 0-127.5 52.5T300-500q0 75 52.5 127.5T480-320Zm0-72q-45 0-76.5-31.5T372-500q0-45 31.5-76.5T480-608q45 0 76.5 31.5T588-500q0 45-31.5 76.5T480-392Zm0 192q-146 0-266-81.5T40-500q54-137 174-218.5T480-800q146 0 266 81.5T920-500q-54 137-174 218.5T480-200Zm0-300Zm0 220q113 0 207.5-59.5T832-500q-50-101-144.5-160.5T480-720q-113 0-207.5 59.5T128-500q50 101 144.5 160.5T480-280Z");

}
