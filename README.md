## Embed PowerShell commands into Lnk files. Arbitrary file extension.

With this tool we can generate a Lnk file containing PowerShell (Pretty standard). However, we can also force the Lnk file to think it is a different file type. e.g. pdf, xls, etc. 

The Lnk will maintain this illusion when the user hovers over the lnk and it will persist until the user actually executes the lnk. At which point Windows will repair the lnk file data. 

## Usage
<img src="https://github.com/user-attachments/assets/ad431f7f-24e0-4daf-8c68-e2c9c51d6875" width=500>


## Workflow
#### Create the Lnk file
<img src="https://github.com/user-attachments/assets/066c81f5-adf3-4782-9b8a-02cb43c4fd8e" width=500>

### Observe that the target of the Lnk appears as whichever filetype we choose.
Note: We can still see that the arguments for our powerShell are visible. Keep that in mind.

<img src="https://github.com/user-attachments/assets/373b3500-e3da-45d4-97b7-d406709add4a" width=500>
<img src="https://github.com/user-attachments/assets/37f4ad76-0782-4101-94b6-fd69b800af66" width=300>

### Finally if the link is clicked the powershell will run.
<img src="https://github.com/user-attachments/assets/42aed745-e9d1-4873-adca-46b76bc06459" width=500>


## Lnk Format
The Lnk file format is like the wild west. Certain sections may be ommitted. The order of list sometimes doesnt matter. It's a wild ride if you ever want to try RE'ing the file format. For this implementation We've elected to only use the basic sections:

- Header
- ShellLinkItemIdList
- ShellLinkInfo

A decent amount of the data has been stripped out as null bytes. Most of everything in these links that is not 0x00 is required for the link to work.

<img src="https://github.com/user-attachments/assets/88e0ca15-47f0-478e-83b7-e5f16f772b0b" width=500>






