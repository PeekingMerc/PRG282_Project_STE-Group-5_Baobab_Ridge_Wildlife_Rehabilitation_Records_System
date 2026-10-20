# Baobab Ridge Wildlife Rehabilitation Records System
A C# Windows Forms Application that records animals, calculates their status and housing unit from a "Recovery Score", and then generates a summary report. All Data gets stored in text files.


## Members
| **Name** | **Student Number** |
|------|----------------|
| Alexander Martheze | 601218 |
| Boitemogelo Steenkamp | 603659 |
| Ben Philip Van der Westhuizen | 604776 |

*Note: Jason Beckley (602089) was registered in the group but withdrew from the module and did not contribute to the project.*


## Build and Run
**1.** Clone the repository
**2.** Open the .sln file in Visual Studio 2022 (the .NET 8 SDK is required)
**3.** Build and run by pressing "F5"


## Features
The project brief names exactly five features. These are the ones specified and assessed:

| # | Feature | Description | Built by |
|---|---|---|---|
| 1 | **Add animal** | Enter a new animal, validate every field, calculate its status and housing unit from the score, append to `animals.txt` | Ben Philip Van der Westhuizen |
| 2 | **View animals** | Load `animals.txt` on startup and display every valid record in a read-only DataGridView | Boitemogelo Steenkamp |
| 3 | **Update animal** | Load a record by clicking a grid row or searching by ID, edit it, recalculate status and housing unit, rewrite `animals.txt` | Ben Philip Van der Westhuizen |
| 4 | **Delete animal** | Select a row, confirm with a Yes/No dialog, remove it from `animals.txt` and the grid | Boitemogelo Steenkamp |
| 5 | **Summary report** | Calculate totals, averages and per-status counts, display them on the form, and write `summary.txt` | Alexander Martheze |

*The brief names exactly these five features, however, the supporting code that ties them together, like the Animal class, the classification method, FileHandler (loading, saving, validation), the form layout, and the .csproj .NET 8 setup — is separate work not listed as a specific "feature" in the project requirements.*


## File Formats
animals.txt displays one animal per line, with fields separated by "|":
```
ID|Name|Species|Age|Score|Status|Housing Unit
WR-0002|Naledi|Serval|3|62|Recovering|Outdoor Enclosure
```

`summary.txt` displays a text report that reflects the most recent summary generation. It contains the totals animals, average age, average recovery, score and the count of each status. It's overwritten each time with an updated summary report.


## Known Limitations
- Lines in animals.txt that are "malformed" are skipped when loading and are not kept when the file is rewritten
- An animal ID cannot be changed after the animal is saved


### Note
The project requires each student to do their own PR merge with main. Alexander worked on the `feature/summary-report` branch and Boitemogelo worked on the `feature/view-animals` branch. Neither of them did a PR merge with main, so to demonstrate their ability, they created the other two branches, being `demonstration/form-cleanup` & `docs/add-comments` which they successfully merged to main as specified by the project. 
