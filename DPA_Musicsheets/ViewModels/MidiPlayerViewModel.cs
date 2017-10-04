using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sanford.Multimedia.Midi;
using System;

namespace DPA_Musicsheets.ViewModels
{
    public class MidiPlayerViewModel : ViewModelBase
    {
        private FileHandler _fileHandler;

        private OutputDevice _outputDevice;

        // De sequencer maakt het mogelijk om een sequence af te spelen.
        // Deze heeft een timer en geeft events op de juiste momenten.
        private Sequencer _sequencer;

        private bool _running;

        public MidiPlayerViewModel(FileHandler fileHandler)
        {
            // De OutputDevice is een midi device of het midikanaal van je PC.
            // Hierop gaan we audio streamen.
            // DeviceID 0 is je audio van je PC zelf.
            _outputDevice = new OutputDevice(0);
            _sequencer = new Sequencer();

            // Wanneer een channelmessage langskomt sturen we deze direct door naar onze audio.
            // Channelmessages zijn tonen met commands als NoteOn en NoteOff
            // In midi wordt elke noot gespeeld totdat NoteOff is benoemd. Wanneer dus nooit een NoteOff komt nadat die een NoteOn heeft gehad
            // zal deze note dus oneindig lang blijven spelen.
            _sequencer.ChannelMessagePlayed += ChannelMessagePlayed;

            // Wanneer de sequence klaar is moeten we alles closen en stoppen.
            _sequencer.PlayingCompleted += (playingSender, playingEvent) =>
            {
                _sequencer.Stop();
                _running = false;
            };

            _fileHandler = fileHandler;
            _fileHandler.MidiSequenceChanged += (src, args) =>
            {
                StopCommand.Execute(null);
                _sequencer.Sequence = args.MidiSequence;
                UpdateButtons();
            };
        }

        private void UpdateButtons()
        {
            PlayCommand.RaiseCanExecuteChanged();
            PauseCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

        private void ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            try
            {
                _outputDevice.Send(e.Message);
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is OutputDeviceException)
            {
                // Don't crash when we can't play
                // We have to do it this way because IsDisposed on
                // _outDevice may be false when it is being disposed
                // so this is the only safe way to prevent race conditions
            }
        }

        public RelayCommand PlayCommand => new RelayCommand(() =>
        {
            if (!_running)
            {
                _running = true;
                _sequencer.Continue();
                UpdateButtons();
            }
        }, () => !_running && _sequencer.Sequence != null);

        public RelayCommand StopCommand => new RelayCommand(() =>
        {
            _running = false;
            _sequencer.Stop();
            _sequencer.Position = 0;
            UpdateButtons();
        }, () => _running);

        public RelayCommand PauseCommand => new RelayCommand(() =>
        {
            _running = false;
            _sequencer.Stop();
            UpdateButtons();
        }, () => _running);

        public override void Cleanup()
        {
            base.Cleanup();

            _sequencer.Stop();
            _sequencer.Dispose();
            _outputDevice.Dispose();
        }
    }
}
