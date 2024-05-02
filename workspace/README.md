# Provider Identity Portal (PIDP) / Rebranded as OneHealthID Service

## Table of Contents

[TOC]

## Installation and Configuration

The installation and configuration of the PIdP development environment is sequentially ordered to ensure software dependencies are available when needed during setup.

### Installation

The following list includes the required software needed to run the application, as well as, the suggested IDE with extensions for web client development, and software for source control management and API development/testing.

#### Git and GitKraken

[Download](https://git-scm.com/downloads) and install the Git version control system, and optionally [download](https://www.gitkraken.com) and install the free GitKraken Git GUI client.

Clone the PIdP repository into a project directory GitKraken or the terminal by typing:

```bash
git clone https://github.com/bcgov/moh-pidp
```

#### Node

[Download](https://nodejs.org/en/) and install **Node v16.10.x** or greater

#### VS Code

[Download](https://code.visualstudio.com/) and install VSCode and accept the prompt to install the recommended extensions when the PIdP repository is initially opened in VSCode.

#### PostMan

[Download](https://www.getpostman.com/apps) and install the Postman HTTP client.

#### Nx Workspace

This project was generated using [Nx](https://nx.dev) and uses an abstraction of the Angular CLI to execute schematics for creating Angular artifacts which requires the global installation of the Nx CLI.

```bash
npm install -g nx
```

It's suggested to install the [Nx Console](https://marketplace.visualstudio.com/items?itemName=nrwl.angular-console) VSCode extension, which has been included in the projects suggested extensions.

#### Yarn

[Yarn](https://yarnpkg.com/) is the default package manager for the project. Version 1.x of the package manager needs to be [installed](https://classic.yarnpkg.com/lang/en/). After installation in the workspace folder type:

```bash
yarn install
```

## Building and Running

### Development Server

To build, run, and open a local in memory development server which will run at `http://localhost:4200/` and automatically reload when you change any of the source files for development go to the applciation folder and type:

```bash
nx serve APP_NAME
```

other way of running using yarn is 
```bash
yarn nx serve pidp
```

### Build Application

To build the project and have the build artifacts stored in the `dist/` directory type:

```bash
nx build APP_NAME
```

other way of building using yarn is 
```bash
yarn nx build pidp
```
### Running Unit Tests

Execute the unit tests via [Jest](https://jestjs.io) by typing:

```bash
nx test APP_NAME
```

Execute the unit tests for affected by a change by typing:

```bash
nx affected:test
```

Other way of running unit tests is

```bash
yarn nx test
```

### Running End-to-End Tests

Execute the end-to-end tests via [Cypress](https://www.cypress.io) by typing:

```bash
nx e2e APP_NAME
```

Execute the end-to-end tests for affected by a change by typing:

```bash
nx affected:e2e
```

### Generators

#### Generate an application

When using Nx, you can create multiple applications and libraries in the same workspace. To generate an application run:

```bash
nx g @nx/angular:app APP_NAME
```

#### Generate a library

Libraries are shareable across libraries and applications. They can be imported from `@workspace/LIB_NAME`. To generate a library run:

```bash
nx g @nx/angular:lib LIB_NAME
```

### Components Structure

In the workspace folder the app folder contains the applications. Every application has it's own folder.
pidp is the folder for the pidp application. The folder for components is pidp->src->app->features. 
features folder has nested folders like access, admin, auth, faq ... which determine the side menu options in the home page of the application. 
main.ts is the application entry file. in this file the keycloak configuration is fetched.
app.routing.ts has top level routing. which internally uses the shell routing.
shell-routing.ts has routing for different sections like profile, training, faq and history...
Taking profile section as an example:
It has profile.route.ts which determined the routing for the profile routing. In the similar way faq, history... has route.ts files.
Inside the profile folder there are subfolders for different sections like college license, personal information...
Inside these folders the components(pages for some folders) folder contains ts, html, css, spec(unit test) files.
taking personal-information as example:
personal-information.state.ts has getter methods for the form elements.
personal-information.resource.service.ts makes api calls to backend through http methods.
personal-information-routing.route.ts has the routing
personal-information.page.html has the template content
personal-information.page.scss has the styling part
personal-information.page.spec.ts has the unit test cases for the component
personal-information.page.ts has the event handlers for the form elements.

#### Code Scaffolding

Generate code scaffolding for the application like modules, components, services, etc;

Run `nx g @nx/angular:component my-component --project=my-app` to generate a new component.

```bash
nx g @nx/angular:component my-component --project=my-app
```

### Understand Your Workspace

To see a diagram of the dependencies of your projects type:

```bash
nx dep-graph
```

## Coding Styles

Coding styles should adhere to the [Angular Style Guide](https://angular.io/docs/ts/latest/guide/style-guide.html) with assistance from EditorConfig, Prettier, and ESLint setups for the project that automatically apply code formatting.

## Getting Help

Visit the [Nx Documentation](https://nx.dev) to learn more.
