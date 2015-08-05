using UnityEngine;
using VRCSDK2;
using System.Text;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * Class to handler loading in outside text documents.
     * This also includes a build in display mode.
     * </summary>
     **/
    class VRC_CT_ExternalTextView : MonoBehaviour
    {
        /**
         * <summary>
         * Location on the web of the .txt document
         * </summary>
         **/
        public string txtURL;

        /**
         * <summary>
         * The number of lines to display at one time
         * </summary>
         **/
        public int LinesToDisplay = 10;

        /**
         * <summary>
         * The display for the text
         * </summary>
         **/
        public TextMesh TextArea;

        /**
         * <summary>
         * Should the Text displayed be scrolling or in distinct parts
         * </summary>
         **/
        public bool Scroll = false;

        /**
         * <summary>
         * Should the Text wrap when the end or beginning hits
         * </summary>
         **/
        public bool Wrap = false;

        /**
         * <summary>
         * The words of the .txt document separated out into groups of LinesToDisplay
         * </summary>
         **/
        private string[] Text;
        private int currentTextGroup = 0;
        private WWW textDocument;

        public void Awake()
        {
            textDocument = new WWW(txtURL);

            StartCoroutine("Init");
        }

        public System.Collections.IEnumerator Init()
        {
            while (!textDocument.isDone)
            {
                yield return null;
            }

            SplitDocument();
        }

        public void SplitDocument()
        {
            string[] split = textDocument.text.Split(new char[] { '\n' });
            if (!Scroll)
            {
                Text = new string[(int)Mathf.Ceil(split.Length / (float)LinesToDisplay)];
                int curIndex = 0;
                int curPart = 0;
                StringBuilder textPart = new StringBuilder();
                while (curIndex < split.Length)
                {
                    for (int i = 0; i < 10 && i + curIndex < split.Length; i++)
                    {
                        textPart.Append(split[curIndex + i]);
                    }
                    Text[curPart] = textPart.ToString() + "\n";
                    textPart = new StringBuilder();

                    curIndex += 10;
                }
            }
            else
            {
                Text = split;
            }

            UpdateView();
        }

        public void UpdateView()
        {
            if (!Scroll)
            {
                TextArea.text = Text[currentTextGroup];
            }
            else
            {
                StringBuilder text = new StringBuilder();
                for (int i = 0; i < 10 && i + currentTextGroup < Text.Length; i++)
                {
                    text.Append(Text[i + currentTextGroup] + "\n");
                }
                TextArea.text = text.ToString();
            }
        }

        public void PageTurn(bool forwards)
        {
            if (forwards)
            {
                currentTextGroup++;
                if (currentTextGroup == Text.Length)
                {
                    if (Wrap)
                    {
                        currentTextGroup = 0;
                    }
                    else
                    {
                        currentTextGroup--;
                    }
                }
            }
            else
            {
                currentTextGroup--;
                if (currentTextGroup == -1)
                {
                    if (Wrap)
                    {
                        currentTextGroup = Text.Length - 1;
                    }
                    else
                    {
                        currentTextGroup = 0;
                    }
                }
            }
            UpdateView();
        }
    }

    public class VRC_CT_PageTurnEventSpawn : VRC_CT_CustomEventSpawn
    {
        public override VRC_CT_CustomEvent Create(VRC_EventHandler.VrcEvent e)
        {
            VRC_CT_PageTurnEvent customEvent = new VRC_CT_PageTurnEvent();
            customEvent.SetEvent(e);
            return customEvent;
        }
    }

    public class VRC_CT_PageTurnEvent : VRC_CT_CustomEvent
    {
        private VRC_CT_ExternalTextView view;

        public override void SetEvent(VRC_EventHandler.VrcEvent EventContents)
        {
            base.SetEvent(EventContents);
            view = EventContents.ParameterObject.GetComponent<VRC_CT_ExternalTextView>();
        }

        public override void TriggerEvent()
        {
            if (view != null)
            {
                view.PageTurn(EventContents.ParameterBool);
            }
        }
    }
}
