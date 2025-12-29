# DirectoryService
# Directory Service 🏢

**Enterprise-ready centralized directory service for organizational structure management in microservices ecosystems.**

## 📖 Overview

Directory Service is a production-grade backend service designed as a **Single Source of Truth** for organizational data. It provides centralized management of departments, positions, and locations across distributed microservices architecture, eliminating data duplication and ensuring consistency.

### 🎯 Business Problem Solved
In modern microservices ecosystems, services like HR, Warehouse, Logistics, and Finance independently store organizational data, leading to:
- ❌ **Data inconsistency** (departments updated in HR but not in Warehouse)
- ❌ **Duplicated efforts** (same data stored in 10+ different databases)
- ❌ **Complex maintenance** (changes require updates across all services)

Directory Service solves this by providing **one authoritative API** for all organizational data.

## ✨ Key Features

### 🏗️ Hierarchical Department Management
- **Tree structure** with parent-child relationships
- **Bulk reorganization** with transaction safety
- **Path optimization** using materialized paths
- **Automatic depth calculation**

### 🔗 Intelligent Relationships
- **Many-to-many** department ↔ location mapping
- **Many-to-many** department ↔ position mapping
- **Complex validation** of business rules
- **Soft delete** with historical tracking

### ⚡ Performance & Reliability
- **Race condition protection** with pessimistic locking
- **Optimized SQL queries** for hierarchical operations
- **Full transaction support** with ACID compliance
- **Concurrent access** handling

### 🔍 Advanced Querying
- **Pagination & filtering** for all entities
- **Analytical queries** (Top-5 departments by positions)
- **Tree traversal** operations
- **Full-text search** capabilities

## 🏗️ Architecture

Built with **Clean Architecture** and **Domain-Driven Design** principles:
