# KTool

KTool is a tool library that supports Unity developers in simplifying their work.

## How do I get set up?

 1. Open PacakageManager window
 2. Click button [+]
 3. Select [Add package form git URL...]
 4. paste url: https://github.com/KemLib/KTool.git?path=/Packages/KTool
 5. Click button [Add]

If you want to use the latest version, replace the URL.
	https://github.com/KemLib/KTool.git?path=/Packages/KTool#develop

## Features

 - **Advertisement**: easily implement advertising display features in the application, see more [KTool.Admob].
 - **Attribute**: the attribute allow you to easily select the value of the variable in the Inspector.
 - **Script Creater**: editor to create scripts from available templates, automatically adding Namespaces by folder.
 - **FileIo**: file read/write library in asset or data.
 - **Init**: object initialization system.

### Attribute

|Attribute|Variable Type|Description|
|--|--|--|
|GetAsset|string[], Object[]|get all assets in the folder|
|GetComponent|Component, Component[]|get all component in the curent GameObject or in children of curent GameObject|
|SelectAsset|string, string[], Object, Object[]|dropdown list to select assets in the folder|
|GetAssetAttribute|Component, Component[]|dropdown list to select component in the curent GameObject or in children of curent GameObject|
|SelectAgentId|int, int[]|dropdown list to select AgentId|
|SelectLayer|int, int[], string, string[]|dropdown list to select Layer|
|SelectSortingLayer|int, int[], string, string[]|dropdown list to select SortingLayer|
|SelectScene|int, int[], string, string[]|dropdown list to select Scene|
|SelectTag|int, int[], string, string[]|dropdown list to select Tag|

### Script Creater

 1. Right-click on the Project window.
 2. Select KTool -> Create Script
 3. Enter class name
 4. Click [Save]

You can create a new template script, see the SettingCreateScript file in the "Assets/KTool/AssetCreater/Editor/Script" folder.

### Init

Suppose you need to create an application that displays ads, first you need to initialize the ads, then download the ads, and wait for the ads to be ready to display.

 1. Create InitContainer
 2. In the InitContainer, pull the ad initialization object into step 1, pull the ad loading object into step 2.
 3. If you want to load the scene after completing the work, check AfterInit.
 4. Create InitManager
 5. Play
