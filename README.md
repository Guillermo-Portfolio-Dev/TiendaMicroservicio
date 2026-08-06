# TiendaMicroservicio

Ecosistema de **Microservicios** desarrollado en **.NET**, diseñado bajo el patrón **Vertical Slice Architecture** y preparado para despliegue distribuido mediante **Docker Compose**. Implementa persistencia políglota y ruteo centralizado a través de un API Gateway.

---

## ⚡ Componentes y Arquitectura

* **`ApiGateway`**: Puerta de entrada unificada configurada con **Ocelot** para el enrutamiento, balanceo de carga y aislamiento de microservicios backend.
* **`Usuarios.Api`**: Microservicio encargado del dominio de usuarios y autenticación.
* **`Productos.Api`**: Microservicio para el catálogo de productos y gestión de inventario.
* **`Orders.Api`**: Microservicio enfocado en la gestión, procesamiento y estado de órdenes/pedidos.

---

## 🛠️ Tecnologías y Prácticas

* **Plataforma:** .NET / C#
* **Arquitectura:** Microservices Architecture & Vertical Slice Architecture
* **API Gateway:** Ocelot API Gateway
* **Bases de Datos (Políglota):** MongoDB, MySQL, PostgreSQL
* **Contenedores:** Docker & Docker Compose
* **Patrones & Conceptos:** Decoupled Architecture, Single Responsibility, RESTful Endpoints

---

## 🏗️ Diagrama de Flujo

```text
               ┌───────────────────────┐
               │      Cliente / API    │
               └───────────┬───────────┘
                           │
                           ▼
               ┌───────────────────────┐
               │    ApiGateway (Ocelot)│
               └─┬─────────┬─────────┬─┘
                 │         │         │
      ┌──────────┘         │         └──────────┐
      ▼                    ▼                    ▼
┌──────────────┐   ┌──────────────┐   ┌──────────────┐
│ Usuarios.Api │   │Productos.Api │   │  Orders.Api  │
└──────┬───────┘   └──────┬───────┘   └──────┬───────┘
       │                  │                  │
       ▼                  ▼                  ▼
  [ Database ]       [ Database ]       [ Database ]
