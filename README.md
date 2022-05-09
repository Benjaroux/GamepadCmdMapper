# GamepadCmdMapper

GamepadCmdMapper is a tool used for mapping controller buttons with windows commands.

## Usage
```
GamepadCmdMapper.exe "Parameters.txt"
```

## Parameters file
1 parameter / line, a parameter is represented as follows :
```
[Pattern 1];[Duration 1];[Command 1]
[Pattern 2];[Duration 2];[Command 2]
...
```

With :
* ***Pattern*** : button or combination of buttons pressed at the same time required for matching the pattern. Each buttons must be separated by comma
* ***Duration*** : Pressed buttons duration (in milliseconds) required for matching the pattern
* ***Command*** : Command to execute for this pattern
 
## Examples
Press "A" for 1000 ms will execute the command "taskkill /F /IM notepad.exe"
```
A;1000;taskkill /F /IM notepad.exe
```
Press "Start" + "Back" for 3000 ms will execute the command "taskkill /F /IM win32calc.exe"
```
Start,Back;3000;taskkill /F /IM win32calc.exe
```

## Install the service
Run the following command as admin
```
sc.exe create "GamepadCmdMapper" binpath="[Full path to GamepadCmdMapper.exe] [Full path to Parameters.txt]"
```

This will create the service "GamepadCmdMapper" with the defined parameters

Notes :
* The errors will be logged in the Applcation Eventlog 
* If you edit the parameters file, don't forget to restart the service for applying the new settings

## Uninstall the service
Dtop the service and run the following command as admin
```
sc.exe delete "GamepadCmdMapper"
```

## Supported buttons
Only tested with xbox360 controller
```
Up
Down
Left
Right
Start
Back
LS
RS
LB
RB
A
B
X
Y
```
