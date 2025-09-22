import React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const Navigation: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { user, logout, hasRole } = useAuth();

    const navItems = [
        { path: '/dashboard', label: 'Dashboard', icon: '📊', roles: ['Admin', 'Teacher', 'Student'] },
        { path: '/classes', label: 'Classes', icon: '🏫', roles: ['Admin', 'Teacher'] },
        { path: '/subjects', label: 'Subjects', icon: '📚', roles: ['Admin', 'Teacher'] },
        { path: '/schedule', label: 'Schedule', icon: '📅', roles: ['Admin', 'Teacher', 'Student'] },
        { path: '/enrollment', label: 'Enrollment', icon: '👥', roles: ['Admin', 'Student'] },
        { path: '/payments', label: 'Payments', icon: '💳', roles: ['Admin', 'Teacher'] },
        { path: '/exams', label: 'Exams', icon: '📝', roles: ['Admin', 'Teacher'] },
    ].filter(item => hasRole('Admin') || item.roles.some(role => hasRole(role)));

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <nav className="bg-white shadow-lg">
            <div className="max-w-7xl mx-auto px-4">
                <div className="flex justify-between h-16">
                    <div className="flex">
                        <div className="flex-shrink-0 flex items-center">
                            <h1 className="text-xl font-bold text-gray-800">Learning Center</h1>
                        </div>
                        <div className="hidden sm:ml-6 sm:flex sm:space-x-8">
                            {navItems.map((item) => (
                                <Link
                                    key={item.path}
                                    to={item.path}
                                    className={`${location.pathname === item.path
                                        ? 'border-blue-500 text-gray-900'
                                        : 'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700'
                                        } inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium`}
                                >
                                    <span className="mr-2">{item.icon}</span>
                                    {item.label}
                                </Link>
                            ))}
                        </div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <div className="text-sm text-gray-700">
                            Welcome, <span className="font-medium">{user?.firstName} {user?.lastName}</span>
                            <span className="ml-2 px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded-full">
                                {user?.roles?.[0]}
                            </span>
                        </div>
                        <button
                            onClick={handleLogout}
                            className="bg-gray-800 text-white px-3 py-2 rounded-md text-sm font-medium hover:bg-gray-700"
                        >
                            Logout
                        </button>
                    </div>
                </div>
            </div>
        </nav>
    );
};

export default Navigation;
