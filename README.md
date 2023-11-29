# Hotel POS System

## Clone

Clone the repository:

```shell
git clone https://github.com/NTIG-Uppsala/hotel-pos-system
```

## Run

Before running the project, navigate to the `src/HotelPosSystem` directory:

```shell
cd src/HotelPosSystem
```

To run the project:

```shell
dotnet run
```

## Publish

Navigate to the project directory:

```shell
cd src/HotelPosSystem
```

Build the project to distribute to end user:

```shell
dotnet publish
```

## Run tests

Navigate to the `src` folder and run:

```shell
dotnet build -c Release ; dotnet test
```

## Format Code

This project uses an [.editorconfig](./.editorconfig) file that defines the project's code style.

To fix the project's formatting, navigate to the `src` directory and run:

```shell
dotnet format
```

## Pre-commit hook

Commits can be prevented if formatting is incorrect or tests fail. 

To do this, create a file named `pre-commit` in the `.git/hooks` directory with the following content:

```shell
#!/bin/sh

set -e

readonly SOLUTION_PATH="./src/HotelPosSystem.sln"

if ! dotnet build -c Release $SOLUTION_PATH
then
    echo -e "\nCommit aborted because build failed."
    exit 1
fi

if ! dotnet format --verify-no-changes $SOLUTION_PATH
then
    echo -e "\nCommit aborted because code formatting is incorrect. Please run 'dotnet format'."
    exit 1
fi

if ! dotnet test $SOLUTION_PATH
then
    echo -e "\nCommit aborted because tests failed."
    exit 1
fi
```

The `.git` folder may be hidden by default.
