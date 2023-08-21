# Overview

Product CRUD application with JWT authentication and custom user login/register. This is a fullstack app with a separate backend and frontend. The app uses JWT (JSON Web Token) for authentication and authorization. The app uses MongoDB as database.

The backend has a REST API with automatically generated interactive OpenAPI 3.0 Swagger documentation. This documentation is generated only in development mode and can be accessed from endpoint `/swagger`. Most API calls require authentication so a JWT access token has to be included in request headers as `Authorization: Bearer <your_jwt_token>`. A JWT token can be obtained on user login.

![API](./docs/images/api.JPG?raw=true)

# Tech stack

- Frontend is built with Svelte and SvelteKit
- Backend is built with .NET and ASP.NET core
- Main languages are C# and TypeScript
- MongoDB as database
- Docker containers

# About JWT

This application uses JSON Web Token for authentication and authorization. Note that the JWT implementation only generates access tokens. More production ready implementation would have refresh tokens and token revoke features. The access tokens are generated on user login and expire in 5 minutes. The frontend application saves the token to browser local storage. Also note that this is not the most secure way to store JWT tokens but for this application's purpose it is enough.

# About users

Users can be logged in and registered via the backend application's REST API. Users have username and password. Passwords are hashed and salted with BCrypt hashing algorithm.

Users have roles which are used in the app's authorization process. Registered users have only role `User` but there is a default user with role `Admin`. The admin role is required to create, edit and delete products via REST API. Other users can only view and fetch products.

Use these default users for testing:

Username | Role | Password
-------- | ---- | --------
TestUser | User | Password1!
AdminUser | Admin | Password2!

# About database

This application uses MongoDB as database. MongoDB is a NoSQL document database where entities (documents) are stored to collections.

This application has 3 collections:

- `users` has all app users
- `roles` has all app roles
- `products` has all products

In the application's schema, user documents have embedded role documents. This way a specific user's roles can be checked easily without additional database queries.

To change your MongoDB connection string, modify backend application's `appsettings.json`. The default database name is `product_db`. Make sure to create the database and collections before running!
