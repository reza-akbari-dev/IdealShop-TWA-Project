import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function AdminLogin() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      await axios.post(
        "https://localhost:7138/api/admin/login",
        { email, password },
        { withCredentials: true }
      );

      // ✅ Set admin flag and redirect
      localStorage.setItem("isAdmin", "true");
      window.location.href = "/admin-panel"; // force reload to refresh Navbar
    } catch (err) {
      setError("Invalid email or password.");
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: "500px" }}>
      <h2 className="text-center mb-4 text-primary">Admin Login</h2>

      {error && <div className="alert alert-danger">{error}</div>}

      <form onSubmit={handleLogin}>
        <div className="mb-3">
          <label className="form-label">Email:</label>
          <input
            type="email"
            className="form-control"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Password:</label>
          <input
            type="password"
            className="form-control"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        <button type="submit" className="btn btn-primary w-100">
          Login
        </button>
      </form>
    </div>
  );
}

export default AdminLogin;
