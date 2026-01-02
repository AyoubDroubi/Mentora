# ? Frontend Quick Test Checklist

## ?? Quick Start

### 1. Start Servers
```sh
# Terminal 1: Backend
cd Server/src/Mentora.Api
dotnet run

# Terminal 2: Frontend  
cd Client
npm run dev
```

### 2. Open Browser
```
http://localhost:8000
```

---

## ? Test Flow (15 minutes)

### Step 1: Login (2 min)
```
? Go to /login
? Enter: saad@mentora.com / Saad@123
? Click "Login"
? Should redirect to /study-planner
? Check console: No errors
```

---

### Step 2: Study Planner Dashboard (3 min)
```
? Check header: "Welcome back, [Name]!"
? Verify stats load:
  - Total Study Time
  - Progress Percentage
  - Tasks (X/Y)
  - Events (X upcoming)

? Click each tool card:
  - Todo ? Opens /todo
  - Notes ? Opens /notes
  - Timer ? Opens /timer
  - Planner ? Opens /planner
  - Attendance ? Opens /attendance
  - Quiz ? Opens /quiz (TBD)

? Profile avatar clickable ? Opens /profile
```

---

### Step 3: Todo Page (2 min)
```
? Navigate to /todo
? Create 3 tasks:
  - "Complete Assignment"
  - "Read Chapter 5"
  - "????? ???????" (Arabic)

? Toggle first task (mark complete)
? Check summary updates:
  - Total: 3
  - Completed: 1
  - Pending: 2
  - Rate: 33%

? Filter by "Active" ? Shows 2 tasks
? Filter by "Completed" ? Shows 1 task
? Delete one task
? Check summary updates
```

---

### Step 4: Planner Page (2 min)
```
? Navigate to /planner
? Create 3 events:
  - Past event (yesterday)
    Title: "Past Meeting"
    Date: 2026-01-09
    Time: 10:00
  
  - Today event
    Title: "Current Meeting"
    Date: [today]
    Time: 15:00
  
  - Future event (tomorrow)
    Title: "Future Exam"
    Date: 2026-01-15
    Time: 10:00

? Mark past event as attended
? Verify colors:
  - Attended: Green ?
  - Upcoming: Blue
  - Missed: Red (past, not attended)

? Delete one event
```

---

### Step 5: Notes Page (2 min)
```
? Navigate to /notes
? Create note:
  Title: "Algorithm Notes"
  Content: "Big O: O(1), O(log n), O(n)"

? Create Arabic note:
  Title: "??????? ????"
  Content: "????? ?????? ???????"

? Edit first note:
  - Click edit button (pencil)
  - Change title
  - Click "Update Note"
  - Verify updated date changes

? Delete one note
```

---

### Step 6: Pomodoro Timer (2 min)
```
? Navigate to /timer
? Check total study time displayed
? Select "Work (25min)"
? Click "Start"
? Click "Pause" (check pause count increases)
? Click "Start" again
? Click "Reset"
? Verify focus score decreases with pauses

? Start new session
? Let it run for 10 seconds
? Click "Pause"
? Manually complete (for testing):
  - Wait for timer to reach 0:00
  - Or use browser dev tools to skip time
? Verify auto-save success message
? Check total study time updates
```

---

### Step 7: Attendance Page (2 min)
```
? Navigate to /attendance
? Check progress circle:
  - Shows 0-100%
  - Animated

? Verify progress breakdown:
  - Tasks Progress (50%)
  - Events Attendance (50%)
  - Progress bars showing

? Check weekly calendar:
  - 7 days displayed
  - Tasks count per day
  - Events count per day

? Change history dropdown:
  - Last 7 days
  - Last 30 days
  - Last 90 days

? Verify history summary:
  - Tasks: Total/Completed/Pending
  - Events: Total/Attended/Missed

? Check recent activities list:
  - Shows completed tasks
  - Shows events with status
```

---

## ? Cross-Page Verification

### Navigation (1 min)
```
? From Todo ? Click "Dashboard" ? Goes to /study-planner
? From Planner ? Click avatar ? Goes to /profile
? From Notes ? Click "Dashboard" ? Goes to /study-planner
? Back button works on all pages
```

### Stats Update (2 min)
```
? Go to /todo
? Complete 2 tasks
? Go to /study-planner
? Verify dashboard stats updated:
  - Tasks count changed
  - Progress percentage updated

? Go to /planner
? Mark event as attended
? Go to /attendance
? Verify progress updated
```

### Arabic Text (1 min)
```
? Create todo with Arabic title
? Create event with Arabic title
? Create note with Arabic content
? Verify all display correctly
? No garbled characters
```

---

## ?? Common Issues & Fixes

### Issue 1: Stats Not Loading
```
Problem: Dashboard shows 0 for all stats
Fix: Check browser console for errors
     Verify backend is running
     Check network tab for API calls
```

### Issue 2: Login Redirects to Login
```
Problem: After login, redirects back to login
Fix: Check localStorage has tokens
     Verify JWT is valid
     Check console for auth errors
```

### Issue 3: Progress Not Updating
```
Problem: Attendance progress stays at 0%
Fix: Create tasks and complete some
     Create past events and mark attended
     Refresh /attendance page
```

### Issue 4: Arabic Text Garbled
```
Problem: Arabic shows as ????
Fix: Check Content-Type header has charset=utf-8
     Verify database collation is utf8
     Check browser encoding
```

---

## ?? Expected Results

### After All Tests

```
Dashboard Stats:
? Total Study Time: ~25m (from timer test)
? Progress: ~40-60% (depends on tasks/events)
? Tasks: 2-3 remaining
? Events: 2-3 upcoming
? Notes: 1-2 saved

Todo Summary:
? Total: 2-3
? Completed: 1-2
? Pending: 1-2
? Rate: 33-66%

Planner:
? Upcoming: 1-2 events
? All Events: 2-3 total
? 1 attended (green)
? 1 upcoming (blue)

Attendance:
? Progress: 40-60%
? Tasks: 50% contribution
? Events: 50% contribution
? Weekly calendar populated
```

---

## ? Sign-Off

**Tested By:** _______________  
**Date:** _______________  
**Time Taken:** _____ minutes  

**Overall Status:**
- [ ] ? PASS - All features working
- [ ] ?? PASS with minor issues
- [ ] ? FAIL - Critical issues found

**Notes:**
```
_______________________________
_______________________________
_______________________________
```

---

## ?? Quick Commands

```sh
# Start backend
cd Server/src/Mentora.Api && dotnet run

# Start frontend
cd Client && npm run dev

# Open browser
start http://localhost:8000

# Login
Email: saad@mentora.com
Password: Saad@123
```

---

## ?? Bug Report Template

```
Page: [Todo/Planner/Notes/Timer/Attendance]
Issue: [Description]

Steps to reproduce:
1. 
2. 
3. 

Expected: 
Actual: 

Screenshot: [if applicable]
Console errors: [copy from browser console]
```

---

**Last Updated:** 2026-01-10  
**Version:** 1.0.0  

---

## ?? Ready to Test!

**Time needed:** 15 minutes  
**All pages:** ? Ready  
**Test flow:** ? Defined  

**Start testing now!** ??
