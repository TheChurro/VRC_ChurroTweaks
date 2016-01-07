using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRC_ChurroTweaks.ObjectManagement
{
    class VRC_CT_DestroyObjectEventSpawn : VRC_CT_CustomEventSpawn
    {
        public override VRC_CT_CustomEvent Create(CT_Event e)
        {
            VRC_CT_DestroyObjectEvent evt = new VRC_CT_DestroyObjectEvent();
            evt.SetEvent(e);
            return evt;
        }
    }

    class VRC_CT_DestroyObjectEvent : VRC_CT_CustomEvent
    {
        public override void TriggerEvent()
        {
            if (EventContents.getGameObjectPerferred0() != null)
            {
                UnityEngine.Object.Destroy(EventContents.getGameObjectPerferred0(), EventContents.ParameterFloat);
                if (EventContents.ParameterObject0 == null)
                    EventContents.ParameterObject1 = null;
                else
                    EventContents.ParameterObject0 = null;
            }
        }
    }
}
