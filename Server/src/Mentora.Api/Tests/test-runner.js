#!/usr/bin/env node

/**
 * Mentora API - Automated Test Runner
 * Tests all endpoints after DDD refactoring
 */

const https = require('https');

const BASE_URL = 'https://localhost:7000/api';
let ACCESS_TOKEN = '';
let REFRESH_TOKEN = '';

// Test results
const results = {
  passed: 0,
  failed: 0,
  tests: []
};

// ANSI colors
const colors = {
  reset: '\x1b[0m',
  green: '\x1b[32m',
  red: '\x1b[31m',
  yellow: '\x1b[33m',
  blue: '\x1b[34m',
  cyan: '\x1b[36m'
};

// Helper: Make HTTP request
async function makeRequest(method, path, body = null, auth = false) {
  return new Promise((resolve, reject) => {
    const url = new URL(path, BASE_URL);
    
    const options = {
      method,
      headers: {
        'Content-Type': 'application/json; charset=utf-8',
        'Accept': 'application/json'
      },
      rejectUnauthorized: false // Accept self-signed certificates
    };

    if (auth && ACCESS_TOKEN) {
      options.headers['Authorization'] = `Bearer ${ACCESS_TOKEN}`;
    }

    const req = https.request(url, options, (res) => {
      let data = '';
      
      res.on('data', (chunk) => {
        data += chunk;
      });

      res.on('end', () => {
        try {
          const jsonData = data ? JSON.parse(data) : {};
          resolve({
            status: res.statusCode,
            headers: res.headers,
            body: jsonData
          });
        } catch (e) {
          resolve({
            status: res.statusCode,
            headers: res.headers,
            body: data
          });
        }
      });
    });

    req.on('error', reject);

    if (body) {
      req.write(JSON.stringify(body));
    }

    req.end();
  });
}

// Helper: Log test result
function logTest(name, passed, message = '') {
  const icon = passed ? '?' : '?';
  const color = passed ? colors.green : colors.red;
  
  console.log(`${color}${icon} ${name}${colors.reset}`);
  
  if (message) {
    console.log(`   ${colors.yellow}${message}${colors.reset}`);
  }

  results.tests.push({ name, passed, message });
  
  if (passed) {
    results.passed++;
  } else {
    results.failed++;
  }
}

// Helper: Test assertion
function assert(condition, message) {
  if (!condition) {
    throw new Error(message);
  }
}

// Test: Authentication Module
async function testAuthentication() {
  console.log(`\n${colors.cyan}=== Testing Authentication Module ===${colors.reset}\n`);

  try {
    // Test 1: Login
    const loginRes = await makeRequest('POST', '/auth/login', {
      email: 'saad@mentora.com',
      password: 'Saad@123',
      deviceInfo: 'Test Runner'
    });

    assert(loginRes.status === 200, 'Login should return 200');
    assert(loginRes.body.isSuccess, 'Login should be successful');
    assert(loginRes.body.accessToken, 'Should return access token');
    
    ACCESS_TOKEN = loginRes.body.accessToken;
    REFRESH_TOKEN = loginRes.body.refreshToken;
    
    logTest('Authentication: Login', true, 'Access token received');
  } catch (error) {
    logTest('Authentication: Login', false, error.message);
  }

  try {
    // Test 2: Get Current User
    const meRes = await makeRequest('GET', '/auth/me', null, true);
    
    assert(meRes.status === 200, 'Should return 200');
    assert(meRes.body.userId, 'Should return user ID');
    assert(meRes.body.email, 'Should return email');
    
    logTest('Authentication: Get Me', true, `User: ${meRes.body.email}`);
  } catch (error) {
    logTest('Authentication: Get Me', false, error.message);
  }
}

// Test: User Profile Module
async function testUserProfile() {
  console.log(`\n${colors.cyan}=== Testing User Profile Module ===${colors.reset}\n`);

  try {
    // Test 1: Check Profile Exists
    const existsRes = await makeRequest('GET', '/userprofile/exists', null, true);
    
    assert(existsRes.status === 200, 'Should return 200');
    
    logTest('Profile: Check Exists', true, `Exists: ${existsRes.body.exists}`);
  } catch (error) {
    logTest('Profile: Check Exists', false, error.message);
  }

  try {
    // Test 2: Get Profile
    const profileRes = await makeRequest('GET', '/userprofile', null, true);
    
    if (profileRes.status === 404) {
      logTest('Profile: Get Profile', true, 'No profile yet (expected)');
    } else {
      assert(profileRes.status === 200, 'Should return 200');
      assert(profileRes.body.university, 'Should have university');
      logTest('Profile: Get Profile', true, `University: ${profileRes.body.university}`);
    }
  } catch (error) {
    logTest('Profile: Get Profile', false, error.message);
  }

  try {
    // Test 3: Update Profile (Arabic)
    const updateRes = await makeRequest('PUT', '/userprofile', {
      bio: '???? ???? ?????',
      location: '????? ??????',
      phoneNumber: '+962791234567',
      university: '??????? ????????',
      major: '???? ???????',
      expectedGraduationYear: 2026,
      currentLevel: 'Junior',
      timezone: 'Asia/Amman'
    }, true);
    
    assert(updateRes.status === 200, 'Should return 200');
    assert(updateRes.body.university === '??????? ????????', 'Should save Arabic text');
    
    logTest('Profile: Update (Arabic)', true, 'Arabic text saved correctly');
  } catch (error) {
    logTest('Profile: Update (Arabic)', false, error.message);
  }
}

// Test: Todo Module
async function testTodo() {
  console.log(`\n${colors.cyan}=== Testing Todo Module ===${colors.reset}\n`);

  let todoId = null;

  try {
    // Test 1: Create Todo
    const createRes = await makeRequest('POST', '/todo', {
      title: 'Test Todo - Complete Assignment'
    }, true);
    
    assert(createRes.status === 200, 'Should return 200');
    assert(createRes.body.success, 'Should be successful');
    assert(createRes.body.data.id, 'Should return todo ID');
    
    todoId = createRes.body.data.id;
    
    logTest('Todo: Create', true, `Created: ${createRes.body.data.title}`);
  } catch (error) {
    logTest('Todo: Create', false, error.message);
  }

  try {
    // Test 2: Get All Todos
    const listRes = await makeRequest('GET', '/todo', null, true);
    
    assert(listRes.status === 200, 'Should return 200');
    assert(listRes.body.success, 'Should be successful');
    assert(Array.isArray(listRes.body.data), 'Should return array');
    
    logTest('Todo: Get All', true, `Found ${listRes.body.data.length} todos`);
  } catch (error) {
    logTest('Todo: Get All', false, error.message);
  }

  try {
    // Test 3: Toggle Todo
    if (todoId) {
      const toggleRes = await makeRequest('PATCH', `/todo/${todoId}`, null, true);
      
      assert(toggleRes.status === 200, 'Should return 200');
      assert(toggleRes.body.success, 'Should be successful');
      
      logTest('Todo: Toggle', true, `Completed: ${toggleRes.body.data.isCompleted}`);
    }
  } catch (error) {
    logTest('Todo: Toggle', false, error.message);
  }

  try {
    // Test 4: Get Summary
    const summaryRes = await makeRequest('GET', '/todo/summary', null, true);
    
    assert(summaryRes.status === 200, 'Should return 200');
    assert(summaryRes.body.success, 'Should be successful');
    assert(typeof summaryRes.body.data.totalTasks === 'number', 'Should have totalTasks');
    
    logTest('Todo: Get Summary', true, `Total: ${summaryRes.body.data.totalTasks}, Completed: ${summaryRes.body.data.completedTasks}`);
  } catch (error) {
    logTest('Todo: Get Summary', false, error.message);
  }

  try {
    // Test 5: Delete Todo
    if (todoId) {
      const deleteRes = await makeRequest('DELETE', `/todo/${todoId}`, null, true);
      
      assert(deleteRes.status === 200, 'Should return 200');
      assert(deleteRes.body.success, 'Should be successful');
      
      logTest('Todo: Delete', true, 'Todo deleted successfully');
    }
  } catch (error) {
    logTest('Todo: Delete', false, error.message);
  }
}

// Test: Planner Module
async function testPlanner() {
  console.log(`\n${colors.cyan}=== Testing Planner Module ===${colors.reset}\n`);

  let eventId = null;

  try {
    // Test 1: Create Event
    const createRes = await makeRequest('POST', '/planner/events', {
      title: 'Test Event - Midterm Exam',
      eventDateTime: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString()
    }, true);
    
    assert(createRes.status === 200, 'Should return 200');
    assert(createRes.body.success, 'Should be successful');
    assert(createRes.body.data.id, 'Should return event ID');
    
    eventId = createRes.body.data.id;
    
    logTest('Planner: Create Event', true, `Created: ${createRes.body.data.title}`);
  } catch (error) {
    logTest('Planner: Create Event', false, error.message);
  }

  try {
    // Test 2: Get Upcoming Events
    const upcomingRes = await makeRequest('GET', '/planner/events/upcoming', null, true);
    
    assert(upcomingRes.status === 200, 'Should return 200');
    assert(upcomingRes.body.success, 'Should be successful');
    
    logTest('Planner: Get Upcoming', true, `Found ${upcomingRes.body.data.length} upcoming events`);
  } catch (error) {
    logTest('Planner: Get Upcoming', false, error.message);
  }

  try {
    // Test 3: Delete Event
    if (eventId) {
      const deleteRes = await makeRequest('DELETE', `/planner/events/${eventId}`, null, true);
      
      assert(deleteRes.status === 200, 'Should return 200');
      assert(deleteRes.body.success, 'Should be successful');
      
      logTest('Planner: Delete Event', true, 'Event deleted successfully');
    }
  } catch (error) {
    logTest('Planner: Delete Event', false, error.message);
  }
}

// Test: Notes Module
async function testNotes() {
  console.log(`\n${colors.cyan}=== Testing Notes Module ===${colors.reset}\n`);

  let noteId = null;

  try {
    // Test 1: Create Note
    const createRes = await makeRequest('POST', '/notes', {
      title: 'Test Note - Design Patterns',
      content: 'Singleton, Factory, Observer, Strategy'
    }, true);
    
    assert(createRes.status === 200, 'Should return 200');
    assert(createRes.body.success, 'Should be successful');
    assert(createRes.body.data.id, 'Should return note ID');
    
    noteId = createRes.body.data.id;
    
    logTest('Notes: Create', true, `Created: ${createRes.body.data.title}`);
  } catch (error) {
    logTest('Notes: Create', false, error.message);
  }

  try {
    // Test 2: Get All Notes
    const listRes = await makeRequest('GET', '/notes', null, true);
    
    assert(listRes.status === 200, 'Should return 200');
    assert(listRes.body.success, 'Should be successful');
    
    logTest('Notes: Get All', true, `Found ${listRes.body.data.length} notes`);
  } catch (error) {
    logTest('Notes: Get All', false, error.message);
  }

  try {
    // Test 3: Delete Note
    if (noteId) {
      const deleteRes = await makeRequest('DELETE', `/notes/${noteId}`, null, true);
      
      assert(deleteRes.status === 200, 'Should return 200');
      assert(deleteRes.body.success, 'Should be successful');
      
      logTest('Notes: Delete', true, 'Note deleted successfully');
    }
  } catch (error) {
    logTest('Notes: Delete', false, error.message);
  }
}

// Test: Study Sessions Module
async function testStudySessions() {
  console.log(`\n${colors.cyan}=== Testing Study Sessions Module ===${colors.reset}\n`);

  try {
    // Test 1: Save Session
    const saveRes = await makeRequest('POST', '/study-sessions', {
      durationMinutes: 45,
      pauseCount: 2,
      focusScore: 85
    }, true);
    
    assert(saveRes.status === 200, 'Should return 200');
    assert(saveRes.body.success, 'Should be successful');
    
    logTest('Study Sessions: Save', true, `Duration: ${saveRes.body.data.durationMinutes}min`);
  } catch (error) {
    logTest('Study Sessions: Save', false, error.message);
  }

  try {
    // Test 2: Get Summary
    const summaryRes = await makeRequest('GET', '/study-sessions/summary', null, true);
    
    assert(summaryRes.status === 200, 'Should return 200');
    assert(summaryRes.body.success, 'Should be successful');
    
    logTest('Study Sessions: Get Summary', true, `Total: ${summaryRes.body.data.formatted}`);
  } catch (error) {
    logTest('Study Sessions: Get Summary', false, error.message);
  }
}

// Test: Attendance Module
async function testAttendance() {
  console.log(`\n${colors.cyan}=== Testing Attendance Module ===${colors.reset}\n`);

  try {
    // Test 1: Get Summary
    const summaryRes = await makeRequest('GET', '/study-planner/attendance/summary', null, true);
    
    assert(summaryRes.status === 200, 'Should return 200');
    assert(summaryRes.body.success, 'Should be successful');
    assert(typeof summaryRes.body.data.progressPercentage === 'number', 'Should have progress percentage');
    
    logTest('Attendance: Get Summary', true, `Progress: ${summaryRes.body.data.progressPercentage}%`);
  } catch (error) {
    logTest('Attendance: Get Summary', false, error.message);
  }

  try {
    // Test 2: Get Weekly Progress
    const weeklyRes = await makeRequest('GET', '/study-planner/attendance/weekly', null, true);
    
    assert(weeklyRes.status === 200, 'Should return 200');
    assert(weeklyRes.body.success, 'Should be successful');
    
    logTest('Attendance: Get Weekly', true, `Week: ${weeklyRes.body.data.weekStart} to ${weeklyRes.body.data.weekEnd}`);
  } catch (error) {
    logTest('Attendance: Get Weekly', false, error.message);
  }
}

// Print summary
function printSummary() {
  console.log(`\n${colors.blue}${'='.repeat(50)}${colors.reset}`);
  console.log(`${colors.cyan}Test Summary${colors.reset}`);
  console.log(`${colors.blue}${'='.repeat(50)}${colors.reset}\n`);
  
  console.log(`${colors.green}? Passed: ${results.passed}${colors.reset}`);
  console.log(`${colors.red}? Failed: ${results.failed}${colors.reset}`);
  console.log(`Total: ${results.passed + results.failed}`);
  
  const percentage = Math.round((results.passed / (results.passed + results.failed)) * 100);
  console.log(`\nSuccess Rate: ${percentage}%`);
  
  if (results.failed > 0) {
    console.log(`\n${colors.red}Failed Tests:${colors.reset}`);
    results.tests
      .filter(t => !t.passed)
      .forEach(t => {
        console.log(`  ${colors.red}? ${t.name}${colors.reset}`);
        console.log(`     ${t.message}`);
      });
  }
  
  console.log(`\n${colors.blue}${'='.repeat(50)}${colors.reset}\n`);
}

// Main test runner
async function runTests() {
  console.log(`${colors.cyan}`);
  console.log('??????????????????????????????????????????');
  console.log('?   Mentora API - DDD Test Runner       ?');
  console.log('?   Testing New Architecture            ?');
  console.log('??????????????????????????????????????????');
  console.log(`${colors.reset}\n`);

  try {
    await testAuthentication();
    await testUserProfile();
    await testTodo();
    await testPlanner();
    await testNotes();
    await testStudySessions();
    await testAttendance();
  } catch (error) {
    console.error(`${colors.red}Fatal Error: ${error.message}${colors.reset}`);
  }

  printSummary();
  
  process.exit(results.failed > 0 ? 1 : 0);
}

// Run tests
runTests();
