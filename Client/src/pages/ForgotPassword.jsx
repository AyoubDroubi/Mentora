import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Mail, ArrowLeft, GraduationCap, CheckCircle, AlertCircle } from "lucide-react";
import authService from "../services/authService";

export default function ForgotPassword() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const isValidEmail = (email) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

  const goBack = () => {
    navigate("/");
  };

  const goToLogin = () => {
    navigate("/login");
  };

  const handleSendResetLink = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    if (!isValidEmail(email)) {
      setError("Please enter a valid email address");
      return;
    }

    setLoading(true);

    try {
      const result = await authService.forgotPassword(email);
      
      if (result.success) {
        setMessage(result.message || "If the email exists, a password reset link has been sent");
        setEmail(""); // Clear email field
      } else {
        setError(result.message || "Failed to send reset link");
      }
    } catch (err) {
      console.error('Forgot password error:', err);
      setError("An unexpected error occurred. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-[#F6FFF8] flex items-center justify-center p-4 text-[#2C3E3F] relative">
      <button
        onClick={goBack}
        disabled={loading}
        className="absolute top-6 left-6 flex items-center gap-2 text-[#6B9080] hover:text-[#A4C3B2] transition-colors disabled:opacity-50"
      >
        <ArrowLeft className="w-5 h-5" />
        <span>Back to Home</span>
      </button>

      <div className="w-full max-w-md relative">
        <div className="flex items-center justify-center mb-6">
          <div className="w-10 h-10 bg-gradient-to-br from-[#6B9080] to-[#A4C3B2] rounded-lg flex items-center justify-center shadow-md">
            <GraduationCap className="w-6 h-6 text-white" />
          </div>
          <span className="ml-2 text-2xl font-semibold">Mentora</span>
        </div>

        <div className="bg-white border border-[#A4C3B2] rounded-2xl shadow-xl p-8">
          <h2 className="text-3xl text-center mb-2">Reset Password</h2>
          <p className="text-[#6B9080] text-center mb-6">
            Enter your email to receive a reset link
          </p>

          {/* Success Message */}
          {message && (
            <div className="mb-4 p-3 bg-green-50 border border-green-200 rounded-lg flex items-center gap-2 text-green-700">
              <CheckCircle className="w-5 h-5 flex-shrink-0" />
              <span className="text-sm">{message}</span>
            </div>
          )}

          {/* Error Message */}
          {error && (
            <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded-lg flex items-center gap-2 text-red-700">
              <AlertCircle className="w-5 h-5 flex-shrink-0" />
              <span className="text-sm">{error}</span>
            </div>
          )}

          <form className="space-y-6" onSubmit={handleSendResetLink}>
            <div className="space-y-2">
              <label htmlFor="email" className="text-sm text-[#6B9080] block">
                Email
              </label>
              <div className="relative">
                <Mail className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-[#6B9080]" />
                <input
                  id="email"
                  type="email"
                  placeholder="your.email@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#6B9080]/40 focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none disabled:opacity-50 disabled:cursor-not-allowed"
                  disabled={loading}
                  required
                />
              </div>
            </div>

            <button
              type="submit"
              disabled={loading || !email.trim()}
              className="w-full py-3 rounded-xl font-medium shadow-lg transition disabled:opacity-50 disabled:cursor-not-allowed bg-[#6B9080] hover:bg-[#577466] text-white flex items-center justify-center gap-2"
            >
              {loading ? (
                <>
                  <svg
                    className="animate-spin h-5 w-5 text-white"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <circle
                      className="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      strokeWidth="4"
                    ></circle>
                    <path
                      className="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                    ></path>
                  </svg>
                  <span>Sending...</span>
                </>
              ) : (
                "Send Reset Link"
              )}
            </button>
          </form>

          <div className="mt-6 text-center">
            <p className="text-[#6B9080]">
              Remembered your password?{" "}
              <button
                type="button"
                onClick={goToLogin}
                disabled={loading}
                className="text-[#2C3E3F] hover:text-[#6B9080] font-medium disabled:opacity-50"
              >
                Login
              </button>
            </p>
          </div>
        </div>

        <div className="absolute top-20 left-10 w-20 h-20 bg-[#A4C3B2]/40 rounded-full blur-xl" />
        <div className="absolute bottom-20 right-10 w-32 h-32 bg-[#6B9080]/30 rounded-full blur-xl" />
      </div>
    </div>
  );
}
