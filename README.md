# RAG Project

## Overview

This project is a modular and Docker-based Retrieval-Augmented Generation (RAG) system. It integrates Python-based OCR preprocessing, Tesseract OCR, C# API for RAG operations, and Chroma DB for managing vector embeddings. RAG systems enhance information retrieval by combining embedding-based search with generative AI capabilities, making it ideal for unstructured document search, Q&A systems, and context-aware content generation.

---

## Features

- **Document Collections Management**:
  - Create, retrieve, and delete document collections.
  - Add multiple documents to a collection for OCR and embedding.
- **Embeddings Management**:
  - Generate embeddings for documents using configurable embedding models.
  - Query documents based on vector similarity.
- **Integration**:
  - Python OCR API for document preprocessing and OCR.
  - C# RAG API for embedding generation, document management, and querying.
  - Chroma DB for fast and scalable vector search.
- **Flexible Embedding Models**:
  - Supports OpenAI and Ollama-based embedding models via a factory design pattern.

---

## Getting Started

### Prerequisites

- Docker installed on your machine
- A compatible embedding model service (e.g., OpenAI or Ollama)

### Folder Structure

```
src/
├── OCR (Python, Tesseract wrapper)
├── RAG (C#, main API)
    ├── ApiResponses
    ├── BLL
    ├── Common
    ├── Endpoints
    ├── ExternalServices
    ├── Handlers
    ├── Models
    ├── Repository
    ├── Requests
    ├── Services
        ├── EmbeddingServiceFactory
        ├── OllamaEmbeddingService
        ├── OpenAIEmbeddingService
    ├── Validators
    └── appsettings.json
```

### Setup Instructions

1. **Clone the Repository**:

   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Configure the Environment**:

   - Update `appsettings.json` in the `RAG` folder with your embedding model settings. Example:
     ```json
     "EmbeddingModelSettings": {
       "ProviderName": "Ollama",
       "ModelName": "mxbai-embed-large",
       "Token": null,
       "Url": "http://host.docker.internal:11434/v1/embeddings"
     }
     ```

3. **Run Docker Compose**:

   ```bash
   docker-compose up --build
   ```

   - Services will run on the following ports:
     - **OCR API**: `http://localhost:8081`
     - **RAG API**: `http://localhost:8080`
     - **Chroma DB**: `http://localhost:8000`

4. **Verify Services**:

   - Access the RAG API Swagger documentation at `http://localhost:8080/swagger`.

---

## Usage

### 1. Document Collection Endpoints

- **Create Collection**:
  ```http
  POST /DocumentCollection
  {
    "name": "example-collection"
  }
  ```
- **Retrieve Collections**:
  ```http
  GET /DocumentCollection
  ```
- **Delete Collection**:
  ```http
  DELETE /DocumentCollection/{collectionName}
  ```

### 2. Embedding Endpoints

- **Add Document to Collection**:
  ```http
  POST /Embeddings
  {
    "collectionName": "example-collection",
    "filePath": "path/to/document.pdf"
  }
  ```
- **Query Collection**:
  ```http
  GET /QueryEmbeddings
  {
    "collectionName": "example-collection",
    "query": "search query"
  }
  ```
- **Delete Embedding**:
  ```http
  DELETE /Embeddings/{embeddingId}
  ```

---

## What is RAG?

Retrieval-Augmented Generation (RAG) is a hybrid approach that combines information retrieval techniques with generative AI models. It enables:

- Searching through vast amounts of unstructured data.
- Generating precise, context-aware answers to user queries.
- Enhancing traditional search systems with AI-powered embeddings and vector similarity.

### Use Cases

- Question-Answering systems
- Context-based document retrieval
- Legal or technical document analysis
- Content generation with factual accuracy

---

## Licensing

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

---

## Contributions

Contributions, issues, and feature requests are welcome. Please open an issue or submit a pull request to contribute.

---

## Contact

For questions or support, please contact [Your Name or Team Email].

