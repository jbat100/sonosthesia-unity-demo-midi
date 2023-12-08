# sonosthesia-unity-demo-midi

This Unity application demonstrates the sonosthesia MIDI modules. These modules aim to provide an abstraction layer for MIDI messages, with a human readable API for MIDI input and output. The core message types and abstract input and output APIs are provided by

- [com.sonosthesia.adaptivemidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.adaptivemidi)

While concrete implementations are provided by 

- [com.sonosthesia.rtmidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.rtmidi)
- [com.sonosthesia.timelinemidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.timelinemidi)
- [com.sonosthesia.pack](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.pack)

# Installation

Note that to add those packages to your Unity project you will need to add the following scoped registeries to your `Packages/package.json` file (`Keijiro` only necessary for [com.sonosthesia.rtmidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.rtmidi))


```
"scopedRegistries": [
    {
      "name": "Neuecc",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.neuecc.unirx",
        "com.cysharp.unitask"
      ]
    },
    {
      "name": "Keijiro",
      "url": "https://registry.npmjs.com",
      "scopes": [
        "jp.keijiro"
      ]
    },
    {
      "name": "Sonosthesia",
      "url": "https://registry.npmjs.com",
      "scopes": [
        "com.sonosthesia"
      ]
    }
  ]
```

# Scenes

UI test and monitoring tools which plug into the abstraction layer API are provided by 

## MIDI I/O Scenes

### RtMIDIInput

![MIDIKeys_short](https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/0ba0138f-77af-492f-9f02-ee240a0dcec3)

Listens to MIDI input port messages on the local machine.

### RtMIDIOutput

![MIDIMonitor_short](https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/f959be4f-2c3c-45cd-9ad5-f1d8d4f6acd6)

Sends messages to MIDI output port on the local machine. 

### TimelineMIDIInput

Generates MIDI messages from a midi file track using the Unity timeline.

### MergedTimelineMIDIInput

Generates MIDI messages from multiple midi file tracks using the Unity timeline.

### PackRawMIDIInput

Connects to a running [sonosthesia-daw-connector](https://github.com/jbat100/sonosthesia-live-connect/tree/main/sonosthesia-daw-connector) to receive MIDI messages from a remote machine. 

### PackRawMIDIOutput

Connects to a running [sonosthesia-daw-connector](https://github.com/jbat100/sonosthesia-live-connect/tree/main/sonosthesia-daw-connector) to send MIDI messages to a remote machine.

## Channels

### MIDI Note Channels

MIDI note on, note off and polyphonic aftertouch messages can be combined to create a sonosthesia channel with each note represented as a separate stream of [MIDINote](https://github.com/jbat100/sonosthesia-unity-packages/blob/main/packages/com.sonosthesia.adaptivemidi/Runtime/Messages/MIDINote.cs) with a variable `Pressure` field. 

![MIDI1_short](https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/149eff13-db93-4d81-bd79-42ccb74c4289)

### MPE Note Channels

MIDI note on, note off, control change (74), channel aftertouch and pitch bend messages can be combined to create a sonosthesia channel with each MPE note represented as a stream of [MPENote](https://github.com/jbat100/sonosthesia-unity-packages/blob/main/packages/com.sonosthesia.adaptivemidi/Runtime/MPE/MPENote.cs) with variable `Slide`, `Pressure` and `Bend` fields. 

![MPE1_short](https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/4d5e91b8-e695-48f9-8b14-a359827139df)

## Sync and Transport

MIDI clock, start, stop and continue messages can be used to synchronize Unity with a DAW. Bar, Beat and Sixteenth info is infered based on provided time signature. 

![MIDITransport_short](https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/402aa1b2-4264-4a5a-9749-879fe2389ed7)

### RtTransport

### PackTransport

### Known limitations

- [Song Position Pointer](http://midi.teragonaudio.com/tech/midispec/ssp.htm) messages are not sent by Ableton Live when looping for some reason.
- [com.sonosthesia.rtmidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.rtmidi) version misses a [Song Position Pointer](http://midi.teragonaudio.com/tech/midispec/ssp.htm) message when clicking on the scrub area in Ableton Live. This problem does not occur with [com.sonosthesia.pack](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.pack).
- Time signature must be provided as it is not provided by MIDI sync messages. 
- Time signature changes are not supported.










