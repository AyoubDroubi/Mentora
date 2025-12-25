import React, { useState } from "react";
import { Mail, ArrowLeft, GraduationCap } from "lucide-react";

export default function ForgotPassword() {
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const isValidEmail = (email) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

  const goBack = () => alert("Back to Home");
  const goToLogin = () => alert("Go to Login");

  const handleSendResetLink = (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    if (!isValidEmail(email)) {
      setError("Please enter a valid email address");
      return;
    }

    setLoading(true);

    setTimeout(() => {
      setMessage("Reset link sent to your email! (Demo)");
      setLoading(false);
    }, 1000);
  };

  return (
    <div className="min-h-screen bg-[#F6FFF8] flex items-center justify-center p-4 text-[#2C3E3F] relative">
      <button
        onClick={goBack}
        className="absolute top-6 left-6 flex items-center gap-2 text-[#6B9080] hover:text-[#A4C3B2]"
      >
        <ArrowLeft className="w-5 h-5" />
        <span>Back to Home</span>
      </button>

      <div className="w-full max-w-md relative">
        <div className="flex items-center justify-center mb-6">
          <div className="w-10 h-10 bg-gradient-to-br from-[#6B9080] to-[#A4C3B2] rounded-lg flex items-center justify-center">
            <GraduationCap className="w-6 h-6 text-white" />
          </div>
          <span className="ml-2 text-2xl font-semibold">Mentora</span>
        </div>

        <div className="bg-white border-2 border-[#A4C3B2] rounded-2xl shadow-xl p-8">
          <h2 className="text-3xl text-center mb-2">Reset Password</h2>
          <p className="text-[#6B9080] text-center mb-6">
            Enter your email to receive a reset link
          </p>

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
                  className="pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none"
                  disabled={loading}
                />
              </div>
              {error && <p className="text-red-500 text-sm mt-1">{error}</p>}
              {message && <p className="text-green-600 text-sm mt-1">{message}</p>}
            </div>

            <button
              type="submit"
              disabled={loading || !email.trim()}
              className={`w-full py-3 rounded-xl font-medium shadow-lg ${
                loading || !email.trim()
                  ? "bg-gray-300 text-gray-600 cursor-not-allowed"
                  : "bg-[#6B9080] hover:bg-[#577466] text-white"
              }`}
            >
              {loading ? "Sending..." : "Send Reset Link"}
            </button>
          </form>

          <div className="mt-6 text-center">
            <p className="text-[#6B9080]">
              Remembered your password?{" "}
              <button onClick={goToLogin} className="text-[#2C3E3F] hover:text-[#6B9080] font-medium">
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
