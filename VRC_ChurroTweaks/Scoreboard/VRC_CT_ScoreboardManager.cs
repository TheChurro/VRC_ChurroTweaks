using System.Collections.Generic;
using System;
using UnityEngine;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * A class that will keep track of different VRC_CT_ScoreboardValue scripts. 
     * This includes adjusting thier values (adding or removing scores, ticking timers)
     * and updating VRC_CT_Scoreboard scripts text values.
     * 
     * When a game ending value is reached on one of the ScoreboardValues this class
     * will call the "GameEnded" event on the event handler attached to the object, as well
     * as "GameStarted" when initial game setup is done after starting.
     * </summary>
     **/
	public class VRC_CT_ScoreboardManager : VRC_SerializableBehaviour
	{
        /**
         * <summary>
         * A list of GameObjects that have VRC_CT_Scoreboard scripts on them.
         * These scripts will be added to a list to be updated when the value
         * attached to them changes.
         * </summary>
         **/
	    public List<GameObject> Scoreboards;

	    private List<VRC_CT_ScoreboardValue> Values;
	    private List<VRC_CT_Scoreboard> ScoreboardTexts;

	    private VRC_EventHandler Handler;
	    private bool GameHasEnded = true;
	    private bool GameShouldEnd = false;

        private VRC_DataStorage Storage;

	    void Start()
	    {
	        Handler = gameObject.GetComponent<VRC_EventHandler>();
            Storage = gameObject.GetComponent<VRC_DataStorage>();
	        Values = new List<VRC_CT_ScoreboardValue>(gameObject.GetComponents<VRC_CT_ScoreboardValue>());
	        ScoreboardTexts = new List<VRC_CT_Scoreboard>();
	        foreach (GameObject g in Scoreboards)
	        {
	            ScoreboardTexts.AddRange(g.GetComponents<VRC_CT_Scoreboard>());
	        }
	    }

	    void Update()
	    {

	        if (GameHasEnded)
	        {
	            return;
	        }

            // Loop through values finding timer values and increment them

	        foreach (VRC_CT_ScoreboardValue v in Values)
	        {
	            if (v.Type == VRC_CT_ScoreboardValueTypes.COUNTDOWN_TIMER)
	            {
	                GameShouldEnd = GameShouldEnd || v.ChangeValue(-Time.deltaTime, Handler);
	            }
	            else if (v.Type == VRC_CT_ScoreboardValueTypes.COUNTUP_TIMER)
	            {
	                GameShouldEnd = GameShouldEnd || v.ChangeValue(Time.deltaTime, Handler);
	            }

	            foreach (VRC_CT_Scoreboard s in ScoreboardTexts)
	            {
                    // update Scoreboards that show the values
	                s.UpdateValue(v.ValueName, v.FormatValue());
	            }
	        }

	        if (GameShouldEnd)
	        {
	            EndGame();
	        }
	    }

        /**
         * <summary>
         * Add or subtract the VRC_CT_ScoreboardValue named "valueName" by "deltaValue"
         * </summary>
         **/
	    public void ChangeValue(string valueName, float deltaValue)
	    {
	        if (GameHasEnded)
	        {
	            return;
	        }

	        foreach (VRC_CT_ScoreboardValue v in Values)
	        {
	            if (v.ValueName == valueName)
	            {
	                GameShouldEnd = GameShouldEnd || v.ChangeValue(deltaValue, Handler);
	                
	                foreach (VRC_CT_Scoreboard s in ScoreboardTexts)
	                {
	                    s.UpdateValue(v.ValueName, v.FormatValue());
	                }

	                return;
	            }
	        }
	    }

        /**
         * <summary>
         * Set the VRC_CT_ScoreboardValue named "valueName" to "newValue"
         * </summary>
         **/
	    public void SetValue(string valueName, float newValue)
	    {
	        if (GameHasEnded)
	        {
	            return;
	        }

	        foreach (VRC_CT_ScoreboardValue v in Values)
	        {
	            if (v.ValueName == valueName)
	            {
	                GameShouldEnd = GameShouldEnd || v.SetValue(newValue, Handler);

	                foreach (VRC_CT_Scoreboard s in ScoreboardTexts)
	                {
	                    s.UpdateValue(v.ValueName, v.FormatValue());
	                }

	                return;
	            }
	        }
	    }

        /**
         * <summary>
         * Reset the VRC_CT_ScoreboardValue named "valueName"
         * </summary>
         **/
        public void ResetValue(string valueName)
        {
            if (GameHasEnded)
            {
                return;
            }

            foreach (VRC_CT_ScoreboardValue v in Values)
            {
                if (v.ValueName == valueName)
                {
                    v.ResetValue();

                    foreach (VRC_CT_Scoreboard s in ScoreboardTexts)
                    {
                        s.UpdateValue(v.ValueName, v.FormatValue());
                    }

                    return;
                }
            }
        }

        /**
         * <summary>
         * Get the VRC_CT_ScoreboardValue named "ValueName"
         * </summary>
         **/
        public int GetValue(string valueName)
        {
            foreach (VRC_CT_ScoreboardValue v in Values)
            {
                if (v.ValueName == valueName)
                {
                    return v.GetValue();
                }
            }

            return -1;
        }

        /**
         * <summary>
         * Triggers "GameEnded" on the attached VRC_EventHandler
         * </summary>
         **/
	    public void EndGame()
	    {
	        GameHasEnded = true;

	        Handler.TriggerEvent("GameEnded", VRC_EventHandler.VrcBroadcastType.Always);
	    }

        /**
         * <summary>
         * If the game is currently ended, reset all values to their starting values, begin timers
         * and allow value changes
         * </summary>
         **/
	    public void StartGame()
	    {
	        if (GameHasEnded)
	        {
	            GameHasEnded = false;
	            GameShouldEnd = false;

	            foreach (VRC_CT_ScoreboardValue v in Values)
	            {
	                v.ResetValue();

	                foreach (VRC_CT_Scoreboard s in ScoreboardTexts)
	                {
	                    s.UpdateValue(v.ValueName, v.FormatValue());
	                }
	            }

	            Handler.TriggerEvent("GameStarted", VRC_EventHandler.VrcBroadcastType.Always);
	        }
	    }

        public override byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            foreach (VRC_CT_ScoreboardValue v in Values)
            {
                if (v.Type == VRC_CT_ScoreboardValueTypes.COUNTDOWN_TIMER || v.Type == VRC_CT_ScoreboardValueTypes.COUNTUP_TIMER)
                {
                    byte[] strBytes = GetBytes(v.ValueName);
                    byte[] numString = System.BitConverter.GetBytes(strBytes.Length);
                    byte[] valueBytes = System.BitConverter.GetBytes(v.GetValue());
                    bytes.AddRange(numString);
                    bytes.AddRange(strBytes);
                    bytes.AddRange(valueBytes);
                }
            }

            return bytes.ToArray();
        }

        public byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public string GetString(byte[] bytes, int startIndex, int numBytes)
        {
            string output = "";

            for (int i = 0; i < numBytes; i += 2 )
            {
                char nextChar = System.BitConverter.ToChar(bytes, startIndex + i);
                output += nextChar;
            }
            return output;
        }

        public override void SetBytes(byte[] stream)
        {
            int index = 0;
            while (index < stream.Length)
            {
                int numBytesForString = System.BitConverter.ToInt32(stream, index);
                index += 4;
                string valueName = GetString(stream, index, numBytesForString);
                index += numBytesForString;
                int currentValue = System.BitConverter.ToInt32(stream, index);
                index += 4;

                foreach (VRC_CT_ScoreboardValue v in Values)
                {
                    if (v.ValueName.Equals(valueName))
                    {
                        v.SetValueForced(currentValue);
                    }
                }
            }
        }
	}

    /**
     * <summary>
     * A class that contains a value and helper functions used by ScoreboardManagers.
     * This class should be added to GameObjects that have a VRC_CT_ScoreboardManager.
     * This thing has a lot of options, but you shouldn't be wanting for functionality.
     * (I'll get to custom render support later)
     * </summary>
     **/
	public class VRC_CT_ScoreboardValue : MonoBehaviour
	{
        /**
         * <summary>
         * Unique identifier for the scoreboard manager to use
         * </summary>
         **/
	    public string ValueName = "";

        /**
         * <summary>
         * SCORE is for values that increase in value to reach a limit
         * STOCK is for values that decrease in value to reach a limit
         * COUNTUP_TIMER is for values that increase in value to reach a limit and will increase with time passing      
         * COUNTDOWN_TIMER is for values that decrease in value to reach a limit and will decreae with time passing
         * </summary>
         **/
	    public VRC_CT_ScoreboardValueTypes Type = VRC_CT_ScoreboardValueTypes.SCORE;
	    public float OriginalValue = 0;
	    private float CurrentValue = 0;
	    public bool CanGoBelowZero = false;
        /**
         * <summary>
         * When this ScoreboardValue reaches a certain value(Limit), will an event be called
         * </summary>
         **/
	    public bool IsLimited = false;
	    public float Limit = 0;
	    public string OnLimitReachedEvent = "";
        /**
         * <summary>
         * When this value hits it's limit should EndGame() be called on its ScoreboardManager
         * </summary>
         **/
	    public bool OnLimitReachedShouldEndGame = false;
        /**
         * <summary>
         * If true, once hitting its limit this value will not change until it is reset
         * </summary>
         **/
	    public bool IsHardLimit = true;
        /**
         * <summary>
         * If IsHardLimit == false and the ScoreboardValue falls below its limit, this event will be called
         * </summary>
         **/
	    public string OnSoftLimitLostEvent = "";
        /**
         * <summary>
         * When formatting the string representation show "CurrentValue / Limit"
         * </summary>
         **/
	    public bool ShowLimit = false;
        /**
         * <summary>
         * Can soft limits go past their limit
         * </summary>
         **/
	    public bool CanExceedLimit = false;

	    private bool HasReachedLimit = false;

	    public bool ChangeValue(float deltaValue, VRC_EventHandler handler)
	    {
	        if (!IsLimited)
	        {
	            CurrentValue += deltaValue;

	            if (CurrentValue < 0 && !CanGoBelowZero)
	            {
	                CurrentValue = 0;
	            }

	            return false;
	        }

	        if (!IsHardLimit || !HasReachedLimit)
	        {
	            CurrentValue += deltaValue;
	            if (CurrentValue < 0 && !CanGoBelowZero)
	            {
	                CurrentValue = 0;
	            }

	            if (!CanExceedLimit)
	            {
	                if (((int)Type) % 2 == 0)
	                {
	                    if (CurrentValue >= Limit)
	                    {
	                        CurrentValue = Limit;
	                    }
	                }
	                else
	                {
	                    if (CurrentValue <= Limit)
	                    {
	                        CurrentValue = Limit;
	                    }
	                }
	            }

	            if (deltaValue > 0)
	            {
	                if (CurrentValue - deltaValue < Limit && CurrentValue >= Limit)
	                {
	                    if ((((int)Type) % 2) == 0)
	                    {
	                        HasReachedLimit = true;
	                        handler.TriggerEvent(OnLimitReachedEvent, VRC_EventHandler.VrcBroadcastType.Always);
	                    }
	                    else
	                    {
	                        HasReachedLimit = false;
	                        handler.TriggerEvent(OnSoftLimitLostEvent, VRC_EventHandler.VrcBroadcastType.Always);
	                        return true;
	                    }
	                }
	            }
	            else
	            {
	                if (CurrentValue - deltaValue > Limit && CurrentValue <= Limit)
	                {
	                    if ((((int)Type) % 2) == 0)
	                    {
	                        HasReachedLimit = false;
	                        handler.TriggerEvent(OnSoftLimitLostEvent, VRC_EventHandler.VrcBroadcastType.Always);
	                    }
	                    else
	                    {
	                        HasReachedLimit = true;
	                        handler.TriggerEvent(OnLimitReachedEvent, VRC_EventHandler.VrcBroadcastType.Always);
	                    }
	                }
	            }
	        }

	        return (HasReachedLimit && OnLimitReachedShouldEndGame);
	    }

	    public bool SetValue(float newValue, VRC_EventHandler handler)
	    {
	        if (IsHardLimit && HasReachedLimit)
	        {
	            return OnLimitReachedShouldEndGame;
	        }

	        CurrentValue = newValue;

	        if (CurrentValue < 0 && !CanGoBelowZero)
	        {
	            CurrentValue = 0;
	        }

	        if (!CanExceedLimit)
	        {
	            if (((int)Type) % 2 == 0)
	            {
	                if (CurrentValue >= Limit)
	                {
	                    CurrentValue = Limit;
	                }
	            }
	            else
	            {
	                if (CurrentValue <= Limit)
	                {
	                    CurrentValue = Limit;
	                }
	            }
	        }

	        if (HasReachedLimit)
	        {
	            if ((int)Type % 2 == 0)
	            {
	                HasReachedLimit = CurrentValue >= Limit;
	            }
	            else
	            {
	                HasReachedLimit = CurrentValue <= Limit;
	            }

	            if (!HasReachedLimit)
	            {
	                handler.TriggerEvent(OnSoftLimitLostEvent, VRC_EventHandler.VrcBroadcastType.Always);
	            }
	        }
	        else
	        {
	            if ((int)Type % 2 == 0)
	            {
	                HasReachedLimit = CurrentValue >= Limit;
	            }
	            else
	            {
	                HasReachedLimit = CurrentValue <= Limit;
	            }

	            if (HasReachedLimit)
	            {
	                handler.TriggerEvent(OnLimitReachedEvent, VRC_EventHandler.VrcBroadcastType.Always);
	            }
	        }

	        return (HasReachedLimit && OnLimitReachedShouldEndGame);
	    }

        public int GetValue()
        {
            return (int)CurrentValue;
        }

        /**
         * <summary>
         * Return a string representation of the value
         * </summary>
         **/
	    public string FormatValue()
	    {
	        string output = "";

	        if ((int)Type < 2)
	        {
	            output = "" + CurrentValue;

	            if (ShowLimit)
	            {
	                output += " / " + Limit;
	            }
	        }
	        else
	        {
	            int rawInt = (int)CurrentValue;
	            int hour = rawInt / 3600;
	            int min = (rawInt % 3600) / 60;
	            int sec = rawInt % 60;

	            if (hour > 0)
	            {
	                output += hour + ":";
	            }

	            if (min > 0 || hour != 0)
	            {
	                if (min < 10)
	                {
	                    output += "0";
	                }
	                output += min + ":";
	            }

                if (sec < 10)
                {
                    output += "0";
                }

	            output += sec;

	            if (ShowLimit)
	            {
	                output += " / ";

	                rawInt = (int)Limit;
	                hour = rawInt / 3600;
	                min = (rawInt % 3600) / 60;
	                sec = rawInt % 60;

	                if (hour > 0)
	                {
	                    output += hour + ":";
	                }

	                if (min > 0 || hour != 0)
	                {
	                    if (min < 10)
	                    {
	                        output += "0";
	                    }
	                    output += min + ":";
	                }

                    if (sec < 10)
                    {
                        output += "0";
                    }

	                output += sec;
	            }
	        }

	        return output;
	    }

	    public void ResetValue()
	    {
	        CurrentValue = OriginalValue;
	        HasReachedLimit = false;
	    }

        /**
         * <summary>
         * This function allows you to set a ScoreboardValue's CurrentValue without checking to see if it can go there or
         * if it should fire events.
         * </summary>
         **/
        public void SetValueForced(int currentValue)
        {
            this.CurrentValue = currentValue;
        }
    }

	public enum VRC_CT_ScoreboardValueTypes
	{
	    SCORE, STOCK, COUNTUP_TIMER, COUNTDOWN_TIMER
	}
}