import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import StudyPlanner from "./pages/StudyPlanner";
import Login from "./pages/Login";
import SignUp from "./pages/SignUp";
import Dashboard from "./pages/Dashboard";
import ForgotPassword from "./pages/ForgotPassword";
import Profile from "./pages/Profile";
import Home from "./pages/Home";
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
import AboutUs from "./pages/AboutUs";
import Quiz from "./pages/Quiz";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<SignUp />} />
        <Route path="/study-planner" element={<StudyPlanner />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/todo" element={<Todo />} />
        <Route path="/timer" element={<Timer />} />
        <Route path="/notes" element={<Notes />} />
        <Route path="/planner" element={<Planner />} />
        <Route path="/attendance" element={<Attendance />} />
        <Route path="/career-builder" element={<CareerBuilder />} />
        <Route path="/create-career-builder" element={<CreateCareerBuilder />} />
        <Route path="/career-plan" element={<CareerPlan />} />
        <Route path="/career-progress" element={<CareerProgress />} />
        <Route path="/career-skills" element={<CareerSkills />} />
        <Route path="/about-us" element={<AboutUs />} />
        <Route path="/quiz" element={<Quiz />} />
      </Routes>
    </BrowserRouter>
  );
}
