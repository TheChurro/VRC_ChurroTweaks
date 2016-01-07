using UnityEngine;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * This class will create VRC_CT_ScoreboardEvents. These events use 
     * -- <value> ParameterObject </value> = a GameObject that has a VRC_CT_ScoreboardManager script. This will be used
     * for finding a VRC_CT_ScoreboardValue with name [ValueName]. If not given will use the GameObject that the
     * EventHandler script is on--
     * -- <value> ParameterString </value> = "change:[ValueName]" or "set:[ValueName]" or "start" or "end"
     * or "reset:[ValueName]" --
     * -- <value> ParameterFloat </value> = the amount to "change" or "set" [ValueName] by or to --
     * </summary>
     **/
    public class VRC_CT_ScoreboardEventSpawn : VRC_CT_CustomEventSpawn
	{
	    public override VRC_CT_CustomEvent Create(CT_Event e)
	    {
	        VRC_CT_ScoreboardEvent Event = new VRC_CT_ScoreboardEvent();
	        Event.SetEvent(e);
	        return Event;
	    }

        public override bool RequiresEventHandlerObject()
        {
            return true;
        }
	}

    public class VRC_CT_ScoreboardEvent : VRC_CT_CustomEvent
	{
        private VRC_CT_ScoreboardManager manager;

        public override void SetEvent(CT_Event EventContents)
        {
            base.SetEvent(EventContents);
            if (EventContents.ParameterObject0 != null)
            {
                manager = EventContents.ParameterObject0.GetComponent<VRC_CT_ScoreboardManager>();
            }
        }

        public override void SetEventHandlerGameObject(GameObject obj)
        {
            if (manager == null)
            {
                manager = obj.GetComponent<VRC_CT_ScoreboardManager>();
            }
        }

	    public override void TriggerEvent()
	    {
			if (EventContents.ParameterString.StartsWith("change:"))
	        {
				manager.ChangeValue(EventContents.ParameterString.Substring("change:".Length), EventContents.ParameterFloat);
	        }
			else if (EventContents.ParameterString.StartsWith("set:"))
	        {
				manager.SetValue(EventContents.ParameterString.Substring("set:".Length), EventContents.ParameterFloat);
	        }
			else if (EventContents.ParameterString.StartsWith("start"))
	        {
	            manager.StartGame();
	        }
            else if (EventContents.ParameterString.StartsWith("end"))
            {
                manager.EndGame();
            }
            else if (EventContents.ParameterString.StartsWith("reset:"))
            {
                manager.ResetValue(EventContents.ParameterString.Substring("reset:".Length));
            }
	    }
	}
}