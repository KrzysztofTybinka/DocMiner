# Use the official Python image
FROM python:3.9-slim

# Set the working directory inside the container
WORKDIR /app

# Install system dependencies for pdf2image (Poppler) and Tesseract
RUN apt-get update && apt-get install -y \
    poppler-utils \
    tesseract-ocr \
    tesseract-ocr-eng && \
    apt-get clean

# Set the system PATH
ENV PATH="/usr/bin:$PATH"

# Copy the requirements file
COPY requirements.txt .

# Install Python dependencies
RUN pip install --no-cache-dir -r requirements.txt

# Copy your application code
COPY . .

# Expose the port your Flask app runs on
EXPOSE 8081

# Ensure Gunicorn is in PATH and run it
CMD ["gunicorn", "-b", "0.0.0.0:8081", "main:app"]
