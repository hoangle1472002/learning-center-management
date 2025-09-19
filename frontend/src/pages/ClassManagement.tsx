import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '../components/ui/Card';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';

interface Class {
    id: number;
    name: string;
    description?: string;
    code?: string;
    subjectName: string;
    teacherName: string;
    maxStudents: number;
    currentStudents: number;
    startDate?: string;
    endDate?: string;
    price?: number;
    status?: string;
    room?: string;
    notes?: string;
    isActive: boolean;
    enrollmentRate: number;
}

const ClassManagement: React.FC = () => {
    const [classes, setClasses] = useState<Class[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [filterStatus, setFilterStatus] = useState('');
    const [showCreateModal, setShowCreateModal] = useState(false);

    useEffect(() => {
        fetchClasses();
    }, []);

    const fetchClasses = async () => {
        try {
            setLoading(true);
            const response = await fetch('/api/Class');
            if (response.ok) {
                const data = await response.json();
                setClasses(data);
            }
        } catch (error) {
            console.error('Error fetching classes:', error);
        } finally {
            setLoading(false);
        }
    };

    const filteredClasses = classes.filter(cls => {
        const matchesSearch = cls.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
            cls.subjectName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            cls.teacherName.toLowerCase().includes(searchTerm.toLowerCase());
        const matchesStatus = !filterStatus || cls.status === filterStatus;
        return matchesSearch && matchesStatus;
    });

    const getStatusColor = (status?: string) => {
        switch (status) {
            case 'Active': return 'bg-green-100 text-green-800';
            case 'Draft': return 'bg-gray-100 text-gray-800';
            case 'Completed': return 'bg-blue-100 text-blue-800';
            case 'Cancelled': return 'bg-red-100 text-red-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    };

    if (loading) {
        return (
            <div className="flex items-center justify-center h-64">
                <div className="text-lg">Loading classes...</div>
            </div>
        );
    }

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-3xl font-bold">Class Management</h1>
                <Button onClick={() => setShowCreateModal(true)}>
                    Create New Class
                </Button>
            </div>

            {/* Filters */}
            <div className="flex gap-4">
                <Input
                    placeholder="Search classes..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="max-w-sm"
                />
                <select
                    value={filterStatus}
                    onChange={(e) => setFilterStatus(e.target.value)}
                    className="px-3 py-2 border border-gray-300 rounded-md"
                >
                    <option value="">All Status</option>
                    <option value="Draft">Draft</option>
                    <option value="Active">Active</option>
                    <option value="Completed">Completed</option>
                    <option value="Cancelled">Cancelled</option>
                </select>
            </div>

            {/* Classes Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {filteredClasses.map((cls) => (
                    <Card key={cls.id} className="hover:shadow-lg transition-shadow">
                        <CardHeader>
                            <div className="flex justify-between items-start">
                                <CardTitle className="text-lg">{cls.name}</CardTitle>
                                <span className={`px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(cls.status)}`}>
                                    {cls.status || 'Draft'}
                                </span>
                            </div>
                            <p className="text-sm text-gray-600">{cls.subjectName}</p>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-2">
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Teacher:</span>
                                    <span>{cls.teacherName}</span>
                                </div>
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Students:</span>
                                    <span>{cls.currentStudents}/{cls.maxStudents}</span>
                                </div>
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Enrollment:</span>
                                    <span>{cls.enrollmentRate.toFixed(1)}%</span>
                                </div>
                                {cls.room && (
                                    <div className="flex justify-between text-sm">
                                        <span className="text-gray-600">Room:</span>
                                        <span>{cls.room}</span>
                                    </div>
                                )}
                                {cls.price && (
                                    <div className="flex justify-between text-sm">
                                        <span className="text-gray-600">Price:</span>
                                        <span>${cls.price}</span>
                                    </div>
                                )}
                                <div className="w-full bg-gray-200 rounded-full h-2 mt-2">
                                    <div
                                        className="bg-blue-600 h-2 rounded-full"
                                        style={{ width: `${cls.enrollmentRate}%` }}
                                    ></div>
                                </div>
                            </div>
                            <div className="flex gap-2 mt-4">
                                <Button size="sm" variant="outline">
                                    Edit
                                </Button>
                                <Button size="sm" variant="outline">
                                    View Details
                                </Button>
                                <Button size="sm" variant="outline">
                                    Enroll Students
                                </Button>
                            </div>
                        </CardContent>
                    </Card>
                ))}
            </div>

            {filteredClasses.length === 0 && (
                <div className="text-center py-12">
                    <p className="text-gray-500 text-lg">No classes found</p>
                </div>
            )}
        </div>
    );
};

export default ClassManagement;
