import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FiMail, FiLock, FiUser, FiArrowLeft } from "react-icons/fi";
import { GraduationCap } from "lucide-react";

export default function SignUp() {
  const navigate = useNavigate();
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [agree, setAgree] = useState(false);
  const [errors, setErrors] = useState({});

  const handleSubmit = (e) => {
    e.preventDefault();
    const newErrors = {};

    if (!fullName) newErrors.fullName = "Full name is required";
    if (!email) newErrors.email = "Email is required";
    else if (!email.includes("@")) newErrors.email = "Email must include @";

    // Password validation
    if (!password) newErrors.password = "Password is required";
    else {
      const isValid =
        password.length >= 8 &&
        /[A-Z]/.test(password) &&
        /[a-z]/.test(password) &&
        /[0-9]/.test(password) &&
        /[^A-Za-z0-9]/.test(password);

      if (!isValid) {
        newErrors.password =
          "Password must be at least 8 characters, include uppercase, lowercase, number, and a special character.";
      }
    }

    if (!confirmPassword) newErrors.confirmPassword = "Confirm password is required";
    else if (password !== confirmPassword) newErrors.confirmPassword = "Passwords do not match";

    if (!agree) newErrors.agree = "You must agree to the terms";

    setErrors(newErrors);

    if (Object.keys(newErrors).length === 0) {
      navigate('/study-planner');
    }
  };

  const goBack = () => {
    window.location.href = "/";
  };

  const goToLogin = () => {
    window.location.href = "/login";
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-[#F6FFF8] via-[#EAF4F4] to-[#E8F3E8] flex items-center justify-center p-4 py-12 text-[#2C3E3F] overflow-y-auto">

      {/* Back Button */}
      <button
        onClick={goBack}
        className="absolute top-6 left-6 text-[#6B9080] hover:text-[#A4C3B2] flex items-center gap-2 transition-colors"
      >
        <FiArrowLeft className="w-5 h-5" />
        <span>Back to Home</span>
      </button>

      <div className="w-full max-w-md relative">

        {/* Mentora Logo Outside Card */}
        <div className="flex items-center justify-center mb-6">
          <div className="w-10 h-10 bg-gradient-to-br from-[#6B9080] to-[#A4C3B2] rounded-lg flex items-center justify-center shadow-md">
                      <GraduationCap className="w-6 h-6 text-white" /></div>
          <span className="text-[#2C3E3F] text-2xl font-semibold ml-2">Mentora</span>
        </div> 

        {/* SignUp Card */}
        <div className="bg-white border border-[#A4C3B2] rounded-2xl shadow-2xl p-8">

          <h2 className="text-[#2C3E3F] text-3xl mb-2 text-center">Create Account</h2>
          <p className="text-[#6B9080] text-center mb-6">Start your journey with Mentora</p>

          <form onSubmit={handleSubmit} className="space-y-4">

            {/* Full Name */}
            <div className="space-y-1">
              <label htmlFor="fullName" className="text-sm text-[#6B9080] block">Full Name</label>
              <div className="relative">
                <FiUser className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-[#6B9080]" />
                <input
                  id="fullName"
                  type="text"
                  placeholder="Name"
                  value={fullName}
                  onChange={(e) => setFullName(e.target.value)}
                  className={`pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#6B9080]/40 focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none ${errors.fullName ? "border-2 border-red-500" : ""}`}
                />
                {errors.fullName && <p className="text-red-500 text-sm mt-1">{errors.fullName}</p>}
              </div>
            </div>

            {/* Email */}
            <div className="space-y-1">
              <label htmlFor="email" className="text-sm text-[#6B9080] block">Email</label>
              <div className="relative">
                <FiMail className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-[#6B9080]" />
                <input
                  id="email"
                  type="email"
                  placeholder="your.email@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className={`pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#6B9080]/40 focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none ${errors.email ? "border-2 border-red-500" : ""}`}
                />
                {errors.email && <p className="text-red-500 text-sm mt-1">{errors.email}</p>}
              </div>
            </div>

            {/* Password */}
            <div className="space-y-1">
              <label htmlFor="password" className="text-sm text-[#6B9080] block">Password</label>
              <div className="relative">
                <FiLock className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-[#6B9080]" />
                <input
                  id="password"
                  type="password"
                  placeholder="••••••••"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className={`pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#6B9080]/40 focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none ${errors.password ? "border-2 border-red-500" : ""}`}
                />
                {errors.password && <p className="text-red-500 text-sm mt-1">{errors.password}</p>}
              </div>
            </div>

            {/* Confirm Password */}
            <div className="space-y-1">
              <label htmlFor="confirmPassword" className="text-sm text-[#6B9080] block">Confirm Password</label>
              <div className="relative">
                <FiLock className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-[#6B9080]" />
                <input
                  id="confirmPassword"
                  type="password"
                  placeholder="••••••••"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  className={`pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#6B9080]/40 focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none ${errors.confirmPassword ? "border-2 border-red-500" : ""}`}
                />
                {errors.confirmPassword && <p className="text-red-500 text-sm mt-1">{errors.confirmPassword}</p>}
              </div>
            </div>

            {/* Terms & Conditions */}
            <div className="flex items-center gap-2 text-sm">
              <input
                type="checkbox"
                checked={agree}
                onChange={(e) => setAgree(e.target.checked)}
                className="rounded border-[#A4C3B2] bg-[#F6FFF8]"
              />
              <label className="text-[#6B9080]">
                I agree to the{" "}
                <button type="button" className="text-[#6B9080] hover:text-[#2C3E3F]">
                  Terms & Conditions
                </button>
              </label>
            </div>
            {errors.agree && <p className="text-red-500 text-sm mt-1">{errors.agree}</p>}

            {/* Create Account Button */}
            <button
              type="submit"
              className="w-full bg-[#6B9080] hover:bg-[#577466] text-white py-3 rounded-xl font-medium shadow-lg transition"
            >
              Create Account
            </button>
          </form>

          {/* Login Link */}
          <div className="mt-4 text-center">
            <p className="text-[#6B9080]">
              Already have an account?{" "}
              <button onClick={goToLogin} className="text-[#2C3E3F] hover:text-[#6B9080]">Log In</button>
            </p>
          </div>
        </div>

        {/* Decorative blurred circles */}
        <div className="absolute top-20 left-10 w-20 h-20 bg-[#A4C3B2]/40 rounded-full blur-xl" />
        <div className="absolute bottom-20 right-10 w-32 h-32 bg-[#6B9080]/30 rounded-full blur-xl" />
      </div>
    </div>
  );
}
