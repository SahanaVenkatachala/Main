using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace storeEmpowerment
{
    class popupBox
    {
        private string popupMessage;
        public popupBox(string message)
        {
            popupMessage = message;
        }
        public async void Message()
        {
            MessageDialog msgDialog = new MessageDialog("Please Update The App");

            //Commands
            msgDialog.Commands.Add(new UICommand("Update", new UICommandInvokedHandler(CommandHandlers)));
            msgDialog.Commands.Add(new UICommand("Decline", new UICommandInvokedHandler(CommandHandlers)));

            await msgDialog.ShowAsync();
        }

        public async void CommandHandlers(IUICommand commandLabel)
        {
            Frame frame = Window.Current.Content as Frame;
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                //Okay Button.
                case "Update":
                    await Launcher.LaunchUriAsync(new Uri("https://labs.ocset.net/smapp"));
                    // MainPage.mainPagePointer.Text.text = "";
                    break;
                //Quit Button.
                case "Decline":
                    App.localSettings.Values["refresh"] = null;
                    App.localSettings.Values["token"] = null;
                    App.localSettings.Values["expires"] = null;
                    break;
                //end.
            }
        }
        public Task ShowDialog()
        {
            CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            Func<object, Task<bool>> action = null;
            action = async (o) =>
            {
                try
                {
                    if (dispatcher.HasThreadAccess)
                    {
                        await new MessageDialog(this.popupMessage).ShowAsync();
                    }
                    else
                    {
                        dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () => action(o));
                    }
                    return true;
                }
                catch (UnauthorizedAccessException)
                {
                    if (action != null)
                    {
                        Task.Delay(500).ContinueWith(async t => await action(o));
                    }
                }
                return false;
            };
            return action(null);
        }
    }
}
