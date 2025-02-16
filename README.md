
## Flight Reservation - Clean Architecture

This is a sample flight reservation service using .Net 8, Clean Architecture, MediateR, Sql Server, Entity Framework Core and Docker.


## Attention

This project is a quick sample to be used as a template and make sure to change it base on need.
Do not use the exact same secret keys or authentication flow.


[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)


## Predefined Users for test

| Mobile             | Password                                                                |
| ----------------- | ------------------------------------------------------------------ |
| 09121234567 (Admin) | 123456 |
| 09381234567 (Customer) | 123456 |



## Docker

To Dockerize the project using multi stage build.

```bash
  docker build .
```


## Run Locally

Clone the project

```bash
  git clone https://github.com/thisissoroush/FlightReservation-Clean.git
```

Go to the project directory

```bash
  cd FlightReservation
```

Install dependencies

```bash
  dotnet restore
```

Build the project

```bash
  dotnet build
```

Start the server

```bash
  dotnet run
```




## Author

- [@Soroush Nasiri](https://www.github.com/Thisissoroush)

