# WebApplication

The ASP.NET Core WebAPI application and the model for the library. Layered architecture to separate different parts of the app. Keep it simple but cover all requirements. Here is a list of steps you need to go through:
Connecting Entity Framework Core and using database. Creating classes and relationships for all tables.Registering them in EntityFramework.

Implemention the following API:
@baseUrl = localhost:5000


### 1. Get all books. Order by provided value (title or author)
GET https://{{baseUrl}}/api/books?order=author

# Response
# [{
# 	"id": "number",    
# 	"title": "string",
# 	"author": "string",
# 	"rating": "decimal",          	average rating
# 	"reviewsNumber": "decimal"    	count of reviews
# }]

### 2. Get top 10 books with high rating and number of reviews greater than 10. You can filter books by specifying genre. Order by rating
GET https://{{baseUrl}}/api/recommended?genre=horror

# Response
# [{
# 	"id": "number",
# 	"title": "string",
# 	"author": "string",
# 	"rating": "decimal",          	average rating
# 	"reviewsNumber": "decimal"    	count of reviews
# }]

### 3. Get book details with the list of reviews
GET https://{{baseUrl}}/api/books/{id}

# Response
# {
# 	"id": "number",
# 	"title": "string",
# 	"author": "string",
# 	"cover": "string",
# 	"content": "string",
# 	"rating": "decimal",          	average rating
# 	"reviews": [{
#     	    "id": "number",
#     	    "message": "string",
#     	    "reviewer": "string",
# 	}]
# }}

### 4. Delete a book using a secret key. Save the secret key in the config of your application. Compare this key with query param
DELETE https://{{baseUrl}}/api/books/{id}?secret=qwerty

### 5. Save a new book.
POST https://{{baseUrl}}/api/books/save

{
	"id": "number",             	// if id is not provided create a new book, otherwise - update an existing one
	"title": "string",
	"cover": "string",          	// save image as base64
	"content": "string",
	"genre": "string",
	"author": "string"
}

# Response
# {
# 	"id": "number"
# }

### 6. Save a review for the book.
PUT https://{{baseUrl}}/api/books/{id}/review

{
	"message": "string",
	"reviewer": "string",
}

# Response
# {
# 	"id": "number"
# }

### 7. Rate a book
PUT https://{{baseUrl}}/api/books/{id}/rate

{
	"score": "number",    	// score can be from 1 to 5
}


Validating all values that come into your app (query params, body, etc)

Returning an appropriate status code for every response.

Connecting middlewares for error handling and logging. Logging every request in a simple format (make sure to log HTTP method,  headers, query params and body) in the console.
Notes:
Not creating instances (DbContext, services, mappers) manually via “new()”. Using Dependency Injection instead.  
Instead added a data seeding. For better testing seeded data for 10 books.
Using DTO for all requests and responses (can to use Automapper). Not put entities in the response.
In order to implement validation using the Fluent Validation package.
Using async/await wherever possible (Controllers returns Task<TResult>)
To test the endpoints used either Postman or this extension for Visual Studio Code. The format of the requests above is supported by this extension.
