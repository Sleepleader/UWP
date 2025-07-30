using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace App2
{
    public sealed partial class SettingPage : Page
    {
        private const string ThemeSettingKey = "AppThemePreference";

        public SettingPage()
        {
            this.InitializeComponent();
            LoadSavedTheme();
        }

        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                ElementTheme theme;

                switch (radioButton.Name)
                {
                    case "LightThemeRadioButton":
                        theme = ElementTheme.Light;
                        break;
                    case "DarkThemeRadioButton":
                        theme = ElementTheme.Dark;
                        break;
                    default: // 包括 SystemThemeRadioButton 和任何未知/无效的值
                        theme = ElementTheme.Default;
                        break;
                }

                ApplyTheme(theme);
                SaveThemePreference(radioButton.Name);
            }
        }

        private void ApplyTheme(ElementTheme theme)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null)
            {
                rootFrame.RequestedTheme = theme;
            }
        }

        private void LoadSavedTheme()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey(ThemeSettingKey))
            {
                var savedThemeName = (string)localSettings.Values[ThemeSettingKey];
                switch (savedThemeName)
                {
                    case "LightThemeRadioButton":
                        LightThemeRadioButton.IsChecked = true;
                        ApplyTheme(ElementTheme.Light);
                        break;
                    case "DarkThemeRadioButton":
                        DarkThemeRadioButton.IsChecked = true;
                        ApplyTheme(ElementTheme.Dark);
                        break;
                    default:
                        SystemThemeRadioButton.IsChecked = true;
                        ApplyTheme(ElementTheme.Default);
                        break;
                }
            }
            else
            {
                SystemThemeRadioButton.IsChecked = true;
                ApplyTheme(ElementTheme.Default);
            }
        }

        private void SaveThemePreference(string themeName)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[ThemeSettingKey] = themeName;
        }
    }
}