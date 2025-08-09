# Personal Tax Income Calculator – AU 2024–25

A simple **.NET 8** API for calculating **Australian resident personal income tax** for the **2024–25 financial year**, **without Medicare levy** in v1.  
Includes unit tests and a historical reference to the original Java version (see [`/docs/legacy-java`](docs/legacy-java)).

---

## Features (v1)
- Defaults to **Australian resident tax rates (2024–25)**.
- Calculates income tax based on current tax brackets.
- No Medicare levy applied (future version may add this).
- Structured in a clean, testable, and extensible architecture.
- Unit tests included for verification.

---

## Getting Started

### 1. Clone the repo
```bash
git clone https://github.com/<your-username>/personal-tax-income.git
cd personal-tax-income

### 2. Run the API

```bash
cd Tax.Api
dotnet run
