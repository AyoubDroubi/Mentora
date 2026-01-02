import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import api from '../services/api';
import {
  BookOpen,
  GraduationCap,
  Briefcase,
  TrendingUp as TrendingUpIcon,
} from 'lucide-react';

export default function CreateCareerBuilder() {
  const navigate = useNavigate();
  const { user } = useUser();

  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  const questions = [
    {
      id: 'q1',
      type: 'short_answer',
      question: ' Career Field (Short Answer)\n\nWhat career field or job role are you most interested in right now?',
      placeholder: 'e.g., Software Developer, Data Scientist, Product Manager...'
    },
    {
      id: 'q2',
      type: 'short_answer',
      question: 'Career Motivation (Short Answer)\n\nWhy does this career appeal to you?',
      placeholder: 'e.g., I enjoy solving complex problems, creative freedom, helping others...'
    },
    {
      id: 'q3',
      type: 'multiple_choice',
      maxSelections: 2,
      question: ' Industry Interest (Multiple Choice)\n\nWhich industries attract you the most? (Select up to 2)',
      options: [
        'Technology / Software',
        'Cybersecurity',
        'Data & AI',
        'Finance / FinTech',
        'Healthcare / HealthTech',
        'Education / EdTech',
        'E-commerce / Startups',
        'Consulting / Business Services',
        'Government / Public Sector',
        'Other: _______'
      ]
    },
    {
      id: 'q4',
      type: 'multiple_choice',
      maxSelections: 3,
      question: ' Core Strengths (Multiple Choice)\n\nWhat are your top strengths? (Select up to 3)',
      options: [
        'Problem-solving',
        'Analytical thinking',
        'Creativity',
        'Communication',
        'Leadership',
        'Organization & planning',
        'Learning fast',
        'Technical skills',
        'Adaptability'
      ]
    },
    {
      id: 'q5',
      type: 'short_answer',
      question: ' Existing Skills (Short Answer)\n\nWhat technical or practical skills do you already have?',
      placeholder: 'e.g., JavaScript, Python, React, SQL, project management...'
    },
    {
      id: 'q6',
      type: 'multiple_choice',
      maxSelections: null, // Select all that apply
      question: ' Skill Gaps (Multiple Choice)\n\nWhich skills do you need to improve or learn? (Select all that apply)',
      options: [
        'Programming / Technical depth',
        'System design / Architecture',
        'Problem-solving / Algorithms',
        'Communication & presentation',
        'Time management & consistency',
        'Teamwork',
        'Leadership',
        'Real-world experience',
        'Portfolio / Projects'
      ]
    },
    {
      id: 'q7',
      type: 'multiple_choice',
      maxSelections: null, // Select all that apply
      question: ' Experience Level (Multiple Choice)\n\nWhat experience do you currently have? (Select all that apply)',
      options: [
        'Academic projects',
        'Personal projects',
        'Internship',
        'Volunteer work',
        'Hackathons / Competitions',
        'Freelance / Paid work',
        'No real experience yet'
      ]
    },
    {
      id: 'q8',
      type: 'single_choice',
      question: ' Work Preference (Multiple Choice)\n\nHow do you prefer to work?',
      options: [
        'Mostly alone',
        'Mostly with a team',
        'Balanced (alone + team)'
      ]
    },
    {
      id: 'q9',
      type: 'single_choice',
      question: ' Personality Style (Multiple Choice)\n\nWhich best describes you?',
      options: [
        'Analytical',
        'Creative',
        'Structured',
        'Flexible',
        'Leadership-driven'
      ]
    },
    {
      id: 'q10',
      type: 'multiple_choice',
      maxSelections: 2,
      question: ' Enjoyed Activities (Multiple Choice)\n\nWhich activities do you enjoy most? (Select up to 2)',
      options: [
        'Problem-solving',
        'Building things',
        'Research & analysis',
        'Management & coordination',
        'Creative work'
      ]
    },
    {
      id: 'q11',
      type: 'single_choice',
      question: ' Learning Speed & Style (Multiple Choice)\n\nHow do you learn best?',
      options: [
        'Fast learner, self-study',
        'Moderate pace, structured courses',
        'Slow but deep learning',
        'Learning by doing (projects)',
        'Learning with mentorship/guidance'
      ]
    },
    {
      id: 'q12',
      type: 'single_choice',
      question: ' Pressure Handling (Multiple Choice)\n\nHow do you handle stress and deadlines?',
      options: [
        'I perform better under pressure',
        'I handle it well with planning',
        'I struggle but improve with structure',
        'I get overwhelmed easily'
      ]
    },
    {
      id: 'q13',
      type: 'mixed',
      question: ' Future Vision (Multiple Choice + Short Answer)\n\nWhere do you see yourself in 2–5 years?',
      options: [
        'Junior role',
        'Mid-level professional',
        'Specialist / Expert',
        'Team lead / Manager',
        'Founder / Entrepreneur'
      ],
      hasSalaryField: true,
      salaryPlaceholder: 'Target starting salary (optional): _______'
    },
    {
      id: 'q14',
      type: 'mixed',
      question: 'Constraints & Time (Multiple Choice)\n\nWhat is your biggest current challenge?',
      options: [
        'Lack of skills',
        'Lack of consistency',
        'Lack of confidence',
        'Time management',
        'No clear direction'
      ],
      hasTimeField: true,
      timeOptions: [
        '5–7 hours',
        '8–12 hours',
        '13–20 hours',
        '20+ hours'
      ]
    }
  ];

  const [currentStep, setCurrentStep] = useState(0);
  const [answers, setAnswers] = useState({});
  const [showResult, setShowResult] = useState(false);
  const [status, setStatus] = useState('Draft');
  const [hasCompletedAssessment, setHasCompletedAssessment] = useState(false);
  const [showQuiz, setShowQuiz] = useState(false);

  const updateAnswer = (questionId, value, isMultiple = false) => {
    if (isMultiple) {
      setAnswers(prev => {
        const currentAnswers = prev[questionId] || [];
        if (currentAnswers.includes(value)) {
          return { ...prev, [questionId]: currentAnswers.filter(v => v !== value) };
        } else {
          return { ...prev, [questionId]: [...currentAnswers, value] };
        }
      });
    } else {
      setAnswers(prev => ({ ...prev, [questionId]: value }));
    }
  };

  const updateMixedAnswer = (questionId, field, value) => {
    setAnswers(prev => ({
      ...prev,
      [questionId]: {
        ...(prev[questionId] || {}),
        [field]: value
      }
    }));
  };

  const handleNext = () => {
    const currentAnswer = answers[currentQuestion.id];

    let isValid = false;

    if (currentQuestion.type === 'short_answer') {
      isValid = currentAnswer && currentAnswer.trim() !== '';
    } else if (currentQuestion.type === 'multiple_choice') {
      if (currentQuestion.maxSelections) {
        isValid = currentAnswer && currentAnswer.length > 0 && currentAnswer.length <= currentQuestion.maxSelections;
      } else {
        isValid = currentAnswer && currentAnswer.length > 0;
      }
    } else if (currentQuestion.type === 'single_choice') {
      isValid = currentAnswer && currentAnswer !== '';
    } else if (currentQuestion.type === 'mixed') {
      isValid = currentAnswer && currentAnswer.option && currentAnswer.option !== '';
    }

    if (isValid) {
      if (currentStep < questions.length - 1) {
        setCurrentStep(currentStep + 1);
      } else {
        setShowResult(true);
        setHasCompletedAssessment(true);
        setStatus('Completed');
      }
    }
  };

  const handlePrevious = () => {
    if (currentStep > 0) {
      setCurrentStep(currentStep - 1);
    }
  };

  const resetForm = () => {
    setAnswers({});
    setCurrentStep(0);
    setShowResult(false);
    setHasCompletedAssessment(false);
    setStatus('Draft');
  };

  const handleSubmitGenerate = async () => {
    if (!hasCompletedAssessment) return;

    try {
      // Submit answers to backend using the configured API client
      const response = await api.post('/career-quiz/submit', { answers });

      if (response.data.success) {
        console.log('✅ Career plan generated:', response.data);
        // Navigate to the generated plan
        if (response.data.planId) {
          navigate(`/career-plan/${response.data.planId}`);
        } else {
          navigate('/career-builder');
        }
      } else {
        console.error('❌ Failed to submit assessment:', response.data.message);
        alert(response.data.message || 'Failed to generate career plan');
      }
    } catch (error) {
      console.error('❌ Error submitting assessment:', error);
      alert(error.response?.data?.message || 'Error connecting to server. Please try again.');
    }
  };

  
    const Header = () => (
         <header
           className="px-6 py-4 flex items-center justify-between shadow-lg"
           style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
         >
           <div className="flex items-center gap-3">
             <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
               <BookOpen className="w-6 h-6" style={{ color: M.primary }} />
             </div>
             <span className="text-white text-xl font-bold">Mentora - career builder</span>
           </div>
     
           <div className="flex items-center gap-4">
             <img
             onClick={() => navigate('/profile')}
               src={user.avatar}
               alt="Profile"
               className="w-10 h-10 rounded-full border-2 border-white hover:scale-110 hover:opacity-90 transition-all cursor-pointer"
             />
           </div>
         </header>
       );
     



  if (showResult) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <Header />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
            <h2 className="text-2xl font-bold mb-4" style={{ color: M.text }}>Career Assessment Result</h2>
            <p className="text-lg mb-6" style={{ color: M.muted }}>Based on your answers, here is your career assessment:</p>

            <div className="space-y-4">
              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Career Interests:</h3>
                <p>{answers.q1 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Appealing Factors:</h3>
                <p>{answers.q2 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Industries:</h3>
                <p>{Array.isArray(answers.q3) ? answers.q3.join(', ') : answers.q3 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Strengths:</h3>
                <p>{Array.isArray(answers.q4) ? answers.q4.join(', ') : answers.q4 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Skills:</h3>
                <p>{answers.q5 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Skills to Learn:</h3>
                <p>{Array.isArray(answers.q6) ? answers.q6.join(', ') : answers.q6 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Experience:</h3>
                <p>{Array.isArray(answers.q7) ? answers.q7.join(', ') : answers.q7 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Work Style:</h3>
                <p>{answers.q8 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Personality:</h3>
                <p>{answers.q9 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Activities:</h3>
                <p>{Array.isArray(answers.q10) ? answers.q10.join(', ') : answers.q10 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Learning:</h3>
                <p>{answers.q11 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Stress Handling:</h3>
                <p>{answers.q12 || 'Not specified'}</p>
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Goals:</h3>
                <p>{answers.q13?.option || 'Not specified'}</p>
                {answers.q13?.salary && <p className="text-sm text-gray-600 mt-1">Salary: {answers.q13.salary}</p>}
              </div>

              <div className="p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold mb-2" style={{ color: M.text }}>Challenges:</h3>
                <p>{answers.q14?.option || 'Not specified'}</p>
                {answers.q14?.time && <p className="text-sm text-gray-600 mt-1">Time: {answers.q14.time}</p>}
              </div>
            </div>

            <div className="flex justify-center gap-4 mt-8">
              <button
                onClick={handleSubmitGenerate}
                disabled={!hasCompletedAssessment}
                className={`px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all ${
                  !hasCompletedAssessment ? 'bg-gray-300 cursor-not-allowed' : ''
                }`}
                style={{ background: hasCompletedAssessment ? M.primary : undefined }}
              >
                Go to Career Plan
              </button>
              <button
                onClick={resetForm}
                className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
                style={{ background: M.secondary }}
              >
                Retake Assessment
              </button>
            </div>
          </div>
      </main>
    </div>
  );
}

  if (!showQuiz) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <Header />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-3xl p-8 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <h1 className="text-3xl font-bold mb-4" style={{ color: M.text }}>Career Assessment</h1>
            <p className="text-lg mb-6" style={{ color: M.muted }}>
              Take our comprehensive career assessment to discover your ideal career path and get personalized recommendations.
            </p>
            <button
              onClick={() => setShowQuiz(true)}
              className="px-8 py-4 rounded-lg text-white font-medium hover:shadow-lg transition-all text-lg mb-6"
              style={{ background: M.primary }}
            >
              Start Career Quiz
            </button>

            {/* Status Info */}
            <div className="flex flex-col items-center gap-3">
              <span className={`px-4 py-2 rounded-full text-sm font-medium ${
                status === 'Draft' ? 'bg-yellow-100 text-yellow-800' : 'bg-green-100 text-green-800'
              }`}>
                {status === 'Draft' ? 'Draft' : 'Completed'}
              </span>
            </div>
          </div>
        </main>
      </div>
    );
  }

  const currentQuestion = questions[currentStep];

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <Header />
      <main className="container mx-auto px-4 mt-6">
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <div className="mb-6">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-2xl font-bold" style={{ color: M.text }}>Career Assessment</h2>
              <div className="flex items-center gap-4">
                <span className={`px-3 py-1 rounded-full text-sm font-medium ${
                  status === 'Draft' ? 'bg-yellow-100 text-yellow-800' : 'bg-green-100 text-green-800'
                }`}>
                  {status}
                </span>
              </div>
            </div>
            <div className="w-full bg-gray-200 rounded-full h-2">
              <div
                className="h-2 rounded-full transition-all duration-300"
                style={{ width: `${((currentStep + 1) / questions.length) * 100}%`, background: M.primary }}
              />
            </div>
          </div>

          <div className="mb-8">
            <h3 className="text-xl font-semibold mb-6" style={{ color: M.text, whiteSpace: 'pre-line' }}>
              {currentQuestion.question}
            </h3>

            <div className="space-y-3">
              {currentQuestion.type === 'short_answer' && (
                <textarea
                  placeholder={currentQuestion.placeholder}
                  value={answers[currentQuestion.id] || ''}
                  onChange={(e) => updateAnswer(currentQuestion.id, e.target.value)}
                  className="w-full p-4 border-2 border-gray-200 rounded-lg focus:border-[#6B9080] focus:outline-none resize-none"
                  rows={4}
                  style={{ color: M.text }}
                />
              )}

              {currentQuestion.type === 'single_choice' && currentQuestion.options && (
                currentQuestion.options.map((option, index) => (
                  <label
                    key={index}
                    className={`flex items-center p-4 rounded-lg border-2 cursor-pointer transition-all hover:shadow-md ${
                      answers[currentQuestion.id] === option
                        ? 'border-[#6B9080] bg-[#F6FFF8]'
                        : 'border-gray-200 hover:border-gray-300'
                    }`}
                  >
                    <input
                      type="radio"
                      name={currentQuestion.id}
                      value={option}
                      checked={answers[currentQuestion.id] === option}
                      onChange={(e) => updateAnswer(currentQuestion.id, e.target.value)}
                      className="mr-3 text-[#6B9080] focus:ring-[#6B9080]"
                    />
                    <span className="text-lg" style={{ color: M.text }}>{option}</span>
                  </label>
                ))
              )}

              {currentQuestion.type === 'multiple_choice' && currentQuestion.options && (
                <>
                  {currentQuestion.maxSelections && (
                    <p className="text-sm mb-2" style={{ color: M.muted }}>
                      Select up to {currentQuestion.maxSelections} options
                    </p>
                  )}
                  {currentQuestion.options.map((option, index) => (
                    <label
                      key={index}
                      className={`flex items-center p-4 rounded-lg border-2 cursor-pointer transition-all hover:shadow-md ${
                        answers[currentQuestion.id] && answers[currentQuestion.id].includes(option)
                          ? 'border-[#6B9080] bg-[#F6FFF8]'
                          : 'border-gray-200 hover:border-gray-300'
                      }`}
                    >
                      <input
                        type="checkbox"
                        value={option}
                        checked={answers[currentQuestion.id] && answers[currentQuestion.id].includes(option)}
                        onChange={(e) => updateAnswer(currentQuestion.id, option, true)}
                        className="mr-3 text-[#6B9080] focus:ring-[#6B9080]"
                      />
                      <span className="text-lg" style={{ color: M.text }}>{option}</span>
                    </label>
                  ))}
                </>
              )}

              {currentQuestion.type === 'mixed' && (
                <div className="space-y-4">
                  <div>
                    <select
                      value={answers[currentQuestion.id]?.option || ''}
                      onChange={(e) => updateMixedAnswer(currentQuestion.id, 'option', e.target.value)}
                      className="w-full p-4 border-2 border-gray-200 rounded-lg focus:border-[#6B9080] focus:outline-none"
                      style={{ color: M.text }}
                    >
                      <option value="">Select an option</option>
                      {currentQuestion.options.map((option, index) => (
                        <option key={index} value={option}>{option}</option>
                      ))}
                    </select>
                  </div>
                  {currentQuestion.hasSalaryField && (
                    <input
                      type="text"
                      placeholder={currentQuestion.salaryPlaceholder}
                      value={answers[currentQuestion.id]?.salary || ''}
                      onChange={(e) => updateMixedAnswer(currentQuestion.id, 'salary', e.target.value)}
                      className="w-full p-4 border-2 border-gray-200 rounded-lg focus:border-[#6B9080] focus:outline-none"
                      style={{ color: M.text }}
                    />
                  )}
                  {currentQuestion.hasTimeField && (
                    <select
                      value={answers[currentQuestion.id]?.time || ''}
                      onChange={(e) => updateMixedAnswer(currentQuestion.id, 'time', e.target.value)}
                      className="w-full p-4 border-2 border-gray-200 rounded-lg focus:border-[#6B9080] focus:outline-none"
                      style={{ color: M.text }}
                    >
                      <option value="">Select time commitment</option>
                      {currentQuestion.timeOptions.map((time, index) => (
                        <option key={index} value={time}>{time}</option>
                      ))}
                    </select>
                  )}
                </div>
              )}
            </div>
          </div>

          <div className="flex justify-between">
            <button
              onClick={handlePrevious}
              disabled={currentStep === 0}
              className={`px-6 py-3 rounded-lg font-medium transition-all ${
                currentStep === 0
                  ? 'bg-gray-300 text-gray-500 cursor-not-allowed'
                  : 'text-white hover:shadow-lg'
              }`}
              style={{ background: currentStep === 0 ? undefined : M.primary }}
            >
              Previous
            </button>

            <button
              onClick={handleNext}
              disabled={!answers[currentQuestion.id]}
              className={`px-6 py-3 rounded-lg font-medium transition-all ${
                !answers[currentQuestion.id]
                  ? 'bg-gray-300 text-gray-500 cursor-not-allowed'
                  : 'text-white hover:shadow-lg'
              }`}
              style={{ background: answers[currentQuestion.id] ? M.primary : undefined }}
            >
              {currentStep === questions.length - 1 ? 'Go to Career Plan' : 'Next'}
            </button>
          </div>
        </div>

        <div className="flex justify-center">
          <button
            onClick={resetForm}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.primary }}
          >
            Reset Assessment
          </button>
        </div>
      </main>
    </div>
  );
}
