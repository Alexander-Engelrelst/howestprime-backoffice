# Howestprime Backoffice

## Overview
This repository contains the blazor backoffice for the Howestprime project.

* **Application Demo**: [Watch a demo of the full application](https://youtu.be/mCZmW9P8Di4)

> **Note**: The client used to actually "buy" the tickets as shown in the demo was not written by me and was
> provided by Howest teaching staff.


## Ecosystem Repositories
* **Movies Microservice Repository**: [Howestprime Movies Microservice](https://github.com/Alexander-Engelrelst/howestprime-movies)
* **Ticketing Microservice Repository**: [Howestprime Ticketing Microservice](https://github.com/Alexander-Engelrelst/howestprime-ticketing)
* **Mobile Application Repository**: [Howestprime Mobile Application](https://github.com/Alexander-Engelrelst/howestprime-mobile)
* **Test environment Repository**: [Howestprime Test Environment](https://github.com/Alexander-Engelrelst/howestprime-infra-test)
* **Production environment Repository**: [Howestprime Production Environment](https://github.com/Alexander-Engelrelst/howestprime-infra-prod)

## Usage
Since this is a project consisting of multiple repositories, trying to run it is not recommended.

To see the full application in action, please watch the [demo video](https://youtu.be/mCZmW9P8Di4)

If you really wish to run the application yourself, please follow the instructions in the [test environment 
repository](https://github.com/Alexander-Engelrelst/howestprime-infra-test)

## Personal Contribution
I primarily developed and maintained the **Howestprime.Backoffice** project. Aside from custom endpoints and routes 
I implemented within the **ApiClient** project, the remaining boilerplate infrastructure in the solution was provided 
by Howest teaching staff.
## Key Highlights
* **Custom validation attribute**: A custom validation attribute to ensure movies can only be added 2 years before 
  their release date. [See code](./Howestprime.Backoffice/ViewModels/Attributes/MovieYearRangeAttribute.cs)
* **Reusable Blazor form component**: A reusable form component that is written such that it behaves slightly 
  differenlty depending on whether the user tries to register a new movie or modify an existing one. [See code](./Howestprime.Backoffice/Components/MovieEntryForm.razor)