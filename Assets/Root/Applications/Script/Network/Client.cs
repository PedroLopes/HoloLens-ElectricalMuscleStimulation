using HoloToolkit.Unity;

namespace MuscleDeck
{
    public class Client : Singleton<Client>
    {
        public bool useTCP;
        public bool useUDP;

        //private void Awake()
        //{
        //    DontDestroyOnLoad(gameObject);
        //}

        public void SendString(string msg)
        {
#if !UNITY_EDITOR
        if (useTCP)
            TCPClient.Instance.SendString(msg);
        if (useUDP)
            UDPClient.Instance.SendString(msg);
#else
            RehaStimListener.DebugHandle(msg);
#endif
        }

        public void SendMessage(Message msg)
        {
            string serialized = msg.ToString();

            SendString(serialized);
        }

        public void SendMessage(MessageType type)
        {
            Message msg = new Message(type);
            SendMessage(msg);
        }

        public void SendMessage(MessageType type, BasicPayload payload)
        {
            Message msg = new Message(type, payload);
            SendMessage(msg);
        }
    }
}