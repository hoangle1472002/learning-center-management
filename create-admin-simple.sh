#!/bin/bash

echo "🚀 Creating Admin Account for Learning Center..."

# Create a simple admin account using curl
echo "📝 Creating admin user..."

# First, let's try to register a new admin user
curl -X POST "https://localhost:7206/api/Auth/register" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "firstName": "Admin",
    "lastName": "User",
    "email": "admin@learningcenter.com",
    "password": "Admin123!",
    "confirmPassword": "Admin123!",
    "phoneNumber": "+1234567890",
    "address": "123 Admin Street, Admin City",
    "dateOfBirth": "1990-01-01T00:00:00Z",
    "gender": "Other"
  }' 2>/dev/null

echo -e "\n\n🔐 Testing login with admin account..."

# Test login
curl -X POST "https://localhost:7206/api/Auth/login" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "email": "admin@learningcenter.com",
    "password": "Admin123!"
  }' 2>/dev/null

echo -e "\n\n✅ Admin account creation complete!"
echo "📧 Email: admin@learningcenter.com"
echo "🔑 Password: Admin123!"
echo "🌐 API URL: https://localhost:7206"
echo "📚 Swagger: https://localhost:7206/swagger"
