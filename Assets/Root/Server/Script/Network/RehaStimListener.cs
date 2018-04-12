using System;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class RehaStimListener : UDPServer
    {
#if UNITY_EDITOR
        private static RehaStimListener _Instance;
        private static GameObject _Container;
        protected bool initialized = false;

        protected Dictionary<MessageType, AbstractMessageHandler> handlers;

        public static RehaStimListener Default
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<RehaStimListener>();

                    if (_Instance == null)
                    {
                        _Container = new GameObject("RehaStimListener");
                        _Container.AddComponent<RehaStimListener>();

                        _Instance = _Container.GetComponent<RehaStimListener>();
                    }
                }
                _Instance.Init();

                return _Instance;
            }
        }

        public void Init()
        {
            if (!initialized)
            {
                InitHandlers();
                initialized = true;
            }
        }

        protected void InitHandlers()
        {
            handlers = new Dictionary<MessageType, AbstractMessageHandler>();
            handlers.Add(MessageType.Solid, HandlerFactory<SolidHandler>.Get());
            handlers.Add(MessageType.Electro, HandlerFactory<ElectroHandler>.Get());
            handlers.Add(MessageType.SliderEnd, HandlerFactory<SliderEndHandler>.Get());
            handlers.Add(MessageType.Bump, HandlerFactory<BumpHandler>.Get());
            handlers.Add(MessageType.Hole, HandlerFactory<HoleHandler>.Get());
            handlers.Add(MessageType.DetentSlider, HandlerFactory<DetentHandler>.Get());
            handlers.Add(MessageType.Tracking, HandlerFactory<TrackingHandler>.Get());
            // Todo Change to real handler
            handlers.Add(MessageType.ButtonContact, HandlerFactory<ButtonHandler>.Get());
            handlers.Add(MessageType.ButtonUpdate, HandlerFactory<ButtonHandler>.Get());
            handlers.Add(MessageType.DetentDial, HandlerFactory<DialDetentHandler>.Get());

            // Pilot Stuff
            handlers.Add(MessageType.GridButtonCodeContact, HandlerFactory<GridButtonCodeHandler>.Get());
            handlers.Add(MessageType.GridButtonCodeUpdate, HandlerFactory<GridButtonCodeHandler>.Get());
            handlers.Add(MessageType.GridButtonContact, HandlerFactory<GridButtonHandler>.Get());
            handlers.Add(MessageType.GridButtonUpdate, HandlerFactory<GridButtonHandler>.Get());

            // Catapult
            handlers.Add(MessageType.CatapultArm, HandlerFactory<CatapultHandler>.Get());

            // Box
            handlers.Add(MessageType.BoxTouching, HandlerFactory<BoxHandler>.Get());

            // Marble
            handlers.Add(MessageType.MarbleImpact, HandlerFactory<MarbleHandler>.Get());

            // Walkthrough
            handlers.Add(MessageType.WalkthroughDial, HandlerFactory<WalkthroughHandler>.Get());
        }

        protected override void Start()
        {
            base.Start();
            Init();
        }

        public static void DebugHandle(string msg)
        {
            RehaStimListener.Default.processMessage(msg);
        }

        [Tooltip("max Value in ms")]
        public int maxWidth = 200;

        public bool printDebug = false;

        protected override void processMessage(string raw)
        {
            if (printDebug)
                Debug.Log("Got message: " + raw);

            try
            {
                Message msg = JsonUtility.FromJson<Message>(raw);
                AbstractMessageHandler handler;
                switch (msg.type)
                {
                    case MessageType.Solid:
                    case MessageType.Electro:
                    case MessageType.SliderEnd:
                    //case MessageType.Bump:
                    //case MessageType.Hole:
                    case MessageType.DetentSlider:
                    case MessageType.Tracking:
                    case MessageType.Push:
                    case MessageType.ButtonContact:
                    case MessageType.ButtonUpdate:
                    case MessageType.DetentDial:
                        if (handlers.TryGetValue(msg.type, out handler))
                            handler.HandleMessage(msg);
                        else
                            print("no handler " + msg.type);
                        break;

                    case MessageType.Stop:
                        ChannelList.Stop();
                        break;

                    case MessageType.Bump:
                    case MessageType.Hole:
                        break;

                    //Pilot Stuff
                    //case MessageType.ButtonGridCode:
                    //    print(msg.payload.ToString());
                    //    break;

                    case MessageType.GridButtonUpdate:
                    case MessageType.GridButtonCodeUpdate:
                    case MessageType.GridButtonContact:
                    case MessageType.GridButtonCodeContact:
                        //case MessageType.PullSlider:
                        if (handlers.TryGetValue(msg.type, out handler))
                            handler.HandleMessage(msg);
                        else
                            print("no handler " + msg.type);
                        break;

                    // Catapult
                    case MessageType.CatapultArm:
                    case MessageType.CatapultRelease:
                    case MessageType.CatapultTarget:
                        if (handlers.TryGetValue(MessageType.CatapultArm, out handler))
                            handler.HandleMessage(msg);
                        else
                            print("no handler " + msg.type);
                        break;

                    // Box Game
                    case MessageType.BoxTouching:
                    case MessageType.BoxMoving:
                    case MessageType.BoxLedge:
                        if (handlers.TryGetValue(MessageType.BoxTouching, out handler))
                            handler.HandleMessage(msg);
                        else
                            print("no handler " + msg.type);
                        break;

                    // Marble Game
                    case MessageType.MarbleImpact:
                    case MessageType.MarblePosition:
                    case MessageType.MarbleSuccess:
                    case MessageType.MarbleWallCollision:
                        if (handlers.TryGetValue(MessageType.MarbleImpact, out handler))
                            handler.HandleMessage(msg);
                        else
                            print("no handler " + msg.type);
                        break;

                    // Walkthrough
                    case MessageType.WalkthroughDial:
                    case MessageType.WalkthroughDialDetent:
                    case MessageType.WalkthroughDialStatus:
                    case MessageType.WalkthroughButton:
                    case MessageType.WalkthroughButtonContact:
                    case MessageType.WalkthroughButtonPosition:
                    case MessageType.EscapeElectro:
                    case MessageType.EscapeDetent:
                        if (handlers.TryGetValue(MessageType.WalkthroughDial, out handler))
                            handler.HandleMessage(msg);
                        else
                            print("no handler " + msg.type);
                        break;

                    default:
                        print("Unknown Message: " + raw);
                        break;
                }
            }
            catch (Exception e) { }
        }

#endif
    }
}