# Overview
The purpose of DarkLnk is to exploit the .lnk format and achieve 2 goals:

1. Allow the attacker to emulate any file extension. docx, mov, pdf, mp3, etc
2. Retain the the .lnk functionality to call PowerShell

&nbsp;

The general idea is that a malicious DarkLnk .lnk file will be delivered to a target and the file icon, properties, and context will all appear to be a valid .lnk to a choosen filetype. However, the .lnk file will still point towards PowerShell and execute PowerShell commands. This all works great.

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
- Add more attacker flexibility. I've designed this tool in a way I see fit. However, that might not meet the needs of other organizations. I'd like to add full control over all the important data sections.
- Add more obfuscation capabilities. There is plenty of "dead space" within the .lnk file format. There are many sections that can be filled with arbitrary data and it will not affect the execution of the .lnk.

&nbsp;

# Usage
<img src="https://github.com/user-attachments/assets/ad431f7f-24e0-4daf-8c68-e2c9c51d6875" width=500>

&nbsp;

# Example Workflow

With this tool we can generate a .lnk file containing PowerShell (Pretty standard). However, we can also force the .lnk file to think it is a different file type. e.g. pdf, xls, etc. The .lnk will maintain this illusion when the user hovers over the lnk and it will persist until the user actually executes the .lnk. At which point Windows will repair the .lnk file data. 


### Step 1: Create the .lnk file
<img src="https://github.com/user-attachments/assets/066c81f5-adf3-4782-9b8a-02cb43c4fd8e" width=500>

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

A decent amount of the data has been stripped out as null bytes. Most of everything in these links that is not 0x00 is required for the link to work.

<img src="https://github.com/user-attachments/assets/88e0ca15-47f0-478e-83b7-e5f16f772b0b" width=500>






