using ChatApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class Form1 : Form
    {

        //string data;
        //static int checkPort = 9555;
        static int sendPort = 9999;
        //NetworkStream stream;
        //TcpClient tcpClient = new TcpClient();
        //static TcpListener tcpListener = new TcpListener(IPAddress.Any, 0);
        UdpClient client1 = new UdpClient();
        UdpClient client2 = new UdpClient(sendPort);
        List<Person> people = new List<Person>();

        MemoryStream memoryStream = new MemoryStream();
        IFormatter bf = new BinaryFormatter();

        MemoryStream memoryStream2 = new MemoryStream();
        //IFormatter bf2 = new BinaryFormatter();



        public Form1()
        {
            InitializeComponent();
            btnSend.Enabled = false;
            

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
            tbNick.Enabled = false;

            client2.BeginReceive(new AsyncCallback(Listen1), null);
            timer1.Enabled = true;
            timer1.Interval = 1000;

            //client2.BeginReceive(new AsyncCallback(Listen2), null); //Udp

        }
        void Listen1(IAsyncResult ar) ///***
        {
            IPEndPoint remoteIp = new IPEndPoint(IPAddress.Any, sendPort);
            byte[] received = client2.EndReceive(ar, ref remoteIp);

            //data = Encoding.UTF8.GetString(received);

            memoryStream2 = new MemoryStream(received);
            Post receivedPost = (Post)bf.Deserialize(memoryStream2);

            if (receivedPost.MessageType == 0) // 0 = Isim ve online durumu
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    #region OnlineCheck


                    foreach (Person item in lbPersons.Items)
                    {
                        if (item.Ip == remoteIp.Address.ToString())
                        {
                            if (receivedPost.IsOnline == false)
                            {
                                item.IsOnline = receivedPost.IsOnline;
                                lbPersons.Items[GetLbItemIndex(item)] = item;

                                foreach (Person person in people)
                                {
                                    if (person.Ip == item.Ip)
                                    {
                                        person.IsOnline = receivedPost.IsOnline;
                                    }
                                }
                            }


                            //item.IsOnline = true;
                            //item.receiveDate = DateTime.Now;


                            break;
                        }
                    }


                    #endregion
                    Persons(remoteIp.Address.ToString(), receivedPost.Message);

                }));
            }
            else if (receivedPost.MessageType == 1)
            {


                this.Invoke(new MethodInvoker(delegate
                {

                    Person selectedItem = (Person)lbPersons.SelectedItem;



                    if (selectedItem != null && remoteIp.Address.ToString() == selectedItem.Ip)
                    {

                        foreach (Person person in people)
                        {
                            if (person.Ip == selectedItem.Ip)
                            {
                                rtbMessages.Text += $"{person.Name}: {receivedPost.Message}\n";
                                person.LastMessages.Add($"{person.Name}:{receivedPost.Message}");
                                selectedItem.LastMessages.Add($"{person.Name}:{receivedPost.Message}");
                                rtbMessages.SelectionStart = rtbMessages.TextLength;
                                rtbMessages.Focus();
                            }
                        }
                    }
                    else
                    {
                        foreach (Person person in people)
                        {
                            if (remoteIp.Address.ToString() == person.Ip)
                            {

                                person.UnreadMessage++;
                                person.LastMessages.Add($"{person.Name}: {receivedPost.Message}\t\t {DateTime.Now}");


                                foreach (Person person1 in lbPersons.Items)
                                {
                                    if (person.Ip == person1.Ip)
                                    {

                                        person1.Name = person.Name;
                                        person1.Name += $" ({person.UnreadMessage})";
                                        person1.LastMessages.Clear();
                                        for (int i = 0; i < person.LastMessages.Count(); i++)
                                        {
                                            person1.LastMessages.Add(person.LastMessages[i]);
                                        }
                                        Person checkListItem = (Person)lbPersons.Items[GetLbItemIndex(person1)];
                                        if (GetLbItemIndex(person1) != -1 && checkListItem.Ip == person1.Ip)
                                        {
                                            lbPersons.Items[GetLbItemIndex(person1)] = person1;

                                        }
                                        return;
                                    }
                                }

                            }
                        }

                    }


                }));

            }

            client2.BeginReceive(new AsyncCallback(Listen1), null);


        }
        //static int c = 0;
        public void Persons(string ip, string name)
        {
            bool flag = false;


            foreach (IPAddress iPAddress in GetLocalIp())
            {
                if (ip == iPAddress.ToString()/* && c < 2*/)
                {
                    //Person person = new Person(ip, name) { IsOnline = true, receiveDate = DateTime.Now, UnreadMessage = 0, LastMessages = new List<string>() };

                    //lbPersons.Items.Add(person);
                    //people.Add(new Person(ip, name) { IsOnline = true, receiveDate = DateTime.Now, UnreadMessage = 0, LastMessages = new List<string>() });

                    //lbPersons.Update();
                    //c++;
                    return;
                }
            }

            if (lbPersons.Items.Count == 0)
            {
                foreach (Person person1 in people)
                {
                    if (person1.Ip == ip)
                    {

                        // person.receiveDate = DateTime.Now;

                        person1.IsOnline = true; //!
                        person1.Name = name;
                        lbPersons.Items.Add(new Person(person1.Ip, person1.Name) { IsOnline = person1.IsOnline, LastMessages = person1.LastMessages, receiveDate = person1.receiveDate, UnreadMessage = person1.UnreadMessage });
                        lbPersons.Update();
                        return;
                    }

                }

                Person person = new Person(ip, name) { IsOnline = true, receiveDate = DateTime.Now, UnreadMessage = 0, LastMessages = new List<string>() };

                lbPersons.Items.Add(person);
                people.Add(new Person(ip, name) { IsOnline = true, receiveDate = DateTime.Now, UnreadMessage = 0, LastMessages = new List<string>() });

                lbPersons.Update();
                return;
            }

            foreach (Person item in lbPersons.Items)
            {
                if (item.Ip == ip)
                {

                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                foreach (Person person in people)
                {
                    if (person.Ip == ip)
                    {

                        // person.receiveDate = DateTime.Now;

                        person.IsOnline = true; //!
                        person.Name = name;
                        Person person_ = new Person(person.Ip, person.Name) { IsOnline = person.IsOnline, receiveDate = person.receiveDate, UnreadMessage = person.UnreadMessage, LastMessages = new List<string>() };

                        foreach (var message in person.LastMessages)
                        {
                            person_.LastMessages.Add(message);
                        }
                        lbPersons.Items.Add(person_);
                        lbPersons.Update();
                        return;
                    }

                }
            }


            if (!flag)
            {
                Person person = new Person(ip, name) { IsOnline = true, receiveDate = DateTime.Now, UnreadMessage = 0, LastMessages = new List<string>() };
                lbPersons.Items.Add(person);
                people.Add(new Person(ip, name) { IsOnline = true, receiveDate = DateTime.Now, UnreadMessage = 0, LastMessages = new List<string>() });
            }



        }
        public List<IPAddress> GetLocalIp()
        {
            List<IPAddress> iPAddresses = new List<IPAddress>();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    iPAddresses.Add(ipAddress);
                }
            }
            return iPAddresses;

        }

        static int timeCount = 0;
        private void timer1_Tick(object sender, EventArgs e) ///***
        {

            timeCount++;

            //UdpClient udpCheck = new UdpClient();//
            IPEndPoint remoteIp = new IPEndPoint(IPAddress.Parse("255.255.255.255"), sendPort);
            Post p = new Post { IsOnline = true, Message = tbNick.Text, MessageType = 0 };
            memoryStream2 = new MemoryStream();
            bf.Serialize(memoryStream2, p);

            byte[] sendData = memoryStream2.GetBuffer();
            client1.Send(sendData, sendData.Length, remoteIp);



            if (timeCount == 3)
            {
                //foreach (Person person in lbPersons.Items)
                //{
                //    if (person.receiveDate.Second + 6 < DateTime.Now.Second && person != lbPersons.SelectedItem)
                //    {
                //        lbPersons.Items.Remove(person);
                //        return;
                //    }


                //}
                try
                {


                    foreach (Person person in lbPersons.Items)//!!
                    {

                        if (person != lbPersons.SelectedItem && person.IsOnline == false)
                        {
                            lbPersons.Items.Remove(person);
                            lbPersons.Update();
                            timeCount = 0;
                            return;

                        }
                    }


                    timeCount = 0;
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Hata {ex}");
                }

            }


        }

        static List<string> nickUpdateCheckIp = new List<string>() { "" };
        private void lbPersons_SelectedIndexChanged(object sender, EventArgs e)///////////!!!!!!!!!!!
        {
            if (lbPersons.SelectedItem != null)
            {
                btnSend.Enabled = true;


                //if (lbPersons.SelectedItems.Count > 1)
                //{
                //    rtbMessages.Clear();



                //    //Person selectedItem = (Person)lbPersons.SelectedItem;
                //    //rtbMessages.Clear();
                //    //if (nickUpdateCheckIp != "" && selectedItem.Ip != nickUpdateCheckIp)
                //    //{
                //    //    Person updateItem = (Person)lbPersons.Items[GetLbItemIndex(new Person(nickUpdateCheckIp, ""))];
                //    //    foreach (Person person in people)
                //    //    {
                //    //        if (person.Ip == updateItem.Ip)
                //    //        {
                //    //            updateItem.Name = person.Name;
                //    //            lbPersons.Items[GetLbItemIndex(new Person(nickUpdateCheckIp, ""))] = updateItem;
                //    //            nickUpdateCheckIp = "";
                //    //            break;
                //    //        }
                //    //    }
                //    //}

                //    ////////////////////////////////////////////////////////////

                //    //Person selectedItem = (Person)lbPersons.SelectedItem;
                //    //if (selectedItem.LastMessages.Count != 0)
                //    //{
                //    //    foreach (string message in selectedItem.LastMessages)
                //    //    {
                //    //        rtbMessages.Text += message + "\n";////
                //    //    }

                //    //}
                //}
                /*else*/
                if (lbPersons.SelectedItems.Count == 1)
                {
                    for (int j = 0; j < lbPersons.SelectedItems.Count; j++)
                    {
                        Person selectedItem = (Person)lbPersons.SelectedItems[j];
                        rtbMessages.Clear();



                        for (int i = 0; i < nickUpdateCheckIp.Count; i++)
                        {
                            if (nickUpdateCheckIp[i] != "" && selectedItem.Ip != nickUpdateCheckIp[i])
                            {
                                Person updateItem = (Person)lbPersons.Items[GetLbItemIndex(new Person(nickUpdateCheckIp[i], ""))];
                                foreach (Person person in people)
                                {
                                    if (person.Ip == updateItem.Ip)
                                    {
                                        updateItem.Name = person.Name;
                                        lbPersons.Items[GetLbItemIndex(person)] = updateItem;
                                        nickUpdateCheckIp.RemoveAt(i);
                                        break;
                                    }
                                }
                            }
                        }
                        try
                        {
                            if (selectedItem.LastMessages.Count != 0)
                            {
                                foreach (string message in selectedItem.LastMessages)
                                {
                                    rtbMessages.Text += message + "\n";////
                                    rtbMessages.SelectionStart = rtbMessages.TextLength;
                                    rtbMessages.Focus();
                                }

                            }
                            foreach (Person person in people)
                            {
                                if (person.Ip == selectedItem.Ip)
                                {
                                    if (person.UnreadMessage > 0)
                                    {
                                        person.UnreadMessage = 0;
                                        //selectedItem.UnreadMessage = 0;
                                        nickUpdateCheckIp.Add(selectedItem.Ip);
                                        return;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }


                    }


                }

            }


        }

        #region TcpListen
        //public void TcpListen(IAsyncResult ar)
        //{

        //    TcpListener listener = (TcpListener)ar.AsyncState;
        //    tcpClient = listener.EndAcceptTcpClient(ar);

        //    using (stream = tcpClient.GetStream())
        //    {

        //        byte[] buffer = new byte[2048];
        //        stream.Read(buffer, 0, buffer.Length);
        //        int recv = 0;
        //        foreach (byte b in buffer)
        //        {
        //            if (b != 0)
        //            {
        //                recv++;

        //            }

        //        }
        //        string request = Encoding.UTF8.GetString(buffer, 0, recv);
        //        this.Invoke(new MethodInvoker(delegate
        //        {
        //            Person selectedItem = (Person)lbPersons.SelectedItem;
        //            rtbMessages.Focus();
        //            rtbMessages.AppendText($"{selectedItem.Name}: {request}\n");
        //            selectedItem.LastMessages += $"{selectedItem.Name}: {request}\n";

        //        }));
        //    }


        //    //listener.Stop();

        //    tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpListen), tcpListener);
        //}
        #endregion
        //void Listen2(IAsyncResult ar)  ///***
        //{

        //    IPEndPoint remoteIp = new IPEndPoint(IPAddress.Any, sendPort);
        //    byte[] received = client2.EndReceive(ar, ref remoteIp);
        //    memoryStream = new MemoryStream(received);
        //    //data = Encoding.UTF8.GetString(received);
        //    Post receivedPost = (Post)bf.Deserialize(memoryStream);



        //    client2.BeginReceive(new AsyncCallback(Listen2), null);
        //}

        public int GetLbItemIndex(Person person)
        {
            for (int i = 0; i < lbPersons.Items.Count; i++)
            {
                Person selectedItem = (Person)lbPersons.Items[i];
                if (person.Ip == selectedItem.Ip)
                {
                    return i;
                }
            }
            return -1;
        }
        private void btnSend_Click(object sender, EventArgs e)///***
        {

            #region UdpClient Object Send

            if (lbPersons.SelectedItems.Count > 1)
            {
                memoryStream = new MemoryStream();
                bf.Serialize(memoryStream, new Post { MessageType = 1, Message = tbMessage.Text.Trim() });
                byte[] sendData = memoryStream.GetBuffer();

                rtbMessages.Text += $"Sen: {tbMessage.Text.Trim()}\n";
                rtbMessages.SelectionStart = rtbMessages.TextLength;
                rtbMessages.Focus();
                foreach (Person selectedItem_ in lbPersons.SelectedItems)
                {
                    Person selectedItem = (Person)selectedItem_;
                    IPEndPoint remoteIp = new IPEndPoint(IPAddress.Parse(selectedItem.Ip), sendPort);

                    client2.Send(sendData, sendData.Length, remoteIp);


                    selectedItem.LastMessages.Add($"Sen: {tbMessage.Text.Trim()}");

                    foreach (Person person in people)
                    {
                        if (person.Ip == selectedItem.Ip)
                        {
                            person.LastMessages.Add($"Sen: {tbMessage.Text.Trim()}");
                        }
                    }

                }
                tbMessage.Clear();
            }
            else if (lbPersons.SelectedItems.Count == 1)
            {
                Person selectedItem = (Person)lbPersons.SelectedItem;
                memoryStream = new MemoryStream();
                IPEndPoint remoteIp = new IPEndPoint(IPAddress.Parse(selectedItem.Ip), sendPort);
                bf.Serialize(memoryStream, new Post { MessageType = 1, Message = tbMessage.Text.Trim() });
                byte[] sendData = memoryStream.GetBuffer();



                //client2.Connect(selectedItem.Ip, sendPort);
                client2.Send(sendData, sendData.Length, remoteIp);

                rtbMessages.Text += $"Sen: {tbMessage.Text.Trim()}\n";
                rtbMessages.SelectionStart = rtbMessages.TextLength;
                rtbMessages.Focus();
                selectedItem.LastMessages.Add($"Sen: {tbMessage.Text.Trim()}");
                foreach (Person person in people)
                {
                    if (person.Ip == selectedItem.Ip)
                    {
                        person.LastMessages.Add($"Sen: {tbMessage.Text.Trim()}");
                    }
                }
                tbMessage.Clear();
            }

            #endregion
            #region UdpClient Send
            //try
            //{

            //    Person selectedItem = (Person)lbPersons.SelectedItem;
            //    //IPEndPoint remoteIp = new IPEndPoint(IPAddress.Parse(selectedItem.Ip), sendPort);
            //    byte[] sendData = Encoding.UTF8.GetBytes(tbMessage.Text.Trim());

            //    client2.Connect(selectedItem.Ip, sendPort);
            //    client2.Send(sendData, sendData.Length);
            //    rtbMessages.Focus();
            //    rtbMessages.Text += $"Sen: {tbMessage.Text.Trim()}\n";
            //    selectedItem.LastMessages.Add($"Sen: {tbMessage.Text.Trim()}");
            //    foreach (Person person in people)
            //    {
            //        if (person.Ip == selectedItem.Ip)
            //        {
            //            person.LastMessages.Add($"Sen: {tbMessage.Text.Trim()}");
            //        }
            //    }
            //    tbMessage.Clear();
            //}
            //catch (NullReferenceException)
            //{

            //    MessageBox.Show("Mesaj göndermek istediğiniz kişiyi seçin", "Hata", MessageBoxButtons.OK);
            //}

            #endregion
            #region TcpClient Send
            //Person selectedItem = (Person)lbPersons.SelectedItem;
            //TcpClient tcpClient2 = new TcpClient();

            //tcpClient2.Connect(selectedItem.Ip, sendPort);

            //int byteCount = Encoding.UTF8.GetByteCount(tbMessage.Text.Trim() + 1);
            //byte[] sendData = new byte[byteCount];
            //sendData = Encoding.UTF8.GetBytes(tbMessage.Text.Trim());

            //NetworkStream stream = tcpClient2.GetStream();
            //stream.Write(sendData, 0, sendData.Length);

            //rtbMessages.Focus();
            //rtbMessages.AppendText($"Sen: {tbMessage.Text.Trim()}\n");
            //selectedItem.LastMessages += $"Sen: {tbMessage.Text.Trim()}\n";
            //tbMessage.Clear();

            //tcpClient2.Close();
            //stream.Close();
            #endregion
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            memoryStream = new MemoryStream();
            IPEndPoint remoteIp = new IPEndPoint(IPAddress.Parse("255.255.255.255"), sendPort);

            bf.Serialize(memoryStream, new Post { IsOnline = false, MessageType = 0 });

            byte[] sendData = memoryStream.GetBuffer();
            client1.Send(sendData, sendData.Length, remoteIp);


        }
    }
}
