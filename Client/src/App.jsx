import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./contexts/AuthContext";
import { ProfileProvider } from "./contexts/ProfileContext";
import { UserProvider } from "./contexts/UserContext";
import ProtectedRoute from "./components/ProtectedRoute";

// Public pages
import Home from "./pages/Home";
import Login from "./pages/Login";
import SignUp from "./pages/SignUp";
import ForgotPassword from "./pages/ForgotPassword";
import AboutUs from "./pages/AboutUs";

// Protected pages
import Dashboard from "./pages/Dashboard";
import StudyPlanner from "./pages/StudyPlanner";
import Profile from "./pages/Profile";
import Todo from "./pages/Todo";
import Timer from "./pages/Timer";
import Notes from "./pages/Notes";
import Planner from "./pages/Planner";
import Attendance from "./pages/Attendance";
import CareerBuilder from "./pages/CareerBuilder";
import CreateCareerBuilder from "./pages/CreateCareerBuilder";
import CareerPlan from "./pages/CareerPlan";
import CareerProgress from "./pages/CareerProgress";
import CareerSkills from "./pages/CareerSkills";
import Quiz from "./pages/Quiz";
import TestAuth from "./pages/TestAuth";

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <UserProvider>
          <ProfileProvider>
            <Routes>
              {/* Public Routes */}
              <Route path="/" element={<Home />} />
              <Route path="/login" element={<Login />} />
              <Route path="/signup" element={<SignUp />} />
              <Route path="/forgot-password" element={<ForgotPassword />} />
              <Route path="/about-us" element={<AboutUs />} />

              {/* Test Auth Page - Protected */}
              <Route
                path="/test-auth"
                element={
                  <ProtectedRoute>
                    <TestAuth />
                  </ProtectedRoute>
                }
              />

              {/* Protected Routes */}
              <Route
                path="/dashboard"
                element={
                  <ProtectedRoute>
                    <Dashboard />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/study-planner"
                element={
                  <ProtectedRoute>
                    <StudyPlanner />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/profile"
                element={
                  <ProtectedRoute>
                    <Profile />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/todo"
                element={
                  <ProtectedRoute>
                    <Todo />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/timer"
                element={
                  <ProtectedRoute>
                    <Timer />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/notes"
                element={
                  <ProtectedRoute>
                    <Notes />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/planner"
                element={
                  <ProtectedRoute>
                    <Planner />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/attendance"
                element={
                  <ProtectedRoute>
                    <Attendance />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/career-builder"
                element={
                  <ProtectedRoute>
                    <CareerBuilder />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/create-career-builder"
                element={
                  <ProtectedRoute>
                    <CreateCareerBuilder />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/career-plan/:id"
                element={
                  <ProtectedRoute>
                    <CareerPlan />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/career-progress"
                element={
                  <ProtectedRoute>
                    <CareerProgress />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/career-skills"
                element={
                  <ProtectedRoute>
                    <CareerSkills />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/quiz"
                element={
                  <ProtectedRoute>
                    <Quiz />
                  </ProtectedRoute>
                }
              />
            </Routes>
          </ProfileProvider>
        </UserProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}
