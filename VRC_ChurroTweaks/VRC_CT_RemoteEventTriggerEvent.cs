using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * This class creates VRC_CT_RemoteEventTriggerEvents. These events will trigger an event on the EventHandler on its
     * ParameterObject. This object need not be a child of the GameObject which the Event is called from. These events use...
     * -- <value> ParameterString </value> : The EventName to call --
     * -- <value> ParameterBoolOp </value> : If True, the EventHandler used can change, If False the EventHandler used cannot
     * change --
     * -- <value> ParameterObject </value> : The GameObject with the EventHandler to trigger events on --
     * </summary>
     **/
    public class VRC_CT_RemoteEventTriggerEventSpawn : VRC_CT_CustomEventSpawn
    {
        public override VRC_CT_CustomEvent Create(VRCSDK2.VRC_EventHandler.VrcEvent e)
        {
            VRC_CT_RemoteEventTriggerEvent customEvent = new VRC_CT_RemoteEventTriggerEvent();
            customEvent.SetEvent(e);
            return customEvent;
        }
    }

    public class VRC_CT_RemoteEventTriggerEvent : VRC_CT_CustomEvent
    {
        private VRC_EventHandler handler;

        public override void SetEvent(VRC_EventHandler.VrcEvent EventContents)
        {
            EventContents.ParameterBool = VRC_EventHandler.BooleanOp(EventContents.ParameterBoolOp, true);
            base.SetEvent(EventContents);

            if (!EventContents.ParameterBool)
            {
                handler = EventContents.ParameterObject.GetComponent<VRC_EventHandler>();
            }
        }

        public override void TriggerEvent()
        {
            if (EventContents.ParameterBool)
            {
                handler = EventContents.ParameterObject.GetComponent<VRC_EventHandler>();
            }

            handler.TriggerEvent(EventContents.ParameterString, EventContents.ParameterFloat == 0 ? VRC_EventHandler.VrcBroadcastType.Always
                : EventContents.ParameterFloat > 1 ? VRC_EventHandler.VrcBroadcastType.Master : VRC_EventHandler.VrcBroadcastType.Local);
        }
    }
}
