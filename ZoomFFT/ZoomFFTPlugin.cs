using System.Windows.Forms;

using SDRSharp.Common;
using SDRSharp.Radio;
using System;

namespace SDRSharp.Average
{
    public class AveragePlugin : ISharpPlugin
    {
        private const string _displayName = "IF Average";

        private IFProcessor _ifProcessor;
        private ZoomPanel _controlPanel;


        public string DisplayName
        {
            get { return _displayName; }
        }

        public bool HasGui
        {
            get { return true; }
        }

        public UserControl Gui
        {
            get { return _controlPanel; }
        }

        public void Initialize(ISharpControl control)
        {
            Flags.Load();

            try
            {
                _ifProcessor = new IFProcessor(control);
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem with starting IF procesor"+ e.Source, "Important Message");
            }
          //  _ifProcessor.Control.Visible = Utils.GetBooleanSetting("enableZoomIF");
            
            _controlPanel = new ZoomPanel(_ifProcessor);
        }
        
        public void Close()
        {
            _ifProcessor.StopRecording();
            Flags.save();
            // Utils.SaveSetting("enableZoomIF", _ifProcessor.Control.Visible);
        }        
    }
}
