# RAG Project

## Overview
This modular, Docker-based Retrieval-Augmented Generation (RAG) system includes Python OCR preprocessing with Tesseract, a C# API for RAG operations, and Chroma DB for vector embeddings. By supporting Ollama-based embedding models, it can run fully offline, ensuring secure handling of sensitive data.

---

## Features

- **Document Collections Management**:
  - Create, retrieve, and delete document collections.
  - Add multiple documents to a collection.
- **Embeddings Management**:
  - Generate embeddings for documents using configurable embedding models.
  - Query documents based on vector similarity.
- **Document prcessing**:
  - .pdf files preprocessing and OCR.
  - Cleanup and chunking documents.
  - Chunks embedding.
  - Chroma DB for fast and scalable vector search.
- **Supported Embedding Models**:
  - OpenAI
  - Ollama

---

## Getting Started

### Prerequisites

- Docker installed on your machine

### Setup Instructions

1. **Clone the Repository**:

   ```bash
   git clone [https://github.com/KrzysztofTybinka/DocMiner](https://github.com/KrzysztofTybinka/DocMiner)
   cd <repository-folder>
   ```

2. **Configure the Environment**:

   - Update `appsettings.json` in the `RAG` folder with your embedding model settings. Example:
     ```json
     "EmbeddingModelSettings": {
       "ProviderName": "Ollama",
       "ModelName": "mxbai-embed-large",
       "Token": null,
       "Url": "http://localhost:11434/v1/embeddings"
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

