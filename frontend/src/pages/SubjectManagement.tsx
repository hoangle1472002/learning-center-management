import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '../components/ui/Card';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';

interface Subject {
    id: number;
    name: string;
    description?: string;
    code?: string;
    price: number;
    duration: number;
    level?: string;
    prerequisites?: string;
    isActive: boolean;
    classCount: number;
    studentCount: number;
}

const SubjectManagement: React.FC = () => {
    const [subjects, setSubjects] = useState<Subject[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [filterLevel, setFilterLevel] = useState('');
    const [filterActive, setFilterActive] = useState<boolean | null>(null);
    const [showCreateModal, setShowCreateModal] = useState(false);

    useEffect(() => {
        fetchSubjects();
    }, []);

    const fetchSubjects = async () => {
        try {
            setLoading(true);
            const response = await fetch('/api/Subject');
            if (response.ok) {
                const data = await response.json();
                setSubjects(data);
            }
        } catch (error) {
            console.error('Error fetching subjects:', error);
        } finally {
            setLoading(false);
        }
    };

    const filteredSubjects = subjects.filter(subject => {
        const matchesSearch = subject.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
            (subject.description && subject.description.toLowerCase().includes(searchTerm.toLowerCase())) ||
            (subject.code && subject.code.toLowerCase().includes(searchTerm.toLowerCase()));
        const matchesLevel = !filterLevel || subject.level === filterLevel;
        const matchesActive = filterActive === null || subject.isActive === filterActive;
        return matchesSearch && matchesLevel && matchesActive;
    });

    const getLevelColor = (level?: string) => {
        switch (level) {
            case 'Beginner': return 'bg-green-100 text-green-800';
            case 'Intermediate': return 'bg-yellow-100 text-yellow-800';
            case 'Advanced': return 'bg-red-100 text-red-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    };

    if (loading) {
        return (
            <div className="flex items-center justify-center h-64">
                <div className="text-lg">Loading subjects...</div>
            </div>
        );
    }

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-3xl font-bold">Subject Management</h1>
                <Button onClick={() => setShowCreateModal(true)}>
                    Create New Subject
                </Button>
            </div>

            {/* Filters */}
            <div className="flex gap-4">
                <Input
                    placeholder="Search subjects..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="max-w-sm"
                />
                <select
                    value={filterLevel}
                    onChange={(e) => setFilterLevel(e.target.value)}
                    className="px-3 py-2 border border-gray-300 rounded-md"
                >
                    <option value="">All Levels</option>
                    <option value="Beginner">Beginner</option>
                    <option value="Intermediate">Intermediate</option>
                    <option value="Advanced">Advanced</option>
                </select>
                <select
                    value={filterActive === null ? '' : filterActive.toString()}
                    onChange={(e) => setFilterActive(e.target.value === '' ? null : e.target.value === 'true')}
                    className="px-3 py-2 border border-gray-300 rounded-md"
                >
                    <option value="">All Status</option>
                    <option value="true">Active</option>
                    <option value="false">Inactive</option>
                </select>
            </div>

            {/* Subjects Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {filteredSubjects.map((subject) => (
                    <Card key={subject.id} className="hover:shadow-lg transition-shadow">
                        <CardHeader>
                            <div className="flex justify-between items-start">
                                <CardTitle className="text-lg">{subject.name}</CardTitle>
                                <div className="flex gap-2">
                                    {subject.level && (
                                        <span className={`px-2 py-1 rounded-full text-xs font-medium ${getLevelColor(subject.level)}`}>
                                            {subject.level}
                                        </span>
                                    )}
                                    <span className={`px-2 py-1 rounded-full text-xs font-medium ${subject.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
                                        {subject.isActive ? 'Active' : 'Inactive'}
                                    </span>
                                </div>
                            </div>
                            {subject.code && (
                                <p className="text-sm text-gray-600">Code: {subject.code}</p>
                            )}
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-2">
                                {subject.description && (
                                    <p className="text-sm text-gray-600 line-clamp-2">{subject.description}</p>
                                )}
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Price:</span>
                                    <span className="font-medium">${subject.price}</span>
                                </div>
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Duration:</span>
                                    <span>{subject.duration} hours</span>
                                </div>
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Classes:</span>
                                    <span>{subject.classCount}</span>
                                </div>
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Students:</span>
                                    <span>{subject.studentCount}</span>
                                </div>
                                {subject.prerequisites && (
                                    <div className="text-sm">
                                        <span className="text-gray-600">Prerequisites:</span>
                                        <p className="text-xs text-gray-500 mt-1">{subject.prerequisites}</p>
                                    </div>
                                )}
                            </div>
                            <div className="flex gap-2 mt-4">
                                <Button size="sm" variant="outline">
                                    Edit
                                </Button>
                                <Button size="sm" variant="outline">
                                    View Details
                                </Button>
                                <Button size="sm" variant="outline">
                                    View Classes
                                </Button>
                            </div>
                        </CardContent>
                    </Card>
                ))}
            </div>

            {filteredSubjects.length === 0 && (
                <div className="text-center py-12">
                    <p className="text-gray-500 text-lg">No subjects found</p>
                </div>
            )}
        </div>
    );
};

export default SubjectManagement;
