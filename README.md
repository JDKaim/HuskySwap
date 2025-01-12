# HuskySwap

[Live site taken down at request of UW >> ~~husky-swap.azurewebsites.net~~ <<]

[**HuskySwap Living Document**](https://docs.google.com/document/d/1b-036ETsawjpZUHPB2AdQ41adTJuzHHXcQ2e1PDJsd4/edit?usp=sharing)

[Demo Presentation Slides](https://docs.google.com/presentation/d/117dGuEK98-TwAPGUBijfTXKM1hCZNRdOGHY__NDKyi0/edit?usp=sharing)

[Developer Documentation](https://github.com/JDKaim/HuskySwap/blob/main/DEVELOPER_DOCUMENTATION.md)

[User Documentation](https://github.com/JDKaim/HuskySwap/blob/main/USER_DOCUMENTATION.md)

## **Project Description**

HuskySwap is a public website where people can list and trade classes before and at the beginning of each quarter.
The platform simplifies course management by allowing students to communicate and swap seats in a class, eliminating the uncertainty and inefficiency of the current course adjustment process by providing a platform for students to find and trade seats with others easily.

## **Running the project on Docker**

Build the Docker container:

    docker build -t huskyswap .

Run the Docker Container and bind on port 8080:

    docker run -d -p 8080:80 huskyswap

Access the HuskySwap interface using http://localhost:8080

## **Setting Up and Running Source Code**

The source code can be obtained by cloning the repository hosted at https://github.com/jdkaim/HuskySwap.

To set up the **BACKEND**, you must:

Download .NET if you have not already: https://dotnet.microsoft.com/en-us/download

Navigate to the src directory:

    cd src

Restore .NET Core dependencies

    dotnet restore

Run the application

    dotnet run --project LightNap.WebApi

To set up the **FRONTEND**, you must:

Navigate to the lightnap-ng directory in a different terminal

    cd src/lightnap-ng

Install Angular dependencies

    npm install

Run the Angular app

    npm run start

The website will be hosted locally on http://localhost:4200

* You must run **both** Frontend and Backend based on the instruction above to host a local server!

----------------------------------------------------------------------------------

## **Testing**

To test the **BACKEND**, you must:

Navigate to the src directory:

    cd src

Restore .NET Core dependencies

    dotnet restore

Run testing

    dotnet test

Tests can be added in LightNap.Core.Tests. Tests should be put in a new folder under LightNap.Core.Tests if they don't fall under a preexisting folder's category.

There are no naming conventions for new tests, if they are under the folder please name them with sanity. Standard .NET testing structures should be used, and one can model existing tests to do so.

To test the **FRONTEND**, you must:

Navigate to the lightnap-ng directory in a different terminal

    cd src/lightnap-ng

Install Angular dependencies

    npm install

Run tests

    npm test

Tests can be added in lightnap-ng in the same folders as the code that they intend to test. The files should be given the same name as the file they desire to test, however the suffix must be changed from .ts to .spec.ts. Tests use Jasmine and Karma for Angular, and one can model existing tests to do so.

----------------------------------------------------------------------------------

## **Building**

When a commit is merged to the main branch, it will automatically kick off a completely automated CI/CD pipeline that will build the project.

-----------------------------------------------------------------------------------

### **Addressed Peer Review**:

1. Awkward Setting Button and Sidebar Toggle Button location
   - Did not get around to fixing as was focusing on other issues
2. Having trouble logging in as Admin
   - Wrong Admin Account Email and Password was sent; Fixed by sending the correct issues.
3. Need to download a specific version of dotnet in order to run everything
   - Updated in developer documentation
4. Unable to create an account
   - Fixed, noted in developer docs, build the backend twice and the issue should clear up. 

