# ?? Services Directory

## ?? Overview

This directory contains all API service modules for communicating with the Mentora backend (DDD architecture).

---

## ?? Files

| File | Purpose | Main Functions |
|------|---------|----------------|
| **api.js** | Base Axios instance | HTTP client with auth interceptors |
| **todoService.js** | Todo management | CRUD + summary |
| **plannerService.js** | Event/Calendar | CRUD + attendance |
| **notesService.js** | Notes management | CRUD |
| **studyQuizService.js** | Study quiz | Questions + submission |
| **studySessionsService.js** | Study tracking | Session CRUD + summary |
| **attendanceService.js** | Progress tracking | Summary + history + weekly |
| **userProfileService.js** | User profile | Profile CRUD |
| **index.js** | Exports hub | Centralized exports |

---

## ?? Quick Start

### Import Services

```javascript
// Import all services
import {
  todoService,
  plannerService,
  notesService,
  studyQuizService,
  studySessionsService,
  attendanceService,
  userProfileService
} from './services';

// Or import individual service
import todoService from './services/todoService';
```

---

## ?? Usage Examples

### Todo Service

```javascript
// Get all todos
const response = await todoService.getAllTodos();
if (response.success) {
  console.log(response.data); // Array of todos
}

// Create todo
await todoService.createTodo('Complete assignment');

// Toggle completion
await todoService.toggleTodo(todoId);

// Delete todo
await todoService.deleteTodo(todoId);

// Get summary
const summary = await todoService.getSummary();
// { totalTasks, completedTasks, pendingTasks, completionRate }
```

### Planner Service

```javascript
// Get upcoming events
const response = await plannerService.getUpcomingEvents();

// Create event
await plannerService.createEvent({
  title: 'Exam',
  eventDateTime: '2026-01-20T10:00:00Z'
});

// Mark attended
await plannerService.markAttended(eventId);
```

### Notes Service

```javascript
// Get all notes
const response = await notesService.getAllNotes();

// Create note
await notesService.createNote({
  title: 'Algorithm Notes',
  content: 'Big O: O(1), O(log n), O(n)...'
});

// Update note
await notesService.updateNote(noteId, {
  title: 'Updated Title'
});
```

### Study Sessions Service

```javascript
// Save session
await studySessionsService.saveSession({
  durationMinutes: 45,
  pauseCount: 2,
  focusScore: 85
});

// Get summary
const summary = await studySessionsService.getSummary();
// { totalMinutes, hours, minutes, formatted: "10h 30m" }
```

### Attendance Service

```javascript
// Get progress summary
const summary = await attendanceService.getSummary();
// { progressPercentage, totalTasks, completedTasks, ... }

// Get weekly progress
const weekly = await attendanceService.getWeeklyProgress();
```

---

## ?? Authentication

All services automatically handle authentication:

- ? Access token added to all requests
- ? Token refresh handled automatically
- ? Redirect to login on auth failure

No need to manually add `Authorization` header!

---

## ?? Error Handling

```javascript
try {
  const response = await todoService.createTodo(title);
  
  if (response.success) {
    // Handle success
  }
} catch (error) {
  // Handle error
  const message = error.response?.data?.message || 'Operation failed';
  console.error(message);
}
```

---

## ?? Arabic Text

All services support Arabic text:

```javascript
await todoService.createTodo('????? ???????');
await plannerService.createEvent({
  title: '?????? ???????',
  eventDateTime: '2026-01-20T10:00:00Z'
});
```

---

## ?? Full Documentation

See [FRONTEND-BACKEND-INTEGRATION.md](../FRONTEND-BACKEND-INTEGRATION.md) for:
- Detailed API documentation
- Complete examples
- Best practices
- Error handling patterns

---

## ?? Quick Reference

### Common Patterns

**Fetch Data on Mount**
```javascript
useEffect(() => {
  const fetchData = async () => {
    const response = await todoService.getAllTodos();
    if (response.success) {
      setTodos(response.data);
    }
  };
  fetchData();
}, []);
```

**Create with State Update**
```javascript
const handleCreate = async (title) => {
  const response = await todoService.createTodo(title);
  if (response.success) {
    setTodos([response.data, ...todos]);
  }
};
```

**Update with State Modification**
```javascript
const handleToggle = async (id) => {
  const response = await todoService.toggleTodo(id);
  if (response.success) {
    setTodos(todos.map(t => t.id === id ? response.data : t));
  }
};
```

**Delete with State Removal**
```javascript
const handleDelete = async (id) => {
  const response = await todoService.deleteTodo(id);
  if (response.success) {
    setTodos(todos.filter(t => t.id !== id));
  }
};
```

---

## ? Ready to Use!

All services are configured and ready to use with the new DDD backend architecture.

**Start building:** Import the service you need and start making API calls! ??
