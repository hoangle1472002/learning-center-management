#!/usr/bin/env python3
import requests
import json
import sys

# Disable SSL warnings
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

def test_api():
    base_url = "https://localhost:7206"
    
    print("🔍 Testing Learning Center API...")
    
    # Test 1: Check if API is running
    try:
        response = requests.get(f"{base_url}/swagger/index.html", verify=False, timeout=5)
        if response.status_code == 200:
            print("✅ API is running")
        else:
            print(f"❌ API returned status: {response.status_code}")
            return False
    except Exception as e:
        print(f"❌ Cannot connect to API: {e}")
        return False
    
    # Test 2: Test login with admin account
    print("\n🔐 Testing Admin Login...")
    login_data = {
        "email": "admin@learningcenter.com",
        "password": "Admin123!"
    }
    
    try:
        response = requests.post(
            f"{base_url}/api/Auth/login",
            json=login_data,
            verify=False,
            timeout=10
        )
        
        print(f"Status Code: {response.status_code}")
        print(f"Response: {response.text}")
        
        if response.status_code == 200:
            print("✅ Admin login successful!")
            token_data = response.json()
            print(f"Token: {token_data.get('token', 'N/A')[:50]}...")
            return True
        else:
            print(f"❌ Login failed: {response.text}")
            return False
            
    except Exception as e:
        print(f"❌ Login error: {e}")
        return False

if __name__ == "__main__":
    success = test_api()
    if success:
        print("\n🎉 API is working! You can now login with:")
        print("   Email: admin@learningcenter.com")
        print("   Password: Admin123!")
    else:
        print("\n💥 API test failed!")
        sys.exit(1)
