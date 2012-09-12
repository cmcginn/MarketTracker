using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Midi;
using MarketTracker.Core.Domain;
namespace MarketTracker.Audio.Extensions
{
    public static class AudioExtensions
    {
        const int SMOOTHING = 1000;
        const int CHANNEL = 2;
        const int VELOCITY = 100;
        const int DURATION = 100;
        const int MIDI_FILE_TYPE = 1;
        const int DELTA_TICKS_PER_QUARTER_NOTE = 1;
        const string EXPORT_DIRECTORY = "/Export";
        public static List<MidiEvent> ToMidiEvents(this List<Quote> quotes)
        {
            List<MidiEvent> result = new List<MidiEvent>();
            
            var min = quotes.Min(n => n.Close) * SMOOTHING;
            var max = quotes.Max(n => n.Close) * SMOOTHING;
            var scale = max-min;
            long absoluteTime = 0;
            quotes.ForEach(n =>
                {
                    var smoothClose = n.Close * SMOOTHING;
                    var percentage = (int)((1 - (max - smoothClose) / scale) * 100);
                    var noteOnEvent = new NoteOnEvent(absoluteTime, CHANNEL, percentage, VELOCITY, DURATION);
                    result.Add(noteOnEvent);
                    absoluteTime += 1;                    
                });
            return result;
        }

        public static MidiEventCollection ToMidiEventCollection(this List<MidiEvent> events, int track)
        {
            var result = new MidiEventCollection(MIDI_FILE_TYPE, DELTA_TICKS_PER_QUARTER_NOTE);
            events.ForEach(n => result.AddEvent(n, track));
            return result;
        }

        public static MidiEventCollection ToMidiEventCollection(this List<Quote> quotes, int track)
        {
            var events = quotes.ToMidiEvents();
            var result = new MidiEventCollection(MIDI_FILE_TYPE, DELTA_TICKS_PER_QUARTER_NOTE);
            events.ForEach(n => result.AddEvent(n, track));
            return result;
        }

        public static string WriteMidiFile(this MidiEventCollection midiEvents)
        {
            var path = String.Format("{0}/{1}.mid", Properties.Audio.Default.ExportDirectory, Guid.NewGuid());
            midiEvents.PrepareForExport();
            MidiFile.Export(path, midiEvents);
            return path;

        }
    }

   
}
