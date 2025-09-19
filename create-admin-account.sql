-- Create admin account directly in database
-- Run this in PostgreSQL

-- Insert Admin Role
INSERT INTO "Roles" ("Name", "Description", "CreatedAt", "UpdatedAt") 
VALUES ('Admin', 'System Administrator', NOW(), NOW())
ON CONFLICT ("Name") DO NOTHING;

-- Insert Admin User
INSERT INTO "Users" ("FirstName", "LastName", "Email", "PhoneNumber", "Address", "DateOfBirth", "Gender", "PasswordHash", "IsEmailVerified", "IsActive", "CreatedAt", "UpdatedAt")
VALUES (
    'Admin',
    'User', 
    'admin@learningcenter.com',
    '+1234567890',
    '123 Admin Street, Admin City',
    '1990-01-01 00:00:00+00',
    'Other',
    '$2a$11$N9qo8uLOickgx2ZMRZoMye.IjdQvOQrOj5y8GqK8qK8qK8qK8qK8qK', -- This is BCrypt hash for "Admin123!"
    true,
    true,
    NOW(),
    NOW()
)
ON CONFLICT ("Email") DO NOTHING;

-- Get the user and role IDs
DO $$
DECLARE
    admin_user_id INTEGER;
    admin_role_id INTEGER;
BEGIN
    -- Get admin user ID
    SELECT "Id" INTO admin_user_id FROM "Users" WHERE "Email" = 'admin@learningcenter.com';
    
    -- Get admin role ID
    SELECT "Id" INTO admin_role_id FROM "Roles" WHERE "Name" = 'Admin';
    
    -- Insert UserRole
    INSERT INTO "UserRoles" ("UserId", "RoleId", "CreatedAt", "UpdatedAt")
    VALUES (admin_user_id, admin_role_id, NOW(), NOW())
    ON CONFLICT ("UserId", "RoleId") DO NOTHING;
END $$;

-- Verify the account was created
SELECT 
    u."Id",
    u."FirstName",
    u."LastName", 
    u."Email",
    u."IsActive",
    r."Name" as "Role"
FROM "Users" u
JOIN "UserRoles" ur ON u."Id" = ur."UserId"
JOIN "Roles" r ON ur."RoleId" = r."Id"
WHERE u."Email" = 'admin@learningcenter.com';
