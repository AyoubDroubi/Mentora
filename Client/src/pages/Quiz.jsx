import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import SharedHeader from '../components/SharedHeader';
import { HelpCircle, CheckSquare, Play, RotateCcw, ChevronRight, ChevronLeft } from 'lucide-react';

export default function StudyPlannerAssessment() {
  const navigate = useNavigate();
  const { user, setUser } = useUser();

  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  const [isLoggedIn, setIsLoggedIn] = useState(true);

  // Quiz state 
  const [quizzes, setQuizzes] = useState([
    {
      id: 1,
      title: 'Study Plan Assessment',
      description: 'Answer questions to get a personalized study plan',
      questions: 14,
      duration: '10-15 min',
      completed: false,
      score: null
    }
  ]);

  const [quizStarted, setQuizStarted] = useState(false);
  const [currentQuizId, setCurrentQuizId] = useState(null);
  const [quizAnswers, setQuizAnswers] = useState({});
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [showResults, setShowResults] = useState(false);
  const [studyPlan, setStudyPlan] = useState(null);

  const quizQuestions = [
    {
      question: "What are your top 3 academic or learning goals for the next 30–90 days?",
      type: "open-ended",
      options: []
    },
    {
      question: "Which subject or skill do you want to improve the most, and why?",
      type: "open-ended",
      options: []
    },
    {
      question: "How many hours per day can you realistically study?",
      type: "single-choice",
      options: [
        "Less than 1 hour",
        "1–2 hours",
        "2–3 hours",
        "3–4 hours",
        "More than 4 hours"
      ]
    },
    {
      question: "At what times of day do you feel most productive?",
      type: "multiple-choice",
      options: [
        "Early morning (5–9 AM)",
        "Late morning (9–12 PM)",
        "Afternoon (12–5 PM)",
        "Evening (5–9 PM)",
        "Night (9 PM–1 AM)"
      ]
    },
    {
      question: "Which days of the week are available for studying?",
      type: "multiple-choice",
      options: [
        "Sunday",
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday"
      ]
    },
    {
      question: "How do you usually plan your study sessions?",
      type: "single-choice",
      options: [
        "I don't plan; I study when I feel like it",
        "Rough plan in my head",
        "To-do list or notes app",
        "Calendar or time-blocking",
        "Strict daily schedule"
      ]
    },
    {
      question: "How do you usually start a study session?",
      type: "single-choice",
      options: [
        "Start immediately",
        "Delay a little, then start",
        "Get distracted (phone/social media)",
        "Wait until I 'feel motivated'",
        "Avoid starting unless there's pressure"
      ]
    },
    {
      question: "How long can you stay focused before needing a break?",
      type: "single-choice",
      options: [
        "Less than 15 minutes",
        "15–30 minutes",
        "30–45 minutes",
        "45–60 minutes",
        "More than 60 minutes"
      ]
    },
    {
      question: "What are the main things that distract you while studying?",
      type: "multiple-choice",
      options: [
        "Phone / social media",
        "Noise or environment",
        "Boredom",
        "Stress or anxiety",
        "Fatigue",
        "Overthinking / perfectionism"
      ]
    },
    {
      question: "Which subjects or topics do you struggle with the most, and why?",
      type: "open-ended",
      options: []
    },
    {
      question: "What do you struggle with the most?",
      type: "single-choice",
      options: [
        "Remembering information",
        "Understanding concepts",
        "Applying what I learn",
        "All of the above"
      ]
    },
    {
      question: "What is your biggest challenge in studying consistently?",
      type: "single-choice",
      options: [
        "Procrastination",
        "Lack of motivation",
        "Poor time management",
        "Burnout / low energy",
        "Stress or anxiety",
        "Distractions"
      ]
    },
    {
      question: "Do you have upcoming exams, projects, or deadlines?",
      type: "open-ended",
      options: []
    },
    {
      question: "How would you describe your study personality?",
      type: "single-choice",
      options: [
        "Very structured and disciplined",
        "Semi-organized",
        "Chaotic but motivated",
        "Procrastinator",
        "Only motivated close to exams"
      ]
    }
  ];

 

  // Header 
  const Header = () => (
    <header className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}>
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
          <HelpCircle className="w-6 h-6" style={{ color: M.primary }} />
        </div>
        <span className="text-white text-xl font-bold">Mentora - Study Planner</span>
      </div>

      <nav className="flex items-center gap-4">
        

        {isLoggedIn ? (
          <button
            onClick={() => navigate('/profile')}
            className="ml-4 w-10 h-10 rounded-full border-2 border-white overflow-hidden hover:scale-110 transition-transform"
            title="Open profile"
          >
            <img src={user.avatar} alt="avatar" className="w-full h-full object-cover" />
          </button>
        ) : (
          <button onClick={() => navigate('/login')} className="ml-4 bg-white text-[#6B9080] px-4 py-2 rounded-lg font-medium hover:shadow-lg transition-shadow">Login</button>
        )}
      </nav>
    </header>
  );

  const startQuiz = (id) => {
    setCurrentQuizId(id);
    setQuizStarted(true);
  };

  const nextQuestion = () => {
    if (currentQuestionIndex < quizQuestions.length - 1) {
      setCurrentQuestionIndex(currentQuestionIndex + 1);
    }
  };

  const prevQuestion = () => {
    if (currentQuestionIndex > 0) {
      setCurrentQuestionIndex(currentQuestionIndex - 1);
    }
  };

  const generateStudyPlan = (answers) => {
    // Simple AI-like analysis based on answers
    const goals = answers[0] || '';
    const subject = answers[1] || '';
    const hours = answers[2] || '';
    const days = answers[3] || '';
    const planning = answers[4] || '';
    const focus = answers[5] || '';
    const struggles = answers[6] || '';
    const challenge = answers[7] || '';
    const deadlines = answers[8] || '';
    const environment = answers[9] || '';
    const techniques = answers[10] || '';
    const motivation = answers[11] || '';

    let studyPlan = {
      title: "Your Personalized Study Plan",
      summary: "",
      recommendations: [],
      schedule: [],
      tips: []
    };

    // Generate summary based on goals
    if (goals.includes('Improve grades')) {
      studyPlan.summary = "Focus on targeted improvement in key subjects with structured revision.";
    } else if (goals.includes('Master new skills')) {
      studyPlan.summary = "Emphasize skill-building through practical application and consistent practice.";
    } else if (goals.includes('Prepare for exams')) {
      studyPlan.summary = "Prioritize exam preparation with focused study sessions and mock tests.";
    }

    // Schedule based on availability
    if (hours.includes('2-3 hours')) {
      studyPlan.schedule.push("2-3 focused study sessions per day");
    } else if (hours.includes('4-5 hours')) {
      studyPlan.schedule.push("4-5 hours of structured study daily");
    }

    if (days.includes('Weekdays only')) {
      studyPlan.schedule.push("Dedicated weekday study routine");
    } else if (days.includes('Every day')) {
      studyPlan.schedule.push("Balanced daily study including weekends");
    }

    // Techniques recommendations
    if (focus.includes('25-45 minutes')) {
      studyPlan.recommendations.push("Use Pomodoro technique (25 min study + 5 min break)");
    }

    if (challenge.includes('Distractions')) {
      studyPlan.tips.push("Create a dedicated study space free from distractions");
      studyPlan.tips.push("Use website blockers during study sessions");
    }

    if (environment.includes('Home desk')) {
      studyPlan.tips.push("Optimize your home study space for better focus");
    }

    // Default recommendations if analysis is minimal
    if (studyPlan.recommendations.length === 0) {
      studyPlan.recommendations = [
        "Set specific, achievable daily goals",
        "Use active recall and spaced repetition",
        "Take regular breaks to maintain focus",
        "Track your progress weekly"
      ];
    }

    if (studyPlan.tips.length === 0) {
      studyPlan.tips = [
        "Stay consistent with your study schedule",
        "Get adequate sleep for better concentration",
        "Stay hydrated and maintain healthy eating habits",
        "Review material regularly to reinforce learning"
      ];
    }

    return studyPlan;
  };

  const submitQuiz = () => {
    const plan = generateStudyPlan(quizAnswers);
    setStudyPlan(plan);
    setShowResults(true);

    // Mark quiz as completed
    setQuizzes(q => q.map(quiz =>
      quiz.id === currentQuizId
        ? { ...quiz, completed: true, score: 85 }
        : quiz
    ));
  };

  const resetQuiz = () => {
    setQuizStarted(false);
    setCurrentQuizId(null);
    setCurrentQuestionIndex(0);
    setQuizAnswers({});
    setShowResults(false);
    setStudyPlan(null);
  };

  // QUIZ LIST VIEW
  if (!quizStarted) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <SharedHeader title="Mentora - Quiz" />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-2xl p-6 shadow-lg mb-6 border" style={{ borderColor: M.bg3 }}>
            <h2 className="text-xl font-bold text-[#2C3E3F] mb-2 flex items-center gap-2">
              <HelpCircle className="w-6 h-6" style={{ color: M.primary }} />
              Study Plan Assessment
            </h2>
            <p className="text-sm text-[#5A7A6B] mb-4">Take the assessment to get a personalized study plan</p>
          </div>

          <div className="space-y-4">
            {quizzes.map(quiz => (
              <div
                key={quiz.id}
                className="bg-white rounded-2xl p-5 shadow-lg border hover:shadow-xl transition-all"
                style={{ borderColor: M.bg3 }}
              >
                <div className="flex items-start justify-between mb-3">
                  <div className="flex-1">
                    <h3 className="font-bold text-[#2C3E3F] text-lg">{quiz.title}</h3>
                    <div className="flex items-center gap-4 mt-2 text-sm text-[#5A7A6B]">
                      <span className="flex items-center gap-1">
                        <HelpCircle className="w-4 h-4" />
                        {quiz.questions} questions
                      </span>
                    </div>
                  </div>
                  {quiz.completed && (
                    <div className="flex items-center gap-2 px-3 py-1 rounded-full bg-green-100 text-green-700">
                      <CheckSquare className="w-4 h-4" />
                      <span className="text-sm font-medium">{quiz.score}%</span>
                    </div>
                  )}
                </div>

                <button
                  onClick={() => startQuiz(quiz.id)}
                  className="w-full mt-3 px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center justify-center gap-2"
                  style={{ background: quiz.completed ? M.secondary : M.primary }}
                >
                  {quiz.completed ? (
                    <>
                      <RotateCcw className="w-4 h-4" />
                      Retake Assessment
                    </>
                  ) : (
                    <>
                      <Play className="w-4 h-4" />
                      Start Assessment
                    </>
                  )}
                </button>
              </div>
            ))}
          </div>
        </main>
      </div>
    );
  }

  // RESULTS VIEW
  if (showResults && studyPlan) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <SharedHeader title="Mentora - Quiz Results" />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-2xl p-6 shadow-lg mb-6 border" style={{ borderColor: M.bg3 }}>
            <h2 className="text-xl font-bold text-[#2C3E3F] mb-2 flex items-center gap-2">
              <CheckSquare className="w-6 h-6" style={{ color: M.primary }} />
              {studyPlan.title}
            </h2>
            <p className="text-sm text-[#5A7A6B]">{studyPlan.summary}</p>
          </div>

          {studyPlan.schedule.length > 0 && (
            <div className="bg-white rounded-2xl p-6 shadow-lg mb-6 border" style={{ borderColor: M.bg3 }}>
              <h3 className="text-lg font-semibold text-[#2C3E3F] mb-4">📅 Recommended Schedule</h3>
              <ul className="space-y-2">
                {studyPlan.schedule.map((item, index) => (
                  <li key={index} className="flex items-center gap-3 text-[#5A7A6B]">
                    <div className="w-2 h-2 rounded-full" style={{ background: M.primary }}></div>
                    {item}
                  </li>
                ))}
              </ul>
            </div>
          )}

          {studyPlan.recommendations.length > 0 && (
            <div className="bg-white rounded-2xl p-6 shadow-lg mb-6 border" style={{ borderColor: M.bg3 }}>
              <h3 className="text-lg font-semibold text-[#2C3E3F] mb-4">💡 Study Recommendations</h3>
              <ul className="space-y-2">
                {studyPlan.recommendations.map((rec, index) => (
                  <li key={index} className="flex items-center gap-3 text-[#5A7A6B]">
                    <div className="w-2 h-2 rounded-full" style={{ background: M.primary }}></div>
                    {rec}
                  </li>
                ))}
              </ul>
            </div>
          )}

          {studyPlan.tips.length > 0 && (
            <div className="bg-white rounded-2xl p-6 shadow-lg mb-6 border" style={{ borderColor: M.bg3 }}>
              <h3 className="text-lg font-semibold text-[#2C3E3F] mb-4">✨ Additional Tips</h3>
              <ul className="space-y-2">
                {studyPlan.tips.map((tip, index) => (
                  <li key={index} className="flex items-center gap-3 text-[#5A7A6B]">
                    <div className="w-2 h-2 rounded-full" style={{ background: M.secondary }}></div>
                    {tip}
                  </li>
                ))}
              </ul>
            </div>
          )}

          

          <div className="flex gap-4">
            <button
              onClick={resetQuiz}
              className="flex-1 px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center justify-center gap-2"
              style={{ background: M.secondary }}
            >
              <RotateCcw className="w-4 h-4" />
              Retake Assessment
            </button>
            <button
              onClick={() => navigate('/study-planner')}
              className="flex-1 px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center justify-center gap-2"
              style={{ background: M.primary }}
            >
              Create Study Plan
            </button>
          </div>
        </main>
      </div>
    );
  }

  // QUIZ TAKING VIEW
  const currentQuestion = quizQuestions[currentQuestionIndex];
  const isLastQuestion = currentQuestionIndex === quizQuestions.length - 1;
  const isFirstQuestion = currentQuestionIndex === 0;
  const hasAnsweredFirstQuestion = quizAnswers[0] !== undefined && quizAnswers[0] !== '';

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <SharedHeader title="Mentora - Quiz" />
      <main className="container mx-auto px-4 mt-6">
        <div className="bg-white rounded-2xl p-6 shadow-lg mb-6 border" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-xl font-bold text-[#2C3E3F] flex items-center gap-2">
              <HelpCircle className="w-6 h-6" style={{ color: M.primary }} />
              Assessment in Progress
            </h2>
            <span className="text-sm text-[#5A7A6B]">
              Question {currentQuestionIndex + 1} of {quizQuestions.length}
            </span>
          </div>
          <div className="w-full bg-gray-200 rounded-full h-2 mb-4">
            <div
              className="h-2 rounded-full transition-all"
              style={{ width: `${((currentQuestionIndex + 1) / quizQuestions.length) * 100}%`, background: M.primary }}
            ></div>
          </div>
        </div>

        <div className="bg-white rounded-2xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <h3 className="text-lg font-semibold text-[#2C3E3F] mb-4">{currentQuestion.question}</h3>
          <div className="space-y-3">
            {currentQuestion.type === "open-ended" ? (
              <textarea
                placeholder="Type your answer here..."
                value={quizAnswers[currentQuestionIndex] || ''}
                onChange={(e) => setQuizAnswers(prev => ({ ...prev, [currentQuestionIndex]: e.target.value }))}
                rows={4}
                className="w-full px-4 py-3 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                style={{ borderColor: M.bg3 }}
              />
            ) : currentQuestion.type === "multiple-choice" ? (
              currentQuestion.options.map((option, index) => (
                <label key={index} className="flex items-center gap-3 p-3 rounded-lg border hover:bg-gray-50 cursor-pointer transition-all" style={{ borderColor: M.bg3 }}>
                  <input
                    type="checkbox"
                    value={option}
                    checked={(quizAnswers[currentQuestionIndex] || []).includes(option)}
                    onChange={(e) => {
                      const currentAnswers = quizAnswers[currentQuestionIndex] || [];
                      const newAnswers = e.target.checked
                        ? [...currentAnswers, option]
                        : currentAnswers.filter(ans => ans !== option);
                      setQuizAnswers(prev => ({ ...prev, [currentQuestionIndex]: newAnswers }));
                    }}
                    className="w-4 h-4 text-[#6B9080] focus:ring-[#6B9080]"
                  />
                  <span className="text-[#2C3E3F]">{option}</span>
                </label>
              ))
            ) : (
              currentQuestion.options.map((option, index) => (
                <label key={index} className="flex items-center gap-3 p-3 rounded-lg border hover:bg-gray-50 cursor-pointer transition-all" style={{ borderColor: M.bg3 }}>
                  <input
                    type="radio"
                    name={`question-${currentQuestionIndex}`}
                    value={option}
                    checked={quizAnswers[currentQuestionIndex] === option}
                    onChange={(e) => setQuizAnswers(prev => ({ ...prev, [currentQuestionIndex]: e.target.value }))}
                    className="w-4 h-4 text-[#6B9080] focus:ring-[#6B9080]"
                  />
                  <span className="text-[#2C3E3F]">{option}</span>
                </label>
              ))
            )}
          </div>
        </div>

        <div className="flex justify-between items-center">
          <button
            onClick={prevQuestion}
            disabled={currentQuestionIndex === 0}
            className={`px-6 py-3 rounded-lg font-medium flex items-center gap-2 transition-all ${
              currentQuestionIndex === 0
                ? 'bg-gray-200 text-gray-400 cursor-not-allowed'
                : 'text-white hover:shadow-lg'
            }`}
            style={currentQuestionIndex === 0 ? {} : { background: M.secondary }}
          >
            <ChevronLeft className="w-4 h-4" />
            Previous
          </button>

          {isLastQuestion ? (
            <button
              onClick={submitQuiz}
              className="px-8 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center gap-2"
              style={{ background: M.primary }}
            >
              Submit Assessment
              <CheckSquare className="w-4 h-4" />
            </button>
          ) : (
            <button
              onClick={() => {
                if (isFirstQuestion || hasAnsweredFirstQuestion) {
                  nextQuestion();
                } else {
                  alert('Please answer the first question before proceeding.');
                }
              }}
              disabled={
                currentQuestion.type === "open-ended"
                  ? !quizAnswers[currentQuestionIndex] || quizAnswers[currentQuestionIndex].trim() === ''
                  : currentQuestion.type === "multiple-choice"
                  ? !quizAnswers[currentQuestionIndex] || quizAnswers[currentQuestionIndex].length === 0
                  : !quizAnswers[currentQuestionIndex]
              }
              className={`px-6 py-3 rounded-lg font-medium flex items-center gap-2 transition-all ${
                (currentQuestion.type === "open-ended"
                  ? !quizAnswers[currentQuestionIndex] || quizAnswers[currentQuestionIndex].trim() === ''
                  : currentQuestion.type === "multiple-choice"
                  ? !quizAnswers[currentQuestionIndex] || quizAnswers[currentQuestionIndex].length === 0
                  : !quizAnswers[currentQuestionIndex])
                  ? 'bg-gray-200 text-gray-400 cursor-not-allowed'
                  : 'text-white hover:shadow-lg'
              }`}
              style={
                (currentQuestion.type === "open-ended"
                  ? !quizAnswers[currentQuestionIndex] || quizAnswers[currentQuestionIndex].trim() === ''
                  : currentQuestion.type === "multiple-choice"
                  ? !quizAnswers[currentQuestionIndex] || quizAnswers[currentQuestionIndex].length === 0
                  : !quizAnswers[currentQuestionIndex])
                  ? {}
                  : { background: M.primary }
              }
            >
              Next
              <ChevronRight className="w-4 h-4" />
            </button>
          )}
        </div>
      </main>
    </div>
  );
}}