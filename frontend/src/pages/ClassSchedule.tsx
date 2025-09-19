import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '../components/ui/Card';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';

interface ClassSchedule {
    id: number;
    className: string;
    subjectName: string;
    teacherName: string;
    dayOfWeek: string;
    startTime: string;
    endTime: string;
    room?: string;
    isActive: boolean;
}

const ClassSchedule: React.FC = () => {
    const [schedules, setSchedules] = useState<ClassSchedule[]>([]);
    const [loading, setLoading] = useState(true);
    const [selectedClass, setSelectedClass] = useState<number | null>(null);
    const [searchTerm, setSearchTerm] = useState('');
    const [filterDay, setFilterDay] = useState('');

    const daysOfWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

    useEffect(() => {
        fetchSchedules();
    }, [selectedClass]);

    const fetchSchedules = async () => {
        try {
            setLoading(true);
            if (selectedClass) {
                const response = await fetch(`/api/Class/${selectedClass}/schedule`);
                if (response.ok) {
                    const data = await response.json();
                    setSchedules(data);
                }
            } else {
                // Fetch all schedules (you might need to implement this endpoint)
                setSchedules([]);
            }
        } catch (error) {
            console.error('Error fetching schedules:', error);
        } finally {
            setLoading(false);
        }
    };

    const filteredSchedules = schedules.filter(schedule => {
        const matchesSearch = schedule.className.toLowerCase().includes(searchTerm.toLowerCase()) ||
            schedule.subjectName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            schedule.teacherName.toLowerCase().includes(searchTerm.toLowerCase());
        const matchesDay = !filterDay || schedule.dayOfWeek === filterDay;
        return matchesSearch && matchesDay;
    });

    const groupSchedulesByDay = (schedules: ClassSchedule[]) => {
        const grouped: { [key: string]: ClassSchedule[] } = {};
        daysOfWeek.forEach(day => {
            grouped[day] = schedules.filter(s => s.dayOfWeek === day);
        });
        return grouped;
    };

    const formatTime = (time: string) => {
        return new Date(`2000-01-01T${time}`).toLocaleTimeString('en-US', {
            hour: '2-digit',
            minute: '2-digit',
            hour12: true
        });
    };

    if (loading) {
        return (
            <div className="flex items-center justify-center h-64">
                <div className="text-lg">Loading schedules...</div>
            </div>
        );
    }

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-3xl font-bold">Class Schedule</h1>
                <Button>
                    Add Schedule
                </Button>
            </div>

            {/* Filters */}
            <div className="flex gap-4">
                <Input
                    placeholder="Search schedules..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="max-w-sm"
                />
                <select
                    value={filterDay}
                    onChange={(e) => setFilterDay(e.target.value)}
                    className="px-3 py-2 border border-gray-300 rounded-md"
                >
                    <option value="">All Days</option>
                    {daysOfWeek.map(day => (
                        <option key={day} value={day}>{day}</option>
                    ))}
                </select>
                <select
                    value={selectedClass || ''}
                    onChange={(e) => setSelectedClass(e.target.value ? parseInt(e.target.value) : null)}
                    className="px-3 py-2 border border-gray-300 rounded-md"
                >
                    <option value="">All Classes</option>
                    {/* You would populate this with actual classes */}
                </select>
            </div>

            {/* Schedule Grid */}
            <div className="grid grid-cols-1 lg:grid-cols-7 gap-4">
                {daysOfWeek.map(day => {
                    const daySchedules = groupSchedulesByDay(filteredSchedules)[day] || [];
                    return (
                        <Card key={day} className="min-h-[400px]">
                            <CardHeader>
                                <CardTitle className="text-center text-lg">{day}</CardTitle>
                            </CardHeader>
                            <CardContent>
                                <div className="space-y-2">
                                    {daySchedules.length === 0 ? (
                                        <p className="text-gray-500 text-sm text-center py-4">No classes</p>
                                    ) : (
                                        daySchedules
                                            .sort((a, b) => a.startTime.localeCompare(b.startTime))
                                            .map(schedule => (
                                                <div
                                                    key={schedule.id}
                                                    className="p-3 bg-blue-50 rounded-lg border border-blue-200"
                                                >
                                                    <div className="text-sm font-medium text-blue-900">
                                                        {schedule.className}
                                                    </div>
                                                    <div className="text-xs text-blue-700">
                                                        {schedule.subjectName}
                                                    </div>
                                                    <div className="text-xs text-blue-600">
                                                        {schedule.teacherName}
                                                    </div>
                                                    <div className="text-xs text-blue-500 mt-1">
                                                        {formatTime(schedule.startTime)} - {formatTime(schedule.endTime)}
                                                    </div>
                                                    {schedule.room && (
                                                        <div className="text-xs text-blue-500">
                                                            Room: {schedule.room}
                                                        </div>
                                                    )}
                                                </div>
                                            ))
                                    )}
                                </div>
                            </CardContent>
                        </Card>
                    );
                })}
            </div>

            {/* List View */}
            <div className="mt-8">
                <h2 className="text-xl font-semibold mb-4">All Schedules</h2>
                <div className="space-y-2">
                    {filteredSchedules.map(schedule => (
                        <Card key={schedule.id} className="hover:shadow-md transition-shadow">
                            <CardContent className="p-4">
                                <div className="flex justify-between items-center">
                                    <div>
                                        <h3 className="font-medium">{schedule.className}</h3>
                                        <p className="text-sm text-gray-600">{schedule.subjectName}</p>
                                        <p className="text-sm text-gray-500">{schedule.teacherName}</p>
                                    </div>
                                    <div className="text-right">
                                        <p className="font-medium">{schedule.dayOfWeek}</p>
                                        <p className="text-sm text-gray-600">
                                            {formatTime(schedule.startTime)} - {formatTime(schedule.endTime)}
                                        </p>
                                        {schedule.room && (
                                            <p className="text-sm text-gray-500">Room: {schedule.room}</p>
                                        )}
                                    </div>
                                </div>
                            </CardContent>
                        </Card>
                    ))}
                </div>
            </div>

            {filteredSchedules.length === 0 && (
                <div className="text-center py-12">
                    <p className="text-gray-500 text-lg">No schedules found</p>
                </div>
            )}
        </div>
    );
};

export default ClassSchedule;
