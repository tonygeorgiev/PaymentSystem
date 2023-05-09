# Merchant Controller API

The `MerchantController` API is a RESTful API that exposes endpoints for performing CRUD operations on merchants and processing payments. The API is built using ASP.NET Core and relies on an `IMerchantService` and `ITransactionService` interface to communicate with the underlying business logic.

## Endpoints

### `GET /api/merchant`

Retrieves a list of all merchants.

#### Parameters

None.

#### Returns

A JSON array of `Merchant` objects.

### `GET /api/merchant/{id}`

Retrieves a specific merchant by ID.

#### Parameters

- `id` - the ID of the merchant to retrieve.

#### Returns

A `Merchant` object.

### `POST /api/merchant`

Creates a new merchant.

#### Parameters

A `MerchantCreateModel` object in the request body.

#### Returns

The newly created merchant.

### `PUT /api/merchant/{id}`

Updates an existing merchant.

#### Parameters

- `id` - the ID of the merchant to update.
- A `MerchantUpdateModel` object in the request body.

#### Returns

None.

### `DELETE /api/merchant/{id}`

Deletes an existing merchant.

#### Parameters

- `id` - the ID of the merchant to delete.

#### Returns

None.

### `POST /api/merchant/import`

Imports a CSV file of merchants.

#### Parameters

- A CSV file in the request body.

#### Returns

None.

### `POST /api/merchant/{merchantId}/payments`

Processes a payment for a specific merchant.

#### Parameters

- `merchantId` - the ID of the merchant to process the payment for.
- A `PaymentModel` object in the request body.

#### Returns

None.

# Transaction Controller

The `TransactionController` class is responsible for handling HTTP requests related to transactions. It contains methods for getting all transactions, getting a specific transaction by ID, creating a new transaction, updating an existing transaction, deleting a transaction, and deleting old transactions.

## Endpoints

### GET /api/transaction
Returns all transactions.

### GET /api/transaction/{id}
Returns a specific transaction by ID.

### POST /api/transaction
Creates a new transaction.

### PUT /api/transaction/{id}
Updates an existing transaction.

### DELETE /api/transaction/{id}
Deletes a transaction.

### POST /api/transaction/delete-old-transactions
Deletes old transactions.

## Controllers

### TransactionController
This is the main controller for transactions. It has an instance of `ITransactionService` and `IMapper` to handle transactions.

### Actions

#### GetAll
Handles GET requests to retrieve all transactions.

##### Request
```
GET /api/transaction
```
##### Response
```
200 OK
[
    {
        "id": "42d0bba3-8c53-4b9f-84e4-afe1fa7c4a4d",
        "merchantId": "e09e8b1e-19f9-4aa6-94bb-d14982140d51",
        "amount": 10.0,
        "status": "Approved",
        "customerEmail": "test@example.com",
        "customerPhone": "1234567890",
        "transactionType": "Charge",
        "referencedTransactionId": null,
        "timestamp": "2023-05-07T17:29:18.685Z"
    },
    {
        "id": "a2e1e60b-4cf7-4bf4-8e8b-c8c045f4c7e6",
        "merchantId": "8d1e590c-0b9d-43f7-8ec3-28d7e6e15d17",
        "amount": 50.0,
        "status": "Approved",
        "customerEmail": "test@example.com",
        "customerPhone": "1234567890",
        "transactionType": "Charge",
        "referencedTransactionId": null,
        "timestamp": "2023-05-07T17:29:18.685Z"
    }
]
