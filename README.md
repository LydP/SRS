# SRS (WIP)

## Introduction

This is a simple implementation of a spaced repetition system. For more information on spaced repetition
and the spacing effect, see:

- [Spacing effect](https://en.wikipedia.org/wiki/Spacing_effect) _Wikipedia_
- [Spaced repetition](https://en.wikipedia.org/wiki/Spaced_repetition) _Wikipedia_

This project is currently incomplete. Specifically, there are a few bugs (explained below), the code 
needs refactoring to be more object oriented, and the spacing algorithm is not fully implemented. 

## Usage

When running in Visual Studio, copy the db and card_fronts folders to the output directory. 


When started, the program displays the following screen with a start button. 

![Alt text](./readme_images/start_screen.jpg?raw=true "Title")

Click the start button. After clicking the start button, the program will display a screen showing
the card's front and a Show Answer button (the card front may be different than that shown below).

![Alt text](./readme_images/card_front.jpg?raw=true "Title")

When ready, click the Show Answer button. Doing so will display the back of the card with a red button
and a green button.

![Alt text](./readme_images/card_back.jpg?raw=true "Title")

The red button is clicked when the user doesn't know/remember the word's meaning and pronunciation,
and the green button is clicked otherwise. Upon clicking one of these buttons, the program displays
the front of the next card.

When all sessions are complete, the program displays the following screen.

![Alt text](./readme_images/end_screen.jpg?raw=true "Title")

At this point, simply exit the program.

## DB Files

### program.db
The program.db file keeps a record of sessions. It contains the ```run_info``` table, which contains
two columns: ```sess_begin``` and ```sess_count```. ```sess_begin``` stores the time at which the
start button is clicked for each new session in the format MM:dd:yyyy:HH:mm:ss. This
file is used to determine when to resume a previous session, start a new session, or both.

### Japanese.db
The Japanese.db file contains and keeps records for each card and each review. It contains the tables
```statistics``` and ```cards```. The ```statistics``` table keeps track of each review and the 
```cards``` table keeps track of card information, including the most recent values from
```statistics```. This table is initialized with code in the
[database](https://github.com/LydP/database) repository. For more information,
[see the readme](https://github.com/LydP/database/blob/main/README.md) for that repository.

The information in the ```popularity```, ```front```, and ```back``` columns of the ```cards```
table comes from [Optimized Kore](https://docs.google.com/spreadsheets/d/1uaUcQNyADAwP4k5rb0UNiQ1c8wPtWl1plqDHQryr75E/edit#gid=0).

### A Note About Sessions

A session is the reviewing/learning of a deck of cards. If the user has not completed a session when the
program is closed, that session is resumed when the user reruns the program. Multiple sessions can also
occur in a single run if a new session is scheduled at the same time the user is completing a prior
session, such as when rerunning the program the day after closing it mid-session.

## Spacing Algorithm and ```runPython()```

I(n) = n - 1 for n <= 4 where n is the session and I(n) is the interval. For n > 4, the program runs
a more complex spacing algorithm. I'm implementing this algorithm in Python, hence the
```runPython()``` method. When this algorithm is complete, I may implement it in C#, if it's
feasible to do so. I have not included the Python file in this repository, so the program will not
work properly after a certain number of runs. 

These lines will result in erros if the ```runPython()``` method is accessed.
```C#
start.FileName = @"...python.exe"; // complete path to python

start.Arguments = @"...main.py"; // complete path to python file
```

## Known Bugs

### NullReferenceException When Exiting the Program
Exiting the program before clicking Start results in a NullReferenceException. This happens because
variables used in the ```Form1_FormClosing()``` method remain uninitialized unless Start is
clicked. However, changing the code to initialize these variables upon program start instead of in
the ```startButton_Click()``` method results in other problems. I expect this to be easily fixed
with some exception handling.

### Incorrectly Managing Decks
The program has two ```List<int>()``` variables, acting as card decks. ```learningDeck```
contains cards that the user has not seen before, and ```newDeck``` contains cards the user is 
reviewing. When the user sees a card in ```learningDeck```, it is moved to ```newDeck``` since
every viewing of the card from then on will be a review. When the user correclty recalls the back of
a card in ```newDeck``` (i.e. clicks the green button), that card is removed from ```newDeck```,
the information in Japanese.db is appropriately updated, and the user won't see that card until it 
appears in a future session. 

Currently, under certain conditions I haven't yet identified, this process fails, and cards aren't
removed from ```newDeck```. The result is that the program runs indefinitely, even when clicking
the green button for each card.

## Refactoring

I worked mainly with C before undertaking this project, so I started it by following the procedural
paradigm. This was mostly because I wanted to get something working as quickly as possible and I
already had my hands full learning C# and SQLite, so I fell back on the paradigm I knew. Doing this
has resulted in very spaghetti-fied code. I've already started remedying this by creating the Deck
and Database classes. 
