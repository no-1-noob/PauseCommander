[![Build](https://github.com/no-1-noob/PauseCommander/actions/workflows/createbuild.yaml/badge.svg?branch=master)](https://github.com/no-1-noob/PauseCommander/actions/workflows/createbuild.yaml)

A mod that allows you to activate automated pauses using your voice. So you can activate it in a fast section and it automatically pauses when there is time to pause and get back into the song afterwards.

  ![image](https://github.com/no-1-noob/PauseCommander/assets/91905916/b6c5e931-ee49-4a9b-8f7f-670ee055a544)

<b>This mod is based on https://github.com/no-1-noob/VoiceCommander so get this one too</b>

## Commands
The available commands are as follows. The Keywords and Certainties that are needed to trigger a command can be configured/overwritten in the VoiceCommander settings.

|Command|Certainty|Action|
|---|---|---|
|Activate pause|0.85|Activate the next automatic pause if a suitable pause is found and start a countdown to trigger the pause|
|Disable pause|0.85|Disable the next automatic pause|
|Immediate pause|0.85|Immediately pause the song|

## Settings

|Setting|Description|Default|
|---|---|---|
|Min pause length|How long must the gap between two notes be to be classified as a potential pause?|1.5s|
|Controller disconnect pauses|Automatically pause if controller is disconnected (Only tested on Index)|Disabled|
## Counters+

Pause found:

![20240618130043_1](https://github.com/no-1-noob/PauseCommander/assets/91905916/d3163f2f-8abc-4a69-9174-14a39e691a1b)


No Pause found:

![image](https://github.com/no-1-noob/PauseCommander/assets/91905916/7b132e95-0c0f-4083-b2fe-7a14fddb447f)
