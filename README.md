# sonosthesia-unity-demo-midi

This Unity application demonstrates the sonosthesia MIDI packages, which are part of a wider set of [packages](https://github.com/jbat100/sonosthesia-unity-packages) aimed at facilitating the creation of immersive, interactive audio visual art. These modules provide an abstraction layer for MIDI messages, with a human readable API for MIDI input and output. The core message types and abstract input and output APIs are provided by

- [com.sonosthesia.adaptivemidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.adaptivemidi)

While concrete implementations are provided by 

- [com.sonosthesia.rtmidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.rtmidi)
- [com.sonosthesia.timelinemidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.timelinemidi)
- [com.sonosthesia.pack](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.pack)

# Installation

Note that to add those packages to your Unity project you will need to add the following [scoped registeries](https://docs.unity3d.com/Manual/upm-scoped.html) to your `Packages/package.json` file (`Keijiro` only necessary for [com.sonosthesia.rtmidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.rtmidi)). 


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

UI test and monitoring tools which plug into the abstraction layer API are contained in the [Assets folder](https://github.com/jbat100/sonosthesia-unity-demo-midi/tree/main/MIDIDemo/Assets/UI) of the application itself.

## MIDI I/O Scenes

### RtMIDIInput and RtMIDIOutput

Platform support is limited to Windows, macOS and Linux due to the dependency on [jp.keijiro.rtmidi](https://github.com/keijiro/jp.keijiro.rtmidi)

<p align="center">
    <img width="445" alt="RtMIDIInput" src="https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/fe3b33a8-b1a4-46bd-9506-e459ab1ea969" width="50%">
</p>

<p align="center">
  <img src="https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/0ba0138f-77af-492f-9f02-ee240a0dcec3" width="75%"/>
</p>

Listen to MIDI input port messages on the local machine using the `RtMIDIInputStream` component from [com.sonosthesia.rtmidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.rtmidi).

<p align="center">
    <img width="445" alt="RTMIDIOutput" src="https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/a9289488-6ad0-4d91-bb33-d8e8ca17ce2e" width="50%">
</p>

<p align="center">
  <img src="https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/f959be4f-2c3c-45cd-9ad5-f1d8d4f6acd6" width="75%"/>
</p>

Send messages to MIDI output port on the local machine using the `RtMIDIOutputStream` from [com.sonosthesia.rtmidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.rtmidi). 

### TimelineMIDIInput and MergedTimelineMIDIInput

Supports all platforms. Generate MIDI messages from midi file tracks using the Unity timeline. Uses the `TimelineMIDIOutput` Implemented in [com.sonosthesia.timelinemidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.timelinemidi).

### PackRawMIDIInput and PackRawMIDIOutput

Supports all platforms. Connect to a running [sonosthesia-daw-connector](https://github.com/jbat100/sonosthesia-daw-connector) to send and receive MIDI messages to and from a remote machine (including mobile and VR platforms). Follow setup and config instructions and ensure that the installed version matches the [com.sonosthesia.pack](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.pack) package version in your unity project. You can ensure a specific version of the sonosthesia-daw-connector using `@` 

```
npm install -g sonosthesia-daw-connector@1.3.0
```

MIDI input uses the `PackRawMIDIInputStream`

<p align="center">
    <img width="445" alt="PackMIDIInput" src="https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/6e4d35a2-bc82-40c0-9538-2fcb5a027ac0" width="50%">
</p>

MIDI output uses the `PackRawMIDIOutputStream`

<p align="center">
    <img width="443" alt="PackMIDIOutput" src="https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/011f3a7e-0cdb-428e-9b5e-42c755582150" width="50%">
</p>

## Channels

Channels provide a higher level of abstraction to MDI notes and MPE notes by combining different types of messages to create UniRx data streams for each note. They are implemented in the [com.sonosthesia.midi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.midi) package.

### MIDI Note Channels

MIDI note on, note off and polyphonic aftertouch messages can be combined to create a sonosthesia channel with each note represented as a separate stream of [MIDINote](https://github.com/jbat100/sonosthesia-unity-packages/blob/main/packages/com.sonosthesia.adaptivemidi/Runtime/Messages/MIDINote.cs) with a variable `Pressure` field. 

<p align="center">
  <img src="https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/149eff13-db93-4d81-bd79-42ccb74c4289" width="75%"/>
</p>

### MPE Note Channels

MIDI note on, note off, control change (74), channel aftertouch and pitch bend messages can be combined to create a sonosthesia channel with each MPE note represented as a stream of [MPENote](https://github.com/jbat100/sonosthesia-unity-packages/blob/main/packages/com.sonosthesia.adaptivemidi/Runtime/MPE/MPENote.cs) with variable `Slide`, `Pressure` and `Bend` fields. 

<p align="center">
  <img src="https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/4d5e91b8-e695-48f9-8b14-a359827139df" width="75%"/>
</p>


## Sync and Transport

MIDI [Song Position Pointer](http://midi.teragonaudio.com/tech/midispec/ssp.htm), [Clock](http://midi.teragonaudio.com/tech/midispec/clock.htm), [Start](http://midi.teragonaudio.com/tech/midispec/start.htm), [Stop](http://midi.teragonaudio.com/tech/midispec/stop.htm) and [Continue](http://midi.teragonaudio.com/tech/midispec/continue.htm) messages can be used to synchronize Unity with a DAW. Bar, Beat and Sixteenth info is infered based on provided time signature. 

<p align="center">
  <img src="https://github.com/jbat100/sonosthesia-unity-demo-midi/assets/1318918/402aa1b2-4264-4a5a-9749-879fe2389ed7" width="75%"/>
</p>

Note the MIDI Sync functionality must be enabled in your DAW for the relevant MIDI output. In Ableton Live the [song](https://help.ableton.com/hc/en-us/articles/209071149-Synchronizing-Live-via-MIDI) MIDI clock type must be used. If unsure of the required settings of your particular DAW you can use applications like [MIDIMonitor](https://www.snoize.com/midimonitor/) to check that it is generating the required sync messages.

### RtTransport and PackTransport

Provide example transport tracking with two different MIDI backends.

### Known limitations

- [Song Position Pointer](http://midi.teragonaudio.com/tech/midispec/ssp.htm) messages are not sent by Ableton Live when looping for some reason.
- [com.sonosthesia.rtmidi](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.rtmidi) version misses a [Song Position Pointer](http://midi.teragonaudio.com/tech/midispec/ssp.htm) message when clicking on the scrub area in Ableton Live. This problem does not occur with [com.sonosthesia.pack](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.pack).
- Time signature must be provided as it is not provided by MIDI sync messages. 
- Time signature changes are not supported.










