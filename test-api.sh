#!/bin/bash

echo "Testing Learning Center API..."

# Test Swagger
echo "1. Testing Swagger UI..."
curl -s -k https://localhost:7206/swagger/index.html | head -3

echo -e "\n\n2. Testing Subject API..."
curl -X GET "https://localhost:7206/api/Subject" -k -H "Content-Type: application/json"

echo -e "\n\n3. Testing Class API..."
curl -X GET "https://localhost:7206/api/Class" -k -H "Content-Type: application/json"

echo -e "\n\n4. Testing Login API..."
curl -X POST "https://localhost:7206/api/Auth/login" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "email": "admin@learningcenter.com",
    "password": "Admin123!"
  }'

echo -e "\n\nAPI Test Complete!"
