import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { Toaster } from 'react-hot-toast';
import { AuthProvider } from './contexts/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import Login from './pages/Login';
import ClassManagement from './pages/ClassManagement';
import SubjectManagement from './pages/SubjectManagement';
import ClassSchedule from './pages/ClassSchedule';
import Enrollment from './pages/Enrollment';
import PaymentManagement from './pages/PaymentManagement';
import ExamManagement from './pages/ExamManagement';
import Navigation from './components/Navigation';
import { queryClient } from './lib/queryClient';

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <Router>
          <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
            <Routes>
              <Route path="/" element={<Navigate to="/login" replace />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<div>Register Page - Coming Soon</div>} />
              <Route path="/dashboard" element={
                <ProtectedRoute requiredRoles={['Admin', 'Teacher', 'Student']}>
                  <div>
                    <Navigation />
                    <div className="p-6">
                      <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-6">
                        Dashboard
                      </h1>
                      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                        <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow">
                          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">
                            Total Students
                          </h3>
                          <p className="text-3xl font-bold text-primary-600">150</p>
                        </div>
                        <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow">
                          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">
                            Total Teachers
                          </h3>
                          <p className="text-3xl font-bold text-primary-600">25</p>
                        </div>
                        <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow">
                          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">
                            Active Classes
                          </h3>
                          <p className="text-3xl font-bold text-primary-600">12</p>
                        </div>
                        <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow">
                          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">
                            Revenue
                          </h3>
                          <p className="text-3xl font-bold text-primary-600">$45,000</p>
                        </div>
                      </div>
                    </div>
                  </div>
                </ProtectedRoute>
              } />
              <Route path="/classes" element={
                <ProtectedRoute requiredRoles={['Admin', 'Teacher']}>
                  <div>
                    <Navigation />
                    <div className="p-6">
                      <ClassManagement />
                    </div>
                  </div>
                </ProtectedRoute>
              } />
              <Route path="/subjects" element={
                <ProtectedRoute requiredRoles={['Admin', 'Teacher']}>
                  <div>
                    <Navigation />
                    <div className="p-6">
                      <SubjectManagement />
                    </div>
                  </div>
                </ProtectedRoute>
              } />
              <Route path="/schedule" element={
                <ProtectedRoute requiredRoles={['Admin', 'Teacher', 'Student']}>
                  <div>
                    <Navigation />
                    <div className="p-6">
                      <ClassSchedule />
                    </div>
                  </div>
                </ProtectedRoute>
              } />
              <Route path="/enrollment" element={
                <ProtectedRoute requiredRoles={['Admin', 'Student']}>
                  <div>
                    <Navigation />
                    <div className="p-6">
                      <Enrollment />
                    </div>
                  </div>
                </ProtectedRoute>
              } />
            <Route path="/payments" element={
              <ProtectedRoute requiredRoles={['Admin', 'Teacher']}>
                <div>
                  <Navigation />
                  <PaymentManagement />
                </div>
              </ProtectedRoute>
            } />
            <Route path="/exams" element={
              <ProtectedRoute requiredRoles={['Admin', 'Teacher']}>
                <div>
                  <Navigation />
                  <ExamManagement />
                </div>
              </ProtectedRoute>
            } />
            <Route path="*" element={<Navigate to="/login" replace />} />
            </Routes>
          </div>
        </Router>
        <Toaster position="top-right" />
        <ReactQueryDevtools initialIsOpen={false} />
      </AuthProvider>
    </QueryClientProvider>
  );
}

export default App;
