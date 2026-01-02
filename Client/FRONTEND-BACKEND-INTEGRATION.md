# ?? Frontend-Backend Integration Guide

## ?? Overview

This guide shows how to integrate the React frontend with the new **DDD (Domain-Driven Design)** backend architecture.

---

## ??? Service Architecture

### New Services Created

All services are located in `Client/src/services/`:

| Service | File | Purpose |
|---------|------|---------|
| **Todo** | `todoService.js` | Manage todos (CRUD + summary) |
| **Planner** | `plannerService.js` | Manage events (CRUD + attendance) |
| **Notes** | `notesService.js` | Manage notes (CRUD) |
| **Study Quiz** | `studyQuizService.js` | Quiz questions & submission |
| **Study Sessions** | `studySessionsService.js` | Track study time (Pomodoro) |
| **Attendance** | `attendanceService.js` | Progress tracking |
| **User Profile** | `userProfileService.js` | User profile management |
| **API** | `api.js` | Base Axios instance |

---

## ?? Quick Start

### 1. Import Services

```javascript
// Import individual service
import todoService from '../services/todoService';

// Or import from index
import { todoService, plannerService, notesService } from '../services';
```

---

### 2. Use in Components

#### Example: Todo Component

```javascript
import React, { useState, useEffect } from 'react';
import { todoService } from '../services';

const TodoList = () => {
  const [todos, setTodos] = useState([]);
  const [loading, setLoading] = useState(false);

  // Fetch todos
  useEffect(() => {
    const fetchTodos = async () => {
      try {
        setLoading(true);
        const response = await todoService.getAllTodos();
        if (response.success) {
          setTodos(response.data);
        }
      } catch (error) {
        console.error('Error:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchTodos();
  }, []);

  // Create todo
  const handleCreate = async (title) => {
    try {
      const response = await todoService.createTodo(title);
      if (response.success) {
        setTodos([response.data, ...todos]);
      }
    } catch (error) {
      console.error('Error:', error);
    }
  };

  // Toggle completion
  const handleToggle = async (id) => {
    try {
      const response = await todoService.toggleTodo(id);
      if (response.success) {
        setTodos(todos.map(t => t.id === id ? response.data : t));
      }
    } catch (error) {
      console.error('Error:', error);
    }
  };

  return (
    <div>
      {/* Your UI here */}
    </div>
  );
};
```

---

## ?? Service APIs

### 1?? Todo Service

```javascript
import { todoService } from '../services';

// Get all todos (with optional filter)
const todos = await todoService.getAllTodos('all'); // 'all', 'active', 'completed'

// Get summary
const summary = await todoService.getSummary();
// Returns: { totalTasks, completedTasks, pendingTasks, completionRate }

// Create todo
const newTodo = await todoService.createTodo('Complete assignment');

// Toggle completion
const updated = await todoService.toggleTodo(todoId);

// Delete todo
await todoService.deleteTodo(todoId);
```

---

### 2?? Planner Service

```javascript
import { plannerService } from '../services';

// Get all events
const events = await plannerService.getAllEvents();

// Get events by date
const eventsOnDate = await plannerService.getAllEvents('2026-01-15');

// Get upcoming events
const upcoming = await plannerService.getUpcomingEvents();

// Create event
const newEvent = await plannerService.createEvent({
  title: 'Midterm Exam',
  eventDateTime: '2026-01-20T10:00:00Z'
});

// Mark as attended
await plannerService.markAttended(eventId);

// Delete event
await plannerService.deleteEvent(eventId);
```

---

### 3?? Notes Service

```javascript
import { notesService } from '../services';

// Get all notes
const notes = await notesService.getAllNotes();

// Get specific note
const note = await notesService.getNoteById(noteId);

// Create note
const newNote = await notesService.createNote({
  title: 'Algorithm Notes',
  content: 'Big O notation: O(1), O(log n), O(n)...'
});

// Update note (full or partial)
await notesService.updateNote(noteId, {
  title: 'Updated Title',
  content: 'Updated content'
});

// Delete note
await notesService.deleteNote(noteId);
```

---

### 4?? Study Quiz Service

```javascript
import { studyQuizService } from '../services';

// Get questions
const questions = await studyQuizService.getQuestions();

// Submit answers
const result = await studyQuizService.submitQuiz({
  '1': 'Answer to question 1',
  '2': 'Answer to question 2',
  // ... more answers
});
// Returns: { attemptId, createdAt, studyPlan }

// Get latest attempt
const latest = await studyQuizService.getLatestAttempt();
```

---

### 5?? Study Sessions Service

```javascript
import { studySessionsService } from '../services';

// Get all sessions
const sessions = await studySessionsService.getAllSessions(50); // limit: 50

// Get summary
const summary = await studySessionsService.getSummary();
// Returns: { totalMinutes, hours, minutes, formatted }

// Get by date range
const rangeSessions = await studySessionsService.getSessionsByRange(
  '2026-01-01',
  '2026-01-31'
);

// Save session
const newSession = await studySessionsService.saveSession({
  durationMinutes: 45,
  startTime: new Date().toISOString(), // optional
  pauseCount: 2, // optional
  focusScore: 85 // optional (0-100)
});

// Delete session
await studySessionsService.deleteSession(sessionId);
```

---

### 6?? Attendance Service

```javascript
import { attendanceService } from '../services';

// Get summary (progress = 50% tasks + 50% events)
const summary = await attendanceService.getSummary();
// Returns: {
//   totalTasks, completedTasks, pendingTasks, taskCompletionRate,
//   totalPastEvents, attendedEvents, upcomingEvents, attendanceRate,
//   progressPercentage, breakdown: { tasksContribution, eventsContribution }
// }

// Get history
const history = await attendanceService.getHistory(30); // last 30 days
// Returns: { period, events, tasks, summary }

// Get weekly progress
const weekly = await attendanceService.getWeeklyProgress();
// Returns: { weekStart, weekEnd, days: [...] }
```

---

## ?? Authentication

All services automatically handle authentication using the interceptor in `api.js`:

```javascript
// Token is automatically added to all requests
// No need to manually add Authorization header

// The interceptor also handles token refresh automatically
// If access token expires, it will:
// 1. Use refresh token to get new access token
// 2. Retry the failed request with new token
// 3. If refresh fails, redirect to login
```

---

## ?? Error Handling

### Standard Pattern

```javascript
try {
  const response = await todoService.createTodo(title);
  
  if (response.success) {
    // Handle success
    console.log('Todo created:', response.data);
  }
} catch (error) {
  // Handle error
  const errorMessage = error.response?.data?.message || 'Operation failed';
  console.error('Error:', errorMessage);
  
  // Show error to user
  setError(errorMessage);
}
```

### Common Error Responses

| Status | Meaning | Example |
|--------|---------|---------|
| 400 | Bad Request | Missing required field, invalid data |
| 401 | Unauthorized | Token expired or missing |
| 403 | Forbidden | Not authorized to access resource |
| 404 | Not Found | Resource doesn't exist |
| 500 | Server Error | Internal server error |

---

## ?? Arabic Text Support

All services support Arabic text out of the box:

```javascript
// Create todo with Arabic title
await todoService.createTodo('????? ????? ???????');

// Create event with Arabic title
await plannerService.createEvent({
  title: '?????? ????? ????????',
  eventDateTime: '2026-01-20T10:00:00Z'
});

// Create note with mixed Arabic/English
await notesService.createNote({
  title: 'Algorithm Analysis - ????? ???????????',
  content: 'Big O Notation: O(1), O(log n), O(n)...'
});
```

The `api.js` interceptor automatically sets:
```javascript
headers: {
  'Content-Type': 'application/json; charset=utf-8',
  'Accept-Language': 'en,ar'
}
```

---

## ?? Response Format

All API responses follow this format:

### Success Response

```json
{
  "success": true,
  "message": "Operation successful",
  "data": {
    "id": "guid",
    "title": "Todo title",
    "isCompleted": false,
    "createdAt": "2026-01-10T12:00:00Z"
  }
}
```

### Error Response

```json
{
  "success": false,
  "message": "Error message",
  "errors": {
    "field": ["Error detail 1", "Error detail 2"]
  }
}
```

---

## ?? Real-time Updates

### Using React State

```javascript
const [todos, setTodos] = useState([]);

// Create and update state
const handleCreate = async (title) => {
  const response = await todoService.createTodo(title);
  if (response.success) {
    setTodos([response.data, ...todos]); // Add to beginning
  }
};

// Update and modify state
const handleToggle = async (id) => {
  const response = await todoService.toggleTodo(id);
  if (response.success) {
    setTodos(todos.map(t => t.id === id ? response.data : t));
  }
};

// Delete and remove from state
const handleDelete = async (id) => {
  const response = await todoService.deleteTodo(id);
  if (response.success) {
    setTodos(todos.filter(t => t.id !== id));
  }
};
```

---

## ?? Best Practices

### 1. Loading States

```javascript
const [loading, setLoading] = useState(false);

const fetchData = async () => {
  setLoading(true);
  try {
    const response = await todoService.getAllTodos();
    // Handle response
  } catch (error) {
    // Handle error
  } finally {
    setLoading(false); // Always reset loading
  }
};
```

### 2. Error States

```javascript
const [error, setError] = useState(null);

const createTodo = async (title) => {
  setError(null); // Clear previous errors
  try {
    const response = await todoService.createTodo(title);
    // Handle success
  } catch (err) {
    setError(err.response?.data?.message || 'Operation failed');
  }
};
```

### 3. Optimistic Updates

```javascript
// Update UI immediately, rollback if fails
const handleToggle = async (id) => {
  // Save previous state
  const previousTodos = [...todos];
  
  // Update UI optimistically
  setTodos(todos.map(t => 
    t.id === id ? { ...t, isCompleted: !t.isCompleted } : t
  ));
  
  try {
    await todoService.toggleTodo(id);
  } catch (error) {
    // Rollback on error
    setTodos(previousTodos);
    setError('Failed to update todo');
  }
};
```

---

## ?? Testing Integration

### Test with HTTP File

```http
### 1. Login
POST https://localhost:7000/api/auth/login
Content-Type: application/json

{
  "email": "test@mentora.com",
  "password": "Test@123",
  "deviceInfo": "Chrome"
}

### Save token
@accessToken = {{login.response.body.accessToken}}

### 2. Test Todo API
GET https://localhost:7000/api/todo
Authorization: Bearer {{accessToken}}
```

---

## ?? Complete Example: Study Planner Dashboard

```javascript
import React, { useState, useEffect } from 'react';
import {
  todoService,
  plannerService,
  studySessionsService,
  attendanceService
} from '../services';

const StudyPlannerDashboard = () => {
  const [todos, setTodos] = useState([]);
  const [upcomingEvents, setUpcomingEvents] = useState([]);
  const [summary, setSummary] = useState(null);
  const [attendance, setAttendance] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchDashboardData();
  }, []);

  const fetchDashboardData = async () => {
    setLoading(true);
    try {
      // Fetch all data in parallel
      const [
        todosRes,
        eventsRes,
        summaryRes,
        attendanceRes
      ] = await Promise.all([
        todoService.getAllTodos('active'),
        plannerService.getUpcomingEvents(),
        studySessionsService.getSummary(),
        attendanceService.getSummary()
      ]);

      if (todosRes.success) setTodos(todosRes.data);
      if (eventsRes.success) setUpcomingEvents(eventsRes.data);
      if (summaryRes.success) setSummary(summaryRes.data);
      if (attendanceRes.success) setAttendance(attendanceRes.data);
    } catch (error) {
      console.error('Error fetching dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Loading...</div>;

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6">Study Planner Dashboard</h1>

      {/* Progress Summary */}
      {attendance && (
        <div className="bg-blue-100 p-6 rounded-lg mb-6">
          <h2 className="text-xl font-semibold mb-4">Progress Overview</h2>
          <div className="text-4xl font-bold text-blue-600">
            {attendance.progressPercentage}%
          </div>
          <p className="text-gray-600">
            Overall Progress (50% Tasks + 50% Events)
          </p>
        </div>
      )}

      {/* Study Time Summary */}
      {summary && (
        <div className="bg-green-100 p-6 rounded-lg mb-6">
          <h2 className="text-xl font-semibold mb-2">Total Study Time</h2>
          <div className="text-3xl font-bold text-green-600">
            {summary.formatted}
          </div>
        </div>
      )}

      {/* Pending Todos */}
      <div className="mb-6">
        <h2 className="text-xl font-semibold mb-3">Pending Tasks</h2>
        {todos.length === 0 ? (
          <p className="text-gray-500">No pending tasks</p>
        ) : (
          <ul className="space-y-2">
            {todos.map(todo => (
              <li key={todo.id} className="p-3 bg-white rounded shadow">
                {todo.title}
              </li>
            ))}
          </ul>
        )}
      </div>

      {/* Upcoming Events */}
      <div>
        <h2 className="text-xl font-semibold mb-3">Upcoming Events</h2>
        {upcomingEvents.length === 0 ? (
          <p className="text-gray-500">No upcoming events</p>
        ) : (
          <ul className="space-y-2">
            {upcomingEvents.map(event => (
              <li key={event.id} className="p-3 bg-white rounded shadow">
                <p className="font-semibold">{event.title}</p>
                <p className="text-sm text-gray-600">
                  {new Date(event.eventDateTime).toLocaleString()}
                </p>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default StudyPlannerDashboard;
```

---

## ?? Next Steps

1. ? Services created and ready
2. ? Authentication handled automatically
3. ? Arabic text supported
4. ? Error handling pattern established
5. ? Examples provided

### Start Building

```bash
# Install dependencies (if needed)
cd Client
npm install

# Start development server
npm run dev

# Your app is ready at:
# http://localhost:8000
```

---

## ?? Additional Resources

- [Complete API Tests](../Server/src/Mentora.Api/Tests/complete-api-tests.http)
- [DDD Testing Guide](../DDD-TESTING-GUIDE.md)
- [API Documentation](../Server/src/Mentora.Documentation/Docs/API-QUICK-REFERENCE.md)

---

**? Frontend is now fully integrated with the new DDD Backend!** ??
