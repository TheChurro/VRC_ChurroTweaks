using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * This class will create VRC_CT_ScoreboardComparatorEvents.
     * These events use
     * -- <value> ParameterObject </value>. This is the object with the VRC_CT_ScoreboardManager. If null the GameObject
     * with the event will be used instead
     * -- <value> ParameterString </value>. This should be formatted as a ternary expression in general. You can leave
     * out the else case, but generally it follows [ScoreboardValueName] [Integer Conditionals]
     * [ScoreboardValueName] ? [TrueCaseEventName] : [FalseCaseEventName]
     * -- Full list of formattings is given...
     * 
     * key:
     * only one value may be an Integer Value, the other must be a ScoreboardValue --
     * (Integer Conditionals) = (Combination of greater than, less than, equal to and not[!]) --
     * (EventName1) is triggered if the operation returns true --
     * (EventName2) is triggered if the operation returns false --
     * 
     * --
     * 
     * (ScoreboardValueName or Integer Value) (space here) (Integer Conditionals) (ScoreboardValueName or Integer Value)
     * (space here) (EventName1)
     * 
     * -- or --
     * 
     * (ScoreboardValueName or Integer Value) (space here) (Integer Conditionals) (ScoreboardValueName or Integer Value)
     * (space here) ("?") (space here) (EventName1) 
     * [optionally you can add either {(space here) (EventName2)} or {(space here) (":") (space here) (EventName2)}]
     * 
     * -- or --
     * 
     * (ScoreboardValueName or Integer Value) (space here) (Integer Conditionals) (ScoreboardValueName or Integer Value)
     * (space here) (":") (space here) (EventName2)
     * 
     * </summary>
     **/
    public class VRC_CT_ScoreboardComparatorEventSpawn : VRC_CT_CustomEventSpawn
    {
        /**
         * <summary>
         * If you want the Scoreboard's EventHandler to recieve Comparator Event triggers then set this to true,
         * setting it to false will use the EventHandler that the ComaratorEvent is on.
         * </summary>
         **/
        public bool UseScoreboardHandler = true;

        public override bool RequiresEventHandlerObject()
        {
            return true;
        }

        public override VRC_CT_CustomEvent Create(CT_Event e)
        {
            VRC_CT_ScoreboardComparatorEvent compareEvent = new VRC_CT_ScoreboardComparatorEvent();
            compareEvent.SetEvent(e);
            compareEvent.UseScoreboardHandler = UseScoreboardHandler;
            return compareEvent;
        }
    }

    public class VRC_CT_ScoreboardComparatorEvent : VRC_CT_CustomEvent
    {
        public bool UseScoreboardHandler = true;

        private bool didLoad = false;

        private string Value1;
        private int intValue1;
        private string Value2;
        private int intValue2;
        private int compareBehavior = 0;
        private string compareTrueEvent;
        private string compareFalseEvent;

        private int LESS_THAN = 1;
        private int GREATER_THAN = 2;
        private int EQUALS = 4;

        private VRC_CT_ScoreboardManager scoreboard;
        private VRC_EventHandler handler;

        public override void SetEvent(CT_Event EventContents)
        {
            base.SetEvent(EventContents);
            string[] stringSplit = EventContents.ParameterString.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);

            VRC_CT_EventHandler.print("StringSplit.Length = " + stringSplit.Length);

            if (EventContents.ParameterObject0 != null)
            {
                scoreboard = EventContents.ParameterObject0.GetComponent<VRC_CT_ScoreboardManager>();
                handler = EventContents.ParameterObject0.GetComponent<VRC_EventHandler>();
            }

            if (stringSplit.Length < 5 || (stringSplit.Length == 4 && !(stringSplit[3].Equals(":") || stringSplit[3].Equals("?"))))
            {
                return;
            }

            Value1 = stringSplit[0];
            try
            {
                intValue1 = int.Parse(Value1);
                Value1 = "";
            }
            catch (Exception e) { }
            if (Value1 != "" && Value1.StartsWith("\"") && Value1.EndsWith("\""))
            {
                Value1 = Value1.Substring(1, Value1.Length - 2);
            }

            Value2 = stringSplit[2];
            try
            {
                intValue2 = int.Parse(Value2);
                Value2 = "";
            }
            catch (Exception e) { }
            if (Value2 != "" && Value2.StartsWith("\"") && Value2.EndsWith("\""))
            {
                Value2 = Value2.Substring(1, Value2.Length - 2);
            }

            if (Value1 == Value2)
            {
                return;
            }

            VRC_CT_EventHandler.print("Values have been found: " + (Value1 == "" ? intValue1.ToString() : Value1) + " and " + (Value2 == "" ? intValue2.ToString() : Value2));

            try
            {
                compareBehavior = int.Parse(stringSplit[1]);
                if (compareBehavior < 0)
                {
                    compareBehavior = 0;
                }
                else if (compareBehavior > 7)
                {
                    compareBehavior = 7;
                }
            }
            catch (Exception e)
            {
                if (stringSplit[1].Contains("<"))
                {
                    compareBehavior |= LESS_THAN;
                }
                if (stringSplit[1].Contains(">"))
                {
                    compareBehavior |= GREATER_THAN;
                }
                if (stringSplit[1].Contains("="))
                {
                    compareBehavior |= EQUALS;
                }
                if (stringSplit[1].Contains("!"))
                {
                    int newBehavior = 0;

                    if ((compareBehavior & LESS_THAN) == 0)
                    {
                        newBehavior |= LESS_THAN;
                    }

                    if ((compareBehavior & GREATER_THAN) == 0)
                    {
                        newBehavior |= GREATER_THAN;
                    }

                    if ((compareBehavior & EQUALS) == 0)
                    {
                        newBehavior |= EQUALS;
                    }
                    compareBehavior = newBehavior;
                }
            }

            // Operator Compiled: 0 - Always False, 1 - Less Than, 2 - Greater Than, 3 - Not, 
            // 4 - Equals, 5 - Less Than or Equal To, 6 - Greater Than or Equal To, 7 - Always True

            if (stringSplit[3].Equals("?"))
            {
                // '?' and True Case
                compareTrueEvent = stringSplit[4];

                if (stringSplit.Length > 5)
                {
                    if (stringSplit[5].Equals(":") && stringSplit.Length > 6)
                    {
                        // ':' and False Case
                        compareFalseEvent = stringSplit[6];
                    }
                    else
                    {
                        // False Case
                        compareFalseEvent = stringSplit[5];
                    }
                }
            }
            else if (stringSplit[3].Equals(":"))
            {
                // ':' and False Case
                compareFalseEvent = stringSplit[4];
            }
            else
            {
                // True Case
                compareTrueEvent = stringSplit[3];

                if (stringSplit.Length > 4)
                {
                    if (stringSplit[4].Equals(":") && stringSplit.Length > 5)
                    {
                        // ':' False Case
                        compareFalseEvent = stringSplit[5];
                    }
                    else
                    {
                        // False Case
                        compareFalseEvent = stringSplit[4];
                    }
                }
            }

            didLoad = true;
        }

        public override void SetEventHandlerGameObject(UnityEngine.GameObject obj)
        {
            if (handler == null || !UseScoreboardHandler)
            {
                handler = obj.GetComponent<VRC_EventHandler>();
            }

            if (scoreboard == null)
            {
                scoreboard = obj.GetComponent<VRC_CT_ScoreboardManager>();
            }

            didLoad = handler != null && scoreboard != null && didLoad;
        }

        public override void TriggerEvent()
        {
            if (didLoad)
            {
                if (Value1 != "")
                {
                    intValue1 = scoreboard.GetValue(Value1);
                }
                if (Value2 != "")
                {
                    intValue2 = scoreboard.GetValue(Value2);
                }

                bool returnTrue = false;

                if ((compareBehavior & LESS_THAN) == LESS_THAN)
                {
                    returnTrue |= intValue1 < intValue2;
                }
                if ((compareBehavior & GREATER_THAN) == GREATER_THAN)
                {
                    returnTrue |= intValue1 > intValue2;
                }
                if ((compareBehavior & EQUALS) == EQUALS)
                {
                    returnTrue |= intValue1 == intValue2;
                }

                if (returnTrue)
                {
                    handler.TriggerEvent(compareTrueEvent, VRC_EventHandler.VrcBroadcastType.Always);
                    VRC_CT_EventHandler.print("Trigger Event: " + compareTrueEvent);
                }
                else
                {
                    handler.TriggerEvent(compareFalseEvent, VRC_EventHandler.VrcBroadcastType.Always);
                    VRC_CT_EventHandler.print("Trigger Event: " + compareFalseEvent);
                }
            }
        }
    }
}
