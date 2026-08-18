# Database Schema Overview

## Core System Tables
- **Departments**: Primary Key `Id`
- **Employees**: Primary Key `Id`, Foreign Key `DepartmentId` -> `Departments(Id)`
- **Teams**: Primary Key `Id`, Foreign Key `DepartmentId` -> `Departments(Id)`
- **TeamMembers**: Composite Primary Key (`TeamId`, `EmployeeId`)