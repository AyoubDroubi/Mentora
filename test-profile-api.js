/**
 * Automated Test Script for User Profile API
 * Run with: node test-profile-api.js
 */

const https = require('https');

// Configuration
const BASE_URL = 'https://localhost:7000/api';
const TEST_EMAIL = `test.${Date.now()}@mentora.com`;
const TEST_PASSWORD = 'Test@123456';

// Test results
const results = {
  total: 0,
  passed: 0,
  failed: 0,
  errors: []
};

// Color codes for console
const colors = {
  reset: '\x1b[0m',
  green: '\x1b[32m',
  red: '\x1b[31m',
  yellow: '\x1b[33m',
  blue: '\x1b[34m'
};

// HTTP Client (allows self-signed certificates)
const agent = new https.Agent({ rejectUnauthorized: false });

function makeRequest(method, path, body = null, token = null) {
  return new Promise((resolve, reject) => {
    const url = new URL(`${BASE_URL}${path}`);
    
    const options = {
      method,
      hostname: url.hostname,
      port: url.port,
      path: url.pathname + url.search,
      headers: {
        'Content-Type': 'application/json; charset=utf-8',
        'Accept': 'application/json'
      },
      agent
    };

    if (token) {
      options.headers['Authorization'] = `Bearer ${token}`;
    }

    const req = https.request(options, (res) => {
      let data = '';
      
      res.on('data', (chunk) => {
        data += chunk;
      });
      
      res.on('end', () => {
        try {
          const jsonData = data ? JSON.parse(data) : {};
          resolve({ status: res.statusCode, data: jsonData });
        } catch (e) {
          resolve({ status: res.statusCode, data: data });
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

function log(message, color = 'reset') {
  console.log(`${colors[color]}${message}${colors.reset}`);
}

function logTest(name, passed, details = '') {
  results.total++;
  if (passed) {
    results.passed++;
    log(`? ${name}`, 'green');
  } else {
    results.failed++;
    log(`? ${name}`, 'red');
    if (details) {
      log(`  ${details}`, 'red');
    }
    results.errors.push({ test: name, details });
  }
}

async function runTests() {
  log('\n?? Starting Comprehensive Profile API Tests\n', 'blue');
  
  let accessToken = '';

  // ========================================
  // Setup: Register and Login
  // ========================================
  log('?? Setup: Authentication', 'yellow');
  
  try {
    // Register
    const registerRes = await makeRequest('POST', '/Auth/register', {
      firstName: '????',
      lastName: '????',
      email: TEST_EMAIL,
      password: TEST_PASSWORD,
      confirmPassword: TEST_PASSWORD
    });
    logTest('Register new user', registerRes.status === 200, 
      registerRes.status !== 200 ? JSON.stringify(registerRes.data) : '');

    // Login
    const loginRes = await makeRequest('POST', '/Auth/login', {
      email: TEST_EMAIL,
      password: TEST_PASSWORD,
      deviceInfo: 'Node.js/Test'
    });
    logTest('Login user', loginRes.status === 200 && loginRes.data.accessToken, 
      !loginRes.data.accessToken ? 'No access token received' : '');
    
    if (loginRes.data.accessToken) {
      accessToken = loginRes.data.accessToken;
    } else {
      log('\n? Cannot proceed without access token', 'red');
      return;
    }
  } catch (error) {
    log(`\n? Setup failed: ${error.message}`, 'red');
    return;
  }

  // ========================================
  // Group 1: Profile Retrieval (Before Creation)
  // ========================================
  log('\n?? Group 1: Profile Retrieval (Before Creation)', 'yellow');
  
  try {
    const getRes = await makeRequest('GET', '/UserProfile', null, accessToken);
    logTest('Get profile (should be 404)', getRes.status === 404);

    const existsRes = await makeRequest('GET', '/UserProfile/exists', null, accessToken);
    logTest('Check exists (should be false)', 
      existsRes.status === 200 && existsRes.data.exists === false);

    const completionRes = await makeRequest('GET', '/UserProfile/completion', null, accessToken);
    logTest('Get completion (should be 0)', 
      completionRes.status === 200 && completionRes.data.completionPercentage === 0);
  } catch (error) {
    log(`Error in Group 1: ${error.message}`, 'red');
  }

  // ========================================
  // Group 2: Valid Profile Creation
  // ========================================
  log('\n?? Group 2: Valid Profile Creation', 'yellow');
  
  try {
    // Complete profile
    const completeRes = await makeRequest('PUT', '/UserProfile', {
      bio: '???? ???? ????? ???? ????????',
      location: '????? ??????',
      phoneNumber: '+962791234567',
      dateOfBirth: '2000-05-15',
      university: '??????? ????????',
      major: '???? ???????',
      expectedGraduationYear: 2026,
      currentLevel: 2,
      timezone: 'Asia/Amman',
      linkedInUrl: 'https://linkedin.com/in/test',
      gitHubUrl: 'https://github.com/test'
    }, accessToken);
    logTest('Create complete profile', completeRes.status === 200);

    // Minimal profile
    const minimalRes = await makeRequest('PUT', '/UserProfile', {
      university: 'Test University',
      major: 'Computer Science',
      expectedGraduationYear: 2026,
      currentLevel: 2,
      timezone: 'UTC'
    }, accessToken);
    logTest('Create minimal profile', minimalRes.status === 200);
  } catch (error) {
    log(`Error in Group 2: ${error.message}`, 'red');
  }

  // ========================================
  // Group 3: Validation Tests (Should Fail)
  // ========================================
  log('\n?? Group 3: Validation Tests (Should Fail)', 'yellow');
  
  const validationTests = [
    {
      name: 'Missing university',
      data: { major: 'CS', expectedGraduationYear: 2026, currentLevel: 2, timezone: 'UTC' }
    },
    {
      name: 'Missing major',
      data: { university: 'Test', expectedGraduationYear: 2026, currentLevel: 2, timezone: 'UTC' }
    },
    {
      name: 'Invalid year (too low)',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2020, currentLevel: 2, timezone: 'UTC' }
    },
    {
      name: 'Invalid year (too high)',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2055, currentLevel: 2, timezone: 'UTC' }
    },
    {
      name: 'Invalid level (too low)',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2026, currentLevel: -1, timezone: 'UTC' }
    },
    {
      name: 'Invalid level (too high)',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2026, currentLevel: 5, timezone: 'UTC' }
    },
    {
      name: 'Invalid timezone',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2026, currentLevel: 2, timezone: 'Invalid/TZ' }
    },
    {
      name: 'Invalid phone',
      data: { phoneNumber: '123-456', university: 'Test', major: 'CS', expectedGraduationYear: 2026, currentLevel: 2, timezone: 'UTC' }
    },
    {
      name: 'Invalid LinkedIn URL',
      data: { linkedInUrl: 'not-url', university: 'Test', major: 'CS', expectedGraduationYear: 2026, currentLevel: 2, timezone: 'UTC' }
    }
  ];

  for (const test of validationTests) {
    try {
      const res = await makeRequest('PUT', '/UserProfile', test.data, accessToken);
      logTest(test.name, res.status === 400, 
        res.status !== 400 ? `Expected 400, got ${res.status}` : '');
    } catch (error) {
      logTest(test.name, false, error.message);
    }
  }

  // ========================================
  // Group 4: Edge Cases
  // ========================================
  log('\n?? Group 4: Edge Cases', 'yellow');
  
  const edgeCases = [
    {
      name: 'Study level 0 (Freshman)',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2028, currentLevel: 0, timezone: 'UTC' }
    },
    {
      name: 'Study level 4 (Graduate)',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2024, currentLevel: 4, timezone: 'UTC' }
    },
    {
      name: 'Min year (2024)',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2024, currentLevel: 4, timezone: 'UTC' }
    },
    {
      name: 'Max year (2050)',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2050, currentLevel: 0, timezone: 'UTC' }
    },
    {
      name: 'Timezone Asia/Dubai',
      data: { university: 'Test', major: 'CS', expectedGraduationYear: 2026, currentLevel: 2, timezone: 'Asia/Dubai' }
    },
    {
      name: 'Special chars in bio',
      data: { bio: '?? Tech enthusiast ??', university: 'Test', major: 'CS', expectedGraduationYear: 2026, currentLevel: 2, timezone: 'UTC' }
    }
  ];

  for (const test of edgeCases) {
    try {
      const res = await makeRequest('PUT', '/UserProfile', test.data, accessToken);
      logTest(test.name, res.status === 200, 
        res.status !== 200 ? JSON.stringify(res.data) : '');
    } catch (error) {
      logTest(test.name, false, error.message);
    }
  }

  // ========================================
  // Group 5: Profile Retrieval (After Creation)
  // ========================================
  log('\n?? Group 5: Profile Retrieval (After Creation)', 'yellow');
  
  try {
    const getRes = await makeRequest('GET', '/UserProfile', null, accessToken);
    logTest('Get profile (should be 200)', getRes.status === 200);

    const existsRes = await makeRequest('GET', '/UserProfile/exists', null, accessToken);
    logTest('Check exists (should be true)', 
      existsRes.status === 200 && existsRes.data.exists === true);

    const completionRes = await makeRequest('GET', '/UserProfile/completion', null, accessToken);
    logTest('Get completion (should be > 0)', 
      completionRes.status === 200 && completionRes.data.completionPercentage > 0);
  } catch (error) {
    log(`Error in Group 5: ${error.message}`, 'red');
  }

  // ========================================
  // Group 6: Timezone Utilities
  // ========================================
  log('\n?? Group 6: Timezone Utilities', 'yellow');
  
  try {
    const timezonesRes = await makeRequest('GET', '/UserProfile/timezones');
    logTest('Get all timezones', 
      timezonesRes.status === 200 && Array.isArray(timezonesRes.data));

    const jordanRes = await makeRequest('GET', '/UserProfile/timezones?location=Jordan');
    logTest('Get timezones for Jordan', jordanRes.status === 200);

    const validateRes = await makeRequest('GET', '/UserProfile/validate-timezone?timezone=Asia/Amman');
    logTest('Validate timezone', 
      validateRes.status === 200 && validateRes.data.isValid === true);

    const invalidRes = await makeRequest('GET', '/UserProfile/validate-timezone?timezone=Invalid/TZ');
    logTest('Validate invalid timezone', 
      invalidRes.status === 200 && invalidRes.data.isValid === false);
  } catch (error) {
    log(`Error in Group 6: ${error.message}`, 'red');
  }

  // ========================================
  // Group 7: Authorization Tests
  // ========================================
  log('\n?? Group 7: Authorization Tests', 'yellow');
  
  try {
    const noTokenRes = await makeRequest('GET', '/UserProfile');
    logTest('Get profile without token (should be 401)', noTokenRes.status === 401);

    const invalidTokenRes = await makeRequest('GET', '/UserProfile', null, 'invalid.token');
    logTest('Get profile with invalid token (should be 401)', invalidTokenRes.status === 401);
  } catch (error) {
    log(`Error in Group 7: ${error.message}`, 'red');
  }

  // ========================================
  // Results Summary
  // ========================================
  log('\n' + '='.repeat(50), 'blue');
  log('?? Test Results Summary', 'blue');
  log('='.repeat(50), 'blue');
  log(`Total Tests: ${results.total}`);
  log(`Passed: ${results.passed}`, 'green');
  log(`Failed: ${results.failed}`, results.failed > 0 ? 'red' : 'green');
  log(`Success Rate: ${((results.passed / results.total) * 100).toFixed(2)}%`, 
    results.failed === 0 ? 'green' : 'yellow');

  if (results.errors.length > 0) {
    log('\n? Failed Tests:', 'red');
    results.errors.forEach((error, index) => {
      log(`${index + 1}. ${error.test}`, 'red');
      if (error.details) {
        log(`   ${error.details}`, 'red');
      }
    });
  }

  log('\n? Testing completed!', 'green');
}

// Run tests
runTests().catch(error => {
  log(`\n? Fatal error: ${error.message}`, 'red');
  console.error(error);
});
