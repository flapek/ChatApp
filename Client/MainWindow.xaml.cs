using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _all = "All";
        private const string _yourself = "Yourself";
        private HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44312/chat")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    messagesList.Items.Add(newMessage);
                });
            });

            connection.On<string>("UserConnected", (userId) =>
            {
                Dispatcher.Invoke(() =>
                {
                    usersList.Items.Add(new ListViewItem()
                    {
                        Content = userId
                    });
                });
            });

            connection.On<string>("UserDisconnected", (userId) =>
            {
                Dispatcher.Invoke(() =>
                {

                });
            });

            try
            {
                await connection.StartAsync();
                messagesList.Items.Add("Connection started");
                connectButton.IsEnabled = false;
                sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var x = usersList.SelectedItem as ListViewItem;
                var recipient = (usersList.SelectedItem as ListViewItem).Content;

                if (recipient.Equals(_all))
                    await connection.InvokeAsync("SendMessageToAll",
                        userTextBox.Text, messageTextBox.Text);
                else if (recipient.Equals(_yourself))
                    await connection.InvokeAsync("SendMessageToCaller",
                        userTextBox.Text, messageTextBox.Text);
                else
                    await connection.InvokeAsync("SendMessageToUser",
                        recipient, userTextBox.Text, messageTextBox.Text);
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

    }
}
