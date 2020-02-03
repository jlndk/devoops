# MiniTwit DevOps project
A Twitter clone made a long time ago, in a galaxy far, far away. Over the course of the semester, we will bring this application up to date and deploy it using all the DevOps skills we learn.

## Flag Tool

This tool is used for flagging twits, which hides them in the web app.

| Command                | Meaning                                          |
|------------------------|--------------------------------------------------|
| `flag_tool <tweet_id>` | Flags a twit.                                    |
| `flag_tool -i`         | Dumps a list of all twits and authors to STDOUT. |
| `flag_tool -h`         | Shows a help screen.                             |

## Control script

This script allows for some quick management of various aspects the application.

| Command | Meaning                                                                                                                                  |
|---------|------------------------------------------------------------------------------------------------------------------------------------------|
| init    | Initializes a SQLite database file at the default location (`/tmp/minitwit.db)`).                                                        |
| start   | Runs the server, piping the output to a log file (`/tmp/out.log`). Also creates a file `/tmp/minitwit.pid` with the server's process id. |
| stop    | Kills the server.                                                                                                                        |
| inspect | Show a list of all twits and authors in a interactive view (using the `less` command).                                                   |
| flag    | Shorthand for running the Flagging Tool, see the [documentation](#flag-tool) for details.                                                |


## Server configuration

To change the server configuration, edit the variables towards the top of `minitwit.py`. The variables and their meanings can be found in the table below.

| Variable   |                                                          Meaning                                                          |     Default value    |                  Valid values                  |
|------------|:-------------------------------------------------------------------------------------------------------------------------:|:--------------------:|:----------------------------------------------:|
| `DATABASE`   | The path to the SQLite database file. If changed, make sure to move the old DB file or create a new one with `make init`. | `"/tmp/minitwit.db"` | Any valid file path.                           |
| `PER_PAGE`   | How many twits to show per page.                                                                                          | `30`                 | Any integer number above 0.                    |
| `DEBUG`      | Whether to run the Flask app in a debug mode. **Set to False for production.**                                            | `True`               | `True` or `False`.                             |
| `SECRET_KEY` | Key used for security such as signing cookies. **Remember to change this from the default value!**                        | `"development key"`  | Any string. Recommended to be long and random. |

## Setup
To setup the application on your own machine, you'll need to do a few things.

### Run the server
 1. Make sure you have python 3 installed.
 1. (Optional) Edit the config variables in `minitwit.py`. The default config is only for development.
 2. Install the Python depencencies with `pip3 install -r requirements.txt`.
 2. Prepare the database by running `make init`.
 3. You can now run the server with `python3 minitwit.py`

### (Optional) Flag Tool
 1. Install the required depencencies with `apt-get install libsqlite3-dev`.
 2. Build the tool with `make build`.
 3. Can now be run with `./flag_tool`. See the [documentation](#flag-tool) for more info.

### (Optional) Run the tests
 1. Run the command `python3 minitwit_tests.py`.
