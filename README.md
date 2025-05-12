# Coworking - Sistema de Reservas

Este é um sistema de gerenciamento de reservas para um espaço de coworking, desenvolvido como parte de um desafio técnico. A solução utiliza **.NET 8**, **PostgreSQL**, e segue os princípios da **Clean Architecture**, com separação de responsabilidades entre as camadas `Domain`, `Application`, `Infra` e `Web`.

## 🛠 Tecnologias e Padrões Utilizados

- ASP.NET Core 8 (MVC + Web API)
- PostgreSQL
- Entity Framework Core
- xUnit (testes unitários)
- Moq (mocks em testes)
- AutoMapper
- Repository + Unit of Work
- Clean Architecture
- Validações com DataAnnotations + custom validations

## 🧠 Organização em Camadas

- **Domain**: contém as entidades, interfaces dos repositórios e regras de negócio puras.
- **Application**: contém os serviços de aplicação (orquestração da lógica), DTOs, validações e mapeamentos.
- **Infrastructure**: implementação de acesso a dados via EF Core, Unit of Work e migrations.
- **Web**: camada de apresentação (Razor Pages), controladores e endpoints públicos.

## 📌 Funcionalidades Implementadas

- CRUD de usuários, salas e reservas
- Validação de conflitos de reserva (horários sobrepostos)
- Validação de cancelamento com no mínimo 24h de antecedência
- Integração com banco PostgreSQL
- Testes unitários de serviços e repositórios
- Geração automática de documentação via Swagger

## 🧪 Executando os Testes

```bash
dotnet test
