# Baobab Ridge Wildlife Rehabilitation Records System
A C# Windows Forms Application that records animals, calculates their status and housing unit from a "Recovery Score", and then generates a summary report. All Data gets stored in a text file.

## Members
**Name**						**Student Number**
Jason Beckley					602089
Alexander Martheze				601218
Boitemogelo Steenkamp			603659
Ben Philip Van der Westhuizen	604776

## Build and Run
**1.** Close the repository.
**2.** Open the .sln file in VSC2022
**3.** Build and run by pressing "F5".

## Features
- Add an animal (with validation)
- View all animals in a grid
- Update an animal (load by grid click or ID search)
- Delete an animal (with confirmation)
- Generate a summary report (shown on the form and saved to summary.txt)

## File Formats
animals.txt displays one animal per line, with fields seperated by "|":
	*ID|Name|Species|Age|Score|Status|Housing Unit*
	*WR-0002|Naledi|Serval|3|62|Recovering|Outdoor Enclosure*

summary.txt displays a text report taht reflects the most recent summary generation. It contains the totals animals, average age, average recovery, score and the count of each status.

## Known Limitations
- n/a