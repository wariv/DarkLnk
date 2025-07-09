# Overview
The purpose of DarkLnk is to exploit the .lnk format and achieve 2 goals:

1. Allow the attacker to emulate any file extension. docx, mov, pdf, mp3, etc
2. Retain the the .lnk functionality to call PowerShell

&nbsp;

The general idea is that a malicious DarkLnk .lnk file will be delivered to a target and the file icon, properties, and context will all appear to be a valid .lnk to a choosen filetype. However, the .lnk file will still point towards PowerShell and execute PowerShell commands.

**The Good**
- The icon reflects the extension choosen by the attacker.
- Hovering over the .lnk also reflects the extension choosen by the attacker.
- The .lnk properties **mostly** show the file type properties choosen by the attacker.
- To an average or even above average user this appears to be a valid .lnk shortcut.


**The Bad**

- The attackers PowerShell payload is still in plain text.
- The PowerShell arguments are visible in the Properties -> Target
- After running the .lnk Windows Link Healing will "fix" the link and will appear like a normal PowerShell .lnk file.

**The Future**
- ~~Add more attacker flexibility~~ (Completed)
- ~~Add more obfuscation capabilities~~ (Completed)
- Find out why only PowerShell works and not other binaries like cmd.exe

&nbsp;

# Usage
<img src="https://github.com/user-attachments/assets/0f37b03c-4328-48b7-92b4-a488ce1cd59a" width=800>

&nbsp;

# Example Workflow

With this tool we can generate a .lnk file containing PowerShell (Pretty standard). However, we can also force the .lnk file to think it is a different file type. e.g. pdf, xls, etc. The .lnk will maintain this illusion when the user hovers over the lnk and it will persist until the user actually executes the .lnk. At which point Windows will repair the .lnk file data. 


### Step 1: Create the .lnk file
<img src="https://github.com/user-attachments/assets/42a7dcc8-4b63-4668-8d7f-0fb47d5631d6" width=500>


&nbsp;

**Observations** 
- The target of the .lnk appears as whichever filetype we chose.
<img src="https://github.com/user-attachments/assets/373b3500-e3da-45d4-97b7-d406709add4a" width=500>

&nbsp;

- We can still see that the arguments for our powerShell are visible...
<img src="https://github.com/user-attachments/assets/37f4ad76-0782-4101-94b6-fd69b800af66" width=300>

&nbsp;

### Step 2: Execute the .lnk file

&nbsp;

**Observations** 
- The PowerShell executed and we can see that it created a test directory.
- Windows link healing repaired the .lnk and it now resembles a PowerShell  .lnk
<img src="https://github.com/user-attachments/assets/42aed745-e9d1-4873-adca-46b76bc06459" width=500>

&nbsp;


## Lnk Format
The Lnk file format is like the wild west. Certain sections may be ommitted. The order of list sometimes doesnt matter. It's a wild ride if you ever want to try RE'ing the file format. For this implementation We've elected to only use the basic sections:

- Header
- ShellLinkItemIdList
- ShellLinkInfo

A decent amount of the data has been stripped out as null bytes. Most of everything in these links that is not 0x00 is required for the link to work. Many of these void areas can be modified with the tools obfuscation techniques.

<img src="https://github.com/user-attachments/assets/88e0ca15-47f0-478e-83b7-e5f16f772b0b" width=500>






