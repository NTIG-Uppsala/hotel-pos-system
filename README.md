# Hotel point of sale system

## Releases

Releases can be found [here](https://github.com/NTIG-Uppsala/hotel-pos-system/releases).

### Version numbers

Releases are given a version number based on [Semantic Versioning 2.0.0](https://semver.org/spec/v2.0.0.html).

### Changelog

There is a changelog for this project [here](./CHANGELOG.md). Its format is based on [keep a changelog 1.1.0](https://keepachangelog.com/en/1.1.0/).

## Development setup

### Clone

Clone the repository:

```shell
git clone https://github.com/NTIG-Uppsala/hotel-pos-system
```

### Install .NET SDK

Make sure you have the .NET 8 SDK installed. You can get it from [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

### Entity Framework Core

#### Installation

```shell
dotnet tool install --global dotnet-ef
```

#### Database update

Run this once when setting up the project and after each [migration](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) from the Git repository:

```shell
dotnet ef database update --project ./src/HotelPosSystem/HotelPosSystem.csproj
```

## Run

Run the following command from the Git repository:

```shell
dotnet run --project ./src/HotelPosSystem/HotelPosSystem.csproj
```

## Publish

To build the project for distribution, run the following command from the Git repository:

```shell
dotnet publish ./src/HotelPosSystem/HotelPosSystem.csproj
```

## Run tests

Navigate to the `src` folder and run:

```shell
dotnet build -c Release ; dotnet test
```

**Do not interact with your computer while the tests are running. Doing so might cause the tests to fail unexpectedly.**

## Format code

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

**The tests might fail unexpectedly if you interact with your computer while the pre-commit hook is running.**

The `.git` folder may be hidden by default.

