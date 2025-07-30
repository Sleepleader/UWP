using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // 导航到首页并设置SelectedItem
            NavigationViewItem homeItem = MyNavigationView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(item => item.Tag.ToString() == "Home");
            if (homeItem != null)
            {
                MyNavigationView.SelectedItem = homeItem;
            }
            NavigationFrame.Navigate(typeof(HomePage));
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                // 用户点击了默认的“设置”项
                NavigationFrame.Navigate(typeof(SettingPage));
            }
            else
            {
                // 确保 InvokedItemContainer 不为 null，并且能够安全地获取 Tag 属性
                var item = args.InvokedItemContainer as NavigationViewItem;
                if (item != null && item.Tag != null)
                {
                    var navItemTag = item.Tag.ToString();
                    switch (navItemTag)
                    {
                        case "Home":
                            NavigationFrame.Navigate(typeof(HomePage));
                            break;
                        case "Library":
                            NavigationFrame.Navigate(typeof(LibraryPage));
                            break;
                            // Add more cases for other items if needed
                    }
                }
            }
        }
    }
}