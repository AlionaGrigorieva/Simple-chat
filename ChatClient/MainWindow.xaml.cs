using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool IsConnected = false;
        ServiceChatClient client;
        int ID;
        public MainWindow()
        {
            InitializeComponent();
        }
        void ConnectU()
        {
            if (!IsConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(UserNameTextbox.Text);
                UserNameTextbox.IsEnabled = false; 
                ConnectButton.Content = "Disconnect";
                IsConnected = true;
            }
        }
        void DisconnectU()
        {
            if (IsConnected)
            {
                client.Disconnect(ID);
                client = null;
                UserNameTextbox.IsEnabled = true;
                ConnectButton.Content = "Connect";
                IsConnected = false;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
            {
                DisconnectU();
            }
            else
            {
                ConnectU();
            }
        }

        public void MsgCallback(string msg)
        {
            ChatListbox.Items.Add(msg);
            ChatListbox.ScrollIntoView(ChatListbox.Items[ChatListbox.Items.Count-1]);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectU();
        }

        private void MessageTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (client != null)
                {
                    client.SendMsg(MessageTextbox.Text, ID);
                    MessageTextbox.Text = "";
                }
            }
        }
    }
}
