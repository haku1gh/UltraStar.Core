# UltraStar Core

## What is part of the library

### Not Included

- Views (e.g. in Unity, OpenGL, Vulkan, ...)

### Included

roughly everything else of the Game?

definitely:
- Decoding of video and buffering of video frames **(already tested)**
- Decoding of audio and buffering of audio frames **(already tested)**
- Decoding of images **(already tested)**
- Resizing of images **(already tested)**
- Recording of audio **(already tested)**
- Audio playback **(already tested)**
- Applying effects to audio streams
  - Karaoke effect **(already tested)**
  - Echo effect **(already tested)**
- Timer / Synchronization
  - Synchronize everything to audio file
  - Get audio timestamps **(already tested)**
  - Get video timestamps **(already tested)**
  - Get lyrics timestamps
  - Use high quality timer **(already tested)**
- Reading song files (TXT)
  - Find algorythm to use correct text encoding **(already tested)**
  - Read all tags and do basic correctness checks **(already tested)**
  - Read all lyrics
- Handle main game states
- Handle settings
  - Read settings **(already tested)**
  - Write settings
- Use of a database
  - Demo access **(already tested)**
  - Cache
  - Highscores
- Logging **(already tested)**
- Multiple Remote Display with Synchronization
- Connect to APP via REST
- Provide real karaoke queueing
- A good randomizer

unclear:
- Localization (log files shall not contain localized information)

## Intended use of ThirdParty Libraries

- FFmpeg 4.2.2
  - License: LGPL 3.0
  - Use for: decode video; decode audio, decode images, resize images
- BASS 2.4.15
  - License: custom, free for non commercial use
  - Use for: record audio; play audio; use DSP framework for audio streams
- GStreamer AudioFx (not directly used, but some formulas will be reused)
  - License: LGPL 2.0
  - Use for: apply audio effects
- NGettext 0.6.5
  - License: MIT
  - Use for: localization
- Serilog 2.9.0
  - License: Apache License 2.0
  - Use for: logging
- SQLite-net 1.6.292 (not clear if this SQLite wrapper implementation will be used)
  - License: MIT
  - Use for: database
  - There are no real libraries (.so files) for Linux available
    - Found a possibility with interop files (this should work for Win32, Win64 and Linux)
- DBreeze 1.95
  - License: BSD 2-clause
  - Use for: database
  - PRO: Easily portable as completely written in C#
  - CON: Its a NoSQL database. Search might be a bit more difficult. Multiple files in a folder for the database.
