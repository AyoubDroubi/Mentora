import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FiMail, FiLock, FiArrowLeft } from "react-icons/fi";
import { GraduationCap } from "lucide-react";

 export default function Login() {
   const navigate = useNavigate();
    const [email, setEmail] = useState("");
   const [password, setPassword] = useState("");
   const [passwordError, setPasswordError] = useState("");

   const handleSubmit = (e) => {
       e.preventDefault();

    const isValid =
       password.length >= 8 &&
        /[A-Z]/.test(password) &&
       /[a-z]/.test(password) &&
        /[0-9]/.test(password) &&
         /[^A-Za-z0-9]/.test(password);

     if (!isValid) {
       setPasswordError(
         "Password must be at least 8 characters, include uppercase, lowercase, number, and a special character."
       );
      return;
    } 

    setPasswordError("");
    console.log("Login:", { email, password });
    navigate('/study-planner');
  };

   const goBack = () => {
    window.location.href = "/";
  };

  const goToSignup = () => {
    window.location.href = "/signup";
  };

       const goToForgotPassword = () => {
        window.location.href = "/forgot-password";
    };
     return (
       <div className="min-h-screen bg-[#F6FFF8] flex items-center justify-center p-4 relative text-[#2C3E3F]">

          {/* Back Button */}
          <button
           onClick={goBack}
           className="absolute top-6 left-6 text-[#6B9080] hover:text-[#A4C3B2] flex items-center gap-2 transition-colors"
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

            <form onSubmit={handleSubmit} className="space-y-6">

            {/* Email */}
              <div className="space-y-2">
                <label htmlFor="email" className="text-sm text-[#6B9080] mb-2 block">Email</label>
                <div className="relative">
                  <FiMail className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-[#6B9080]" />
                  <input
                   id="email"
                     type="email"
                    placeholder="your.email@example.com"
                    value={email}
                   onChange={(e) => setEmail(e.target.value)}
                    required
                    className="pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#6B9080]/40 focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none"
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
                   className="pl-11 w-full px-4 py-3 rounded-xl bg-[#F6FFF8] border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#6B9080]/40 focus:border-[#6B9080] focus:ring-2 focus:ring-[#6B9080] outline-none"
                 />
                </div>
 
  {passwordError && (
                  <p className="text-red-500 text-sm mt-1">{passwordError}</p>
                  )}
             </div>

              {/* Remember */}
            <div className="flex items-center justify-between text-sm">
               <label className="flex items-center gap-2 text-[#6B9080] cursor-pointer">
                <input type="checkbox" className="rounded border-[#A4C3B2]" />
                 Remember me
              </label>
                 <button  onClick={goToForgotPassword}  className="text-[#6B9080] hover:text-[#2C3E3F]">
                Forgot Password?
               </button>
            </div>
 
            {/* Submit */}
             <button
             type="submit"
                 className="w-full bg-[#6B9080] hover:bg-[#577466] text-white py-3 rounded-xl font-medium shadow-lg transition"
             >
                Login
            </button>
             </form>

          {/* Signup Link */}
          <div className="mt-6 text-center">
            <p className="text-[#6B9080]">
                Don't have an account?{" "}
                <button onClick={goToSignup} className="text-[#2C3E3F] hover:text-[#6B9080]">
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
