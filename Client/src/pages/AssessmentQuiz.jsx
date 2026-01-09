import React, { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAssessment } from '../contexts/AssessmentContext';
import SharedHeader from '../components/SharedHeader';
import LoadingSpinner from '../components/LoadingSpinner';
import { 
  AcademicCapIcon, 
  CheckCircleIcon, 
  ClockIcon, 
  ArrowLeftIcon, 
  ArrowRightIcon,
  ChartBarIcon
} from '@heroicons/react/24/outline';

export default function AssessmentQuiz() {
  const navigate = useNavigate();
  const {
    loading,
    error,
    currentAttempt,
    questions,
    currentQuestion,
    currentResponse,
    currentQuestionIndex,
    assessmentProgress,
    isFirstQuestion,
    isLastQuestion,
    startNewAssessment,
    loadQuestions,
    saveResponse,
    skipQuestion,
    submitResponses,
    completeCurrentAssessment,
    goToNextQuestion,
    goToPreviousQuestion,
    goToQuestion,
    clearError,
  } = useAssessment();

  // Local State
  const [assessmentStarted, setAssessmentStarted] = useState(false);
  const [selectedValue, setSelectedValue] = useState('');
  const [startTime, setStartTime] = useState(null);
  const [notes, setNotes] = useState('');
  const [showStartForm, setShowStartForm] = useState(true);
  
  // Form State
  const [major, setMajor] = useState('');
  const [studyLevel, setStudyLevel] = useState('');

  // Available Options
  const majors = [
    { value: 'ComputerScience', label: 'Computer Science' },
    { value: 'Engineering', label: 'Engineering' },
    { value: 'Business', label: 'Business' },
    { value: 'Medicine', label: 'Medicine' },
    { value: 'Arts', label: 'Arts' },
  ];

  const studyLevels = [
    { value: 'Freshman', label: 'Freshman (Year 1)' },
    { value: 'Sophomore', label: 'Sophomore (Year 2)' },
    { value: 'Junior', label: 'Junior (Year 3)' },
    { value: 'Senior', label: 'Senior (Year 4)' },
  ];

  // Initialize current response
  useEffect(() => {
    if (currentResponse) {
      setSelectedValue(currentResponse.responseValue || '');
      setNotes(currentResponse.notes || '');
    } else {
      setSelectedValue('');
      setNotes('');
    }
    setStartTime(Date.now());
  }, [currentQuestionIndex, currentResponse]);

  // Start Assessment
  const handleStartAssessment = async () => {
    if (!major || !studyLevel) {
      alert('Please select both major and study level');
      return;
    }

    try {
      await startNewAssessment({ major, studyLevel });
      setShowStartForm(false);
      setAssessmentStarted(true);
    } catch (err) {
      console.error('Failed to start assessment:', err);
    }
  };

  // Save Current Answer
  const handleSaveAnswer = useCallback(() => {
    if (!currentQuestion || !selectedValue.trim()) return;

    const responseTimeSeconds = startTime ? Math.floor((Date.now() - startTime) / 1000) : null;
    
    saveResponse(
      currentQuestion.id,
      selectedValue,
      responseTimeSeconds,
      notes || null
    );
  }, [currentQuestion, selectedValue, startTime, notes, saveResponse]);

  // Handle Next Question
  const handleNext = async () => {
    handleSaveAnswer();

    if (isLastQuestion) {
      // Submit all responses and complete assessment
      try {
        await submitResponses();
        await completeCurrentAssessment();
        
        // Navigate to study plan generation page
        navigate('/study-plan-generated');
      } catch (err) {
        console.error('Failed to complete assessment:', err);
      }
    } else {
      goToNextQuestion();
    }
  };

  // Handle Previous Question
  const handlePrevious = () => {
    handleSaveAnswer();
    goToPreviousQuestion();
  };

  // Handle Skip Question
  const handleSkip = () => {
    if (!currentQuestion) return;
    
    skipQuestion(currentQuestion.id);
    
    if (!isLastQuestion) {
      goToNextQuestion();
    }
  };

  // Handle Option Selection
  const handleOptionSelect = (value) => {
    setSelectedValue(value);
  };

  // Render Question Based on Type
  const renderQuestionInput = () => {
    if (!currentQuestion) return null;

    switch (currentQuestion.questionType) {
      case 'MultipleChoice':
      case 'SingleChoice':
        return (
          <div className="space-y-3">
            {currentQuestion.options?.map((option, index) => (
              <label
                key={index}
                className={`flex items-center p-4 border-2 rounded-lg cursor-pointer transition-all ${
                  selectedValue === option
                    ? 'border-indigo-600 bg-indigo-50'
                    : 'border-gray-200 hover:border-indigo-300 bg-white'
                }`}
              >
                <input
                  type="radio"
                  name="answer"
                  value={option}
                  checked={selectedValue === option}
                  onChange={(e) => handleOptionSelect(e.target.value)}
                  className="w-5 h-5 text-indigo-600 focus:ring-indigo-500"
                />
                <span className="ml-3 text-gray-900">{option}</span>
              </label>
            ))}
          </div>
        );

      case 'Scale':
        return (
          <div className="space-y-4">
            <div className="flex justify-between text-sm text-gray-600">
              <span>{currentQuestion.minValue || 1}</span>
              <span>{currentQuestion.maxValue || 10}</span>
            </div>
            <input
              type="range"
              min={currentQuestion.minValue || 1}
              max={currentQuestion.maxValue || 10}
              value={selectedValue || currentQuestion.minValue || 1}
              onChange={(e) => handleOptionSelect(e.target.value)}
              className="w-full h-2 bg-gray-200 rounded-lg appearance-none cursor-pointer accent-indigo-600"
            />
            <div className="text-center">
              <span className="text-3xl font-bold text-indigo-600">{selectedValue || currentQuestion.minValue || 1}</span>
            </div>
          </div>
        );

      case 'Text':
        return (
          <textarea
            value={selectedValue}
            onChange={(e) => handleOptionSelect(e.target.value)}
            rows={4}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-600 focus:border-transparent"
            placeholder="Type your answer here..."
          />
        );

      case 'Number':
        return (
          <input
            type="number"
            value={selectedValue}
            onChange={(e) => handleOptionSelect(e.target.value)}
            min={currentQuestion.minValue}
            max={currentQuestion.maxValue}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-600 focus:border-transparent"
            placeholder="Enter a number..."
          />
        );

      default:
        return null;
    }
  };

  // Show Start Form
  if (showStartForm) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-indigo-50 via-white to-purple-50">
        <SharedHeader />
        
        <div className="max-w-2xl mx-auto px-4 py-12">
          <div className="bg-white rounded-2xl shadow-xl p-8">
            {/* Header */}
            <div className="text-center mb-8">
              <div className="flex justify-center mb-4">
                <div className="p-4 bg-indigo-100 rounded-full">
                  <AcademicCapIcon className="w-12 h-12 text-indigo-600" />
                </div>
              </div>
              <h1 className="text-3xl font-bold text-gray-900 mb-2">
                Student Assessment
              </h1>
              <p className="text-gray-600">
                Let's create a personalized study plan based on your profile
              </p>
            </div>

            {/* Form */}
            <div className="space-y-6">
              {/* Major Selection */}
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Select Your Major *
                </label>
                <select
                  value={major}
                  onChange={(e) => setMajor(e.target.value)}
                  className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-600 focus:border-transparent"
                  required
                >
                  <option value="">Choose your major...</option>
                  {majors.map((m) => (
                    <option key={m.value} value={m.value}>
                      {m.label}
                    </option>
                  ))}
                </select>
              </div>

              {/* Study Level Selection */}
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Current Study Level *
                </label>
                <select
                  value={studyLevel}
                  onChange={(e) => setStudyLevel(e.target.value)}
                  className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-600 focus:border-transparent"
                  required
                >
                  <option value="">Choose your level...</option>
                  {studyLevels.map((level) => (
                    <option key={level.value} value={level.value}>
                      {level.label}
                    </option>
                  ))}
                </select>
              </div>

              {/* Info Box */}
              <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
                <div className="flex items-start">
                  <ClockIcon className="w-5 h-5 text-blue-600 mt-0.5 mr-3 flex-shrink-0" />
                  <div className="text-sm text-blue-800">
                    <p className="font-medium mb-1">Assessment Details:</p>
                    <ul className="list-disc list-inside space-y-1">
                      <li>Duration: ~15-20 minutes</li>
                      <li>Questions: 15-20 questions</li>
                      <li>You can skip questions if needed</li>
                      <li>AI-powered study plan will be generated</li>
                    </ul>
                  </div>
                </div>
              </div>

              {/* Error Message */}
              {error && (
                <div className="bg-red-50 border border-red-200 rounded-lg p-4 text-red-800 text-sm">
                  {error}
                </div>
              )}

              {/* Start Button */}
              <button
                onClick={handleStartAssessment}
                disabled={loading || !major || !studyLevel}
                className="w-full py-4 bg-indigo-600 text-white font-medium rounded-lg hover:bg-indigo-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors flex items-center justify-center"
              >
                {loading ? (
                  <LoadingSpinner size="sm" />
                ) : (
                  <>
                    <AcademicCapIcon className="w-5 h-5 mr-2" />
                    Start Assessment
                  </>
                )}
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Show Loading
  if (loading && !currentQuestion) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-indigo-50 via-white to-purple-50">
        <SharedHeader />
        <div className="flex items-center justify-center h-[calc(100vh-80px)]">
          <LoadingSpinner />
        </div>
      </div>
    );
  }

  // Show Assessment Questions
  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-50 via-white to-purple-50">
      <SharedHeader />

      <div className="max-w-4xl mx-auto px-4 py-8">
        {/* Progress Bar */}
        <div className="mb-8">
          <div className="flex items-center justify-between mb-2">
            <span className="text-sm font-medium text-gray-700">
              Question {currentQuestionIndex + 1} of {questions.length}
            </span>
            <span className="text-sm font-medium text-indigo-600">
              {assessmentProgress}% Complete
            </span>
          </div>
          <div className="w-full bg-gray-200 rounded-full h-3">
            <div
              className="bg-gradient-to-r from-indigo-600 to-purple-600 h-3 rounded-full transition-all duration-300"
              style={{ width: `${assessmentProgress}%` }}
            />
          </div>
        </div>

        {/* Question Card */}
        {currentQuestion && (
          <div className="bg-white rounded-2xl shadow-xl p-8 mb-6">
            {/* Question Header */}
            <div className="mb-6">
              <div className="flex items-start justify-between mb-4">
                <div className="flex-1">
                  <span className="inline-block px-3 py-1 bg-indigo-100 text-indigo-800 text-xs font-semibold rounded-full mb-3">
                    {currentQuestion.category}
                  </span>
                  <h2 className="text-2xl font-bold text-gray-900 mb-2">
                    {currentQuestion.questionText}
                  </h2>
                  {currentQuestion.helpText && (
                    <p className="text-sm text-gray-600 mt-2">
                      {currentQuestion.helpText}
                    </p>
                  )}
                </div>
                {currentQuestion.isRequired && (
                  <span className="text-red-500 text-xl font-bold ml-4">*</span>
                )}
              </div>
            </div>

            {/* Question Input */}
            <div className="mb-6">
              {renderQuestionInput()}
            </div>

            {/* Optional Notes */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Additional Notes (Optional)
              </label>
              <textarea
                value={notes}
                onChange={(e) => setNotes(e.target.value)}
                rows={2}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-600 focus:border-transparent text-sm"
                placeholder="Any additional thoughts or context..."
              />
            </div>
          </div>
        )}

        {/* Navigation Buttons */}
        <div className="flex items-center justify-between">
          <button
            onClick={handlePrevious}
            disabled={isFirstQuestion}
            className="flex items-center px-6 py-3 text-gray-700 bg-white rounded-lg border-2 border-gray-300 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
          >
            <ArrowLeftIcon className="w-5 h-5 mr-2" />
            Previous
          </button>

          <button
            onClick={handleSkip}
            className="px-6 py-3 text-gray-600 bg-white rounded-lg border-2 border-gray-300 hover:bg-gray-50 transition-colors"
          >
            Skip Question
          </button>

          <button
            onClick={handleNext}
            disabled={currentQuestion?.isRequired && !selectedValue.trim()}
            className="flex items-center px-6 py-3 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
          >
            {isLastQuestion ? (
              <>
                <CheckCircleIcon className="w-5 h-5 mr-2" />
                Complete Assessment
              </>
            ) : (
              <>
                Next
                <ArrowRightIcon className="w-5 h-5 ml-2" />
              </>
            )}
          </button>
        </div>

        {/* Error Message */}
        {error && (
          <div className="mt-6 bg-red-50 border border-red-200 rounded-lg p-4 text-red-800 text-sm">
            {error}
          </div>
        )}
      </div>
    </div>
  );
}
