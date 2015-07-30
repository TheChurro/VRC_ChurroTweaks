using System;
using System.Collections.Generic;
using UnityEngine;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * This class will call events when entities with a VRC_CT_ObjectTags script enter it.
     * The entity's ObjectTags must have the tag TagToTriggerEvent for the event to fire.
     * If TagToTriggerEvent is empty, then any object will trigger both the enter and exit events.
     * 
     * The EventTypesToUpdate list contains the string identifier for custom events that will 
     * recieve the entering GameObject as their object parameter when the object enters the trigger.
     * 
     * <example>
     * i.e. EnterEventTrigger == "DoStuffs" and EventTypesToUpdate.Contains("coolEvent").
     * If when calling the "DoStuffs" event the program finds a custom event under that name of type
     * "coolEvent" that event will have its ParameterObject replaced with the trigger object before
     * being called.
     * </example>
     * </summary>
     **/
	public class VRC_CT_TaggedObjectTriggerEnterEventTrigger : MonoBehaviour
	{
		public string TagToTriggerEvent;
	    public string EnterEventTrigger;
	    public string ExitEventTrigger;
		
        /**
         * <summary>
         * A list of CustomEvent names used when determining if an event should have
         * its ParameterObject changed.
         * </summary>
         **/
		public List<string> EventTypesToUpdate;

	    private VRC_EventHandler Handler;
		private VRC_CT_EventHandler ChurroHandler;

	    void Start()
	    {
	        Handler = gameObject.GetComponent<VRC_EventHandler>();

			if (EventTypesToUpdate.Count > 0)
			{
				ChurroHandler = gameObject.GetComponent<VRC_CT_EventHandler>();
			}
	    }

	    void OnTriggerEnter(Collider col)
	    {
			VRC_CT_ObjectTags tags = col.gameObject.GetComponent<VRC_CT_ObjectTags>();

	        if (tags != null && tags.hasTag(TagToTriggerEvent))
	        {
				if (ChurroHandler != null)
				{
					foreach (VRC_CT_CustomEvent e in ChurroHandler.CompiledEvents)
					{
						if (e.EventContents.Name == EnterEventTrigger && EventTypesToUpdate.Contains(e.EventName))
						{
							e.EventContents.ParameterObject = col.gameObject;
						}
					}
				}
	            Handler.TriggerEvent(EnterEventTrigger, VRC_EventHandler.VrcBroadcastType.Always);
	        }
	    }

	    void OnTriggerExit(Collider col)
	    {
			VRC_CT_ObjectTags tags = col.GetComponent<VRC_CT_ObjectTags>();
			
			if (tags != null && tags.hasTag(TagToTriggerEvent))
	        {
				if (ChurroHandler != null)
				{
					foreach (VRC_CT_CustomEvent e in ChurroHandler.CompiledEvents)
					{
						if (e.EventContents.Name == EnterEventTrigger && EventTypesToUpdate.Contains(e.EventName))
						{
							e.EventContents.ParameterObject = col.gameObject;
						}
					}
				}

	            Handler.TriggerEvent(ExitEventTrigger, VRC_EventHandler.VrcBroadcastType.Always);
	        }
	    }
	}
}