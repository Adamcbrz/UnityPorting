﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyPlugin.Facebook;
using UnityPlayer;

namespace Platformer
{
    public partial class FacebookIntegration : UserControl
    {
        public FacebookIntegration()
        {
            this.InitializeComponent();

            // facebook overlay integration
            //WebPopup.Opened += PopupOpened;
            //WebPopup.Closed += PopupClosed;
            MyPlugin.Facebook.FacebookGateway.Instance.Initialise(FacebookOverlay);
            MyPlugin.Facebook.FacebookGateway.Instance.StateChanged += FacebookStateChanged;
        }

        public void HideFacebookOverlay()
        {

            if (MainPage.Current.FacebookGrid.Visibility == Visibility.Visible)
            {
                MainPage.Current.FacebookGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void FacebookStateChanged(FacebookRequest request, NavigationState state)
        {
            switch (state)
            {
                case NavigationState.UserInput:
                    MainPage.Current.FacebookLoading.Visibility = Visibility.Collapsed;
                    break;

                case NavigationState.Done:
                    MainPage.Current.FacebookGrid.Visibility = Visibility.Collapsed;
                    MainPage.Current.FacebookLoading.Visibility = Visibility.Collapsed;
                    break;

                case NavigationState.Error:
                    MainPage.Current.FacebookGrid.Visibility = Visibility.Visible;
                    MainPage.Current.FacebookLoading.Visibility = Visibility.Collapsed;
                    //FacebookOverlay.NavigateToString(""); // Why is this here? Doesn't make sense to show an empty page if there is an error
                    break;

                case NavigationState.Navigating:
                    MainPage.Current.FacebookLoading.Visibility = Visibility.Visible;;

                    switch (request)
                    {
                        case FacebookRequest.Logout:
                            MainPage.Current.FacebookGrid.Visibility = Visibility.Collapsed;
                            break;

                        case FacebookRequest.Login:
                            MainPage.Current.FacebookGrid.Visibility = Visibility.Visible;
                            break;
                        case FacebookRequest.InviteRequest:
                            MainPage.Current.FacebookGrid.Visibility = Visibility.Visible;
                            break;
                    }
                    break;
            }
        }

        private void CancelWeb(object sender, RoutedEventArgs e)
        {
            MyPlugin.Facebook.FacebookGateway.Instance.Cancel();
        }
    }
}
