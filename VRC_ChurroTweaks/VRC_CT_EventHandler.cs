﻿using System;
using System.Collections.Generic;
using UnityEngine;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * This class allows for the implementation of VRC_CT_CustomEvent scripts. Any VRC_CT_CustomEventSpawn scripts
     * added to a GameObject holding a VRC_CT_EventHandler or an ancestor of said object that has a VRC_CT_CustomEventCollector
     * will all that type of custom event to be used. When there is a CT_EventHandler on an object, add all of your Events
     * to the EventInstructions list. To indicate that an Event is to be a custom event format the ParameterString as so:
     * -EventTypeName:[Normal ParameterString], where EventTypeName is customizeable from the VRC_CT_CustomEventSpawn for that
     * event. Duplicate event type names default to the first CustomEventSpawner on the GameObject with the CT_EventHandler.
     * Only ten different EventNames can be used that have custom events under them per CT_EventHandler.
     * </summary>
     **/
	public class VRC_CT_EventHandler : MonoBehaviour
	{
	    public List<VRC_EventHandler.VrcEvent> EventInstructions;

	    public HashSet<VRC_CT_CustomEventSpawn> CustomEvents;

	    private VRC_EventHandler Handler;

	    private List<string> CustomEventNames;
	    public List<VRC_CT_CustomEvent> CompiledEvents;

	    void Start()
	    {
			FindCustomEvents();
	        CustomEventNames = new List<string>();
	        CompiledEvents = new List<VRC_CT_CustomEvent>();
	        Handler = gameObject.GetComponent<VRC_EventHandler>();
	        Compile();
	    }

		public void FindCustomEvents()
		{
			if (CustomEvents == null)
			{
				CustomEvents = new HashSet<VRC_CT_CustomEventSpawn>();
				foreach (VRC_CT_CustomEventSpawn es in gameObject.GetComponents<VRC_CT_CustomEventSpawn>())
				{
					CustomEvents.Add(es);
				}
			}
		}

	    private void Compile()
	    {
	        Handler.Events.Clear();
	        foreach (VRC_EventHandler.VrcEvent e in EventInstructions)
	        {
	            if (e.ParameterString.StartsWith("-"))
	            {
	                foreach (VRC_CT_CustomEventSpawn ep in CustomEvents)
	                {
	                    if (e.ParameterString.StartsWith("-" + ep.EventTypeName + ":"))
	                    {
	                        VRC_EventHandler.VrcEvent customEvent = CloneEvent(e);
	                        customEvent.ParameterString = customEvent.ParameterString.Substring(2 + ep.EventTypeName.Length);
							VRC_CT_CustomEvent compiledEvent = ep.Create(customEvent);
							compiledEvent.EventName = ep.EventTypeName;

                            if (ep.RequiresEventHandlerObject())
                            {
                                compiledEvent.SetEventHandlerGameObject(this.gameObject);
                            }
							CompiledEvents.Add(compiledEvent);

	                        if (!CustomEventNames.Contains(e.Name))
	                        {
	                            VRC_EventHandler.VrcEvent customEventTrigger = new VRC_EventHandler.VrcEvent();
	                            customEventTrigger.Name = e.Name;
	                            customEventTrigger.EventType = VRC_EventHandler.VrcEventType.SendMessage;
	                            customEventTrigger.ParameterString = "TriggerEvent" + CustomEventNames.Count;
	                            customEventTrigger.ParameterObject = this.gameObject;

	                            Handler.Events.Add(customEventTrigger);
	                            CustomEventNames.Add(e.Name);
	                        }
	                    }
	                }
	            }
	            else
	            {
	                Handler.Events.Add(CloneEvent(e));
	            }
	        }
	    }

	    public VRC_EventHandler.VrcEvent CloneEvent(VRC_EventHandler.VrcEvent e)
	    {
	        VRC_EventHandler.VrcEvent returnEvent = new VRC_EventHandler.VrcEvent();

	        returnEvent.Name = e.Name;
	        returnEvent.EventType = e.EventType;
	        returnEvent.ParameterBool = e.ParameterBool;
	        returnEvent.ParameterBoolOp = e.ParameterBoolOp;
	        returnEvent.ParameterFloat = e.ParameterFloat;
	        returnEvent.ParameterObject = e.ParameterObject;
	        returnEvent.ParameterString = e.ParameterString;

	        return returnEvent;
	    }

	    public void TriggerEvent0()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[0])
	            {
	                e.TriggerEvent();
	            }
	        }
	    }

	    public void TriggerEvent1()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[1])
	            {
	                e.TriggerEvent();
	                
	            }
	        }
	    }

	    public void TriggerEvent2()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[2])
	            {
	                e.TriggerEvent();
	            }
	        }
	    }

	    public void TriggerEvent3()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[3])
	            {
	                e.TriggerEvent();
	            }
	        }
	    }

	    public void TriggerEvent4()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[4])
	            {
	                e.TriggerEvent();
	            }
	        }
	    }

	    public void TriggerEvent5()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[5])
	            {
	                e.TriggerEvent();
	            }
	        }
	    }

	    public void TriggerEvent6()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[6])
	            {
	                e.TriggerEvent();
	            }
	        }
	    }

	    public void TriggerEvent7()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[7])
	            {
	                e.TriggerEvent();
	            }
	        }
	    }

	    public void TriggerEvent8()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[8])
	            {
	                e.TriggerEvent();
	            }
	        }
	    }

	    public void TriggerEvent9()
	    {
	        foreach (VRC_CT_CustomEvent e in CompiledEvents)
	        {
	            if (e.GetEvent().Name == CustomEventNames[9])
	            {
	                e.TriggerEvent();
	            }
	        }
	    }
	}
}