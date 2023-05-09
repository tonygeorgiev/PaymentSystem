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
