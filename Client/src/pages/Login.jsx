import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FiMail, FiLock, FiArrowLeft, FiAlertCircle } from "react-icons/fi";
import { GraduationCap } from "lucide-react";
import { useAuth } from "../contexts/AuthContext";

export default function Login() {
  const navigate = useNavigate();
  const { login, loading } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [rememberMe, setRememberMe] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    // Basic validation
    if (!email || !password) {
      setError("Please enter both email and password");
      return;
    }

    // Email format validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      setError("Please enter a valid email address");
      return;
    }

    try {
      const result = await login(email, password);
      
      if (result.success) {
        // Successful login - redirect to dashboard
        navigate('/dashboard');
      } else {
        // Login failed - show error
        setError(result.message || 'Invalid email or password');
      }
    } catch (err) {
      console.error('Login error:', err);
      setError('An unexpected error occurred. Please try again.');
    }
  };

  const goBack = () => {
    navigate("/");
  };

  const goToSignup = () => {
    navigate("/signup");
  };

  const goToForgotPassword = () => {
    navigate("/forgot-password");
  };

  return (
    <div className="min-h-screen bg-[#F6FFF8] flex items-center justify-center p-4 relative text-[#2C3E3F]">
      {/* Back Button */}
      <button
        onClick={goBack}
        className="absolute top-6 left-6 text-[#6B9080] hover:text-[#A4C3B2] flex items-center gap-2 transition-colors"
        disabled={loading}
      >
        <FiArrowLeft className="w-5 h-5" />
        <span>Back to Home</span>
      </button>

      <div className="w-full max-w-md relative">
        {/* Logo */}
        <div className="flex items-center justify-center mb-6 relative z-10">
          <div className="w-10 h-10 bg-gradient-to-br from-[#6B9080] to-[#A4C3B2] rounded-lg flex items-center justify-center shadow-md">
            <GraduationCap className="w-6 h-6 text-white" />
          </div>
          <span className="text-[#2C3E3F] text-2xl font-semibold ml-2">Mentora</span>
        </div>

        {/* Card */}
        <div className="bg-white border border-[#A4C3B2] shadow-xl rounded-2xl p-8">
          <h2 className="text-[#2C3E3F] text-3xl mb-2 text-center">Welcome Back</h2>
          <p className="text-[#6B9080] text-center mb-6">Login to continue your journey</p>

          {/* Error Message */}
          {error && (
            <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded-lg flex items-center gap-2 text-red-700">
              <FiAlertCircle className="w-5 h-5 flex-shrink-0" />
              <span className="text-sm">{error}</span>
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Email */}
            <div className="space-y-2">
              <label htmlFor="email" className="text-sm text-[#6B9080] mb-2 block">
                Email
              </label>
              <div className="relative">
                <FiMail className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-[#6B9080]" />
                <input
                  id="email"
                  type="email"
                  placeholder="your.email@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                  disabled={loading}
                  className="pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#6B9080]/40 focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none disabled:opacity-50 disabled:cursor-not-allowed"
                />
              </div>
            </div>

            {/* Password */}
            <div className="space-y-2">
              <label htmlFor="password" className="text-sm text-[#6B9080] mb-2 block">
                Password
              </label>
              <div className="relative">
                <FiLock className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-[#6B9080]" />
                <input
                  id="password"
                  type="password"
                  placeholder="••••••••"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                  disabled={loading}
                  className="pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#6B9080]/40 focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none disabled:opacity-50 disabled:cursor-not-allowed"
                />
              </div>
            </div>

            {/* Remember & Forgot */}
            <div className="flex items-center justify-between text-sm">
              <label className="flex items-center gap-2 text-[#6B9080] cursor-pointer">
                <input
                  type="checkbox"
                  checked={rememberMe}
                  onChange={(e) => setRememberMe(e.target.checked)}
                  disabled={loading}
                  className="rounded border-[#A4C3B2] disabled:opacity-50"
                />
                Remember me
              </label>
              <button
                type="button"
                onClick={goToForgotPassword}
                disabled={loading}
                className="text-[#6B9080] hover:text-[#2C3E3F] disabled:opacity-50"
              >
                Forgot Password?
              </button>
            </div>

            {/* Submit */}
            <button
              type="submit"
              disabled={loading}
              className="w-full bg-[#6B9080] hover:bg-[#577466] text-white py-3 rounded-xl font-medium shadow-lg transition disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
            >
              {loading ? (
                <>
                  <svg className="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                  </svg>
                  <span>Logging in...</span>
                </>
              ) : (
                'Login'
              )}
            </button>
          </form>

          {/* Signup Link */}
          <div className="mt-6 text-center">
            <p className="text-[#6B9080]">
              Don't have an account?{" "}
              <button
                type="button"
                onClick={goToSignup}
                disabled={loading}
                className="text-[#2C3E3F] hover:text-[#6B9080] font-medium disabled:opacity-50"
              >
                Sign Up
              </button>
            </p>
          </div>
        </div>

        {/* Decorative circles */}
        <div className="absolute top-20 left-10 w-20 h-20 bg-[#A4C3B2]/40 rounded-full blur-xl" />
        <div className="absolute bottom-20 right-10 w-32 h-32 bg-[#6B9080]/30 rounded-full blur-xl" />
      </div>
    </div>
  );
}
