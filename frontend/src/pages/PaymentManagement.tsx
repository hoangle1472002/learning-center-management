import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { toast } from 'react-hot-toast';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../components/ui/Card';
import { apiClient } from '../lib/axios';
import { format } from 'date-fns';

interface Payment {
    id: number;
    studentId: number;
    studentName: string;
    classId: number;
    className: string;
    amount: number;
    dueDate: string;
    paidDate?: string;
    status: string;
    paymentMethod?: string;
    notes?: string;
    transactionId?: string;
    createdAt: string;
}

interface CreatePaymentRequest {
    studentId: number;
    classId: number;
    amount: number;
    dueDate: string;
    paymentMethod?: string;
    notes?: string;
}

interface ProcessPaymentRequest {
    paymentMethod: string;
    transactionId?: string;
    notes?: string;
    receiptUrl?: string;
}

const PaymentManagement: React.FC = () => {
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [showProcessModal, setShowProcessModal] = useState(false);
    const [selectedPayment, setSelectedPayment] = useState<Payment | null>(null);
    const [filters, setFilters] = useState({
        status: '',
        studentId: '',
        classId: ''
    });

    const queryClient = useQueryClient();

    // Fetch payments
    const { data: payments = [], isLoading } = useQuery({
        queryKey: ['payments', filters],
        queryFn: async () => {
            const params = new URLSearchParams();
            if (filters.status) params.append('status', filters.status);
            if (filters.studentId) params.append('studentId', filters.studentId);
            if (filters.classId) params.append('classId', filters.classId);

            const response = await apiClient.get(`/api/Payment?${params.toString()}`);
            return response.data;
        }
    });

    // Create payment mutation
    const createPaymentMutation = useMutation({
        mutationFn: async (data: CreatePaymentRequest) => {
            const response = await apiClient.post('/api/Payment', data);
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['payments'] });
            toast.success('Payment created successfully');
            setShowCreateModal(false);
        },
        onError: (error: any) => {
            toast.error(error.response?.data?.message || 'Failed to create payment');
        }
    });

    // Process payment mutation
    const processPaymentMutation = useMutation({
        mutationFn: async ({ paymentId, data }: { paymentId: number; data: ProcessPaymentRequest }) => {
            const response = await apiClient.post(`/api/Payment/${paymentId}/process`, data);
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['payments'] });
            toast.success('Payment processed successfully');
            setShowProcessModal(false);
            setSelectedPayment(null);
        },
        onError: (error: any) => {
            toast.error(error.response?.data?.message || 'Failed to process payment');
        }
    });

    const handleCreatePayment = (data: CreatePaymentRequest) => {
        createPaymentMutation.mutate(data);
    };

    const handleProcessPayment = (data: ProcessPaymentRequest) => {
        if (selectedPayment) {
            processPaymentMutation.mutate({ paymentId: selectedPayment.id, data });
        }
    };

    const getStatusColor = (status: string) => {
        switch (status.toLowerCase()) {
            case 'paid':
                return 'bg-green-100 text-green-800';
            case 'pending':
                return 'bg-yellow-100 text-yellow-800';
            case 'overdue':
                return 'bg-red-100 text-red-800';
            default:
                return 'bg-gray-100 text-gray-800';
        }
    };

    if (isLoading) {
        return (
            <div className="flex items-center justify-center h-64">
                <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-primary-600"></div>
            </div>
        );
    }

    return (
        <div className="p-6">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Payment Management</h1>
                <Button onClick={() => setShowCreateModal(true)}>
                    Create Payment
                </Button>
            </div>

            {/* Filters */}
            <Card className="mb-6">
                <CardHeader>
                    <CardTitle>Filters</CardTitle>
                </CardHeader>
                <CardContent>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Status
                            </label>
                            <select
                                value={filters.status}
                                onChange={(e) => setFilters({ ...filters, status: e.target.value })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500"
                            >
                                <option value="">All Status</option>
                                <option value="Pending">Pending</option>
                                <option value="Paid">Paid</option>
                                <option value="Overdue">Overdue</option>
                            </select>
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Student ID
                            </label>
                            <Input
                                type="number"
                                value={filters.studentId}
                                onChange={(e) => setFilters({ ...filters, studentId: e.target.value })}
                                placeholder="Enter student ID"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Class ID
                            </label>
                            <Input
                                type="number"
                                value={filters.classId}
                                onChange={(e) => setFilters({ ...filters, classId: e.target.value })}
                                placeholder="Enter class ID"
                            />
                        </div>
                    </div>
                </CardContent>
            </Card>

            {/* Payments Table */}
            <Card>
                <CardHeader>
                    <CardTitle>Payments</CardTitle>
                    <CardDescription>
                        Manage student payments and track payment status
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    <div className="overflow-x-auto">
                        <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
                            <thead className="bg-gray-50 dark:bg-gray-800">
                                <tr>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                                        Student
                                    </th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                                        Class
                                    </th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                                        Amount
                                    </th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                                        Due Date
                                    </th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                                        Status
                                    </th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                                        Actions
                                    </th>
                                </tr>
                            </thead>
                            <tbody className="bg-white dark:bg-gray-900 divide-y divide-gray-200 dark:divide-gray-700">
                                {payments.map((payment: Payment) => (
                                    <tr key={payment.id}>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900 dark:text-white">
                                            {payment.studentName}
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">
                                            {payment.className}
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-white">
                                            ${payment.amount.toFixed(2)}
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">
                                            {format(new Date(payment.dueDate), 'MMM dd, yyyy')}
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap">
                                            <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(payment.status)}`}>
                                                {payment.status}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                                            {payment.status === 'Pending' && (
                                                <Button
                                                    size="sm"
                                                    onClick={() => {
                                                        setSelectedPayment(payment);
                                                        setShowProcessModal(true);
                                                    }}
                                                >
                                                    Process
                                                </Button>
                                            )}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </CardContent>
            </Card>

            {/* Create Payment Modal */}
            {showCreateModal && (
                <CreatePaymentModal
                    onClose={() => setShowCreateModal(false)}
                    onSubmit={handleCreatePayment}
                    isLoading={createPaymentMutation.isPending}
                />
            )}

            {/* Process Payment Modal */}
            {showProcessModal && selectedPayment && (
                <ProcessPaymentModal
                    payment={selectedPayment}
                    onClose={() => {
                        setShowProcessModal(false);
                        setSelectedPayment(null);
                    }}
                    onSubmit={handleProcessPayment}
                    isLoading={processPaymentMutation.isPending}
                />
            )}
        </div>
    );
};

// Create Payment Modal Component
const CreatePaymentModal: React.FC<{
    onClose: () => void;
    onSubmit: (data: CreatePaymentRequest) => void;
    isLoading: boolean;
}> = ({ onClose, onSubmit, isLoading }) => {
    const [formData, setFormData] = useState<CreatePaymentRequest>({
        studentId: 0,
        classId: 0,
        amount: 0,
        dueDate: '',
        paymentMethod: '',
        notes: ''
    });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <Card className="w-full max-w-md">
                <CardHeader>
                    <CardTitle>Create Payment</CardTitle>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit} className="space-y-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Student ID
                            </label>
                            <Input
                                type="number"
                                value={formData.studentId}
                                onChange={(e) => setFormData({ ...formData, studentId: parseInt(e.target.value) })}
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Class ID
                            </label>
                            <Input
                                type="number"
                                value={formData.classId}
                                onChange={(e) => setFormData({ ...formData, classId: parseInt(e.target.value) })}
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Amount
                            </label>
                            <Input
                                type="number"
                                step="0.01"
                                value={formData.amount}
                                onChange={(e) => setFormData({ ...formData, amount: parseFloat(e.target.value) })}
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Due Date
                            </label>
                            <Input
                                type="date"
                                value={formData.dueDate}
                                onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Payment Method
                            </label>
                            <Input
                                value={formData.paymentMethod}
                                onChange={(e) => setFormData({ ...formData, paymentMethod: e.target.value })}
                                placeholder="e.g., Cash, Bank Transfer"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Notes
                            </label>
                            <textarea
                                value={formData.notes}
                                onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500"
                                rows={3}
                            />
                        </div>
                        <div className="flex justify-end space-x-2">
                            <Button type="button" variant="outline" onClick={onClose}>
                                Cancel
                            </Button>
                            <Button type="submit" disabled={isLoading}>
                                {isLoading ? 'Creating...' : 'Create Payment'}
                            </Button>
                        </div>
                    </form>
                </CardContent>
            </Card>
        </div>
    );
};

// Process Payment Modal Component
const ProcessPaymentModal: React.FC<{
    payment: Payment;
    onClose: () => void;
    onSubmit: (data: ProcessPaymentRequest) => void;
    isLoading: boolean;
}> = ({ payment, onClose, onSubmit, isLoading }) => {
    const [formData, setFormData] = useState<ProcessPaymentRequest>({
        paymentMethod: '',
        transactionId: '',
        notes: '',
        receiptUrl: ''
    });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <Card className="w-full max-w-md">
                <CardHeader>
                    <CardTitle>Process Payment</CardTitle>
                    <CardDescription>
                        Process payment for {payment.studentName} - ${payment.amount.toFixed(2)}
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit} className="space-y-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Payment Method *
                            </label>
                            <select
                                value={formData.paymentMethod}
                                onChange={(e) => setFormData({ ...formData, paymentMethod: e.target.value })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500"
                                required
                            >
                                <option value="">Select payment method</option>
                                <option value="Cash">Cash</option>
                                <option value="Bank Transfer">Bank Transfer</option>
                                <option value="Credit Card">Credit Card</option>
                                <option value="Check">Check</option>
                            </select>
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Transaction ID
                            </label>
                            <Input
                                value={formData.transactionId}
                                onChange={(e) => setFormData({ ...formData, transactionId: e.target.value })}
                                placeholder="Enter transaction ID"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Receipt URL
                            </label>
                            <Input
                                value={formData.receiptUrl}
                                onChange={(e) => setFormData({ ...formData, receiptUrl: e.target.value })}
                                placeholder="Enter receipt URL"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Notes
                            </label>
                            <textarea
                                value={formData.notes}
                                onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
                                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500"
                                rows={3}
                            />
                        </div>
                        <div className="flex justify-end space-x-2">
                            <Button type="button" variant="outline" onClick={onClose}>
                                Cancel
                            </Button>
                            <Button type="submit" disabled={isLoading}>
                                {isLoading ? 'Processing...' : 'Process Payment'}
                            </Button>
                        </div>
                    </form>
                </CardContent>
            </Card>
        </div>
    );
};

export default PaymentManagement;
