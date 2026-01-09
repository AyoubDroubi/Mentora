import React, { createContext, useContext, useState, useCallback } from 'react';
import PropTypes from 'prop-types';
import * as assessmentService from '../services/assessmentService';

const AssessmentContext = createContext();

export const useAssessment = () => {
  const context = useContext(AssessmentContext);
  if (!context) {
    throw new Error('useAssessment must be used within an AssessmentProvider');
  }
  return context;
};

export const AssessmentProvider = ({ children }) => {
  // State Management
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  
  // Assessment State
  const [currentAttempt, setCurrentAttempt] = useState(null);
  const [questions, setQuestions] = useState([]);
  const [responses, setResponses] = useState({});
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  
  // Study Plans State
  const [studyPlans, setStudyPlans] = useState([]);
  const [currentStudyPlan, setCurrentStudyPlan] = useState(null);
  const [planProgress, setPlanProgress] = useState(null);

  // ============================================
  // Assessment Questions
  // ============================================

  const loadQuestions = useCallback(async (targetMajor = null) => {
    try {
      setLoading(true);
      setError(null);
      const data = await assessmentService.getAssessmentQuestions(targetMajor);
      setQuestions(data);
      return data;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load questions');
      throw err;
    } finally {
      setLoading(false);
    }
  }, []);

  // ============================================
  // Assessment Attempt
  // ============================================

  const startNewAssessment = useCallback(async (data) => {
    try {
      setLoading(true);
      setError(null);
      const attempt = await assessmentService.startAssessment(data);
      setCurrentAttempt(attempt);
      setResponses({});
      setCurrentQuestionIndex(0);
      
      // Load questions for the selected major
      await loadQuestions(data.major);
      
      return attempt;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to start assessment');
      throw err;
    } finally {
      setLoading(false);
    }
  }, [loadQuestions]);

  const saveResponse = useCallback((questionId, responseValue, responseTimeSeconds = null, notes = null) => {
    setResponses(prev => ({
      ...prev,
      [questionId]: {
        questionId,
        responseValue,
        responseTimeSeconds,
        notes,
        isSkipped: false
      }
    }));
  }, []);

  const skipQuestion = useCallback((questionId) => {
    setResponses(prev => ({
      ...prev,
      [questionId]: {
        questionId,
        responseValue: '',
        isSkipped: true
      }
    }));
  }, []);

  const submitResponses = useCallback(async () => {
    if (!currentAttempt) {
      throw new Error('No active assessment attempt');
    }

    try {
      setLoading(true);
      setError(null);
      
      const responsesArray = Object.values(responses);
      const result = await assessmentService.submitAssessmentResponses(
        currentAttempt.id,
        responsesArray
      );
      
      setCurrentAttempt(result);
      return result;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to submit responses');
      throw err;
    } finally {
      setLoading(false);
    }
  }, [currentAttempt, responses]);

  const completeCurrentAssessment = useCallback(async () => {
    if (!currentAttempt) {
      throw new Error('No active assessment attempt');
    }

    try {
      setLoading(true);
      setError(null);
      
      const result = await assessmentService.completeAssessment(currentAttempt.id);
      setCurrentAttempt(result);
      
      // Add generated study plan to list
      if (result.studyPlan) {
        setStudyPlans(prev => [result.studyPlan, ...prev]);
        setCurrentStudyPlan(result.studyPlan);
      }
      
      return result;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to complete assessment');
      throw err;
    } finally {
      setLoading(false);
    }
  }, [currentAttempt]);

  const loadUserAttempts = useCallback(async (status = null) => {
    try {
      setLoading(true);
      setError(null);
      const attempts = await assessmentService.getUserAssessmentAttempts(status);
      return attempts;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load attempts');
      throw err;
    } finally {
      setLoading(false);
    }
  }, []);

  const loadAttempt = useCallback(async (attemptId) => {
    try {
      setLoading(true);
      setError(null);
      const attempt = await assessmentService.getAssessmentAttempt(attemptId);
      setCurrentAttempt(attempt);
      
      // Rebuild responses from saved data
      if (attempt.responses) {
        const responsesMap = {};
        attempt.responses.forEach(r => {
          responsesMap[r.questionId] = r;
        });
        setResponses(responsesMap);
      }
      
      return attempt;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load attempt');
      throw err;
    } finally {
      setLoading(false);
    }
  }, []);

  // ============================================
  // Study Plans
  // ============================================

  const loadUserStudyPlans = useCallback(async (status = null) => {
    try {
      setLoading(true);
      setError(null);
      const plans = await assessmentService.getUserStudyPlans(status);
      setStudyPlans(plans);
      return plans;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load study plans');
      throw err;
    } finally {
      setLoading(false);
    }
  }, []);

  const loadStudyPlan = useCallback(async (planId) => {
    try {
      setLoading(true);
      setError(null);
      const plan = await assessmentService.getStudyPlan(planId);
      setCurrentStudyPlan(plan);
      
      // Load progress
      const progress = await assessmentService.getStudyPlanProgress(planId);
      setPlanProgress(progress);
      
      return plan;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load study plan');
      throw err;
    } finally {
      setLoading(false);
    }
  }, []);

  const updatePlanStatus = useCallback(async (planId, status) => {
    try {
      setLoading(true);
      setError(null);
      const updated = await assessmentService.updateStudyPlanStatus(planId, status);
      
      // Update current plan if it's the same
      if (currentStudyPlan?.id === planId) {
        setCurrentStudyPlan(updated);
      }
      
      // Update in list
      setStudyPlans(prev => 
        prev.map(p => p.id === planId ? updated : p)
      );
      
      return updated;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to update plan status');
      throw err;
    } finally {
      setLoading(false);
    }
  }, [currentStudyPlan]);

  const updatePlanStepStatus = useCallback(async (planId, stepId, status) => {
    try {
      setLoading(true);
      setError(null);
      const updated = await assessmentService.updateStepStatus(planId, stepId, status);
      
      // Refresh current plan
      if (currentStudyPlan?.id === planId) {
        await loadStudyPlan(planId);
      }
      
      return updated;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to update step status');
      throw err;
    } finally {
      setLoading(false);
    }
  }, [currentStudyPlan, loadStudyPlan]);

  const completeCheckpoint = useCallback(async (planId, checkpointId) => {
    try {
      setLoading(true);
      setError(null);
      const updated = await assessmentService.markCheckpointCompleted(planId, checkpointId);
      
      // Refresh current plan
      if (currentStudyPlan?.id === planId) {
        await loadStudyPlan(planId);
      }
      
      return updated;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to complete checkpoint');
      throw err;
    } finally {
      setLoading(false);
    }
  }, [currentStudyPlan, loadStudyPlan]);

  const completeResource = useCallback(async (planId, resourceId, rating = null, notes = null) => {
    try {
      setLoading(true);
      setError(null);
      const data = {};
      if (rating) data.userRating = rating;
      if (notes) data.userNotes = notes;
      
      const updated = await assessmentService.markResourceCompleted(planId, resourceId, data);
      
      // Refresh current plan
      if (currentStudyPlan?.id === planId) {
        await loadStudyPlan(planId);
      }
      
      return updated;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to complete resource');
      throw err;
    } finally {
      setLoading(false);
    }
  }, [currentStudyPlan, loadStudyPlan]);

  // ============================================
  // Navigation Helpers
  // ============================================

  const goToNextQuestion = useCallback(() => {
    if (currentQuestionIndex < questions.length - 1) {
      setCurrentQuestionIndex(prev => prev + 1);
    }
  }, [currentQuestionIndex, questions.length]);

  const goToPreviousQuestion = useCallback(() => {
    if (currentQuestionIndex > 0) {
      setCurrentQuestionIndex(prev => prev - 1);
    }
  }, [currentQuestionIndex]);

  const goToQuestion = useCallback((index) => {
    if (index >= 0 && index < questions.length) {
      setCurrentQuestionIndex(index);
    }
  }, [questions.length]);

  // ============================================
  // Computed Values
  // ============================================

  const assessmentProgress = React.useMemo(() => {
    const answeredCount = Object.keys(responses).length;
    return assessmentService.calculateAssessmentProgress(answeredCount, questions.length);
  }, [responses, questions.length]);

  const isLastQuestion = React.useMemo(() => {
    return currentQuestionIndex === questions.length - 1;
  }, [currentQuestionIndex, questions.length]);

  const isFirstQuestion = React.useMemo(() => {
    return currentQuestionIndex === 0;
  }, [currentQuestionIndex]);

  const currentQuestion = React.useMemo(() => {
    return questions[currentQuestionIndex] || null;
  }, [questions, currentQuestionIndex]);

  const currentResponse = React.useMemo(() => {
    return currentQuestion ? responses[currentQuestion.id] : null;
  }, [currentQuestion, responses]);

  // ============================================
  // Reset Functions
  // ============================================

  const resetAssessment = useCallback(() => {
    setCurrentAttempt(null);
    setQuestions([]);
    setResponses({});
    setCurrentQuestionIndex(0);
    setError(null);
  }, []);

  const resetStudyPlan = useCallback(() => {
    setCurrentStudyPlan(null);
    setPlanProgress(null);
    setError(null);
  }, []);

  const clearError = useCallback(() => {
    setError(null);
  }, []);

  // ============================================
  // Context Value
  // ============================================

  const value = {
    // State
    loading,
    error,
    currentAttempt,
    questions,
    responses,
    currentQuestionIndex,
    studyPlans,
    currentStudyPlan,
    planProgress,
    
    // Assessment Methods
    loadQuestions,
    startNewAssessment,
    saveResponse,
    skipQuestion,
    submitResponses,
    completeCurrentAssessment,
    loadUserAttempts,
    loadAttempt,
    
    // Study Plan Methods
    loadUserStudyPlans,
    loadStudyPlan,
    updatePlanStatus,
    updatePlanStepStatus,
    completeCheckpoint,
    completeResource,
    
    // Navigation
    goToNextQuestion,
    goToPreviousQuestion,
    goToQuestion,
    
    // Computed
    assessmentProgress,
    isLastQuestion,
    isFirstQuestion,
    currentQuestion,
    currentResponse,
    
    // Reset
    resetAssessment,
    resetStudyPlan,
    clearError,
  };

  return (
    <AssessmentContext.Provider value={value}>
      {children}
    </AssessmentContext.Provider>
  );
};

AssessmentProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

export default AssessmentContext;
